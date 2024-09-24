using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using ManyConsole;

namespace Asv.Mavlink.Shell;

public class FtpCommand:ConsoleCommand
{
    private string _connectionString = "tcp://127.0.0.1:7343";
    private readonly CancellationTokenSource _cancel = new CancellationTokenSource();

    public FtpCommand()
    {
        IsCommand("ftp", "FTP browser");
        HasOption("cs=", $"Connection string. Default '{_connectionString}'", _ => _connectionString = _);
    }
    
    public override int Run(string[] remainingArguments)
    {
        RunAsync().Wait();
        return 0;
    }

    private async Task RunAsync()
    {
        using var port = PortFactory.Create(_connectionString);
        port.Enable();
        using var conn = MavlinkV2Connection.Create(port);
        var identity = new MavlinkClientIdentity(255, 255, 1, 1);
        var seq = new PacketSequenceCalculator();
        using var ftpClient = new FtpClient(new MavlinkFtpClientConfig(), conn,identity,seq,TimeProvider.System);
        var ftpEx = new FtpClientEx(ftpClient);
        try
        {
            /*
            await ftpEx.Refresh("/");
            await ftpEx.Refresh("@SYS");
            */
            await using var mem = File.OpenWrite("test.txt");
            await ftpEx.BurstDownloadFile("@PARAM/param.pck?withdefaults=1", mem, new CallbackProgress<double>(Console.WriteLine) );
            //await ftpEx.BurstDownloadFile("/terrain/test.txt", mem, new CallbackProgress<double>(Console.WriteLine) );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
    }
}