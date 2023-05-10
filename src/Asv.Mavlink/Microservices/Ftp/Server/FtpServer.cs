using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class FtpServer : MavlinkMicroserviceServer, IFtpServer
{
    private readonly FtpConfig _cfg;

    public FtpServer(IMavlinkV2Connection connection, MavlinkServerIdentity identity, FtpConfig config,
        IPacketSequenceCalculator seq, IScheduler rxScheduler) : base("FTP", connection, identity, seq, rxScheduler)
    {
        _cfg = config;

        AnyRequest = InternalFilter<FileTransferProtocolPacket>(_ => _.Payload.TargetSystem,
                _ => _.Payload.TargetComponent)
            .Select(_ => new FtpMessagePayload(_.Payload.Payload))
            .Publish().RefCount();

        TerminateSessionRequest = AnyRequest.Where(_ => _.OpCodeId == OpCode.TerminateSession);

        ResetSessionsRequest = AnyRequest.Where(_ => _.OpCodeId == OpCode.ResetSessions);

        ListDirectoryRequest = AnyRequest.Where(_ => _.OpCodeId == OpCode.ListDirectory);

        OpenFileRORequest = AnyRequest.Where(_ => _.OpCodeId == OpCode.OpenFileRO);

        ReadFileRequest = AnyRequest.Where(_ => _.OpCodeId == OpCode.ReadFile);

        CreateFileRequest = AnyRequest.Where(_ => _.OpCodeId == OpCode.CreateFile);

        WriteFileRequest = AnyRequest.Where(_ => _.OpCodeId == OpCode.WriteFile);

        RemoveFileRequest = AnyRequest.Where(_ => _.OpCodeId == OpCode.RemoveFile);

        CreateDirectoryRequest = AnyRequest.Where(_ => _.OpCodeId == OpCode.CreateDirectory);

        RemoveDirectoryRequest = AnyRequest.Where(_ => _.OpCodeId == OpCode.RemoveDirectory);

        OpenFileWORequest = AnyRequest.Where(_ => _.OpCodeId == OpCode.OpenFileWO);

        TruncateFileRequest = AnyRequest.Where(_ => _.OpCodeId == OpCode.TruncateFile);

        RenameRequest = AnyRequest.Where(_ => _.OpCodeId == OpCode.Rename);

        CalcFileCRC32Request = AnyRequest.Where(_ => _.OpCodeId == OpCode.CalcFileCRC32);

        BurstReadFileRequest = AnyRequest.Where(_ => _.OpCodeId == OpCode.BurstReadFile);
    }

    public IObservable<FtpMessagePayload> AnyRequest { get; }

    public IObservable<FtpMessagePayload> ResetSessionsRequest { get; set; }

    public IObservable<FtpMessagePayload> TerminateSessionRequest { get; set; }

    public IObservable<FtpMessagePayload> ListDirectoryRequest { get; set; }

    public IObservable<FtpMessagePayload> OpenFileRORequest { get; set; }

    public IObservable<FtpMessagePayload> ReadFileRequest { get; set; }

    public IObservable<FtpMessagePayload> CreateFileRequest { get; set; }

    public IObservable<FtpMessagePayload> WriteFileRequest { get; set; }

    public IObservable<FtpMessagePayload> RemoveFileRequest { get; set; }

    public IObservable<FtpMessagePayload> CreateDirectoryRequest { get; set; }

    public IObservable<FtpMessagePayload> RemoveDirectoryRequest { get; set; }

    public IObservable<FtpMessagePayload> OpenFileWORequest { get; set; }

    public IObservable<FtpMessagePayload> TruncateFileRequest { get; set; }

    public IObservable<FtpMessagePayload> RenameRequest { get; set; }

    public IObservable<FtpMessagePayload> CalcFileCRC32Request { get; set; }

    public IObservable<FtpMessagePayload> BurstReadFileRequest { get; set; }
}