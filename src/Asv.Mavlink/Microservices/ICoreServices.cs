using System;
using System.Diagnostics.Metrics;
using Asv.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

public interface ICoreServices
{
    IProtocolConnection Connection { get; }
    IPacketSequenceCalculator Sequence { get; }
    ILoggerFactory Log { get; }
    TimeProvider TimeProvider { get; }
    IMeterFactory Metrics { get; }
    
}



public class CoreServices(
    IProtocolConnection connection,
    IPacketSequenceCalculator? sequence = null,
    ILoggerFactory? logFactory = null,
    TimeProvider? timeProvider = null,
    IMeterFactory? metrics = null)
    : ICoreServices
{
    public IProtocolConnection Connection { get; } = connection;
    public IPacketSequenceCalculator Sequence { get; } = sequence ?? new PacketSequenceCalculator();
    public ILoggerFactory Log { get; } = logFactory ?? NullLoggerFactory.Instance;
    public TimeProvider TimeProvider { get; } = timeProvider ?? TimeProvider.System;
    public IMeterFactory Metrics { get; } = metrics ?? new DefaultMeterFactory();
}