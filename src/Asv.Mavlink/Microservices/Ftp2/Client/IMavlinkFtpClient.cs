using System;
using System.Buffers;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;

public interface IMavlinkFtpClient
{
    Task<ReadSession> OpenFileRead(string path, CancellationToken cancel = default);
}

public class MavlinkFtpClient : MavlinkMicroserviceClient, IMavlinkFtpClient
{
    private readonly ILogger _logger;
    private readonly IObservable<FileTransferProtocolPacket> _ftpMessages;
    private uint _sequence;

    public MavlinkFtpClient(
        IMavlinkV2Connection connection,
        MavlinkClientIdentity identity, 
        IPacketSequenceCalculator seq,
        IScheduler? scheduler = null,
        ILogger? logger = null
        ):base("FTP",connection,identity,seq,scheduler,logger)
    {
        _logger = logger ?? NullLogger.Instance;
        _ftpMessages = connection
            .FilterVehicle(identity)
            .Filter<FileTransferProtocolPacket>()
            .Publish()
            .RefCount();
    }

    public Task Read(ReadSession readSession, CancellationToken cancel = default)
    {
        
        return Task.CompletedTask;
    }
    
    public async Task<ReadSession> OpenFileRead(string path,CancellationToken cancel = default)
    {
        var result = await InternalCall<FileTransferProtocolPacket,FileTransferProtocolPacket, FileTransferProtocolPacket>(p =>
        {
            p.WriteSequenceNumber(NextSequence());
            p.WriteOpcode(FtpOpcode.OpenFileRO);
            var byteSize = MavlinkFtpHelper.FtpEncoding.GetByteCount(path);
            var pathArray = ArrayPool<byte>.Shared.Rent(byteSize);
            try
            {
                MavlinkFtpHelper.FtpEncoding.GetBytes(path, pathArray);
                var pathSpan = new ReadOnlySpan<byte>(pathArray, 0, byteSize);
                p.WriteData(pathSpan);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(pathArray);
            }
        }, p=>true, p =>p, timeoutMs:50, attemptCount:6, cancel: cancel).ConfigureAwait(false);
        InternalCheckNack(result,FtpOpcode.OpenFileRO);
        var resultSize = result.ReadSize();
        // ACK on success. The payload must specify fields: session = file session id, size = 4, data = length of file that has been opened.
        if (resultSize != 4)
        {
            throw new FtpException($"Error to {FtpOpcode.OpenFileRO:G}: Invalid size of response");
        }
        var fileSize = (uint) MemoryMarshal.Read<uint>(result.Payload.Payload);
        return new ReadSession(result.ReadSession(),fileSize); 
    }

    private static void InternalCheckNack(FileTransferProtocolPacket result, FtpOpcode action)
    {
        if (!result.CheckNack(out var err, out var fsError)) return;
        Debug.Assert(err!=null);
        if (fsError != null)
        {
            throw new FtpNackException(action,err.Value, fsError.Value);    
        }

        throw new FtpNackException(action,err.Value);
    }

    private ushort NextSequence()
    {
        return (ushort)(Interlocked.Increment(ref _sequence) % ushort.MaxValue);
    }
}

public struct ReadSession(byte session, uint size)
{
    public byte Session = session;
    public uint Size = size;
}