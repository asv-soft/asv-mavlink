using System.Linq;
using Asv.IO;
using ConsoleAppFramework;
using System.Threading;
using Spectre.Console;
using System;
using System.Threading.Tasks;

namespace Asv.Mavlink.Shell
{
    public class DownloadMissionItemsCommand
    {
        /// <summary>
        /// Command that allows you to select a device and watch its mission items
        /// </summary>
        /// <param name="connectionString">-cs, The address of the connection to the mavlink device</param>
        /// <param name="iterations">-i, States how many iterations should the program work through</param>
        /// <param name="devicesTimeout">-dt, (in seconds) States the lifetime of a mavlink device that shows no Heartbeat</param>
        /// <param name="refreshRate">-r, (in ms) States how fast should the console be refreshed</param>
        [Command("show-mission")]
        public async Task<int> RunShowMission(
            string connectionString,  
            uint? iterations = null, 
            uint devicesTimeout = 10, 
            uint refreshRate = 9000)
        {
            ShellCommandsHelper.CreateDeviceExplorer(connectionString, out IDeviceExplorer deviceExplorer);
            
            var device = await Task.Run(() => ShellCommandsHelper.DeviceAwaiter(deviceExplorer));

            if (device.Microservices.FirstOrDefault(x => x is IMissionClient) is not IMissionClient)
                return 0;

            if (device.Microservices.FirstOrDefault(x => x is MissionClient) is not MissionClient mission)
                return 0;

            var table = new Table();
            table.AddColumn("Mission TargetComponent");
            table.AddColumn("Mission Name");
            table.AddColumn("Mission TargetSystem");
            table.AddColumn("Mission Payload Seq");
            table.AddColumn("Mission Params");
            table.AddColumn("Mission x,y,z");
            table.AddColumn("Mission Frame");

            var cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, args) =>
            {
                args.Cancel = true; // Prevent immediate exit
                cancellationTokenSource.Cancel();
                AnsiConsole.MarkupLine("[bold yellow]Program interrupted. Exiting gracefully...[/]");
            };

            var iterationCount = 0;
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (iterationCount >= iterations) break;

                AnsiConsole.Clear();  

                table.Rows.Clear(); 

                var count = await Task.Run(() => mission.MissionRequestCount(cancellationTokenSource.Token), cancellationTokenSource.Token);
                for (var i = 0; i < count; i++)
                {
                    var missionItem = await Task.Run(() => mission.MissionRequestItem((ushort)i, cancellationTokenSource.Token), cancellationTokenSource.Token);
                    
                    double x = MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(missionItem.X);
                    double y = MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(missionItem.Y);
                    double z = missionItem.Z;

                    var missionParams = $"{missionItem.Param1}, {missionItem.Param2}, {missionItem.Param3}, {missionItem.Param4}";
                    var missionCoordinates = $"{x:F6}, {y:F6}, {z:F3}";
                    
                    table.AddRow(
                        missionItem.TargetComponent.ToString(), 
                        missionItem.Command.ToString(),
                        missionItem.TargetSystem.ToString(),
                        missionItem.Seq.ToString(),
                        missionParams,
                        missionCoordinates,
                        missionItem.Frame.ToString()
                    );
                }

                AnsiConsole.Write(table);  
                iterationCount++;
                
                await Task.Delay((int)refreshRate, cancellationTokenSource.Token);
            }

            return 0;
        }
    }
}
