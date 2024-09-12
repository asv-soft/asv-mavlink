using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

public interface IMavlinkFtpClient
{
    
}

public class MavlinkFtpClient : MavlinkMicroserviceClient, IMavlinkFtpClient
{
    private readonly ILogger _logger;
    private readonly IObservable<FileTransferProtocolPacket> _ftpMessages;

    public MavlinkFtpClient(
        IMavlinkV2Connection connection,
        MavlinkClientIdentity identity, 
        IPacketSequenceCalculator seq,
        IScheduler? scheduler = null,
        ILogger? logger = null
        ):base("FTP",connection,identity,seq,scheduler,logger)
    {
        _logger = logger ?? NullLogger.Instance;
        _ftpMessages = connection
            .FilterVehicle(identity)
            .Filter<FileTransferProtocolPacket>()
            .Publish()
            .RefCount();
        
    }
}