using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using ConsoleAppFramework;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class GetSetHomeCommand
{
    /// <summary>
    ///  Set home position 
    /// </summary>
    /// <param name="connection">-cs, The address of the connection to the mavlink device, e.g. tcp://127.0.0.1:5760</param>
    /// <param name="cancellationToken"></param>
    [Command("get-set-home-test")]
    public async Task<int> SetHomePosition(string connection, CancellationToken cancellationToken = default)
    {
        ShellCommandsHelper.CreateDeviceExplorer(connection, out var deviceExplorer);

        var device = await ShellCommandsHelper.DeviceAwaiter(deviceExplorer, (uint)1000);
        if (device is null)
        {
            AnsiConsole.MarkupLine("[red]error:[/] cannot connect to the device.");
            return 1;
        }

        var position = device.GetMicroservice<IPositionClientEx>();
        if (position is null)
        {
            AnsiConsole.MarkupLine($"[red]error:[/] can't find IControlClient service for device {device}.");
            return 1;
        }
        
        var commands = device.GetMicroservice<ICommandClient>();
        if (commands is null)
        {
            AnsiConsole.MarkupLine($"[red]error:[/] can't find ICommandClient service for device {device}.");
            return 1;
        }
        
        while (position.Home.CurrentValue == null || position.Home.CurrentValue.Value == GeoPoint.Zero)
        {
            await position.GetHomePosition(cancellationToken);
        }
        var home = position.Home.CurrentValue.Value;
        AnsiConsole.MarkupLine($"[green]HOME:[/]{home}");
        
        var newHome = home.RadialPoint(1000, 90); // move home position to west 1 km
        
        await position.SetHomePosition(newHome, cancellationToken);
        
        while (position.Home.CurrentValue.Value == home)
        {
            await position.GetHomePosition(cancellationToken);
        }
        AnsiConsole.MarkupLine($"[green]NEW HOME:[/]{position.Home.CurrentValue}");
        
        return 0;
    }
    
    /// <summary>
    ///  Takeoff command test
    /// </summary>
    /// <param name="connection">-cs, The address of the connection to the mavlink device, e.g. tcp://127.0.0.1:5760</param>
    /// <param name="cancellationToken"></param>
    [Command("takeoff-test")]
    public async Task<int> TakeOff(string connection = "tcp://127.0.0.1:5762", CancellationToken cancellationToken = default)
    {
        ShellCommandsHelper.CreateDeviceExplorer(connection, out var deviceExplorer);

        var device = await ShellCommandsHelper.DeviceAwaiter(deviceExplorer, (uint)1000);
        if (device is null)
        {
            AnsiConsole.MarkupLine("[red]error:[/] cannot connect to the device.");
            return 1;
        }

        var control = device.GetMicroservice<IControlClient>();
        if (control is null)
        {
            AnsiConsole.MarkupLine($"[red]error:[/] can't find IControlClient service for device {device}.");
            return 1;
        }
		
        await control.TakeOff(500, cancellationToken);

        return 0;
    }

}