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
    private readonly CancellationTokenSource _cts;
    private readonly MockFileSystem _fileSystem;

    public FtpServerExTest(ITestOutputHelper log)
        : base(log)
    {
        _cts = new CancellationTokenSource();
        _fileSystem = SetUpFileSystem(_config.RootDirectory);
    }

    private static MockFileSystem SetUpFileSystem(string root)
    {
        var mockFileCfg = new MockFileSystemOptions { CurrentDirectory = root };
        var fileSystem = new MockFileSystem(mockFileCfg);
        fileSystem.AddDirectory(mockFileCfg.CurrentDirectory);

        return fileSystem;
    }

    private readonly MavlinkFtpServerExConfig _config =
        new() { RootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp") };

    private readonly MavlinkFtpServerConfig _configBase = new();

    protected override FtpServerEx CreateServer(MavlinkIdentity identity, CoreServices core)
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
        Assert.Equal((uint)"Test content".Length, handle.Size);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        var result = await Server.FileRead(
            new ReadRequest(handle.Session, 0, (byte)buffer.Length),
            buffer,
            _cts.Token
        );

        // Assert
        Assert.Equal((byte)buffer.Length, result.ReadCount);
        Assert.Equal("Test", System.Text.Encoding.UTF8.GetString(buffer));
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task WriteFile_Success()
    {
        // Arrange
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        const byte session = 0;
        const byte size = 5;
        var buffer = new byte[] { 1, 2, 3, 4, 5 };
        _fileSystem.AddEmptyFile(filePath);
        await Server.OpenFileWrite(filePath, _cts.Token);

        // Act
        await Server.WriteFile(new WriteRequest(session, 0, size), buffer, _cts.Token);
        await Server.TerminateSession(session);

        // Assert
        var writtenData = new byte[size];
        await Server.OpenFileRead(filePath, _cts.Token);
        await Server.FileRead(new ReadRequest(session, 0, size), writtenData);
        Assert.Equal(buffer, writtenData);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task ListDirectory_Success()
    {
        // Arrange
        var dirPath = _fileSystem.Path.Combine(_config.RootDirectory, "testDir");
        _fileSystem.AddDirectory(dirPath);
        _fileSystem.AddFile(
            _fileSystem.Path.Combine(dirPath, "file1.txt"),
            new MockFileData("File1 content")
        );
        _fileSystem.AddFile(
            _fileSystem.Path.Combine(dirPath, "file2.txt"),
            new MockFileData("File2 content")
        );
        using var memory = MemoryPool<char>.Shared.Rent(256);

        // Act
        var result = await Server.ListDirectory(dirPath, 0, memory.Memory);

        // Assert
        const string expectedOutput = "Ffile1.txt\t13\0Ffile2.txt\t13\0";
        var output = new string(memory.Memory.Span[..result]);
        Assert.Equal(expectedOutput, output);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task OpenFileWrite_WithEmptyFile_Success()
    {
        // Arrange
        const string fileName = "test.txt";
        const string fileDirName = "file";
        var root = _fileSystem.Path.Combine(_config.RootDirectory, "temp");
        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, fileName);
        _fileSystem.AddFile(filePath, new MockFileData(string.Empty));

        // Act
        var result = await Server.OpenFileWrite(filePath);

        // Assert
        Assert.Equal(0, result.Session);
        Assert.Equal(0u, result.Size);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        const string fileDirName = "files";
        var root = _fileSystem.Path.Combine(_config.RootDirectory, "temp");
        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, "test.txt");
        var filePath2 = _fileSystem.Path.Combine(fileDir, "test2.txt");
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(fileDir, "Folder1"));
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(fileDir, "Folder2"));
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(fileDir, "Folder3"));
        _fileSystem.AddFile(filePath, new MockFileData("Something"));
        _fileSystem.AddFile(filePath2, new MockFileData(string.Empty));

        using var memory = MemoryPool<char>.Shared.Rent();

        // Act
        var result = await Server.ListDirectory(
            fileDir,
            offset,
            memory.Memory,
            CancellationToken.None
        );

        // Assert
        var listOfEntries = memory.Memory[..result].ToString();
        Assert.Equal(realListOfEntries.Length, result);
        Assert.Equal(realListOfEntries, listOfEntries);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task ListDirectory_PastTheEndOfFile_ThrowsEOF()
    {
        // Arrange
        const string fileDirName = "files";
        var root = _fileSystem.Path.Combine(_config.RootDirectory, "temp");
        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, "test.txt");
        var filePath2 = _fileSystem.Path.Combine(fileDir, "test2.txt");
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(fileDir, "Folder1"));
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(fileDir, "Folder2"));
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(fileDir, "Folder3"));
        _fileSystem.AddFile(filePath, new MockFileData("Something"));
        _fileSystem.AddFile(filePath2, new MockFileData(string.Empty));
        using var memory = MemoryPool<char>.Shared.Rent();

        // Act
        var task = Server.ListDirectory(fileDir, 5, memory.Memory, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<FtpNackEndOfFileException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task WriteFile_NoDuplicateFile_Success()
    {
        // Arrange
        const string fileName = "test.txt";
        const string fileDirName = "file";
        var root = _fileSystem.Path.Combine(_config.RootDirectory, "temp");
        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("12345"));
        var request = new WriteRequest(0, 0, 5);
        var readRequest = new ReadRequest(0, 0, 5);
        var readBuffer = new byte[5];
        var buffer = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        await Server.OpenFileWrite(filePath);
        await Server.WriteFile(request, buffer);
        await Server.TerminateSession(0);
        await Server.OpenFileRead(filePath);
        var readResult = await Server.FileRead(readRequest, readBuffer);

        // Assert
        Assert.True(readResult.ReadCount == buffer.Length);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task TerminateSession_ReadAfterResetOneOfActiveSessions_ThrowsNack()
    {
        // Arrange
        const string fileName = "test.txt";
        const string fileName1 = "test1.txt";
        const string fileDirName = "file";
        var root = _fileSystem.Path.Combine(_config.RootDirectory, "temp");
        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, fileName);
        var filePath1 = _fileSystem.Path.Combine(fileDir, fileName1);
        _fileSystem.AddFile(filePath, new MockFileData("12345"));
        _fileSystem.AddFile(filePath1, new MockFileData("12345"));
        var request = new ReadRequest(0, 0, 5);
        var request1 = new ReadRequest(1, 0, 5);

        var buffer = new byte[5];

        // Act
        await Server.OpenFileRead(filePath);
        await Server.OpenFileRead(filePath1);
        await Server.TerminateSession(0);
        var task = Server.FileRead(request, buffer);
        var activeSessionResult = await Server.FileRead(request1, buffer);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.True(activeSessionResult.ReadCount == 5);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task ResetSessions_ReadAfterReset_ThrowsNack()
    {
        // Arrange
        const string fileName = "test.txt";
        const string fileDirName = "file";
        var root = _fileSystem.Path.Combine(_config.RootDirectory, "temp");

        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("12345"));

        var request = new ReadRequest(0, 0, 5);
        var buffer = new byte[2];

        // Act
        await Server.OpenFileRead(filePath);
        await Server.ResetSessions();
        var task = Server.FileRead(request, buffer);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task TruncateFile_TruncatePart_Success()
    {
        // Arrange
        const string fileName = "test.txt";
        const string fileDirName = "file";
        var root = _fileSystem.Path.Combine(_config.RootDirectory, "temp");

        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("1234567890"));
        var request = new TruncateRequest(filePath, 5);

        // Act
        await Server.TruncateFile(request);
        var result = await Server.OpenFileRead(filePath);

        // Assert
        Assert.True(result.Size == request.Offset);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task ListDirectory_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        var dirPath = _fileSystem.Path.Combine(_config.RootDirectory, "testDir");
        _fileSystem.AddDirectory(dirPath);
        _fileSystem.AddFile(
            _fileSystem.Path.Combine(dirPath, "file1.txt"),
            new MockFileData("File1 content")
        );
        _fileSystem.AddFile(
            _fileSystem.Path.Combine(dirPath, "file2.txt"),
            new MockFileData("File2 content")
        );
        using var memory = MemoryPool<char>.Shared.Rent(256);

        // Act
        var task = Server.ListDirectory(dirPath, 0, memory.Memory, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task OpenFileRead_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        // Act
        var task = Server.OpenFileRead(filePath, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task OpenFileWrite_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        // Act
        var task = Server.OpenFileWrite(filePath, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task RemoveFile_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        // Act
        var task = Server.RemoveFile(filePath, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task CreateFile_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");

        // Act
        var task = Server.CreateFile(filePath, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task CreateDirectory_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        var dirPath = _fileSystem.Path.Combine(_config.RootDirectory, "testDir");

        // Act
        var task = Server.CreateDirectory(dirPath, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task RemoveDirectory_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        var dirPath = _fileSystem.Path.Combine(_config.RootDirectory, "testDir");
        _fileSystem.AddDirectory(dirPath);

        // Act
        var task = Server.RemoveDirectory(dirPath, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task Rename_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        var oldPath = _fileSystem.Path.Combine(_config.RootDirectory, "oldName.txt");
        var newPath = _fileSystem.Path.Combine(_config.RootDirectory, "newName.txt");
        _fileSystem.AddFile(oldPath, new MockFileData("Test content"));

        // Act
        var task = Server.Rename(oldPath, newPath, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task TruncateFile_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("1234567890"));
        var request = new TruncateRequest(filePath, 5);

        // Act
        var task = Server.TruncateFile(request, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task FileRead_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("12345"));
        var handle = await Server.OpenFileRead(filePath, CancellationToken.None);
        var buffer = new byte[5];
        var request = new ReadRequest(handle.Session, 0, 5);

        // Act
        var task = Server.FileRead(request, buffer, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task WriteFile_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("12345"));
        var request = new WriteRequest(0, 0, 5);
        var buffer = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var task = Server.WriteFile(request, buffer, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task CalcFileCrc32_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        // Act
        var task = Server.CalcFileCrc32(filePath, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
}
