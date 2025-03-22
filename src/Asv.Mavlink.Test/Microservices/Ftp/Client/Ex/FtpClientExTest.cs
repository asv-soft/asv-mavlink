using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(FtpClientEx))]
public class FtpClientExTest : ClientTestBase<FtpClientEx>
{
    private readonly TaskCompletionSource _tcs = new();
    private readonly CancellationTokenSource _cts;
    
    private readonly MavlinkFtpClientConfig _clientExConfig = new ()
    {
        TimeoutMs = 1000,
        CommandAttemptCount = 5,
        TargetNetworkId = 0,
        BurstTimeoutMs = 100
    };

    public FtpClientExTest(ITestOutputHelper log) 
        : base(log)
    {
        _cts = new CancellationTokenSource();
        _cts.Token.Register(() => _tcs.TrySetCanceled());
    }

    protected override FtpClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new FtpClientEx(new FtpClient(identity, _clientExConfig, core));
    }

    [Fact]
    public async Task UploadFile_Success()
    {
        // Arrange
        const string filePath = "/path/to/new_file.txt";
        var data = new byte[500];
        new Random().NextBytes(data);
        var stream = new MemoryStream(data);

        var writtenData = new ArrayBufferWriter<byte>(data.Length);
        
        // Act
        Link.Server.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.CreateFile)
            {
                var requestedPath = packet.ReadDataAsString();

                Assert.Equal(filePath, requestedPath);

                var response = CreateAckResponse(packet, FtpOpcode.CreateFile);
                response.WriteSession(2);
                _ = Link.Server.Send(response);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        Link.Server.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.WriteFile)
            {
                var size = packet.ReadSize();
                var innerData = packet.Payload.Payload.AsSpan(12, size).ToArray();
                writtenData.Write(innerData);

                var response = CreateAckResponse(packet, FtpOpcode.WriteFile);
                response.WriteSize(4);

                _ = Link.Server.Send(response);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        Link.Server.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.TruncateFile)
            {
                var response = CreateAckResponse(packet, FtpOpcode.TruncateFile);

                _ = Link.Server.Send(response);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        Link.Server.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.TerminateSession)
            {
                var response = CreateAckResponse(packet, FtpOpcode.TerminateSession);
                _ = Link.Server.Send(response);
                response.WriteSize(4);

                Time.Advance(TimeSpan.FromMilliseconds(10));
                
                _tcs.TrySetResult();
            }
        });
        
        var resultTask = Client.UploadFile(filePath, stream);
        await _tcs.Task.ConfigureAwait(false);
        await resultTask.ConfigureAwait(false);
        
        // Assert
        Assert.Equal(data, writtenData.WrittenMemory.ToArray());
    }

    [Fact]
    public async Task DownloadFile_Success()
    {
        // Arrange
        const string path = "/path/to/file.txt";
        var stream = new MemoryStream(500);
        var data = new byte[500];
        new Random().NextBytes(data);
        var skip = 0;
        var take = 239;
        
        // Act
        Link.Server.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.OpenFileRO)
            {
                var requestedPath = packet.ReadDataAsString();

                Assert.Equal(path, requestedPath);

                var response = CreateAckResponse(packet, FtpOpcode.OpenFileRO);
                response.WriteDataAsUint(500);
                response.WriteSize(4);

                _ = Link.Server.Send(response);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        Link.Server.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.ReadFile)
            {
                var response = CreateAckResponse(packet, FtpOpcode.ReadFile);
                var mem = new ReadOnlySpan<byte>(data, skip, take);
                response.WriteData(mem);
                skip += 239;
                take = Math.Min(239, data.Length - skip);
                
                _ = Link.Server.Send(response);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        Link.Server.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.TerminateSession)
            {
                var response = CreateAckResponse(packet, FtpOpcode.TerminateSession);
                _ = Link.Server.Send(response);

                Time.Advance(TimeSpan.FromMilliseconds(10));
                
                _tcs.TrySetResult();
            }
        });
        
        var task = Client.DownloadFile(path, stream);
        await _tcs.Task.ConfigureAwait(false);
        await task.ConfigureAwait(false);
        
        // Assert
        Assert.Equal(data, stream.ToArray());
    }
    
    [Fact]
    public async Task BurstDownloadFile_Success()
    {
        // Arrange
        uint fileSize = 500;
        var path = "/path/to/file.txt";
        var stream = new MemoryStream((int)fileSize);
        var data = new byte[fileSize];
        new Random().NextBytes(data);
        uint skip = 0;
        byte take = 239;
        
        // Act
        Link.Server.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.OpenFileRO)
            {
                var requestedPath = packet.ReadDataAsString();

                Assert.Equal(path, requestedPath);

                var response = CreateAckResponse(packet, FtpOpcode.OpenFileRO);
                response.WriteDataAsUint(fileSize);
                response.WriteSize(4);

                _ = Link.Server.Send(response);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        Link.Server.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.BurstReadFile)
            {
                while (skip < fileSize)
                {
                    var nextSkip = Math.Min(take, fileSize - skip);
                    var response = CreateAckResponse(packet, FtpOpcode.BurstReadFile);
                    response.WriteSize(take);
                    response.WriteOffset(skip);
                    response.WriteData(data.AsSpan().Slice((int)skip, (int)nextSkip));
                    skip += nextSkip;
                    if (skip < fileSize)
                    {
                        response.WriteBurstComplete(1);
                    }
                
                    _ = Link.Server.Send(response);

                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            }
        });
        Link.Server.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.ReadFile)
            {
                var response = CreateAckResponse(packet, FtpOpcode.ReadFile);
                response.WriteSize(4);
                
                _ = Link.Server.Send(response);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        Link.Server.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.TerminateSession)
            {
                var response = CreateAckResponse(packet, FtpOpcode.TerminateSession);
                response.WriteSize(4);
                
                _ = Link.Server.Send(response);
                
                Time.Advance(TimeSpan.FromMilliseconds(10));
                
                _tcs.TrySetResult();
            }
        });
        
        var task = Client.BurstDownloadFile(path, stream);
        await _tcs.Task.ConfigureAwait(false);
        await task.ConfigureAwait(false);
        
        // Assert
        Assert.Equal(data, stream.ToArray());
    }
    
    private FileTransferProtocolPacket CreateAckResponse(FileTransferProtocolPacket requestPacket,
        FtpOpcode originOpCode)
    {
        var response = new FileTransferProtocolPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Sequence = requestPacket.Sequence,
            Payload =
            {
                TargetSystem = requestPacket.SystemId,
                TargetComponent = requestPacket.ComponentId,
                TargetNetwork = requestPacket.Payload.TargetNetwork,
                Payload = new byte[251],
            }
        };

        response.WriteOpcode(FtpOpcode.Ack);
        response.WriteSequenceNumber(requestPacket.ReadSequenceNumber());
        response.WriteSession(requestPacket.ReadSession());
        response.WriteSize(0);
        response.WriteOriginOpCode(originOpCode);

        return response;
    }
}