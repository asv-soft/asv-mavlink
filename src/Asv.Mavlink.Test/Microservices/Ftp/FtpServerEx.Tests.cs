using System;
using System.IO;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class FtpServerExTests
{
    private readonly ITestOutputHelper _output;

    public FtpServerExTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private string SetUpRootFolder()
    {
        var tempFolder = Path.Combine(Path.GetTempPath(), "temp");
        if (Directory.Exists(tempFolder))
        {
            Directory.Delete(tempFolder, true);
        }
        
        Directory.CreateDirectory(tempFolder);
        return tempFolder;
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
    
    [Fact]
    public async Task OpenFileRead_WithEmptyFile_Success()
    {
        //Arrange
        var fileName = "test.txt";
        var root = SetUpRootFolder();
        var filePath = Path.Combine(root, fileName);
        var stream = File.Create(filePath);
        stream.Close();
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);
        var cfg = new MavlinkFtpServerExConfig();
        var serverEx = new FtpServerEx(cfg, server, root);

        var result = await serverEx.OpenFileRead(fileName);
        
        Assert.Equal(0, result.Session);
        Assert.Equal(0u, result.Size);
    }
}