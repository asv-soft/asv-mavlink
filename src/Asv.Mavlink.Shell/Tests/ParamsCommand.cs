using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.IO;
using ConsoleAppFramework;
using Microsoft.Extensions.Logging.Abstractions;
using ObservableCollections;
using R3;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class ParamsCommand
{
    private readonly CancellationTokenSource _cancel = new();
    private readonly Subject<ConsoleKeyInfo> _userInput = new();

    private int _pageSize = 30;
    private string _search = string.Empty;
    private int _skip;
    private IClientDevice? _device;
    private IProtocolRouter _router;
    private IReadOnlyObservableDictionary<string, ParamItem> _list;
    private IParamsClientEx _paramsClientEx;

    /// <summary>
    /// Command for reading and displaying device parameters
    /// </summary>
    /// <param name="connectionString">-cs, Connection address to the mavlink device etc: tcp://127.0.0.1:5762</param>
    /// <param name="pageSize">-s, Maximum number of rows in the table</param>
    [Command("params")]
    public async Task RunParams(string connectionString = "tcp://127.0.0.1:5762", int pageSize = 30)
    {
        _pageSize = pageSize;
        SetupKeyHandlers();
        _router = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
        }).CreateRouter("ROUTER");
        _router.AddPort(connectionString);
        var identity = new MavlinkClientIdentity(255, 255, 1, 1);
        var seq = new PacketSequenceCalculator();
        var core = new CoreServices(_router, seq, NullLoggerFactory.Instance, TimeProvider.System,
            new DefaultMeterFactory());
        var id = new MavlinkClientDeviceId("PARAMS", identity);
        var config = new MavlinkClientDeviceConfig
        {
            Heartbeat = new HeartbeatClientConfig
            {
                HeartbeatTimeoutMs = 2000,
                LinkQualityWarningSkipCount = 3,
                RateMovingAverageFilter = 10
            }
        };
        var devices = DeviceExplorer.Create(_router, x =>
        {
            x.Factories.RegisterDefaultDevices(new MavlinkIdentity(255, 255), seq, new InMemoryConfiguration());
        });

        devices.Devices.CreateView(CreateDevice(connectionString, identity, core, id, config));
        //_device = CreateDevice(connectionString, identity, core, id, config);
        

        await _device.WaitUntilConnect(10000, TimeProvider.System);
        //TODO: Wait and init
        // var parammClient = _device.Microservices.FirstOrDefault(x => x.Id == "Param");
        // _device.Microservices.Params.Items.Filter(_=> _search.IsNullOrWhiteSpace() || _.Name.Contains(_search, StringComparison.InvariantCultureIgnoreCase))
        //     .Bind(out _list).Subscribe();
        // parammClient.
        // await params.ReadAll(new Progress<double>(_ => Console.WriteLine("Read params progress:" + TextRender.Progress(_, 20))));
        //TODO: Init params
        
        while (!_cancel.IsCancellationRequested)
        {
            Redraw();
            await Task.Delay(1000, _cancel.Token);
        }
    }


    private void SetupKeyHandlers()
    {
        _userInput.Where(_ => _.Key == ConsoleKey.Backspace && !string.IsNullOrEmpty(_search)).Subscribe(_ =>
        {
            _skip = 0;
            _search = _search.Substring(0, _search.Length - 1);
            Redraw();
        });
        _userInput.Where(_ => char.IsLetterOrDigit(_.KeyChar) || _.KeyChar == '_').Subscribe(_ =>
        {
            _skip = 0;
            _search += _.KeyChar;
            Redraw();
        });
        _userInput.Where(_ => _.Key == ConsoleKey.RightArrow).Subscribe(_ =>
        {
            _skip += _pageSize;
            Redraw();
        });
        _userInput.Where(_ => _.Key == ConsoleKey.LeftArrow).Subscribe(_ =>
        {
            _skip -= _pageSize;
            if (_skip < 0) _skip = 0;
            Redraw();
        });
    }

    private void KeyListen()
    {
        while (!_cancel.IsCancellationRequested)
        {
            var key = Console.ReadKey(true);
            _userInput.OnNext(key);
        }
    }

    private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
        if (e.Cancel) _cancel.Cancel(false);
    }

    private void Redraw()
    {
        AnsiConsole.Clear();

        if (_device != null)
        {
            var linkState = _device.Link.State.CurrentValue;
            var stateColor = linkState == LinkState.Connected ? "green" : "red";
            AnsiConsole.MarkupLine($"Connection state: [{stateColor}]{linkState}[/]");
        }

        AnsiConsole.MarkupLine(
            "[yellow]Use Left/Right arrows for navigation (<-|->), ESC to exit, and type for search[/]");
        AnsiConsole.Markup($"Search: [green]{_search}[/]");
        
        var items = _list.ToArray();
        
        var table = new Table()
            .Border(TableBorder.Square)
            .AddColumn("Name")
            .AddColumn("Value")
            .AddColumn("Type");
        
        foreach (var item in items.Skip(_skip).Take(_pageSize))
        {
            // table.AddRow(
            //     item.Name,
            //     item.Value.ToString() ?? string.Empty,
            //     item.Type.ToString());
        }
        
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine(
            $"Showing [{_skip} - {Math.Min(_skip + _pageSize, items.Length)}] of {items.Length} items");
        AnsiConsole.Write(table);
    }

    private MavlinkClientDevice CreateDevice(string cs, MavlinkClientIdentity identity, IMavlinkContext core, MavlinkClientDeviceId id,
        MavlinkClientDeviceConfig config)
    {
        Task.Factory.StartNew(_ => KeyListen(), _cancel.Token);
        Console.CancelKeyPress += Console_CancelKeyPress;
        return new MavlinkClientDevice(id, config,
            ImmutableArray<IClientDeviceExtender>.Empty, core);
    }
}