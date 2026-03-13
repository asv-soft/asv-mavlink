using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using ConsoleAppFramework;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class TakeOffCommand
{
	/// <summary>
	///     Test motors at any autopilots
	/// </summary>
	/// <param name="connection">-cs, The address of the connection to the mavlink device, e.g. tcp://127.0.0.1:5760</param>
	/// <param name="cancellationToken"></param>
	[Command("takeoff-test")]
	public async Task<int> TakeOff(string connection, CancellationToken cancellationToken = default)
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