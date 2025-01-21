using System.Linq;
using Asv.IO;
using ConsoleAppFramework;
using System.Threading;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class DownloadMissionElements
{
    /// <summary>
    /// Selecting a device and uploading its missions
    /// </summary>
    /// <param name="connectionString">-cs, The address of the connection to the mavlink device</param>
    /// <param name="iterations">-i, States how many iterations should the program work through</param>
    /// <param name="devicesTimeout">-dt, (in seconds) States the lifetime of a mavlink device that shows no Heartbeat</param>
    /// <param name="refreshRate">-r, (in ms) States how fast should the console be refreshed</param>
    [Command("show-mission")]
    public int RunShowMission(string connectionString = "tcp://127.0.0.1:5762", uint? iterations = null, uint devicesTimeout = 10, uint refreshRate = 3000)
    {
        ShellCommandsHelper.CreateDeviceExplorer(connectionString, out IDeviceExplorer deviceExplorer);
        var Device = ShellCommandsHelper.DeviceAwaiter(deviceExplorer);
        
        if (Device.Microservices.FirstOrDefault(_ => _ is IMissionClient) is not IMissionClient missionClient)
            return 0;
        var mission = Device.Microservices.FirstOrDefault(_ => _ is MissionClient) as MissionClient; 
        var count = mission.MissionRequestCount(new CancellationToken());
        var table = new Table();
        table.AddColumn("Mission ID");
        table.AddColumn("Mission Name");
        table.AddColumn("Mission IsCompleted");
        table.AddColumn("Mission Status");
        table.AddColumn("Mission Params");
        
        for (int i = 0; i < count.Result; i++)
        {
            var missionItem = mission.MissionRequestItem((ushort)i, new CancellationToken());
            var missionParams = $"{missionItem.Result.Param1}, {missionItem.Result.Param2}, {missionItem.Result.Param3}, {missionItem.Result.Param4}";

            table.AddRow(
                missionItem.Id.ToString(),       
                missionItem.GetType().Name,
                // or missionItem.Result.MissionType.ToString(),
                missionItem.IsCompleted ? "Completed" : "Not Completed",
                missionItem.Status.ToString(),
                missionParams
            );
        }
        
        AnsiConsole.Render(table);
    
        return 0;
    }
    
}