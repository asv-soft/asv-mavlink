using System;
using System.Buffers;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(FtpServerEx))]
public class FtpServerExTest : ServerTestBase<FtpServerEx>
{
    private readonly TaskCompletionSource _tcs = new(); 
    private readonly CancellationTokenSource _cts;
    private readonly MockFileSystem _fileSystem;
    
    public FtpServerExTest(ITestOutputHelper log) : base(log)
    {
        _cts = new CancellationTokenSource(TimeSpan.FromSeconds(3), TimeProvider.System);
        _cts.Token.Register(() => _tcs.TrySetCanceled());
        _fileSystem = SetUpFileSystem(_config.RootDirectory);
    }
    
    private static MockFileSystem SetUpFileSystem(string root)
    {
        var mockFileCfg = new MockFileSystemOptions
        {
            CurrentDirectory = root
        };
        var fileSystem = new MockFileSystem(mockFileCfg);
        fileSystem.AddDirectory(mockFileCfg.CurrentDirectory);

        return fileSystem;
    }
    
    private readonly MavlinkFtpServerExConfig _config = new()
    {
        RootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp")
    };
    private readonly MavlinkFtpServerConfig _configBase = new()
    {
        
    };
    
    protected override FtpServerEx CreateClient(MavlinkIdentity identity, CoreServices core)
    {
        return new FtpServerEx(new FtpServer(identity, _configBase, core), _config, _fileSystem);
    }

    [Fact]
    public async Task OpenFileRead_Success()
    {
        // Arrange
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        // Act
        var handle = await Server.OpenFileRead(filePath, _cts.Token);

        // Assert
        Assert.Equal((uint) "Test content".Length, handle.Size);
    }

    [Fact]
    public async Task OpenFileWrite_Success()
    {
        // Arrange
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");

        // Act
        await Server.CreateFile(filePath, _cts.Token);
        var handle = await Server.OpenFileWrite(filePath, _cts.Token);

        // Assert
        Assert.Equal(1, handle.Session);
    }

    [Fact]
    public async Task FileRead_Success()
    {
        // Arrange
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));
        var handle = await Server.OpenFileRead("test.txt", _cts.Token);
        var buffer = new byte[4];

        // Act
        var result = await Server.FileRead(new ReadRequest(handle.Session, 0, (byte) buffer.Length), buffer, _cts.Token);

        // Assert
        Assert.Equal((byte) buffer.Length, result.ReadCount);
        Assert.Equal("Test", System.Text.Encoding.UTF8.GetString(buffer));
    }

    [Fact]
    public async Task WriteFile_Success()
    {
        // Arrange
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        const byte session = 0;
        const byte size = 5;
        var buffer = new byte[] { 1, 2, 3, 4, 5 };
        
        await Server.OpenFileWrite(filePath, _cts.Token);

        // Act
        await Server.WriteFile(new WriteRequest(session, 0, size), buffer, _cts.Token);
        await Server.TerminateSession(session);

        // Assert
        var writtenData = new byte[size];
        await Server.OpenFileRead(filePath, _cts.Token);
        await Server.FileRead(new ReadRequest(session, 0, size), writtenData);
        Assert.Equal(buffer, writtenData);
    }

    [Fact]
    public async Task RemoveFile_Success()
    {
        // Arrange
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        // Act
        await Server.RemoveFile("test.txt", _cts.Token);

        // Assert
        Assert.False(_fileSystem.File.Exists(filePath));
    }

    [Fact]
    public async Task ListDirectory_Success()
    {
        // Arrange
        var dirPath = _fileSystem.Path.Combine(_config.RootDirectory, "testDir");
        _fileSystem.AddDirectory(dirPath);
        _fileSystem.AddFile(_fileSystem.Path.Combine(dirPath, "file1.txt"), new MockFileData("File1 content"));
        _fileSystem.AddFile(_fileSystem.Path.Combine(dirPath, "file2.txt"), new MockFileData("File2 content"));
        using var memory = MemoryPool<char>.Shared.Rent(256);

        // Act
        var result = await Server.ListDirectory(dirPath, 0, memory.Memory);

        // Assert
        const string expectedOutput = "Ffile1.txt\t13\0Ffile2.txt\t13\0";
        var output = new string(memory.Memory.Span[..result]);
        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public async Task CalcFileCrc32_Success()
    {
        // Arrange
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        // Act
        var crc32 = await Server.CalcFileCrc32("test.txt", _cts.Token);

        var fileBytes = await _fileSystem.File.ReadAllBytesAsync(filePath, _cts.Token);
        var realCrc32 = Crc32Mavlink.Accumulate(fileBytes);
        
        // Assert
        Assert.Equal(crc32, realCrc32);
    }

    [Fact]
    public async Task RemoveDirectory_Success()
    {
        // Arrange
        var dirPath = _fileSystem.Path.Combine(_config.RootDirectory, "testDir");
        _fileSystem.AddDirectory(dirPath);

        // Act
        await Server.RemoveDirectory(dirPath, _cts.Token);

        // Assert
        Assert.False(_fileSystem.Directory.Exists(dirPath));
    }

    [Fact]
    public async Task CreateDirectory_Success()
    {
        // Arrange
        var directory = _fileSystem.Path.Combine(_config.RootDirectory, "fileDirName");

        // Act
        await Server.CreateDirectory(directory, _cts.Token);

        // Assert
        Assert.True(_fileSystem.Directory.Exists(directory));
    }
    
    [Fact]
    public async Task CreateFile_Success()
    {
        // Arrange
        var file = _fileSystem.Path.Combine(_config.RootDirectory, "file.txt");

        // Act
        await Server.CreateFile(file, _cts.Token);

        // Assert
        Assert.True(_fileSystem.File.Exists(file));
    }

    [Fact]
    public async Task Rename_Success()
    {
        // Arrange
        var oldPath = _fileSystem.Path.Combine(_config.RootDirectory, "oldName.txt");
        var newPath = _fileSystem.Path.Combine(_config.RootDirectory, "newName.txt");
        _fileSystem.AddFile(oldPath, new MockFileData("Test content"));

        // Act
        await Server.Rename(oldPath, newPath, _cts.Token);
        
        // Assert
        Assert.False(_fileSystem.File.Exists(oldPath));
        Assert.True(_fileSystem.File.Exists(newPath));
    }
}