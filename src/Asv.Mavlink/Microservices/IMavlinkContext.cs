using System;
using System.Diagnostics.Metrics;
using Asv.IO;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public interface IMavlinkContext:IMicroserviceContext
{
    IPacketSequenceCalculator Sequence { get; }
}



public class CoreServices : MicroserviceContext, IMavlinkContext
{
    public CoreServices(IProtocolConnection connection,
        IPacketSequenceCalculator? sequence = null,
        ILoggerFactory? logFactory = null,
        TimeProvider? timeProvider = null,
        IMeterFactory? metrics = null) : base(connection, logFactory, timeProvider, metrics)
    {
        Sequence = sequence ?? new PacketSequenceCalculator();
    }

    public CoreServices(IPacketSequenceCalculator seq,IMicroserviceContext context) 
        : base(context)
    {
        Sequence = seq;
    }
    public CoreServices(IPacketSequenceCalculator seq, IProtocolConnection connection, IProtocolFactory factory) 
        : base(connection,factory.LoggerFactory,factory.TimeProvider,factory.MeterFactory)
    {
        Sequence = seq;
    }

    public IPacketSequenceCalculator Sequence { get; }
}