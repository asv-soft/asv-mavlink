using System;
using System.Threading.Tasks;
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
        void Start(TimeSpan rate);
        bool IsStarted { get; }
        IRxValue<PacketTransponderState> State { get; }
        void Stop();
        Task Set(Action<TPayload> changeCallback);
    }
}
