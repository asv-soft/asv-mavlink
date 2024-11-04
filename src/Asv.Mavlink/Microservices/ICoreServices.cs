using System;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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
    IPacketSequenceCalculator? sequence = null,
    ILoggerFactory? logFactory = null,
    TimeProvider? timeProvider = null,
    IMeterFactory? metrics = null)
    : ICoreServices
{
    public IMavlinkV2Connection Connection { get; } = connection;
    public IPacketSequenceCalculator Sequence { get; } = sequence ?? new PacketSequenceCalculator();
    public ILoggerFactory Log { get; } = logFactory ?? NullLoggerFactory.Instance;
    public TimeProvider TimeProvider { get; } = timeProvider ?? TimeProvider.System;
    public IMeterFactory Metrics { get; } = metrics ?? new DefaultMeterFactory();
}