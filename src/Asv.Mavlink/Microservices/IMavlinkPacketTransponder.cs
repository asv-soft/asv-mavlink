using System;
using Asv.Common;

namespace Asv.Mavlink
{
    public enum PacketTransponderState
    {
        Ok,
        Skipped,
        ErrorToSend,
    }

    public interface IMavlinkPacketTransponder<TPacket, out TPayload> : IDisposable
        where TPacket : IPacketV2<TPayload>, new()
        where TPayload : IPayload, new()
    {
        void Start(DateTimeOffset dueTime,TimeSpan period);
        bool IsStarted { get; }
        IRxValue<PacketTransponderState> State { get; }
        void Stop();
        void Set(Action<TPayload> changeCallback);
    }
}
