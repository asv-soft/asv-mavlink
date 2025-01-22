using System.Linq;
using Asv.IO;
using ConsoleAppFramework;
using System.Threading;
using Spectre.Console;
using System;

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
        public int RunShowMission(
            string connectionString,  
            uint? iterations = null, 
            uint devicesTimeout = 10, 
            uint refreshRate = 9000)
        {
            ShellCommandsHelper.CreateDeviceExplorer(connectionString, out IDeviceExplorer deviceExplorer);
            
            var device = ShellCommandsHelper.DeviceAwaiter(deviceExplorer);
            if (device == null)
            {
                AnsiConsole.MarkupLine("[red]Device not found or timeout reached![/]");
                return 1;
            }
            
            if (device.Microservices.FirstOrDefault(x => x is IMissionClient) is not IMissionClient)
                return 0;

            if (device.Microservices.FirstOrDefault(x => x is MissionClient) is not MissionClient mission)
                return 0;

            var table = new Table();
            table.AddColumn("Mission ID");
            table.AddColumn("Mission Name");
            table.AddColumn("Mission IsCompleted");
            table.AddColumn("Mission Status");
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

            
            int iterationCount = 0;
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (iterations.HasValue && iterationCount >= iterations.Value) break;

                AnsiConsole.Clear();  

                table.Rows.Clear(); 

                var count = mission.MissionRequestCount(CancellationToken.None);
                for (var i = 0; i < count.Result; i++)
                {
                    var missionItem = mission.MissionRequestItem((ushort)i, CancellationToken.None);
                    
                    double x = Convert.ToDouble(missionItem.Result.X) / 1000000.0;
                    double y = Convert.ToDouble(missionItem.Result.Y) / 1000000.0;
                    double z = Convert.ToDouble(missionItem.Result.Z) / 1000.0;

                   
                    var missionParams = $"{missionItem.Result.Param1}, {missionItem.Result.Param2}, {missionItem.Result.Param3}, {missionItem.Result.Param4}";
                    var missionCoordinates = $"{x:F6}, {y:F6}, {z:F3}";
                    
                    table.AddRow(
                        missionItem.Id.ToString(), 
                        missionItem.Result.Command.ToString(),
                        missionItem.IsCompleted ? "Completed" : "Not Completed",
                        missionItem.Status.ToString(),
                        missionParams,
                        missionCoordinates,
                        missionItem.Result.Frame.ToString()
                    );
                }

                AnsiConsole.Write(table);  
                iterationCount++;
                
                Thread.Sleep((int)refreshRate);
            }

            return 0;
        }
    }
}
