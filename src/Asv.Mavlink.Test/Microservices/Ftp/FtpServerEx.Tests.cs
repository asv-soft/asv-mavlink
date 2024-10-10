using System;
using System.Buffers;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
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
    public async Task ListDirectory_PastTheEndOfFile_ThrowsEOF()
    {
        // Arrange
        var root = Path.Combine("D:", "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, "files");
        var filePath = fileSystem.Path.Combine(fileDir, "test.txt");
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

        // Act + Assert
        await Assert.ThrowsAsync<FtpNackEndOfFileException>(async () => await serverEx.ListDirectory(fileDir, 5, memory.Memory, CancellationToken.None));
    }
    
    [Fact]
    public async Task ListDirectory_TooManyEntries_Success()
    {
        // Arrange
        var realListOfEntries =
            "DFolder1\0DFolder2\0DFolder3\0DFolder11" +
            "\0DFolder22\0DFolder33\0DFolder111\0DFolder222" +
            "\0DFolder333\0DFolder1111\0DFolder2222\0DFolder3333" +
            "\0DFolder11111\0DFolder22222\0DFolder33333\0DFolder111111" +
            "\0DFolder222222\0DFolder333333\0DFolder1111111\0DFolder2222222\0";
        var root = Path.Combine("D:", "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, "files");
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder1"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder2"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder3"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder11"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder22"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder33"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder111"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder222"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder333"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder1111"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder2222"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder3333"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder11111"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder22222"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder33333"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder111111"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder222222"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder333333"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder1111111"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder2222222"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder3333333"));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);
        using var memory = MemoryPool<char>.Shared.Rent();

        // Act
        var result = await serverEx.ListDirectory(fileDir, 0, memory.Memory, CancellationToken.None);
        
        // Assert
        var listOfEntries = memory.Memory[..result].ToString();
        Assert.Equal(realListOfEntries.Length, result);
        Assert.Equal(realListOfEntries, listOfEntries);
    }
    
    [Fact]
    public async Task ListDirectory_TooManyEntriesGetAllEntries_Success()
    {
        // Arrange
        var realListOfEntries =
            "DFolder1\0DFolder2\0DFolder3\0DFolder11" +
            "\0DFolder22\0DFolder33\0DFolder111\0DFolder222" +
            "\0DFolder333\0DFolder1111\0DFolder2222\0DFolder3333" +
            "\0DFolder11111\0DFolder22222\0DFolder33333\0DFolder111111" +
            "\0DFolder222222\0DFolder333333\0DFolder1111111\0DFolder2222222" +
            "\0DFolder3333333\0";
        var root = Path.Combine("D:", "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, "files");
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder1"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder2"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder3"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder11"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder22"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder33"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder111"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder222"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder333"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder1111"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder2222"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder3333"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder11111"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder22222"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder33333"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder111111"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder222222"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder333333"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder1111111"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder2222222"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder3333333"));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);
        using var memory = MemoryPool<char>.Shared.Rent();
        using var memory2 = MemoryPool<char>.Shared.Rent();

        // Act
        var result = await serverEx.ListDirectory(fileDir, 0, memory.Memory, CancellationToken.None);
        var result2 = await serverEx.ListDirectory(fileDir, 20, memory2.Memory, CancellationToken.None);
        
        // Assert
        var listOfEntries = memory.Memory[..result] + memory2.Memory[..result2].ToString();
        Assert.Equal(realListOfEntries.Length, listOfEntries.Length);
        Assert.Equal(realListOfEntries, listOfEntries);
    }
    
    [Theory]
    [InlineData(0, "DFolder1\0DFolder2\0DFolder3\0Ftest.txt\t9\0Ftest2.txt\t0\0")]
    [InlineData(1, "DFolder2\0DFolder3\0Ftest.txt\t9\0Ftest2.txt\t0\0")]
    [InlineData(2, "DFolder3\0Ftest.txt\t9\0Ftest2.txt\t0\0")]
    [InlineData(3, "Ftest.txt\t9\0Ftest2.txt\t0\0")]
    [InlineData(4, "Ftest2.txt\t0\0")]
    public async Task ListDirectory_WithOffset_Success(uint offset, string realListOfEntries)
    {
        // Arrange
        var root = Path.Combine("D:", "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, "files");
        var filePath = fileSystem.Path.Combine(fileDir, "test.txt");
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
        var result = await serverEx.ListDirectory(fileDir, offset, memory.Memory, CancellationToken.None);
        
        // Assert
        var listOfEntries = memory.Memory[..result].ToString();
        Assert.Equal(realListOfEntries.Length, result);
        Assert.Equal(realListOfEntries, listOfEntries);
    }

    #endregion
}