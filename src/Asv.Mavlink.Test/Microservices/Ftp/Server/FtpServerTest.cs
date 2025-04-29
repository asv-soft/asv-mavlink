using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(FtpServer))]
public class FtpServerTest(ITestOutputHelper log) : ServerTestBase<FtpServer>(log)
{
    private readonly MavlinkFtpServerConfig _config = new()
    {
        NetworkId = 0,
        BurstReadChunkDelayMs = 100
    };

    protected override FtpServer CreateServer(MavlinkIdentity identity, CoreServices core)
    {
        return new FtpServer(identity, _config, core);
    }

    [Fact]
    public async Task CalcFileCrc32_NormalData_Success()
    {
        // Arrange
        const string expectedPath = "/path/to/file.txt";
        const uint expectedCrc32 = 0xDEADBEEF;
        var requestedPath = string.Empty;

        Server.CalcFileCrc32 = async (path, _) =>
        {
            requestedPath = path;
            return await Task.FromResult(expectedCrc32);
        };

        var requestPacket = CreateRequestPacket(FtpOpcode.CalcFileCRC32);
        requestPacket.WriteSize((byte)expectedPath.Length);
        requestPacket.WriteDataAsString(expectedPath);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Client.Send(requestPacket);

        // Assert
        Assert.NotNull(responsePacket);
        Assert.Equal(FtpOpcode.Ack, responsePacket!.ReadOpcode());
        var crc32 = responsePacket!.ReadDataAsUint();
        Assert.Equal(expectedCrc32, crc32);
        Assert.Equal(expectedPath, requestedPath);
    }

    [Fact]
    public async Task TruncateFile_NormalData_Success()
    {
        // Arrange
        const string expectedPath = "/path/to/file.txt";
        const uint expectedOffset = 1024U;
        var requestedPath = string.Empty;
        var requestedOffset = 0U;

        Server.TruncateFile = async (request, _) =>
        {
            requestedPath = request.Path;
            requestedOffset = request.Offset;
            await Task.CompletedTask;
        };

        var requestPacket = CreateRequestPacket(FtpOpcode.TruncateFile);
        requestPacket.WriteOffset(expectedOffset);
        requestPacket.WriteSize((byte)expectedPath.Length);
        requestPacket.WriteDataAsString(expectedPath);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Client.Send(requestPacket);

        // Assert
        Assert.NotNull(responsePacket);
        Assert.Equal(FtpOpcode.Ack, responsePacket!.ReadOpcode());
        Assert.Equal(expectedPath, requestedPath);
        Assert.Equal(expectedOffset, requestedOffset);
    }

    [Fact]
    public async Task RemoveDirectory_NormalData_Success()
    {
        // Arrange
        const string expectedPath = "/path/to/directory";
        var requestedPath = string.Empty;

        Server.RemoveDirectory = async (path, _) =>
        {
            requestedPath = path;
            await Task.CompletedTask;
        };

        var requestPacket = CreateRequestPacket(FtpOpcode.RemoveDirectory);
        requestPacket.WriteSize((byte)expectedPath.Length);
        requestPacket.WriteDataAsString(expectedPath);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Client.Send(requestPacket);

        // Assert
        Assert.NotNull(responsePacket);
        Assert.Equal(FtpOpcode.Ack, responsePacket!.ReadOpcode());
        Assert.Equal(expectedPath, requestedPath);
    }

    [Fact]
    public async Task RemoveFile_NormalData_Success()
    {
        // Arrange
        const string expectedPath = "/path/to/file.txt";
        var requestedPath = string.Empty;

        Server.RemoveFile = async (path, _) =>
        {
            requestedPath = path;
            await Task.CompletedTask;
        };

        var requestPacket = CreateRequestPacket(FtpOpcode.RemoveFile);
        requestPacket.WriteSize((byte)expectedPath.Length);
        requestPacket.WriteDataAsString(expectedPath);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Client.Send(requestPacket);

        // Assert
        Assert.NotNull(responsePacket);
        Assert.Equal(FtpOpcode.Ack, responsePacket!.ReadOpcode());
        Assert.Equal(expectedPath, requestedPath);
    }

    [Fact]
    public async Task ResetSessions_NormalData_Success()
    {
        // Arrange
        var resetCalled = false;

        Server.ResetSessions = async _ =>
        {
            resetCalled = true;
            await Task.CompletedTask;
        };

        var requestPacket = CreateRequestPacket(FtpOpcode.ResetSessions);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Client.Send(requestPacket);

        // Assert
        Assert.True(resetCalled);
        Assert.NotNull(responsePacket);
        Assert.Equal(FtpOpcode.Ack, responsePacket!.ReadOpcode());
    }

    [Fact]
    public async Task CreateDirectory_NormalData_Success()
    {
        // Arrange
        const string expectedPath = "/path/to/new_directory";
        var requestedPath = string.Empty;

        Server.CreateDirectory = async (path, _) =>
        {
            requestedPath = path;
            await Task.CompletedTask;
        };

        var requestPacket = CreateRequestPacket(FtpOpcode.CreateDirectory);
        requestPacket.WriteSize((byte)expectedPath.Length);
        requestPacket.WriteDataAsString(expectedPath);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Client.Send(requestPacket);

        // Assert
        Assert.NotNull(responsePacket);
        Assert.Equal(FtpOpcode.Ack, responsePacket!.ReadOpcode());
        Assert.Equal(expectedPath, requestedPath);
    }

    [Fact]
    public async Task Rename_NormalData_Success()
    {
        // Arrange
        const string expectedOldPath = "/path/to/old_name.txt";
        const string expectedNewPath = "/path/to/new_name.txt";
        var requestedOldPath = string.Empty;
        var requestedNewPath = string.Empty;

        Server.Rename = async (path1, path2, _) =>
        {
            requestedOldPath = path1;
            requestedNewPath = path2;
            await Task.CompletedTask;
        };

        var requestPacket = CreateRequestPacket(FtpOpcode.Rename);
        requestPacket.WriteSize((byte)(expectedOldPath.Length + expectedNewPath.Length));
        requestPacket.WriteDataAsString(expectedOldPath + '\0' + expectedNewPath);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Client.Send(requestPacket);

        // Assert
        Assert.NotNull(responsePacket);
        Assert.Equal(FtpOpcode.Ack, responsePacket!.ReadOpcode());
        Assert.Equal(expectedOldPath, requestedOldPath);
        Assert.Equal(expectedNewPath, requestedNewPath);
    }

    [Fact]
    public async Task ListDirectory_NormalData_Success()
    {
        // Arrange
        const string expectedPath = "/path/to/directory";
        const uint expectedOffset = 0;
        var requestedPath = string.Empty;
        var requestedOffset = 0U;
        const string expectedDirectoryListing = "Ffile1.txt\t123\nFfile2.txt\t456\nDdir1/\n";
        var listingChars = expectedDirectoryListing.ToCharArray();

        Server.ListDirectory = (path, offset, buffer, _) =>
        {
            requestedPath = path;
            requestedOffset = offset;
            listingChars.CopyTo(buffer.Span);
            return Task.FromResult((byte)listingChars.Length);
        };

        var requestPacket = CreateRequestPacket(FtpOpcode.ListDirectory);
        requestPacket.WriteOffset(expectedOffset);
        requestPacket.WriteSize((byte)expectedPath.Length);
        requestPacket.WriteDataAsString(expectedPath);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Client.Send(requestPacket);

        // Assert
        Assert.NotNull(responsePacket);
        Assert.Equal(FtpOpcode.Ack, responsePacket!.ReadOpcode());
        var listing = responsePacket!.ReadDataAsString();
        Assert.Equal(expectedDirectoryListing, listing);

        Assert.Equal(expectedPath, requestedPath);
        Assert.Equal(expectedOffset, requestedOffset);
    }

    [Fact]
    public async Task FileRead_NormalData_Success()
    {
        // Arrange
        const byte expectedSessionId = 1;
        const uint expectedSkip = 0U;
        const byte expectedTake = 100;
        var requestedSessionId = (byte)0;
        var requestedSkip = 0U;
        var requestedTake = (byte)0;

        var data = new byte[expectedTake];
        new Random().NextBytes(data);

        Server.FileRead = async (request, buffer, _) =>
        {
            requestedSessionId = request.Session;
            requestedSkip = request.Skip;
            requestedTake = request.Take;
            data.CopyTo(buffer.Span);
            return await Task.FromResult(new ReadResult(request.Take, request));
        };

        var requestPacket = CreateRequestPacket(FtpOpcode.ReadFile);
        requestPacket.WriteSession(expectedSessionId);
        requestPacket.WriteOffset(expectedSkip);
        requestPacket.WriteSize(expectedTake);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Client.Send(requestPacket);

        // Assert
        Assert.NotNull(responsePacket);
        Assert.Equal(FtpOpcode.Ack, responsePacket!.ReadOpcode());

        var actualSessionId = responsePacket!.ReadSession();
        var actualSize = responsePacket!.ReadSize();
        var actualOffset = responsePacket!.ReadOffset();
        var receivedData = responsePacket!.Payload.Payload.AsSpan(12, actualSize).ToArray();

        Assert.Equal(expectedSessionId, actualSessionId);
        Assert.Equal(expectedTake, actualSize);
        Assert.Equal(expectedSkip, actualOffset);
        Assert.Equal(data, receivedData);

        Assert.Equal(expectedSessionId, requestedSessionId);
        Assert.Equal(expectedSkip, requestedSkip);
        Assert.Equal(expectedTake, requestedTake);
    }

    [Fact]
    public async Task OpenFileRead_NormalData_Success()
    {
        // Arrange
        const string expectedPath = "/path/to/file.txt";
        const byte expectedSessionId = 1;
        const uint expectedFileSize = 2048U;
        var requestedPath = string.Empty;

        Server.OpenFileRead = async (path, _) =>
        {
            requestedPath = path;
            return await Task.FromResult(new ReadHandle(expectedSessionId, expectedFileSize));
        };

        var requestPacket = CreateRequestPacket(FtpOpcode.OpenFileRO);
        requestPacket.WriteSession(expectedSessionId);
        requestPacket.WriteSize((byte)expectedPath.Length);
        requestPacket.WriteDataAsString(expectedPath);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Client.Send(requestPacket);

        // Assert
        Assert.NotNull(responsePacket);
        Assert.Equal(FtpOpcode.Ack, responsePacket!.ReadOpcode());

        var actualSessionId = responsePacket!.ReadSession();
        var actualFileSize = responsePacket!.ReadDataAsUint();

        Assert.Equal(expectedSessionId, actualSessionId);
        Assert.Equal(expectedFileSize, actualFileSize);
        Assert.Equal(expectedPath, requestedPath);
    }

    [Fact]
    public async Task OpenFileWrite_NormalData_Success()
    {
        // Arrange
        const string expectedPath = "/path/to/file.txt";
        const byte expectedSessionId = 2;
        const uint expectedFileSize = 17U;


        Server.OpenFileWrite = async (_, _) => await Task.FromResult(new WriteHandle(expectedSessionId, expectedFileSize));

        var requestPacket = CreateRequestPacket(FtpOpcode.OpenFileWO);
        requestPacket.WriteSession(expectedSessionId);
        requestPacket.WriteSize((byte)expectedFileSize);
        requestPacket.WriteDataAsString(expectedPath);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Server.Send(requestPacket);

        // Assert
        Assert.NotNull(responsePacket);

        var actualSize = responsePacket!.ReadSize();
        var actualPath = responsePacket!.ReadDataAsString();
        Assert.Equal(expectedFileSize, actualSize);
        Assert.Equal(expectedPath, actualPath);
    }

    [Fact]
    public async Task TerminateSession_NormalData_Success()
    {
        // Arrange
        const byte expectedSessionId = 1;
        var requestedSessionId = (byte)0;

        Server.TerminateSession = async (sessionId, _) =>
        {
            requestedSessionId = sessionId;
            await Task.CompletedTask;
        };

        var requestPacket = CreateRequestPacket(FtpOpcode.TerminateSession);
        requestPacket.WriteSession(expectedSessionId);
        requestPacket.WriteSize(0);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Server.Send(requestPacket);

        // Assert
        Assert.NotNull(responsePacket);
        requestedSessionId = responsePacket!.ReadSession();
        Assert.Equal(expectedSessionId, requestedSessionId);
    }

    [Fact]
    public async Task WriteFile_NormalData_Success()
    {
        // Arrange
        const byte expectedSessionId = 1;
        const uint expectedOffset = 0U;
        const byte expectedSize = 100;

        var requestedSessionId = (byte)0;
        var requestedSkip = 0U;
        var requestedTake = (byte)0;

        var data = new byte[expectedSize];
        new Random().NextBytes(data);

        Server.WriteFile = async (request, buffer, _) =>
        {
            requestedSessionId = request.Session;
            requestedSkip = request.Skip;
            requestedTake = request.Take;
            Assert.Equal(data, buffer.Span[..expectedSize].ToArray());
            await Task.CompletedTask;
        };

        var requestPacket = CreateRequestPacket(FtpOpcode.WriteFile);
        requestPacket.WriteSession(expectedSessionId);
        requestPacket.WriteOffset(expectedOffset);
        requestPacket.WriteSize(expectedSize);
        requestPacket.WriteData(data);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Server.Send(requestPacket);

        // Assert
        Assert.NotNull(responsePacket);
        requestedSessionId = responsePacket!.ReadSession();
        requestedSkip = responsePacket!.ReadOffset();
        requestedTake = responsePacket!.ReadSize();
        Assert.Equal(expectedSessionId, requestedSessionId);
        Assert.Equal(expectedOffset, requestedSkip);
        Assert.Equal(expectedSize, requestedTake);
    }

    [Fact]
    public async Task BurstReadFile_NormalData_Success()
    {
        // Arrange
        const byte expectedSessionId = 1;
        const uint expectedOffset = 0U;
        const byte expectedSize = 100;

        var dataChunks = new byte[expectedSize];
        new Random().NextBytes(dataChunks);

        var originStream = new MemoryStream(dataChunks);

        Server.BurstReadFile = async (request, buffer, cancel) =>
        {
            if (originStream.Length == 0 || request.Skip >= originStream.Length)
            {
                return new BurstReadResult(0, true, request);
            }

            originStream.Position = request.Skip;
            var bytesToRead = (int)Math.Min(request.Take, originStream.Length - request.Skip);
            var readBytes = await originStream.ReadAsync(buffer[..bytesToRead], cancel).ConfigureAwait(false);
            var eof = originStream.Position >= originStream.Length;
            return new BurstReadResult((byte)readBytes, eof, request);
        };

        var requestPacket = CreateRequestPacket(FtpOpcode.BurstReadFile);
        requestPacket.WriteSession(expectedSessionId);
        requestPacket.WriteOffset(expectedOffset);
        requestPacket.WriteSize(expectedSize);
        requestPacket.WriteData(dataChunks);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Server.Send(requestPacket);

        // Assert
        Assert.NotNull(responsePacket);
        Assert.Equal(FtpOpcode.BurstReadFile, responsePacket!.ReadOpcode());

        var mb = new ArrayBufferWriter<byte>();
        responsePacket!.ReadData(mb);
        var resultData = mb.WrittenMemory.ToArray();
        Assert.Equal(dataChunks, resultData);
        
        await originStream.DisposeAsync();
    }

    [Fact]
    public async Task CreateFile_NormalData_Success()
    {
        // Arrange
        const string expectedPath = "/path/to/new_file.txt";
        const byte expectedSessionId = 1;
        var requestedPath = string.Empty;

        Server.CreateFile = async (path, _) =>
        {
            requestedPath = path;
            return await Task.FromResult(expectedSessionId);
        };

        var requestPacket = CreateRequestPacket(FtpOpcode.CreateFile);
        requestPacket.WriteSize(0);
        requestPacket.WriteDataAsString(expectedPath);

        FileTransferProtocolPacket? responsePacket = null;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(packet =>
            {
                responsePacket = packet;
            });

        // Act
        await Link.Server.Send(requestPacket);

        // Assert
        Assert.NotNull(responsePacket);
        requestedPath = responsePacket!.ReadDataAsString();
        Assert.Equal(expectedPath, requestedPath);
    }

    [Fact]
    public async Task FtpPacketSend_Cancel_ThrowsByCancellation()
    {
        // Arrange
        using var cts = new System.Threading.CancellationTokenSource();
        await cts.CancelAsync();

        var requestPacket = CreateRequestPacket(FtpOpcode.CreateFile);
        var called = 0;

        using var subscription = Link.Client
            .RxFilterByType<FileTransferProtocolPacket>()
            .Take(1)
            .Subscribe(_ => called++);
        
        // Act
        var task = Link.Server.Send(requestPacket, cts.Token);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await task;
        });
        Assert.Equal(0, called);
    }

    private FileTransferProtocolPacket CreateRequestPacket(FtpOpcode opcode)
    {
        var pkt = new FileTransferProtocolPacket
        {
            SystemId = Identity.SystemId,
            ComponentId = Identity.ComponentId,
            Sequence = 1,
            Payload =
            {
                TargetSystem = Identity.SystemId,
                TargetComponent = Identity.ComponentId,
                TargetNetwork = _config.NetworkId,
                Payload = new byte[251],
            }
        };
        pkt.WriteOpcode(opcode);
        pkt.WriteSession(0);
        pkt.WriteSize(0);
        return pkt;
    }
}