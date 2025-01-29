using System;
using System.Collections.Generic;
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
        try
        {
            var protocol = Protocol.Create(builder =>
            {
                builder.SetDefaultLog();
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
                    DeviceCheckIntervalMs = 1000,
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

    public static async Task<IClientDevice> DeviceAwaiter(IDeviceExplorer deviceExplorer)
    {
        var input = string.Empty;
        var list = new List<IClientDevice>();
        var isChosen = false;
        var count = 1;
        
        while (!isChosen)
        {
            AnsiConsole.Clear();
            if (deviceExplorer.Devices.Count == 0)
            {
                continue;  
            }

            foreach (var device in deviceExplorer.Devices)
            {
                AnsiConsole.WriteLine($@"{count}: {device.Key}");
                count++;
            }
            
            AnsiConsole.MarkupLine("Select a device by ID or press [green]'S'[/] to reload the device list");

            input = AnsiConsole.Ask<string>("Input: ");

            if (input.ToLower() == "s")
            {
                continue; 
            }
            
            if (int.TryParse(input, out var deviceId) && deviceId > 0 && deviceId <= deviceExplorer.Devices.Count)
            {
                isChosen = true; 
                list.AddRange(deviceExplorer.Devices.Values);
            }
            else
            {
                AnsiConsole.WriteLine("Invalid input. Please enter a valid device ID or 'S' to reload.");
            }
        }

        AnsiConsole.Clear();
        return list[int.Parse(input) - 1];
    }
}