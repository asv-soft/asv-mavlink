using System;
using System.Buffers;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class FtpServerExTests
{
    private readonly ITestOutputHelper _output;
    private readonly FakeTimeProvider _fakeTime;

    public FtpServerExTests(ITestOutputHelper output)
    {
        _fakeTime = new FakeTimeProvider();
        _output = output;
    }

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
        server = new FtpServer(new MavlinkFtpServerConfig(), link.Server, serverId, serverSeq,
            TaskPoolScheduler.Default, new TestLogger(_output, "SERVER"));
    }
    
    private void SetUpClientAndServer(out IFtpClient client, out IFtpServer server,
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

    #region OpenFileRead

    [Fact]
    public async Task OpenFileRead_WithEmptyFile_Success()
    {
        // Arrange
        var fileName = "test.txt";
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        var relativeFilePath = Path.Combine(fileDirName, fileName);
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        fileSystem.AddFile(filePath, new MockFileData(string.Empty));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);

        // Act
        var result = await serverEx.OpenFileRead(relativeFilePath);
        
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
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        var relativeFilePath = Path.Combine(fileDirName, fileName);
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        fileSystem.AddFile(filePath, new MockFileData(string.Empty));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);

        // Act
        var result = await serverEx.OpenFileWrite(relativeFilePath);
        
        // Assert
        Assert.Equal(0, result.Session);
        Assert.Equal(0u, result.Size);
    }

    #endregion

    #region ListDirectory

    [Fact]
    public async Task ListDirectory_WithClientEx_Success()
    {
        // Arrange
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpClientAndServer(out var client, out var server, (packet) => true, (packet) => true);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        var filePath = fileSystem.Path.Combine(fileDir, "test.txt");
        var filePath2 = fileSystem.Path.Combine(fileDir, "test2.txt");
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder1"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder2"));
        fileSystem.AddDirectory(Path.Combine(fileDir, "Folder3"));
        fileSystem.AddFile(filePath, new MockFileData("Something"));
        fileSystem.AddFile(filePath2, new MockFileData(string.Empty));
        var clientEx = new FtpClientEx(client, _fakeTime);
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);
        
        // Act
        await clientEx.Refresh(fileDirName);
        
        // Assert
        clientEx.Entries.Do(_ => {}).Bind(out var result).Subscribe();
        Assert.Equal(6, result.Count);
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
        var fileDirName = "files";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
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
        var result = await serverEx.ListDirectory(fileDirName, offset, memory.Memory, CancellationToken.None);
        
        // Assert
        var listOfEntries = memory.Memory[..result].ToString();
        Assert.Equal(realListOfEntries.Length, result);
        Assert.Equal(realListOfEntries, listOfEntries);
    }

    [Fact]
    public async Task ListDirectory_PastTheEndOfFile_ThrowsEOF()
    {
        // Arrange
        var fileDirName = "files";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
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
        await Assert.ThrowsAsync<FtpNackEndOfFileException>(async () => await serverEx.ListDirectory(fileDirName, 5, memory.Memory, CancellationToken.None));
    }
    
    [Fact]
    public async Task ListDirectory_TooManyEntries_Success()
    {
        // Arrange
        var fileDirName = "files";
        var realListOfEntries =
            "DFolder1\0DFolder2\0DFolder3\0DFolder11" +
            "\0DFolder22\0DFolder33\0DFolder111\0DFolder222" +
            "\0DFolder333\0DFolder1111\0DFolder2222\0DFolder3333" +
            "\0DFolder11111\0DFolder22222\0DFolder33333\0DFolder111111" +
            "\0DFolder222222\0DFolder333333\0DFolder1111111\0DFolder2222222\0";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
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
        var result = await serverEx.ListDirectory(fileDirName, 0, memory.Memory, CancellationToken.None);
        
        // Assert
        var listOfEntries = memory.Memory[..result].ToString();
        Assert.Equal(realListOfEntries.Length, result);
        Assert.Equal(realListOfEntries, listOfEntries);
    }
    
    [Fact]
    public async Task ListDirectory_TooManyEntriesGetAllEntries_Success()
    {
        // Arrange
        var fileDirName = "files";
        var realListOfEntries =
            "DFolder1\0DFolder2\0DFolder3\0DFolder11" +
            "\0DFolder22\0DFolder33\0DFolder111\0DFolder222" +
            "\0DFolder333\0DFolder1111\0DFolder2222\0DFolder3333" +
            "\0DFolder11111\0DFolder22222\0DFolder33333\0DFolder111111" +
            "\0DFolder222222\0DFolder333333\0DFolder1111111\0DFolder2222222" +
            "\0DFolder3333333\0";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
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
        var result = await serverEx.ListDirectory(fileDirName, 0, memory.Memory, CancellationToken.None);
        var result2 = await serverEx.ListDirectory(fileDirName, 20, memory2.Memory, CancellationToken.None);
        
        // Assert
        var listOfEntries = memory.Memory[..result] + memory2.Memory[..result2].ToString();
        Assert.Equal(realListOfEntries.Length, listOfEntries.Length);
        Assert.Equal(realListOfEntries, listOfEntries);
    }

    #endregion

    #region CreateFile

    [Fact]
    public async Task CreateFile_EmptyFileFolder_Success()
    {
        // Arrange
        var fileName = "test.txt";
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        var relativeFilePath = Path.Combine(fileDirName, fileName);
        fileSystem.AddDirectory(fileDir);
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);

        // Act
        var result = await serverEx.CreateFile(relativeFilePath);
        
        // Assert
        Assert.Equal(0, result);
        Assert.True(fileSystem.File.Exists(filePath));
    }

    #endregion

    #region WriteFile

    

    #endregion

    #region CreateDirectory

    [Fact]
    public async Task CreateDirectory_NoDuplicateDirectory_Success()
    {
        // Arrange
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);

        // Act
        await serverEx.CreateDirectory(fileDirName);
        
        // Assert
        Assert.True(fileSystem.Directory.Exists(fileDir));
    }

    #endregion
    
    #region CalcCrc32

    

    #endregion

    #region TerminateSession

    

    #endregion
    
    #region ResetSession
    
    
    
    #endregion
    
    #region Rename

    

    #endregion

    #region ReadFile

    

    #endregion

    #region BurstReadFile

    

    #endregion

    #region RemoveFile

    [Fact]
    public async Task RemoveFile_FileExists_Success()
    {
        // Arrange
        var fileName = "test.txt";
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        var relativeFilePath = Path.Combine(fileDirName, fileName);
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        fileSystem.AddFile(filePath, new MockFileData(string.Empty));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);

        // Act
        await serverEx.RemoveFile(relativeFilePath);
        
        // Assert
        Assert.False(fileSystem.File.Exists(filePath));
    }

    #endregion
    
    #region RemoveDirectory

    [Fact]
    public async Task RemoveDirectory_PathExists_Success()
    {
        // Arrange
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = SetUpFileSystem(root);
        SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        fileSystem.AddDirectory(fileDir);
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);

        // Act
        await serverEx.RemoveDirectory(fileDir);
        
        // Assert
        Assert.False(fileSystem.Directory.Exists(fileDir));
    }

    #endregion

    #region TruncateFile

    

    #endregion
}