using System;
using System.Buffers;
using Asv.Mavlink.Test;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using ConsoleAppFramework;

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
    
    [SimpleJob(RuntimeMoniker.HostProcess)]
    [RPlotExporter]
    [MemoryDiagnoser]
    public class SerializationPacket
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private TestTypesPacket _packet;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private byte[] _data;

        [GlobalSetup]
        public void Setup()
        {
            _packet = new TestTypesPacket();
            _data = ArrayPool<byte>.Shared.Rent(280); 
            
            var writeSpan = new Span<byte>(_data);
            _packet.Serialize(ref writeSpan);
        }

        [Benchmark]
        public void Deserialize()
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

        [GlobalCleanup]
        public void Cleanup()
        {
            ArrayPool<byte>.Shared.Return(_data); 
        }
    }
}
