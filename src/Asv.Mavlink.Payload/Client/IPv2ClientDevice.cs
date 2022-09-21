using System;
using Asv.Common;

namespace Asv.Mavlink.Payload
{
    public interface IPv2ClientDevice : IDisposable
    {
        IPayloadV2Client Client { get; }
        IRxValue<VehicleInitState> InitState { get; }
        IRxValue<VehicleStatusMessage> TextStatus { get; }
        IRxValue<LinkState> Link { get; }
        IRxValue<int> PacketRateHz { get; }
        IRxValue<double> LinkQuality { get; }
        IPv2ClientParamsInterface Params { get; }
        IPv2ClientBaseInterface Base { get; }
        IPv2PowerCycle Power { get; }
        IPv2ClientRttInterface Rtt { get; }
        IPv2ClientMissionInterface Mission { get; }
    }
}
