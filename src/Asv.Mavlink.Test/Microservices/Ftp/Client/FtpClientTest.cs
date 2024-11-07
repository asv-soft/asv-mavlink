using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class FtpClientTest : ClientTestBase<FtpClient>
{
    private readonly MavlinkFtpClientConfig _config = new()
    {
        TimeoutMs = 1000,
        CommandAttemptCount = 5,
        TargetNetworkId = 0,
        BurstTimeoutMs = 1000
    };

    public FtpClientTest(ITestOutputHelper log) : base(log)
    {
    }

    protected override FtpClient CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new FtpClient(identity, _config, core);
    }

    private FileTransferProtocolPacket CreateAckResponse(FileTransferProtocolPacket requestPacket, FtpOpcode originOpCode)
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

    [Fact]
    public async Task ResetSessions_Success()
    {
        // Arrange
        var tcs = new TaskCompletionSource<FileTransferProtocolPacket>();

        // Set up server response before starting client operation
        Link.Server.Filter<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.ResetSessions)
            {
                // Prepare server response
                var response = CreateAckResponse(packet, FtpOpcode.ResetSessions);

                // Send the response asynchronously
                _ = Link.Server.Send(response, default);

                // Advance time to allow the client to process the response
                Time.Advance(TimeSpan.FromMilliseconds(10));

                tcs.TrySetResult(response);
            }
        });

        // Act
        var resultTask = Client.ResetSessions();

        // Wait for the client to receive and process the response
        await tcs.Task.ConfigureAwait(false);

        var result = await resultTask.ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task RemoveDirectory_Success()
    {
        // Arrange
        var path = "/path/to/directory";
        var tcs = new TaskCompletionSource<FileTransferProtocolPacket>();

        // Set up server response before starting client operation
        Link.Server.Filter<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.RemoveDirectory)
            {
                var requestedPath = packet.ReadDataAsString();
                Assert.Equal(path, requestedPath);

                var response = CreateAckResponse(packet, FtpOpcode.RemoveDirectory);

                _ = Link.Server.Send(response, default);

                Time.Advance(TimeSpan.FromMilliseconds(10));

                tcs.TrySetResult(response);
            }
        });

        // Act
        var resultTask = Client.RemoveDirectory(path);

        // Wait for the client to receive and process the response
        await tcs.Task.ConfigureAwait(false);

        var result = await resultTask.ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task RemoveFile_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";
        var tcs = new TaskCompletionSource<FileTransferProtocolPacket>();

        // Set up server response before starting client operation
        Link.Server.Filter<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.RemoveFile)
            {
                var requestedPath = packet.ReadDataAsString();
                Assert.Equal(path, requestedPath);

                var response = CreateAckResponse(packet, FtpOpcode.RemoveFile);

                _ = Link.Server.Send(response, default);

                Time.Advance(TimeSpan.FromMilliseconds(10));

                tcs.TrySetResult(response);
            }
        });

        // Act
        var resultTask = Client.RemoveFile(path);

        // Wait for the client to receive and process the response
        await tcs.Task.ConfigureAwait(false);

        var result = await resultTask.ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task TruncateFile_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";
        var offset = 1024U;
        var request = new TruncateRequest(path, offset);
        var tcs = new TaskCompletionSource<FileTransferProtocolPacket>();

        // Set up server response before starting client operation
        Link.Server.Filter<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.TruncateFile)
            {
                var requestedPath = packet.ReadDataAsString();
                var requestedOffset = packet.ReadOffset();

                Assert.Equal(path, requestedPath);
                Assert.Equal(offset, requestedOffset);

                var response = CreateAckResponse(packet, FtpOpcode.TruncateFile);

                _ = Link.Server.Send(response, default);

                Time.Advance(TimeSpan.FromMilliseconds(10));

                tcs.TrySetResult(response);
            }
        });

        // Act
        var resultTask = Client.TruncateFile(request);

        // Wait for the client to receive and process the response
        await tcs.Task.ConfigureAwait(false);

        var result = await resultTask.ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task CalcFileCrc32_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";
        var expectedCrc32 = 0xDEADBEEF;
        var tcs = new TaskCompletionSource<FileTransferProtocolPacket>();

        // Set up server response before starting client operation
        Link.Server.Filter<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.CalcFileCRC32)
            {
                var requestedPath = packet.ReadDataAsString();

                Assert.Equal(path, requestedPath);

                var response = CreateAckResponse(packet, FtpOpcode.CalcFileCRC32);
                response.WriteSize(4);
                response.WriteDataAsUint(expectedCrc32);

                _ = Link.Server.Send(response, default);

                Time.Advance(TimeSpan.FromMilliseconds(10));

                tcs.TrySetResult(response);
            }
        });

        // Act
        var crc32Task = Client.CalcFileCrc32(path);

        // Wait for the client to receive and process the response
        await tcs.Task.ConfigureAwait(false);

        var crc32 = await crc32Task.ConfigureAwait(false);

        // Assert
        Assert.Equal(expectedCrc32, crc32);
    }

    [Fact]
    public async Task CreateDirectory_Success()
    {
        // Arrange
        var path = "/path/to/new_directory";
        var tcs = new TaskCompletionSource<FileTransferProtocolPacket>();

        // Set up server response before starting client operation
        Link.Server.Filter<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.CreateDirectory)
            {
                var requestedPath = packet.ReadDataAsString();

                Assert.Equal(path, requestedPath);

                var response = CreateAckResponse(packet, FtpOpcode.CreateDirectory);

                _ = Link.Server.Send(response, default);

                Time.Advance(TimeSpan.FromMilliseconds(10));

                tcs.TrySetResult(response);
            }
        });

        // Act
        var resultTask = Client.CreateDirectory(path);

        // Wait for the client to receive and process the response
        await tcs.Task.ConfigureAwait(false);

        var result = await resultTask.ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task Rename_Success()
    {
        // Arrange
        var oldPath = "/path/to/old_name.txt";
        var newPath = "/path/to/new_name.txt";
        var tcs = new TaskCompletionSource<FileTransferProtocolPacket>();

        // Set up server response before starting client operation
        Link.Server.Filter<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.Rename)
            {
                var response = CreateAckResponse(packet, FtpOpcode.Rename);

                _ = Link.Server.Send(response, default);

                Time.Advance(TimeSpan.FromMilliseconds(10));

                tcs.TrySetResult(response);
            }
        });

        // Act
        var resultTask = Client.Rename(oldPath, newPath, CancellationToken.None);

        // Wait for the client to receive and process the response
        await tcs.Task.ConfigureAwait(false);

        var result = await resultTask.ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task ListDirectory_Success()
    {
        // Arrange
        var path = "/path/to/directory";
        uint offset = 0;
        var tcs = new TaskCompletionSource<FileTransferProtocolPacket>();
        var directoryListing = "file1.txt\nfile2.txt\ndir1/\n";

        // Set up server response before starting client operation
        Link.Server.Filter<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.ListDirectory)
            {
                var requestedPath = packet.ReadDataAsString();
                var requestedOffset = packet.ReadOffset();

                Assert.Equal(path, requestedPath);
                Assert.Equal(offset, requestedOffset);

                var response = CreateAckResponse(packet, FtpOpcode.ListDirectory);
                response.WriteSize((byte)directoryListing.Length);
                response.WriteDataAsString(directoryListing);

                _ = Link.Server.Send(response, default);

                Time.Advance(TimeSpan.FromMilliseconds(10));

                tcs.TrySetResult(response);
            }
        });

        // Act
        var resultTask = Client.ListDirectory(path, offset);

        // Wait for the client to receive and process the response
        await tcs.Task.ConfigureAwait(false);

        var result = await resultTask.ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
        var listing = result.ReadDataAsString();
        Assert.Equal(directoryListing, listing);
    }

    [Fact]
    public async Task OpenFileRead_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";
        var fileSize = 2048U;
        var tcs = new TaskCompletionSource<FileTransferProtocolPacket>();
        var sessionId = (byte)1;

        // Set up server response before starting client operation
        Link.Server.Filter<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.OpenFileRO)
            {
                var requestedPath = packet.ReadDataAsString();

                Assert.Equal(path, requestedPath);

                var response = CreateAckResponse(packet, FtpOpcode.OpenFileRO);
                response.WriteSession(sessionId);
                response.WriteSize(4);
                response.WriteDataAsUint(fileSize);

                _ = Link.Server.Send(response, default);

                Time.Advance(TimeSpan.FromMilliseconds(10));

                tcs.TrySetResult(response);
            }
        });

        // Act
        var handleTask = Client.OpenFileRead(path);

        // Wait for the client to receive and process the response
        await tcs.Task.ConfigureAwait(false);

        var handle = await handleTask.ConfigureAwait(false);

        // Assert
        Assert.NotNull(handle);
        Assert.Equal(sessionId, handle.Session);
        Assert.Equal(fileSize, handle.Size);
    }

    [Fact]
    public async Task OpenFileWrite_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";
        var fileSize = 0U;
        var tcs = new TaskCompletionSource<FileTransferProtocolPacket>();
        var sessionId = (byte)2;

        // Set up server response before starting client operation
        Link.Server.Filter<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.OpenFileWO)
            {
                var requestedPath = packet.ReadDataAsString();

                Assert.Equal(path, requestedPath);

                var response = CreateAckResponse(packet, FtpOpcode.OpenFileWO);
                response.WriteSession(sessionId);
                response.WriteSize(4);
                response.WriteDataAsUint(fileSize);

                _ = Link.Server.Send(response, default);

                Time.Advance(TimeSpan.FromMilliseconds(10));

                tcs.TrySetResult(response);
            }
        });

        // Act
        var handleTask = Client.OpenFileWrite(path);

        // Wait for the client to receive and process the response
        await tcs.Task.ConfigureAwait(false);

        var handle = await handleTask.ConfigureAwait(false);

        // Assert
        Assert.NotNull(handle);
        Assert.Equal(sessionId, handle.Session);
        Assert.Equal(fileSize, handle.Size);
    }

    [Fact]
    public async Task TerminateSession_Success()
    {
        // Arrange
        var sessionId = (byte)1;
        var tcs = new TaskCompletionSource<FileTransferProtocolPacket>();

        // Set up server response before starting client operation
        Link.Server.Filter<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.TerminateSession)
            {
                var requestedSessionId = packet.ReadSession();

                Assert.Equal(sessionId, requestedSessionId);

                var response = CreateAckResponse(packet, FtpOpcode.TerminateSession);

                _ = Link.Server.Send(response, default);

                Time.Advance(TimeSpan.FromMilliseconds(10));

                tcs.TrySetResult(response);
            }
        });

        // Act
        var terminateTask = Client.TerminateSession(sessionId);

        // Wait for the client to receive and process the response
        await tcs.Task.ConfigureAwait(false);

        await terminateTask.ConfigureAwait(false);

        // Assert
        // No exception means success
    }

        [Fact]
    public async Task CreateFile_Success()
    {
        // Arrange
        var path = "/path/to/new_file.txt";
        var tcs = new TaskCompletionSource<FileTransferProtocolPacket>();

        // Set up server response before starting client operation
        Link.Server.Filter<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.CreateFile)
            {
                var requestedPath = packet.ReadDataAsString();

                Assert.Equal(path, requestedPath);

                var response = CreateAckResponse(packet, FtpOpcode.CreateFile);

                _ = Link.Server.Send(response, default);

                Time.Advance(TimeSpan.FromMilliseconds(10));

                tcs.TrySetResult(response);
            }
        });

        // Act
        var resultTask = Client.CreateFile(path);

        // Wait for the client to receive and process the response
        await tcs.Task.ConfigureAwait(false);

        var result = await resultTask.ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }
}
