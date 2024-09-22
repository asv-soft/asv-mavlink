using System;
using System.Threading;
using System.Threading.Tasks;
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
        using var ftpClient = new FtpClient(new MavlinkFtpClientConfig(), conn,identity,seq );
        var charBuffer = new char[255];
        var buffer = new Memory<char>(charBuffer);
        try
        {
            var result = await ftpClient.ListDirectory("/terrain", 0, buffer);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
    }
}