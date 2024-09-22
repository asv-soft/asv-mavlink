using System;
using System.Buffers;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

namespace Asv.Mavlink;

public interface IMavlinkFtpClient
{
    Task<OpenReadResult> OpenFileRead(string path, CancellationToken cancel = default);
    Task<ReadResult> Read(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default);
}

public class ReadResult
{
    
}

public class MavlinkFtpClientConfig
{
    public int TimeoutMs { get; set; } = 50;
    public int CommandAttemptCount { get; set; } = 6;
    public byte TargetNetworkId { get; set; } = 0;
}

public class MavlinkFtpClient : MavlinkMicroserviceClient, IMavlinkFtpClient
{
    private readonly MavlinkFtpClientConfig _config;
    private readonly ILogger _logger;
    private uint _sequence;

    public MavlinkFtpClient(
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

   
    
    public async Task<OpenReadResult> OpenFileRead(string path,CancellationToken cancel = default)
    {
        MavlinkFtpHelper.CheckPath(path);
        _logger.ZLogInformation($"Call {FtpOpcode.OpenFileRO:G}: '{path}'"); 
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
        _logger.ZLogInformation($"Success {FtpOpcode.OpenFileRO:G}: session={sessionId}, size={fileSize}, '{path}'={fileSize}");
        return new OpenReadResult(sessionId,fileSize); 
    }

    private async Task<FileTransferProtocolPacket> InternalFtpCall(FtpOpcode opCode,Action<FileTransferProtocolPacket> fillPacket, CancellationToken cancel)
    {
        var result = await InternalCall<FileTransferProtocolPacket,FileTransferProtocolPacket, FileTransferProtocolPacket>(p =>
        {
            fillPacket(p);
            p.Payload.TargetComponent = Identity.TargetComponentId;
            p.Payload.TargetSystem = Identity.TargetSystemId;
            p.Payload.TargetNetwork = _config.TargetNetworkId;
            p.WriteSequenceNumber(NextSequence());
            p.WriteOpcode(opCode);
        }, filter:p=>p.ReadOriginOpCode() == opCode,
           resultGetter: p =>p, 
           timeoutMs: _config.TimeoutMs, 
           attemptCount: _config.CommandAttemptCount, 
           cancel: cancel).ConfigureAwait(false);
        InternalCheckNack(result,_logger);
        return result;
    }
    
    public Task<ReadResult> Read(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    private static void InternalCheckNack(FileTransferProtocolPacket result, ILogger logger)
    {
        var opCode = result.ReadOpcode();
        var originOpCode = result.ReadOriginOpCode();
        if (opCode != FtpOpcode.Nak)
        {
            return;
        }
        var size = result.ReadSize();
        var error = (NackError)result.ReadDataFirstByte();
        if (size == 2 && error == NackError.FailErrno)
        {
            var fsError = result.ReadDataSecondByte();
            logger.ZLogError($"Error to {originOpCode}: {error:G} with FS error code:{fsError}");
            throw new FtpNackException(originOpCode,error, fsError);    
        }
        logger.ZLogError($"Error to {originOpCode}: {error:G}");
        throw new FtpNackException(originOpCode,error);
    }

    private ushort NextSequence()
    {
        return (ushort)(Interlocked.Increment(ref _sequence) % ushort.MaxValue);
    }
}

public struct ReadRequest(byte session, byte skip, byte take)
{
    public byte Session = session;
    public byte Skip = skip;
    public byte Take = take;
}

public struct OpenReadResult(byte session, uint size)
{
    public byte Session = session;
    public uint Size = size;
}