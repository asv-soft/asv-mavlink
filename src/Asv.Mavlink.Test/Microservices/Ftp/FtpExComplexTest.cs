using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Common;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class FtpExComplexTest : ComplexTestBase<FtpClientEx, FtpServerEx>
{
    private MockFileSystem _fileSystem = null!;
    private readonly TaskCompletionSource<FileTransferProtocolPacket> _tcs = new();
    private readonly CancellationTokenSource _cts;
    
    private readonly MavlinkFtpServerConfig _serverConfig = new()
    {
        BurstReadChunkDelayMs = 0,
        NetworkId = 0
    };

    private readonly MavlinkFtpClientConfig _clientConfig = new()
    {
        TimeoutMs = 100,
        CommandAttemptCount = 5,
        TargetNetworkId = 0,
        BurstTimeoutMs = 100
    };

    private readonly MavlinkFtpServerExConfig _serverExConfig = new()
    {
        RootDirectory = AppDomain.CurrentDomain.BaseDirectory
    };
    
    public FtpExComplexTest(ITestOutputHelper log) : base(log)
    {
        _cts = new CancellationTokenSource();
        _cts.Token.Register(() => _tcs.TrySetCanceled());
    }
    
    protected override FtpServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        _fileSystem = SetUpFileSystem(_serverExConfig.RootDirectory);
        return new FtpServerEx(new FtpServer(identity, _serverConfig, core), _serverExConfig, _fileSystem);
    }

    protected override FtpClientEx CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        return new FtpClientEx(new FtpClient(identity, _clientConfig, core));
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

    [Theory]
    [InlineData(100, 239)]
    [InlineData(239, 239)]
    [InlineData(239, 10)]
    [InlineData(1024 * 10, 200)]
    public async Task DownloadFile_ToStream_Success(int size, byte partSize)
    {
        _ = Server;
        const string fileName = "test.txt";
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        var originContent = 
            Random.Shared.GetItems(
                Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(), 
                size);
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        using var streamToSave = new MemoryStream(size);

        // Act
        var progress = 0.0;
        await Client.DownloadFile(fileName, streamToSave, new CallbackProgress<double>(x => progress = x), partSize,
            _cts.Token);
        // Assert

        streamToSave.Seek(0, SeekOrigin.Begin);
        Assert.Equal(1, progress);
        Assert.Equal(originContent.Length, streamToSave.Length);
        foreach (var b in originContent)
        {
            Assert.Equal(streamToSave.ReadByte(), b);
        }
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
    }

    [Theory]
    [InlineData(100, 239)]
    [InlineData(239, 239)]
    [InlineData(239, 10)]
    [InlineData(1024 * 10, 200)]
    public async Task DownloadFile_ToBufferWriter_Success(int size, byte partSize)
    {
        _ = Server;
        const string fileName = "test.txt";
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        var originContent = 
            Random.Shared.GetItems(
                Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(), 
                size);
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        var buffer = new ArrayBufferWriter<byte>(size);
        var progress = 0.0;
        
        // Act
        await Client.DownloadFile(fileName, buffer, new CallbackProgress<double>(x => { progress = x; }), partSize,
            _cts.Token);
        
        // Assert
        Assert.Equal(originContent.Length, buffer.WrittenCount);
        Assert.Equal(1, progress);
        for (var index = 0; index < originContent.Length; index++)
        {
            Assert.Equal(originContent[index], buffer.WrittenSpan[index]);
        }
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
    }

    [Theory]
    [InlineData(100, 239)]
    [InlineData(239, 239)]
    [InlineData(239, 10)]
    [InlineData(1024 * 10, 200)]
    public async Task BurstDownloadFile_ToStream_Success(int size, byte partSize)
    {
        // Arrange
        _ = Server;
        const string fileName = "test.txt";
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        var originContent = 
            Random.Shared.GetItems(
                Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(), 
                size);
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        using var streamToSave = new MemoryStream(size);
        var progress = 0.0;
        
        // Act
        await Client.BurstDownloadFile(fileName, streamToSave, new CallbackProgress<double>(x => progress = x),
            partSize, _cts.Token);
        
        // Assert
        streamToSave.Seek(0, SeekOrigin.Begin);
        Assert.Equal(1, progress);
        Assert.Equal(originContent.Length, streamToSave.Length);
        foreach (var b in originContent)
        {
            Assert.Equal(streamToSave.ReadByte(), b);
        }
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
    }

    [Theory]
    [InlineData(100, 239)]
    [InlineData(239, 239)]
    [InlineData(239, 10)]
    [InlineData(1024 * 10, 200)]
    public async Task BurstDownloadFile_ToBufferWriter_Success(int size, byte partSize)
    {
        // Arrange
        _ = Server;
        const string fileName = "test.txt";
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        var originContent = 
            Random.Shared.GetItems(
                Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(), 
                size);
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        var bufferWriter = new ArrayBufferWriter<byte>();
        
        // Act
        var progress = 0.0;
        await Client.BurstDownloadFile(fileName, bufferWriter, new CallbackProgress<double>(x => progress = x),
            partSize, _cts.Token);
        
        // Assert
        Assert.Equal(1, progress);
        Assert.Equal(originContent.Length, bufferWriter.WrittenCount);
        for (var index = 0; index < originContent.Length; index++)
        {
            Assert.Equal(originContent[index], bufferWriter.WrittenSpan[index]);
        }
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
    }

    [Theory]
    [InlineData("/")]
    public async Task Refresh_NormalData_Success(string refreshPath)
    {
        // Arrange
        _ = Server;
        var localRoot = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "folder");
        _fileSystem.AddDirectory(localRoot);
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(localRoot, "innerFolder"));
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(localRoot, "innerFolder", "veryInnerFolder"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(localRoot, "innerFolder", "veryInnerFolder", "veryInnerFile1.log"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(localRoot, "innerFolder", "veryInnerFolder", "veryInnerFile2.log"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(localRoot, "innerFolder", "innerFile1.bmp"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(localRoot, "file1.txt"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(localRoot, "file2.txt"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(localRoot, "file3.txt"));

        
        const char d = MavlinkFtpHelper.DirectorySeparator;
        var expectedFiles = new[]
        {
            $"{d}",
            $"{d}folder{d}",
            $"{d}folder{d}innerFolder{d}",
            $"{d}folder{d}file1.txt",
            $"{d}folder{d}file2.txt",
            $"{d}folder{d}file3.txt",
            $"{d}folder{d}innerFolder{d}veryInnerFolder{d}",
            $"{d}folder{d}innerFolder{d}innerFile1.bmp",
            $"{d}folder{d}innerFolder{d}veryInnerFolder{d}veryInnerFile1.log",
            $"{d}folder{d}innerFolder{d}veryInnerFolder{d}veryInnerFile2.log",
        };


        // Act
        await Client.Refresh(refreshPath, true, _cts.Token);

        // Assert
        Client.Entries.Keys.Should().Equal(expectedFiles);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
    }

    [Theory]
    [InlineData(100, 239)]
    [InlineData(239, 239)]
    [InlineData(239, 10)]
    [InlineData(1024 * 10, 200)]
    public async Task DownloadFile_ToStream_ThrowsByCancellation(int size, byte partSize)
    {
        // Arrange
        _ = Server;
        await _cts.CancelAsync();
        const string fileName = "test.txt";
        Debug.Assert(_fileSystem != null, nameof(_fileSystem) + " != null");
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        var originContent = 
            Random.Shared.GetItems(
                Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(), 
                size);
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        using var streamToSave = new MemoryStream(size);
        
        // Act
        var task = Client.DownloadFile(fileName, streamToSave, new CallbackProgress<double>(_ => { }), partSize,
            _cts.Token);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await task;
        });
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
    }

    [Theory]
    [InlineData(100, 239)]
    [InlineData(239, 239)]
    [InlineData(239, 10)]
    [InlineData(1024 * 10, 200)]
    public async Task DownloadFile_ToBufferWriter_ThrowsByCancellation(int size, byte partSize)
    {
        // Arrange
        _ = Server;
        await _cts.CancelAsync();
        const string fileName = "test.txt";
        Debug.Assert(_fileSystem != null, nameof(_fileSystem) + " != null");
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        var originContent = 
            Random.Shared.GetItems(
                Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(), 
                size);
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        var buffer = new ArrayBufferWriter<byte>();
        
        // Act
        var task = Client.DownloadFile(fileName, buffer, new CallbackProgress<double>(_ => { }), partSize,
            _cts.Token);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await task;
        });
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
    }

    [Theory]
    [InlineData(100, 239)]
    [InlineData(239, 239)]
    [InlineData(239, 10)]
    [InlineData(1024 * 10, 200)]
    public async Task BurstDownloadFile_ToStream_ThrowsByCancellation(int size, byte partSize)
    {
        // Arrange
        _ = Server;
        await _cts.CancelAsync();
        const string fileName = "test.txt";
        Debug.Assert(_fileSystem != null, nameof(_fileSystem) + " != null");
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        var originContent = 
            Random.Shared.GetItems(
                Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(), 
                size);
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        using var streamToSave = new MemoryStream(size);
        
        // Act
        var task = Client.BurstDownloadFile(fileName, streamToSave, new CallbackProgress<double>(_ => { }), partSize,
            _cts.Token);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await task;
        });
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
    }

    [Theory]
    [InlineData(100, 239)]
    [InlineData(239, 239)]
    [InlineData(239, 10)]
    [InlineData(1024 * 10, 200)]
    public async Task BurstDownloadFile_ToBufferWriter_ThrowsByCancellation(int size, byte partSize)
    {
        // Arrange
        _ = Server;
        await _cts.CancelAsync();
        const string fileName = "test.txt";
        Debug.Assert(_fileSystem != null, nameof(_fileSystem) + " != null");
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        var originContent = 
            Random.Shared.GetItems(
                Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(), 
                size);
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        var buffer = new ArrayBufferWriter<byte>();
        
        // Act 
        var task = Client.BurstDownloadFile(fileName, buffer, new CallbackProgress<double>(_ => { }), partSize,
            _cts.Token);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await task;
        });
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
    }

    [Theory]
    [InlineData("/")]
    [InlineData("")]
    public async Task Refresh_NormalData_ThrowsByCancellation(string refreshPath)
    {
        // Arrange
        _ = Server;
        await _cts.CancelAsync();
        Debug.Assert(_fileSystem != null, nameof(_fileSystem) + " != null");
        var localRoot = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "folder");
        _fileSystem.AddDirectory(localRoot);
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(localRoot, "file1.txt"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(localRoot, "file2.txt"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(localRoot, "file3.txt"));
        
        // Act
        var task = Client.Refresh(refreshPath, true, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await task;
        });
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal((int)Link.Client.Statistic.TxMessages, (int)Link.Server.Statistic.RxMessages);
    }
    
    

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cts.Dispose();
        }

        base.Dispose(disposing);
    }
}