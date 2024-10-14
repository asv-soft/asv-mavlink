using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class FtpSession_Tests
{
    private readonly FakeTimeProvider _fakeTime;
    private readonly ITestOutputHelper _output;
    private FtpServerEx _serverEx;
    private string _filePath;

    public FtpSession_Tests()
    {
        FtpServerExTests test = new FtpServerExTests(_output);
        var fileName = "test.txt";
        var fileDirName = "file";
        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        var fileSystem = test.SetUpFileSystem(root);
        test.SetUpServer(out var server);
        var fileDir = fileSystem.Path.Combine(root, fileDirName);
        _filePath = Path.Combine(fileDirName, fileName);
        var filePath = fileSystem.Path.Combine(fileDir, fileName);
        fileSystem.AddFile(filePath, new MockFileData(string.Empty));
        var cfg = new MavlinkFtpServerExConfig
        {
            RootDirectory = root,
        };
        _serverEx = new FtpServerEx(cfg, server, fileSystem);
    }

    [Fact]
    public async Task Open_File_Read_Max_Amount()
    {
        // Act
        var result = await OpenAmountOfSessions(255);
        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Open_File_Read_More_Than_Max_Amount()
    {
        //Act
        var result = await OpenAmountOfSessions(256);
        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Reset_Opened_Sessions_And_Open_Them_Again()
    {
        //Act
        var openResult = await OpenAmountOfSessions(255);
        await _serverEx.ResetSessions();
        var reopenResult = await OpenAmountOfSessions(255);
        //Assert
        Assert.True(openResult);
        Assert.True(reopenResult);
    }

    [Fact]
    public async Task Open_File_Read_From_Different_Sessions()
    {
        //Act
        await OpenAmountOfSessions(254);
        var readresult = await _serverEx.OpenFileRead(_filePath);
        Assert.Equal(255, readresult.Session);
        Assert.False(readresult.Size > 0);
    }
    
    private async Task<bool> OpenAmountOfSessions(int numberOfSessions)
    {
        try
        {
            for (int i = 0; i <= numberOfSessions; i++)
            {
                await _serverEx.OpenFileRead(_filePath);
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}