using System;
using System.Buffers;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using ConsoleAppFramework;
using Microsoft.Extensions.Logging;
using TimeProviderExtensions;

namespace Asv.Mavlink.Shell;

public class BenchmarkFtpServerExCommand
{
    /// <summary>
    /// Benchmark test
    /// </summary>
    [Command("benchmark-ftp-server")]
    public void RunBenchmarkFtpServerEx()
    {
        BenchmarkRunner.Run<BenchmarkFtpServerEx>();
    }
}

[SimpleJob(RuntimeMoniker.HostProcess)]
[RPlotExporter]
[MemoryDiagnoser]
public class BenchmarkFtpServerEx
{
    private FtpServerEx _server = null!;
    private CancellationTokenSource _cts = null!;
    private MockFileSystem _fileSystem = null!;
    private readonly MavlinkFtpServerExConfig _config =
        new() { RootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp") };
    private readonly MavlinkFtpServerConfig _configBase = new();

    [GlobalSetup]
    public void Setup()
    {
        _cts = new CancellationTokenSource();
        var mockFileCfg = new MockFileSystemOptions { CurrentDirectory = _config.RootDirectory };
        _fileSystem = new MockFileSystem(mockFileCfg);
        _fileSystem.AddDirectory(mockFileCfg.CurrentDirectory);

        var serverTime = new ManualTimeProvider();
        var seq = new PacketSequenceCalculator();
        var identity = new MavlinkIdentity(3, 4);
        var logFactory = new LoggerFactory();

        var protocol = Protocol.Create(builder =>
        {
            builder.SetTimeProvider(serverTime);
            builder.RegisterMavlinkV2Protocol();
            builder.Formatters.RegisterSimpleFormatter();
        });
        var link = protocol.CreateVirtualConnection();
        var core = new CoreServices(link.Server, seq, logFactory, serverTime, new DefaultMeterFactory());
        _server = new FtpServerEx(new FtpServer(identity, _configBase, core), _config, _fileSystem);
    }
    
    [Benchmark]
    public async Task OpenFileRead()
    {
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));
        
        await _server.OpenFileRead(filePath, _cts.Token);
    }

    [Benchmark]
    public async Task OpenFileReadAsync()
    {
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));
        
        await _server.OpenFileReadAsync(filePath, _cts.Token);
    }

    [Benchmark]
    public async Task RemoveFile()
    {
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        await _server.RemoveFile("test.txt", _cts.Token);
    }
    
    [Benchmark]
    public async Task RemoveFileAsync()
    {
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        await _server.RemoveFileAsync("test.txt", _cts.Token);
    }
    
    [Benchmark]
    public async Task CalcFileCrc32()
    {
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        await _server.CalcFileCrc32("test.txt", _cts.Token);
    }
    
    [Benchmark]
    public async Task CalcFileCrc32Async()
    {
        var filePath = _fileSystem.Path.Combine(_config.RootDirectory, "test.txt");
        _fileSystem.AddFile(filePath, new MockFileData("Test content"));

        await _server.CalcFileCrc32Async("test.txt", _cts.Token);
    }
    
    [Benchmark]
    public async Task TruncateFile()
    {
        const string fileName = "test.txt";
        const string fileDirName = "file";
        var root = _fileSystem.Path.Combine(_config.RootDirectory, "temp");

        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("1234567890"));
        var request = new TruncateRequest(filePath, 5);

        await _server.TruncateFile(request);
    }
    
    [Benchmark]
    public async Task TruncateFileAsync()
    {
        const string fileName = "test.txt";
        const string fileDirName = "file";
        var root = _fileSystem.Path.Combine(_config.RootDirectory, "temp");

        var fileDir = _fileSystem.Path.Combine(root, fileDirName);
        var filePath = _fileSystem.Path.Combine(fileDir, fileName);
        _fileSystem.AddFile(filePath, new MockFileData("1234567890"));
        var request = new TruncateRequest(filePath, 5);

        await _server.TruncateFileAsync(request);
    }

    [Benchmark]
    public async Task ListDirectory()
    {
        var dirPath = _fileSystem.Path.Combine(_config.RootDirectory, "testDir");
        _fileSystem.AddDirectory(dirPath);
        _fileSystem.AddFile(
            _fileSystem.Path.Combine(dirPath, "file1.txt"),
            new MockFileData("File1 content")
        );
        _fileSystem.AddFile(
            _fileSystem.Path.Combine(dirPath, "file2.txt"),
            new MockFileData("File2 content")
        );
        using var memory = MemoryPool<char>.Shared.Rent(256);

        // Act
        await _server.ListDirectory(dirPath, 0, memory.Memory);
    }
    
    [Benchmark]
    public async Task ListDirectoryAsync()
    {
        var dirPath = _fileSystem.Path.Combine(_config.RootDirectory, "testDir");
        _fileSystem.AddDirectory(dirPath);
        _fileSystem.AddFile(
            _fileSystem.Path.Combine(dirPath, "file1.txt"),
            new MockFileData("File1 content")
        );
        _fileSystem.AddFile(
            _fileSystem.Path.Combine(dirPath, "file2.txt"),
            new MockFileData("File2 content")
        );
        using var memory = MemoryPool<char>.Shared.Rent(256);

        // Act
        await _server.ListDirectoryAsync(dirPath, 0, memory.Memory);
    }
}
