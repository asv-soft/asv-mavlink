using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
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
    private FtpServerExHelper _helper;
    private ITestOutputHelper _output;

    public FtpServerExIntegrationTests(ITestOutputHelper output)
    {
        _helper = new FtpServerExHelper();
        _output = output;
        var fileName = "test.txt";
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = _helper.SetUpFileSystem(root);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        _filePath = Path.Combine(fileDirName, fileName);
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        fileSystem.AddFile(filePath, new MockFileData("12345"));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };

        _helper.SetUpClientAndServer(out var client, out var server, (packet) => true, (packet) => true);
        _serverEx = new FtpServerEx(cfg, server, fileSystem);
        _clientEx = new FtpClientEx(client,_fakeTime);
    }
    [Fact]
    public async Task DownloadFile_ClientExAtemptToDownloadFile_Success()
    {
        byte size = 5;
        var arr = new byte[size];
        var buffer = new MemoryStream(arr);
        var request = new ReadRequest(0, 0, size);
        await _clientEx.Base.OpenFileRead(_filePath);
        await _clientEx.DownloadFile(_filePath, buffer);
        await _clientEx.Base.OpenFileRead(_filePath);
        var result = await _clientEx.Base.ReadFile(request);
        Assert.True(size==result.ReadSize());
    }
    
}