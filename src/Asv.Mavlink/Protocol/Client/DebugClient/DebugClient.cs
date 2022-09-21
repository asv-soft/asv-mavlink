using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Client
{
    public class DebugClient: MavlinkMicroserviceClient, IDebugClient
    {
        private readonly RxValue<DebugFloatArrayPayload> _debugFloatArray = new();
        private readonly CancellationTokenSource _disposeCancel = new();

        public DebugClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, IPacketSequenceCalculator seq):base(connection, identity, seq,"DEBUG")
        {
            Filter<DebugFloatArrayPacket>()
                .Select(_ => _.Payload)
                .Subscribe(_debugFloatArray, _disposeCancel.Token);
            Disposable.Add(_debugFloatArray);
        }

        private static string ConvertToKey(char[] payloadName)
        {
            return new string(payloadName.Where(_=>_ != 0).ToArray());
        }

        

        private static KeyValuePair<string, float> ConvertValue(NamedValueFloatPacket _)
        {
            return new KeyValuePair<string, float>(ConvertToKey(_.Payload.Name), _.Payload.Value);
        }

        private static KeyValuePair<string, int> ConvertValue(NamedValueIntPacket _)
        {
            return new KeyValuePair<string, int>(ConvertToKey(_.Payload.Name), _.Payload.Value);
        }

        public IObservable<KeyValuePair<string, float>> NamedFloatValue => Filter<NamedValueFloatPacket>()
            .Select(ConvertValue)
            .Publish()
            .RefCount();

        public IObservable<KeyValuePair<string, int>> NamedIntValue => Filter<NamedValueIntPacket>()
            .Select(ConvertValue)
            .Publish()
            .RefCount();
        public IRxValue<DebugFloatArrayPayload> DebugFloatArray => _debugFloatArray;
        
    }
}
