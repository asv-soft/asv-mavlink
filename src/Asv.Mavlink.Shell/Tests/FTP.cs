using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using ManyConsole;

namespace Asv.Mavlink.Shell;

public class FTP : ConsoleCommand
{
    private string _connectionString = "tcp://127.0.0.1:5762";
    private readonly CancellationTokenSource _cancel = new CancellationTokenSource();
    private readonly ReaderWriterLockSlim _rw = new ReaderWriterLockSlim();
    private readonly List<DisplayRow> _items = new List<DisplayRow>();
    private DateTime _lastUpdate = DateTime.Now;
    private readonly List<IPacketV2<IPayload>> _lastPackets = new List<IPacketV2<IPayload>>();
    private int MaxHistorySize = 20;
    private int _packetCount;
    private int _lastPacketCount;

    public FTP()
    {
        IsCommand("ftp", "Ftp on SITL tests");
        HasOption("cs=", $"Connection string. Default '{_connectionString}'", _ => _connectionString = _);
    }

    public override int Run(string[] remainingArguments)
    {
        Task.Factory.StartNew(KeyListen);
        var conn = MavlinkV2Connection.Create(_connectionString);
        
        conn.Subscribe(OnPacket);
        conn.DeserializePackageErrors.Subscribe(OnError);

        FtpClientEx client = new FtpClientEx(new FtpClient(conn, 
            new MavlinkClientIdentity { SystemId = 123, ComponentId = 123, TargetComponentId = 1, TargetSystemId = 1 }, 
            new FtpConfig(), 
            new PacketSequenceCalculator(), 
            TaskPoolScheduler.Default));

        //var result = client.ListDirectory(".", _cancel.Token);
        
        // Task.WaitAll(client.RemoveFile("Noname1.psd", _cancel.Token));
        //
        // Task.WaitAll(client.RemoveFile("Noname2.psd", _cancel.Token));
        //
        // Task.WaitAll(client.RemoveFile("Noname3.psd", _cancel.Token));

        //Task.WaitAll(result);
        
        //var list = result.Result;

        // Stopwatch sw = new Stopwatch();
        //
        // sw.Start();
        //
        // Task.WaitAll(client.BurstReadFile("cygwin1.dll", "C:\\Users\\VitalyAnofriev\\Documents\\cygwin1.dll", _cancel.Token));
        //
        // sw.Stop();
        //
        Task.Delay(500);
        
        // Task.WaitAll(client.CreateFile("test2.dat",  _cancel.Token));
        // Task.WaitAll(client.ResetSessions(_cancel.Token));
        // Task.WaitAll(client.CreateDirectory("abba2\\",  _cancel.Token));
        // Task.WaitAll(client.ResetSessions(_cancel.Token));
        // var openFile = client.OpenFileWO("test.dat", _cancel.Token);
        // Task.WaitAll(openFile);
        // if (openFile.Result.OpCodeId == OpCode.ACK)
        // {
        //     var res = client.WriteFile(Encoding.UTF8.GetBytes("TEST_STRING"), 0,
        //         openFile.Result.Session, _cancel.Token);
        //     Task.WaitAll(res);
        //     if (res.Result.OpCodeId == OpCode.ACK)
        //     {
        //         Task.WaitAll(client.TerminateSession(openFile.Result.Session, _cancel.Token));
        //         
        //         Task.Delay(100, _cancel.Token).Wait();
        //     }
        // }

        while (!_cancel.IsCancellationRequested)
        {
            Redraw();
            Task.Delay(3000, _cancel.Token).Wait();
        }
        
        return 0;
    }

    private void OnError(DeserializePackageException ex)
    {
        try
        {
            _rw.EnterWriteLock();
            var exist = _items.FirstOrDefault(_ => ex.MessageId == _.Msg);
            if (exist == null)
            {
                _items.Add(new DisplayRow { Msg = ex.MessageId, Message = ex.Message });
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

    private void Redraw()
    {
        DisplayRow[] items;
        IPacketV2<IPayload>[] packets;
        try
        {
            _rw.EnterReadLock();
            items = _items.ToArray();
            packets = _lastPackets.Where(_ => _ is FileTransferProtocolPacket).ToArray();
        }
        finally
        {
            _rw.ExitReadLock();
        }
        var time = DateTime.Now - _lastUpdate;
        foreach (var item in items)
        {
            item.Freq = (item.Count/ time.TotalSeconds).ToString("0.0 Hz");
            item.Count = 0;
        }
        _lastUpdate = DateTime.Now;
        Console.Clear();
        Console.WriteLine("Press Q for exit");
        Console.WriteLine("MAVLink inspector {0:0.0 Hz}:", ((_packetCount - _lastPacketCount) / time.TotalSeconds));
        TextTable.PrintTableFromObject(Console.WriteLine,new  DoubleTextTableBorder(),1,int.MaxValue,items.OrderBy(_=>_.Msg),_=>_.Msg,_=>_.Message,_=>_.Freq);
        Console.WriteLine("Last {0} packets of {1}:",packets.Length, _packetCount);
        _lastPacketCount = _packetCount;
        foreach (var packetV2 in packets)
        {
            Console.WriteLine(packetV2);
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

    private void OnPacket(IPacketV2<IPayload> packet)
    {
        Interlocked.Increment(ref _packetCount);
        try
        {
            _rw.EnterWriteLock();
            _lastPackets.Add(packet);
            if (_lastPackets.Count >= MaxHistorySize) _lastPackets.RemoveAt(0);
            var exist = _items.FirstOrDefault(_ => packet.MessageId == _.Msg);
            if (exist == null)
            {
                _items.Add(new DisplayRow {Msg = packet.MessageId, Message = packet.Name});
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
