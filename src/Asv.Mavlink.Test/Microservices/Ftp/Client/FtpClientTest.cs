using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class FtpClientTest(ITestOutputHelper log) : ClientTestBase<FtpClient>(log)
{
    private readonly CancellationTokenSource _cts = new();
    private readonly MavlinkFtpClientConfig _config = new()
    {
        TimeoutMs = 1000,
        CommandAttemptCount = 5,
        TargetNetworkId = 0,
        BurstTimeoutMs = 1000
    };

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
        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.ResetSessions)
                {
                    var response = CreateAckResponse(packet, FtpOpcode.ResetSessions);
                    response.WriteSize(0);
                    await Link.Server.Send(response, ct).ConfigureAwait(false);
                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            });

        var result = await Client.ResetSessions().ConfigureAwait(false);

        Assert.NotNull(result);
        var ackOpcode = result.ReadOpcode();
        var ackSession = result.ReadSession();
        var ackSize = result.ReadSize();
        var origin = result.ReadOriginOpCode();
        Assert.Equal(FtpOpcode.Ack, ackOpcode);
        Assert.Equal(FtpOpcode.ResetSessions, origin);
        Assert.Equal(0, ackSession);
        Assert.Equal(0, ackSize);
    }

    [Fact]
    public async Task RemoveDirectory_Success()
    {
        const string expectedPath = "/path/to/directory";

        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.RemoveDirectory)
                {
                    var p = packet.ReadDataAsString();
                    var response = CreateAckResponse(packet, FtpOpcode.RemoveDirectory);
                    response.WriteSize((byte)p.Length);
                    response.WriteDataAsString(p);
                    await Link.Server.Send(response, ct).ConfigureAwait(false);
                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            });

        var result = await Client.RemoveDirectory(expectedPath).ConfigureAwait(false);

        Assert.NotNull(result);
        var ackOpcode = result.ReadOpcode();
        var origin = result.ReadOriginOpCode();
        var actualPath = result.ReadDataAsString();
        Assert.Equal(FtpOpcode.Ack, ackOpcode);
        Assert.Equal(FtpOpcode.RemoveDirectory, origin);
        Assert.Equal(expectedPath, actualPath);
    }

    [Fact]
    public async Task RemoveFile_Success()
    {
        const string expectedPath = "/path/to/file.txt";

        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.RemoveFile)
                {
                    var p = packet.ReadDataAsString();
                    var response = CreateAckResponse(packet, FtpOpcode.RemoveFile);
                    response.WriteSize((byte)p.Length);
                    response.WriteDataAsString(p);
                    await Link.Server.Send(response, ct).ConfigureAwait(false);
                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            });

        var result = await Client.RemoveFile(expectedPath).ConfigureAwait(false);

        Assert.NotNull(result);
        var ackOpcode = result.ReadOpcode();
        var origin = result.ReadOriginOpCode();
        var actualPath = result.ReadDataAsString();
        Assert.Equal(FtpOpcode.Ack, ackOpcode);
        Assert.Equal(FtpOpcode.RemoveFile, origin);
        Assert.Equal(expectedPath, actualPath);
    }

    [Fact]
    public async Task TruncateFile_Success()
    {
        const string expectedPath = "/path/to/file.txt";
        const uint expectedOffset = 1024U;
        var request = new TruncateRequest(expectedPath, expectedOffset);

        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.TruncateFile)
                {
                    var off = packet.ReadOffset();
                    var p = packet.ReadDataAsString();
                    var response = CreateAckResponse(packet, FtpOpcode.TruncateFile);
                    response.WriteSize((byte)(p.Length + 4));
                    response.WriteOffset(off);
                    response.WriteDataAsString(p);
                    await Link.Server.Send(response, ct).ConfigureAwait(false);
                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            });

        var result = await Client.TruncateFile(request).ConfigureAwait(false);

        Assert.NotNull(result);
        var ackOpcode = result.ReadOpcode();
        var origin = result.ReadOriginOpCode();
        var actualOffset = result.ReadOffset();
        var actualPath = result.ReadDataAsString();
        Assert.Equal(FtpOpcode.Ack, ackOpcode);
        Assert.Equal(FtpOpcode.TruncateFile, origin);
        Assert.Equal(expectedOffset, actualOffset);
        Assert.Equal(expectedPath, actualPath);
    }

    [Fact]
    public async Task CalcFileCrc32_Success()
    {
        // Arrange
        const string path = "/path/to/file.txt";
        const uint expectedCrc32 = 0xDEADBEEF;

        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.CalcFileCRC32)
                {
                    var response = CreateAckResponse(packet, FtpOpcode.CalcFileCRC32);
                    response.WriteSize(4);
                    response.WriteDataAsUint(expectedCrc32);

                    await Link.Server.Send(response, ct).ConfigureAwait(false);
                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            });

        // Act
        var crc32 = await Client.CalcFileCrc32(path).ConfigureAwait(false);

        // Assert
        Assert.Equal(expectedCrc32, crc32);
    }

    [Fact]
    public async Task CreateDirectory_Success()
    {
        // Arrange
        const string path = "/path/to/new_directory";

        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.CreateDirectory)
                {
                    var response = CreateAckResponse(packet, FtpOpcode.CreateDirectory);
                    await Link.Server.Send(response, ct).ConfigureAwait(false);
                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            });

        // Act
        var result = await Client.CreateDirectory(path).ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task Rename_Success()
    {
        const string oldPath = "/path/to/old_name.txt";
        const string newPath = "/path/to/new_name.txt";

        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.Rename)
                {
                    var renameData = packet.ReadDataAsString();
                    var response = CreateAckResponse(packet, FtpOpcode.Rename);
                    response.WriteSize((byte)renameData.Length);
                    response.WriteDataAsString(renameData);
                    await Link.Server.Send(response, ct).ConfigureAwait(false);
                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            });

        var result = await Client.Rename(oldPath, newPath, _cts.Token).ConfigureAwait(false);

        Assert.NotNull(result);
        var ackOpcode = result.ReadOpcode();
        var origin = result.ReadOriginOpCode();
        var renameString = result.ReadDataAsString();
        var split = renameString.Split('\0');
        Assert.Equal(FtpOpcode.Ack, ackOpcode);
        Assert.Equal(FtpOpcode.Rename, origin);
        Assert.Equal(oldPath, split[0]);
        Assert.Equal(newPath, split[1]);
    }

    [Fact]
    public async Task ListDirectory_Success()
    {
        // Arrange
        const string path = "/path/to/directory";
        const uint offset = 0;
        const string directoryListing = "file1.txt\nfile2.txt\ndir1/\n";

        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.ListDirectory)
                {
                    var response = CreateAckResponse(packet, FtpOpcode.ListDirectory);
                    response.WriteSize((byte)directoryListing.Length);
                    response.WriteDataAsString(directoryListing);

                    await Link.Server.Send(response, ct).ConfigureAwait(false);
                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            });

        // Act
        var result = await Client.ListDirectory(path, offset).ConfigureAwait(false);

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
        const string path = "/path/to/file.txt";
        const uint fileSize = 2048U;
        const byte sessionId = 1;

        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.OpenFileRO)
                {
                    var response = CreateAckResponse(packet, FtpOpcode.OpenFileRO);
                    response.WriteSession(sessionId);
                    response.WriteSize(4);
                    response.WriteDataAsUint(fileSize);

                    await Link.Server.Send(response, ct).ConfigureAwait(false);
                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            });

        // Act
        var handle = await Client.OpenFileRead(path).ConfigureAwait(false);

        // Assert
        Assert.Equal(sessionId, handle.Session);
        Assert.Equal(fileSize, handle.Size);
    }

    [Fact]
    public async Task OpenFileWrite_Success()
    {
        // Arrange
        const string path = "/path/to/file.txt";
        const uint fileSize = 10U;
        const byte sessionId = 2;

        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.OpenFileWO)
                {
                    var response = CreateAckResponse(packet, FtpOpcode.OpenFileWO);
                    response.WriteSession(sessionId);
                    response.WriteSize(4);
                    response.WriteDataAsUint(fileSize);

                    await Link.Server.Send(response, ct).ConfigureAwait(false);
                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            });

        // Act
        var handle = await Client.OpenFileWrite(path).ConfigureAwait(false);

        // Assert
        Assert.Equal(sessionId, handle.Session);
        Assert.Equal(fileSize, handle.Size);
    }

    [Fact]
    public async Task TerminateSession_Success()
    {
        // Arrange
        const byte sessionId = 1;
        byte responseSessionId = 0;

        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.TerminateSession)
                {
                    var response = CreateAckResponse(packet, FtpOpcode.TerminateSession);
                    responseSessionId = response.ReadSession();
                    await Link.Server.Send(response, ct).ConfigureAwait(false);
                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            });

        // Act
        await Client.TerminateSession(sessionId).ConfigureAwait(false);

        // Assert
        Assert.Equal(sessionId, responseSessionId);
    }

    [Fact]
    public async Task CreateFile_Success()
    {
        // Arrange
        const string path = "/path/to/new_file.txt";
        var actualPath = string.Empty;

        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.CreateFile)
                {
                    actualPath = packet.ReadDataAsString();
                    var response = CreateAckResponse(packet, FtpOpcode.CreateFile);
                    await Link.Server.Send(response, ct).ConfigureAwait(false);
                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            });

        // Act
        await Client.CreateFile(path).ConfigureAwait(false);

        // Assert
        Assert.Equal(path, actualPath);
    }

    [Fact]
    public async Task ReadFile_Success()
    {
        // Arrange
        const byte sessionId = 1;
        const uint skip = 0U;
        const byte take = 100;
        var request = new ReadRequest(sessionId, skip, take);
        var data = new byte[take];
        new Random().NextBytes(data);

        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.ReadFile)
                {
                    var response = CreateAckResponse(packet, FtpOpcode.ReadFile);
                    response.WriteSession(sessionId);
                    response.WriteSize((byte)data.Length);
                    response.WriteData(data);

                    await Link.Server.Send(response, ct).ConfigureAwait(false);
                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            });

        // Act
        var result = await Client.ReadFile(request).ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
        var size = result.ReadSize();
        var receivedData = result.Payload.Payload.AsSpan(12, size).ToArray();
        Assert.Equal(data, receivedData);
    }

    [Fact]
    public async Task WriteFile_Success()
    {
        const byte sessionId = 1;
        const uint skip = 0U;
        const byte take = 100;
        var request = new WriteRequest(sessionId, skip, take);
        var data = new byte[take];
        new Random().NextBytes(data);

        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.WriteFile)
                {
                    var s = packet.ReadSession();
                    var off = packet.ReadOffset();
                    var sz = packet.ReadSize();
                    var chunk = packet.Payload.Payload.AsSpan(12, sz).ToArray();
                    var response = CreateAckResponse(packet, FtpOpcode.WriteFile);
                    response.WriteSession(s);
                    response.WriteOffset(off);
                    response.WriteSize(sz);
                    response.WriteData(chunk);
                    await Link.Server.Send(response, ct).ConfigureAwait(false);
                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            });

        var result = await Client.WriteFile(request, data).ConfigureAwait(false);

        Assert.NotNull(result);
        var ackOpcode = result.ReadOpcode();
        var ackSession = result.ReadSession();
        var ackOffset = result.ReadOffset();
        var ackSize = result.ReadSize();
        var origin = result.ReadOriginOpCode();
        var ackData = result.Payload.Payload.AsSpan(12, ackSize).ToArray();
        Assert.Equal(FtpOpcode.Ack, ackOpcode);
        Assert.Equal(FtpOpcode.WriteFile, origin);
        Assert.Equal(sessionId, ackSession);
        Assert.Equal(skip, ackOffset);
        Assert.Equal(take, ackSize);
        Assert.Equal(data, ackData);
    }

    [Fact]
    public async Task BurstReadFile_Success()
    {
        // Arrange
        const byte sessionId = 1;
        const uint skip = 0U;
        const byte take = 100;
        var request = new ReadRequest(sessionId, skip, take);
        var dataChunks = new List<byte[]>
        {
            new byte[50],
            new byte[50]
        };
        new Random().NextBytes(dataChunks[0]);
        new Random().NextBytes(dataChunks[1]);

        var totalDataReceived = new List<byte>();

        Link.Server
            .RxFilterByType<FileTransferProtocolPacket>()
            .SubscribeAwait(async (packet, ct) =>
            {
                if (packet.ReadOpcode() == FtpOpcode.BurstReadFile)
                {
                    // Send data in chunks
                    for (var i = 0; i < dataChunks.Count; i++)
                    {
                        var response = CreateAckResponse(packet, FtpOpcode.BurstReadFile);
                        response.WriteSession(sessionId);
                        response.WriteSize((byte)dataChunks[i].Length);
                        response.WriteData(dataChunks[i]);
                        response.WriteBurstComplete(i == dataChunks.Count - 1 ? (byte)1 : (byte)0);

                        await Link.Server.Send(response, ct).ConfigureAwait(false);
                        Time.Advance(TimeSpan.FromMilliseconds(10));
                    }
                }
            });

        // Act
        await Client.BurstReadFile(request, packet =>
        {
            var size = packet.ReadSize();
            var chunk = packet.Payload.Payload.AsSpan(12, size).ToArray();
            totalDataReceived.AddRange(chunk);
        }).ConfigureAwait(false);

        // Assert
        var expectedData = dataChunks.SelectMany(d => d).ToArray();
        Assert.Equal(expectedData, totalDataReceived.ToArray());
    }

    [Fact]
    public async Task ResetSessions_Cancel_Throws()
    {
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.ResetSessions(_cts.Token);
        });
    }

    [Fact]
    public async Task RemoveDirectory_Cancel_Throws()
    {
        const string path = "/path/to/directory";
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.RemoveDirectory(path, _cts.Token);
        });
    }

    [Fact]
    public async Task RemoveFile_Cancel_Throws()
    {
        const string path = "/path/to/file.txt";
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.RemoveFile(path, _cts.Token);
        });
    }

    [Fact]
    public async Task TruncateFile_Cancel_Throws()
    {
        const string path = "/path/to/file.txt";
        var request = new TruncateRequest(path, 1024);
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.TruncateFile(request, _cts.Token);
        });
    }

    [Fact]
    public async Task CalcFileCrc32_Cancel_Throws()
    {
        const string path = "/path/to/file.txt";
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.CalcFileCrc32(path, _cts.Token);
        });
    }

    [Fact]
    public async Task CreateDirectory_Cancel_Throws()
    {
        const string path = "/path/to/new_directory";
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.CreateDirectory(path, _cts.Token);
        });
    }

    [Fact]
    public async Task Rename_Cancel_Throws()
    {
        const string oldPath = "/path/to/old_name.txt";
        const string newPath = "/path/to/new_name.txt";
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.Rename(oldPath, newPath, _cts.Token);
        });
    }

    [Fact]
    public async Task ListDirectory_Cancel_Throws()
    {
        const string path = "/path/to/directory";
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.ListDirectory(path, 0, _cts.Token);
        });
    }

    [Fact]
    public async Task OpenFileRead_Cancel_Throws()
    {
        const string path = "/path/to/file.txt";
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.OpenFileRead(path, _cts.Token);
        });
    }

    [Fact]
    public async Task OpenFileWrite_Cancel_Throws()
    {
        const string path = "/path/to/file.txt";
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.OpenFileWrite(path, _cts.Token);
        });
    }

    [Fact]
    public async Task TerminateSession_Cancel_Throws()
    {
        const byte sessionId = 1;
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.TerminateSession(sessionId, _cts.Token);
        });
    }

    [Fact]
    public async Task CreateFile_Cancel_Throws()
    {
        const string path = "/path/to/new_file.txt";
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.CreateFile(path, _cts.Token);
        });
    }

    [Fact]
    public async Task ReadFile_Cancel_Throws()
    {
        const byte sessionId = 1;
        var request = new ReadRequest(sessionId, 0, 100);
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.ReadFile(request, _cts.Token);
        });
    }

    [Fact]
    public async Task WriteFile_Cancel_Throws()
    {
        const byte sessionId = 1;
        var request = new WriteRequest(sessionId, 0, 100);
        var data = new byte[100];
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.WriteFile(request, data, _cts.Token);
        });
    }

    [Fact]
    public async Task BurstReadFile_Cancel_Throws()
    {
        const byte sessionId = 1;
        var request = new ReadRequest(sessionId, 0, 100);
        await _cts.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.BurstReadFile(request, _ => { }, _cts.Token);
        });
    }
}