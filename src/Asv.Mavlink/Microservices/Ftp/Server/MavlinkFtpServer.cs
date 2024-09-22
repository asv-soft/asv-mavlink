using System;
using System.Buffers;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;

public class MavlinkFtpServerConfig
{
    public byte NetworkId { get; set; } = 0;
    
}

public class MavlinkFtpServer : MavlinkMicroserviceServer, IMavlinkFtpServer
{
    private readonly MavlinkFtpServerConfig _config;
    private readonly ILogger _logger;
    private ushort? _lastRoSequenceNumber;
    private OpenReadResult _lastResult;

    public MavlinkFtpServer(
        MavlinkFtpServerConfig config,
        IMavlinkV2Connection connection, 
        MavlinkIdentity identity, 
        IPacketSequenceCalculator seq, 
        IScheduler? rxScheduler = null, 
        ILogger? logger = null) : base("FTP", connection, identity, seq, rxScheduler, logger)
    {
        _config = config;
        _logger = logger ?? NullLogger.Instance;
        connection
            .Filter<FileTransferProtocolPacket>()
            .Where(x => x.Payload.TargetComponent == identity.ComponentId &&
                        x.Payload.TargetSystem == identity.SystemId && _config.NetworkId == x.Payload.TargetNetwork)
            .Subscribe(OnFtpMessage)
            .DisposeItWith(Disposable);
    }

    private void OnFtpMessage(FileTransferProtocolPacket input)
    {
        switch (input.ReadOpcode())
        {
            case FtpOpcode.None:
                break;
            case FtpOpcode.TerminateSession:
                break;
            case FtpOpcode.ResetSessions:
                break;
            case FtpOpcode.ListDirectory:
                break;
            case FtpOpcode.OpenFileRO:
                InternalOpenFileRo(input);
                break;
            case FtpOpcode.ReadFile:
                InternalRead(input);
                break;
            case FtpOpcode.CreateFile:
                break;
            case FtpOpcode.WriteFile:
                break;
            case FtpOpcode.RemoveFile:
                break;
            case FtpOpcode.CreateDirectory:
                break;
            case FtpOpcode.RemoveDirectory:
                break;
            case FtpOpcode.OpenFileWO:
                break;
            case FtpOpcode.TruncateFile:
                break;
            case FtpOpcode.Rename:
                break;
            case FtpOpcode.CalcFileCRC32:
                break;
            case FtpOpcode.BurstReadFile:
                break;
            case FtpOpcode.Ack:
                break;
            case FtpOpcode.Nak:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public FileReadDelegate? FileRead {private get; set; }
    
    private async void InternalRead(FileTransferProtocolPacket input)
    {
        if (FileRead == null)
        {
            await ReplyNack(input,NackError.Fail).ConfigureAwait(false);
            return;
        }
        var size = input.ReadSize();
        var session = input.ReadSession();
        var offset = input.ReadOffset();
        if (size > MavlinkFtpHelper.MaxDataSize)
        {
            await ReplyNack(input,NackError.InvalidDataSize).ConfigureAwait(false);
            return;
        }
        try
        {
            
            _logger.ZLogTrace($"{LogRecv}ReadFile(session={session}, offset={offset}, size={size})");
            using var buffer = MemoryPool<byte>.Shared.Rent(size);
            var result = await FileRead(new ReadRequest(session, offset, size),buffer.Memory, DisposeCancel).ConfigureAwait(false);
            _logger.ZLogTrace($"{LogSend}Success ReadFile(session={session}, offset={offset}, size={size}): readCount={result.ReadCount}");
            await InternalFtpReply(input,FtpOpcode.Ack, p =>
            {
                p.WriteSession(session);
                p.WriteSize(result.ReadCount);
                p.WriteOffset(offset);
                // ReSharper disable once AccessToDisposedClosure
                p.WriteData(buffer.Memory.Span[..result.ReadCount]);
            }).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            await ReplyNack(input,NackError.Fail,e).ConfigureAwait(false);
        }
    }
    
    public OpenFileReadDelegate? OpenFileRead { private get; set; }
    
    private async void InternalOpenFileRo(FileTransferProtocolPacket input)
    {
        if (OpenFileRead == null)
        {
            await ReplyNack(input,NackError.Fail).ConfigureAwait(false);
            return;
        }
            
        try
        {
            var path = input.ReadDataAsString();
            MavlinkFtpHelper.CheckFilePath(path);
            var sequenceNumber = input.ReadSequenceNumber();
            if (_lastRoSequenceNumber == sequenceNumber)
            {
                // If the drone (client) receives a message with the same sequence number then it assumes that its ACK/NAK response was lost.
                // In this case it should resend the response (the sequence number is not iterated, because it is as though the previous response was not sent). 
                _logger.ZLogWarning($"{LogRecv}Duplicate OpenFileRead({path})");
            }
            else
            {
                _logger.ZLogInformation($"{LogRecv}OpenFileRead({path})");
                _lastResult = await OpenFileRead(path, DisposeCancel).ConfigureAwait(false);
                _logger.ZLogInformation($"{LogSend}Success OpenFileRead({path}): session={_lastResult.Session}, size={_lastResult.Size}");
                _lastRoSequenceNumber = sequenceNumber;
            }
          
            await InternalFtpReply(input,FtpOpcode.Ack, p =>
            {
                p.WriteSession(_lastResult.Session);
                p.WriteDataAsUint(_lastResult.Size);
            }).ConfigureAwait(false);

        }
        catch (Exception e)
        {
            await ReplyNack(input,NackError.Fail,e).ConfigureAwait(false);
        }
        
    }

    
    private Task ReplyNack(FileTransferProtocolPacket req, NackError err, Exception? ex = null)
    {
        var originOpCode = req.ReadOriginOpCode();
        if (ex == null)
        {
            _logger.ZLogError($"Error to execute {originOpCode:G}: {MavlinkFtpHelper.GetErrorMessage(err)}");    
        }
        else
        {
            _logger.ZLogError(ex,$"Error to execute {originOpCode:G}: {MavlinkFtpHelper.GetErrorMessage(err)}. Exception: {ex.Message}");
        }
        
        return InternalFtpReply(req,FtpOpcode.Nak,x => x.WriteDataAsByte((byte)err));
    }
    private Task ReplyNackFailErrno(FileTransferProtocolPacket req,FtpOpcode originOpCode, byte fsErrorCode, Exception? ex = null)
    {
        if (ex == null)
        {
            _logger.ZLogError($"Error to execute {originOpCode:G}: {MavlinkFtpHelper.GetErrorMessage(NackError.FailErrno)} with fsError:{fsErrorCode}");    
        }
        else
        {
            _logger.ZLogError($"Error to execute {originOpCode:G}: {MavlinkFtpHelper.GetErrorMessage(NackError.FailErrno)} with fsError:{fsErrorCode}. Exception: {ex.Message}");
        }
        
        return InternalFtpReply(req,FtpOpcode.Nak,x => x.WriteDataAsTwoByte((byte)NackError.FailErrno,fsErrorCode));
    }

    


    private Task InternalFtpReply(FileTransferProtocolPacket req, FtpOpcode replyOpCode,Action<FileTransferProtocolPacket> fillPacket)
    {
        return InternalSend<FileTransferProtocolPacket>(p =>
        {
            fillPacket(p);
            p.Payload.TargetComponent = req.ComponentId;
            p.Payload.TargetSystem = req.SystemId;
            p.Payload.TargetNetwork = req.Payload.TargetNetwork;
            var originSeq = p.ReadSequenceNumber();
            p.WriteSequenceNumber( (ushort)((originSeq + 1)% ushort.MaxValue));
            p.WriteOpcode(replyOpCode);
            var originOpCode = req.ReadOpcode();
            p.WriteOriginOpCode(originOpCode);
        }, cancel: DisposeCancel);
    }
    
}