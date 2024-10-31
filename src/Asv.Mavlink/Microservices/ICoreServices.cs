using System;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public interface ICoreServices
{
    IMavlinkV2Connection Connection { get; }
    IPacketSequenceCalculator Sequence { get; }
    ILoggerFactory Log { get; }
    TimeProvider TimeProvider { get; }
    IMeterFactory Metrics { get; }
    
}



public class CoreServices(
    IMavlinkV2Connection connection,
    IPacketSequenceCalculator sequence,
    ILoggerFactory logFactory,
    TimeProvider timeProvider,
    IMeterFactory metrics)
    : ICoreServices
{
    public IMavlinkV2Connection Connection { get; } = connection;
    public IPacketSequenceCalculator Sequence { get; } = sequence;
    public ILoggerFactory Log { get; } = logFactory;
    public TimeProvider TimeProvider { get; } = timeProvider;
    public IMeterFactory Metrics { get; } = metrics;
}