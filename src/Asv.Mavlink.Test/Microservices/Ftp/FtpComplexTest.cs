using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Common;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class FtpComplexTest : ComplexTestBase<FtpClient, FtpServerEx>
{
    private readonly TaskCompletionSource<FileTransferProtocolPacket> _tcs = new();
    private readonly CancellationTokenSource _cts;
    private MockFileSystem _fileSystem = null!;

    public FtpComplexTest(ITestOutputHelper log)
        : base(log)
    {
        _cts = new CancellationTokenSource();
        _cts.Token.Register(() => _tcs.TrySetCanceled());
    }

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
        var mockFileCfg = new MockFileSystemOptions
        {
            CurrentDirectory = root
        };
        var fileSystem = new MockFileSystem(mockFileCfg);
        fileSystem.AddDirectory(mockFileCfg.CurrentDirectory);

        return fileSystem;
    }
    
    private readonly MavlinkFtpServerExConfig _serverExConfig = new()
    {
        RootDirectory = AppDomain.CurrentDomain.BaseDirectory
    };

    protected override FtpServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        _fileSystem = SetUpFileSystem(_serverExConfig.RootDirectory);
        return new FtpServerEx(new FtpServer(identity, _serverConfig, core), _serverExConfig, _fileSystem);
    }

    protected override FtpClient CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        return new FtpClient(identity, _clientConfig, core);
    }

    [Fact]
    public async Task ResetSessions_NormalRequest_Success()
    {
        // Arrange
        _ = Server;

        // Act
        var response = await Client.ResetSessions(_cts.Token);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(FtpOpcode.ResetSessions, response.ReadOriginOpCode());
    }

    [Theory]
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
    }

    [Theory]
    [InlineData("/path/to/file.txt")]
    [InlineData("path/file.txt")]
    public async Task RemoveFile_NormalRequest_Success(string expectedPath)
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
    }

    [Theory]
    [InlineData("/path/to/file.txt", 3000, 1024)]
    [InlineData("/path/to/file.txt", 1024, 1024)]
    public async Task TruncateFile_NormalData_Success(string path, int fileSize, uint offset)
    {
        // Arrange
        _ = Server;
        
        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddFile(fullPath, new MockFileData(string.Concat(Enumerable.Repeat("A" , fileSize))));
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
    }

    [Theory]
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
    }

    [Theory]
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
    }

    [Theory]
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
    }

    [Theory]
    [InlineData(new []{"file1.txt", "file2.txt", "lib1.a"}, "/", "Ffile1.txt\t0\0Ffile2.txt\t0\0Flib1.a\t0\0")]
    [InlineData(new []{"folder/file1.txt", "folder/file2.txt", "folder/dir1/"}, "/folder/","Ddir1\0Ffile1.txt\t0\0Ffile2.txt\t0\0")]
    public async Task ListDirectory_CurrentDirectory_Success(string[] path, string root, string expectedListingResult)
    {
        // Arrange
        _ = Server;
        path.ForEach(x =>
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
        var response = await Client.ListDirectory(root, 0, _cts.Token);
        var actualListing = response.ReadDataAsString();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(FtpOpcode.Ack, response.ReadOpcode());
        Assert.Equal(expectedListingResult, actualListing);
    }

    [Theory]
    [InlineData("/path/to/file.txt", 2048)]
    [InlineData("/path/to/file.txt", 1)]
    public async Task OpenFileRead_NormalData_Success(string path, uint fileSize)
    {
        // Arrange
        _ = Server;
        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddFile(fullPath, new MockFileData(string.Concat(Enumerable.Repeat("A", (int)fileSize))));
        
        var localFilePath = _fileSystem.FileInfo.New(fullPath).FullName;

        // Act
        var handle = await Client.OpenFileRead(path, _cts.Token);

        // Assert
        _fileSystem.AllFiles.Should().Contain(localFilePath);
        handle.Size.Should().Be(fileSize);
    }

    [Theory]
    [InlineData("/path/to/file.txt", 2048)]
    [InlineData("/path/to/file.txt", 1)]
    public async Task OpenFileWrite_NormalData_Success(string path, uint fileSize)
    {
        // Arrange
        _ = Server;
        var fullPath = _fileSystem.MakeFullPath(path, _serverExConfig.RootDirectory);
        _fileSystem.AddFile(fullPath, new MockFileData(string.Concat(Enumerable.Repeat("A", (int)fileSize))));
        
        var localFilePath = _fileSystem.FileInfo.New(fullPath).FullName;

        // Act
        var handle = await Client.OpenFileWrite(path, _cts.Token);

        // Assert
        _fileSystem.AllFiles.Should().Contain(localFilePath);
        handle.Size.Should().Be(fileSize);
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
    }

    [Theory]
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
        var data = new byte[size];
        new Random().NextBytes(data);
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
        var data = new byte[writeTake];
        new Random().NextBytes(data);
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
        var data = new byte[size];
        new Random().NextBytes(data);

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
    }
}
