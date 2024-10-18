using System;
using System.Buffers;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink;
using Asv.Mavlink.V2.Common;
using DynamicData;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class FtpServerExTests
{
    private readonly ITestOutputHelper _output;
    private readonly FakeTimeProvider _fakeTime;
    private FtpServerExHelper _helper;

    public FtpServerExTests(ITestOutputHelper output)
    {
        _helper = new FtpServerExHelper();
        _fakeTime = new FakeTimeProvider();
        _output = output;
    }

    #region OpenFileRead

    [Fact]
    public async Task OpenFileRead_WithEmptyFile_Success()
    {
        // Arrange
        var fileName = "test.txt";
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
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

    [Theory]
    [InlineData("1234567890")]
    [InlineData(
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi vestibulum vel lorem sed pretium. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Sed condimentum tempus est, vel volutpat dui euismod eu. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.")]
    [InlineData("**(*(*@@&#&!#&!@^#__)_+[][][][]")]
    public async Task OpenFileRead_WithDataInFile_Success(string fileData)
    {
        // Arrange
        var fileName = "test.txt";
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        var relativeFilePath = Path.Combine(fileDirName, fileName);
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        fileSystem.AddFile(filePath, new MockFileData(fileData));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);

        // Act
        var result = await serverEx.OpenFileRead(relativeFilePath);
        Assert.True(fileSystem.File.Exists(relativeFilePath));
        // Assert
        Assert.Equal(0, result.Session);
        Assert.Equal((uint)fileData.Length, result.Size);
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
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
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
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpClientAndServer(out var client, out var server, (packet) => true, (packet) => true);
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
        clientEx.Entries.Do(_ => { }).Bind(out var result).Subscribe();
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
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
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
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
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
        await Assert.ThrowsAsync<FtpNackEndOfFileException>(async () =>
            await serverEx.ListDirectory(fileDirName, 5, memory.Memory, CancellationToken.None));
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
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
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
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
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
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
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

    [Fact]
    public async Task WriteFile_NoDuplicateFile_Success()
    {
        var fileName = "test.txt";
        var fileName1 = "test1.txt";
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        var relativeFilePath1 = Path.Combine(fileDirName, fileName1);
        fileSystem.AddFile(filePath, new MockFileData("12345"));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);
        var request = new WriteRequest(0, 0, 5);
        var readRequest = new ReadRequest(0, 0, 5);
        var readBuffer = new byte[5];
        var buffer = new byte[] { 1, 2, 3, 4, 5 };
        // Act
        await serverEx.OpenFileWrite(relativeFilePath1);
        await serverEx.WriteFile(request, buffer);
        await serverEx.TerminateSession(0);
        await serverEx.OpenFileRead(relativeFilePath1);
        var readResult = await serverEx.FileRead(readRequest, readBuffer);
        // Assert
        Assert.True(readResult.ReadCount == buffer.Length);
    }

    #endregion

    #region CreateDirectory

    [Fact]
    public async Task CreateDirectory_NoDuplicateDirectory_Success()
    {
        // Arrange
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
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

    [Fact]
    public async Task CalcCrc32_CalculateCyclicRedundancyCheck_Success()
    {
        // Arrange
        var fileName = "test.txt";
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        var relativeFilePath = Path.Combine(fileDirName, fileName);
        fileSystem.AddFile(filePath, new MockFileData("12345"));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);
        // Act
        var result = await serverEx.CalcFileCrc32(relativeFilePath);
        // Assert
        _output.WriteLine(result.ToString());
        Assert.True(result != 0);
    }

    #endregion

    #region TerminateSession

    [Fact]
    public async Task TerminateSession_ReadAfterResetOneOfActiveSessions_Fault()
    {
        // Arrange
        var fileName = "test.txt";
        var fileName1 = "test1.txt";
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        var filePath1 = fileSystem.Path.Combine(fileDir, fileName1);
        var relativeFilePath = Path.Combine(fileDirName, fileName);
        var relativeFilePath1 = Path.Combine(fileDirName, fileName1);
        fileSystem.AddFile(filePath, new MockFileData("12345"));
        fileSystem.AddFile(filePath1, new MockFileData("12345"));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);
        var request = new ReadRequest(0, 0, 5);
        var request1 = new ReadRequest(1, 0, 5);

        var buffer = new byte[5];
        // Act
        await serverEx.OpenFileRead(relativeFilePath);
        await serverEx.OpenFileRead(relativeFilePath1);
        await serverEx.TerminateSession(0);
        // Assert

        await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await serverEx.FileRead(request, buffer); });
        var result1 = await serverEx.FileRead(request1, buffer);
        Assert.True(result1.ReadCount == 5);
    }

    #endregion

    #region ResetSessions

    [Fact]
    public async Task ResetSessions_ReadAfterReset_Fault()
    {
        // Arrange
        var fileName = "test.txt";
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        var relativeFilePath = Path.Combine(fileDirName, fileName);
        fileSystem.AddFile(filePath, new MockFileData("12345"));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);
        var request = new ReadRequest(0, 0, 5);
        var buffer = new byte[2];

        // Act
        await serverEx.OpenFileRead(relativeFilePath);
        await serverEx.ResetSessions();

        //Assert
        var result = await Assert.ThrowsAsync<FtpNackException>(async () =>
        {
            await serverEx.FileRead(request, buffer);
        });
        _output.WriteLine($"{result.Source} {result.Message}");
    }

    #endregion

    #region Rename

    [Fact]
    public async Task RenameFile_Rename_Success()
    {
        // Arrange
        var fileName = "test.txt";
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        var beginFilePath = Path.Combine(fileDirName, fileName);
        var finalPath = Path.Combine(fileDirName, "renamed_test.txt");
        fileSystem.AddFile(filePath, new MockFileData("12345"));

        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);

        // Act
        await serverEx.Rename(beginFilePath, finalPath);
        var result = await serverEx.OpenFileRead(finalPath);

        // Assert
        Assert.True(result.Size == 5);
    }

    #endregion

    #region ReadFile

    [Fact]
    public async Task ReadFile_ReadFromFile_Success()
    {
        // Arrange
        var fileName = "test.txt";
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        var relativeFilePath = Path.Combine(fileDirName, fileName);
        fileSystem.AddFile(filePath, new MockFileData("12345"));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);
        var request = new ReadRequest(0, 0, 3);
        var buffer = new byte[5];

        // Act
        await serverEx.OpenFileRead(relativeFilePath);
        var result = await serverEx.FileRead(request, buffer);

        // Assert
        Assert.Equal(request.Take, result.ReadCount);
    }

    #endregion

    #region BurstReadFile
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(239)]
    public async Task BurstReadFile_WithClientEx_Success(byte partSize)
    {
        // Arrange
        var fileName = "test.txt";
        var fileDirName = "file";
        var fileContent = "Something good band test read me pls32, gogogo";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = _helper.SetUpFileSystem(root);
        _helper.SetUpClientAndServer(
            out var client, 
            out var server, 
            (packet) => true, 
            (packet) => true
        );
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        fileSystem.AddFile(filePath, new MockFileData(fileContent));
        var clientEx = new FtpClientEx(client, _fakeTime);
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        var serverEx = new FtpServerEx(cfg, server, fileSystem);
        using var streamToSave = new MemoryStream();
        var progress = new Progress<double>(); 
        // Act
        await clientEx.BurstDownloadFile(
            Path.Combine(fileDirName, fileName), 
            streamToSave, 
            progress, 
            partSize, 
            CancellationToken.None
        );
        
        // Assert
        var result = await _helper.ConvertStreamToString(streamToSave, 0);
        Assert.Equal(fileContent, result);
    }

#endregion

#region RemoveFile

[Fact]
public async Task RemoveFile_FileExists_Success()
{
    // Arrange
    var fileName = "test.txt";
    var fileDirName = "file";
    var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
    var fileSystem = _helper.SetUpFileSystem(root);
    _helper.SetUpServer(out var server);
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

[Theory]
[InlineData("test1.txt")]
[InlineData("path/test1.txt")]
[InlineData("path/path/test1.txt")]
public async Task RemoveFile_FileExists_Fault(string wrongFileName)
{
    // Arrange
    var fileName = "test.txt";
    var fileDirName = "file";
    var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
    var fileSystem = _helper.SetUpFileSystem(root);
    _helper.SetUpServer(out var server);
    var fileDir = fileSystem.Path.Combine(root, fileDirName);
    var relativeFilePath = Path.Combine(fileDirName, wrongFileName);
    var filePath = fileSystem.Path.Combine(fileDir, fileName);
    fileSystem.AddFile(filePath, new MockFileData(string.Empty));
    var cfg = new MavlinkFtpServerExConfig
    {
        RootDirectory = root,
    };
    var serverEx = new FtpServerEx(cfg, server, fileSystem);

    // Act & Assert
    Assert.False(fileSystem.File.Exists(relativeFilePath));
    var result =
        await Assert.ThrowsAsync<FtpNackException>(async () => { await serverEx.RemoveFile(relativeFilePath); });
    _output.WriteLine($"{result.Message}");
}

#endregion

#region RemoveDirectory

[Fact]
public async Task RemoveDirectory_PathExists_Success()
{
    // Arrange
    var fileDirName = "file";
    var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
    var fileSystem = _helper.SetUpFileSystem(root);
    _helper.SetUpServer(out var server);
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

[Theory]
[InlineData("*")]
[InlineData("/root/")]
[InlineData("ade,q;l,")]
[InlineData(".txt")]
[InlineData("1233434")]
public async Task RemoveDirectory_PathExists_Fault(string wrongPath)
{
    // Arrange
    var fileDirName = "file";
    var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
    var fileSystem = _helper.SetUpFileSystem(root);
    _helper.SetUpServer(out var server);
    var fileDir = fileSystem.Path.Combine(root, fileDirName);
    fileSystem.AddDirectory(fileDir);
    var cfg = new MavlinkFtpServerExConfig
    {
        RootDirectory = root,
    };
    var serverEx = new FtpServerEx(cfg, server, fileSystem);
    fileDir = fileSystem.Path.Combine(root, wrongPath);
    // Act & Assert
    var result =
        await Assert.ThrowsAsync<FtpNackException>(async () => { await serverEx.RemoveDirectory(fileDir); });
    _output.WriteLine($"{result.Message}");
}

#endregion

#region TruncateFile

[Theory]
[InlineData("1234567", 1)]
[InlineData("1234567", 5)]
[InlineData("1234567", 7)]
public async Task TruncateFile_TruncatePart_Success(string mockData, byte trimlength)
{
    // Arrange
    var fileName = "test.txt";
    var fileDirName = "file";
    var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
    var fileSystem = _helper.SetUpFileSystem(root);
    _helper.SetUpServer(out var server);
    var fileDir = fileSystem.Path.Combine(root, fileDirName);
    var relativeFilePath = Path.Combine(fileDirName, fileName);
    var filePath = fileSystem.Path.Combine(fileDir, fileName);
    fileSystem.AddFile(filePath, new MockFileData(mockData));
    var cfg = new MavlinkFtpServerExConfig
    {
        RootDirectory = root,
    };
    var serverEx = new FtpServerEx(cfg, server, fileSystem);
    var request = new TruncateRequest(relativeFilePath, trimlength);
    // Act
    await serverEx.TruncateFile(request);
    var result = await serverEx.OpenFileRead(relativeFilePath);
    // Assert
    _output.WriteLine($"{result.Size}=={request.Offset}");
    Assert.True(result.Size == request.Offset);
}

[Theory]
[InlineData("1234567", 8)]
[InlineData("1234567", 0)]
[InlineData("", 3)]
public async Task TruncateFile_TruncatePart_Fault(string mockData, byte trimlength)
{
    // Arrange
    var fileName = "test.txt";
    var fileDirName = "file";
    var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
    var fileSystem = _helper.SetUpFileSystem(root);
    _helper.SetUpServer(out var server);
    var fileDir = fileSystem.Path.Combine(root, fileDirName);
    var relativeFilePath = Path.Combine(fileDirName, fileName);
    var filePath = fileSystem.Path.Combine(fileDir, fileName);
    fileSystem.AddFile(filePath, new MockFileData(mockData));
    var cfg = new MavlinkFtpServerExConfig
    {
        RootDirectory = root,
    };
    var serverEx = new FtpServerEx(cfg, server, fileSystem);
    var request = new TruncateRequest(relativeFilePath, trimlength);
    //Act & Assert

    var result = await Assert.ThrowsAsync<FtpNackException>(async () => { await serverEx.TruncateFile(request); });
    _output.WriteLine($"{result.Message}");
}

#endregion

}