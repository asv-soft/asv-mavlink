using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

using ConsoleAppFramework;
using ObservableCollections;
using R3;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class DevicesInfoCommand
{
    private uint _refreshRate;
    private IProtocolRouter _router;
    private ISynchronizedView<KeyValuePair<MavlinkIdentity,IClientDevice>,MavlinkDeviceModel> _list;


    /// <summary>
    /// Command that shows info about devices in the mavlink network
    /// </summary>
    /// <param name="connectionString">-cs, The address of the connection to the mavlink device</param>
    /// <param name="iterations">-i, States how many iterations should the program work through</param>
    /// <param name="devicesTimeout">-dt, (in seconds) States the lifetime of a mavlink device that shows no Heartbeat</param>
    /// <param name="refreshRate">-r, (in ms) States how fast should the console be refreshed</param>
    /// <returns></returns>
    [Command("devices-info")]
    public int Run(string connectionString, uint? iterations = null, uint devicesTimeout = 10, uint refreshRate = 3000)
    {
        RunAsync(connectionString, iterations, devicesTimeout, refreshRate).Wait();
        ConsoleAppHelper.WaitCancelPressOrProcessExit();
        return 0;
    }
    
    private async Task RunAsync(string connectionString, uint? iterations, uint devicesTimeout,  uint refreshRate)
    {
        _refreshRate = refreshRate;
        var protocol = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
        });
        _router = protocol.CreateRouter("DEFAULT");
        _router.AddPort(connectionString);
        
        var devices = new List<IClientDeviceProvider>
        {
            new GenericClientDeviceFactory(new GenericDeviceConfig()),
            new AdsbClientDeviceProvider(new AdsbClientDeviceConfig(), Array.Empty<ParamDescription>()),
            new GbsClientDeviceProvider(new GbsClientDeviceConfig()),
            new RadioClientDeviceProvider(new RadioClientDeviceConfig()),
            new RfsaClientDeviceProvider(new RfsaClientDeviceConfig()),
            new RsgaClientDeviceProvider(new RsgaClientDeviceConfig()),
            new SdrClientDeviceProvider(new SdrClientDeviceConfig()),
            new ArduCopterClientDeviceFactory(new VehicleClientDeviceConfig()),
            new ArduPlaneClientDeviceProvider(new VehicleClientDeviceConfig()),
            new Px4CopterClientDeviceFactory(new VehicleClientDeviceConfig()),
            new Px4PlaneClientDeviceFactory(new VehicleClientDeviceConfig())
        };
        
        
        var id = new MavlinkIdentity(254, 254);
        var core = new CoreServices(_router, new PacketSequenceCalculator(), protocol.LoggerFactory, protocol.TimeProvider,protocol.MeterFactory);
        var factory = new ClientDeviceFactory(id, devices, core);
        var browser = new ClientDeviceBrowser(factory, new DeviceBrowserConfig(), core);
        
        _list = browser.Devices.CreateView(x => new MavlinkDeviceModel(x.Value, TimeProvider.System));
            
        
        var table = new Table();
        table.Title("[[ [yellow]Devices Info[/] ]]");
        table.AddColumn("Id");
        table.AddColumn("Type");
        table.AddColumn("System Id");
        table.AddColumn("Component Id");
        table.AddColumn("Mavlink Version");
        table.AddColumn("Modes");
        table.AddColumn("System Status");
        table.AddColumn("Heartbeat Rate");
        
        try
        {
            await AnsiConsole.Live(table)
                .AutoClear(false)
                .Overflow(VerticalOverflow.Ellipsis)
                .Cropping(VerticalOverflowCropping.Top)
                .StartAsync(async ctx =>
                {
                    var runForever = iterations == null;
                    Console.CancelKeyPress += (_, _) =>
                    {
                        runForever = false;
                    };
                    
                    while (iterations > 0 || runForever)
                    {
                        iterations--;
                        table.Rows.Clear(); 
                        RenderRows(table, _list);
                        ctx.Refresh(); 
                        await Task.Delay(TimeSpan.FromMilliseconds(_refreshRate));
                    }
                });
        }
        finally
        {
            AnsiConsole.Markup("\nDone");
            _list.Dispose();
            await browser.DisposeAsync();
            await _router.DisposeAsync();
        }
    }

    private void RenderRows(Table table, ISynchronizedView<KeyValuePair<MavlinkIdentity, IClientDevice>, MavlinkDeviceModel> devices)
    {
        foreach (var device in devices.Unfiltered)
        {
            table.AddRow(
                $"[red]{device.View.DeviceFullId}[/]",
                $"{device.View.Type}",
                $"{device.View.SystemId}",
                $"{device.View.ComponentId}",
                $"{device.View.MavlinkVersion}",
                $"{device.View.BaseModeText}",
                $"{device.View.SystemStatusText}",
                $"[green]{device.View.RateText}[/]"
            );
        }
    }

    private class MavlinkDeviceModel
    {
        private uint _rate;
        private long _lastUpdate;
        private uint _lastRate;

        public MavlinkDeviceModel(IClientDevice info,TimeProvider timeProvider )
        {
            DeviceFullId = info.Identity.Target;
            Type = info.Class.ToString();
            SystemId = info.Identity.Target.SystemId;
            ComponentId = info.Identity.Target.ComponentId;
            if (info.Heartbeat.RawHeartbeat.CurrentValue != null)
                MavlinkVersion = info.Heartbeat.RawHeartbeat.CurrentValue.MavlinkVersion;
            RateText = "0.0 Hz";
            
            Observable.Timer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), timeProvider).Subscribe(_ =>
            {
                var time = Interlocked.Exchange(ref _lastUpdate, timeProvider.GetTimestamp());
                var cnt = Interlocked.Exchange(ref _lastRate,0);
                var rate = (cnt / timeProvider.GetElapsedTime(time).TotalSeconds);
                RateText = $"{rate:F1} Hz";
            });

            info.Heartbeat.RawHeartbeat.ThrottleLast(TimeSpan.FromSeconds(2), timeProvider)
                .Subscribe(_ =>
                {
                    Interlocked.Increment(ref _lastRate);
                    ToggleLinkPing = false;
                    ToggleLinkPing = true;
                });

            info.Heartbeat.RawHeartbeat.Subscribe(_ => Interlocked.Increment(ref _rate));
            info.Heartbeat.RawHeartbeat.Subscribe(_ => BaseModeText = $"{_.BaseMode.ToString("F").Replace("MavModeFlag", "")}");
            info.Heartbeat.RawHeartbeat.Subscribe(_ => SystemStatusText = _.SystemStatus.ToString("G").Replace("MavState", ""));
        }

        public MavlinkIdentity DeviceFullId { get; set; }
        public string Type { get; }
        public byte SystemId { get; }
        public byte ComponentId { get; }
        public byte MavlinkVersion { get; }
        public bool ToggleLinkPing { get; private set; }
        public string RateText { get; private set; }
        public string BaseModeText { get; private set; }
        public string SystemStatusText { get; private set; }

       
    }
}