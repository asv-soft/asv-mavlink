using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;

using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink;

/// <summary>
/// Configuration for the MAVLink FTP server.
/// </summary>
public class MavlinkFtpServerConfig
{
    /// <summary>
    /// Gets or sets the target network ID.
    /// </summary>
    public byte NetworkId { get; set; } = 0;
    
    /// <summary>
    /// Gets or sets the delay in milliseconds between burst read chunks.
    /// </summary>
    public int BurstReadChunkDelayMs { get; set; } = 30;
}

/// <inheritdoc cref="IFtpServer"/>
public sealed class FtpServer : MavlinkMicroserviceServer, IFtpServer
{
    private readonly MavlinkFtpServerConfig _config;
    private readonly ILogger _logger;
    private ushort? _lastRoSequenceNumber;
    private ushort? _lastWoSequenceNumber;
    private ReadHandle _lastReadHandle;
    private WriteHandle _lastWriteHandle;
    private readonly IDisposable _filter;

    public FtpServer(MavlinkIdentity identity, MavlinkFtpServerConfig config, IMavlinkContext core) 
        : base(MavlinkFtpHelper.FtpMicroserviceName, identity, core)
    {
        _config = config;
        _logger = core.LoggerFactory.CreateLogger<FtpServer>();
        _filter = core.Connection
            .RxFilterByType<FileTransferProtocolPacket>()
            .Where(x => 
                x.Payload.TargetComponent == identity.ComponentId 
                && x.Payload.TargetSystem == identity.SystemId 
                && _config.NetworkId == x.Payload.TargetNetwork
            )
            .SubscribeAwait(OnFtpMessage);
    }

    private async ValueTask OnFtpMessage(FileTransferProtocolPacket input, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            _logger.ZLogWarning($"FTP message cancellation requested.");
            return;
        }
        try
        {
            switch (input.ReadOpcode())
            {
                case FtpOpcode.None:
                    break;
                case FtpOpcode.TerminateSession:
                    await InternalTerminateSession(input, cancellationToken).ConfigureAwait(false);
                    break;
                case FtpOpcode.ResetSessions:
                    await InternalResetSessions(input, cancellationToken).ConfigureAwait(false);
                    break;
                case FtpOpcode.ListDirectory:
                    await InternalListDirectory(input, cancellationToken).ConfigureAwait(false);
                    break;
                case FtpOpcode.OpenFileRO:
                    await InternalOpenFileRo(input, cancellationToken).ConfigureAwait(false);
                    break;
                case FtpOpcode.ReadFile:
                    await InternalFileRead(input, cancellationToken).ConfigureAwait(false);
                    break;
                case FtpOpcode.CreateDirectory:
                    await InternalCreateDirectory(input, cancellationToken).ConfigureAwait(false);
                    break;
                case FtpOpcode.RemoveDirectory:
                    await InternalRemoveDirectory(input, cancellationToken).ConfigureAwait(false);
                    break;
                case FtpOpcode.OpenFileWO:
                    await InternalOpenFileWrite(input, cancellationToken).ConfigureAwait(false);
                    break;
                case FtpOpcode.Rename:
                    await InternalRename(input, cancellationToken).ConfigureAwait(false);
                    break;
                case FtpOpcode.RemoveFile:
                    await InternalRemoveFile(input, cancellationToken).ConfigureAwait(false);
                    break;
                case FtpOpcode.CalcFileCRC32:
                    await InternalCalcFileCrc32(input, cancellationToken).ConfigureAwait(false);
                    break;
                case FtpOpcode.TruncateFile:
                    await InternalTruncateFile(input, cancellationToken).ConfigureAwait(false);
                    break;
                case FtpOpcode.CreateFile:
                    await InternalCreateFile(input, cancellationToken).ConfigureAwait(false);
                    break;
                case FtpOpcode.WriteFile:
                    await InternalWriteFile(input, cancellationToken).ConfigureAwait(false);
                    break;
                case FtpOpcode.BurstReadFile:
                    await InternalBurstReadFile(input, cancellationToken).ConfigureAwait(false);
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
                await ReplyNackFailErrno(input, e.FsErrorCode.Value, e, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await ReplyNack(input, e.NackError, e, cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            await ReplyNack(input, NackError.Fail, e, cancellationToken).ConfigureAwait(false);
        }
    }

    #region CalcFileCrc32

    public CalcFileCrc32? CalcFileCrc32 { private get; set; }

    private async Task InternalCalcFileCrc32(FileTransferProtocolPacket input, CancellationToken cancel = default)
    {
        if (CalcFileCrc32 is null)
        {
            throw new FtpNackException(FtpOpcode.Nak, NackError.UnknownCommand);
        }

        var path = input.ReadDataAsString();
        _logger.ZLogInformation($"{Id} Start calculate CRC32 for: ({path})");
        var result = await CalcFileCrc32(path, cancel).ConfigureAwait(false);
        await InternalFtpReply(
            input, 
            FtpOpcode.Ack, 
            packet => packet.WriteDataAsUint(result), 
            cancel
        ).ConfigureAwait(false);
    }

    #endregion

    #region Truncate File

    public TruncateFile? TruncateFile { private get; set; }

    private async Task InternalTruncateFile(FileTransferProtocolPacket input, CancellationToken cancel = default)
    {
        if (TruncateFile is null)
        {
            throw new FtpNackException(FtpOpcode.TruncateFile, NackError.UnknownCommand);
        }

        var path = input.ReadDataAsString();
        var offset = input.ReadOffset();
        _logger.ZLogInformation($"{Id} Truncate file: ({path}) to size {offset}");
        await TruncateFile(new TruncateRequest(path, offset), cancel).ConfigureAwait(false);
        await InternalFtpReply(
            input, 
            FtpOpcode.Ack, 
            p => p.WriteSize(0), 
            cancel
        ).ConfigureAwait(false);
    }

    #endregion

    #region Remove Directory

    public RemoveDirectory? RemoveDirectory { private get; set; }

    private async Task InternalRemoveDirectory(FileTransferProtocolPacket input, CancellationToken cancel = default)
    {
        if (RemoveDirectory is null)
        {
            throw new FtpNackException(FtpOpcode.Nak, NackError.UnknownCommand);
        }

        var path = input.ReadDataAsString();
        await RemoveDirectory(path, cancel).ConfigureAwait(false);
        await InternalFtpReply(
            input, 
            FtpOpcode.Ack, 
            p => { p.WriteSize(0); }, 
            cancel
        ).ConfigureAwait(false);
    }

    #endregion

    #region RemoveFile
    
    public RemoveFile? RemoveFile { get; set; }

    private async Task InternalRemoveFile(FileTransferProtocolPacket input, CancellationToken cancel = default)
    {
        if (RemoveFile is null)
        {
            throw new FtpNackException(FtpOpcode.RemoveFile, NackError.UnknownCommand);
        }

        var path = input.ReadDataAsString();
        await RemoveFile(path, cancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{Id} Removed file: ({path})");
        await InternalFtpReply(
            input, 
            FtpOpcode.Ack, 
            p => { p.WriteSize(0); }, 
            cancel
        ).ConfigureAwait(false);
    }

    #endregion

    #region ResetSessions
    
    public ResetSessionsDelegate? ResetSessions { private get; set; }

    private async Task InternalResetSessions(FileTransferProtocolPacket input, CancellationToken cancel = default)
    {
        if (ResetSessions is null)
        {
            throw new FtpNackException(FtpOpcode.ResetSessions, NackError.UnknownCommand);
        }

        await ResetSessions(cancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{Id}Success to reset Sessions!)");
        await InternalFtpReply(
            input, 
            FtpOpcode.Ack, 
            p => { p.WriteSize(0); }, 
            cancel
        ).ConfigureAwait(false);
    }

    #endregion

    #region ListDirectory

    public ListDirectoryDelegate? ListDirectory { private get; set; }

    private async Task InternalListDirectory(FileTransferProtocolPacket input, CancellationToken cancel = default)
    {
        if (ListDirectory is null)
        {
            throw new FtpNackException(FtpOpcode.ListDirectory, NackError.UnknownCommand);
        }
        
        var path = input.ReadDataAsString();
        var offset = input.ReadOffset();
        _logger.ZLogInformation($"{Id}ListDirectory path: ({path}), offset: ({offset})");
        using var buffer = MemoryPool<char>.Shared.Rent(MavlinkFtpHelper.MaxDataSize);
        MavlinkFtpHelper.CheckFolderPath(path);
        var result = await ListDirectory(path, offset, buffer.Memory, cancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{Id}Success ListDirectory path: ({path}), offset: ({offset}): readCount={result}");
        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            // ReSharper disable once AccessToDisposedClosure
            p.WriteDataAsString(buffer.Memory.Span[..result]);
        }, cancel).ConfigureAwait(false);
    }

    #endregion

    #region CreateDirectory

    public CreateDirectory? CreateDirectory { private get; set; }

    private async Task InternalCreateDirectory(FileTransferProtocolPacket input, CancellationToken cancel = default)
    {
        if (CreateDirectory is null)
        {
            throw new FtpNackException(FtpOpcode.CreateDirectory, NackError.UnknownCommand);
        };
        
        var path = input.ReadDataAsString();
        MavlinkFtpHelper.CheckFilePath(path);

        _logger.ZLogInformation($"{Id} Create directory path: ({path})");
        await CreateDirectory(path, cancel).ConfigureAwait(false);
        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSize(0);
        }, cancel).ConfigureAwait(false);
    }

    #endregion

    #region CreateFile

    public CreateFile? CreateFile { private get; set; }

    private async Task InternalCreateFile(FileTransferProtocolPacket input, CancellationToken cancel = default)
    {
        if (CreateFile is null)
        {
            throw new FtpNackException(FtpOpcode.CreateFile, NackError.UnknownCommand);
        }
        
        var path = input.ReadDataAsString();
        MavlinkFtpHelper.CheckFilePath(path);

        _logger.ZLogInformation($"{Id}CreateFile file path: ({path})");
        var result = await CreateFile(path, cancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{Id}Success CreateFile file path: ({path})");
        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSession(result);
        }, cancel).ConfigureAwait(false);
    }

    #endregion

    #region TerminateSession

    public TerminateSessionDelegate? TerminateSession { private get; set; }

    private async Task InternalTerminateSession(FileTransferProtocolPacket input, CancellationToken cancel = default)
    {
        if (TerminateSession == null)
        {
            throw new FtpNackException(FtpOpcode.TerminateSession, NackError.UnknownCommand);
        }

        var session = input.ReadSession();
        _logger.ZLogInformation($"{Id}TerminateSession(session={session})");
        await TerminateSession(session, cancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{Id}Success TerminateSession(session={session})");
        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSize(0);
        }, cancel).ConfigureAwait(false);
    }

    #endregion

    #region FileRead
    
    public FileReadDelegate? FileRead { private get; set; }

    private async Task InternalFileRead(FileTransferProtocolPacket input, CancellationToken cancel = default)
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

        _logger.ZLogTrace($"{Id}ReadFile(session={session}, offset={offset}, size={size})");
        using var buffer = MemoryPool<byte>.Shared.Rent(size);
        var result = await FileRead(new ReadRequest(session, offset, size), buffer.Memory, cancel)
            .ConfigureAwait(false);
        _logger.ZLogTrace(
            $"{Id}Success ReadFile(session={session}, offset={offset}, size={size}): readCount={result.ReadCount}");
        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSession(session);
            p.WriteSize(result.ReadCount);
            p.WriteOffset(offset);
            // ReSharper disable once AccessToDisposedClosure
            p.WriteData(buffer.Memory.Span[..result.ReadCount]);
        }, cancel).ConfigureAwait(false);
    }

    #endregion

    #region OpenFileRead
    public OpenFileReadDelegate? OpenFileRead { private get; set; }

    private async Task InternalOpenFileRo(FileTransferProtocolPacket input, CancellationToken cancel = default)
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
            _logger.ZLogWarning($"{Id}Duplicate OpenFileRead({path})");
        }
        else
        {
            _logger.ZLogInformation($"{Id}OpenFileRead({path})");
            _lastReadHandle = await OpenFileRead(path, cancel).ConfigureAwait(false);
            _logger.ZLogInformation(
                $"{Id}Success OpenFileRead({path}): session={_lastReadHandle.Session}, size={_lastReadHandle.Size}");
            _lastRoSequenceNumber = sequenceNumber;
        }

        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSession(_lastReadHandle.Session);
            p.WriteDataAsUint(_lastReadHandle.Size);
        }, cancel).ConfigureAwait(false);
    }

    #endregion

    #region OpenFileWrite
    
    public OpenFileWriteDelegate? OpenFileWrite { get; set; }

    private async Task InternalOpenFileWrite(FileTransferProtocolPacket input, CancellationToken cancel = default)
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
            _logger.ZLogWarning($"{Id}Duplicate OpenFileWrite({path})");
        }
        else
        {
            _logger.ZLogInformation($"{Id}OpenFileWrite({path})");
            _lastWriteHandle = await OpenFileWrite(path, cancel).ConfigureAwait(false);
            _logger.ZLogInformation(
                $"{Id}Success OpenFileWrite({path}): session={_lastWriteHandle.Session}, size={_lastWriteHandle.Size}");
            _lastWoSequenceNumber = sequenceNumber;
        }

        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSession(_lastWriteHandle.Session);
            p.WriteDataAsUint(_lastWriteHandle.Size);
        }, cancel).ConfigureAwait(false);
    }

    #endregion

    #region RenameFile

    public RenameDelegate? Rename { private get; set; }
    private async Task InternalRename(FileTransferProtocolPacket input, CancellationToken cancel = default)
    {
        if (Rename is null) throw new FtpNackException(FtpOpcode.Rename, NackError.UnknownCommand);
        var path = input.ReadDataAsString();
        var split = path.Split('\0');
        if (split.Length != 2) throw new FtpNackException(FtpOpcode.Rename, NackError.UnknownCommand);
        var path1 = split[0];
        var path2 = split[1];
        await Rename(path1, path2, cancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{Id} Rename: ({path1}) to ({path2})");
        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSize(0);
        }, cancel).ConfigureAwait(false);
    }

    #endregion

    #region BurstReadFile

    public BurstReadFileDelegate? BurstReadFile { private get; set; }

    private async Task InternalBurstReadFile(FileTransferProtocolPacket input, CancellationToken cancel = default)
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
        _logger.ZLogTrace($"{Id}BurstReadFile(session={session}, offset={offset}, size={size})");
        var isOver = false;
        while (!isOver)
        {
            using var buffer = MemoryPool<byte>.Shared.Rent(size);
            var result = await BurstReadFile(new ReadRequest(session, offset, size), buffer.Memory, cancel).ConfigureAwait(false);
            _logger.ZLogTrace(
                $"{Id}Success BurstReadFile(session={session}, offset={offset}, size={size}): readCount={result.ReadCount}, isLastChunk = {result.IsLastChunk}");
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
            }, cancel).ConfigureAwait(false);
            isOver = result.IsLastChunk;
            offset += result.ReadCount;
            
            if (_config.BurstReadChunkDelayMs>0)
            {
                await Task.Delay(
                        TimeSpan.FromMilliseconds(_config.BurstReadChunkDelayMs), 
                        Context.TimeProvider, 
                        cancel
                ).ConfigureAwait(false);
            }
        }
    }

    #endregion

    #region WriteFile

    public WriteFile? WriteFile { private get; set; }

    private async Task InternalWriteFile(FileTransferProtocolPacket input, CancellationToken cancel = default)
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
        
        _logger.ZLogTrace($"{Id}WriteFile(session={session}, offset={offset}, size={size})");
        await WriteFile(new WriteRequest(session, offset, size), buffer.Memory, cancel)
            .ConfigureAwait(false);
        _logger.ZLogTrace(
            $"{Id}Success WriteFile(session={session}, offset={offset}, size={size})");
        await InternalFtpReply(input, FtpOpcode.Ack, p =>
        {
            p.WriteSize(0);
        }, cancel).ConfigureAwait(false);
    }

    #endregion

    #region ReplyNack

    private ValueTask ReplyNack(
        FileTransferProtocolPacket req, 
        NackError err, 
        Exception? ex = null,
        CancellationToken cancel = default
    )
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

        return InternalFtpReply(
            req, 
            FtpOpcode.Nak, 
            x => x.WriteDataAsByte((byte)err), 
            cancel
        );
    }

    private ValueTask ReplyNackFailErrno(
        FileTransferProtocolPacket req, 
        byte fsErrorCode, 
        Exception? ex = null, 
        CancellationToken cancel = default
    )
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

        return InternalFtpReply(
            req, 
            FtpOpcode.Nak, 
            x => x.WriteDataAsTwoByte((byte)NackError.FailErrno, fsErrorCode),
            cancel
        );
    }

    #endregion

    private ValueTask InternalFtpReply(
        FileTransferProtocolPacket req, 
        FtpOpcode replyOpCode,
        Action<FileTransferProtocolPacket> fillPacket, 
        CancellationToken cancel = default
    )
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
        }, 
            cancel: cancel
        );
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