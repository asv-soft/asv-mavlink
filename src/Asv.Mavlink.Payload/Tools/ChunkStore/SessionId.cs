using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class SessionId : ISizedSpanSerializable
    {
        public const int MaxByteSize = 16;

        public Guid Guid { get; set; } = Guid.Empty;

        public SessionId(Guid guid)
        {
            Guid = guid;
        }

        public SessionId()
        {
            
        }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Guid = new Guid(BinSerialize.ReadBlock(ref buffer, MaxByteSize));
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteBlock(ref buffer, new ReadOnlySpan<byte>(Guid.ToByteArray()));
        }

        public int GetByteSize()
        {
            return MaxByteSize;
        }

        public override string ToString()
        {
            return Guid.ToString();
        }
    }
}
