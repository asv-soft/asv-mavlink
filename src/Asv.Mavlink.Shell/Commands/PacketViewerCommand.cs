using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.V2.Common;
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
    private SourceList<PacketModel> PacketsSource { get; set; } = new();
    private ReadOnlyObservableCollection<PacketModel> _packets;
    private string consoleSearch;

    [Command("packetviewer")]
    public void Run(string connection)
    {
        consoleSearch = AnsiConsole.Prompt(new TextPrompt<string>("Type: "));
        _table = new Table().Expand()
            .AddColumns("Time", "Source", "Size", "Type", "Message")
            .Title("[aqua]Packet Viewer[/]")
            .Border(TableBorder.Rounded);
        _router = new MavlinkRouter(MavlinkV2Connection.RegisterDefaultDialects);
        _router.AddPort(new MavlinkPortConfig()
        {
            ConnectionString = connection,
            IsEnabled = true,
            Name = "Client"
        });
        _router.WrapToV2ExtensionEnabled = true;
        PacketsSource.LimitSizeTo(1000).Subscribe();
        PacketsSource.Connect().Sort(SortExpressionComparer<PacketModel>.Descending(_ => _.Time)).Bind(out _packets)
            .Subscribe(_ => OutputMessage(_packets.First()));
        _router.Buffer(TimeSpan.FromSeconds(1)).Select(GetPacketAndAddToCollection).Subscribe(PacketsSource.AddRange);
        AnsiConsole.Live(_table)
            .Start(ctx => 
        {
            while (true)
            {
                Task.Delay(1000);
                ctx.Refresh();
            }
          
        });
        _packets.WhenValueChanged(_ => _.Count).Subscribe(_ => { OutputMessage(_packets.Last()); }).Dispose();
    }

    private void OutputMessage(PacketModel packet)
    {
        _table.InsertRow(0, $"{packet.Time}", $"{packet.Source}", $"{packet.Size}", $"{packet.Type}",
            $"{packet.Message}");
        if (_table.Rows.Count == 20)
        {
            _table.RemoveRow(19);
        }
       
    }

    private List<PacketModel> GetPacketAndAddToCollection(IList<IPacketV2<IPayload>> pkt)
    {
        return consoleSearch == String.Empty ?
              pkt.Select(item => new PacketModel(item)).ToList() 
            : pkt.Where(_ => _.Name.Contains(consoleSearch)).Select(item => new PacketModel(item)).ToList();
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