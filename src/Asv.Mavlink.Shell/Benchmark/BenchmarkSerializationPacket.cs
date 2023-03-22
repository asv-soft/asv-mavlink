using System;
using Asv.Mavlink.V2.Test;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using ManyConsole;

namespace Asv.Mavlink.Shell
{
    public class BenchmarkSerializationPacket : ConsoleCommand
    {
        public BenchmarkSerializationPacket()
        {
            IsCommand("benchmark-serialization", "Benchmark serialization packet test");
        }

        public override int Run(string[] remainingArguments)
        {
            var summary = BenchmarkRunner.Run<SerializationPacket>();
            return 0;
        }
    }


    [SimpleJob(RuntimeMoniker.Net461)] 
    [SimpleJob(RuntimeMoniker.CoreRt50)]
    //[SimpleJob(RuntimeMoniker.Mono)]
    [RPlotExporter]
    [MemoryDiagnoser]
    public class SerializationPacket
    {
        private TestTypesPacket _packet;
        private byte[] _data = new byte[250];

        [GlobalSetup]
        public void Setup()
        {
            _packet = new TestTypesPacket();
            _data = new byte[280];
           

        }

        [Benchmark]
        public void DeserializeSpan()
        {
            var span = new ReadOnlySpan<byte>(_data);
            _packet.Deserialize(ref span);
        }

        [Benchmark]
        public void SerializeSpan()
        {
            var span = new Span<byte>(_data);
            _packet.Serialize(ref span);
        }

        
    }
}
