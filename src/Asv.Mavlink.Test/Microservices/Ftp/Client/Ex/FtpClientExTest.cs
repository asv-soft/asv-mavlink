using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(FtpClientEx))]
public class FtpClientExTest : ClientTestBase<FtpClientEx>
{
    private readonly TaskCompletionSource _tcs = new(); 
    private readonly CancellationTokenSource _cts;
        
    private readonly MavlinkFtpClientConfig _config = new ()
    {
        TimeoutMs = 1000,
        CommandAttemptCount = 5,
        TargetNetworkId = 0,
        BurstTimeoutMs = 100
    };

    public FtpClientExTest(ITestOutputHelper log) : base(log)
    {
        _cts = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
        _cts.Token.Register(() => _tcs.TrySetCanceled());
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

    protected override FtpClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core) => new(new FtpClient(identity, _config, core));
    
    [Fact]
    public async Task DownloadFile_WithProgressReporting_ReportsProgress()
    {
        // Arrange
        var path = "/test/progressFile.txt";
        var memoryStream = new MemoryStream();
        double lastProgress = 0;
        var progress = new Progress<double>(p => lastProgress = p);

        // Act
        await Client.DownloadFile(path, memoryStream, progress, _cts.Token);

        // Assert
        Assert.True(lastProgress > 0);
        Assert.Equal(1.0, lastProgress); // Ensure progress reaches 100%
    }

    [Fact]
    public async Task BurstDownloadFile_Success()
    {
        // Arrange
        var path = "/test/burstFile.txt";
        var memoryStream = new MemoryStream();

        // Act
        await Client.BurstDownloadFile(path, memoryStream, null, 64, _cts.Token);

        // Assert
        Assert.True(memoryStream.Length > 0);
    }

    [Fact]
    public async Task DownloadLargeFile_Success()
    {
        // Arrange
        var path = "/test/largeFile.txt";
        var memoryStream = new MemoryStream();

        // Act
        await Client.DownloadFile(path, memoryStream, null, _cts.Token);

        // Assert
        Assert.True(memoryStream.Length > MavlinkFtpHelper.MaxDataSize); // File size exceeds single read capacity
    }

    [Fact]
    public async Task Refresh_ClearsCacheBeforeReloading()
    {
        // Arrange
        var path = "/cacheTest";
        await Client.Refresh(path, recursive: false, _cts.Token);
        var initialCount = Client.Entries.Count;

        // Act
        await Client.Refresh(path, recursive: false, _cts.Token);

        // Assert
        Assert.Equal(initialCount, Client.Entries.Count); // Cache should be cleared and reloaded with the same count
    }

    [Fact]
    public async Task BurstDownloadFile_InvalidPartSize_ThrowsException()
    {
        // Arrange
        var path = "/test/invalidBurstSizeFile.txt";
        var memoryStream = new MemoryStream();
        var invalidPartSize = (byte)(MavlinkFtpHelper.MaxDataSize + 1);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Client.BurstDownloadFile(path, memoryStream, null, invalidPartSize, _cts.Token));
    }

    
    [Fact]
    public async Task Refresh_SingleLevelDirectory_Success()
    {
        // Arrange
        var path = "/test";
        
        // Act
        await Client.Refresh(path, recursive: false, _cts.Token);
        
        // Assert
        Assert.Contains(path, Client.Entries.Keys);
    }

    [Fact]
    public async Task Refresh_RecursiveDirectory_Success()
    {
        // Arrange
        const string path = "/test";
        
        // Act
        await Client.Refresh(path, recursive: true, _cts.Token);
        
        // Assert
        Assert.Contains(path, Client.Entries.Keys);
        Assert.True(Client.Entries.Count > 1); // Verify subdirectories are added
    }

    [Fact]
    public async Task DownloadFile_Success()
    {
        // Arrange
        const string path = "/test/file.txt";
        var memoryStream = new MemoryStream();

        // Act
        await Client.DownloadFile(path, memoryStream, null, _cts.Token);
        await Client.Base.OpenFileRead(path, _cts.Token);

        var result = await Client.Base.ReadFile(new ReadRequest(0, 0, 10));
        
        // Assert
        Assert.True(memoryStream.Length > 0);
    }

    [Fact]
    public async Task DownloadFile_FileNotFound()
    {
        // Arrange
        const string invalidPath = "/test/nonexistent.txt";
        var memoryStream = new MemoryStream();

        // Act & Assert
        await Assert.ThrowsAsync<FtpNackException>(() => Client.DownloadFile(invalidPath, memoryStream, null, _cts.Token));
    }

    [Fact]
    public async Task Refresh_InvalidPath_ThrowsException()
    {
        // Arrange
        const string invalidPath = "/invalidPath";

        // Act & Assert
        await Assert.ThrowsAsync<FtpNackException>(() => Client.Refresh(invalidPath, recursive: false, _cts.Token));
    }
}