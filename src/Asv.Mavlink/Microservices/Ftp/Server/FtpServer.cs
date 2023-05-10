using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class FtpServer : MavlinkMicroserviceServer, IFtpServer
{
    private readonly FtpConfig _cfg;
    
    public FtpServer(IMavlinkV2Connection connection, MavlinkServerIdentity identity, FtpConfig config,
        IPacketSequenceCalculator seq, IScheduler rxScheduler) : base("FTP", connection, identity, seq, rxScheduler)
    {
        _cfg = config;
        
        ResetSessionsRequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
        
        TerminateSessionRequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
        
        ListDirectoryRequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
        
        OpenFileRORequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
        
        ReadFileRequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
        
        CreateFileRequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
        
        WriteFileRequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
        
        RemoveFileRequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
        
        CreateDirectoryRequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
        
        RemoveDirectoryRequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
        
        OpenFileWORequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
        
        TruncateFileRequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
        
        RenameRequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
        
        CalcFileCRC32Request = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
        
        BurstReadFileRequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();
    }

    public IObservable<FtpMessagePayload> ResetSessionsRequest { get; }
    
    public IObservable<FtpMessagePayload> TerminateSessionRequest { get; }
    
    public IObservable<FtpMessagePayload> ListDirectoryRequest { get; }
    
    public IObservable<FtpMessagePayload> OpenFileRORequest { get; }
    
    public IObservable<FtpMessagePayload> ReadFileRequest { get; }
    
    public IObservable<FtpMessagePayload> CreateFileRequest { get; }
    
    public IObservable<FtpMessagePayload> WriteFileRequest { get; }
    
    public IObservable<FtpMessagePayload> RemoveFileRequest { get; }
    
    public IObservable<FtpMessagePayload> CreateDirectoryRequest { get; }
    
    public IObservable<FtpMessagePayload> RemoveDirectoryRequest { get; }
    
    public IObservable<FtpMessagePayload> OpenFileWORequest { get; }
    
    public IObservable<FtpMessagePayload> TruncateFileRequest { get; }
    
    public IObservable<FtpMessagePayload> RenameRequest { get; }
    
    public IObservable<FtpMessagePayload> CalcFileCRC32Request { get; }
    
    public IObservable<FtpMessagePayload> BurstReadFileRequest { get; }
}