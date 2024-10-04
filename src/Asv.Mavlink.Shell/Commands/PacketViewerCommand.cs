using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Engines;
using ConsoleAppFramework;
using DynamicData;
using DynamicData.Binding;
using Newtonsoft.Json;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class PacketViewerCommand
{
    private Table _table;
    private Table _packetTable;
    private Table _headerTable;
    private Thread _actionsThread;
    private IMavlinkRouter _router;
    private bool _isCancel;
    private bool _isSearching;
    private bool _isPause;
    private string _consoleSearch;
    private string _consoleSize = "10";
    private readonly SourceList<PacketModel> _packetsSource = new();
    private ReadOnlyObservableCollection<PacketModel> _packets;
    private readonly ConcurrentQueue<PacketModel> _outputCollection = new();
    private readonly Subject<Func<PacketModel, bool>> _filterNameUpdate = new();

    [Command("packetviewer")]
    public void Run(string connection)
    {
        _headerTable = new Table().Expand().AddColumns("[red]F6[/]", "[red]F7[/]", "[red]F8[/]", "[red]F9[/]", "[red]ENTER[/]");
        _headerTable.AddRow("Search:", $"Size:{_consoleSize}", "Pause", "End", "Submit"); 
        _table = new Table().Expand().AddColumn("Controls")
            .Title("Packet Viewer")
            .Border(TableBorder.Rounded);
        _table.AddRow(_headerTable);
        _table.AddRow("");
        _packetTable = new Table().AddColumns("Time", "Source", "Size", "Name", "Message").Expand();
        AnsiConsole.Status().Start("Create router...", ctx =>
        {
            ctx.Spinner(Spinner.Known.Aesthetic);
            ctx.SpinnerStyle(Style.Parse("green"));
            _router = new MavlinkRouter(MavlinkV2Connection.RegisterDefaultDialects);
            _router.AddPort(new MavlinkPortConfig()
            {
                ConnectionString = connection,
                IsEnabled = true,
                Name = "Client"
            });
            _router.WrapToV2ExtensionEnabled = true;
        });
        _filterNameUpdate.OnNext(FilterBySourceType);
        _packetsSource.LimitSizeTo(1000).Subscribe();
        _packetsSource.Connect().Sort(SortExpressionComparer<PacketModel>.Descending(_ => _.Time)).Bind(out _packets)
            .Subscribe(_ => SetPacketsInQueue(_packets.First()));
        _router.Buffer(TimeSpan.FromSeconds(1)).Select(GetPacketAndAddToCollection)
            .Subscribe(_packetsSource.AddRange);
        _actionsThread = new Thread(InterceptConsoleActions);
        _actionsThread.Start();
        AnsiConsole.Live(_table).AutoClear(true).StartAsync(async ctx =>
        {
            while (_isCancel is false)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(35));
                UpdateTable();
                if (_table is null) continue;
                ctx.Refresh();
            }
        });
    }
    
    private void UpdateTable()
    {
        if (_isPause) return;
        var dequeue = _outputCollection.TryDequeue(out var packetModel);
        if (dequeue)
        {
            _packetTable.InsertRow(0, $"{packetModel.Time}", $"{packetModel.Source}", $"{packetModel.Size}",
                $"{packetModel.Type}", $"{packetModel.Message}");
            _table.UpdateCell(1, 0, _packetTable);
        }

        var parsed = int.TryParse(_consoleSize, out var size);
        if (parsed is false) return;
        if (_packetTable.Rows.Count <= size) return;
        for (var i = size; i < _packetTable.Rows.Count; i++)
        {
            _packetTable.RemoveRow(i);
        }
    }

    private void UpdateSearchCellInActive() => _headerTable.UpdateCell(0, 0, $"Search: {_consoleSearch}");
    private void UpdateSearchCellActive() => _headerTable.UpdateCell(0, 0, $"[aqua]Search:[/] {_consoleSearch}");
    private void UpdateSizeCellInActive() => _headerTable.UpdateCell(0, 1, $"Size: {_consoleSize}");
    private void UpdateSizeCellActive() => _headerTable.UpdateCell(0, 1, $"[aqua]Size:[/] {_consoleSize}");
    private void UpdatePauseCellActive() => _headerTable.UpdateCell(0, 2, $"[aqua]Pause[/]");
    private void UpdatePauseCellInActive() => _headerTable.UpdateCell(0, 2, $"Pause");

  

    private async Task HighlightSubmitCell()
    {
        _headerTable.UpdateCell(0, 4, $"[aqua]Submit[/]");
        await Task.Delay(TimeSpan.FromMilliseconds(500));
        _headerTable.UpdateCell(0, 4, $"Submit");
    }

    private async Task HighlightEndCell()
    {
        _headerTable.UpdateCell(0, 3, $"[aqua]End[/]");
        await Task.Delay(TimeSpan.FromMilliseconds(500));
        _headerTable.UpdateCell(0, 3, $"End");
    }
    
    private void InterceptConsoleActions()
    {
        while (true)
        {
            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.F6:
                {
                    _isSearching = true;
                    UpdateSearchCellActive();
                    UpdateSizeCellInActive();
                    while (_isSearching)
                    {
                        var keysearch = Console.ReadKey(true);
                        switch (keysearch.Key)
                        {
                            case ConsoleKey.Backspace when _consoleSearch == string.Empty || _consoleSearch is null:
                                continue;
                            case ConsoleKey.Backspace:
                                _consoleSearch = _consoleSearch.Remove(_consoleSearch.Length - 1, 1);
                                UpdateSearchCellActive();
                                break;
                            case ConsoleKey.Enter:
                                _isSearching = false;
                                UpdateSearchCellInActive();
                                HighlightSubmitCell();
                                break;
                            default:
                                _consoleSearch += keysearch.KeyChar;
                                UpdateSearchCellActive();
                                break;
                        }
                    }
                    break;
                }
                case ConsoleKey.F7:
                {
                    _isSearching = false;
                    UpdateSizeCellActive();
                    UpdateSearchCellInActive();
                    while (_isSearching is false)
                    {
                        var keysearch = Console.ReadKey(true);
                        switch (keysearch.Key)
                        {
                            case ConsoleKey.Backspace when _consoleSize is "" or null:
                                continue;
                            case ConsoleKey.Backspace:
                                _consoleSize = _consoleSize.Remove(_consoleSize.Length - 1, 1);
                                UpdateSizeCellActive();
                                break;
                            case ConsoleKey.Enter:
                                _isSearching = true;
                                UpdateSizeCellInActive();
                                HighlightSubmitCell();
                                break;
                            default:
                                _consoleSize += keysearch.KeyChar;
                                UpdateSizeCellActive();
                                break;
                        }
                    }
                    break;
                }
                case ConsoleKey.F8:
                {
                    _isPause = true;
                    UpdatePauseCellActive();
                    var keyPause = Console.ReadKey(true);
                    if (keyPause.Key is ConsoleKey.Enter or ConsoleKey.F8)
                    {
                        HighlightSubmitCell();
                        UpdatePauseCellInActive();
                        _isPause = false;
                    }
                    break;
                }
                case ConsoleKey.F9:
                {
                    HighlightEndCell();
                    _isCancel = true;
                    _router.Dispose();
                    _packetsSource.Dispose();
                    _actionsThread.Interrupt();
                    return;
                }
                case ConsoleKey.Enter:
                {
                    HighlightSubmitCell();
                    break;
                }
            }
        }
    }

    private bool FilterBySourceType(PacketModel vm)
    {
        return _consoleSearch is null || vm.Type.Contains(_consoleSearch, StringComparison.InvariantCultureIgnoreCase);
    }

    private void SetPacketsInQueue(PacketModel packet)
    {
        if (packet is null) return;
        _outputCollection.Enqueue(packet);
    }

    private List<PacketModel> GetPacketAndAddToCollection(IList<IPacketV2<IPayload>> pkt)
    {
        if (_consoleSearch is null ||
            pkt.Where(_ => _.Name.Contains(_consoleSearch, StringComparison.InvariantCultureIgnoreCase)) is null)
        {
            return pkt.Select(item => new PacketModel(item)).ToList();
        }

        return pkt.Where(_ => _.Name.Contains(_consoleSearch, StringComparison.InvariantCultureIgnoreCase))
            .Select(item => new PacketModel(item)).ToList();
    }

    private class PacketModel(IPacketV2<IPayload> packetV2)
    {
        public readonly string Type = packetV2.Name;
        public DateTime Time { get; } = DateTime.Now;
        public readonly string Source = $"{packetV2.SystemId}, {packetV2.ComponentId}";
        public readonly string Message = $"{packetV2.Sequence:000}, {ConvertPacket(packetV2)}";
        public string Description = ConvertPacket(packetV2, PacketFormatting.Indented);
        public Guid Id = Guid.NewGuid();
        public readonly int Size = packetV2.GetByteSize();
    }

    private static string ConvertPacket(IPacketV2<IPayload> packet, PacketFormatting formatting = PacketFormatting.None)
    {
        var CanConvert = packet != null;
        if (packet == null) throw new ArgumentException("Incoming packet was not initialized!");
        if (!CanConvert) throw new ArgumentException("Converter can not convert incoming packet!");

        var result = formatting switch
        {
            PacketFormatting.None => JsonConvert.SerializeObject(packet.Payload, Formatting.None),
            PacketFormatting.Indented => JsonConvert.SerializeObject(packet.Payload, Formatting.Indented),
            _ => throw new ArgumentException("Wrong packet formatting!")
        };

        return result;
    }

    private enum PacketFormatting
    {
        /// <summary>
        /// One-line formatting
        /// </summary>
        None,

        /// <summary>
        /// Represents the formatting options for packet content.
        /// </summary>
        Indented
    }
}