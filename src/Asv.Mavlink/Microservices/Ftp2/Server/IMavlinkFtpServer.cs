using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

public interface IMavlinkFtpServer
{
    
}

public class MavlinkFtpServer : MavlinkMicroserviceServer, IMavlinkFtpServer
{
    private readonly ILogger _logger;
    private readonly IObservable<FileTransferProtocolPacket> _ftpMessages;

    public MavlinkFtpServer(
        IMavlinkV2Connection connection, 
        MavlinkIdentity identity, 
        IPacketSequenceCalculator seq, 
        IScheduler? rxScheduler = null, 
        ILogger? logger = null) : base("FTP", connection, identity, seq, rxScheduler, logger)
    {
        _logger = logger ?? NullLogger.Instance;
        _ftpMessages = connection
            .Filter<FileTransferProtocolPacket>()
            .Where(x=>x.Payload.TargetComponent == identity.ComponentId && x.Payload.TargetSystem == identity.SystemId)
            .Publish()
            .RefCount();
        
    }
}