using System;
using System.Reactive.Concurrency;
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
}

public class FtpClient : MavlinkMicroserviceClient, IFtpClient
{
    private readonly MavlinkFtpClientConfig _config;
    private readonly ILogger _logger;
    private uint _sequence;

    public FtpClient(
        MavlinkFtpClientConfig config,
        IMavlinkV2Connection connection,
        MavlinkClientIdentity identity, 
        IPacketSequenceCalculator seq,
        IScheduler? scheduler = null,
        ILogger? logger = null
    ):base("FTP",connection,identity,seq,scheduler,logger)
    {
        _config = config;
        _logger = logger ?? NullLogger.Instance;
        connection
            .FilterVehicle(identity)
            .Filter<FileTransferProtocolPacket>()
            .Subscribe(x=>Interlocked.Exchange(ref _sequence, (uint)x.ReadOpcode()))
            .DisposeItWith(Disposable);
    }
    
    public Task TerminateSession(byte session, CancellationToken cancel = default)
    {
        _logger.ZLogInformation($"{LogSend} {FtpOpcode.TerminateSession:G}({session})");
        return InternalFtpCall(FtpOpcode.TerminateSession,p => p.WriteSession(session), cancel);
    }

    public Task<byte> ListDirectory(string path, uint offset, Memory<char> buffer, CancellationToken cancel = default)
    {
        MavlinkFtpHelper.CheckFolderPath(path);
        _logger.ZLogInformation($"{LogSend} {FtpOpcode.ListDirectory:G}({path})");
        return InternalFtpCall(FtpOpcode.ListDirectory,p =>
        {
            p.WriteDataAsString(path);
            p.WriteOffset(offset);
        }, cancel).ContinueWith(t =>
        {
            var result = t.Result;
            var readCount = result.ReadDataAsString(buffer);
            _logger.ZLogInformation($"{LogRecv} {FtpOpcode.ListDirectory:G}({path}): read={readCount}");
            return readCount;
        }, cancel);
    }

    
    public async Task<ReadResult> ReadFile(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default)
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
        var readCount = result.ReadData(buffer);
        _logger.ZLogTrace($"{LogRecv} {FtpOpcode.ReadFile:G}({request}): read={readCount}");
        return new ReadResult(readCount, request);
    }
    
    public async Task<OpenReadResult> OpenFileRead(string path,CancellationToken cancel = default)
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
        return new OpenReadResult(sessionId,fileSize); 
    }
    private async Task<FileTransferProtocolPacket> InternalFtpCall(FtpOpcode opCode,Action<FileTransferProtocolPacket> fillPacket, CancellationToken cancel, byte? filterSession = null)
    {
        var result = await InternalCall<FileTransferProtocolPacket,FileTransferProtocolPacket, FileTransferProtocolPacket>(p =>
            {
                fillPacket(p);
                p.Payload.TargetComponent = Identity.TargetComponentId;
                p.Payload.TargetSystem = Identity.TargetSystemId;
                p.Payload.TargetNetwork = _config.TargetNetworkId;
                p.WriteSequenceNumber(NextSequence());
                p.WriteOpcode(opCode);
            }, filter:p=> filterSession.HasValue
                ? p.ReadSession() == filterSession.Value && (p.ReadOpcode() == FtpOpcode.Ack || p.ReadOpcode() == FtpOpcode.Nak) && p.ReadOriginOpCode() == opCode
                : (p.ReadOpcode() == FtpOpcode.Ack || p.ReadOpcode() == FtpOpcode.Nak) && p.ReadOriginOpCode() == opCode,
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
            throw new FtpNackException(originOpCode,error);
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