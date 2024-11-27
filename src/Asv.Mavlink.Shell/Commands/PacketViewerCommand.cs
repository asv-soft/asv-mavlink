using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsoleAppFramework;
using R3;
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
    private PacketPrinter _printer;

    /// <summary>
    /// Show packets in real time
    /// </summary>
    /// <param name="connection">-connection, Connection string. Default "tcp://127.0.0.1:5762"</param>
    [Command("packetviewer")]
    public void Run(string connection = "tcp://127.0.0.1:5762")
    {
        _printer = new PacketPrinter(new List<IPacketPrinterHandler>()
        {
            new DefaultPacketHandler(),
            new FtpPacketFormatter(),
            new StatusTextFormatter(),
            new ParamSetFormatter(),
            new ParamValueFormatter()
        });
        _headerTable = new Table().Expand().AddColumns("[red]F6[/]", "[red]F7[/]", "[red]F8[/]", "[red]F9[/]", "[red]ENTER[/]").Title("[aqua]Controls[/]");
        _headerTable.AddRow("Search:", $"Size:{_consoleSize}", "Pause", "End", "Submit"); 
        _table = new Table().Expand().AddColumn("[aqua]Packet Viewer[/]")//.BorderColor(Color.Aqua)
            .Border(TableBorder.None);
        _table.AddRow(_headerTable);
        _table.AddRow("");
        
        _packetTable = new Table().AddColumns("Time", "Type", "Source","Size", "Sequence", "Message").Expand().Title("[aqua]Packets[/]");
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
        _router.RxPipe.Chunk(TimeSpan.FromSeconds(1))
            .Subscribe(GetPacketAndUpdateTable);
        _actionsThread = new Thread(InterceptConsoleActions);
        _actionsThread.Start();
        AnsiConsole.Live(_table).AutoClear(true).StartAsync(async ctx =>
        {
            while (_isCancel is false)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(35));
                if (_table is null) continue;
                ctx.Refresh();
            }
        });
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
    
    private void GetPacketAndUpdateTable(IList<MavlinkMessage> pkt)
    {
        
        if (_isPause) return;
        var result = new List<MavlinkMessage>();
        var filtered = pkt.Where(_ => _.Name.Contains(_consoleSearch, StringComparison.InvariantCultureIgnoreCase));
        if (_consoleSearch is null)
        {
            
            result.AddRange(pkt);
        }
        else
        {
           result.AddRange(pkt.Where(_=>_.Name.Contains(_consoleSearch)));
        }

        foreach (var packet in result)
        {
            var msg = _printer.Print(packet);
            _packetTable.InsertRow(0, $@"{DateTime.Now}", $"{packet.Name}", $"{packet.ComponentId},{packet.SystemId}", $"{packet.GetByteSize()}", Markup.Escape($"{packet.Sequence}"),  Markup.Escape($"{msg}"));
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
}