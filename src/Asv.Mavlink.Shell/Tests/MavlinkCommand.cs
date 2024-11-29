using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using ConsoleAppFramework;
using R3;
using Spectre.Console;

namespace Asv.Mavlink.Shell
{
    public class MavlinkCommand
    {
        private string _connectionString = "tcp://127.0.0.1:5762";
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        private readonly ReaderWriterLockSlim _rw = new ReaderWriterLockSlim();
        private readonly List<DisplayRow> _items = new List<DisplayRow>();
        private DateTime _lastUpdate = DateTime.Now;
        private readonly List<MavlinkMessage> _lastPackets = new List<MavlinkMessage>();
        private int MaxHistorySize = 20;
        private int _packetCount;
        private int _lastPacketCount;

        /// <summary>
        /// Listen MAVLink packages and print statistic
        /// </summary>
        /// <param name="connection">-cs, Connection string. Default "tcp://127.0.0.1:5762"</param>
        [Command("mavlink")]
        public void RunMavlink(string? connection = null)
        {
            _connectionString = connection ?? _connectionString;
            Task.Factory.StartNew(KeyListen);
            var conn = Protocol.Create(builder =>
            {
                builder.RegisterMavlinkV2Protocol();
            }).CreateRouter("ROUTER");

            conn.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(OnPacket);
            
            while (!_cancel.IsCancellationRequested)
            {
                Redraw();
                Task.Delay(3000, _cancel.Token).Wait();
            }
        }

       

        private void Redraw()
        {
            DisplayRow[] items;
            MavlinkMessage[] packets;
            try
            {
                _rw.EnterReadLock();
                items = _items.ToArray();
                packets = _lastPackets.ToArray();
            }
            finally
            {
                _rw.ExitReadLock();
            }
            var time = DateTime.Now - _lastUpdate;
            foreach (var item in items)
            {
                item.Freq = (item.Count / time.TotalSeconds).ToString("0.0 Hz");
                item.Count = 0;
            }
            _lastUpdate = DateTime.Now;

            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold]Press [red]Q[/] to exit[/]");
            AnsiConsole.MarkupLine(
                $"[bold]MAVLink inspector [blue]{((_packetCount - _lastPacketCount) / time.TotalSeconds):0.0} Hz[/]:[/]");

            var table = new Table();
            table.AddColumns("[blue]Msg[/]", "[blue]Message[/]", "[yellow]Frequency[/]");
            foreach (var item in items.OrderBy(_ => _.Msg))
            {
                table.AddRow(
                    new Markup($"[blue]{item.Message}[/]"),
                    new Markup($"[blue]{item.Msg.ToString()}[/]"),
                    new Markup($"[yellow]{item.Freq}[/]"));
            }

            AnsiConsole.Write(table);

            AnsiConsole.MarkupLine($"[bold]Last [yellow]{packets.Length}[/] packets of [yellow]{_packetCount}[/]:[/]");
            _lastPacketCount = _packetCount;

            foreach (var packetV2 in packets)
            {
                AnsiConsole.WriteLine($"{packetV2}");
                ;
            }
        }

        private void KeyListen()
        {
            while (true)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Q:
                        _cancel.Cancel(false);
                        break;
                }
            }
        }

        private void OnPacket(MavlinkMessage packet)
        {
            Interlocked.Increment(ref _packetCount);
            try
            {
                _rw.EnterWriteLock();
                _lastPackets.Add(packet);
                if (_lastPackets.Count >= MaxHistorySize) _lastPackets.RemoveAt(0);
                var exist = _items.FirstOrDefault(r => packet.Id == r.Msg);
                if (exist == null)
                {
                    _items.Add(new DisplayRow { Msg = packet.Id, Message = packet.Name });
                }
                else
                {
                    exist.Count++;
                }
            }
            finally
            {
                _rw.ExitWriteLock();
            }
        }

        internal class DisplayRow
        {
            public string Message { get; set; }
            public int Count { get; set; }
            public string Freq { get; set; }
            public int Msg { get; set; }
        }
    }
}