using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Spectre.Console;
using ZLogger;

namespace Asv.Mavlink.Shell;

public static class ShellCommandsHelper
{
    public static void CreateDeviceExplorer(string connection, out IDeviceExplorer deviceExplorer, ILogger? logger = null)
    {
        logger ??= NullLogger.Instance;
        var factory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.AddDebug();
        });
        try
        {
            var protocol = Protocol.Create(builder =>
            {
                builder.SetLog(factory);
                builder.RegisterMavlinkV2Protocol();
                builder.Features.RegisterBroadcastFeature<MavlinkMessage>();
                builder.Formatters.RegisterSimpleFormatter();
            });
            var router = protocol.CreateRouter("ROUTER");
            router.AddPort(connection);
            var seq = new PacketSequenceCalculator();
            var identity = new MavlinkIdentity(255, 255);
            deviceExplorer = DeviceExplorer.Create(router, builder =>
            {
                builder.SetLog(protocol.LoggerFactory);
                builder.SetMetrics(protocol.MeterFactory);
                builder.SetTimeProvider(protocol.TimeProvider);
                builder.SetConfig(new ClientDeviceBrowserConfig()
                {
                    DeviceTimeoutMs = 1000,
                    DeviceCheckIntervalMs = 30_000,
                });
                builder.Factories.RegisterDefaultDevices(
                    new MavlinkIdentity(identity.SystemId, identity.ComponentId),
                    seq,
                    new InMemoryConfiguration());
            });
        }
        catch (Exception e)
        {
            logger.ZLogError($"{e.Message}");
            AnsiConsole.Clear();
            AnsiConsole.WriteLine($"Unable to create router: {e.Message}");
            AnsiConsole.Ask<string>("Press any key to exit");
            throw;
        }
    }
    public static async Task<IClientDevice?> DeviceAwaiter(IDeviceExplorer deviceExplorer, uint refreshRate)
    {
        var input = string.Empty;
        var list = new List<IClientDevice>();
        var isChosen = false;
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;
    
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true;  
            cancellationTokenSource.Cancel(); 
        };

        while (!isChosen)
        {
            try
            {
                token.ThrowIfCancellationRequested();

                AnsiConsole.Clear();
                var count = 1;
                var devices = deviceExplorer.InitializedDevices.ToImmutableList();
                if (devices.Count == 0)
                {
                    AnsiConsole.MarkupLine("Waiting for connections...");
                    await DelayWithCancellation(refreshRate, token);
                    continue;
                }

                foreach (var device in devices)
                {
                    AnsiConsole.WriteLine($@"{count}: {device.Id}");
                    count++;
                }

                AnsiConsole.MarkupLine("Select a device by ID or write [green]'S'[/] to continue search");

                input = AnsiConsole.Ask<string>("Input(q to quit): ");

                if (input.ToLower() == "q")
                {
                    await cancellationTokenSource.CancelAsync();
                }

                if (input.ToLower() == "s")
                {
                    continue;
                }

                if (
                    int.TryParse(input, out var deviceId) 
                    && deviceId > 0 
                    && deviceId <= devices.Count
                )
                {
                    isChosen = true;
                    list.AddRange(devices);
                }
                else
                {
                    AnsiConsole.WriteLine("Invalid input. Please enter a valid device ID or 'S' to search again.");
                }
            }
            catch (OperationCanceledException)
            {
                AnsiConsole.Clear();
                return null; 
            }
        }

        AnsiConsole.Clear();
        return list[int.Parse(input) - 1];
    }
  
    private static async Task DelayWithCancellation(uint milliseconds, CancellationToken token)
    {
        var delayTask = Task.Delay((int)milliseconds, token);
        await delayTask;
    }
}