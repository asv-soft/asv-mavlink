using System;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppFramework;

namespace Asv.Mavlink.Shell;

class Program
{
    static async Task Main(string[] args)
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
        Console.BackgroundColor = ConsoleColor.Black;
        
        var app = ConsoleApp.Create();
        app.Add<ExampleCommand>();
        app.Add<GenerateCommand>();
        app.Add<FtpTreeDirectory>();
        app.Add<FtpBrowserDirectory>();
        app.Add<DevicesInfoCommand>();
        app.Add<VirtualAdsbCommand>();
        app.Add<ExportSdrData>();
        app.Add<MavProxy>();
        app.Add<BenchmarkBinSerializationCommand>();
        app.Add<BenchmarkSerializationPacket>();
        app.Add<MavlinkCommand>();
        app.Add<PacketViewerCommand>();
        app.Add<CreateVirtualFtpServerCommand>();
        app.Add<GenerateDiagnostics>();
        app.Add<TestGenerateDiagnosticsCommand>();
        app.Add<ParamsCommand>();
        app.Add<PrintVehicleStateCommand>();
        app.Add<DownloadMissionItemsCommand>();
        app.Add<MotorTestCommand>();
        
        await app.RunAsync(args);
    }
}

