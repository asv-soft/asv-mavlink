using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(FtpClientEx))]
public class FtpClientExTest : ClientTestBase<FtpClientEx>
{
    private const byte ACKLength = 4;
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
    public async Task UploadFile_NormalData_Success()
    {
        // Arrange
        const int dataSize = 500;
        const string filePath = "/path/to/new_file.txt";
        const byte session = 1;
        const int dataOffset = 12;
        const int expectedCreateFileCount = 1;
        const int expectedWriteFileCount = 3;
        const int expectedTerminateSessionCount = 1;
        var realCreateFileCount = 0;
        var realWriteFileCount = 0;
        var realTerminateSessionCount = 0;
        
        var data = new byte[dataSize];
        Random.Shared.NextBytes(data);
        var stream = new MemoryStream(data);
        var writtenData = new ArrayBufferWriter<byte>(data.Length);
        
        using var sub1 = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() == FtpOpcode.CreateFile)
            {
                realCreateFileCount++;
                var requestedPath = packet.ReadDataAsString();

                if (filePath != requestedPath)
                {
                    throw new Exception($"{filePath} is not equal to {requestedPath}");
                }

                var response = CreateAckResponse(packet, FtpOpcode.CreateFile);
                response.WriteSession(session);
                await Link.Server.Send(response, ct);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        using var sub2 = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() == FtpOpcode.WriteFile)
            {
                realWriteFileCount++;
                var pSession = packet.ReadSession();
                var size = packet.ReadSize();
                
                if (session != pSession)
                {
                    throw new Exception($"{session} is not equal to {pSession}");
                }
                
                var innerData = packet.Payload.Payload.AsMemory(dataOffset, size);
                writtenData.Write(innerData.Span);

                var response = CreateAckResponse(packet, FtpOpcode.WriteFile);
                response.WriteSize(0);

                await Link.Server.Send(response, ct);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        using var sub3 = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() == FtpOpcode.TerminateSession)
            {
                realTerminateSessionCount++;
                var pSession = packet.ReadSession();
                
                if (session != pSession)
                {
                    throw new Exception($"{session} is not equal to {pSession}");
                }
                
                var response = CreateAckResponse(packet, FtpOpcode.TerminateSession);
                await Link.Server.Send(response, ct);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        
        // Act
        await Client.UploadFile(filePath, stream).ConfigureAwait(false);
        
        // Assert
        var allPacketsSent = (uint)(
            realCreateFileCount + 
            realWriteFileCount + 
            realTerminateSessionCount
        );
        Assert.True(data.IsDeepEqual(writtenData.WrittenMemory.ToArray()));
        Assert.Equal(expectedCreateFileCount, realCreateFileCount);
        Assert.Equal(expectedWriteFileCount, realWriteFileCount);
        Assert.Equal(expectedTerminateSessionCount, realTerminateSessionCount);
        Assert.Equal(allPacketsSent, Link.Client.Statistic.TxMessages);
        Assert.Equal(allPacketsSent, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Fact]
    public async Task DownloadFile_NormalData_Success()
    {
        // Arrange
        const int dataSize = 500;
        const string path = "/path/to/file.txt";
        const byte session = 1;
        const int expectedOpenFileRoCount = 1;
        const int expectedReadFileCount = 3;
        const int expectedTerminateSessionCount = 1;
        var skip = 0;
        var take = MavlinkFtpHelper.MaxDataSize;
        var realOpenFileRoCount = 0;
        var realReadFileCount = 0;
        var realTerminateSessionCount = 0;
        
        var stream = new MemoryStream(dataSize);
        var data = new byte[dataSize];
        Random.Shared.NextBytes(data);
        
        using var sub1 = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() == FtpOpcode.OpenFileRO)
            {
                realOpenFileRoCount++;
                
                var requestedPath = packet.ReadDataAsString();
                if (path != requestedPath)
                {
                    throw new Exception($"{path} is not equal to {requestedPath}");
                }

                var response = CreateAckResponse(packet, FtpOpcode.OpenFileRO);
                response.WriteSession(session);
                response.WriteDataAsUint(dataSize);

                await Link.Server.Send(response, ct);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        using var sub2 = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() == FtpOpcode.ReadFile)
            {
                realReadFileCount++;
                var pSession = packet.ReadSession();
                if (session != pSession)
                {
                    throw new Exception($"{session} is not equal to {pSession}");
                }
                
                var response = CreateAckResponse(packet, FtpOpcode.ReadFile);
                var mem = data.AsMemory(skip, take);
                response.WriteData(mem.Span);
                skip += MavlinkFtpHelper.MaxDataSize;
                take = (byte)(data.Length - skip > MavlinkFtpHelper.MaxDataSize ? MavlinkFtpHelper.MaxDataSize : data.Length - skip);
                
                await Link.Server.Send(response, ct);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        using var sub3 = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() == FtpOpcode.TerminateSession)
            {
                realTerminateSessionCount++;
                var pSession = packet.ReadSession();
                if (session != pSession)
                {
                    throw new Exception($"{session} is not equal to {pSession}");
                }
                
                var response = CreateAckResponse(packet, FtpOpcode.TerminateSession);
                await Link.Server.Send(response, ct);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        
        // Act
        await Client.DownloadFile(path, stream).ConfigureAwait(false);
        
        // Assert
        var allPacketsSent = (uint)(
            realOpenFileRoCount + 
            realReadFileCount + 
            realTerminateSessionCount
        );
        Assert.True(data.IsDeepEqual(stream.ToArray()));
        Assert.Equal(expectedOpenFileRoCount, realOpenFileRoCount);
        Assert.Equal(expectedReadFileCount, realReadFileCount);
        Assert.Equal(expectedTerminateSessionCount, realTerminateSessionCount);
        Assert.Equal(allPacketsSent, Link.Client.Statistic.TxMessages);
        Assert.Equal(allPacketsSent, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }
    
    [Fact]
    public async Task BurstDownloadFile_NormalData_Success()
    {
        // Arrange
        const uint fileSize = 500;
        const string path = "/path/to/file.txt";
        const int expectedOpenFileRoCount = 1;
        const int expectedBurstReadFileCount = 1;
        const int expectedTerminateSessionCount = 1;
        const int expectedClientAckCount = 5;
        const byte take = MavlinkFtpHelper.MaxDataSize;
        const byte session = 1;
        var skip = 0u;
        var realOpenFileRoCount = 0;
        var realBurstReadFileCount = 0;
        var realTerminateSessionCount = 0;
        var realClientAckCount = 0;
        
        var stream = new MemoryStream((int)fileSize);
        var data = new byte[fileSize];
        Random.Shared.NextBytes(data);
        
        using var sub1 = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() == FtpOpcode.OpenFileRO)
            {
                realOpenFileRoCount++;
                    
                var requestedPath = packet.ReadDataAsString();
                if (path != requestedPath)
                {
                    throw new Exception($"{path} is not equal to {requestedPath}");
                }

                var response = CreateAckResponse(packet, FtpOpcode.OpenFileRO);
                response.WriteSession(session);
                response.WriteDataAsUint(fileSize);

                await Link.Server.Send(response, ct);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        using var sub2 = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() == FtpOpcode.BurstReadFile)
            {
                realBurstReadFileCount++;
                while (skip < fileSize)
                {
                    var pSession = packet.ReadSession();
                    if (session != pSession)
                    {
                        throw new Exception($"{session} is not equal to {pSession}");
                    }
                    
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
                
                    await Link.Server.Send(response, ct);

                    Time.Advance(TimeSpan.FromMilliseconds(10));
                }
            }
        });
        using var sub3 = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() == FtpOpcode.ReadFile)
            {
                throw new Exception("Wrong read command");
            }
        });
        using var sub4 = Link.Server.RxFilterByType<FileTransferProtocolPacket>().SubscribeAwait(async (packet, ct) =>
        {
            if (packet.ReadOpcode() == FtpOpcode.TerminateSession)
            {
                realTerminateSessionCount++;
                
                var pSession = packet.ReadSession();
                if (session != pSession)
                {
                    throw new Exception($"{session} is not equal to {pSession}");
                }
                
                var response = CreateAckResponse(packet, FtpOpcode.TerminateSession);
                
                await Link.Server.Send(response, ct);
                
                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        using var sub5 = Link.Client.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet  =>
        {
            if (packet.ReadOpcode() == FtpOpcode.Ack)
            {
                realClientAckCount++;
            }
        });
        
        // Act
        await Client.BurstDownloadFile(path, stream).ConfigureAwait(false);
        
        // Assert
        var allPacketsSent = (uint)(
            realOpenFileRoCount + 
            realBurstReadFileCount +
            realTerminateSessionCount
        );
        Assert.True(data.IsDeepEqual(stream.ToArray()));
        Assert.Equal(expectedOpenFileRoCount, realOpenFileRoCount);
        Assert.Equal(expectedBurstReadFileCount, realBurstReadFileCount);
        Assert.Equal(expectedTerminateSessionCount, realTerminateSessionCount);
        Assert.Equal(expectedClientAckCount, realClientAckCount);
        Assert.Equal(allPacketsSent, Link.Client.Statistic.TxMessages);
        Assert.Equal((uint) realClientAckCount, Link.Client.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
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
        response.WriteSize(ACKLength);
        response.WriteOriginOpCode(originOpCode);

        return response;
    }
}