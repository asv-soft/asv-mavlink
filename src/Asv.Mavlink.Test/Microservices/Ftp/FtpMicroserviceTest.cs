using System.IO;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;


public class FtpMicroserviceTest
{
    private readonly ITestOutputHelper _output;

    public FtpMicroserviceTest(ITestOutputHelper output)
    {
        _output = output;
    }
    
    //[Fact(Skip = "Fatal error")]
    [Fact]
    public async Task Ftp_Directory_Existence_After_Creation()
    {
        var link = new VirtualMavlinkConnection();
        
        var server = new FtpServerEx(new FtpServer(
            link.Server,
            new MavlinkIdentity(13,13),
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        var client = new FtpClientEx(new FtpClient(
            link.Client,
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator()));
        
        string dirName = "pathdir1";

        await client.CreateDirectory(dirName, new CancellationToken());

        Assert.True(Directory.Exists(dirName));
        
        Directory.Delete(dirName);
    }
    
    //[Fact(Skip = "Fatal error")]
    [Fact]
    public async Task Ftp_Directory_Existence_After_Removal()
    {
        var link = new VirtualMavlinkConnection();
        
        var server = new FtpServerEx(new FtpServer(
            link.Server,
            new MavlinkIdentity(13,13),
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        var client = new FtpClientEx(new FtpClient(
            link.Client,
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator()));

        string dirName = "pathdir2"; 
        
        Directory.CreateDirectory(dirName);

        await client.RemoveDirectory(dirName, new CancellationToken());

        Assert.False(Directory.Exists(dirName));
    }

    [Fact(Skip = "Skip this test")]
    //[Fact]
    public async Task Ftp_List_Current_Directory()
    {
        var link = new VirtualMavlinkConnection();
        
        var server = new FtpServerEx(new FtpServer(
            link.Server,
            new MavlinkIdentity(13,13),
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        var client = new FtpClientEx(new FtpClient(
            link.Client,
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator()));

        var result = await client.ListDirectory(".", new CancellationToken());
        
        Assert.Contains(result, _ => _.Name == "zh-Hans");
        Assert.Contains(result, _ => _.Name == "Asv.IO.dll");
        Assert.Contains(result, _ => _.Name == "xunit.runner.visualstudio.dotnetcore.testadapter.dll");
    }
    
    //[Fact(Skip = "Fatal error")]
    [Fact]
    public async Task Ftp_File_Existence_After_Removal()
    {
        var link = new VirtualMavlinkConnection();
        
        var server = new FtpServerEx(new FtpServer(
            link.Server,
            new MavlinkIdentity(13,13),
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        var client = new FtpClientEx(new FtpClient(
            link.Client,
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator()));

        string fileName = "testfile4.dat";
        
        File.Create(fileName).Close();
    
        await client.RemoveFile(fileName, new CancellationToken());
    
        Assert.False(File.Exists(fileName));
    }

    //[Fact(Skip = "Fatal error")]
    [Fact]
    public async Task Ftp_File_Upload()
    {
        var link = new VirtualMavlinkConnection();
        
        var server = new FtpServerEx(new FtpServer(
            link.Server,
            new MavlinkIdentity(13,13),
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        var client = new FtpClientEx(new FtpClient(
            link.Client,
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator()));

        string dirName = "testfolder";
        
        string serverFileName = $".\\{dirName}\\testfile3.dat";
        
        string clientFileName = "testfile3.dat";

        await using (var createdFile = File.Create(clientFileName))
        {
            createdFile.Write(Encoding.ASCII.GetBytes("1,2,3,4,5,6,7,8,9"));
        }
        
        await client.CreateDirectory(dirName, new CancellationToken());
        
        Assert.True(Directory.Exists(dirName));
        
        await client.UploadFile(serverFileName, clientFileName, new CancellationToken());

        Assert.True(File.Exists(serverFileName));

        var serverData = new byte[30];

        await using (var openedFile = File.OpenRead(serverFileName))
        {
            openedFile.Read(serverData);
        }
        
        var clientData = new byte[30];

        await using (var openedFile = File.OpenRead(clientFileName))
        {
            openedFile.Read(clientData);
        }

        Assert.Equal(clientData, serverData);

        File.Delete(serverFileName);
        File.Delete(clientFileName);
    }
    
    //[Fact(Skip = "Fatal error")]
    [Fact]
    public async Task Ftp_File_Read()
    {
        var link = new VirtualMavlinkConnection();
        
        var server = new FtpServerEx(new FtpServer(
            link.Server,
            new MavlinkIdentity(13,13),
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        var client = new FtpClientEx(new FtpClient(
            link.Client,
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator()));

        string dirName = "testfolder";
        
        string clientFileName = $".\\{dirName}\\testfile1.dat";

        string serverFileName = "testfile1.dat";
        
        await client.CreateDirectory(dirName, new CancellationToken());

        await using (var createdFile = File.Create(serverFileName))
        {
            createdFile.Write(Encoding.ASCII.GetBytes("1,2,3,4,5,6,7,8,9"));
        }
        
        Assert.True(Directory.Exists(dirName));
        
        await client.ReadFile(serverFileName, clientFileName, new CancellationToken());

        Assert.True(File.Exists(clientFileName));

        var serverData = new byte[30];

        await using (var openedFile = File.OpenRead(serverFileName))
        {
            openedFile.Read(serverData);
        }
        
        var clientData = new byte[30];

        await using (var openedFile = File.OpenRead(clientFileName))
        {
            openedFile.Read(clientData);
        }

        Assert.Equal(clientData, serverData);

        File.Delete(serverFileName);
        File.Delete(clientFileName);
    }
    
    [Fact(Skip = "Skip this test")]
    //[Fact]
    public async Task Ftp_File_Burst_Read()
    {
        var link = new VirtualMavlinkConnection();
        
        var server = new FtpServerEx(new FtpServer(
            link.Server,
            new MavlinkIdentity(13,13),
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        var client = new FtpClientEx(new FtpClient(
            link.Client,
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator()));

        string dirName = "testfolder";
        
        string clientFileName = $".\\{dirName}\\testfile2.dat";

        string serverFileName = "testfile2.dat";
        
        await client.CreateDirectory(dirName, new CancellationToken());

        await using (var createdFile = File.Create(serverFileName))
        {
            createdFile.Write(Encoding.ASCII.GetBytes("1,2,3,4,5,6,7,8,9"));
        }
        
        Assert.True(Directory.Exists(dirName));
        
        await client.BurstReadFile(serverFileName, clientFileName, new CancellationToken());
        
        Assert.True(File.Exists(clientFileName));

        var serverData = new byte[30];

        await using (var openedFile = File.OpenRead(serverFileName))
        {
            openedFile.Read(serverData);
        }
        
        var clientData = new byte[30];

        await using (var openedFile = File.OpenRead(clientFileName))
        {
            openedFile.Read(clientData);
        }

        Assert.Equal(clientData, serverData);
        
        File.Delete(serverFileName);
        File.Delete(clientFileName);
    }
}