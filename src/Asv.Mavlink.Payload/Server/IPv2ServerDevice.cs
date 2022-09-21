using System;

namespace Asv.Mavlink.Payload
{
    public interface IPv2ServerDevice : IDisposable
    {
        IPayloadV2Server Server { get; }
        IPv2ServerParamsInterface Params { get; }
        IPv2ServerBaseInterface Base { get; }
        IPv2ServerPowerInterface Power { get; }
        IPv2ServerRttInterface Rtt { get; }
        IPv2ServerMissionInterface Mission { get; }
        IPacketSequenceCalculator Seq { get; }
    }
}
