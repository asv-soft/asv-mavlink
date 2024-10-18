using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;


namespace Asv.Mavlink;

public class TraceStreamClient : MavlinkMicroserviceClient, ITraceStreamClient
{
    private readonly RxValue<DebugVectorMessage> _onDebugVectorMessage;
    private readonly RxValue<MemoryVectorMessage> _onMemoryVectorMessage;
    private readonly RxValue<NamedValueIntMessage> _onNamedValueIntMessage;
    private readonly RxValue<NamedValueFloatMessage> _onNamedValueFloatMessage;

    public TraceStreamClient(
        IMavlinkV2Connection connection, 
        MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq,
        TimeProvider? timeProvider = null,
        IScheduler? scheduler = null,
        ILoggerFactory? logFactory = null) 
        : base("TRACESTREAM", connection, identity, seq, timeProvider,scheduler,logFactory)
    {
        Name = new RxValue<string>($"[{identity.TargetSystemId},{identity.TargetComponentId}]")
            .DisposeItWith(Disposable);

        _onDebugVectorMessage = new RxValue<DebugVectorMessage>().DisposeItWith(Disposable);
        _onMemoryVectorMessage = new RxValue<MemoryVectorMessage>().DisposeItWith(Disposable);
        _onNamedValueIntMessage = new RxValue<NamedValueIntMessage>().DisposeItWith(Disposable);
        _onNamedValueFloatMessage = new RxValue<NamedValueFloatMessage>().DisposeItWith(Disposable);

        InternalFilter<DebugVectPacket>()
            .Select(packet => new DebugVectorMessage
            {
                Name = MavlinkTypesHelper.GetString(packet.Payload.Name),
                TimeUsec = packet.Payload.TimeUsec,
                X = packet.Payload.X,
                Y = packet.Payload.Y,
                Z = packet.Payload.Z
            })
            .Subscribe(_onDebugVectorMessage)
            .DisposeItWith(Disposable);

        InternalFilter<MemoryVectPacket>()
            .Select(packet => new MemoryVectorMessage
            {
                Address = packet.Payload.Address,
                Version = packet.Payload.Ver,
                Type = packet.Payload.Type,
                Value = packet.Payload.Value
            })
            .Subscribe(_onMemoryVectorMessage)
            .DisposeItWith(Disposable);

        InternalFilter<NamedValueIntPacket>()
            .Select(packet => new NamedValueIntMessage
            {
                TimeBoot = packet.Payload.TimeBootMs,
                Name = MavlinkTypesHelper.GetString(packet.Payload.Name),
                Value = packet.Payload.Value
            })
            .Subscribe(_onNamedValueIntMessage)
            .DisposeItWith(Disposable);

        InternalFilter<NamedValueFloatPacket>()
            .Select(packet => new NamedValueFloatMessage
            {
                TimeBoot = packet.Payload.TimeBootMs,
                Name = MavlinkTypesHelper.GetString(packet.Payload.Name),
                Value = packet.Payload.Value
            })
            .Subscribe(_onNamedValueFloatMessage)
            .DisposeItWith(Disposable);
    }

    public IRxEditableValue<string> Name { get; }

    public IRxValue<DebugVectorMessage> OnDebugVectorMessage => _onDebugVectorMessage;
    public IRxValue<MemoryVectorMessage> OnMemoryVectorMessage => _onMemoryVectorMessage;
    public IRxValue<NamedValueIntMessage> OnNamedValueIntMessage => _onNamedValueIntMessage;
    public IRxValue<NamedValueFloatMessage> OnNamedValueFloatMessage => _onNamedValueFloatMessage;
}