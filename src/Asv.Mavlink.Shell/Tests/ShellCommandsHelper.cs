using System;
using System.Collections.Generic;
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

    public static IClientDevice DeviceAwaiter(IDeviceExplorer deviceExplorer)
    {
        var input = 0;
        var list = new List<IClientDevice>();
        var isChosen = false;
        while (isChosen == false)
        {
            AnsiConsole.Clear();
            var count = 1;
            foreach (var device in deviceExplorer.Devices)
            {
                AnsiConsole.WriteLine($@"{count}: {device.Key}");
                count++;
            }

            input = AnsiConsole.Ask<int>("Select device or press 0 to wait");
            if (input == 0) continue;
            isChosen = true;
            list.AddRange(deviceExplorer.Devices.Values);
        }
        AnsiConsole.Clear();
        return list[input - 1];
    }
    
}