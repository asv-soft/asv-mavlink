using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Microsoft.Extensions.Time.Testing;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class FtpServerExHelper
{
    private readonly ITestOutputHelper _output;

    public MockFileSystem SetUpFileSystem(string root)
    {
        var mockFileCfg = new MockFileSystemOptions
        {
            CurrentDirectory = root
        };
        var fileSystem = new MockFileSystem(mockFileCfg);
        fileSystem.AddDirectory(mockFileCfg.CurrentDirectory);

        return fileSystem;
    }

    public void SetUpServer(out IFtpServer server)
    {
        var link = new VirtualMavlinkConnection(_ => true, _ => true);
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);

        var serverSeq = new PacketSequenceCalculator();
        server = new FtpServer(new MavlinkFtpServerConfig(), link.Server, serverId, serverSeq,TimeProvider.System,
            TaskPoolScheduler.Default, new TestLoggerFactory(_output, "SERVER"));
    }

    public void SetUpClientAndServer(out IFtpClient client, out IFtpServer server,
        Func<IPacketV2<IPayload>, bool> clientToServer, Func<IPacketV2<IPayload>, bool> serverToClient)
    {
        var timeProvider = new FakeTimeProvider();
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
        client = new FtpClient(new MavlinkFtpClientConfig(), link.Client, clientId, clientSeq, timeProvider,
            TaskPoolScheduler.Default, new TestLoggerFactory(_output, "CLIENT"));

        var serverSeq = new PacketSequenceCalculator();
        server = new FtpServer(new MavlinkFtpServerConfig(), link.Server, serverId, serverSeq,timeProvider,
            TaskPoolScheduler.Default, new TestLoggerFactory(_output, "SERVER"));
    }

    public async Task<string> ConvertStreamToString(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        using var reader = new StreamReader(stream, MavlinkFtpHelper.FtpEncoding);
        return await reader.ReadToEndAsync();
    }
}