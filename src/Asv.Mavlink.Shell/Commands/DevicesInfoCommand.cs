using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.IO;
using ConsoleAppFramework;
using ObservableCollections;
using R3;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class DevicesInfoCommand
{
    private uint _refreshRate;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private IProtocolRouter _router;
    private IDeviceExplorer _explorer;
    private ISynchronizedView<KeyValuePair<DeviceId,IClientDevice>,MavlinkDeviceModel> _list;
    
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


    /// <summary>
    /// Command that shows info about devices in the mavlink network
    /// </summary>
    /// <param name="connectionString">-cs, The address of the connection to the mavlink device</param>
    /// <param name="iterations">-i, States how many iterations should the program work through</param>
    /// <param name="refreshRate">-r, (in ms) States how fast should the console be refreshed</param>
    /// <returns></returns>
    [Command("devices-info")]
    public int Run(string connectionString, uint? iterations = null, uint refreshRate = 3000)
    {
        RunAsync(connectionString, iterations, refreshRate).Wait();
        ConsoleAppHelper.WaitCancelPressOrProcessExit();
        return 0;
    }
    
private async Task RunAsync(string connectionString, uint? iterations,  uint refreshRate)
{
    _refreshRate = refreshRate;
    var protocol = Protocol.Create(builder =>
    {
        builder.RegisterMavlinkV2Protocol();
    });
    _router = protocol.CreateRouter("ROUTER");
    _router.AddPort(connectionString);
    var seq = new PacketSequenceCalculator();
    _explorer = DeviceExplorer.Create(_router, builder =>
    {
        builder.SetLog(protocol.LoggerFactory);
        builder.SetMetrics(protocol.MeterFactory);
        builder.SetTimeProvider(protocol.TimeProvider);
        builder.Factories.RegisterDefaultDevices(new MavlinkIdentity(254, 254), seq, new InMemoryConfiguration());
    });

    _list = _explorer.Devices.CreateView(x => new MavlinkDeviceModel(x.Value, TimeProvider.System));

    var removeSub = _list
        .ObserveRemove()
        .Subscribe(e => e.Value.View.Dispose());

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
                Console.CancelKeyPress += (_, _) => runForever = false;

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
        
        removeSub.Dispose();
        _list.Dispose();

        await _explorer.DisposeAsync();
        await _router.DisposeAsync();    
    }
}

    private void RenderRows(Table table, ISynchronizedView<KeyValuePair<DeviceId, IClientDevice>, MavlinkDeviceModel> devices)
    {
        foreach (var device in devices.Unfiltered)
        {
            if (device.View.State != ClientDeviceState.Complete)
            {
                continue;
            }

            table.AddRow(
                $"[red]{Markup.Escape(device.View.DeviceFullId?.ToString() ?? "")}[/]",
                $"{Markup.Escape(device.View.Type ?? "")}",
                $"{Markup.Escape(device.View.SystemId.ToString())}",
                $"{Markup.Escape(device.View.ComponentId.ToString())}",
                $"{Markup.Escape(device.View.MavlinkVersion.ToString())}",
                $"{Markup.Escape(device.View.BaseModeText ?? "")}",
                $"{Markup.Escape(device.View.SystemStatusText ?? "")}",
                $"[green]{Markup.Escape(device.View.RateText ?? "")}[/]"
            );
        }
    }


    private class MavlinkDeviceModel: IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly IClientDevice _info;
        private uint _rate;
        private long _lastUpdate;
        private uint _lastRate;
        private bool _isInitialized;
        private bool _disposed; 
        
        public ClientDeviceState State => _info.State.CurrentValue;


#pragma warning disable CS8618, CS9264
        public MavlinkDeviceModel(IClientDevice info,TimeProvider timeProvider ) 
#pragma warning restore CS8618, CS9264
       {
           _info = info;
           
           _disposables.Add(_info.State
               .Where(state => state == ClientDeviceState.Complete)
               .Take(1)
               .Subscribe(_ =>
               {
                   if (_disposed) return;
                   if (_isInitialized) return;
                   
                   _isInitialized = true;

                   var heartbeat = _info.GetMicroservice<IHeartbeatClient>() 
                                   ?? throw new Exception("Heartbeat client not initialized");

                   DeviceFullId = heartbeat.Identity.Target;
                   Type = _info.Id.DeviceClass;
                   SystemId = heartbeat.Identity.Target.SystemId;
                   ComponentId = heartbeat.Identity.Target.ComponentId;

                   if (heartbeat.RawHeartbeat.CurrentValue != null)
                   {
                       MavlinkVersion = heartbeat.RawHeartbeat.CurrentValue.MavlinkVersion;
                   }

                   RateText = "0.0 Hz";

                   _disposables.Add(Observable
                       .Timer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), timeProvider)
                       .Subscribe(_ =>
                       {
                           if (_disposed) return;
                           var time = Interlocked.Exchange(ref _lastUpdate, timeProvider.GetTimestamp());
                           var cnt = Interlocked.Exchange(ref _lastRate, 0);
                           var rate = cnt / timeProvider.GetElapsedTime(time).TotalSeconds;
                           RateText = $"{rate:F1} Hz";
                       }));

                   _disposables.Add(heartbeat.RawHeartbeat
                       .ThrottleLast(TimeSpan.FromSeconds(2), timeProvider)
                       .Subscribe(_ =>
                       {
                           if (_disposed) return;
                           Interlocked.Increment(ref _lastRate);
                           ToggleLinkPing = false;
                           ToggleLinkPing = true;
                       }));

                   _disposables.Add(heartbeat.RawHeartbeat
                       .Subscribe(_ =>
                       {
                           if (_disposed) return;
                           Interlocked.Increment(ref _rate);
                       }));

                   _disposables.Add(heartbeat.RawHeartbeat
                       .WhereNotNull()
                       .Subscribe(p =>
                       {
                           if (_disposed) return;
                           BaseModeText = $"{p.BaseMode.ToString("F").Replace("MavModeFlag", "")}";
                       }));

                   _disposables.Add(heartbeat.RawHeartbeat
                       .WhereNotNull()
                       .Subscribe(p =>
                       {
                           if (_disposed) return;
                           SystemStatusText = p.SystemStatus.ToString("G").Replace("MavState", "");
                       }));
               }));
        }
        
        public void Dispose()
        {
            if (_disposed) return;

            _disposables.Dispose();
            _disposed = true;
        }
        
        public MavlinkIdentity DeviceFullId { get; private set; }
        public string Type { get; private set;}
        public byte SystemId { get; private set;}
        public byte ComponentId { get; private set;}
        public byte MavlinkVersion { get; private set;}
        public bool ToggleLinkPing { get; private set; }
        public string RateText { get; private set; }
        public string BaseModeText { get; private set; }
        public string SystemStatusText { get; private set; }
    }
}