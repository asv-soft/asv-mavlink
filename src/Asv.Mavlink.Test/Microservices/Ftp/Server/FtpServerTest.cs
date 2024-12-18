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

[TestSubject(typeof(FtpServer))]
public class FtpServerTest : ServerTestBase<FtpServer>
{
    private readonly TaskCompletionSource<FileTransferProtocolPacket> _tcs = new();
    private readonly CancellationTokenSource _cts;

    private readonly MavlinkFtpServerConfig _config = new()
    {
        NetworkId = 0,
        BurstReadChunkDelayMs = 100
    };

    public FtpServerTest(ITestOutputHelper log) : base(log)
    {
        _cts = new CancellationTokenSource();
        _cts.Token.Register(() => _tcs.TrySetCanceled());
    }

    protected override FtpServer CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        return new FtpServer(identity, _config, core);
    }

    [Fact]
    public async Task CalcFileCrc32_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";
        var expectedCrc32 = 0xDEADBEEF;

        Server.CalcFileCrc32 = async (requestedPath, _) =>
        {
            Assert.Equal(path, requestedPath);
            return await Task.FromResult(expectedCrc32);
        };

        // Simulate client request
        var requestPacket = new FileTransferProtocolPacket
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
        requestPacket.WriteOpcode(FtpOpcode.CalcFileCRC32);
        requestPacket.WriteSession(0);
        requestPacket.WriteSize((byte)path.Length);
        requestPacket.WriteDataAsString(path);

        // Set up server response
        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet => { _tcs.TrySetResult(packet); });

        // Act
        await Link.Client.Send(requestPacket, _cts.Token).ConfigureAwait(false);

        // Wait for the server to process and client to receive the response
        var response = await _tcs.Task.ConfigureAwait(false);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        var crc32 = response.ReadDataAsUint();
        Assert.Equal(expectedCrc32, crc32);
    }

    [Fact]
    public async Task TruncateFile_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";
        var offset = 1024U;

        Server.TruncateFile = async (request, _) =>
        {
            Assert.Equal(path, request.Path);
            Assert.Equal(offset, request.Offset);
            await Task.CompletedTask;
        };

        // Simulate client request
        var requestPacket = new FileTransferProtocolPacket
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
        requestPacket.WriteOpcode(FtpOpcode.TruncateFile);
        requestPacket.WriteSession(0);
        requestPacket.WriteOffset(offset);
        requestPacket.WriteSize((byte)path.Length);
        requestPacket.WriteDataAsString(path);

        // Set up server response
        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet => { _tcs.TrySetResult(packet); });

        // Act
        await Link.Client.Send(requestPacket, _cts.Token).ConfigureAwait(false);

        // Wait for the server to process and client to receive the response
        var response = await _tcs.Task.ConfigureAwait(false);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
    }

    [Fact]
    public async Task RemoveDirectory_Success()
    {
        // Arrange
        var path = "/path/to/directory";

        Server.RemoveDirectory = async (requestedPath, _) =>
        {
            Assert.Equal(path, requestedPath);
            await Task.CompletedTask;
        };

        // Simulate client request
        var requestPacket = new FileTransferProtocolPacket
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
        requestPacket.WriteOpcode(FtpOpcode.RemoveDirectory);
        requestPacket.WriteSession(0);
        requestPacket.WriteSize((byte)path.Length);
        requestPacket.WriteDataAsString(path);

        // Set up server response
        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet => { _tcs.TrySetResult(packet); });

        // Act
        await Link.Client.Send(requestPacket, _cts.Token).ConfigureAwait(false);

        // Wait for the server to process and client to receive the response
        var response = await _tcs.Task.ConfigureAwait(false);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
    }

    [Fact]
    public async Task RemoveFile_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";

        Server.RemoveFile = async (requestedPath, _) =>
        {
            Assert.Equal(path, requestedPath);
            await Task.CompletedTask;
        };

        // Simulate client request
        var requestPacket = new FileTransferProtocolPacket
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
        requestPacket.WriteOpcode(FtpOpcode.RemoveFile);
        requestPacket.WriteSession(0);
        requestPacket.WriteSize((byte)path.Length);
        requestPacket.WriteDataAsString(path);

        // Set up server response
        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet => { _tcs.TrySetResult(packet); });

        // Act
        await Link.Client.Send(requestPacket, _cts.Token).ConfigureAwait(false);

        // Wait for the server to process and client to receive the response
        var response = await _tcs.Task.ConfigureAwait(false);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
    }

    [Fact]
    public async Task ResetSessions_Success()
    {
        // Arrange
        Server.ResetSessions = async _ => { await Task.CompletedTask; };

        // Simulate client request
        var requestPacket = new FileTransferProtocolPacket
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
        requestPacket.WriteOpcode(FtpOpcode.ResetSessions);
        requestPacket.WriteSession(0);
        requestPacket.WriteSize(0);

        // Set up server response
        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet => { _tcs.TrySetResult(packet); });

        // Act
        await Link.Client.Send(requestPacket, _cts.Token).ConfigureAwait(false);

        // Wait for the server to process and client to receive the response
        var response = await _tcs.Task.ConfigureAwait(false);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
    }

    [Fact]
    public async Task CreateDirectory_Success()
    {
        // Arrange
        var path = "/path/to/new_directory";

        Server.CreateDirectory = async (requestedPath, _) =>
        {
            Assert.Equal(path, requestedPath);
            await Task.CompletedTask;
        };

        // Simulate client request
        var requestPacket = new FileTransferProtocolPacket
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
        requestPacket.WriteOpcode(FtpOpcode.CreateDirectory);
        requestPacket.WriteSession(0);
        requestPacket.WriteSize((byte)path.Length);
        requestPacket.WriteDataAsString(path);

        // Set up server response
        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet => { _tcs.TrySetResult(packet); });

        // Act
        await Link.Client.Send(requestPacket, _cts.Token).ConfigureAwait(false);

        // Wait for the server to process and client to receive the response
        var response = await _tcs.Task.ConfigureAwait(false);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
    }

    [Fact]
    public async Task Rename_Success()
    {
        // Arrange
        var oldPath = "/path/to/old_name.txt";
        var newPath = "/path/to/new_name.txt";

        var requestPacket = new FileTransferProtocolPacket
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

        Server.Rename = async (path1, path2, _) =>
        {
            Assert.Equal(oldPath, path1);
            Assert.Equal(newPath, path2);
            await Task.CompletedTask;
        };

        requestPacket.WriteOpcode(FtpOpcode.Rename);
        requestPacket.WriteSession(0);
        requestPacket.WriteSize((byte)(oldPath.Length + newPath.Length));
        requestPacket.WriteDataAsString(oldPath + '\0' + newPath);

        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet => { _tcs.TrySetResult(packet); });

        // Act
        await Link.Client.Send(requestPacket, _cts.Token).ConfigureAwait(false);

        var response = await _tcs.Task.ConfigureAwait(false);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
    }

    [Fact]
    public async Task ListDirectory_Success()
    {
        // Arrange
        var path = "/path/to/directory";
        uint offset = 0;
        var directoryListing = "Ffile1.txt\t123\nFfile2.txt\t456\nDdir1/\n";
        var listingChars = directoryListing.ToCharArray();

        Server.ListDirectory = async (requestedPath, requestedOffset, buffer, _) =>
        {
            Assert.Equal(path, requestedPath);
            Assert.Equal(offset, requestedOffset);
            listingChars.CopyTo(buffer.Span);
            return (byte)listingChars.Length;
        };

        // Simulate client request
        var requestPacket = new FileTransferProtocolPacket
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
        requestPacket.WriteOpcode(FtpOpcode.ListDirectory);
        requestPacket.WriteSession(0);
        requestPacket.WriteOffset(offset);
        requestPacket.WriteSize((byte)path.Length);
        requestPacket.WriteDataAsString(path);

        // Set up server response
        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet => { _tcs.TrySetResult(packet); });

        // Act
        await Link.Client.Send(requestPacket, _cts.Token).ConfigureAwait(false);

        // Wait for the server to process and client to receive the response
        var response = await _tcs.Task.ConfigureAwait(false);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        var listing = response.ReadDataAsString();
        Assert.Equal(directoryListing, listing);
    }

    [Fact]
    public async Task FileRead_Success()
    {
        // Arrange
        var sessionId = (byte)1;
        var offset = 0U;
        var size = (byte)100;
        var data = new byte[size];
        new Random().NextBytes(data);

        Server.FileRead = async (request, buffer, _) =>
        {
            Assert.Equal(sessionId, request.Session);
            Assert.Equal(offset, request.Skip);
            Assert.Equal(size, request.Take);
            data.CopyTo(buffer.Span);
            return await Task.FromResult(new ReadResult(size, request));
        };

        // Simulate client request
        var requestPacket = new FileTransferProtocolPacket
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
        requestPacket.WriteOpcode(FtpOpcode.ReadFile);
        requestPacket.WriteSession(sessionId);
        requestPacket.WriteOffset(offset);
        requestPacket.WriteSize(size);

        // Set up server response
        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet => { _tcs.TrySetResult(packet); });

        // Act
        await Link.Client.Send(requestPacket, _cts.Token).ConfigureAwait(false);

        // Wait for the server to process and client to receive the response
        var response = await _tcs.Task.ConfigureAwait(false);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        var responseSessionId = response.ReadSession();
        var responseSize = response.ReadSize();
        var responseOffset = response.ReadOffset();
        var receivedData = response.Payload.Payload.AsSpan(12, responseSize).ToArray();
        Assert.Equal(sessionId, responseSessionId);
        Assert.Equal(size, responseSize);
        Assert.Equal(offset, responseOffset);
        Assert.Equal(data, receivedData);
    }

    [Fact]
    public async Task OpenFileRead_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";
        var sessionId = (byte)1;
        var fileSize = 2048U;

        Server.OpenFileRead = async (requestedPath, _) =>
        {
            Assert.Equal(path, requestedPath);
            return await Task.FromResult(new ReadHandle(sessionId, fileSize));
        };

        // Simulate client request
        var requestPacket = new FileTransferProtocolPacket
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
        requestPacket.WriteOpcode(FtpOpcode.OpenFileRO);
        requestPacket.WriteSession(0);
        requestPacket.WriteSize((byte)path.Length);
        requestPacket.WriteDataAsString(path);

        // Set up server response
        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet => { _tcs.TrySetResult(packet); });

        // Act
        await Link.Client.Send(requestPacket, _cts.Token).ConfigureAwait(false);

        // Wait for the server to process and client to receive the response
        var response = await _tcs.Task.ConfigureAwait(false);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        response.ReadSession();
        var responseFileSize = response.ReadDataAsUint();
        Assert.Equal(fileSize, responseFileSize);
    }

    [Fact]
    public async Task OpenFileWrite_Success()
    {
        // Arrange
        var path = "/path/to/file.txt";
        var sessionId = (byte)2;
        var fileSize = 17U;

        Server.OpenFileWrite = async (requestedPath, _) =>
        {
            Assert.Equal(path, requestedPath);
            return await Task.FromResult(new WriteHandle(sessionId, fileSize));
        };

        // Simulate client request
        var requestPacket = new FileTransferProtocolPacket
        {
            SystemId = Identity.SystemId,
            ComponentId = Identity.ComponentId,
            Sequence = 1,
            Payload =
            {
                TargetSystem = Identity.SystemId,
                TargetComponent = Identity.ComponentId,
                TargetNetwork = _config.NetworkId,
                Payload = new byte[200],
            }
        };
        requestPacket.WriteOpcode(FtpOpcode.OpenFileWO);
        requestPacket.WriteSession(1);
        requestPacket.WriteSize((byte)fileSize);
        requestPacket.WriteDataAsString(path);

        // Set up client to receive the server's response
        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet => { _tcs.TrySetResult(packet); });

        // Act
        await Link.Server.Send(requestPacket, _cts.Token).ConfigureAwait(false);

        // Wait for the client to receive the response
        var response = await _tcs.Task.ConfigureAwait(false);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(fileSize, response.ReadSize());
    }

    [Fact]
    public async Task TerminateSession_Success()
    {
        // Arrange
        const byte sessionId = (byte)1;

        Server.TerminateSession = async (requestedSessionId, _) =>
        {
            Assert.Equal(sessionId, requestedSessionId);
            await Task.CompletedTask;
        };

        // Simulate client request
        var requestPacket = new FileTransferProtocolPacket
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
        requestPacket.WriteOpcode(FtpOpcode.TerminateSession);
        requestPacket.WriteSession(sessionId);
        requestPacket.WriteSize(0);

        // Set up client to receive the server's response
        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet => { _tcs.TrySetResult(packet); });

        // Act
        await Link.Server.Send(requestPacket, _cts.Token).ConfigureAwait(false);

        // Wait for the client to receive the response
        var response = await _tcs.Task.ConfigureAwait(false);

        // Assert
        Assert.NotNull(response);
    }

    [Fact]
    public async Task WriteFile_Success()
    {
        // Arrange
        var sessionId = (byte)1;
        var offset = 0U;
        var size = (byte)100;
        var data = new byte[size];
        new Random().NextBytes(data);

        Server.WriteFile = async (request, buffer, _) =>
        {
            Assert.Equal(sessionId, request.Session);
            Assert.Equal(offset, request.Skip);
            Assert.Equal(size, request.Take);
            Assert.Equal(data, buffer.Span[..size].ToArray());
            await Task.CompletedTask;
        };

        // Simulate client request
        var requestPacket = new FileTransferProtocolPacket
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
        requestPacket.WriteOpcode(FtpOpcode.WriteFile);
        requestPacket.WriteSession(sessionId);
        requestPacket.WriteOffset(offset);
        requestPacket.WriteSize(size);
        requestPacket.WriteData(data);

        // Set up client to receive the server's response
        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet => { _tcs.TrySetResult(packet); });

        // Act
        await Link.Server.Send(requestPacket, _cts.Token).ConfigureAwait(false);

        // Wait for the client to receive the response
        var response = await _tcs.Task.ConfigureAwait(false);

        // Assert
        Assert.NotNull(response);
    }

    [Fact]
    public async Task BurstReadFile_Success()
    {
        // Arrange
        const byte sessionId = 1;
        const uint offset = 0U;
        const byte size = 100;
        var dataChunks = new byte[size];
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

            Log.WriteLine($"Server Buffer Content: {BitConverter.ToString(buffer[..readBytes].ToArray())}");

            return new BurstReadResult((byte)readBytes, originStream.Position >= originStream.Length, request);
        };

        var requestPacket = new FileTransferProtocolPacket
        {
            SystemId = Identity.SystemId,
            ComponentId = Identity.ComponentId,
            Sequence = 1,
            Payload =
            {
                TargetSystem = Identity.SystemId,
                TargetComponent = Identity.ComponentId,
                TargetNetwork = 0,
                Payload = new byte[200],
            }
        };
        requestPacket.WriteOpcode(FtpOpcode.BurstReadFile);
        requestPacket.WriteSession(sessionId);
        requestPacket.WriteOffset(offset);
        requestPacket.WriteSize(size);
        requestPacket.WriteData(dataChunks);

        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            Log.WriteLine($"Client received packet: {BitConverter.ToString(packet.Payload.Payload)}");
            _tcs.TrySetResult(packet);
        });

        // Act
        await Link.Server.Send(requestPacket, _cts.Token).ConfigureAwait(false);
        var response = await _tcs.Task.ConfigureAwait(false);
        
        // Assert
        
        var mb = new ArrayBufferWriter<byte>();
        response.ReadData(mb);
        var resultData = mb.WrittenMemory.ToArray();
        
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.BurstReadFile, response.ReadOpcode());
        Assert.Equal(dataChunks, resultData);
    }

    [Fact]
    public async Task CreateFile_Success()
    {
        // Arrange
        const string path = "/path/to/new_file.txt";
        const byte sessionId = 1;

        Server.CreateFile = async (requestedPath, _) =>
        {
            Assert.Equal(path, requestedPath);
            return await Task.FromResult(sessionId);
        };

        var requestPacket = new FileTransferProtocolPacket
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
        requestPacket.WriteOpcode(FtpOpcode.CreateFile);
        requestPacket.WriteSession(0);
        requestPacket.WriteSize(0);
        requestPacket.WriteDataAsString(path);

        Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet => { _tcs.TrySetResult(packet); });

        // Act
        await Link.Server.Send(requestPacket, _cts.Token).ConfigureAwait(false);
        var response = await _tcs.Task.ConfigureAwait(false);

        // Assert
        Assert.NotNull(response);
    }
}