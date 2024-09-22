using System;
using System.Buffers;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Common;
using Castle.Core.Logging;
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
        Func<IPacketV2<IPayload>,bool> clientToServer, Func<IPacketV2<IPayload>,bool> serverToClient)
    {
        var link = new VirtualMavlinkConnection(clientToServer,serverToClient);
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);

        var clientSeq = new PacketSequenceCalculator();
        client = new FtpClient(new MavlinkFtpClientConfig(), link.Client, clientId, clientSeq,
            TaskPoolScheduler.Default,new TestLogger(_output,"CLIENT"));

        var serverSeq = new PacketSequenceCalculator();
        server = new FtpServer(new MavlinkFtpServerConfig(), link.Server, serverId, serverSeq, TaskPoolScheduler.Default,new TestLogger(_output,"SERVER") );
    }

    [Theory]
    [InlineData("mftp://test.txt", 0, uint.MaxValue)]
    public async Task Client_Call_OpenFileRead_And_Server_Catch_It(string originPath, byte originSession,uint originFileSize)
    {
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);

        var called = 0;
        server.OpenFileRead = (path, cancel) =>
        {
            Assert.Equal(originPath,path);
            called++;
            return Task.FromResult(new OpenReadResult(originSession,originFileSize));
        };

        var result = await client.OpenFileRead(originPath);
        Assert.Equal(1,called);
        Assert.Equal(originSession,result.Session);
        Assert.Equal(originFileSize,result.Size);
    }    
    
    [Theory]
    [InlineData("mftp://test.txt", 0, uint.MaxValue)]
    public async Task Client_Call_OpenFileRead_With_Skip_One_Request_And_Server_Catch_It_Once(string originPath, byte originSession,uint originFileSize)
    {
        var skip = 3;
        SetUpMicroservice(out var client, out var server, (packet) =>true, (packet) =>  skip-- == 0);

        var called = 0;
        server.OpenFileRead = (path, cancel) =>
        {
            Assert.Equal(originPath,path);
            called++;
            return Task.FromResult(new OpenReadResult(originSession,originFileSize));
        };

        var result = await client.OpenFileRead(originPath);
        Assert.Equal(1,called);
        Assert.Equal(originSession,result.Session);
        Assert.Equal(originFileSize,result.Size);
    }
    
    [Theory]
    [InlineData(1, 0,200)]
    public async Task Client_Call_FileRead_And_Server_Catch_It(byte session,uint skip,byte take)
    {
        SetUpMicroservice(out var client, out var server, (packet) =>true, (packet) =>  true);
        var originData = new byte[take];
        Random.Shared.NextBytes(originData);
        
        var called = 0;
        server.FileRead = (req,buffer, cancel) =>
        {
            called++;
            originData.CopyTo(buffer.Span);
            return Task.FromResult(new ReadResult((byte)originData.Length,req));
        };

        var originRequest = new ReadRequest(session,skip,take);
        using var buffer = MemoryPool<byte>.Shared.Rent(take);
        var result = await client.ReadFile(originRequest, buffer.Memory);
        
        Assert.Equal(1,called);
        Assert.Equal(originRequest,result.Request);
        Assert.Equal(originData.Length,result.ReadCount);
        Assert.Equal(originData,buffer.Memory.Slice(0,originData.Length).ToArray());
        
    }    
}