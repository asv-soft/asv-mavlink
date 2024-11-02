using System;
using System.Buffers;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.ServerExIntegrationTests;

public class FtpServerExIntegrationTests
{
    private readonly FtpServerEx _serverEx;
    private IFtpClientEx _clientEx;
    private readonly FakeTimeProvider _fakeTime;
    private string _filePath;
    private string _directoryPath;
    private FtpServerExHelper _helper;
    private ITestOutputHelper _output;
    private const byte _fileSize = 5;

    public FtpServerExIntegrationTests(ITestOutputHelper output)
    {
        _helper = new FtpServerExHelper();
        _output = output;
        var fileName = "test.txt";
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = _helper.SetUpFileSystem(root);
        _directoryPath = fileSystem.Path.Combine(root, fileDirName);
        _filePath = Path.Combine(fileDirName, fileName);
        var filePath = fileSystem.Path.Combine(_directoryPath, fileName);
        fileSystem.AddFile(filePath, new MockFileData("12345"));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };

        _helper.SetUpClientAndServer(out var client, out var server, (packet) => true, (packet) => true);
        _serverEx = new FtpServerEx(cfg, server, fileSystem);
        _clientEx = new FtpClientEx(client, _fakeTime);
    }

    [Fact]
    public async Task DownloadFile_ClientExAtemptToDownloadFile_Success()
    {
        byte size = _fileSize;
        var arr = new byte[size];
        var buffer = new MemoryStream(arr);
        var request = new ReadRequest(0, 0, size);
        await _clientEx.Base.OpenFileRead(_filePath);
        await _clientEx.DownloadFile(_filePath, buffer);
        await _clientEx.Base.OpenFileRead(_filePath);
        var result = await _clientEx.Base.ReadFile(request);
        Assert.True(size == result.ReadSize());
    }

    [Fact]
    public async Task OpenFileRead_ClientWriteFile_Success()
    {
        // Act
        var result = await _clientEx.Base.OpenFileRead(_filePath);
        // Assert
        Assert.Equal(0, result.Session);
        Assert.Equal(_fileSize, result.Size);
    }

    [Fact]
    public async Task ListDirectory_ClientRefreshServer_Success()
    {
        // Act
        await _clientEx.Refresh("file");
        // Assert
        _clientEx.Entries.Do(_ => { }).Bind(out var result).Subscribe();
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Refresh_ClientTryToRefreshRootWithDirectorySeparatorChar_Success()
    {
        // Act
        await _clientEx.Refresh("/");
        // Assert
        _clientEx.Entries.Do(_ => { }).Bind(out var result).Subscribe();
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ListDirectory_ClientWithClientEx_Fault()
    {
        // Arrange
        using var memory = MemoryPool<char>.Shared.Rent();
        // Act
        await _clientEx.Refresh("file");
        // Assert
        _clientEx.Entries.Do(_ => { }).Bind(out var result).Subscribe();
        await Assert.ThrowsAsync<DirectoryNotFoundException>(async () =>
            await _serverEx.ListDirectory(_filePath, 5, memory.Memory, CancellationToken.None));
    }

    [Fact]
    public async Task CreateFile_ClientCreateNewFile_Success()
    {
        var path = Path.Combine(_directoryPath, "test1.txt");
        var result = await _clientEx.Base.CreateFile(path);
        Assert.Equal(0, result.ReadSize());
    }

    [Fact]
    public async Task WriteFile_ClientExWriteFileAndReadIt_Success()
    {
        var request = new WriteRequest(0, 0, 5);
        var readRequest = new ReadRequest(0, 0, 5);
        var readBuffer = new byte[5];
        var buffer = new byte[] { 5, 4, 3, 2, 1 };
        await _clientEx.Base.OpenFileWrite(_filePath);
        await _clientEx.Base.WriteFile(request, buffer);
        await _clientEx.Base.TerminateSession(0);
        await _clientEx.Base.OpenFileRead(_filePath);
        var readResult = await _clientEx.Base.ReadFile(readRequest, readBuffer);
        Assert.Equal(readResult.ReadCount, readBuffer.Length);
    }

    [Fact]
    public async Task CreateDirectory_ClientCreateDirectory_Success()
    {
        // Arrange
        var directory = Path.Combine(_directoryPath, "dir");
        using var memory = MemoryPool<char>.Shared.Rent();
        await _clientEx.Base.CreateDirectory(directory);
        await Assert.ThrowsAsync<FtpNackException>(async () =>
        {
            await _serverEx.ListDirectory(directory, 0, memory.Memory, CancellationToken.None);
        });
    }
}