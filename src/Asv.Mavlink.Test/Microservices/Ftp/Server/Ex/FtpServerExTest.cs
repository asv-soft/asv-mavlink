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
    public async Task OpenFileRead_SingleFileThatExists_Success()
    {
        // Arrange
        const string fileName = "test.txt";
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        // Act
        var handle = await Server.OpenFileRead(fileName, _cts.Token);

        // Assert
        Assert.Equal((uint)"Test content".Length, handle.Size);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task OpenFileWrite_SingleFileThatExists_Success()
    {
        // Arrange
        const string fileName = "test.txt";

        // Act
        await Server.CreateFile(fileName, _cts.Token);
        var handle = await Server.OpenFileWrite(fileName, _cts.Token);

        // Assert
        Assert.Equal(1, handle.Session);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task FileRead_SingleFileThatExistAndNotEmpty_Success()
    {
        // Arrange
        const string fileName = "test.txt";
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));
        var handle = await Server.OpenFileRead(fileName, _cts.Token);
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
    public async Task WriteFile_SingleFileThatExistsAndIsEmpty_Success()
    {
        // Arrange
        const string fileName = "test.txt";
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, fileName);
        const byte session = 0;
        const byte size = 5;
        var buffer = new byte[] { 1, 2, 3, 4, 5 };
        _fileSystem.AddEmptyFile(filePath);
        await Server.OpenFileWrite(fileName, _cts.Token);

        // Act
        await Server.WriteFile(new WriteRequest(session, 0, size), buffer, _cts.Token);
        await Server.TerminateSession(session);

        // Assert
        var writtenData = new byte[size];
        await Server.OpenFileRead(fileName, _cts.Token);
        await Server.FileRead(new ReadRequest(session, 0, size), writtenData);
        Assert.Equal(buffer, writtenData);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task RemoveFile_SingleFileThatExists_Success()
    {
        // Arrange
        const string fileName = "test.txt";
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        // Act
        await Server.RemoveFile(fileName, _cts.Token);

        // Assert
        Assert.False(_fileSystem.File.Exists(filePath));
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task ListDirectory_FromCustomFolder_Success()
    {
        // Arrange
        const string directoryName = "testDir";
        const string fileName1 = "file1.txt";
        const string fileName2 = "file2.txt";
        var dirPath = _fileSystem.Path.Combine(_config.RootDirectory, directoryName);
        _fileSystem.AddDirectory(dirPath);
        _fileSystem.AddFile(
            _fileSystem.Path.Combine(dirPath, fileName1),
            new MockFileData("File1 content")
        );
        _fileSystem.AddFile(
            _fileSystem.Path.Combine(dirPath, fileName2),
            new MockFileData("File2 content")
        );
        using var memory = MemoryPool<char>.Shared.Rent(256);

        // Act
        var result = await Server.ListDirectory(directoryName, 0, memory.Memory);

        // Assert
        const string expectedOutput = $"F{fileName1}\t13\0F{fileName2}\t13\0";
        var output = new string(memory.Memory.Span[..result]);
        Assert.Equal(expectedOutput, output);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task ListDirectory_FromRootFolder_Success()
    {
        // Arrange
        var relativeRootPath = _fileSystem.Path.DirectorySeparatorChar.ToString();
        const string fileName1 = "file1.txt";
        const string fileName2 = "file2.txt";
        var dirPath = _config.RootDirectory;
        _fileSystem.AddDirectory(dirPath);
        _fileSystem.AddFile(
            _fileSystem.Path.Combine(dirPath, fileName1),
            new MockFileData("File1 content")
        );
        _fileSystem.AddFile(
            _fileSystem.Path.Combine(dirPath, fileName2),
            new MockFileData("File2 content")
        );
        using var memory = MemoryPool<char>.Shared.Rent(256);

        // Act
        var result = await Server.ListDirectory(relativeRootPath, 0, memory.Memory);

        // Assert
        const string expectedOutput = $"F{fileName1}\t13\0F{fileName2}\t13\0";
        var output = new string(memory.Memory.Span[..result]);
        Assert.Equal(expectedOutput, output);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task CalcFileCrc32_SingleFile_Success()
    {
        // Arrange
        const string fileName = "test.txt";
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        // Act
        var crc32 = await Server.CalcFileCrc32(fileName, _cts.Token);

        var fileBytes = await _fileSystem.File.ReadAllBytesAsync(filePath, _cts.Token);
        var realCrc32 = Crc32Mavlink.Accumulate(fileBytes);

        // Assert
        Assert.Equal(crc32, realCrc32);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task RemoveDirectory_SingleDirectoryThatExists_Success()
    {
        // Arrange
        const string directoryName = "testDir";
        var dirPath = _fileSystem.Path.Combine(_config.RootDirectory, directoryName);
        _fileSystem.AddDirectory(dirPath);

        // Act
        await Server.RemoveDirectory(directoryName, _cts.Token);

        // Assert
        Assert.False(_fileSystem.Directory.Exists(dirPath));
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task CreateDirectory_SingleDirectory_Success()
    {
        // Arrange
        const string directory = "fileDirName";
        var realDirectoryPath = _fileSystem.Path.Combine(_config.RootDirectory, directory);

        // Act
        await Server.CreateDirectory(directory, _cts.Token);

        // Assert
        Assert.True(_fileSystem.Directory.Exists(realDirectoryPath));
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task CreateFile_SingleFile_Success()
    {
        // Arrange
        const string file = "file.txt";
        var fullFilePath = _fileSystem.Path.Combine(_config.RootDirectory, file);

        // Act
        await Server.CreateFile(file, _cts.Token);

        // Assert
        Assert.True(_fileSystem.File.Exists(fullFilePath));
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task Rename_SingleFile_Success()
    {
        // Arrange
        const string fileNameOld = "oldName.txt";
        const string fileNameNew = "newName.txt";
        var oldPath = _fileSystem.Path.Combine(_config.RootDirectory, fileNameOld);
        var newPath = _fileSystem.Path.Combine(_config.RootDirectory, fileNameNew);
        _fileSystem.AddFile(oldPath, new MockFileData("Test content"));

        // Act
        await Server.Rename(fileNameOld, fileNameNew, _cts.Token);

        // Assert
        Assert.False(_fileSystem.File.Exists(oldPath));
        Assert.True(_fileSystem.File.Exists(newPath));
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task Rename_SingleFolder_Success()
    {
        // Arrange
        const string folderNameOld = "oldName";
        const string folderNameNew = "newName";
        var oldPath = _fileSystem.Path.Combine(_config.RootDirectory, folderNameOld);
        var newPath = _fileSystem.Path.Combine(_config.RootDirectory, folderNameNew);
        _fileSystem.AddFile(oldPath, new MockFileData("Test content"));

        // Act
        await Server.Rename(folderNameOld, folderNameNew, _cts.Token);

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
        const string rootDirectoryName = "temp";
        var root = _fileSystem.Path.Combine(_config.RootDirectory, rootDirectoryName);
        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, fileName);
        var relativeFilePath = _fileSystem.Path.Combine(rootDirectoryName, fileDirName, fileName);
        _fileSystem.AddFile(filePath, new MockFileData(string.Empty));

        // Act
        var result = await Server.OpenFileWrite(relativeFilePath);

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
        const string rootFolderName = "temp";
        var root = _fileSystem.Path.Combine(_config.RootDirectory, rootFolderName);
        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var relativeFileDir= _fileSystem.Path.Combine(rootFolderName, fileDirName);
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
            relativeFileDir,
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
        const string tempFolderName = "temp";
        var relativeFileDir = _fileSystem.Path.Combine(tempFolderName, fileDirName);
        var root = _fileSystem.Path.Combine(_config.RootDirectory, tempFolderName);
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
        var task = Server.ListDirectory(relativeFileDir, 5, memory.Memory, CancellationToken.None);

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
        const string tempFolderName = "temp";
        var root = _fileSystem.Path.Combine(_config.RootDirectory, tempFolderName);
        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, fileName);
        var relativeFilePath = _fileSystem.Path.Combine(tempFolderName, fileDirName, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("12345"));
        var request = new WriteRequest(0, 0, 5);
        var readRequest = new ReadRequest(0, 0, 5);
        var readBuffer = new byte[5];
        var buffer = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        await Server.OpenFileWrite(relativeFilePath);
        await Server.WriteFile(request, buffer);
        await Server.TerminateSession(0);
        await Server.OpenFileRead(relativeFilePath);
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
        const string tempFolderName = "temp";
        const int fileSize = 5;
        var root = _fileSystem.Path.Combine(_config.RootDirectory, tempFolderName);
        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, fileName);
        var filePath1 = _fileSystem.Path.Combine(fileDir, fileName1);
        var relativeFilePath = _fileSystem.Path.Combine(tempFolderName, fileDirName, fileName);
        var relativeFilePath1 = _fileSystem.Path.Combine(tempFolderName, fileDirName, fileName1);
        
        _fileSystem.AddFile(filePath, new MockFileData("12345"));
        _fileSystem.AddFile(filePath1, new MockFileData("12345"));
        var request = new ReadRequest(0, 0, fileSize);
        var request1 = new ReadRequest(1, 0, fileSize);

        var buffer = new byte[fileSize];

        // Act
        await Server.OpenFileRead(relativeFilePath);
        await Server.OpenFileRead(relativeFilePath1);
        await Server.TerminateSession(0);
        var task = Server.FileRead(request, buffer);
        var activeSessionResult = await Server.FileRead(request1, buffer);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(fileSize, activeSessionResult.ReadCount);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task ResetSessions_ReadAfterReset_ThrowsNack()
    {
        // Arrange
        const string fileName = "test.txt";
        const string fileDirName = "file";
        const string tempFolderName = "temp";
        var root = _fileSystem.Path.Combine(_config.RootDirectory, tempFolderName);

        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, fileName);
        var relativeFilePath = _fileSystem.Path.Combine(tempFolderName, fileDirName, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("12345"));

        var request = new ReadRequest(0, 0, 5);
        var buffer = new byte[2];

        // Act
        await Server.OpenFileRead(relativeFilePath);
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
        const string tempFolderName = "temp";
        var root = _fileSystem.Path.Combine(_config.RootDirectory, tempFolderName);

        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, fileName);
        var relativeFilePath = _fileSystem.Path.Combine(tempFolderName, fileDirName, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("1234567890"));
        var request = new TruncateRequest(relativeFilePath, 5);

        // Act
        await Server.TruncateFile(request);
        var result = await Server.OpenFileRead(relativeFilePath);

        // Assert
        Assert.True(result.Size == request.Offset);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task ListDirectory_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        const string fileName1 = "file1.txt";
        const string fileName2 = "file2.txt";
        const string directoryName = "testDir";
        var dirPath = _fileSystem.Path.Combine(_config.RootDirectory, directoryName);
        _fileSystem.AddDirectory(dirPath);
        _fileSystem.AddFile(
            _fileSystem.Path.Combine(dirPath, fileName1),
            new MockFileData("File1 content")
        );
        _fileSystem.AddFile(
            _fileSystem.Path.Combine(dirPath, fileName2),
            new MockFileData("File2 content")
        );
        using var memory = MemoryPool<char>.Shared.Rent(256);

        // Act
        var task = Server.ListDirectory(directoryName, 0, memory.Memory, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task OpenFileRead_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        const string fileName = "test.txt";
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        // Act
        var task = Server.OpenFileRead(fileName, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task OpenFileWrite_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        const string fileName = "test.txt";
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        // Act
        var task = Server.OpenFileWrite(fileName, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task RemoveFile_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        const string fileName = "test.txt";
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        // Act
        var task = Server.RemoveFile(fileName, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task CreateFile_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        const string fileName = "test.txt";

        // Act
        var task = Server.CreateFile(fileName, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task CreateDirectory_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        const string directoryName = "testDir";
        var dirPath = _fileSystem.Path.Combine(_config.RootDirectory, directoryName);

        // Act
        var task = Server.CreateDirectory(directoryName, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task RemoveDirectory_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        const string directoryName = "testDir";
        var dirPath = _fileSystem.Path.Combine(_config.RootDirectory, directoryName);
        _fileSystem.AddDirectory(dirPath);

        // Act
        var task = Server.RemoveDirectory(directoryName, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task Rename_Cancel_ThrowsNack()
    {
        // Arrange
        await _cts.CancelAsync();
        const string fileNameOld = "oldName.txt";
        const string fileNameNew = "newName.txt";
        var oldPath = _fileSystem.Path.Combine(_config.RootDirectory, fileNameOld);
        var newPath = _fileSystem.Path.Combine(_config.RootDirectory, fileNameNew);
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
        const string file = "test.txt";
        var fullFilePath = _fileSystem.Path.Combine(_config.RootDirectory, file);
        _fileSystem.AddFile(fullFilePath, new MockFileData("1234567890"));
        var request = new TruncateRequest(file, 5);

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
        const string file = "test.txt";
        var fullFilePath = _fileSystem.Path.Combine(_config.RootDirectory, file);
        _fileSystem.AddFile(fullFilePath, new MockFileData("12345"));
        var handle = await Server.OpenFileRead(file, CancellationToken.None);
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
        const string file = "test.txt";
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, file);
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        // Act
        var task = Server.CalcFileCrc32(file, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
}
