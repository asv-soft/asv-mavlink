using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using ConsoleAppFramework;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class SetupFrameCommand
{
    /// <summary>
    ///     Select device frame type
    /// </summary>
    /// <param name="connection">-cs, The address of the connection to the mavlink device, e.g. tcp://127.0.0.1:5760</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [Command("setup-frame")]
    public async Task<int> Run(string connection, CancellationToken cancellationToken)
    {
        try
        {
            ShellCommandsHelper.CreateDeviceExplorer(connection, out var deviceExplorer);
            
            var device = await ShellCommandsHelper.DeviceAwaiter(deviceExplorer, 3000);
            if (device is null)
            {
                AnsiConsole.MarkupLine("[red]error:[/] cannot connect to the device.");
                return 1;    
            }

            var frameClient = device.GetMicroservice<IFrameClient>();
            if (frameClient is null)
            {
                AnsiConsole.MarkupLine("[red]error:[/] this device is unsupported yet.");
                return 1;    
            }
            
            await frameClient.LoadAvailableFrames(cancellationToken);

            var currentFrame = await frameClient.GetCurrentFrame(cancellationToken);

            AnsiConsole.MarkupLine($"[blue]Current frame:[/] [yellow]{currentFrame?.ToString() ?? "???"}[/]");
            
            var selectedFrame = await AnsiConsole.PromptAsync(
                new SelectionPrompt<IMotorFrame>()
                    .Title("Select [blue]frame[/]:")
                    .AddChoices(frameClient.MotorFrames.Values)
                    .UseConverter(f => f.Id),
                cancellationToken);
            
            await frameClient.SetFrame(selectedFrame, cancellationToken);
            
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[blue]Selected frame:[/] [yellow]{selectedFrame}[/]");
            
            return 0;
        }
        catch (OperationCanceledException)
        {
            AnsiConsole.MarkupLine("\n[blue]info:[/] the command was stopped by the user");
            return 0;
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            return 1;
        }
    }
}