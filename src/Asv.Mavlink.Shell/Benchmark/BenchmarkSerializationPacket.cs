using System;
using Asv.Mavlink.V2.Test;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using ConsoleAppFramework;
using ManyConsole;

namespace Asv.Mavlink.Shell
{
    public class BenchmarkSerializationPacket
    {
        /// <summary>
        /// Benchmark serialization packet test
        /// </summary>
        [Command("benchmark-serialization")]
        public void RunBenchmarkSerialization()
        { 
            BenchmarkRunner.Run<SerializationPacket>();
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
