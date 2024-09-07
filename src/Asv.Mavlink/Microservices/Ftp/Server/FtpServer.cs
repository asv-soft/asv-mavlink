using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class FtpServer : MavlinkMicroserviceServer, IFtpServer
{
    private readonly FtpConfig _cfg;

    public FtpServer(IMavlinkV2Connection connection, MavlinkIdentity identity, FtpConfig config,
        IPacketSequenceCalculator seq, 
        IScheduler? rxScheduler = null
        ,ILogger? logger = null) : base("FTP", connection, identity, seq, rxScheduler,logger)
    {
        _cfg = config;
        
        AnyRequest = InternalFilter<FileTransferProtocolPacket>(p => p.Payload.TargetSystem,
                p => p.Payload.TargetComponent)
            .Select(p => (new DeviceIdentity()
                {
                    ComponentId = p.ComponentId,
                    SystemId = p.SystemId
                }, new FtpMessagePayload(p.Payload.Payload)))
            .Publish().RefCount();

        TerminateSessionRequest = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.TerminateSession);

        ResetSessionsRequest = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.ResetSessions);

        ListDirectoryRequest = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.ListDirectory);

        OpenFileRORequest = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.OpenFileRO);

        ReadFileRequest = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.ReadFile);

        CreateFileRequest = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.CreateFile);

        WriteFileRequest = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.WriteFile);

        RemoveFileRequest = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.RemoveFile);

        CreateDirectoryRequest = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.CreateDirectory);

        RemoveDirectoryRequest = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.RemoveDirectory);

        OpenFileWORequest = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.OpenFileWO);

        TruncateFileRequest = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.TruncateFile);

        RenameRequest = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.Rename);

        CalcFileCRC32Request = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.CalcFileCRC32);

        BurstReadFileRequest = AnyRequest.Where(t => t.Item2.OpCodeId == OpCode.BurstReadFile);
    }

    public Task SendFtpPacket(FtpMessagePayload payload, DeviceIdentity identity, CancellationToken cancel)
    {
        
        return InternalSend<FileTransferProtocolPacket>(p =>
        {
            p.Payload.TargetSystem = identity.SystemId;
            p.Payload.TargetComponent = identity.ComponentId;
            p.Payload.TargetNetwork = _cfg.TargetNetwork;
            var payloadSpan = new Span<byte>(p.Payload.Payload);
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