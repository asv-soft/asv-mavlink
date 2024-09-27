using System;
using System.Buffers;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
    private readonly TimeProvider _timeProvider;
    private readonly ILogger _logger;
    private uint _sequence;
    private readonly TimeSpan _burstTimeout;

    public FtpClient(
        MavlinkFtpClientConfig config,
        IMavlinkV2Connection connection,
        MavlinkClientIdentity identity, 
        IPacketSequenceCalculator seq,
        TimeProvider timeProvider,
        IScheduler? scheduler = null,
        ILogger? logger = null
    ):base("FTP",connection,identity,seq,scheduler,logger)
    {
        _config = config;
        _timeProvider = timeProvider;
        _logger = logger ?? NullLogger.Instance;
        _burstTimeout = TimeSpan.FromMilliseconds(config.BurstTimeoutMs);
    }

    public async Task BurstReadFile(ReadRequest request, Action<FileTransferProtocolPacket> onBurstData, CancellationToken cancel = default)
    {
        if (request.Take > MavlinkFtpHelper.MaxDataSize)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Take), $"Max data size is {MavlinkFtpHelper.MaxDataSize}");
        }
        var totalRead = 0U;
        _logger.ZLogInformation($"{LogSend} {FtpOpcode.BurstReadFile:G} {request}");
        
        var tcs = new TaskCompletionSource();
        await using var tcsCancel = cancel.Register(() => tcs.TrySetCanceled());
        
        // create timer to check if we skip burst complete packet
        var lastUpdate = _timeProvider.GetTimestamp();
        await using var timer = _timeProvider.CreateTimer(x=>
        {
            if (_timeProvider.GetElapsedTime(lastUpdate) <= _burstTimeout) return;
            _logger.ZLogWarning($"Timeout while burst read {request} but not received burst complete packet.");
            tcs.TrySetResult();
        },null,_burstTimeout,_burstTimeout);
        
        using var stream = InternalFilteredVehiclePackets
            .Filter<FileTransferProtocolPacket>()
            .Where(p => p.ReadSession() == request.Session && p.ReadOriginOpCode() == FtpOpcode.BurstReadFile)
            .Subscribe(x =>
            {
                lastUpdate = _timeProvider.GetTimestamp();
                try
                {
                    InternalCheckNack(x,_logger);
                }
                catch (FtpNackEndOfFileException e)
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
                    _logger.ZLogInformation($"{LogRecv} {FtpOpcode.BurstReadFile:G}({request}): burst read complete (read {totalRead} bytes)");
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
        _logger.ZLogTrace($"{LogSend} {FtpOpcode.Rename:G}({path1}) to ({path2})");
        var result = await InternalFtpCall(FtpOpcode.Rename, p =>
        {
            p.WriteDataAsString(path1);
            p.WriteDataAsString(path2);
        }, cancel).ConfigureAwait(false);
        _logger.ZLogTrace($"{LogRecv} {FtpOpcode.Rename:G}({path1}) to ({path2}): read={result.ReadSize()}");
        return result;
    }

    public Task TerminateSession(byte session, CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"{LogSend} {FtpOpcode.TerminateSession:G}({session})");
        return InternalFtpCall(FtpOpcode.TerminateSession,p => p.WriteSession(session), cancel);
    }

    public async Task<FileTransferProtocolPacket> ListDirectory(string path, uint offset, CancellationToken cancel = default)
    {
        MavlinkFtpHelper.CheckFolderPath(path);
        _logger.ZLogInformation($"{LogSend} {FtpOpcode.ListDirectory:G}({path})");
        var result = await InternalFtpCall(FtpOpcode.ListDirectory, p =>
        {
            p.WriteDataAsString(path);
            p.WriteOffset(offset);
        }, cancel).ConfigureAwait(false);
        _logger.ZLogInformation($"{LogRecv} {FtpOpcode.ListDirectory:G}({path}): read={result.ReadSize()}");
        return result;
    }

  

    public async Task<FileTransferProtocolPacket> ReadFile(ReadRequest request, CancellationToken cancel = default)
    {
        if (request.Take > MavlinkFtpHelper.MaxDataSize)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Take), $"Max data size is {MavlinkFtpHelper.MaxDataSize}");
        }
        _logger.ZLogTrace($"{LogSend} {FtpOpcode.ReadFile:G}({request})");
        var result = await InternalFtpCall(FtpOpcode.ReadFile,p =>
        {
            p.WriteSession(request.Session);
            p.WriteSize(request.Take);
            p.WriteOffset(request.Skip);
        }, cancel, request.Session).ConfigureAwait(false);
        _logger.ZLogTrace($"{LogRecv} {FtpOpcode.ReadFile:G}({request}): read={result.ReadSize()}");
        return result;
    }
    
    public async Task<ReadHandle> OpenFileRead(string path,CancellationToken cancel = default)
    {
        MavlinkFtpHelper.CheckFilePath(path);
        _logger.ZLogInformation($"{LogSend} {FtpOpcode.OpenFileRO:G}({path})"); 
        var result = await InternalFtpCall(FtpOpcode.OpenFileRO,p => p.WriteDataAsString(path), cancel).ConfigureAwait(false);
        InternalCheckNack(result,_logger);
        var resultSize = result.ReadSize();
        // ACK on success. The payload must specify fields: session = file session id, size = 4, data = length of file that has been opened.
        if (resultSize != 4)
        {
            _logger.ZLogError($"Unexpected error to {FtpOpcode.OpenFileRO:G}: ACK must be {4} byte length");
            throw new FtpException($"Unexpected error to {FtpOpcode.OpenFileRO:G}: ACK must be {4} byte length");
        }

        var sessionId = result.ReadSession();
        var fileSize = result.ReadDataAsUint();
        _logger.ZLogInformation($"{LogRecv} {FtpOpcode.OpenFileRO:G}({path}): session={sessionId}, size={fileSize}, '{path}'={fileSize}");
        return new ReadHandle(sessionId,fileSize); 
    }
    private async Task<FileTransferProtocolPacket> InternalFtpCall(FtpOpcode opCode,Action<FileTransferProtocolPacket> fillPacket, CancellationToken cancel, byte? filterSession = null, Func<FileTransferProtocolPacket,bool>? additionalFilter = null)
    {
        var result = await InternalCall<FileTransferProtocolPacket,FileTransferProtocolPacket, FileTransferProtocolPacket>(p =>
            {
                fillPacket(p);
                p.Payload.TargetComponent = Identity.TargetComponentId;
                p.Payload.TargetSystem = Identity.TargetSystemId;
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