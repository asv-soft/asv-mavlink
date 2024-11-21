using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(FtpServerEx))]
[TestSubject(typeof(FtpClientEx))]
public class FtpExComplexTest : ComplexTestBase<FtpClientEx, FtpServerEx>, IDisposable
{
    private readonly TaskCompletionSource<FileTransferProtocolPacket> _tcs = new();
    private readonly CancellationTokenSource _cts;

    private MockFileSystem _fileSystem = new();

    private readonly MavlinkFtpServerConfig _serverConfig = new()
    {
        BurstReadChunkDelayMs = 100,
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
        NetworkId = 0,
        BurstReadChunkDelayMs = 100,
        RootDirectory = AppDomain.CurrentDomain.BaseDirectory
    };

    public FtpExComplexTest(ITestOutputHelper log) : base(log)
    {
        _cts = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
        _cts.Token.Register(() => _tcs.TrySetCanceled());
    }

    protected override FtpServerEx CreateServer(MavlinkIdentity identity, ICoreServices core)
    {
        _fileSystem = SetUpFileSystem(_serverExConfig.RootDirectory);
        return new FtpServerEx(new FtpServer(identity, _serverConfig, core), _serverExConfig, _fileSystem);
    }

    protected override FtpClientEx CreateClient(MavlinkClientIdentity identity, ICoreServices core)
    {
        return new FtpClientEx(new FtpClient(identity, _clientConfig, core));
    }

    [Fact]
    public async Task DownloadFile_Success()
    {
        // Arrange
        const int chunkSize = 200;

        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "test.txt");
        var progress = new Progress<double>();
        var dataChunks = new byte[chunkSize];
        new Random().NextBytes(dataChunks);
        var streamToSave = new MemoryStream(chunkSize);

        Server.Base.OpenFileRead = async (path, _) =>
        {
            Assert.Equal(filePath, path);
            await streamToSave.WriteAsync(dataChunks, _);
            return await Task.FromResult(new ReadHandle(0, 100));
        };

        byte readCount = 0;
        Server.Base.FileRead = async (readRequest, buffer, _) =>
        {
            readCount++;
            return await Task.FromResult(new ReadResult(readCount, readRequest));
        };

        Server.Base.TerminateSession = (session, reason) => Task.CompletedTask;

        // Act
        await Client.DownloadFile(filePath, streamToSave, progress, _cts.Token);

        // Assert
        Assert.Equal(dataChunks, streamToSave.GetBuffer());

        await streamToSave.DisposeAsync();
    }

    [Fact]
    public async Task BurstDownloadFile_Success()
    {
        // Arrange
        const string fileName = "test.txt";
        const string fileContent = "Something good band test read me pls32, gogogo";
        const byte size = 100;
        var filePath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, fileName);
        using var streamToSave = new MemoryStream(size);
        var progress = new Progress<double>();
        _fileSystem.AddFile(filePath, new MockFileData(fileContent));
        var dataChunks = new byte[size];
        new Random().NextBytes(dataChunks);
        var originStream = new MemoryStream(dataChunks);
        var data = new byte[size];
        
        Server.Base.OpenFileRead = async (path, _) =>
        {
            Assert.Equal(filePath, path);
            return await Task.FromResult(new ReadHandle(1, 2048u));
        };

        Server.Base.BurstReadFile = async (request, buffer, cancel) =>
        {
            if (originStream.Length == 0 || request.Skip >= originStream.Length)
            {
                return new BurstReadResult(0, true, request);
            }

            originStream.Position = request.Skip;

            var bytesToRead = (int)Math.Min(request.Take, originStream.Length - request.Skip);
            var readBytes = await originStream.ReadAsync(buffer[..bytesToRead], cancel).ConfigureAwait(false);

            return new BurstReadResult((byte)readBytes, originStream.Position >= originStream.Length, request);
        };

        Server.Base.FileRead = async (request, buffer, cancel) =>
        {
            data.CopyTo(buffer.Span);
            return await Task.FromResult(new ReadResult(size, request));
        };
        
        Server.Base.TerminateSession = (session, reason) => Task.CompletedTask;

        Link.Client.Filter<FileTransferProtocolPacket>().Subscribe(p =>
        {
            _tcs.TrySetResult(p);
        });

        // Act
        await Client.BurstDownloadFile(filePath, streamToSave, progress, 239, _cts.Token);
        await _tcs.Task.ConfigureAwait(false);

        // Assert
        var buffer = new byte[size];
        streamToSave.Seek(0, SeekOrigin.Begin);
        _ = streamToSave.Read(buffer, 0, size);
        Assert.Equal(dataChunks, buffer);
    }

    [Fact]
    public async Task Refresh_Success()
    {
        // Arrange
        var fileEntries = new[]
        {
            _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "file1.txt"),
            _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "file2.txt"),
            _fileSystem.Path.Combine(_serverExConfig.RootDirectory, "file3.txt"),
        };
        
        _fileSystem.AddFile(fileEntries[0], new MockFileData("file1"));
        _fileSystem.AddFile(fileEntries[1], new MockFileData("file2"));
        _fileSystem.AddFile(fileEntries[2], new MockFileData("file3"));

        Server.Base.ListDirectory = (path, offset, buffer, cancel) =>
        {
            if (cancel.IsCancellationRequested)
            {
                throw new FtpNackException(FtpOpcode.ListDirectory, NackError.None);
            }
            
            var fullPath = _fileSystem.Path.Combine(_serverExConfig.RootDirectory, path);
            if (!fullPath.Contains(_serverExConfig.RootDirectory))
            {
                fullPath = _serverExConfig.RootDirectory;
            }

            if (!_fileSystem.Path.Exists(fullPath))
            {
                throw new FtpNackException(FtpOpcode.ListDirectory, NackError.FileNotFound);
            }

            uint currentIndex = 0;
            var infos = new List<IFileSystemInfo>();
            var dirInfo = new DirectoryInfo(fullPath);
            var directory = _fileSystem.DirectoryInfo.Wrap(dirInfo);
            var dirInfos = directory.GetFileSystemInfos();

            if (offset >= dirInfos.Length)
            {
                throw new FtpNackEndOfFileException(FtpOpcode.ListDirectory);
            }

            foreach (var info in dirInfos)
            {
                if (currentIndex >= offset)
                {
                    infos.Add(info);
                }

                currentIndex++;
            }

            var result = new List<string>();
            foreach (var entry in infos)
            {
                if (entry.Extension.Length > 0)
                {
                    var file = (IFileInfo)entry;
                    result.Add($"F{file.Name}\t{file.Length}\0");
                    continue;
                }

                result.Add($"D{entry.Name}\0");
            }

            var sb = new StringBuilder(0, MavlinkFtpHelper.MaxDataSize);
            foreach (var str in result.TakeWhile(str => sb.Length + str.Length <= sb.MaxCapacity))
            {
                sb.Append(str);
            }

            if (sb.Length == 0)
            {
                throw new FtpNackException(FtpOpcode.ListDirectory, NackError.Fail);
            }

            sb.ToString().CopyTo(buffer.Span);
            return Task.FromResult((byte)sb.Length);
        };

        // Act
        await Client.Refresh(_serverExConfig.RootDirectory, true);

        // Assert
        Assert.Equal(fileEntries, Client.Entries.Keys);
    }

    private static async Task<string> ConvertStreamToString(Stream stream, long offset)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        using var reader = new StreamReader(stream, MavlinkFtpHelper.FtpEncoding);
        return await reader.ReadToEndAsync();
    }

    private FileTransferProtocolPacket CreateAckResponse(FileTransferProtocolPacket requestPacket,
        FtpOpcode originOpCode)
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

    public void Dispose()
    {
        _cts.Dispose();
    }
}