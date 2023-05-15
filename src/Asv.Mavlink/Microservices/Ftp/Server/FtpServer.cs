using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            .Select(_ => (new DeviceIdentity()
                {
                    ComponentId = _.ComponentId,
                    SystemId = _.SystemId
                }, new FtpMessagePayload(_.Payload.Payload)))
            .Publish().RefCount();

        TerminateSessionRequest = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.TerminateSession);

        ResetSessionsRequest = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.ResetSessions);

        ListDirectoryRequest = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.ListDirectory);

        OpenFileRORequest = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.OpenFileRO);

        ReadFileRequest = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.ReadFile);

        CreateFileRequest = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.CreateFile);

        WriteFileRequest = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.WriteFile);

        RemoveFileRequest = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.RemoveFile);

        CreateDirectoryRequest = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.CreateDirectory);

        RemoveDirectoryRequest = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.RemoveDirectory);

        OpenFileWORequest = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.OpenFileWO);

        TruncateFileRequest = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.TruncateFile);

        RenameRequest = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.Rename);

        CalcFileCRC32Request = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.CalcFileCRC32);

        BurstReadFileRequest = AnyRequest.Where(_ => _.Item2.OpCodeId == OpCode.BurstReadFile);
    }

    public Task SendFtpPacket(FtpMessagePayload payload, DeviceIdentity identity, CancellationToken cancel)
    {
        
        return InternalSend<FileTransferProtocolPacket>(_ =>
        {
            _.Payload.TargetSystem = identity.SystemId;
            _.Payload.TargetComponent = identity.ComponentId;
            _.Payload.TargetNetwork = _cfg.TargetNetwork;
            var payloadSpan = new Span<byte>(_.Payload.Payload);
            payload.Serialize(ref payloadSpan);
        }, cancel);
    }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> AnyRequest { get; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> ResetSessionsRequest { get; set; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> TerminateSessionRequest { get; set; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> ListDirectoryRequest { get; set; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> OpenFileRORequest { get; set; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> ReadFileRequest { get; set; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> CreateFileRequest { get; set; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> WriteFileRequest { get; set; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> RemoveFileRequest { get; set; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> CreateDirectoryRequest { get; set; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> RemoveDirectoryRequest { get; set; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> OpenFileWORequest { get; set; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> TruncateFileRequest { get; set; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> RenameRequest { get; set; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> CalcFileCRC32Request { get; set; }

    public IObservable<(DeviceIdentity, FtpMessagePayload)> BurstReadFileRequest { get; set; }
}