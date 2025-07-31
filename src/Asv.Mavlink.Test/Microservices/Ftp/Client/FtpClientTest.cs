using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class FtpClientTest : ClientTestBase<FtpClient>
{
    private readonly TaskCompletionSource<FileTransferProtocolPacket> _tcs = new();
    private readonly CancellationTokenSource _cts;

    private readonly MavlinkFtpClientConfig _config = new()
    {
        TimeoutMs = 1000,
        CommandAttemptCount = 5,
        TargetNetworkId = 0,
        BurstTimeoutMs = 1000
    };

    public FtpClientTest(ITestOutputHelper log) : base(log)
    {
        _cts = new CancellationTokenSource();
        _cts.Token.Register(() => _tcs.TrySetCanceled());
    }

    protected override FtpClient CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new FtpClient(identity, _config, core);
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
            }
        };

        response.WriteOpcode(FtpOpcode.Ack);
        response.WriteSequenceNumber(requestPacket.ReadSequenceNumber());
        response.WriteSession(requestPacket.ReadSession());
        response.WriteSize(0);
        response.WriteOriginOpCode(originOpCode);

        return response;
    }
    
    private FileTransferProtocolPacket CreateNackResponse(
        FileTransferProtocolPacket requestPacket,
        NackError errorCode = NackError.Fail
        )
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
            }
        };

        response.WriteOpcode(FtpOpcode.Nak);
        response.WriteSequenceNumber(requestPacket.ReadSequenceNumber());
        response.WriteSession(requestPacket.ReadSession());
        response.WriteSize(0);
        response.WriteOriginOpCode(requestPacket.ReadOpcode());
        response.WriteDataAsByte((byte) errorCode);
        
        return response;
    }

    [Fact]
    public async Task ResetSessions_All_Success()
    {
        // Arrange
        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.ResetSessions)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var response = CreateAckResponse(packet, FtpOpcode.ResetSessions);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var resultTask = Client.ResetSessions();

        await _tcs.Task;

        var result = await resultTask;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task RemoveDirectory_SingleDirectory_Success()
    {
        // Arrange
        var path = "/path/to/directory";

        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.RemoveDirectory)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var requestedPath = packet.ReadDataAsString();
            Assert.Equal(path, requestedPath);

            var response = CreateAckResponse(packet, FtpOpcode.RemoveDirectory);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var resultTask = Client.RemoveDirectory(path);

        await _tcs.Task;

        var result = await resultTask;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task RemoveFile_SingleFile_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";

        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.RemoveFile)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var requestedPath = packet.ReadDataAsString();
            Assert.Equal(path, requestedPath);

            var response = CreateAckResponse(packet, FtpOpcode.RemoveFile);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var resultTask = Client.RemoveFile(path);

        await _tcs.Task;

        var result = await resultTask;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task TruncateFile_SingleFile_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";
        var offset = 1024U;
        var request = new TruncateRequest(path, offset);

        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.TruncateFile)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var requestedPath = packet.ReadDataAsString();
            var requestedOffset = packet.ReadOffset();

            Assert.Equal(path, requestedPath);
            Assert.Equal(offset, requestedOffset);

            var response = CreateAckResponse(packet, FtpOpcode.TruncateFile);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var resultTask = Client.TruncateFile(request);

        await _tcs.Task;

        var result = await resultTask;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task CalcFileCrc32_SingleFile_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";
        var expectedCrc32 = 0xDEADBEEF;

        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.CalcFileCRC32)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var requestedPath = packet.ReadDataAsString();

            Assert.Equal(path, requestedPath);

            var response = CreateAckResponse(packet, FtpOpcode.CalcFileCRC32);
            response.WriteSize(4);
            response.WriteDataAsUint(expectedCrc32);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var crc32Task = Client.CalcFileCrc32(path);

        await _tcs.Task;

        var crc32 = await crc32Task;

        // Assert
        Assert.Equal(expectedCrc32, crc32);
    }

    [Fact]
    public async Task CreateDirectory_SingleDirectory_Success()
    {
        // Arrange
        var path = "/path/to/new_directory";

        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.CreateDirectory)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var requestedPath = packet.ReadDataAsString();

            Assert.Equal(path, requestedPath);

            var response = CreateAckResponse(packet, FtpOpcode.CreateDirectory);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var resultTask = Client.CreateDirectory(path);

        await _tcs.Task;

        var result = await resultTask;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task Rename_SingleFile_Success()
    {
        // Arrange
        var oldPath = "/path/to/old_name.txt";
        var newPath = "/path/to/new_name.txt";

        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.Rename)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var response = CreateAckResponse(packet, FtpOpcode.Rename);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var resultTask = Client.Rename(oldPath, newPath, CancellationToken.None);
        await _tcs.Task;

        var result = await resultTask;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }
    
    [Fact]
    public async Task Rename_SingleDirectory_Success()
    {
        // Arrange
        var oldPath = "/path/to/old_name";
        var newPath = "/path/to/new_name";

        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.Rename)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var response = CreateAckResponse(packet, FtpOpcode.Rename);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var resultTask = Client.Rename(oldPath, newPath, CancellationToken.None);
        await _tcs.Task;

        var result = await resultTask;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Theory]
    [InlineData("/path/to/directory")]
    [InlineData("/")] // Root
    public async Task ListDirectory_DifferentPaths_Success(string path)
    {
        // Arrange
        uint offset = 0;
        var directoryListing = "file1.txt\nfile2.txt\ndir1/\n";

        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.ListDirectory)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var requestedPath = packet.ReadDataAsString();
            var requestedOffset = packet.ReadOffset();

            Assert.Equal(path, requestedPath);
            Assert.Equal(offset, requestedOffset);

            var response = CreateAckResponse(packet, FtpOpcode.ListDirectory);
            response.WriteSize((byte)directoryListing.Length);
            response.WriteDataAsString(directoryListing);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var resultTask = Client.ListDirectory(path, offset);
        await _tcs.Task;

        var result = await resultTask;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
        var listing = result.ReadDataAsString();
        Assert.Equal(directoryListing, listing);
    }

    [Fact]
    public async Task OpenFileRead_SingleFile_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";
        var fileSize = 2048U;
        var sessionId = (byte)1;

        // Set up server response before starting client operation
        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.OpenFileRO)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var requestedPath = packet.ReadDataAsString();

            Assert.Equal(path, requestedPath);

            var response = CreateAckResponse(packet, FtpOpcode.OpenFileRO);
            response.WriteSession(sessionId);
            response.WriteSize(4);
            response.WriteDataAsUint(fileSize);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var handleTask = Client.OpenFileRead(path);

        // Wait for the client to receive and process the response
        await _tcs.Task;

        var handle = await handleTask;

        // Assert
        Assert.Equal(sessionId, handle.Session);
        Assert.Equal(fileSize, handle.Size);
    }

    [Fact]
    public async Task OpenFileWrite_SingleFile_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";
        var fileSize = 10U;
        var sessionId = (byte)2;

        // Set up server response before starting client operation
        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.OpenFileWO)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var requestedPath = packet.ReadDataAsString();

            Assert.Equal(path, requestedPath);

            var response = CreateAckResponse(packet, FtpOpcode.OpenFileWO);
            response.WriteSession(sessionId);
            response.WriteSize(4);
            response.WriteDataAsUint(fileSize);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var handleTask = Client.OpenFileWrite(path);

        // Wait for the client to receive and process the response
        await _tcs.Task;

        var handle = await handleTask;

        // Assert
        Assert.Equal(sessionId, handle.Session);
        Assert.Equal(fileSize, handle.Size);
    }

    [Fact]
    public async Task TerminateSession_SingleSession_Success()
    {
        // Arrange
        var sessionId = (byte)1;

        // Set up server response before starting client operation
        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.TerminateSession)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var requestedSessionId = packet.ReadSession();

            Assert.Equal(sessionId, requestedSessionId);

            var response = CreateAckResponse(packet, FtpOpcode.TerminateSession);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var terminateTask = Client.TerminateSession(sessionId);

        // Wait for the client to receive and process the response
        await _tcs.Task;

        await terminateTask;

        // Assert
        // No exception means success
    }

    [Fact]
    public async Task CreateFile_SingleFile_Success()
    {
        // Arrange
        var path = "/path/to/new_file.txt";

        // Set up server response before starting client operation
        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.CreateFile)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var requestedPath = packet.ReadDataAsString();

            Assert.Equal(path, requestedPath);

            var response = CreateAckResponse(packet, FtpOpcode.CreateFile);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var resultTask = Client.CreateFile(path);

        // Wait for the client to receive and process the response
        await _tcs.Task;

        var result = await resultTask;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task ReadFile_SingleFile_Success()
    {
        // Arrange
        var sessionId = (byte)1;
        var skip = 0U;
        var take = (byte)100;
        var request = new ReadRequest(sessionId, skip, take);
        var data = new byte[take];
        Random.Shared.NextBytes(data);

        // Set up server response
        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.ReadFile)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var requestedSessionId = packet.ReadSession();
            var requestedSkip = packet.ReadOffset();
            var requestedSize = packet.ReadSize();

            Assert.Equal(sessionId, requestedSessionId);
            Assert.Equal(skip, requestedSkip);
            Assert.Equal(take, requestedSize);

            var response = CreateAckResponse(packet, FtpOpcode.ReadFile);
            response.WriteSession(sessionId);
            response.WriteSize((byte)data.Length);
            response.WriteData(data);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var readTask = Client.ReadFile(request, CancellationToken.None);

        // Wait for the client to receive and process the response
        await _tcs.Task;

        var result = await readTask;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
        var size = result.ReadSize();
        var receivedData = result.Payload.Payload.AsSpan(12, size).ToArray();
        Assert.Equal(data, receivedData);
    }

    [Fact]
    public async Task WriteFile_SingleFile_Success()
    {
        // Arrange
        var sessionId = (byte)1;
        var skip = 0U;
        var take = (byte)100;
        var request = new WriteRequest(sessionId, skip, take);
        var data = new byte[take];
        Random.Shared.NextBytes(data);

        // Set up server response
        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.WriteFile)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var requestedSkip = packet.ReadOffset();
            var size = packet.ReadSize();
            var requestedData = packet.Payload.Payload.AsSpan(12, size).ToArray();

            Assert.Equal(skip, requestedSkip);
            Assert.Equal(data, requestedData);

            var response = CreateAckResponse(packet, FtpOpcode.WriteFile);

            await Link.Server.Send(response, ct);

            Time.Advance(TimeSpan.FromMilliseconds(10));

            _tcs.TrySetResult(response);
        });

        // Act
        var writeTask = Client.WriteFile(request, data, CancellationToken.None);

        // Wait for the client to receive and process the response
        await _tcs.Task;

        var result = await writeTask;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task BurstReadFile_SingleFile_Success()
    {
        // Arrange
        var sessionId = (byte)1;
        var skip = 0U;
        var take = (byte)100;
        var request = new ReadRequest(sessionId, skip, take);
        var dataChunks = new List<byte[]>
        {
            new byte[50],
            new byte[50]
        };
        Random.Shared.NextBytes(dataChunks[0]);
        Random.Shared.NextBytes(dataChunks[1]);

        var totalDataReceived = new List<byte>();

        // Set up server response
        using var sub = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() != FtpOpcode.BurstReadFile)
            {
                await Link.Server.Send(CreateNackResponse(packet), ct);
                return;
            }
            
            var requestedSessionId = packet.ReadSession();
            var requestedSkip = packet.ReadOffset();
            var requestedTake = packet.ReadSize();

            Assert.Equal(sessionId, requestedSessionId);
            Assert.Equal(skip, requestedSkip);
            Assert.Equal(take, requestedTake);

            // Simulate sending data chunks
            for (int i = 0; i < dataChunks.Count; i++)
            {
                var data = dataChunks[i];
                var response = CreateAckResponse(packet, FtpOpcode.BurstReadFile);
                response.WriteSession(sessionId);
                response.WriteSize((byte)data.Length);
                response.WriteData(data);
                response.WriteBurstComplete(i == dataChunks.Count - 1 ? (byte)1 : (byte)0);

                await Link.Server.Send(response, ct);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }

            _tcs.TrySetResult(packet);
        });

        // Act
        var burstReadTask = Client.BurstReadFile(request, packet =>
        {
            var size = packet.ReadSize();
            var data = packet.Payload.Payload.AsSpan(12, size).ToArray();
            totalDataReceived.AddRange(data);
        }, CancellationToken.None);

        // Wait for the client to receive and process the response
        await _tcs.Task;

        await burstReadTask;

        // Assert
        var expectedData = dataChunks.SelectMany(d => d).ToArray();
        Assert.Equal(expectedData, totalDataReceived.ToArray());
    }

    [Fact]
    public async Task ResetSessions_Cancel_Throws()
    {
        // Arrange
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => { await Client.ResetSessions(_cts.Token); });
    }

    [Fact]
    public async Task RemoveDirectory_Cancel_Throws()
    {
        // Arrange
        var path = "/path/to/directory";
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.RemoveDirectory(path, _cts.Token);
        });
    }

    [Fact]
    public async Task RemoveFile_Cancel_Throws()
    {
        // Arrange
        var path = "/path/to/file.txt";
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.RemoveFile(path, _cts.Token);
        });
    }

    [Fact]
    public async Task TruncateFile_Cancel_Throws()
    {
        // Arrange
        var path = "/path/to/file.txt";
        var request = new TruncateRequest(path, 1024);
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.TruncateFile(request, _cts.Token);
        });
    }

    [Fact]
    public async Task CalcFileCrc32_Cancel_Throws()
    {
        // Arrange
        var path = "/path/to/file.txt";
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.CalcFileCrc32(path, _cts.Token);
        });
    }

    [Fact]
    public async Task CreateDirectory_Cancel_Throws()
    {
        // Arrange
        var path = "/path/to/new_directory";
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.CreateDirectory(path, _cts.Token);
        });
    }

    [Fact]
    public async Task Rename_Cancel_Throws()
    {
        // Arrange
        var oldPath = "/path/to/old_name.txt";
        var newPath = "/path/to/new_name.txt";
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.Rename(oldPath, newPath, _cts.Token);
        });
    }

    [Fact]
    public async Task ListDirectory_Cancel_Throws()
    {
        // Arrange
        var path = "/path/to/directory";
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.ListDirectory(path, 0, _cts.Token);
        });
    }

    [Fact]
    public async Task OpenFileRead_Cancel_Throws()
    {
        // Arrange
        var path = "/path/to/file.txt";
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.OpenFileRead(path, _cts.Token);
        });
    }

    [Fact]
    public async Task OpenFileWrite_Cancel_Throws()
    {
        // Arrange
        var path = "/path/to/file.txt";
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.OpenFileWrite(path, _cts.Token);
        });
    }

    [Fact]
    public async Task TerminateSession_Cancel_Throws()
    {
        // Arrange
        var sessionId = (byte)1;
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.TerminateSession(sessionId, _cts.Token);
        });
    }

    [Fact]
    public async Task CreateFile_Cancel_Throws()
    {
        // Arrange
        var path = "/path/to/new_file.txt";
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.CreateFile(path, _cts.Token);
        });
    }

    [Fact]
    public async Task ReadFile_Cancel_Throws()
    {
        // Arrange
        var sessionId = (byte)1;
        var request = new ReadRequest(sessionId, 0, 100);
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.ReadFile(request, _cts.Token);
        });
    }

    [Fact]
    public async Task WriteFile_Cancel_Throws()
    {
        // Arrange
        var sessionId = (byte)1;
        var request = new WriteRequest(sessionId, 0, 100);
        var data = new byte[100];
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.WriteFile(request, data, _cts.Token);
        });
    }

    [Fact]
    public async Task BurstReadFile_Cancel_Throws()
    {
        // Arrange
        var sessionId = (byte)1;
        var request = new ReadRequest(sessionId, 0, 100);
        await _cts.CancelAsync();

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.BurstReadFile(request, _ => { }, _cts.Token);
        });
    }
}