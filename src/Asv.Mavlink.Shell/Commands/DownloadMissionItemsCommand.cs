using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using ConsoleAppFramework;
using Spectre.Console;

namespace Asv.Mavlink.Shell;


public class DownloadMissionItemsCommand
{
   /// <summary>
   /// Command that allows you to select a device and watch its mission items
   /// </summary>
   /// <param name="connectionString">-cs, The address of the connection to the mavlink device</param>
   /// <param name="iterations">-i, States how many iterations should the program work through</param>
   /// <param name="refreshRate">-r, (in ms) States how fast should the console be refreshed</param>
   [Command("show-mission")]
   public int Run(
       string connectionString,
       uint? iterations = null,
       uint refreshRate = 3000
   )
   {
       try
       {
           RunAsync(connectionString, iterations, refreshRate).Wait();
           return 0;
       }
       catch (Exception e)
       {
           AnsiConsole.MarkupInterpolated($"[bold red]Operation failed with the error message: {e.Message}[/]");
           return -1;
       }
   }


   private async Task RunAsync(
       string connectionString,
       uint? iterations = null,
       uint refreshRate = 3000
   )
   {
       var runForever = iterations == null;
       Console.CancelKeyPress += (sender, args) =>
       {
           args.Cancel = true;
           runForever = false;
       };
      
       ShellCommandsHelper.CreateDeviceExplorer(connectionString, out var deviceExplorer);


       var device = await ShellCommandsHelper.DeviceAwaiter(deviceExplorer);


       if (device.Microservices.FirstOrDefault(x => x is MissionClient) is not MissionClient mission)
       {
           throw new Exception("Mission client is not available.");
       }


       var table = new Table();
       table.AddColumn("Mission Payload Seq");
       table.AddColumn("Mission TargetComponent");
       table.AddColumn("Mission Name");
       table.AddColumn("Mission TargetSystem");
       table.AddColumn("Mission Params");
       table.AddColumn("Mission x,y,z");
       table.AddColumn("Mission Frame");
       table.BorderColor(Color.Green4);


       await AnsiConsole.Live(table)
           .AutoClear(false)
           .Overflow(VerticalOverflow.Ellipsis)
           .Cropping(VerticalOverflowCropping.Top)
           .StartAsync(async ctx =>
           {
               while (iterations > 0 || runForever)
               {
                   iterations--;
                   AnsiConsole.Clear();
                   table.Rows.Clear();
                   await RenderRows(table, mission, CancellationToken.None);
                   ctx.Refresh();
                   await Task.Delay(TimeSpan.FromMilliseconds(refreshRate));
               }
           });
          
       AnsiConsole.MarkupLine("\n[bold yellow]Program interrupted. Exiting gracefully...[/]");
   }


   private async Task RenderRows(Table table, IMissionClient missionClient, CancellationToken cancel)
   {
       var count = await missionClient.MissionRequestCount(cancel);
       for (var i = 0; i < count; i++)
       {
           var missionItem = await missionClient.MissionRequestItem((ushort)i, cancel);


           double x = MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(missionItem.X);
           double y = MavlinkTypesHelper.LatLonFromInt32E7ToDegDouble(missionItem.Y);
           double z = missionItem.Z;


           var missionParams = $"{missionItem.Param1}, {missionItem.Param2}, {missionItem.Param3}, {missionItem.Param4}";
           var missionCoordinates = $"{x:F6}, {y:F6}, {z:F3}";


           table.AddRow(
               missionItem.Seq.ToString(),
               missionItem.TargetComponent.ToString(),
               missionItem.Command.ToString(),
               missionItem.TargetSystem.ToString(),
               missionParams,
               missionCoordinates,
               missionItem.Frame.ToString()
           );
       }
   }
}
