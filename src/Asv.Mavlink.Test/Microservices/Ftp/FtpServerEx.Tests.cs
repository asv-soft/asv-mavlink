using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
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

    private MockFileSystem SetUpFileSystem(string root)
    {
        var mockFileCfg = new MockFileSystemOptions
        {
            CurrentDirectory = root
        };
        var fileSystem = new MockFileSystem(mockFileCfg);
        fileSystem.AddDirectory(mockFileCfg.CurrentDirectory);

        return fileSystem;
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
        // Arrange
        var fileName = "test.txt";
        var root = Path.Combine("D:", "file");
        var fileSystem = SetUpFileSystem(root);
        var fileDir = fileSystem.Path.Combine(root, "file");
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        fileSystem.AddFile(filePath, new MockFileData(string.Empty));
        SetUpMicroservice(out var client, out var server, (packet) => true, (packet) => true);
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);

        // Act
        var result = await serverEx.OpenFileRead(filePath);
        
        // Assert
        Assert.Equal(0, result.Session);
        Assert.Equal(0u, result.Size);
    }
}