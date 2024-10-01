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

public class FtpServer : MavlinkMicroserviceServer, IFtpServer
{
    private readonly MavlinkFtpServerConfig _config;
    private readonly ILogger _logger;
    private ushort? _lastRoSequenceNumber;
    private ReadHandle _lastHandle;

    public FtpServer(
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

    private async void OnFtpMessage(FileTransferProtocolPacket input)
    {
        try
        {
            switch (input.ReadOpcode())
            {
                case FtpOpcode.None:
                    break;
                case FtpOpcode.TerminateSession:
                    InternalTerminateSession(input);
                    break;
                case FtpOpcode.ResetSessions:
                    InternalResetSessions(input);
                    break;
                case FtpOpcode.ListDirectory:
                    break;
                case FtpOpcode.OpenFileRO:
                    InternalOpenFileRo(input);
                    break;
                case FtpOpcode.ReadFile:
                    InternalFileRead(input);
                    break;
                case FtpOpcode.CreateDirectory:
                    InternalCreateDirectory(input);
                    break;
                case FtpOpcode.RemoveDirectory:
                    InternalRemoveDirectory(input);
                    break;
                case FtpOpcode.OpenFileWO:
                    break;
                case FtpOpcode.Rename:
                    InternalRename(input);
                    break;
                case FtpOpcode.RemoveFile:
                    InternalRemoveFile(input);
                    break;
                case FtpOpcode.CalcFileCRC32:
                    InternalCalcFileCrc32(input);
                    break;
                case FtpOpcode.TruncateFile:
                    InternalTruncateFile(input);
                    break;
                case FtpOpcode.CreateFile:
                    break;
                case FtpOpcode.WriteFile:
                    break;
                case FtpOpcode.BurstReadFile:
                    break;
                case FtpOpcode.Ack:
                case FtpOpcode.Nak:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch (FtpNackException e)
        {
            if (e.FsErrorCode != null)
            {
                await ReplyNackFailErrno(input, e.FsErrorCode.Value, e).ConfigureAwait(false);
            }
            else
            {
                await ReplyNack(input, e.NackError, e).ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            await ReplyNack(input, NackError.Fail, e).ConfigureAwait(false);
        }
    }

    #region CalcFileCrc32

    public CalcFileCrc32 CalcFileCrc32 { get; set; }

    private async void InternalCalcFileCrc32(FileTransferProtocolPacket input)
    {
        if (CalcFileCrc32 is null)
        {
            throw new FtpNackException(FtpOpcode.Nak, NackError.UnknownCommand);
        }

        var path = input.ReadDataAsString();
        _logger.ZLogInformation($"{LogRecv} Start calcualte CRC for: ({path})");
        var result = await CalcFileCrc32(path).ConfigureAwait(false);
        await InternalFtpReply(input, FtpOpcode.Ack, packet => packet.WriteDataAsInt(result)).ConfigureAwait(false);
    }

    #endregion

    #region Truncate File

    public TruncateFile? TruncateFile { get; set; }

    private async void InternalTruncateFile(FileTransferProtocolPacket input)
    {
        if (TruncateFile is null)
        {
            throw new FtpNackException(FtpOpcode.TruncateFile, NackError.UnknownCommand);
        }

        var path = input.ReadDataAsString();
        var offset = input.ReadOffset();
        _logger.ZLogInformation($"{LogRecv} Truncate file: ({path}) to size {offset}");
        await TruncateFile(new TruncateRequest(path, offset)).ConfigureAwait(false);
        await InternalFtpReply(input, FtpOpcode.Ack, p => p.WriteSize(0)).ConfigureAwait(false);
    }

    #endregion

    #region Remove Directory

    public RemoveDirectory? RemoveDirectory { get; set; }

    private async void InternalRemoveDirectory(FileTransferProtocolPacket input)
    {
        if (RemoveDirectory is null)
        {
            throw new FtpNackException(FtpOpcode.Nak, NackError.UnknownCommand);
        }

        var path = input.ReadDataAsString();
        await RemoveDirectory(path).ConfigureAwait(false);
        await InternalFtpReply(input, FtpOpcode.Ack, p => { p.WriteSize(0); }).ConfigureAwait(false);
    }

    #endregion

    #region RemoveFile

    public RemoveFile? RemoveFile { get; set; }

    private async void InternalRemoveFile(FileTransferProtocolPacket input)
    {
        if (RemoveFile is null)
        {
            throw new FtpNackException(FtpOpcode.RemoveFile, NackError.UnknownCommand);
        }

        var path = input.ReadDataAsString();
        await RemoveFile(path).ConfigureAwait(false);
        _logger.ZLogInformation($"{LogRecv} Removed file: ({path})");
        await InternalFtpReply(input, FtpOpcode.Ack, p => { p.WriteSize(0); }).ConfigureAwait(false);
    }

    #endregion

    #region ResetSessions

    public ResetSessionsDelegate? ResetSessions { get; set; }

    private async void InternalResetSessions(FileTransferProtocolPacket input)
    {
        if (ResetSessions is null)
        {
            throw new FtpNackException(FtpOpcode.ResetSessions, NackError.UnknownCommand);
        }

        await ResetSessions().ConfigureAwait(false);
        _logger.ZLogInformation($"{LogSend}Success to reset Sessions!)");
        await InternalFtpReply(input, FtpOpcode.Ack, p => { p.WriteSize(0); }).ConfigureAwait(false);
    }

    #endregion

    #region CreateDirectory

    public CreateDirectory? CreateDirectory { get; set; }

    private async void InternalCreateDirectory(FileTransferProtocolPacket input)
    {
        var session = input.ReadSession();
        var path = input.ReadDataAsString();
        var size = input.ReadSize();
        MavlinkFtpHelper.CheckFilePath(path);
        if (size > MavlinkFtpHelper.MaxDataSize)
        {
            throw new FtpNackException(FtpOpcode.CreateDirectory, NackError.InvalidDataSize);
        }

        _logger.ZLogInformation($"{LogRecv} Create directory path: ({path})");
        CreateDirectory = (directory, cancel) => Task.FromResult(new CreateHandle(session, directory));
        await CreateDirectory(path).ConfigureAwait(false);
        await InternalFtpReply(input, FtpOpcode.Ack, p => { p.WriteSize(0); }).ConfigureAwait(false);
    }

    #endregion

    #region TerminateSession

    public TerminateSessionDelegate? TerminateSession { get; set; }

    private async void InternalTerminateSession(FileTransferProtocolPacket input)
    {
        if (TerminateSession == null)
        {
            throw new FtpNackException(FtpOpcode.TerminateSession, NackError.UnknownCommand);
        }

        var session = input.ReadSession();
        _logger.ZLogInformation($"{LogRecv}TerminateSession(session={session})");
        await TerminateSession(session, DisposeCancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{LogSend}Success TerminateSession(session={session})");
    }

    #endregion

    #region FileRead

    public FileReadDelegate? FileRead { private get; set; }

    private async void InternalFileRead(FileTransferProtocolPacket input)
    {
        if (FileRead == null)
        {
            throw new FtpNackException(FtpOpcode.ReadFile, NackError.UnknownCommand);
        }

        var size = input.ReadSize();
        var session = input.ReadSession();
        var offset = input.ReadOffset();
        if (size > MavlinkFtpHelper.MaxDataSize)
        {
            throw new FtpNackException(FtpOpcode.ReadFile, NackError.InvalidDataSize);
        }

        _logger.ZLogTrace($"{LogRecv}ReadFile(session={session}, offset={offset}, size={size})");
        using var buffer = MemoryPool<byte>.Shared.Rent(size);
        var result = await FileRead(new ReadRequest(session, offset, size), buffer.Memory, DisposeCancel)
            .ConfigureAwait(false);
        _logger.ZLogTrace(
            $"{LogSend}Success ReadFile(session={session}, offset={offset}, size={size}): readCount={result.ReadCount}");
        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSession(session);
            p.WriteSize(result.ReadCount);
            p.WriteOffset(offset);
            // ReSharper disable once AccessToDisposedClosure
            p.WriteData(buffer.Memory.Span[..result.ReadCount]);
        }).ConfigureAwait(false);
    }

    #endregion

    #region OpenFileRead
    public OpenFileReadDelegate? OpenFileRead { private get; set; }

    private async void InternalOpenFileRo(FileTransferProtocolPacket input)
    {
        if (OpenFileRead == null)
        {
            throw new FtpNackException(FtpOpcode.OpenFileRO, NackError.UnknownCommand);
        }

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
            _lastHandle = await OpenFileRead(path, DisposeCancel).ConfigureAwait(false);
            _logger.ZLogInformation(
                $"{LogSend}Success OpenFileRead({path}): session={_lastHandle.Session}, size={_lastHandle.Size}");
            _lastRoSequenceNumber = sequenceNumber;
        }

        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSession(_lastHandle.Session);
            p.WriteDataAsUint(_lastHandle.Size);
        }).ConfigureAwait(false);
    }

    #endregion

    #region RenameFile

    public RenameDelegate? Rename { get; set; }
    private async void InternalRename(FileTransferProtocolPacket input)
    {
        if (Rename is null)
        {
            throw new FtpNackException(FtpOpcode.Rename, NackError.UnknownCommand);
        }

        var path1 = input.ReadDataAsString();
        var path2 = input.ReadDataAsString();
        await Rename(path1, path2).ConfigureAwait(false);
        _logger.ZLogInformation($"{LogRecv} Rename: ({path1}) to ({path2})");
        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSize(0);
        }).ConfigureAwait(false);
    }

    #endregion

    #region ReplyNack

    private Task ReplyNack(FileTransferProtocolPacket req, NackError err, Exception? ex = null)
    {
        var originOpCode = req.ReadOriginOpCode();
        if (ex == null)
        {
            _logger.ZLogError($"Error to execute {originOpCode:G}: {MavlinkFtpHelper.GetErrorMessage(err)}");
        }
        else
        {
            _logger.ZLogError(ex,
                $"Error to execute {originOpCode:G}: {MavlinkFtpHelper.GetErrorMessage(err)}. Exception: {ex.Message}");
        }

        return InternalFtpReply(req, FtpOpcode.Nak, x => x.WriteDataAsByte((byte)err));
    }

    private Task ReplyNackFailErrno(FileTransferProtocolPacket req, byte fsErrorCode, Exception? ex = null)
    {
        var originOpCode = req.ReadOriginOpCode();
        var originSession = req.ReadSession();
        if (ex == null)
        {
            _logger.ZLogError(
                $"Error to execute {originOpCode:G}: {MavlinkFtpHelper.GetErrorMessage(NackError.FailErrno)} with fsError:{fsErrorCode}");
        }
        else
        {
            _logger.ZLogError(
                $"Error to execute {originOpCode:G}: {MavlinkFtpHelper.GetErrorMessage(NackError.FailErrno)} with fsError:{fsErrorCode}. Exception: {ex.Message}");
        }

        return InternalFtpReply(req, FtpOpcode.Nak, x => x.WriteDataAsTwoByte((byte)NackError.FailErrno, fsErrorCode));
    }

    #endregion

    private Task InternalFtpReply(FileTransferProtocolPacket req, FtpOpcode replyOpCode,
        Action<FileTransferProtocolPacket> fillPacket)
    {
        return InternalSend<FileTransferProtocolPacket>(p =>
        {
            fillPacket(p);
            p.Payload.TargetComponent = req.ComponentId;
            p.Payload.TargetSystem = req.SystemId;
            p.Payload.TargetNetwork = req.Payload.TargetNetwork;
            var session = req.ReadSession();
            p.WriteSession(session);
            var originSeq = p.ReadSequenceNumber();
            p.WriteSequenceNumber((ushort)((originSeq + 1) % ushort.MaxValue));
            p.WriteOpcode(replyOpCode);
            var originOpCode = req.ReadOpcode();
            p.WriteOriginOpCode(originOpCode);
        }, cancel: DisposeCancel);
    }
}