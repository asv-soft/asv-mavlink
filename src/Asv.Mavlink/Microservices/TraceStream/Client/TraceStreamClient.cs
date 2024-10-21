using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using R3;


namespace Asv.Mavlink;

public class TraceStreamClient : MavlinkMicroserviceClient, ITraceStreamClient
{
    private readonly RxValueBehaviour<DebugVectorMessage?> _onDebugVectorMessage;
    private readonly RxValueBehaviour<MemoryVectorMessage?> _onMemoryVectorMessage;
    private readonly RxValueBehaviour<NamedValueIntMessage?> _onNamedValueIntMessage;
    private readonly RxValueBehaviour<NamedValueFloatMessage?> _onNamedValueFloatMessage;
    private readonly RxValueBehaviour<string> _name;
    private readonly IDisposable _disposeIt;

    public TraceStreamClient(
        MavlinkClientIdentity identity,
        ICoreServices core) 
        : base("TRACESTREAM", identity, core)
    {
        _name = new RxValueBehaviour<string>($"[{identity.Target.SystemId},{identity.Target.ComponentId}]");
        _onDebugVectorMessage = new RxValueBehaviour<DebugVectorMessage?>(default);
        _onMemoryVectorMessage = new RxValueBehaviour<MemoryVectorMessage?>(default);
        _onNamedValueIntMessage = new RxValueBehaviour<NamedValueIntMessage?>(default);
        _onNamedValueFloatMessage = new RxValueBehaviour<NamedValueFloatMessage?>(default);

        var d1 = InternalFilter<DebugVectPacket>()
            .Select(packet => new DebugVectorMessage
            {
                Name = MavlinkTypesHelper.GetString(packet.Payload.Name),
                TimeUsec = packet.Payload.TimeUsec,
                X = packet.Payload.X,
                Y = packet.Payload.Y,
                Z = packet.Payload.Z
            })
            .Subscribe(_onDebugVectorMessage);


        var d2 = InternalFilter<MemoryVectPacket>()
            .Select(packet => new MemoryVectorMessage
            {
                Address = packet.Payload.Address,
                Version = packet.Payload.Ver,
                Type = packet.Payload.Type,
                Value = packet.Payload.Value
            })
            .Subscribe(_onMemoryVectorMessage);

        var d3 = InternalFilter<NamedValueIntPacket>()
            .Select(packet => new NamedValueIntMessage
            {
                TimeBoot = packet.Payload.TimeBootMs,
                Name = MavlinkTypesHelper.GetString(packet.Payload.Name),
                Value = packet.Payload.Value
            })
            .Subscribe(_onNamedValueIntMessage);

        var d4 = InternalFilter<NamedValueFloatPacket>()
            .Select(packet => new NamedValueFloatMessage
            {
                TimeBoot = packet.Payload.TimeBootMs,
                Name = MavlinkTypesHelper.GetString(packet.Payload.Name),
                Value = packet.Payload.Value
            })
            .Subscribe(_onNamedValueFloatMessage);
        
        _disposeIt = Disposable.Combine(_onDebugVectorMessage, _onMemoryVectorMessage, _onNamedValueIntMessage, _onNamedValueFloatMessage, d1, d2, d3, d4);
    }

    public IRxEditableValue<string> Name => _name;

    public IRxValue<DebugVectorMessage?> OnDebugVectorMessage => _onDebugVectorMessage;
    public IRxValue<MemoryVectorMessage?> OnMemoryVectorMessage => _onMemoryVectorMessage;
    public IRxValue<NamedValueIntMessage?> OnNamedValueIntMessage => _onNamedValueIntMessage;
    public IRxValue<NamedValueFloatMessage?> OnNamedValueFloatMessage => _onNamedValueFloatMessage;

    public override void Dispose()
    {
        _disposeIt.Dispose();
        base.Dispose();
    }
}