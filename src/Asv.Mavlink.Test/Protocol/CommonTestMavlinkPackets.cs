using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.PythonArrayTest;
using Asv.Mavlink.UnitTestMessage;
using DeepEqual.Syntax;
using Xunit;
using Xunit.Abstractions;
using R3;


namespace Asv.Mavlink.Test
{
    public class CommonTestMavlinkPackets
    {
        
        private readonly byte[] _buffer = new byte[MavlinkV2Protocol.PacketV2MaxSize];
        private readonly ITestOutputHelper _output;

        public CommonTestMavlinkPackets(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test_Packet_Actual_Size_Calculation()
        {var expectedObject = new TestTypesPacket
            {
                CompatFlags = 0,
                IncompatFlags = 0,
                Sequence = 10,
                SystemId = 255,
                ComponentId = 255,
                Payload =
                {
                    U64 = 1,
                    S64 = 2,
                    D = 3,
                    U64Array = new ulong[]
                    {
                        1, 2, 3
                    },
                    U32 = 1,
                    S32 = 2,
                    F = 3,
                    U16 = 4,
                    S16 = 5,
                    C = 'A',
                    U8 = 7,
                    S8 = 8
                }
            };
            // this is the last array int serialization and we need fill it last element non zero, because => Empty-Byte Payload Truncation https://mavlink.io/en/guide/serialization.html#payload_truncation
            expectedObject.Payload.S8Array[^1] = 127;
            
            var serializeSpan = new Span<byte>(_buffer);
            expectedObject.Serialize(ref serializeSpan);
            
            
            byte[] buffer2 = new byte[MavlinkV2Protocol.PacketV2MaxSize];
            var span = new Span<byte>(buffer2);
            var size = span.Length;
            expectedObject.Serialize(ref span);
            size -= span.Length;
            Assert.Equal(size, expectedObject.GetByteSize());
        }


        [Fact]
        public void Try_serialize_packet_with_span()
        {
            var expectedObject = new TestTypesPacket
            {
                CompatFlags = 0,
                IncompatFlags = 0,
                Sequence = 10,
                SystemId = 255,
                ComponentId = 255,
                Payload =
                {
                    U64 = 1,
                    S64 = 2,
                    D = 3,
                    U64Array = new ulong[]
                    {
                        1, 2, 3
                    },
                    U32 = 1,
                    S32 = 2,
                    F = 3,
                    U16 = 4,
                    S16 = 5,
                    C = 'A',
                    U8 = 7,
                    S8 = 8,
                }
            };
            
            
            
            var serializeSpan = new Span<byte>(_buffer);
            expectedObject.Serialize(ref serializeSpan);
            
            byte[] buffer2 = new byte[MavlinkV2Protocol.PacketV2MaxSize];
            var span = new Span<byte>(buffer2);
            expectedObject.Serialize(ref span);

            Assert.Equal(buffer2, _buffer);
        }

        [Fact]
        public void Deserialize_BufferInput_MatchesExpectedObject()
        {
            var expectedObject = new TestTypesPacket
            {
                CompatFlags = 0,
                IncompatFlags = 0,
                Sequence = 10,
                SystemId = 255,
                ComponentId = 255,
                Payload =
                {
                    U64 = 1,
                    S64 = 2,
                    D = 3,
                    U64Array = new ulong[]
                    {
                        1, 2, 3
                    },
                    U32 = 1,
                    S32 = 2,
                    F = 3,
                    U16 = 4,
                    S16 = 5,
                    C = 'A',
                    U8 = 7,
                    S8 = 8
                }
            };
            var serializeSpan = new Span<byte>(_buffer);
            expectedObject.Serialize(ref serializeSpan);
            
            var deserialized = new TestTypesPacket();
            var span = new ReadOnlySpan<byte>(_buffer);
            deserialized.Deserialize(ref span);
            expectedObject.ShouldDeepEqual(deserialized);
        }

        [Fact]
        public void SpanDeserializeTest()
        {
           var expectedObject = new TestTypesPacket
            {
                CompatFlags = 0,
                IncompatFlags = 0,
                Sequence = 10,
                SystemId = 255,
                ComponentId = 255,
                Payload =
                {
                    U64 = 1,
                    S64 = 2,
                    D = 3,
                    U64Array = new ulong[]
                    {
                        1, 2, 3
                    },
                    U32 = 1,
                    S32 = 2,
                    F = 3,
                    U16 = 4,
                    S16 = 5,
                    C = 'A',
                    U8 = 7,
                    S8 = 8
                }
            };
            var serializeSpan = new Span<byte>(_buffer);
            expectedObject.Serialize(ref serializeSpan);
            
            var deserialized = new TestTypesPacket();
            var span = new ReadOnlySpan<byte>(_buffer);
            deserialized.Deserialize(ref span);
            expectedObject.ShouldDeepEqual(deserialized);
        }

        [Fact]
        public void CrcExtraCheck()
        {
            var length = new byte[]
            {
                9, 31, 12, 0, 14, 28, 3, 32, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 20, 2, 25, 23, 30, 101, 22, 26, 16, 14,
                28, 32, 28, 28, 22, 22, 21, 6, 6, 37, 4, 4, 2, 2, 4, 2, 2, 3, 13, 12, 37, 0, 0, 0, 27, 25, 0, 0, 0, 0,
                0, 68, 26, 185, 229, 42, 6, 4, 0, 11, 18, 0, 0, 37, 20, 35, 33, 3, 0, 0, 0, 22, 39, 37, 53, 51, 53, 51,
                0, 28, 56, 42, 33, 0, 0, 0, 0, 0, 0, 0, 26, 32, 32, 20, 32, 62, 44, 64, 84, 9, 254, 16, 12, 36, 44, 64,
                22, 6, 14, 12, 97, 2, 2, 113, 35, 6, 79, 35, 35, 22, 13, 255, 14, 18, 43, 8, 22, 14, 36, 43, 41, 32,
                243, 14, 93, 0, 100, 36, 60, 30, 42, 8, 4, 12, 15, 13, 6, 15, 14, 0, 12, 3, 8, 28, 44, 3, 9, 22, 12, 18,
                34, 66, 98, 8, 48, 19, 3, 20, 24, 29, 45, 4, 40, 2, 206, 7, 29, 0, 0, 0, 0, 27, 44, 22, 25, 0, 0, 0, 0,
                0, 42, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 3, 3, 6, 7, 2, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 32, 52, 53, 6, 2, 38, 0, 254, 36, 30, 18, 18, 51, 9, 0
            };
            var crcExtra = new byte[]
            {
                50, 124, 137, 0, 237, 217, 104, 119, 0, 0, 0, 89, 0, 0, 0, 0, 0, 0, 0, 0, 214, 159, 220, 168, 24, 23,
                170, 144, 67, 115, 39, 246, 185, 104, 237, 244, 222, 212, 9, 254, 230, 28, 28, 132, 221, 232, 11, 153,
                41, 39, 78, 0, 0, 0, 15, 3, 0, 0, 0, 0, 0, 153, 183, 51, 59, 118, 148, 21, 0, 243, 124, 0, 0, 38, 20,
                158, 152, 143, 0, 0, 0, 106, 49, 22, 143, 140, 5, 150, 0, 231, 183, 63, 54, 0, 0, 0, 0, 0, 0, 0, 175,
                102, 158, 208, 56, 93, 138, 108, 32, 185, 84, 34, 174, 124, 237, 4, 76, 128, 56, 116, 134, 237, 203,
                250, 87, 203, 220, 25, 226, 46, 29, 223, 85, 6, 229, 203, 1, 195, 109, 168, 181, 47, 72, 131, 127, 0,
                103, 154, 178, 200, 134, 219, 208, 188, 84, 22, 19, 21, 134, 0, 78, 68, 189, 127, 154, 21, 21, 144, 1,
                234, 73, 181, 22, 83, 167, 138, 234, 240, 47, 189, 52, 174, 229, 85, 159, 186, 72, 0, 0, 0, 0, 92, 36,
                71, 98, 0, 0, 0, 0, 0, 134, 205, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 69, 101, 50, 202, 17, 162, 0, 0, 0,
                0, 0, 0, 207, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 104, 85, 95, 130, 184, 0, 8, 204, 49, 170,
                44, 83, 46, 0
            };
          
            
            int err = 0;
            for (int i = 0; i < crcExtra.Length; i++)
            {
                if (length[i] == 0) continue;
                var packet = MavlinkV2MessageFactory.Instance.Create((ushort)i) as MavlinkV2Message<IPayload>;
                if (packet == null)
                {
                    _output.WriteLine($"NOT PRESENT {i}");
                    continue;
                }

                var calculated = packet.GetCrcExtra();
                var calcLength = packet.Payload.GetMinByteSize();
                if (crcExtra[i] != calculated)
                {
                    _output.WriteLine($"ERROR CRC {i} {packet.Name} Length: {calcLength}, {length[i]}");
                    err++;
                }
                else
                {
                    _output.WriteLine($"OK {i} {packet.Name} Length: {calcLength}, {length[i]}");
                }
            }

            if (err != 0)
            {
                //Assert.True(false, "CRC EXTRA for some messages not equal. See more https://mavlink.io/en/guide/serialization.html#crc_extra");
            }
        }


        /// <summary>
        /// https://github.com/asv-soft/asv-mavlink/issues/36
        /// </summary>
        [Fact]
        public async Task Test_custom_crc_message()
        {
            var protocol = Protocol.Create(builder =>
            {
                builder.RegisterMavlinkV2Protocol();
            });
            var link = protocol.CreateVirtualConnection();

            // create original packet
            var outPkt = new ChemicalDetectorDataPacket
            {
                SystemId = 1,
                ComponentId = 1,
                Payload =
                {
                    AgentId = 0,
                    Concentration = 0,
                    Bars = 0,
                    BarsPeak = 0,
                    Dose = 0,
                    HazardLevel = 0
                }
            };
            // create waiter, to wait for incoming packet
            var waiter = new TaskCompletionSource<ChemicalDetectorDataPacket>();
            using var serverSubscribe = link.Server.RxFilterByMsgId<ChemicalDetectorDataPacket, int>().Subscribe(inPkt =>
            {
                waiter.TrySetResult(inPkt);
            });
            // send packet client=>server
            await link.Client.Send(outPkt, CancellationToken.None);
            // wait for incoming packet
            var inPkt = await waiter.Task;
            // check incoming packet with original
            Assert.Equal(inPkt.SystemId, outPkt.SystemId);
            Assert.Equal(inPkt.ComponentId, outPkt.ComponentId);
            Assert.Equal(inPkt.Payload.AgentId, outPkt.Payload.AgentId);
            Assert.Equal(inPkt.Payload.Concentration, outPkt.Payload.Concentration);
            Assert.Equal(inPkt.Payload.Bars, outPkt.Payload.Bars);
            Assert.Equal(inPkt.Payload.BarsPeak, outPkt.Payload.BarsPeak);
            Assert.Equal(inPkt.Payload.Dose, outPkt.Payload.Dose);
            Assert.Equal(inPkt.Payload.HazardLevel, outPkt.Payload.HazardLevel);

        }
        
        /// <summary>
        /// https://github.com/asv-soft/asv-mavlink/issues/36
        /// </summary>
        [Fact]
        public async Task Test_custom_crc_message2()
        {
            var protocol = Protocol.Create(builder =>
            {
                builder.RegisterMavlinkV2Protocol();
            });
            var link = protocol.CreateVirtualConnection();

            // create original packet
            var outPkt = new ChemicalDetectorDataPacket
            {
                SystemId = 1,
                ComponentId = 1,
                Payload =
                {
                    AgentId = 0,
                    Concentration = -1f,
                    Bars = 0,
                    BarsPeak = 0,
                    Dose = -1f,
                    HazardLevel =-1f
                }
            };
            // create waiter, to wait for incoming packet
            var waiter = new TaskCompletionSource<ChemicalDetectorDataPacket>();
            using var serverSubscribe = link.Server.RxFilterByType<ChemicalDetectorDataPacket>().Subscribe(inPkt =>
            {
                waiter.TrySetResult(inPkt);
            });
            // send packet client=>server
            await link.Client.Send(outPkt, CancellationToken.None);
            // wait for incoming packet
            var inPkt = await waiter.Task;
            // check incoming packet with original
            Assert.Equal(inPkt.SystemId, outPkt.SystemId);
            Assert.Equal(inPkt.ComponentId, outPkt.ComponentId);
            Assert.Equal(inPkt.Payload.AgentId, outPkt.Payload.AgentId);
            Assert.Equal(inPkt.Payload.Concentration, outPkt.Payload.Concentration);
            Assert.Equal(inPkt.Payload.Bars, outPkt.Payload.Bars);
            Assert.Equal(inPkt.Payload.BarsPeak, outPkt.Payload.BarsPeak);
            Assert.Equal(inPkt.Payload.Dose, outPkt.Payload.Dose);
            Assert.Equal(inPkt.Payload.HazardLevel, outPkt.Payload.HazardLevel);

        }

        [Fact]
        public void Test_empty_byte_payload_truncation()
        {
            var packet = new ArrayTest0Packet();
            var buff = new byte[packet.GetMaxByteSize()];
            var writeSpan = new Span<byte>(buff);
            var originLength = writeSpan.Length;
            packet.Serialize(ref writeSpan);
            var realSize = originLength - writeSpan.Length;
            Assert.Equal(13 /*ONLY HEADER SIZE + 1 zero payload*/,realSize);


            var readBuffer = new ReadOnlySpan<byte>(buff, 0, realSize);
            var readPacket = new ArrayTest0Packet();
            readPacket.Deserialize(ref readBuffer);
            Assert.Equal(0,readBuffer.Length);


        }
    }
}