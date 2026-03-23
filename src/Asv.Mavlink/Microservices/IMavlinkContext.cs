using System;
using System.Diagnostics.Metrics;
using Asv.IO;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public interface IMavlinkContext : IMicroserviceContext
{
    IProtocolMessageFactory<MavlinkMessage, int> MessageFactory { get; }
    IPacketSequenceCalculator Sequence { get; }
}

public class CoreServices : MicroserviceContext, IMavlinkContext
{
    public CoreServices(IProtocolConnection connection,
        IProtocolMessageFactory<MavlinkMessage, int> messageFactory,
        IPacketSequenceCalculator? sequence = null,
        ILoggerFactory? logFactory = null,
        TimeProvider? timeProvider = null,
        IMeterFactory? metrics = null) : base(connection, logFactory, timeProvider, metrics)
    {
        MessageFactory = messageFactory;
        Sequence = sequence ?? new PacketSequenceCalculator();
    }

    public CoreServices(IPacketSequenceCalculator seq, IMicroserviceContext context, IProtocolMessageFactory<MavlinkMessage, int> messageFactory) 
        : base(context)
    {
        Sequence = seq;
        MessageFactory = messageFactory;
    }
    public CoreServices(IPacketSequenceCalculator seq, IProtocolConnection connection, IProtocolFactory factory, IProtocolMessageFactory<MavlinkMessage, int> messageFactory) 
        : base(connection,factory.LoggerFactory,factory.TimeProvider,factory.MeterFactory)
    {
        Sequence = seq;
        MessageFactory = messageFactory;
    }

    public IProtocolMessageFactory<MavlinkMessage, int> MessageFactory { get; }
    public IPacketSequenceCalculator Sequence { get; }
}