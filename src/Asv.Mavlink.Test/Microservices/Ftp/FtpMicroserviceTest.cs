using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
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
    
    [Fact]
    public async Task Ftp_Directory_Existence_After_Creation()
    {
        var link = new VirtualLink();
        
        var server = new FtpServerEx(new FtpServer(
            link.Server,
            new MavlinkServerIdentity{ComponentId = 13, SystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        var client = new FtpClientEx(new FtpClient(
            link.Client,
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        string dirName = "pathdir1";

        await client.CreateDirectory(dirName, new CancellationToken());

        Assert.True(Directory.Exists(dirName));
        
        Directory.Delete(dirName);
    }
    
    [Fact]
    public async Task Ftp_Directory_Existence_After_Removal()
    {
        var link = new VirtualLink();
        
        var server = new FtpServerEx(new FtpServer(
            link.Server,
            new MavlinkServerIdentity{ComponentId = 13, SystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        var client = new FtpClientEx(new FtpClient(
            link.Client,
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));

        string dirName = "pathdir2"; 
        
        Directory.CreateDirectory(dirName);

        await client.RemoveDirectory(dirName, new CancellationToken());

        Assert.False(Directory.Exists(dirName));
    }

    [Fact]
    public async Task Ftp_List_Current_Directory()
    {
        var link = new VirtualLink();
        
        var server = new FtpServerEx(new FtpServer(
            link.Server,
            new MavlinkServerIdentity{ComponentId = 13, SystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        var client = new FtpClientEx(new FtpClient(
            link.Client,
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        var result = await client.ListDirectory(".", new CancellationToken());
        
        Assert.Contains(result, _ => _.Name == "zh-Hans");
        Assert.Contains(result, _ => _.Name == "Asv.IO.dll");
        Assert.Contains(result, _ => _.Name == "xunit.runner.visualstudio.dotnetcore.testadapter.dll");
    }
    
    [Fact]
    public async Task Ftp_File_Existence_After_Removal()
    {
        var link = new VirtualLink();
        
        var server = new FtpServerEx(new FtpServer(
            link.Server,
            new MavlinkServerIdentity{ComponentId = 13, SystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        var client = new FtpClientEx(new FtpClient(
            link.Client,
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));

        string fileName = "testfile1.dat";
        
        File.Create(fileName).Close();
    
        await client.RemoveFile(fileName, new CancellationToken());
    
        Assert.False(File.Exists(fileName));
    }

    [Fact]
    public async Task Ftp_File_Upload()
    {
        var link = new VirtualLink();
        
        var server = new FtpServerEx(new FtpServer(
            link.Server,
            new MavlinkServerIdentity{ComponentId = 13, SystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        var client = new FtpClientEx(new FtpClient(
            link.Client,
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));

        string dirName = "testfolder";
        
        string serverFileName = $".\\{dirName}\\testfile1.dat";

        string clientFileName = "testfile1.dat";
        
        await client.CreateDirectory(dirName, new CancellationToken());
        
        Assert.True(Directory.Exists(dirName));
        
        await client.UploadFile(serverFileName, clientFileName, new CancellationToken());

        Assert.True(File.Exists(serverFileName));

        var serverData = new byte[29];
        
        using (var openedFile = File.OpenRead(serverFileName))
        {
            openedFile.Read(serverData);
        }
        
        var clientData = new byte[29];
        
        using (var openedFile = File.OpenRead(clientFileName))
        {
            openedFile.Read(clientData);
        }

        Assert.Equal(clientData, serverData);
    }
    
    [Fact]
    public async Task Ftp_File_Read()
    {
        var link = new VirtualLink();
        
        var server = new FtpServerEx(new FtpServer(
            link.Server,
            new MavlinkServerIdentity{ComponentId = 13, SystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));
        
        var client = new FtpClientEx(new FtpClient(
            link.Client,
            new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13},
            new FtpConfig(),
            new PacketSequenceCalculator(),
            TaskPoolScheduler.Default));

        string dirName = "testfolder";
        
        string serverFileName = $".\\{dirName}\\testfile1.dat";

        string clientFileName = "testfile1.dat";
        
        await client.CreateDirectory(dirName, new CancellationToken());
        
        Assert.True(Directory.Exists(dirName));
        
        await client.ReadFile(serverFileName, clientFileName, new CancellationToken());

        Assert.True(File.Exists(clientFileName));

        var serverData = new byte[29];
        
        using (var openedFile = File.OpenRead(serverFileName))
        {
            openedFile.Read(serverData);
        }
        
        var clientData = new byte[29];
        
        using (var openedFile = File.OpenRead(clientFileName))
        {
            openedFile.Read(clientData);
        }

        Assert.Equal(clientData, serverData);
    }
}