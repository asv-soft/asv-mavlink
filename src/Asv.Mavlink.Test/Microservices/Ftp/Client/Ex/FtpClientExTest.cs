using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(FtpClientEx))]
public class FtpClientExTest : ClientTestBase<FtpClientEx>
{
    private readonly TaskCompletionSource<FileTransferProtocolPacket> _tcs = new(); 
    private readonly CancellationTokenSource _cts;
    
    private readonly MavlinkFtpClientConfig _clientExConfig = new ()
    {
        TimeoutMs = 1000,
        CommandAttemptCount = 5,
        TargetNetworkId = 0,
        BurstTimeoutMs = 100
    };
    
    private readonly MavlinkFtpServerExConfig _serverExConfig = new ()
    {
        RootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "root")
    };
    
    private FtpServerEx _ftpServerEx;
    private MockFileSystem _fileSystem;

    public FtpClientExTest(ITestOutputHelper log) : base(log)
    {
        _cts = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
        _cts.Token.Register(() => _tcs.TrySetCanceled());
        _ = Client;
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

    private void SetUpFtpServerEx(MavlinkClientIdentity identity, CoreServices core)
    {
        var serverId = new MavlinkIdentity(identity.Target.SystemId, identity.Target.ComponentId);
        var serverConf = new MavlinkFtpServerConfig
        {
            NetworkId = _clientExConfig.TargetNetworkId,
            BurstReadChunkDelayMs = 100
        };
        var server = new FtpServer(serverId, serverConf, core);

        _fileSystem = SetUpFileSystem(_serverExConfig.RootDirectory);
        _ftpServerEx = new FtpServerEx(server, _serverExConfig, _fileSystem);
    }

    protected override FtpClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        SetUpFtpServerEx(identity, core);
        return new FtpClientEx(new FtpClient(identity, _clientExConfig, core));
    }
    
    [Fact]
    public async Task DownloadFile_Success()
    {
        // Arrange
        const string fileName = "test.txt";
        const string fileContent = "Something good band test read me pls32, gogogo";
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        using var streamToSave = new MemoryStream();
        var progress = new Progress<double>();
        _fileSystem.AddFile(filePath, new MockFileData(fileContent));
        
        // Act
        await Client.DownloadFile(filePath, streamToSave, progress, _cts.Token);
        
        // Assert
        var result = await ConvertStreamToString(streamToSave, 0);
        Assert.Equal(fileContent, result);
    }

    [Fact]
    public async Task BurstDownloadFile_Success()
    {
        // Arrange
        const string fileName = "test.txt";
        const string fileContent = "Something good band test read me pls32, gogogo";
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        using var streamToSave = new MemoryStream();
        var progress = new Progress<double>();
        _fileSystem.AddFile(filePath, new MockFileData(fileContent));
        
        // Act
        await Client.BurstDownloadFile(filePath, streamToSave, progress, 239, _cts.Token);
        
        await _tcs.Task.ConfigureAwait(false);
        
        // Assert
        var result = await ConvertStreamToString(streamToSave, 0);
        Assert.Equal(fileContent, result);
    }
    
    [Fact]
    public async Task Refresh_Success()
    {
        // Arrange
        var fileDirName = "temp";
        var fileDir = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, "test.txt");
        var filePath2 = _fileSystem.Path.Combine(fileDir, "test2.txt");
        _fileSystem.AddDirectory(Path.Combine(fileDir, "Folder1"));
        _fileSystem.AddDirectory(Path.Combine(fileDir, "Folder2"));
        _fileSystem.AddDirectory(Path.Combine(fileDir, "Folder3"));
        _fileSystem.AddFile(filePath, new MockFileData("Something"));
        _fileSystem.AddFile(filePath2, new MockFileData(string.Empty));
        
        // Act
        await Client.Refresh(fileDirName, true, _cts.Token);
        
        // Assert
        Assert.Equal(6, Client.Entries.Count);
    }
    
    private static async Task<string> ConvertStreamToString(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        using var reader = new StreamReader(stream, MavlinkFtpHelper.FtpEncoding);
        return await reader.ReadToEndAsync();
    }
    
    private FileTransferProtocolPacket CreateAckResponse(FileTransferProtocolPacket requestPacket, FtpOpcode originOpCode)
    {
        var response = new FileTransferProtocolPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Sequence = requestPacket.Sequence,
            Payload =
            {
                TargetSystem = requestPacket.SystemId,
                TargetComponent = requestPacket.ComponentId,
                TargetNetwork = requestPacket.Payload.TargetNetwork,
                Payload = new byte[251],
            }
        };

        response.WriteOpcode(FtpOpcode.Ack);
        response.WriteSequenceNumber(requestPacket.ReadSequenceNumber());
        response.WriteSession(requestPacket.ReadSession());
        response.WriteSize(0);
        response.WriteOriginOpCode(originOpCode);

        return response;
    }
}