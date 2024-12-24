using System.IO;
using System.Threading.Tasks;
using Asv.IO;
using ConsoleAppFramework;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class CreateVirtualFtpServerCommand
{
    private string _file = "exampleVirtualFtpServerConfig.json";
    private ILoggerFactory _loggerFactory;
    
    /// <summary>
    /// Command creates virtual ftp server.
    /// </summary>
    /// <param name="cfgPath">-cfg, location of the config file</param>
    /// <returns></returns>
    [Command("run-ftp-server")]
    public int Run(string? cfgPath = null)
    {
        _file = cfgPath ?? _file;
        _loggerFactory = new VirtualFtpServerLoggerFactory("FTP_SERVER");
        
        RunAsync().Wait();
        ConsoleAppHelper.WaitCancelPressOrProcessExit();
        return 0;
    }

    private async Task RunAsync()
    {
        AnsiConsole.MarkupLine($"[blue]info[/]: Check config file exist: [green]{_file}[/]");
        
        if (!File.Exists(_file))
        {
            AnsiConsole.MarkupLine($"[yellow]warn[/]: Creating default config file: {_file}");
            var json = JsonConvert.SerializeObject(VirtualFtpServerConfig.Default, Formatting.Indented);
            await File.WriteAllTextAsync(_file, json);
        }

        var cfg = JsonConvert.DeserializeObject<VirtualFtpServerConfig>(await File.ReadAllTextAsync(_file));

        if (cfg is null)
        {
            AnsiConsole.MarkupLine("[red]error[/]: Unable to load cfg[/]");
            return;
        }

        var router = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
        }).CreateRouter("ROUTER");
        
        foreach (var port in cfg.Ports)
        {
            AnsiConsole.MarkupLine($"[green]Add connection port [/]: [yellow]{port}[/]");
            router.AddPort(port);
        }
        var core = new CoreServices(router);
        var device = ServerDevice.Create(new MavlinkIdentity(cfg.SystemId, cfg.ComponentId), core, config =>
        {
            config.RegisterFtp(cfg.FtpServerConfig);
            config.RegisterFtpEx(cfg.FtpServerExConfig);
        });
    }
}