using System;
using System.IO;
using System.Linq;
using Asv.IO;
using DeepEqual.Syntax;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Payload.Test
{
    public class ChunkStoreTest
    {
        private readonly ITestOutputHelper _output;

        public ChunkStoreTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ArgumentsValidation()
        {
            SpanTestHelper.SerializeDeserializeTestBegin(_output.WriteLine);
            SpanTestHelper.TestType(new SessionId(Guid.NewGuid()), _output.WriteLine);
            SpanTestHelper.TestType(new SessionSettings("Name1","Tag1", "Tag2", "Tag3"), _output.WriteLine);
            SpanTestHelper.TestType(new SessionMetadata(new SessionId(Guid.NewGuid()), new SessionSettings("Name1", "Tag1", "Tag2", "Tag3")), _output.WriteLine);
            SpanTestHelper.TestType(new SessionInfo(new SessionMetadata(new SessionId(Guid.NewGuid()), new SessionSettings("Name1", "Tag1", "Tag2", "Tag3")), UInt32.MaxValue,UInt32.MinValue, UInt32.MaxValue,DateTime.Now), _output.WriteLine);

            SpanTestHelper.TestType(new SessionFieldSettings(UInt32.MaxValue,"Name1",ushort.MaxValue), _output.WriteLine);
            SpanTestHelper.TestType(new SessionRecordMetadata(new SessionFieldSettings(UInt32.MaxValue, "Name1", ushort.MaxValue)), _output.WriteLine);
            SpanTestHelper.TestType(new SessionFieldInfo(new SessionRecordMetadata(new SessionFieldSettings(UInt32.MaxValue, "Name1", ushort.MaxValue)), UInt32.MaxValue, UInt32.MaxValue), _output.WriteLine);


        }

        [Fact]
        public void ReadWriteTest()
        {
            var rootFolder = Path.Combine(Path.GetTempPath(),Path.GetRandomFileName());
            var svc = new ChunkFileStore(rootFolder);
            var metadata = svc.Start(new SessionSettings("rec1", "tag1", "tag2"), new[]
            {
                new SessionFieldSettings(0,"BlaBla",256)
            });

            for (uint i = 0; i < 1000; i++)
            {
                var value = i;
                svc.Append(0, (ref Span<byte> data) =>
                {
                    BinSerialize.WritePackedUnsignedInteger(ref data,value);
                });
            };
            svc.Stop();

            var session = svc.GetSessions();
            Assert.NotNull(session);
            Assert.True(session.Any());
            Assert.Equal(session.First(), new SessionId(metadata.SessionId.Guid));

            var readedMetadata = svc.GetSessionInfo(metadata.SessionId);
            readedMetadata.WithDeepEqual(metadata).Assert();

            for (uint i = 0; i < 1000; i++)
            {
                uint value = 0;
                svc.ReadRecord(metadata.SessionId,0,i, (ref ReadOnlySpan<byte> data) =>
                {
                    value = BinSerialize.ReadPackedUnsignedInteger(ref data);
                });
                Assert.Equal(value,i);
            }


        }

       
    }
}
