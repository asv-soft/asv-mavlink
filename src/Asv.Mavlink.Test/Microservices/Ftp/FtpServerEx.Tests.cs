using System.Buffers;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Reactive.Concurrency;
using System.Threading;
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
    
    private void SetUpServer(out IFtpServer server)
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
        server = new FtpServer(new MavlinkFtpServerConfig(), link.Server, serverId, serverSeq,
            TaskPoolScheduler.Default, new TestLogger(_output, "SERVER"));
    }

    #region OpenFileRead

    [Fact]
    public async Task OpenFileRead_WithEmptyFile_Success()
    {
        // Arrange
        var fileName = "test.txt";
        var root = Path.Combine("D:", "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, "file");
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        fileSystem.AddFile(filePath, new MockFileData(string.Empty));
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

    #endregion

    #region OpenFileWrite

    [Fact]
    public async Task OpenFileWrite_WithEmptyFile_Success()
    {
        // Arrange
        var fileName = "test.txt";
        var root = Path.Combine("D:", "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, "file");
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        fileSystem.AddFile(filePath, new MockFileData(string.Empty));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);

        // Act
        var result = await serverEx.OpenFileWrite(filePath);
        
        // Assert
        Assert.Equal(0, result.Session);
        Assert.Equal(0u, result.Size);
    }

    #endregion

    #region ListDirectory

    [Fact]
    public async Task ListDirectory_LittleAmountOfEntries_Success()
    {
        // Arrange
        var fileName = "test.txt";
        var root = Path.Combine("D:", "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, "files");
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        var filePath2 = fileSystem.Path.Combine(fileDir, "test2.txt");
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder1"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder2"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder3"));
        fileSystem.AddFile(filePath, new MockFileData("Something"));
        fileSystem.AddFile(filePath2, new MockFileData(string.Empty));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);
        using var memory = MemoryPool<char>.Shared.Rent();

        // Act
        var result = await serverEx.ListDirectory(fileDir, 0, memory.Memory, CancellationToken.None);
        
        // Assert
        Assert.Equal(52, result);
    }

    #endregion
}