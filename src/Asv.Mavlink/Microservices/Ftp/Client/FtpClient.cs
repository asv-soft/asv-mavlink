using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;

using Microsoft.Extensions.Logging;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public class MavlinkFtpClientConfig
{
    public int TimeoutMs { get; set; } = 500;
    public int CommandAttemptCount { get; set; } = 6;
    public byte TargetNetworkId { get; set; } = 0;
    public int BurstTimeoutMs { get; set; } = 1000;
}

public class FtpClient : MavlinkMicroserviceClient, IFtpClient
{
    private readonly MavlinkFtpClientConfig _config;
    private readonly ILogger _logger;
    private uint _sequence;
    private readonly TimeSpan _burstTimeout;

    public FtpClient(MavlinkClientIdentity identity,
        MavlinkFtpClientConfig config,IMavlinkContext core) 
        : base(MavlinkFtpHelper.FtpMicroserviceName, identity, core)
    {
        _config = config;
        _logger = core.LoggerFactory.CreateLogger<FtpClient>();
        _burstTimeout = TimeSpan.FromMilliseconds(config.BurstTimeoutMs);
    }

    public Task<FileTransferProtocolPacket> ResetSessions(CancellationToken cancellationToken = default)
    {
        _logger.ZLogInformation($"{Id} {FtpOpcode.ResetSessions:G}(Reset all sessions)");
        return InternalFtpCall(FtpOpcode.ResetSessions, packet =>{} , cancellationToken);
    }

    public async Task<FileTransferProtocolPacket> RemoveDirectory(string path, CancellationToken cancellationToken = default)
    {
        _logger.ZLogInformation($"{Id} {FtpOpcode.RemoveDirectory:G} {path}");
        var result = await InternalFtpCall(FtpOpcode.RemoveDirectory, p => p.WriteDataAsString(path), cancellationToken).ConfigureAwait(false);
        _logger.ZLogInformation($"{Id} {result.ReadOpcode():G} {result.ReadSize()}");
        return result;
    }

    public async Task<FileTransferProtocolPacket> RemoveFile(string path, CancellationToken cancellationToken = default)
    {
        _logger.ZLogInformation($"{Id} {FtpOpcode.RemoveFile:G} {path}");
        var result = await InternalFtpCall(FtpOpcode.RemoveFile, p => p.WriteDataAsString(path), cancellationToken).ConfigureAwait(false);
        _logger.ZLogInformation($"{Id} {result.ReadOpcode():G} {result.ReadSize()}");
        return result;
    }

    public async Task<FileTransferProtocolPacket> TruncateFile(TruncateRequest request, CancellationToken cancellationToken = default)
    {
        _logger.ZLogInformation($"{Id} {FtpOpcode.TruncateFile:G} {request.ToString()}");
        var result = await InternalFtpCall(FtpOpcode.TruncateFile, p =>
        {
            p.WriteDataAsString(request.Path);
            p.WriteOffset(request.Offset);
        }, cancellationToken).ConfigureAwait(false);
        _logger.ZLogInformation($"{Id} {FtpOpcode.TruncateFile:G} {request.Path}: truncated to {request.Offset}");
        return result;
    }
    
    public async Task<FileTransferProtocolPacket> WriteFile(WriteRequest request, Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        _logger.ZLogInformation($"{Id} {FtpOpcode.WriteFile:G} {request.ToString()}");
        var result = await InternalFtpCall(FtpOpcode.WriteFile, p =>
        {
            p.WriteSession(request.Session);
            p.WriteSize(request.Take);
            p.WriteOffset(request.Skip);
            p.WriteData(buffer.Span[..request.Take]);
        }, cancellationToken).ConfigureAwait(false);
        _logger
            .ZLogInformation($"{Id} {FtpOpcode.WriteFile:G} size: {request.Take}, offset: {request.Skip}, session: {request.Session}");
        return result;
    }

    public async Task<uint> CalcFileCrc32(string path, CancellationToken cancellationToken = default)
    {
        _logger.ZLogInformation($"{Id} {FtpOpcode.CalcFileCRC32:G} {path}");
        var result = await InternalFtpCall(FtpOpcode.CalcFileCRC32, p => p.WriteDataAsString(path), cancellationToken).ConfigureAwait(false);
        var crc32 = result.ReadDataAsUint();
        _logger.ZLogInformation($"{Id} {result.ReadOpcode():G} {crc32})");
        return crc32;
    }

    public async Task BurstReadFile(ReadRequest request, Action<FileTransferProtocolPacket> onBurstData, CancellationToken cancel = default)
    {
        if (request.Take > MavlinkFtpHelper.MaxDataSize)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Take), $"Max data size is {MavlinkFtpHelper.MaxDataSize}");
        }
        const uint totalRead = 0U;
        _logger.ZLogInformation($"{Id} {FtpOpcode.BurstReadFile:G} {request}");
        
        var tcs = new TaskCompletionSource();
        await using var tcsCancel = cancel.Register(() => tcs.TrySetCanceled());
        
        // create timer to check if we skip burst complete packet
        var lastUpdate = Core.TimeProvider.GetTimestamp();
        await using var timer = Core.TimeProvider.CreateTimer(x=>
        {
            if (Core.TimeProvider.GetElapsedTime(lastUpdate) <= _burstTimeout) return;
            _logger.ZLogWarning($"Timeout while burst read {request} but not received burst complete packet.");
            tcs.TrySetResult();
        },null,_burstTimeout,_burstTimeout);
        
        using var stream = InternalFilter<FileTransferProtocolPacket>(p => p.ReadSession() == request.Session && p.ReadOriginOpCode() == FtpOpcode.BurstReadFile)
            .Subscribe(x =>
            {
                lastUpdate = Core.TimeProvider.GetTimestamp();
                try
                {
                    InternalCheckNack(x,_logger);
                }
                catch (FtpNackEndOfFileException)
                {
                    tcs.SetResult();
                    return;
                }
                catch (Exception e)
                {
                    tcs.TrySetException(e);
                    return;
                }
                onBurstData(x);
                if (x.ReadBurstComplete())
                {
                    _logger.ZLogInformation($"{Id} {FtpOpcode.BurstReadFile:G}({request}): burst read complete (read {totalRead} bytes)");
                    tcs.TrySetResult();
                }
            });
        await InternalFtpCall(FtpOpcode.BurstReadFile,p =>
        {
            p.WriteSession(request.Session);
            p.WriteSize(request.Take);
            p.WriteOffset(request.Skip);
        }, cancel, request.Session).ConfigureAwait(false);
        
        
        // https://mavlink.io/en/services/ftp.html#reading-a-file-burstreadfile
        // ACK on success. The payload must specify fields: session = file session id, size = 4, data = length of file in burst.
        // but Ardupilot doesn't send ACK
        // here we just read first stream packet
        /*var resultSize = result.ReadDataAsUint();
        var size = result.ReadSize();
        if (size != 4)
            throw new FtpException("Unexpected result to BurstReadFile: ACK must be 4 byte length");*/

        await tcs.Task.ConfigureAwait(false);
    }
    
    public async Task<FileTransferProtocolPacket> Rename(string path1, string path2, CancellationToken cancel)
    {
        _logger.ZLogTrace($"{Id} {FtpOpcode.Rename:G}({path1}) to ({path2})");
        var result = await InternalFtpCall(FtpOpcode.Rename, p =>
        {
            p.WriteDataAsString($"{path1}\0{path2}");
        }, cancel).ConfigureAwait(false);
        _logger.ZLogTrace($"{Id} {FtpOpcode.Rename:G}({path1}) to ({path2}): read={result.ReadSize()}");
        return result;
    }
    
    public async Task<FileTransferProtocolPacket> CreateDirectory(string path, CancellationToken cancellationToken = default)
    {
        MavlinkFtpHelper.CheckFolderPath(path);
        _logger.ZLogInformation($"{Id} {FtpOpcode.CreateDirectory:G}({path})");
        var result = await InternalFtpCall(
            FtpOpcode.CreateDirectory,
            p => p.WriteDataAsString(path), 
            cancellationToken)
            .ConfigureAwait(false);
        return result;
    }
    
    public async Task<FileTransferProtocolPacket> CreateFile(string path, CancellationToken cancellationToken = default)
    {
        MavlinkFtpHelper.CheckFilePath(path);
        _logger.ZLogInformation($"{Id} {FtpOpcode.CreateFile:G}({path})");
        var result = await InternalFtpCall(
                FtpOpcode.CreateFile,
                p =>
                {
                    p.WriteDataAsString(path);
                    p.WriteSize((byte)path.Length);
                }, 
                cancellationToken)
            .ConfigureAwait(false);
        return result;
    }

    public Task TerminateSession(byte session, CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"{Id} {FtpOpcode.TerminateSession:G}({session})");
        return InternalFtpCall(FtpOpcode.TerminateSession,p => p.WriteSession(session), cancel);
    }

    public async Task<FileTransferProtocolPacket> ListDirectory(string path, uint offset, CancellationToken cancel = default)
    {
        MavlinkFtpHelper.CheckFolderPath(path);
        _logger.ZLogInformation($"{Id} {FtpOpcode.ListDirectory:G}({path})");
        var result = await InternalFtpCall(FtpOpcode.ListDirectory, p =>
        {
            p.WriteDataAsString(path);
            p.WriteOffset(offset);
        }, cancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{Id} {FtpOpcode.ListDirectory:G}({path}): read={result.ReadSize()}");
        return result;
    }

    public async Task<FileTransferProtocolPacket> ReadFile(ReadRequest request, CancellationToken cancel = default)
    {
        if (request.Take > MavlinkFtpHelper.MaxDataSize)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Take), $"Max data size is {MavlinkFtpHelper.MaxDataSize}");
        }
        _logger.ZLogTrace($"{Id} {FtpOpcode.ReadFile:G}({request})");
        var result = await InternalFtpCall(FtpOpcode.ReadFile,p =>
        {
            p.WriteSession(request.Session);
            p.WriteSize(request.Take);
            p.WriteOffset(request.Skip);
        }, cancel, request.Session).ConfigureAwait(false);
        _logger.ZLogTrace($"{Id} {FtpOpcode.ReadFile:G}({request}): read={result.ReadSize()}");
        return result;
    }
    
    public async Task<ReadHandle> OpenFileRead(string path,CancellationToken cancel = default)
    {
        MavlinkFtpHelper.CheckFilePath(path);
        _logger.ZLogInformation($"{Id} {FtpOpcode.OpenFileRO:G}({path})"); 
        var result = await InternalFtpCall(FtpOpcode.OpenFileRO,p => p.WriteDataAsString(path), cancel).ConfigureAwait(false);
        InternalCheckNack(result, _logger);
        var resultSize = result.ReadSize();
        // ACK on success. The payload must specify fields: session = file session id, size = 4, data = length of file that has been opened.
        if (resultSize != 4)
        {
            _logger.ZLogError($"Unexpected error to {FtpOpcode.OpenFileRO:G}: ACK must be {4} byte length");
            throw new FtpException($"Unexpected error to {FtpOpcode.OpenFileRO:G}: ACK must be {4} byte length");
        }

        var sessionId = result.ReadSession();
        var fileSize = result.ReadDataAsUint();
        _logger.ZLogInformation($"{Id} {FtpOpcode.OpenFileRO:G}({path}): session={sessionId}, size={fileSize}, '{path}'={fileSize}");
        return new ReadHandle(sessionId,fileSize); 
    }
    
    public async Task<WriteHandle> OpenFileWrite(string path, CancellationToken cancel = default)
    {
        MavlinkFtpHelper.CheckFilePath(path);
        _logger.ZLogInformation($"{Id} {FtpOpcode.OpenFileWO:G}({path})"); 
        var result = await InternalFtpCall(
            FtpOpcode.OpenFileWO, 
            p => p.WriteDataAsString(path), cancel)
            .ConfigureAwait(false);
        InternalCheckNack(result, _logger);
        var resultSize = result.ReadSize();
        // ACK on success. The payload must specify fields: session = file session id, size = 4, data = length of file that has been opened.
        if (resultSize != 4)
        {
            _logger.ZLogError($"Unexpected error to {FtpOpcode.OpenFileWO:G}: ACK must be {4} byte length");
            throw new FtpException($"Unexpected error to {FtpOpcode.OpenFileWO:G}: ACK must be {4} byte length");
        }

        var sessionId = result.ReadSession();
        var fileSize = result.ReadDataAsUint();
        _logger.ZLogInformation($"{Id} {FtpOpcode.OpenFileWO:G}({path}): session={sessionId}, size={fileSize}, '{path}'={fileSize}");
        return new WriteHandle(sessionId,fileSize); 
    }
    
    private async Task<FileTransferProtocolPacket> InternalFtpCall(FtpOpcode opCode,Action<FileTransferProtocolPacket> fillPacket, CancellationToken cancel, byte? filterSession = null, Func<FileTransferProtocolPacket,bool>? additionalFilter = null)
    {
        var result = await InternalCall<FileTransferProtocolPacket,FileTransferProtocolPacket, FileTransferProtocolPacket>(p =>
            {
                fillPacket(p);
                p.Payload.TargetComponent = Identity.Target.ComponentId;
                p.Payload.TargetSystem = Identity.Target.SystemId;
                p.Payload.TargetNetwork = _config.TargetNetworkId;
                p.WriteSequenceNumber(NextSequence());
                p.WriteOpcode(opCode);
            }, filter:p=>
            {
                if (p.ReadOpcode() != FtpOpcode.Ack && p.ReadOpcode() != FtpOpcode.Nak) return false;
                if (p.ReadOriginOpCode() != opCode) return false;
                if (filterSession != null && p.ReadSession() != filterSession) return false;
                if (additionalFilter != null && !additionalFilter(p)) return false;
                return true;
            },
            resultGetter: p =>p, 
            timeoutMs: _config.TimeoutMs, 
            attemptCount: _config.CommandAttemptCount, 
            cancel: cancel).ConfigureAwait(false);
        InternalCheckNack(result,_logger);
        return result;
    }
    
    private static void InternalCheckNack(FileTransferProtocolPacket result, ILogger logger)
    {
        var opCode = result.ReadOpcode();
        var originOpCode = result.ReadOriginOpCode();
        if (opCode == FtpOpcode.Ack)
        {
            return;
        }
        if (opCode != FtpOpcode.Nak)
        {
            logger.ZLogError($"Error to {originOpCode:G}: want {FtpOpcode.Nak:G} or {FtpOpcode.Ack:G} but received {opCode:G}");
            throw new FtpException($"Error to {originOpCode:G}: want {FtpOpcode.Nak:G} or {FtpOpcode.Ack:G} but received {opCode:G}");
        }
        var size = result.ReadSize();
        var error = (NackError)result.ReadDataFirstByte();
        if (error == NackError.EOF)
        {
            logger.ZLogInformation($"Receive EOF to {originOpCode}");
            throw new FtpNackEndOfFileException(originOpCode);
        }
        if (size == 2 && error == NackError.FailErrno)
        {
            var fsError = result.ReadDataSecondByte();
            logger.ZLogError($"Error to {originOpCode}: {error:G} with FS error code:{fsError}");
            throw new FtpNackException(originOpCode, fsError);    
        }
        logger.ZLogError($"Error to {originOpCode}: {error:G}");
        throw new FtpNackException(originOpCode,error);
    }

    private ushort NextSequence()
    {
        return (ushort)(Interlocked.Increment(ref _sequence) % ushort.MaxValue);
    }
}