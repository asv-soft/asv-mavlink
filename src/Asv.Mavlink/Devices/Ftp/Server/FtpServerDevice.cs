using System;
using System.IO.Abstractions;
using System.Reactive.Concurrency;
using Asv.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class FtpServerDeviceConfig: ServerDeviceConfig
{
    public required MavlinkFtpServerConfig ServerCfg { get; set; }
    public required MavlinkFtpServerExConfig ServerExCfg { get; set; }
}

public sealed class FtpServerDevice : ServerDevice, IFtpServerDevice
{
    public FtpServerDevice(
        IMavlinkV2Connection connection, 
        IPacketSequenceCalculator seq, 
        MavlinkIdentity identity, 
        FtpServerDeviceConfig config, 
        IFileSystem? fileSystem = null,
        TimeProvider? timeProvider = null, 
        IScheduler? scheduler = null, 
        ILoggerFactory? logFactory = null) : base(connection, seq, identity, config, timeProvider, scheduler, logFactory
    )
    {
        var server = new FtpServer(config.ServerCfg, connection, identity,seq,timeProvider,scheduler,logFactory).DisposeItWith(Disposable);
        Ftp = new FtpServerEx(config.ServerExCfg, server, fileSystem, timeProvider, logFactory).DisposeItWith(Disposable);
    }

    public IFtpServerEx Ftp { get; }
}