using System;
using System.Buffers;
using System.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class FtpMicroserviceTest
{
    private readonly ITestOutputHelper _output;

    public FtpMicroserviceTest(ITestOutputHelper output)
    {
        _output = output;
    }

    private void SetUpMicroservice(out IFtpClient client, out IFtpServer server,
        Func<IPacketV2<IPayload>, bool> clientToServer, Func<IPacketV2<IPayload>, bool> serverToClient)
    {
        var link = new VirtualMavlinkConnection(clientToServer, serverToClient);
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);

        var clientSeq = new PacketSequenceCalculator();
        client = new FtpClient(new MavlinkFtpClientConfig(), link.Client, clientId, clientSeq, TimeProvider.System,
            TaskPoolScheduler.Default, new TestLogger(_output, "CLIENT"));

        var serverSeq = new PacketSequenceCalculator();
        server = new FtpServer(new MavlinkFtpServerConfig(), link.Server, serverId, serverSeq,
            TaskPoolScheduler.Default, new TestLogger(_output, "SERVER"));
    }

    [Theory]
    [InlineData("mftp://test.txt", 0, uint.MaxValue)]
    public async Task Client_Call_OpenFileRead_And_Server_Catch_It(string originPath, byte originSession,
        uint originFileSize)
    {
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);

        var called = 0;
        server.OpenFileRead = (path, cancel) =>
        {
            Assert.Equal(originPath, path);
            called++;
            return Task.FromResult(new ReadHandle(originSession, originFileSize));
        };

        var result = await client.OpenFileRead(originPath);
        Assert.Equal(1, called);
        Assert.Equal(originSession, result.Session);
        Assert.Equal(originFileSize, result.Size);
    }

    [Theory]
    [InlineData("mftp://test.txt", 1177814316)]
    public async Task Client_call_CalcFileCrc32(string path, uint originCheckSum)
    {
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);
        server.CalcFileCrc32 = (path, cancellationToken) => Task.FromResult(originCheckSum);
        var result = await client.CalcFileCrc32(path).ConfigureAwait(false);
        Assert.Equal(result, originCheckSum);
    }
    
    [Theory]
    [InlineData("mftp://test.txt", 10)]
    public async Task Client_Call_TruncateFile_And_Server_Catch_It(string filePath, uint offset)
    {
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);

        var called = 0;

        server.TruncateFile = (request, cancel) =>
        {
            Assert.Equal(filePath, request.Path);
            Assert.Equal(offset, request.Offset);
            called++;
            return Task.FromResult(filePath);
        };

        var result = await client.TruncateFile(new TruncateRequest(filePath, offset)).ConfigureAwait(false);

        Assert.Equal(1, called);
        Assert.Equal((byte)0, result.ReadSize());
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }
    
    [Theory]
    [InlineData("mftp://directory//")]
    public async Task Client_call_CreateDirectory(string directoryPath)
    {
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);
        server.CreateDirectory = (path, cancellationToken) => Task.FromResult(path);

        var result = await client.CreateDirectory(directoryPath);
        Assert.Equal((byte)0, result.ReadSize());
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }
    
    [Theory]
    [InlineData("mftp://test.txt", 0)]
    [InlineData("mftp://test.txt", byte.MaxValue)]
    public async Task CreateFile_WithProperInput_Success(string filePath, byte session)
    {
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);
        server.CreateFile = (path, cancellationToken) => Task.FromResult(session);

        var result = await client.CreateFile(filePath);
        Assert.Equal(session, result.Session);
    }
    
    [Theory]
    [InlineData("mftp://directory//")]
    public async Task Client_call_RemoveDirectory(string directoryPath)
    {
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);
        server.RemoveDirectory = (path, cancellationToken) => Task.FromResult(path);

        var result = await client.RemoveDirectory(directoryPath);
        Assert.Equal((byte)0, result.ReadSize());
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Theory]
    [InlineData("mftp://test.txt")]
    public async Task Client_Call_RemoveFile(string directoryPath)
    {
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);
        server.RemoveFile = (path, cancellationToken) => Task.FromResult(path);

        var result = await client.RemoveFile(directoryPath);
        Assert.Equal((byte)0, result.ReadSize());
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Fact]
    public async Task Client_Call_ResetSession()
    {
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);
        server.ResetSessions = (cancellationToken) => Task.FromResult(cancellationToken);
        var res = await client.ResetSessions();
        Assert.Equal(res.ReadOpcode(),FtpOpcode.Ack );
    }
    


    [Theory]
    [InlineData("mftp://test.txt", 0, uint.MaxValue)]
    public async Task Client_Call_OpenFileRead_With_Skip_One_Request_And_Server_Catch_It_Once(string originPath,
        byte originSession, uint originFileSize)
    {
        var skip = 3;
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => skip-- == 0);

        var called = 0;
        server.OpenFileRead = (path, cancel) =>
        {
            Assert.Equal(originPath, path);
            called++;
            return Task.FromResult(new ReadHandle(originSession, originFileSize));
        };

        var result = await client.OpenFileRead(originPath);
        Assert.Equal(1, called);
        Assert.Equal(originSession, result.Session);
        Assert.Equal(originFileSize, result.Size);
    }
    
    [Theory]
    [InlineData("mftp://test.txt", 0, uint.MaxValue)]
    public async Task OpenFileWrite_WithProperInput_Success(string originPath,
        byte originSession, uint originFileSize)
    {
        var skip = 3;
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => skip-- == 0);

        var called = 0;
        server.OpenFileWrite = (path, cancel) =>
        {
            Assert.Equal(originPath, path);
            called++;
            return Task.FromResult(new WriteHandle(originSession, originFileSize));
        };

        var result = await client.OpenFileWrite(originPath);
        Assert.Equal(1, called);
        Assert.Equal(originSession, result.Session);
        Assert.Equal(originFileSize, result.Size);
    }

    [Theory]
    [InlineData(1, 0, 200)]
    public async Task Client_Call_FileRead_And_Server_Catch_It(byte session, uint skip, byte take)
    {
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);
        var originData = new byte[take];
        Random.Shared.NextBytes(originData);

        var called = 0;
        server.FileRead = (req, buffer, cancel) =>
        {
            called++;
            originData.CopyTo(buffer.Span);
            return Task.FromResult(new ReadResult((byte)originData.Length, req));
        };

        var originRequest = new ReadRequest(session, skip, take);
        using var buffer = MemoryPool<byte>.Shared.Rent(take);
        var result = await client.ReadFile(originRequest, buffer.Memory);
        
        Assert.Equal(1,called);
        Assert.Equal(originRequest,result.Request);
        Assert.Equal(originData.Length,result.ReadCount);
        Assert.Equal(originData,buffer.Memory.Slice(0,originData.Length).ToArray());
    } 
    
    [Theory]
    [InlineData("mftp://test.txt", "mftp://test1.txt")]
    [InlineData("mftp://test", "mftp://test1")]
    public async Task Rename_WithProperInput_Success(string defaultPath, string newPath)
    {
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);

        server.Rename = (path1, path2, cancel) => Task.FromResult((path1, path2, cancel));

        var result = await client.Rename(defaultPath, newPath);
        Assert.Equal((byte)0, result.ReadSize());
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }

    [Theory]
    [InlineData("mftp://test", 0)]
    [InlineData("mftp://test", 45)]
    [InlineData("mftp://test", uint.MaxValue)]
    public async Task ListDirectory_WithProperInput_Success(string path, uint offset)
    {
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);
        var originData = new char[MavlinkFtpHelper.MaxDataSize];
        for (var i = 0; i < originData.Length; i++)
        {
            originData[i] = (char)Random.Shared.Next(0, 32);
        }

        var called = 0;
        server.ListDirectory = (path, offset, buffer, cancel) =>
        {
            called++;
            originData.CopyTo(buffer.Span);
            return Task.FromResult((byte) originData.Length);
        };
        
        using var buffer = MemoryPool<char>.Shared.Rent(MavlinkFtpHelper.MaxDataSize);
        var result = await client.ListDirectory(path, offset, buffer.Memory);
        
        Assert.Equal(1,called);
        Assert.Equal(originData.Length,result);
        Assert.True(originData.SequenceEqual(buffer.Memory.Slice(0,originData.Length).ToArray()));
    }
    
    [Theory]
    [InlineData(1, 0, 238)]
    [InlineData(1, 0, 200)]
    public async Task WriteFile_WithProperInput_Success(byte session, uint skip, byte take)
    {
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);
        var originData = new byte[take];
        Random.Shared.NextBytes(originData);

        var internalBuffer = new byte[take];
        var called = 0;
        server.WriteFile = (req, buffer, cancel) =>
        {
            called++;
            buffer.Span[..take].CopyTo(internalBuffer);
            return Task.CompletedTask;
        };

        var originRequest = new WriteRequest(session, skip, take);
        using var buffer = MemoryPool<byte>.Shared.Rent(take);
        originData.CopyTo(buffer.Memory.Span);
        var result = await client.WriteFile(originRequest, buffer.Memory);
        
        Assert.Equal(1,called);
        Assert.Equal((byte)0, result.ReadSize());
        Assert.True(internalBuffer.SequenceEqual(originData));
        Assert.Equal(FtpOpcode.Ack, result.ReadOpcode());
    }
    
    [Theory]
    [InlineData(1, 0, Byte.MaxValue)]
    [InlineData(1, 0, MavlinkFtpHelper.MaxDataSize+1)]
    public async Task WriteFile_WithWrongDataSize_ThrowsArgumentException(byte session, uint skip, byte take)
    {
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);
        var originData = new byte[take];
        Random.Shared.NextBytes(originData);

        var internalBuffer = new byte[take];
        server.WriteFile = (req, buffer, cancel) =>
        {
            buffer.Span[..take].CopyTo(internalBuffer);
            return Task.CompletedTask;
        };

        var originRequest = new WriteRequest(session, skip, take);
        using var buffer = MemoryPool<byte>.Shared.Rent(take);
        originData.CopyTo(buffer.Memory.Span);
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await client.WriteFile(originRequest, buffer.Memory)
        );
    }
    
    [Theory]
    [InlineData(1, 0, 10, 200)]
    public async Task BurstReadFile_WithProperInput_Success(byte session, uint skip, byte take, byte originArrSize)
    {
        // Assert
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);
        var originData = new byte[originArrSize];
        Random.Shared.NextBytes(originData);
        var originStream = new MemoryStream(originData);
        var called = 0;
        var originRequest = new ReadRequest(session, skip, take);
        server.BurstReadFile = async (req, buffer, cancel) =>
        {
            var bytes = ArrayPool<byte>.Shared.Rent(req.Take);
            try
            {
                called++;
                var isLastChunk = req.Skip + req.Take >= originStream.Length;

                originStream.Position = req.Skip;
                var size = await originStream.ReadAsync(bytes, 0, req.Take, cancel)
                    .ConfigureAwait(false);
                var realBytes = bytes[..size];
                realBytes.CopyTo(buffer);

                return new BurstReadResult((byte)size, isLastChunk, req);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(bytes);
            }
        };
        
        // Act
        try
        {
            var fullResult = new byte[originArrSize];
            await client.BurstReadFile(originRequest, packet =>
            {
                var offset = Convert.ToInt32(packet.ReadOffset());
                var size = packet.ReadSize();
                var resultBuf = new byte[size];
                packet.ReadData(resultBuf);
                Array.Copy(resultBuf, 0, fullResult, offset, size);
            });
            
            // Assert
            Assert.True(originData.SequenceEqual(fullResult));
            Assert.Equal(originArrSize / take, called);
        }
        finally
        {
            originStream.Close();
        }
    }

    [Fact]
    public async Task Client_Call_UploadFile_And_Server_Catch_It()
    {
        SetUpMicroservice(out var client, out var server, packet => true, packet => true);

        const string testFilePath = "mftp://upload/test.txt";
        var testData = new byte[500];
        new Random().NextBytes(testData);
        using var streamToUpload = new MemoryStream(testData);

        const byte expectedSession = 1;
        var expectedFileSize = (uint)testData.Length;

        server.OpenFileWrite = (path, cancel) =>
        {
            Assert.Equal(testFilePath, path);
            return Task.FromResult(new WriteHandle(expectedSession, expectedFileSize));
        };

        var receivedData = new byte[testData.Length];
        server.WriteFile = (request, data, cancel) =>
        {
            Assert.Equal(expectedSession, request.Session);
            var expectedDataLength = request.Take;
            _output.WriteLine($"Write exceeds buffer size. Written={request.Skip + data.Length}. To be write={receivedData.Length}");
            data[..expectedDataLength].CopyTo(receivedData.AsMemory((int)request.Skip, expectedDataLength));
            return Task.CompletedTask;
        };

        server.CreateFile = (path, cancel) =>
        {
            Assert.Equal(testFilePath, path);
            return Task.FromResult(expectedSession);
        };

        server.TerminateSession = (session, cancel) =>
        {
            Assert.Equal(expectedSession, session);
            return Task.CompletedTask;
        };

        var ftpClientEx = new FtpClientEx(client);
        await ftpClientEx.UploadFile(testFilePath, streamToUpload);
        
        var writeHandle = await client.OpenFileWrite(testFilePath);
        Assert.Equal(expectedSession, writeHandle.Session);
        Assert.Equal(testData.Length, receivedData.Length);
        Assert.Equal(testData.ToArray(), receivedData);
    }
}