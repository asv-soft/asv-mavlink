using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public class DebugClient: MavlinkMicroserviceClient, IDebugClient
    {
        public DebugClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq):base("DEBUG", connection, identity, seq)
        {
            DebugFloatArray = InternalFilter<DebugFloatArrayPacket>()
                .Select(_ => _.Payload)
                .Publish()
                .RefCount();
            NamedFloatValue = InternalFilter<NamedValueFloatPacket>()
                .Select(ConvertValue)
                .Publish()
                .RefCount();
            NamedIntValue =  InternalFilter<NamedValueIntPacket>()
                .Select(ConvertValue)
                .Publish()
                .RefCount();
        }

        private static KeyValuePair<string, float> ConvertValue(NamedValueFloatPacket _)
        {
            return new KeyValuePair<string, float>(MavlinkTypesHelper.GetString(_.Payload.Name), _.Payload.Value);
        }

        private static KeyValuePair<string, int> ConvertValue(NamedValueIntPacket _)
        {
            return new KeyValuePair<string, int>(MavlinkTypesHelper.GetString(_.Payload.Name), _.Payload.Value);
        }
        public IObservable<KeyValuePair<string, float>> NamedFloatValue { get; } 
        public IObservable<KeyValuePair<string, int>> NamedIntValue { get; }
        public IObservable<DebugFloatArrayPayload> DebugFloatArray { get; }
    }
        
    
}
