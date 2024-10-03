using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using ConsoleAppFramework;
using DynamicData;
using DynamicData.Binding;
using Newtonsoft.Json;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class PacketViewerCommand
{
    private Table _table;
    private IMavlinkRouter _router;
    private Thread _actionsThread;
    private SourceList<PacketModel> PacketsSource { get; set; } = new();
    private ReadOnlyObservableCollection<PacketModel> _packets;
    private ReadOnlyObservableCollection<PacketModel> _filterByName = new(new ObservableCollection<PacketModel>());
    private string consoleSearch;
    private Subject<Func<PacketModel, bool>> _filterNameUpdate = new();
    private bool _isFilterEnabled;

    [Command("packetviewer")]
    public void Run(string connection)
    {
        UpdateTable();
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
        PacketsSource.LimitSizeTo(1000).Subscribe();
        PacketsSource.Connect().Sort(SortExpressionComparer<PacketModel>.Descending(_ => _.Time)).Bind(out _packets)
            .Subscribe(_ => OutputMessage(_packets.First()));
        _router.Buffer(TimeSpan.FromSeconds(1)).Select(GetPacketAndAddToCollection)
            .Subscribe(PacketsSource.AddRange);
        while (true)
        {
            if (_isFilterEnabled is false)
            {
                _actionsThread = new Thread(InterceptConsoleActions);
                _actionsThread.Start();
                var ctx = AnsiConsole.Live(_table).AutoClear(true);
                ctx.Start(WritePacketViewer);
            }
            consoleSearch = AnsiConsole.Prompt(new TextPrompt<string>("Write Package Name:"));
            UpdateTable();
          
            _isFilterEnabled = false;
            //AnsiConsole.Clear(); console freezes after cleaning it idk where is the problem
        }
    }

    private void UpdateTable()
    {
        _table = new Table().Expand()
            .AddColumns("Time", "Source", "Size", "Name", "Message")
            .Title("[aqua]Packet Viewer[/]")
            .Border(TableBorder.Rounded);
    }

    private void InterceptConsoleActions()
    {
        var actionsTable = new Table();
        actionsTable.AddColumn("Filter");
        actionsTable.AddRow(new Panel("[red]F[/]"));
        AnsiConsole.Write(actionsTable);
        var key = Console.ReadKey();
        if (key.Key == ConsoleKey.F)
        {
            _isFilterEnabled = true;
        }
    }

    private void WritePacketViewer(LiveDisplayContext context)
    {
        while (_isFilterEnabled is false)
        {
            Task.Delay(TimeSpan.FromSeconds(1));
            if (_table is null) return;
            context.Refresh();
        }
    }

    private bool FilterBySourceType(PacketModel vm)
    {
        if (consoleSearch is null) return true;
        return vm.Type.Contains(consoleSearch, StringComparison.InvariantCultureIgnoreCase);
    }

    private void OutputMessage(PacketModel packet)
    {
        if (packet is null) return;
        _table.InsertRow(0, $"{packet.Time}", $"{packet.Source}", $"{packet.Size}", $"{packet.Type}",
            $"{packet.Message}");
        if (_table.Rows.Count == 20)
        {
            _table.RemoveRow(19);
        }
    }

    private List<PacketModel> GetPacketAndAddToCollection(IList<IPacketV2<IPayload>> pkt)
    {
        if (consoleSearch is null ||
            pkt.Where(_ => _.Name.Contains(consoleSearch, StringComparison.InvariantCultureIgnoreCase)) is null)
        {
            return pkt.Select(item => new PacketModel(item)).ToList();
        }

        return pkt.Where(_ => _.Name.Contains(consoleSearch, StringComparison.InvariantCultureIgnoreCase))
            .Select(item => new PacketModel(item)).ToList();
    }

    private class PacketModel(IPacketV2<IPayload> packetV2)
    {
        public string Type = packetV2.Name;
        public DateTime Time { get; } = DateTime.Now;
        public string Source = $"{packetV2.SystemId}, {packetV2.ComponentId}";
        public string Message = $"{packetV2.Sequence:000}, {ConvertPacket(packetV2)}";
        public string Description = ConvertPacket(packetV2, PacketFormatting.Indented);
        public Guid Id = Guid.NewGuid();
        public int Size = packetV2.GetByteSize();
    }

    private static string ConvertPacket(IPacketV2<IPayload> packet, PacketFormatting formatting = PacketFormatting.None)
    {
        var CanConvert = packet != null;
        if (packet == null) throw new ArgumentException("Incoming packet was not initialized!");
        if (!CanConvert) throw new ArgumentException("Converter can not convert incoming packet!");

        string result = string.Empty;

        if (formatting == PacketFormatting.None)
        {
            result = JsonConvert.SerializeObject(packet.Payload, Formatting.None);
        }
        else if (formatting == PacketFormatting.Indented)
        {
            result = JsonConvert.SerializeObject(packet.Payload, Formatting.Indented);
        }
        else
        {
            throw new ArgumentException("Wrong packet formatting!");
        }

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