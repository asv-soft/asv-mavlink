using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink
{
    public class DebugClient: MavlinkMicroserviceClient, IDebugClient
    {
        public DebugClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, IScheduler? scheduler = null,ILogger? logger = null):base("DEBUG", connection, identity, seq,scheduler,logger)
        {
            DebugFloatArray = InternalFilter<DebugFloatArrayPacket>()
                .Select(p => p.Payload)
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
