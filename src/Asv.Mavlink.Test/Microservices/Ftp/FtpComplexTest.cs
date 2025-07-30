using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Common;
using FluentAssertions;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class FtpComplexTest(ITestOutputHelper log) : ComplexTestBase<FtpClient, FtpServerEx>(log)
{
    private readonly CancellationTokenSource _cts = new();
    private MockFileSystem _fileSystem = null!;

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

    private static MockFileSystem SetUpFileSystem(string root)
    {
        var mockFileCfg = new MockFileSystemOptions { CurrentDirectory = root };
        var fileSystem = new MockFileSystem(mockFileCfg);
        fileSystem.AddDirectory(mockFileCfg.CurrentDirectory);

        return fileSystem;
    }

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

    protected override FtpClient CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        return new FtpClient(identity, _clientConfig, core);
    }

    [Fact]
    public async Task ResetSessions_NoActiveSessions_Success()
    {
        // Arrange
        _ = Server;
        var rxCount = 0;
        var txCount = 0;

        using var subRx = Link
            .Server.RxFilterByType<FileTransferProtocolPacket>()
            .Subscribe(_ =>
            {
                rxCount++;
            });
        using var subTx = Link
            .Server.TxFilterByType<FileTransferProtocolPacket>()
            .Subscribe(_ =>
            {
                txCount++;
            });

        // Act
        var response = await Client.ResetSessions(_cts.Token);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.ResetSessions, response.ReadOriginOpCode());
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(rxCount, txCount);
    }
    
    [Fact]
    public async Task ResetSessions_All_Success()
    {
        // Arrange
        _ = Server;
        var rxCount = 0;
        var txCount = 0;
        _fileSystem.AddEmptyFile("file.txt");
        _fileSystem.AddEmptyFile("file1.txt");
        _fileSystem.AddEmptyFile("file2.txt");
        await Server.OpenFileRead("file.txt");
        await Server.OpenFileWrite("file1.txt");
        await Server.OpenFileRead("file2.txt");

        using var subRx = Link
            .Server.RxFilterByType<FileTransferProtocolPacket>()
            .Subscribe(_ =>
            {
                rxCount++;
            });
        using var subTx = Link
            .Server.TxFilterByType<FileTransferProtocolPacket>()
            .Subscribe(_ =>
            {
                txCount++;
            });

        // Act
        var response = await Client.ResetSessions(_cts.Token);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.ResetSessions, response.ReadOriginOpCode());
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(rxCount, txCount);
    }
    
    [Fact]
    public async Task ResetSessions_SingleFileThatIsEmpty_Success()
    {
        // Arrange
        _ = Server;
        var rxCount = 0;
        var txCount = 0;
        _fileSystem.AddEmptyFile("file.txt");
        await Server.OpenFileRead("file.txt");

        using var subRx = Link
            .Server.RxFilterByType<FileTransferProtocolPacket>()
            .Subscribe(_ =>
            {
                rxCount++;
            });
        using var subTx = Link
            .Server.TxFilterByType<FileTransferProtocolPacket>()
            .Subscribe(_ =>
            {
                txCount++;
            });

        // Act
        var response = await Client.ResetSessions(_cts.Token);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.ResetSessions, response.ReadOriginOpCode());
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(rxCount, txCount);
    }

    [Fact]
    public async Task ResetSessions_MultipleSessions_Success()
    {
        // Arrange
        _ = Server;
        var rxCount = 0;
        var txCount = 0;

        using var subRx = Link
            .Server.RxFilterByType<FileTransferProtocolPacket>()
            .Subscribe(_ =>
            {
                rxCount++;
            });
        using var subTx = Link
            .Server.TxFilterByType<FileTransferProtocolPacket>()
            .Subscribe(_ =>
            {
                txCount++;
            });

        const string path = "file.txt";
        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddEmptyFile(fullPath);
        await Client.OpenFileWrite(path, _cts.Token);
        await Client.OpenFileRead(path, _cts.Token);

        // Act
        var response = await Client.ResetSessions(_cts.Token);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.ResetSessions, response.ReadOriginOpCode());
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(rxCount, txCount);
    }

    [Fact]
    public async Task ResetSessions_WriteFileFromResetSession_Throws()
    {
        // Arrange
        _ = Server;
        var rxCount = 0;
        var txCount = 0;

        using var subRx = Link
            .Server.RxFilterByType<FileTransferProtocolPacket>()
            .Subscribe(_ =>
            {
                rxCount++;
            });
        using var subTx = Link
            .Server.TxFilterByType<FileTransferProtocolPacket>()
            .Subscribe(_ =>
            {
                txCount++;
            });

        const string path = "file.txt";
        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddEmptyFile(fullPath);
        var handle = await Client.OpenFileWrite(path, _cts.Token);

        const uint skip = 0;
        const byte take = 1;
        var writeBuffer = new byte[] { 0x1 };

        // Act
        var response = await Client.ResetSessions(_cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(
            () => Client.WriteFile(new WriteRequest(handle.Session, skip, take), writeBuffer)
        );
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.ResetSessions, response.ReadOriginOpCode());
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(rxCount, txCount);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("/path/to/directory")]
    [InlineData("/path/to/directory/")]
    [InlineData("path/to/directory/")]
    [InlineData("path/to/directory")]
    public async Task RemoveDirectory_CorrectPath_Success(string expectedPath)
    {
        // Arrange
        _ = Server;

        var fullPath = _fileSystem.MakeFullPath(expectedPath, _serverExConfig.RootDirectory);
        _fileSystem.AddDirectory(fullPath);

        var localDirPath = _fileSystem.DirectoryInfo.New(fullPath).FullName;

        // Act
        var response = await Client.RemoveDirectory(expectedPath, _cts.Token);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.RemoveDirectory, response.ReadOriginOpCode());
        _fileSystem.AllDirectories.Should().NotContain(localDirPath);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("/path/to/file.txt")]
    [InlineData("path/file.txt")]
    public async Task RemoveFile_BasicRequest_Success(string expectedPath)
    {
        // Arrange
        _ = Server;

        var fullPath = _fileSystem.MakeFullPath(expectedPath, _serverExConfig.RootDirectory);
        _fileSystem.AddEmptyFile(fullPath);
        var localFilePath = _fileSystem.FileInfo.New(fullPath).FullName;

        // Act
        var response = await Client.RemoveFile(expectedPath, _cts.Token);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.RemoveFile, response.ReadOriginOpCode());
        _fileSystem.AllFiles.Should().NotContain(localFilePath);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("/path/to/file.txt", 3000, 1024)]
    [InlineData("/path/to/file.txt", 1024, 1024)]
    public async Task TruncateFile_NormalData_Success(string path, int fileSize, uint offset)
    {
        // Arrange
        _ = Server;

        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddFile(
            fullPath,
            new MockFileData(string.Concat(Enumerable.Repeat("A", fileSize)))
        );
        var localFilePath = _fileSystem.FileInfo.New(fullPath).FullName;

        var request = new TruncateRequest(path, offset);

        // Act
        var response = await Client.TruncateFile(request, _cts.Token);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.TruncateFile, response.ReadOriginOpCode());
        _fileSystem.AllFiles.Should().Contain(localFilePath);
        _fileSystem.FileInfo.New(localFilePath).Length.Should().Be(offset);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("/path/to/file.txt", "123456789", 771566984)]
    [InlineData("/path/to/file.txt", "1234567890", 3315058323)]
    public async Task CalcFileCrc32_NormalData_Success(string path, string data, uint expectedCrc)
    {
        // Arrange
        _ = Server;

        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddFile(fullPath, new MockFileData(data));
        var localFilePath = _fileSystem.FileInfo.New(fullPath).FullName;

        // Act
        var crc32 = await Client.CalcFileCrc32(path, _cts.Token);

        // Assert
        _fileSystem.AllFiles.Should().Contain(localFilePath);
        Assert.Equal(expectedCrc, crc32);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("/")]
    [InlineData("/path/to/directory")]
    public async Task CalcFileCrc32_PathToDirectory_Throws(string path)
    {
        // Arrange
        _ = Server;

        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddDirectory(fullPath);

        // Act
        var task = Client.CalcFileCrc32(path, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("/file.txt")]
    [InlineData("/path/to/directory")]
    public async Task CalcFileCrc32_InexistentPath_Throws(string path)
    {
        // Arrange
        _ = Server;

        // Act
        var task = Client.CalcFileCrc32(path, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("/path/to/directory")]
    [InlineData("/path/to/directory/")]
    [InlineData("path/to/directory/")]
    [InlineData("path/to/directory")]
    public async Task CreateDirectory_NormalData_Success(string path)
    {
        // Arrange
        _ = Server;

        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        var localDirPath = _fileSystem.DirectoryInfo.New(fullPath).FullName;

        // Act
        var response = await Client.CreateDirectory(path, _cts.Token);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.CreateDirectory, response.ReadOriginOpCode());
        _fileSystem.AllDirectories.Should().Contain(localDirPath);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("/path/to/old_name.txt", "/path/to/new_name.txt")]
    [InlineData("path/to/old_name.txt", "path/to/new_name.txt")]
    public async Task Rename_NormalData_Success(string oldPath, string newPath)
    {
        // Arrange
        _ = Server;

        var oldFilePath = _fileSystem.MakeFullPath(oldPath, _serverExConfig.RootDirectory);
        var newFilePath = _fileSystem.MakeFullPath(newPath, _serverExConfig.RootDirectory);
        _fileSystem.AddEmptyFile(oldFilePath);

        var localFileOldPath = _fileSystem.FileInfo.New(oldFilePath).FullName;
        var localFileNewPath = _fileSystem.FileInfo.New(newFilePath).FullName;

        // Act
        var response = await Client.Rename(oldPath, newPath, _cts.Token);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.Rename, response.ReadOriginOpCode());

        _fileSystem.AllFiles.Should().Contain(localFileNewPath);
        _fileSystem.AllFiles.Should().NotContain(localFileOldPath);

        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData(
        new[] { "file1.txt", "file2.txt", "lib1.a" },
        "/",
        0,
        "Ffile1.txt\t0\0Ffile2.txt\t0\0Flib1.a\t0\0"
    )]
    [InlineData(
        new[] { "folder/file1.txt", "folder/file2.txt", "folder/dir1/" },
        "/folder/",
        2,
        "Ffile2.txt\t0\0"
    )]
    public async Task ListDirectory_NormalData_Success(
        string[] entries,
        string root,
        uint offset,
        string expectedListingResult
    )
    {
        // Arrange
        _ = Server;

        entries.ForEach(x =>
        {
            if (x.Last() == '/')
            {
                _fileSystem.AddDirectory(x);
            }
            else
            {
                _fileSystem.AddEmptyFile(x);
            }
        });

        // Act
        var response = await Client.ListDirectory(root, offset, _cts.Token);
        var actualListing = response.ReadDataAsString();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.ListDirectory, response.ReadOriginOpCode());
        Assert.Equal(expectedListingResult, actualListing);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("/path/to/file.txt", 2048)]
    [InlineData("/path/to/file.txt", 0)]
    public async Task OpenFileRead_NormalData_Success(string path, uint fileSize)
    {
        // Arrange
        _ = Server;

        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddFile(
            fullPath,
            new MockFileData(string.Concat(Enumerable.Repeat("A", (int)fileSize)))
        );

        var localFilePath = _fileSystem.FileInfo.New(fullPath).FullName;

        // Act
        var handle = await Client.OpenFileRead(path, _cts.Token);

        // Assert
        _fileSystem.AllFiles.Should().Contain(localFilePath);
        handle.Size.Should().Be(fileSize);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("/path/to/file.txt", 2048)]
    [InlineData("/path/to/file.txt", 0)]
    public async Task OpenFileRead_MultipleRequests_Success(string path, uint fileSize)
    {
        // Arrange
        _ = Server;

        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddFile(
            fullPath,
            new MockFileData(string.Concat(Enumerable.Repeat("A", (int)fileSize)))
        );

        var localFilePath = _fileSystem.FileInfo.New(fullPath).FullName;

        // Act
        var handle1 = await Client.OpenFileRead(path, _cts.Token);
        var handle2 = await Client.OpenFileRead(path, _cts.Token);

        // Assert
        Assert.Equal(handle1.Session, handle2.Session);
        _fileSystem.AllFiles.Should().Contain(localFilePath);
        handle1.Size.Should().Be(fileSize).And.Be(handle2.Size);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("/")]
    [InlineData("/path/tp/directory")]
    [InlineData("/path/tp/directory/")]
    public async Task OpenFileRead_PathToDirectory_Throws(string path)
    {
        // Arrange
        _ = Server;
        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddDirectory(fullPath);

        // Act
        var task = Client.OpenFileRead(path, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("/path/to/file.txt", 2048)]
    [InlineData("/path/to/file.txt", 0)]
    public async Task OpenFileWrite_NormalData_Success(string path, uint fileSize)
    {
        // Arrange
        _ = Server;

        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddFile(
            fullPath,
            new MockFileData(string.Concat(Enumerable.Repeat("A", (int)fileSize)))
        );

        var localFilePath = _fileSystem.FileInfo.New(fullPath).FullName;

        // Act
        var handle = await Client.OpenFileWrite(path, _cts.Token);

        // Assert
        _fileSystem.AllFiles.Should().Contain(localFilePath);
        handle.Size.Should().Be(fileSize);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("/path/to/file.txt", 2048)]
    [InlineData("/path/to/file.txt", 0)]
    public async Task OpenFileWrite_MultipleRequests_Success(string path, uint fileSize)
    {
        // Arrange
        _ = Server;

        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddFile(
            fullPath,
            new MockFileData(string.Concat(Enumerable.Repeat("A", (int)fileSize)))
        );

        var localFilePath = _fileSystem.FileInfo.New(fullPath).FullName;

        // Act
        var handle1 = await Client.OpenFileWrite(path, _cts.Token);
        var handle2 = await Client.OpenFileWrite(path, _cts.Token);

        // Assert
        Assert.Equal(handle1.Session, handle2.Session);
        _fileSystem.AllFiles.Should().Contain(localFilePath);
        handle1.Size.Should().Be(fileSize).And.Be(handle2.Size);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("/")]
    [InlineData("/path/tp/directory")]
    [InlineData("/path/tp/directory/")]
    public async Task OpenFileWrite_PathToDirectory_Throws(string path)
    {
        // Arrange
        _ = Server;
        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddDirectory(fullPath);

        // Act
        var task = Client.OpenFileWrite(path, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () => await task);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Fact]
    public async Task TerminateSession_NormalData_Success()
    {
        // Arrange
        _ = Server;

        var response = await Client.CreateFile("file.txt");
        var session = response.ReadSession();

        // Act
        await Client.TerminateSession(session, _cts.Token);

        // Assert
        await Assert.ThrowsAsync<FtpNackException>(async () =>
        {
            await Client.TerminateSession(session, _cts.Token);
        });
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory] // TODO: Unstable test. May fail if system has another directory separators
    [InlineData("new_file.txt")]
    [InlineData("/new_file.txt")]
    public async Task CreateFile_NormalData_Success(string path)
    {
        // Arrange
        _ = Server;

        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        var localFilePath = _fileSystem.FileInfo.New(fullPath).FullName;

        // Act
        var response = await Client.CreateFile(path, _cts.Token);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.CreateFile, response.ReadOriginOpCode());
        _fileSystem.AllFiles.Should().Contain(localFilePath);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory]
    [InlineData(0, 100, 100)]
    [InlineData(0, 100, 10)]
    [InlineData(0, 10, 100)]
    [InlineData(10, 100, 100)]
    public async Task ReadFile_RandomData_Success(int readSkip, byte readTake, byte size)
    {
        // Arrange
        _ = Server;

        const string path = "/some_file.txt";
        var data = Random.Shared.GetItems(
            Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(),
            size
        );
        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddFile(fullPath, new MockFileData(data));

        var handle = await Client.OpenFileRead(path, _cts.Token);
        var request = new ReadRequest(handle.Session, (uint)readSkip, readTake);

        // Act
        var response = await Client.ReadFile(request, _cts.Token);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.ReadFile, response.ReadOriginOpCode());

        var receivedData = response.Payload.Payload.AsSpan(12, response.ReadSize()).ToArray();
        var expectedData = data.Take(new Range(readSkip, readTake));
        receivedData.Should().BeEquivalentTo(expectedData);

        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory]
    [InlineData(0, 100)]
    [InlineData(10, 100)]
    [InlineData(100, 100)]
    [InlineData(0, 0)]
    public async Task WriteFile_RandomData_Success(int writeSkip, byte writeTake)
    {
        // Arrange
        _ = Server;

        const string path = "/some_file.txt";
        var data = Random.Shared.GetItems(
            Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(),
            writeTake
        );
        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddFile(fullPath, new MockFileData(data));

        var handle = await Client.OpenFileWrite(path, _cts.Token);
        var request = new WriteRequest(handle.Session, (uint)writeSkip, writeTake);

        // Act
        var response = await Client.WriteFile(request, data, _cts.Token);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.WriteFile, response.ReadOriginOpCode());

        var expectedData = data.Take(new Range(writeSkip, writeTake));
        var buffer = new byte[writeTake];
        var lenght = await _fileSystem.FileInfo.New(fullPath).OpenRead().ReadAsync(buffer);
        var receivedData = buffer.Take(new Range(writeSkip, writeTake));

        lenght.Should().Be(writeTake);
        receivedData.Should().BeEquivalentTo(expectedData);

        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    [Theory]
    [InlineData(0, 50, 100)]
    [InlineData(28, 239, 240)]
    [InlineData(0, 239, 0)]
    public async Task BurstReadFile_RandomData_Success(int readSkip, byte readTake, byte size)
    {
        // Arrange
        _ = Server;

        const string path = "/some_file.txt";
        var data = Random.Shared.GetItems(
            Enumerable.Range(0, 255).Select(i => (byte)i).ToArray(),
            size
        );

        var expectedChunks = new List<byte[]>();
        for (var i = readSkip; i < size; i += readTake)
        {
            expectedChunks.Add(data.Take(new Range(i, readTake + i)).ToArray());
        }

        if (expectedChunks.Count == 0)
        {
            expectedChunks.Add([]);
        }

        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddFile(fullPath, new MockFileData(data));
        var handle = await Client.OpenFileRead(path, _cts.Token);
        var request = new ReadRequest(handle.Session, (uint)readSkip, readTake);

        // Act
        List<byte> receivedData = [];
        var receivedChunks = new List<byte[]>();
        await Client.BurstReadFile(
            request,
            packet =>
            {
                var sz = packet.ReadSize();
                var dataSpan = packet.Payload.Payload.AsSpan(12, sz).ToArray();
                receivedData.AddRange(dataSpan);
                receivedChunks.Add(dataSpan);
            },
            _cts.Token
        );

        // Assert
        receivedChunks.Should().BeEquivalentTo(expectedChunks);
        receivedData.Should().BeEquivalentTo(receivedData);

        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
    }

    #region Timeout

    [Fact]
    public async Task BurstReadFile_TimeSkip_ThrowsByTimeout()
    {
        // Arrange
        var emptyRequest = new ReadRequest(0, 0, 0);

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.BurstReadFile(emptyRequest, _ => { }, _cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task ReadFile_TimeSkip_ThrowsByTimeout()
    {
        // Arrange
        var emptyRequest = new ReadRequest(0, 0, 0);

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.ReadFile(emptyRequest, _cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task CalcFileCrc32_TimeSkip_ThrowsByTimeout()
    {
        // Arrange
        const string path = "/some_file.txt";

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.CalcFileCrc32(path, _cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task CreateDirectory_TimeSkip_ThrowsByTimeout()
    {
        // Arrange
        const string path = "/some_directory";

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.CreateDirectory(path, _cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task CreateFile_TimeSkip_ThrowsByTimeout()
    {
        // Arrange
        const string path = "/some_file.txt";

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.CreateFile(path, _cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task ListDirectory_TimeSkip_ThrowsByTimeout()
    {
        // Arrange
        const string path = "/";

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.ListDirectory(path, 0, _cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task Rename_TimeSkip_ThrowsByTimeout()
    {
        // Arrange
        const string path1 = "/some_file1.txt";
        const string path2 = "/some_file2.txt";

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.Rename(path1, path2, _cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task WriteFile_TimeSkip_ThrowsByTimeout()
    {
        // Arrange
        var emptyRequest = new WriteRequest(0, 0, 0);

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.WriteFile(emptyRequest, Memory<byte>.Empty, _cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task OpenFileRead_TimeSkip_ThrowsByTimeout()
    {
        // Arrange
        const string path = "/some_file.txt";

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.OpenFileRead(path, _cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task OpenFileWrite_TimeSkip_ThrowsByTimeout()
    {
        // Arrange
        const string path = "/some_file.txt";

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.OpenFileWrite(path, _cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task RemoveDirectory_TimeSkip_ThrowsByTimeout()
    {
        // Arrange
        const string path = "/some_directory";

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.RemoveDirectory(path, _cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task RemoveFile_TimeSkip_ThrowsByTimeout()
    {
        // Arrange
        const string path = "/some_file.txt";

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.RemoveFile(path, _cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task ResetSessions_TimeSkip_ThrowsByTimeout()
    {
        // Arrange

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.ResetSessions(_cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task TerminateSession_TimeSkip_ThrowsByTimeout()
    {
        // Arrange
        const byte session = 0;

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.TerminateSession(session, _cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task TruncateFile_TimeSkip_ThrowsByTimeout()
    {
        // Arrange
        var emptyRequest = new TruncateRequest();

        // Act
        using var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.TruncateFile(emptyRequest, _cts.Token);
        });

        using var t2 = Task.Factory.StartNew(() =>
        {
            ClientTime.Advance(
                TimeSpan.FromMilliseconds(
                    _clientConfig.TimeoutMs * _clientConfig.CommandAttemptCount + 1
                )
            );
        });

        // Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(_clientConfig.CommandAttemptCount, (int)Link.Client.Statistic.TxMessages);
    }

    #endregion
}
