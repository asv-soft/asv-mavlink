using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class SessionMetadata : ISizedSpanSerializable
    {
        public SessionMetadata()
        {
            
        }

        public SessionMetadata(SessionId sessionId,SessionSettings settings )
        {
            SessionId = sessionId;
            Settings = settings;
        }
        public SessionId SessionId { get; set; }
        public SessionSettings Settings { get; set; }

        public override string ToString()
        {
            return $"id:{SessionId}, {Settings}";
        }
        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Settings ??= new SessionSettings();
            Settings.Deserialize(ref buffer);
            SessionId ??= new SessionId();
            SessionId.Deserialize(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            Settings.Serialize(ref buffer);
            SessionId.Serialize(ref buffer);
        }

        public int GetByteSize()
        {
            return Settings.GetByteSize() + SessionId.GetByteSize();
        }

       
    }
}
