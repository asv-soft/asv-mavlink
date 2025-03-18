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
        var progress = new Progress<double>();
        const uint skip = 0U;
        var data = new byte[500];
        new Random().NextBytes(data);
        var stream = new MemoryStream(data);

        var writtenData = new ArrayBufferWriter<byte>(data.Length);
        
        // Act
        Link.Server.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.OpenFileWO)
            {
                var requestedPath = packet.ReadDataAsString();

                Assert.Equal(filePath, requestedPath);

                var response = CreateAckResponse(packet, FtpOpcode.OpenFileWO);

                _ = Link.Server.Send(response);

                Time.Advance(TimeSpan.FromMilliseconds(10));
            }
        });
        Link.Server.RxFilterByType<FileTransferProtocolPacket>().Subscribe(packet =>
        {
            if (packet.ReadOpcode() == FtpOpcode.WriteFile)
            {
                var requestedSkip = packet.ReadOffset();
                var size = packet.ReadSize();
                var innerData = packet.Payload.Payload.AsSpan(12, size).ToArray();
                writtenData.Write(innerData);

                var response = CreateAckResponse(packet, FtpOpcode.WriteFile);

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
        
        var resultTask = Client.UploadFile(filePath, stream, progress, CancellationToken.None);
        await _tcs.Task.ConfigureAwait(false);
        await resultTask.ConfigureAwait(false);
        
        // Assert
        Assert.Equal(data, writtenData.WrittenMemory.ToArray());
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
        response.WriteSize(4);
        response.WriteOriginOpCode(originOpCode);

        return response;
    }
}