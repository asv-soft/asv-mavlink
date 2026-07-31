using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Xunit;

namespace Asv.Mavlink.Test;

public class FtpExComplexTest(ITestOutputHelper log)
    : ComplexTestBase<FtpClientEx, FtpServerEx>(log)
{
    private MockFileSystem _fileSystem = null!;
    private readonly CancellationTokenSource _cts = new();

    private readonly MavlinkFtpServerConfig _serverConfig =
        new() { BurstReadChunkDelayMs = 0, NetworkId = 0 };

    private readonly MavlinkFtpClientConfig _clientConfig =
        new()
        {
            TimeoutMs = 100,
            CommandAttemptCount = 5,
            TargetNetworkId = 0,
            BurstTimeoutMs = 100,
        };

    private readonly MavlinkFtpServerExConfig _serverExConfig =
        new() { RootDirectory = AppDomain.CurrentDomain.BaseDirectory };

    protected override FtpServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        _fileSystem = SetUpFileSystem(_serverExConfig.RootDirectory);
        return new FtpServerEx(
            new FtpServer(identity, _serverConfig, core),
            _serverExConfig,
            _fileSystem
        );
    }

    protected override FtpClientEx CreateClient(
        MavlinkClientIdentity identity,
        IMavlinkContext core
    )
    {
        return new FtpClientEx(new FtpClient(identity, _clientConfig, core));
    }

    private static MockFileSystem SetUpFileSystem(string root)
    {
        var mockFileCfg = new MockFileSystemOptions { CurrentDirectory = root };
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
        // Arrange
        _ = Server;
        const string fileName = "test.txt";
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        var originContent = Random.Shared.GetItems(
            Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(),
            size
        );
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        using var streamToSave = new MemoryStream(size);

        // Act
        var progress = 0.0;
        await Client.DownloadFile(
            fileName,
            streamToSave,
            new CallbackProgress<double>(x => progress = x),
            partSize,
            _cts.Token
        );
        // Assert

        streamToSave.Seek(0, SeekOrigin.Begin);
        Assert.Equal(1, progress);
        Assert.Equal(originContent.Length, streamToSave.Length);
        foreach (var b in originContent)
        {
            Assert.Equal(streamToSave.ReadByte(), b);
        }
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory]
    [InlineData(100, 239)]
    [InlineData(239, 239)]
    [InlineData(239, 10)]
    [InlineData(1024 * 10, 200)]
    public async Task DownloadFile_ToBufferWriter_Success(int size, byte partSize)
    {
        // Arrange
        _ = Server;
        const string fileName = "test.txt";
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        var originContent = Random.Shared.GetItems(
            Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(),
            size
        );
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        var buffer = new ArrayBufferWriter<byte>(size);
        var progress = 0.0;

        // Act
        await Client.DownloadFile(
            fileName,
            buffer,
            new CallbackProgress<double>(x =>
            {
                progress = x;
            }),
            partSize,
            _cts.Token
        );

        // Assert
        Assert.Equal(originContent.Length, buffer.WrittenCount);
        Assert.Equal(1, progress);
        for (var index = 0; index < originContent.Length; index++)
        {
            Assert.Equal(originContent[index], buffer.WrittenSpan[index]);
        }
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        var originContent = Random.Shared.GetItems(
            Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(),
            size
        );
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        using var streamToSave = new MemoryStream(size);
        var progress = 0.0;

        // Act
        await Client.BurstDownloadFile(
            fileName,
            streamToSave,
            new CallbackProgress<double>(x => progress = x),
            partSize,
            _cts.Token
        );

        // Assert
        streamToSave.Seek(0, SeekOrigin.Begin);
        Assert.Equal(1, progress);
        Assert.Equal(originContent.Length, streamToSave.Length);
        foreach (var b in originContent)
        {
            Assert.Equal(streamToSave.ReadByte(), b);
        }
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        var originContent = Random.Shared.GetItems(
            Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(),
            size
        );
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        var bufferWriter = new ArrayBufferWriter<byte>();

        // Act
        var progress = 0.0;
        await Client.BurstDownloadFile(
            fileName,
            bufferWriter,
            new CallbackProgress<double>(x => progress = x),
            partSize,
            _cts.Token
        );

        // Assert
        Assert.Equal(1, progress);
        Assert.Equal(originContent.Length, bufferWriter.WrittenCount);
        for (var index = 0; index < originContent.Length; index++)
        {
            Assert.Equal(originContent[index], bufferWriter.WrittenSpan[index]);
        }
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task Refresh_RootPath_Success()
    {
        // Arrange
        _ = Server;
        var refreshPath = MavlinkFtpHelper.DirectorySeparator.ToString();
        var localRoot = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "folder");
        _fileSystem.AddDirectory(localRoot);
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(localRoot, "innerFolder"));
        _fileSystem.AddDirectory(
            _fileSystem.Path.Combine(localRoot, "innerFolder", "veryInnerFolder")
        );
        _fileSystem.AddEmptyFile(
            _fileSystem.Path.Combine(
                localRoot,
                "innerFolder",
                "veryInnerFolder",
                "veryInnerFile1.log"
            )
        );
        _fileSystem.AddEmptyFile(
            _fileSystem.Path.Combine(
                localRoot,
                "innerFolder",
                "veryInnerFolder",
                "veryInnerFile2.log"
            )
        );
        _fileSystem.AddEmptyFile(
            _fileSystem.Path.Combine(localRoot, "innerFolder", "innerFile1.bmp")
        );
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(localRoot, "file1.txt"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(localRoot, "file2.txt"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(localRoot, "file3.txt"));

        var d = MavlinkFtpHelper.DirectorySeparator;
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
        Assert.Equal(expectedFiles, Client.Entries.Keys);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        var originContent = Random.Shared.GetItems(
            Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(),
            size
        );
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        using var streamToSave = new MemoryStream(size);

        // Act
        var task = Client.DownloadFile(
            fileName,
            streamToSave,
            new CallbackProgress<double>(_ => { }),
            partSize,
            _cts.Token
        );

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await task;
        });
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        var originContent = Random.Shared.GetItems(
            Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(),
            size
        );
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        var buffer = new ArrayBufferWriter<byte>();

        // Act
        var task = Client.DownloadFile(
            fileName,
            buffer,
            new CallbackProgress<double>(_ => { }),
            partSize,
            _cts.Token
        );

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await task;
        });
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        var originContent = Random.Shared.GetItems(
            Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(),
            size
        );
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        using var streamToSave = new MemoryStream(size);

        // Act
        var task = Client.BurstDownloadFile(
            fileName,
            streamToSave,
            new CallbackProgress<double>(_ => { }),
            partSize,
            _cts.Token
        );

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await task;
        });
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
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
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        var originContent = Random.Shared.GetItems(
            Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(),
            size
        );
        _fileSystem.AddFile(filePath, new MockFileData(originContent));

        var buffer = new ArrayBufferWriter<byte>();

        // Act
        var task = Client.BurstDownloadFile(
            fileName,
            buffer,
            new CallbackProgress<double>(_ => { }),
            partSize,
            _cts.Token
        );

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await task;
        });
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }
    
    [Fact]
    public async Task Refresh_RootPath_ThrowsByCancellation()
    {
        // Arrange
        _ = Server;
        await _cts.CancelAsync();
        var refreshPath = _fileSystem.Path.DirectorySeparatorChar.ToString();
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
    }
    
    [Theory]
    [InlineData("/folder", "/folder/")]
    [InlineData("/folder/", "/folder/")]
    [InlineData("folder", "folder/")]
    [InlineData("folder/", "folder/")]
    public async Task RemoveDirectoryAsync_NonRecursive_Success(
        string clientPath,
        string canonicalPath
    )
    {
        // Arrange
        var server = Server;
        var dir = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "folder");
        _fileSystem.AddDirectory(dir);
        await Client.Refresh(clientPath, false, _cts.Token);
        var cachedKeysBeforeRemoval = Client.Entries.Keys.ToList();
        var calls = RecordCalls(server);

        // Act
        await Client.RemoveDirectory(clientPath, recursive: false, _cts.Token);

        // Assert
        Assert.Equal(new[] { canonicalPath }, calls.RemovedDirectories);
        Assert.False(_fileSystem.Directory.Exists(dir));
        Assert.Contains(canonicalPath, cachedKeysBeforeRemoval);
        Assert.DoesNotContain(canonicalPath, Client.Entries.Keys);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Theory]
    [InlineData("/folder", "/folder/")]
    [InlineData("/folder/", "/folder/")]
    [InlineData("folder", "folder/")]
    [InlineData("folder/", "folder/")]
    public async Task RemoveDirectoryAsync_Recursive_Success(
        string clientPath,
        string canonicalPath
    )
    {
        // Arrange
        var server = Server;
        var root = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "folder");
        _fileSystem.AddDirectory(root);
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(root, "child"));
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(root, "child", "grand"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(root, "file1.txt"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(root, "child", "file2.txt"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(root, "child", "grand", "file3.txt"));
        await Client.Refresh(clientPath, true, _cts.Token);
        var cachedKeysBeforeRemoval = Client.Entries.Keys.ToList();
        var calls = RecordCalls(server);

        // Act
        await Client.RemoveDirectory(clientPath, recursive: true, _cts.Token);

        // Assert
        Assert.Equal(
            new[] { canonicalPath, $"{canonicalPath}child/", $"{canonicalPath}child/grand/" },
            calls.ListedPaths.Distinct().Order(StringComparer.Ordinal)
        );
        Assert.Equal(
            new[]
            {
                $"{canonicalPath}child/file2.txt",
                $"{canonicalPath}child/grand/file3.txt",
                $"{canonicalPath}file1.txt",
            },
            calls.RemovedFiles.Order(StringComparer.Ordinal)
        );
        Assert.Equal(
            new[] { $"{canonicalPath}child/grand/", $"{canonicalPath}child/", canonicalPath },
            calls.RemovedDirectories
        );
        Assert.False(_fileSystem.Directory.Exists(root));
        Assert.Contains(
            cachedKeysBeforeRemoval,
            key => key.StartsWith(canonicalPath, StringComparison.Ordinal)
        );
        Assert.DoesNotContain(
            Client.Entries.Keys,
            key => key.StartsWith(canonicalPath, StringComparison.Ordinal)
        );
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Fact]
    public async Task Refresh_DirectoryWithPrefixSibling_PreservesSiblingCache()
    {
        // Arrange
        _ = Server;
        var folder = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "folder");
        var sibling = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "folder2");
        _fileSystem.AddDirectory(folder);
        _fileSystem.AddDirectory(sibling);
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(sibling, "file.txt"));
        await Client.Refresh("/", true, _cts.Token);

        // Act
        await Client.Refresh("/folder", false, _cts.Token);

        // Assert
        Assert.Contains("/folder2/", Client.Entries.Keys);
        Assert.Contains("/folder2/file.txt", Client.Entries.Keys);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Refresh_EmptyOrWhitespacePath_ThrowsArgumentException(string path)
    {
        // Arrange
        _ = Server;

        // Act
        var task = Client.Refresh(path, false, _cts.Token);

        // Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() => task);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RemoveDirectoryAsync_EmptyOrWhitespacePath_ThrowsArgumentException(string path)
    {
        // Arrange
        _ = Server;

        // Act
        var task = Client.RemoveDirectory(path, recursive: false, _cts.Token);

        // Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() => task);
    }

    [Fact]
    public async Task Refresh_MultiSegmentRelativePath_MaterializesParentChain()
    {
        // Arrange
        _ = Server;
        var dir = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "tes", "bet");
        _fileSystem.AddDirectory(dir);
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(dir, "x.txt"));

        // Act
        await Client.Refresh("tes/bet", false, _cts.Token);

        // Assert
        Assert.Contains("tes/", Client.Entries.Keys);
        Assert.Contains("tes/bet/", Client.Entries.Keys);
        Assert.Contains("tes/bet/x.txt", Client.Entries.Keys);
        Assert.Equal(string.Empty, Client.Entries["tes/"].ParentPath);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Fact]
    public async Task RemoveDirectoryAsync_MountedRoot_EvictsAllCacheProjections()
    {
        // Arrange
        _ = Server;
        var root = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "folder");
        _fileSystem.AddDirectory(root);
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(root, "child"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(root, "file1.txt"));
        var sibling = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "folder2");
        _fileSystem.AddDirectory(sibling);
        await Client.Refresh("/", true, _cts.Token);
        await Client.Refresh("folder", true, _cts.Token);
        var cachedKeysBeforeRemoval = Client.Entries.Keys.ToList();

        // Act
        await Client.RemoveDirectory("folder", recursive: true, _cts.Token);

        // Assert
        Assert.False(_fileSystem.Directory.Exists(root));
        Assert.Contains("/folder/", cachedKeysBeforeRemoval);
        Assert.Contains("folder/", cachedKeysBeforeRemoval);
        Assert.DoesNotContain(
            Client.Entries.Keys,
            key =>
                key.TrimStart(MavlinkFtpHelper.DirectorySeparator)
                    .StartsWith("folder/", StringComparison.Ordinal)
        );
        Assert.Contains("/folder2/", Client.Entries.Keys);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Fact]
    public async Task RemoveDirectoryAsync_NonRecursive_ThrowsNack()
    {
        // Arrange
        _ = Server;
        var folderName = "folder";
        var root = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, folderName);
        _fileSystem.AddDirectory(root);
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(root, "child"));
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(root, "child", "grand"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(root, "file1.txt"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(root, "child", "file2.txt"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(root, "child", "grand", "file3.txt"));

        // Act
        var task = Client.RemoveDirectory($"/{folderName}", recursive: false, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.True(_fileSystem.Directory.Exists(root));
        Assert.True(_fileSystem.File.Exists(_fileSystem.Path.Combine(root, "file1.txt")));
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Fact]
    public async Task RemoveDirectoryAsync_Recursive_ThrowsByCancellation()
    {
        // Arrange
        _ = Server;
        var folderName = "folder";
        var root = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, folderName);
        _fileSystem.AddDirectory(root);
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(root, "file1.txt"));
        await _cts.CancelAsync();

        // Act
        var task = Client.RemoveDirectory($"/{folderName}", recursive: true, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => task);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Fact]
    public async Task Refresh_ListingFailsAfterFirstPage_KeepsPreviousCache()
    {
        // Arrange
        var server = Server;
        var dir = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "folder");
        _fileSystem.AddDirectory(dir);
        // ~15 entries fit into one ListDirectory reply, so 40 files need several pages
        for (var i = 0; i < 40; i++)
        {
            _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(dir, $"file_{i:D2}.txt"));
        }

        await Client.Refresh("/folder", false, _cts.Token);
        var cachedKeysBeforeFailure = Client.Entries.Keys.Order().ToList();
        server.Base.ListDirectory = (path, offset, buffer, cancel) =>
            offset == 0
                ? server.ListDirectory(path, offset, buffer, cancel)
                : throw new FtpNackException(FtpOpcode.ListDirectory, NackError.Fail);

        // Act
        var task = Client.Refresh("/folder", false, _cts.Token);

        // Assert
        var ex = await Assert.ThrowsAsync<FtpNackException>(() => task);
        Assert.Equal(NackError.Fail, ex.NackError);
        Assert.Equal(42, cachedKeysBeforeFailure.Count);
        Assert.Equal(cachedKeysBeforeFailure, Client.Entries.Keys.Order());
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    /// <summary>
    /// Mirrors the "Use case" section of Writerside/topics/Microservices/Ftp/FtpClientEx.md.
    /// </summary>
    [Fact]
    public async Task DocsUseCase_RemoveDirectoryRecursive_RemovesFolderFromServerAndCache()
    {
        // Arrange
        _ = Server;
        var cancel = _cts.Token;
        await Client.Base.CreateDirectory("/FolderExample", cancel);
        await Client.Base.CreateDirectory("/FolderExample/dir1", cancel);
        await Client.Base.CreateDirectory("/FolderExample/dir2", cancel);
        await Client.Base.CreateDirectory("/FolderExample/dir3", cancel);
        var res = await Client.Base.CreateFile("/FolderExample/dir1/temp.txt", cancel);
        await Client.Base.TerminateSession(res.ReadSession(), cancel);
        res = await Client.Base.CreateFile("/FolderExample/dir2/temp.txt", cancel);
        await Client.Base.TerminateSession(res.ReadSession(), cancel);
        await Client.Refresh("/", cancel: cancel);
        var cachedKeysBeforeRemoval = Client.Entries.Keys.ToList();

        // Act
        await Client.RemoveDirectory("/FolderExample", true, cancel);
        var cachedKeysAfterRemoval = Client.Entries.Keys.ToList();
        await Client.Refresh("/", cancel: cancel);

        // Assert
        Assert.Contains("/FolderExample/dir1/temp.txt", cachedKeysBeforeRemoval);
        Assert.Contains("/FolderExample/dir2/temp.txt", cachedKeysBeforeRemoval);
        Assert.Contains("/FolderExample/dir3/", cachedKeysBeforeRemoval);
        Assert.False(
            _fileSystem.Directory.Exists(
                _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "FolderExample")
            )
        );
        Assert.DoesNotContain(
            cachedKeysAfterRemoval,
            key => key.StartsWith("/FolderExample/", StringComparison.Ordinal)
        );
        Assert.DoesNotContain(
            Client.Entries.Keys,
            key => key.StartsWith("/FolderExample/", StringComparison.Ordinal)
        );
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Theory]
    [InlineData("/folder", "/folder/")]
    [InlineData("/folder/", "/folder/")]
    [InlineData("folder", "folder/")]
    [InlineData("folder/", "folder/")]
    public async Task Refresh_AnyPathForm_SendsCanonicalPath(string clientPath, string expectedWirePath)
    {
        // Arrange
        var server = Server;
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(_serverExConfig.RootDirectory, "folder"));
        var calls = RecordCalls(server);

        // Act
        await Client.Refresh(clientPath, false, _cts.Token);

        // Assert
        Assert.NotEmpty(calls.ListedPaths);
        Assert.All(calls.ListedPaths, path => Assert.Equal(expectedWirePath, path));
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Fact]
    public async Task Refresh_SkipRecordsInListing_AdvanceOffset()
    {
        // Arrange
        var calls = RecordCalls(
            Server,
            FakeListing(
                ("/folder/", 0, ["S", "S", "Fa.txt\t1"]),
                ("/folder/", 3, ["Fb.txt\t1", "Fc.txt\t1"])
            )
        );

        // Act
        await Client.Refresh("/folder", false, _cts.Token);

        // Assert
        Assert.Equal(new uint[] { 0, 3, 5 }, calls.ListedOffsets);
        Assert.Contains("/folder/a.txt", Client.Entries.Keys);
        Assert.Contains("/folder/b.txt", Client.Entries.Keys);
        Assert.Contains("/folder/c.txt", Client.Entries.Keys);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Fact]
    public async Task Refresh_ListingShiftsBetweenPages_DoesNotDuplicateEntries()
    {
        // Arrange
        var server = Server;
        server.Base.ListDirectory = FakeListing(
            ("/folder/", 0, ["Fa.txt\t1", "Fb.txt\t1"]),
            ("/folder/", 2, ["Fb.txt\t1", "Fc.txt\t1"])
        );

        // Act
        await Client.Refresh("/folder", false, _cts.Token);

        // Assert
        Assert.Equal(
            new[] { "/", "/folder/", "/folder/a.txt", "/folder/b.txt", "/folder/c.txt" },
            Client.Entries.Keys.Order(StringComparer.Ordinal)
        );
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Fact]
    public async Task Refresh_EmptyListingPage_Completes()
    {
        // Arrange
        var calls = RecordCalls(Server, FakeListing(("/folder/", 0, [])));
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token);
        timeout.CancelAfter(TimeSpan.FromSeconds(5));

        // Act
        await Client.Refresh("/folder", false, timeout.Token);

        // Assert
        Assert.Equal(new uint[] { 0 }, calls.ListedOffsets);
        Assert.Contains("/folder/", Client.Entries.Keys);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Fact]
    public async Task Refresh_CaseDifferentSibling_PreservesSiblingCache()
    {
        // Arrange
        var server = Server;
        server.Base.ListDirectory = FakeListing(
            ("/", 0, ["DLogs", "Dlogs"]),
            ("/logs/", 0, ["Fx.bin\t1"])
        );
        await Client.Refresh("/", false, _cts.Token);

        // Act
        await Client.Refresh("/logs", false, _cts.Token);

        // Assert
        Assert.Contains("/Logs/", Client.Entries.Keys);
        Assert.Contains("/logs/x.bin", Client.Entries.Keys);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Fact]
    public async Task RemoveDirectoryAsync_Recursive_DoesNotCacheAncestors()
    {
        // Arrange
        _ = Server;
        var dir = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "a", "b", "c");
        _fileSystem.AddDirectory(_fileSystem.Path.Combine(dir, "d"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(dir, "x.txt"));

        // Act
        await Client.RemoveDirectory("/a/b/c", recursive: true, _cts.Token);

        // Assert
        Assert.False(_fileSystem.Directory.Exists(dir));
        Assert.Empty(Client.Entries);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Fact]
    public async Task RemoveDirectoryAsync_Recursive_ParentRemovalFails_EvictsRemovedEntriesFromAllProjections()
    {
        // Arrange
        var server = Server;
        var root = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "folder");
        _fileSystem.AddDirectory(root);
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(root, "file1.txt"));
        _fileSystem.AddEmptyFile(_fileSystem.Path.Combine(root, "child", "file2.txt"));
        await Client.Refresh("/", true, _cts.Token);
        await Client.Refresh("folder", true, _cts.Token);
        FailRemoveDirectory(server, "folder/");

        // Act
        var task = Client.RemoveDirectory("/folder", recursive: true, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(() => task);
        Assert.False(_fileSystem.Directory.Exists(_fileSystem.Path.Combine(root, "child")));
        Assert.False(_fileSystem.File.Exists(_fileSystem.Path.Combine(root, "file1.txt")));
        Assert.DoesNotContain(
            Client.Entries.Keys,
            key =>
                key.TrimStart(MavlinkFtpHelper.DirectorySeparator)
                    .StartsWith("folder/child/", StringComparison.Ordinal)
        );
        Assert.DoesNotContain("/folder/file1.txt", Client.Entries.Keys);
        Assert.DoesNotContain("folder/file1.txt", Client.Entries.Keys);
        Assert.Contains("/folder/", Client.Entries.Keys);
        Assert.Contains("folder/", Client.Entries.Keys);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Fact]
    public async Task RemoveDirectoryAsync_Recursive_ParentRemovalFails_EvictsStaleDescendants()
    {
        // Arrange
        var server = Server;
        var root = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "folder");
        var staleFile = _fileSystem.Path.Combine(root, "child", "stale.txt");
        _fileSystem.AddEmptyFile(staleFile);
        await Client.Refresh("/", true, _cts.Token);
        // removed behind the client's back: the fresh listing of child/ will not contain it
        _fileSystem.File.Delete(staleFile);
        FailRemoveDirectory(server, "folder/");

        // Act
        var task = Client.RemoveDirectory("/folder", recursive: true, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(() => task);
        Assert.False(_fileSystem.Directory.Exists(_fileSystem.Path.Combine(root, "child")));
        Assert.DoesNotContain("/folder/child/stale.txt", Client.Entries.Keys);
        Assert.DoesNotContain("/folder/child/", Client.Entries.Keys);
        Assert.Contains("/folder/", Client.Entries.Keys);
        Assert.Equal(Link.Client.Statistic.RxMessages, Link.Server.Statistic.TxMessages);
    }

    [Fact]
    public async Task RemoveDirectoryAsync_CanonicalPathTooLong_ThrowsArgumentOutOfRange()
    {
        // Arrange
        _ = Server;
        // fits the limit as typed, but the canonical form gets a trailing separator
        var path = new string('a', MavlinkFtpHelper.MaxDataSize);

        // Act
        var task = Client.RemoveDirectory(path, recursive: false, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => task);
    }

    private sealed class ServerCalls
    {
        public List<string> ListedPaths { get; } = [];
        public List<uint> ListedOffsets { get; } = [];
        public List<string> RemovedFiles { get; } = [];
        public List<string> RemovedDirectories { get; } = [];
    }

    private static ServerCalls RecordCalls(FtpServerEx server, ListDirectoryDelegate? listing = null)
    {
        var calls = new ServerCalls();
        listing ??= server.ListDirectory;
        server.Base.ListDirectory = (path, offset, buffer, cancel) =>
        {
            calls.ListedPaths.Add(path);
            calls.ListedOffsets.Add(offset);
            return listing(path, offset, buffer, cancel);
        };
        server.Base.RemoveFile = (path, cancel) =>
        {
            calls.RemovedFiles.Add(path);
            return server.RemoveFile(path, cancel);
        };
        server.Base.RemoveDirectory = (path, cancel) =>
        {
            calls.RemovedDirectories.Add(path);
            return server.RemoveDirectory(path, cancel);
        };
        return calls;
    }

    private static void FailRemoveDirectory(FtpServerEx server, string serverPath)
    {
        server.Base.RemoveDirectory = (path, cancel) =>
            path.TrimStart(MavlinkFtpHelper.DirectorySeparator) == serverPath
                ? throw new FtpNackException(FtpOpcode.RemoveDirectory, NackError.Fail)
                : server.RemoveDirectory(path, cancel);
    }

    private static ListDirectoryDelegate FakeListing(
        params (string Path, uint Offset, string[] Records)[] pages
    )
    {
        return (path, offset, buffer, _) =>
        {
            var page = pages.FirstOrDefault(p => p.Path == path && p.Offset == offset);
            if (page.Records is null)
            {
                throw new FtpNackEndOfFileException(FtpOpcode.ListDirectory);
            }

            var data = string.Concat(
                page.Records.Select(record => $"{record}{MavlinkFtpHelper.PathSeparator}")
            );
            data.CopyTo(buffer.Span);
            return Task.FromResult((byte)data.Length);
        };
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
