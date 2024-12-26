using System;
using System.IO;
using Asv.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using ConsoleAppFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Asv.Mavlink.Shell
{
    public class BenchmarkBinSerializationCommand
    {
        /// <summary>
        /// Benchmark test
        /// </summary>
        [Command("benchmark-bin-serialize")]
        public void RunBenchmarkBinSerialize()
        {
            BenchmarkRunner.Run<BinSerialization>();
        }
    }

    public class TestClass : ISizedSpanSerializable
    {
        public uint Uint1 { get; set; } = 100500;
        public double Double1 { get; set; } = Double.Epsilon;
        public string String { get; set; } = "ASDASDASDAS";

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Uint1 = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            Double1 = BinSerialize.ReadDouble(ref buffer);
            String = BinSerialize.ReadString(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WritePackedUnsignedInteger(ref buffer,Uint1);
            BinSerialize.WriteDouble(ref buffer, Double1);
            BinSerialize.WriteString(ref buffer, String);
        }

        public int GetByteSize()
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger(Uint1) +
                   sizeof(double) +
                   BinSerialize.GetSizeForString(String);
        }
    }

    [SimpleJob(RuntimeMoniker.Net461)]
    [SimpleJob(RuntimeMoniker.CoreRt60)]
    //[SimpleJob(RuntimeMoniker.Mono)]
    [RPlotExporter]
    [MemoryDiagnoser]
    public class BinSerialization
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private TestClass _packet;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        [GlobalSetup]
        public void Setup()
        {
            _packet = new TestClass();
        }

        [Benchmark]
        public void SerializeBson()
        {
            var ms = new MemoryStream();
#pragma warning disable CS0618 // Type or member is obsolete
            using var writer = new BsonWriter(ms);
#pragma warning restore CS0618 // Type or member is obsolete
            var serializer = new JsonSerializer();
            serializer.Serialize(writer, _packet);
        }

        [Benchmark]
        public void SerializeBinSerializer()
        {
            var data = new byte[_packet.GetByteSize()];
            var span = new Span<byte>(data);
            _packet.Serialize(ref span);
        }
    }
}
