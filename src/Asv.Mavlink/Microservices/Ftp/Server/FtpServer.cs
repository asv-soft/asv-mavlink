using System;
using System.Buffers;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public class MavlinkFtpServerConfig
{
    public byte NetworkId { get; set; } = 0;
    public int BurstReadChunkDelayMs { get; set; } = 30;
}

public sealed class FtpServer : MavlinkMicroserviceServer, IFtpServer
{
    private readonly MavlinkFtpServerConfig _config;
    private readonly ILogger _logger;
    private ushort? _lastRoSequenceNumber;
    private ushort? _lastWoSequenceNumber;
    private ReadHandle _lastReadHandle;
    private WriteHandle _lastWriteHandle;
    private readonly IDisposable _filter;

    public FtpServer(MavlinkIdentity identity, MavlinkFtpServerConfig config, ICoreServices core) 
        : base("FTP", identity, core)
    {
        _config = config;
        _logger = core.Log.CreateLogger<FtpServer>();
        _filter = core.Connection
            .Filter<FileTransferProtocolPacket>()
            .Where(x => x.Payload.TargetComponent == identity.ComponentId &&
                        x.Payload.TargetSystem == identity.SystemId && _config.NetworkId == x.Payload.TargetNetwork)
            .Subscribe(OnFtpMessage);
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
                    await InternalTerminateSession(input).ConfigureAwait(false);
                    break;
                case FtpOpcode.ResetSessions:
                    await InternalResetSessions(input).ConfigureAwait(false);
                    break;
                case FtpOpcode.ListDirectory:
                    await InternalListDirectory(input).ConfigureAwait(false);
                    break;
                case FtpOpcode.OpenFileRO:
                    await InternalOpenFileRo(input).ConfigureAwait(false);
                    break;
                case FtpOpcode.ReadFile:
                    await InternalFileRead(input).ConfigureAwait(false);
                    break;
                case FtpOpcode.CreateDirectory:
                    await InternalCreateDirectory(input).ConfigureAwait(false);
                    break;
                case FtpOpcode.RemoveDirectory:
                    await InternalRemoveDirectory(input).ConfigureAwait(false);
                    break;
                case FtpOpcode.OpenFileWO:
                    await InternalOpenFileWrite(input).ConfigureAwait(false);
                    break;
                case FtpOpcode.Rename:
                    await InternalRename(input).ConfigureAwait(false);
                    break;
                case FtpOpcode.RemoveFile:
                    await InternalRemoveFile(input).ConfigureAwait(false);
                    break;
                case FtpOpcode.CalcFileCRC32:
                    await InternalCalcFileCrc32(input).ConfigureAwait(false);
                    break;
                case FtpOpcode.TruncateFile:
                    await InternalTruncateFile(input).ConfigureAwait(false);
                    break;
                case FtpOpcode.CreateFile:
                    await InternalCreateFile(input).ConfigureAwait(false);
                    break;
                case FtpOpcode.WriteFile:
                    await InternalWriteFile(input).ConfigureAwait(false);
                    break;
                case FtpOpcode.BurstReadFile:
                    await InternalBurstReadFile(input).ConfigureAwait(false);
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

    public CalcFileCrc32? CalcFileCrc32 { private get; set; }

    private async Task InternalCalcFileCrc32(FileTransferProtocolPacket input)
    {
        if (CalcFileCrc32 is null)
        {
            throw new FtpNackException(FtpOpcode.Nak, NackError.UnknownCommand);
        }

        var path = input.ReadDataAsString();
        _logger.ZLogInformation($"{LogRecv} Start calculate CRC32 for: ({path})");
        var result = await CalcFileCrc32(path, DisposeCancel).ConfigureAwait(false);
        await InternalFtpReply(input, FtpOpcode.Ack, packet => packet.WriteDataAsUint(result)).ConfigureAwait(false);
    }

    #endregion

    #region Truncate File

    public TruncateFile? TruncateFile { private get; set; }

    private async Task InternalTruncateFile(FileTransferProtocolPacket input)
    {
        if (TruncateFile is null)
        {
            throw new FtpNackException(FtpOpcode.TruncateFile, NackError.UnknownCommand);
        }

        var path = input.ReadDataAsString();
        var offset = input.ReadOffset();
        _logger.ZLogInformation($"{LogRecv} Truncate file: ({path}) to size {offset}");
        await TruncateFile(new TruncateRequest(path, offset), DisposeCancel).ConfigureAwait(false);
        await InternalFtpReply(input, FtpOpcode.Ack, p => p.WriteSize(0)).ConfigureAwait(false);
    }

    #endregion

    #region Remove Directory

    public RemoveDirectory? RemoveDirectory { private get; set; }

    private async Task InternalRemoveDirectory(FileTransferProtocolPacket input)
    {
        if (RemoveDirectory is null)
        {
            throw new FtpNackException(FtpOpcode.Nak, NackError.UnknownCommand);
        }

        var path = input.ReadDataAsString();
        await RemoveDirectory(path, DisposeCancel).ConfigureAwait(false);
        await InternalFtpReply(input, FtpOpcode.Ack, p => { p.WriteSize(0); }).ConfigureAwait(false);
    }

    #endregion

    #region RemoveFile
    
    public RemoveFile? RemoveFile { get; set; }

    private async Task InternalRemoveFile(FileTransferProtocolPacket input)
    {
        if (RemoveFile is null)
        {
            throw new FtpNackException(FtpOpcode.RemoveFile, NackError.UnknownCommand);
        }

        var path = input.ReadDataAsString();
        await RemoveFile(path, DisposeCancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{LogRecv} Removed file: ({path})");
        await InternalFtpReply(input, FtpOpcode.Ack, p => { p.WriteSize(0); }).ConfigureAwait(false);
    }

    #endregion

    #region ResetSessions
    
    public ResetSessionsDelegate? ResetSessions { private get; set; }

    private async Task InternalResetSessions(FileTransferProtocolPacket input)
    {
        if (ResetSessions is null)
        {
            throw new FtpNackException(FtpOpcode.ResetSessions, NackError.UnknownCommand);
        }

        await ResetSessions(DisposeCancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{LogSend}Success to reset Sessions!)");
        await InternalFtpReply(input, FtpOpcode.Ack, p => { p.WriteSize(0); }).ConfigureAwait(false);
    }

    #endregion

    #region ListDirectory

    public ListDirectoryDelegate? ListDirectory { private get; set; }

    private async Task InternalListDirectory(FileTransferProtocolPacket input)
    {
        if (ListDirectory is null)
        {
            throw new FtpNackException(FtpOpcode.ListDirectory, NackError.UnknownCommand);
        }
        
        var path = input.ReadDataAsString();
        var offset = input.ReadOffset();
        _logger.ZLogInformation($"{LogRecv}ListDirectory path: ({path}), offset: ({offset})");
        using var buffer = MemoryPool<char>.Shared.Rent(MavlinkFtpHelper.MaxDataSize);
        MavlinkFtpHelper.CheckFolderPath(path);
        var result = await ListDirectory(path, offset, buffer.Memory, DisposeCancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{LogRecv}Success ListDirectory path: ({path}), offset: ({offset}): readCount={result}");
        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            // ReSharper disable once AccessToDisposedClosure
            p.WriteDataAsString(buffer.Memory.Span[..result]);
        }).ConfigureAwait(false);
    }

    #endregion

    #region CreateDirectory

    public CreateDirectory? CreateDirectory { private get; set; }

    private async Task InternalCreateDirectory(FileTransferProtocolPacket input)
    {
        if (CreateDirectory is null)
        {
            throw new FtpNackException(FtpOpcode.CreateDirectory, NackError.UnknownCommand);
        };
        
        var path = input.ReadDataAsString();
        MavlinkFtpHelper.CheckFilePath(path);

        _logger.ZLogInformation($"{LogRecv} Create directory path: ({path})");
        await CreateDirectory(path, DisposeCancel).ConfigureAwait(false);
        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSize(0);
        }).ConfigureAwait(false);
    }

    #endregion

    #region CreateFile

    public CreateFile? CreateFile { private get; set; }

    private async Task InternalCreateFile(FileTransferProtocolPacket input)
    {
        if (CreateFile is null)
        {
            throw new FtpNackException(FtpOpcode.CreateFile, NackError.UnknownCommand);
        }
        
        var path = input.ReadDataAsString();
        MavlinkFtpHelper.CheckFilePath(path);

        _logger.ZLogInformation($"{LogRecv}CreateFile file path: ({path})");
        var result = await CreateFile(path, DisposeCancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{LogRecv}Success CreateFile file path: ({path})");
        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSession(result);
        }).ConfigureAwait(false);
    }

    #endregion

    #region TerminateSession

    public TerminateSessionDelegate? TerminateSession { private get; set; }

    private async Task InternalTerminateSession(FileTransferProtocolPacket input)
    {
        if (TerminateSession == null)
        {
            throw new FtpNackException(FtpOpcode.TerminateSession, NackError.UnknownCommand);
        }

        var session = input.ReadSession();
        _logger.ZLogInformation($"{LogRecv}TerminateSession(session={session})");
        await TerminateSession(session, DisposeCancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{LogSend}Success TerminateSession(session={session})");
        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSize(0);
        }).ConfigureAwait(false);
    }

    #endregion

    #region FileRead
    
    public FileReadDelegate? FileRead { private get; set; }

    private async Task InternalFileRead(FileTransferProtocolPacket input)
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

    private async Task InternalOpenFileRo(FileTransferProtocolPacket input)
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
            _lastReadHandle = await OpenFileRead(path, DisposeCancel).ConfigureAwait(false);
            _logger.ZLogInformation(
                $"{LogSend}Success OpenFileRead({path}): session={_lastReadHandle.Session}, size={_lastReadHandle.Size}");
            _lastRoSequenceNumber = sequenceNumber;
        }

        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSession(_lastReadHandle.Session);
            p.WriteDataAsUint(_lastReadHandle.Size);
        }).ConfigureAwait(false);
    }

    #endregion

    #region OpenFileWrite
    
    public OpenFileWriteDelegate? OpenFileWrite { get; set; }

    private async Task InternalOpenFileWrite(FileTransferProtocolPacket input)
    {
        if (OpenFileWrite == null)
        {
            throw new FtpNackException(FtpOpcode.OpenFileWO, NackError.UnknownCommand);
        }

        var path = input.ReadDataAsString();
        MavlinkFtpHelper.CheckFilePath(path);
        var sequenceNumber = input.ReadSequenceNumber();
        if (_lastWoSequenceNumber == sequenceNumber)
        {
            // If the drone (client) receives a message with the same sequence number then it assumes that its ACK/NAK response was lost.
            // In this case it should resend the response (the sequence number is not iterated, because it is as though the previous response was not sent). 
            _logger.ZLogWarning($"{LogRecv}Duplicate OpenFileWrite({path})");
        }
        else
        {
            _logger.ZLogInformation($"{LogRecv}OpenFileWrite({path})");
            _lastWriteHandle = await OpenFileWrite(path, DisposeCancel).ConfigureAwait(false);
            _logger.ZLogInformation(
                $"{LogSend}Success OpenFileWrite({path}): session={_lastWriteHandle.Session}, size={_lastWriteHandle.Size}");
            _lastWoSequenceNumber = sequenceNumber;
        }

        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSession(_lastWriteHandle.Session);
            p.WriteDataAsUint(_lastWriteHandle.Size);
        }).ConfigureAwait(false);
    }

    #endregion

    #region RenameFile

    public RenameDelegate? Rename { private get; set; }
    private async Task InternalRename(FileTransferProtocolPacket input)
    {
        if (Rename is null)
        {
            throw new FtpNackException(FtpOpcode.Rename, NackError.UnknownCommand);
        }

        var path1 = input.ReadDataAsString();
        var path2 = input.ReadDataAsString();
        await Rename(path1, path2, DisposeCancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{LogRecv} Rename: ({path1}) to ({path2})");
        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSize(0);
        }).ConfigureAwait(false);
    }

    #endregion

    #region BurstReadFile

    public BurstReadFileDelegate? BurstReadFile { private get; set; }

    private async Task InternalBurstReadFile(FileTransferProtocolPacket input)
    {
        if (BurstReadFile is null)
        {
            throw new FtpNackException(FtpOpcode.BurstReadFile, NackError.UnknownCommand);
        }
        
        var session = input.ReadSession();
        var offset = input.ReadOffset();
        var size = input.ReadSize();
        if (size > MavlinkFtpHelper.MaxDataSize)
        {
            throw new FtpNackException(FtpOpcode.BurstReadFile, NackError.InvalidDataSize);
        }
        _logger.ZLogTrace($"{LogRecv}BurstReadFile(session={session}, offset={offset}, size={size})");
        var isOver = false;
        while (!isOver)
        {
            using var buffer = MemoryPool<byte>.Shared.Rent(size);
            var result = await BurstReadFile(new ReadRequest(session, offset, size), buffer.Memory, DisposeCancel).ConfigureAwait(false);
            _logger.ZLogTrace(
                $"{LogSend}Success BurstReadFile(session={session}, offset={offset}, size={size}): readCount={result.ReadCount}, isLastChunk = {result.IsLastChunk}");
            var burstComplete = result.IsLastChunk ? (byte) 1 : (byte) 0;
            var currentOffset = offset;
            await InternalFtpReply(input, FtpOpcode.Ack, p =>
            {
                p.WriteSession(session);
                p.WriteBurstComplete(burstComplete);
                p.WriteOffset(currentOffset);
                p.WriteSize(result.ReadCount);
                // ReSharper disable once AccessToDisposedClosure
                p.WriteData(buffer.Memory.Span[..result.ReadCount]);
            }).ConfigureAwait(false);
            isOver = result.IsLastChunk;
            offset += result.ReadCount;
            await Task.Delay(_config.BurstReadChunkDelayMs).ConfigureAwait(false);
        }
    }

    #endregion

    #region WriteFile

    public WriteFile? WriteFile { private get; set; }

    private async Task InternalWriteFile(FileTransferProtocolPacket input)
    {
        if (WriteFile is null)
        {
            throw new FtpNackException(FtpOpcode.WriteFile, NackError.UnknownCommand);
        }

        var size = input.ReadSize();

        if (size > MavlinkFtpHelper.MaxDataSize)
        {
            throw new FtpNackException(FtpOpcode.WriteFile, NackError.InvalidDataSize);
        }
        
        var session = input.ReadSession();
        var offset = input.ReadOffset();
        
        using var buffer = MemoryPool<byte>.Shared.Rent(size);
        input.ReadData(buffer.Memory);
        
        _logger.ZLogTrace($"{LogRecv}WriteFile(session={session}, offset={offset}, size={size})");
        await WriteFile(new WriteRequest(session, offset, size), buffer.Memory, DisposeCancel)
            .ConfigureAwait(false);
        _logger.ZLogTrace(
            $"{LogSend}Success WriteFile(session={session}, offset={offset}, size={size})");
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

    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _filter.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        if (_filter is IAsyncDisposable filterAsyncDisposable)
            await filterAsyncDisposable.DisposeAsync().ConfigureAwait(false);
        else
            _filter.Dispose();

        await base.DisposeAsyncCore().ConfigureAwait(false);
    }

    #endregion

}