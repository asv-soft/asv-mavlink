using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using ConsoleAppFramework;
using R3;
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
            
            while (!cancellationToken.IsCancellationRequested)
            {
                AnsiConsole.Clear();
                var tcs = new TaskCompletionSource();
                cancellationToken.Register(() => tcs.TrySetCanceled());
                var currentFrameText = frameClient.CurrentFrame?.CurrentValue is null 
                    ? "???"
                    : DroneFrameToReadable(frameClient.CurrentFrame.CurrentValue);
                
                AnsiConsole.MarkupLine($"[blue]Current frame:[/] {currentFrameText}");
            
                var selectedFrame = await AnsiConsole.PromptAsync(
                    new SelectionPrompt<IDroneFrame>()
                        .Title("Select a new [blue]frame[/]:")
                        .AddChoices(frameClient.Frames.Values)
                        .UseConverter(DroneFrameToReadable),
                    cancellationToken);
            
                await frameClient.SetFrame(selectedFrame, cancellationToken);
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[blue]info:[/] Updating frame...");
                using var sub = frameClient.CurrentFrame?.WhereNotNull().Subscribe(v =>
                {
                    if (v.Id == selectedFrame.Id)
                    {
                        tcs.TrySetResult();
                    }
                });

                await tcs.Task;
                AnsiConsole.Clear();
                var selectedFrameText = DroneFrameToReadable(frameClient.CurrentFrame?.CurrentValue);
                AnsiConsole.MarkupLine($"[blue]Selected frame:[/] {selectedFrameText}");

                AnsiConsole.MarkupLine("Press Ctrl-C to exit or any key to continue...");
                await AnsiConsole.Console.Input.ReadKeyAsync(true, cancellationToken);
            }
            
            return 0;
        }
        catch (OperationCanceledException)
        {
            AnsiConsole.MarkupLine("\n[yellow]warn:[/] the command was stopped by the user");
            return 0;
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            return 1;
        }
    }

    private string DroneFrameToReadable(IDroneFrame? droneFrame)
    {
        if (droneFrame is null)
        {
            return "[yellow]???[/]";
        }
        
        if (droneFrame.Meta is null)
        {
            return $"[yellow]{droneFrame.Id}[/]";
        }

        var meta = string.Join("[blue];[/] ", droneFrame.Meta.Select(kv => $"{kv.Key}: {kv.Value}"));
        return $"[yellow]{droneFrame.Id}[/]: ([blue]meta[/]: {meta})";
    }
}