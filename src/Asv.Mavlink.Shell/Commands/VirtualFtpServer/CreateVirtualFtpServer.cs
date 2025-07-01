using System.IO;
using System.Threading.Tasks;
using Asv.IO;
using ConsoleAppFramework;
using Newtonsoft.Json;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class CreateVirtualFtpServerCommand
{
    private const string DefaultConfigFilePath = "exampleVirtualFtpServerConfig.json";

    /// <summary>
    ///     Command creates virtual ftp server.
    /// </summary>
    /// <param name="cfgPath">-cfg, location of the config file</param>
    /// <returns></returns>
    [Command("run-ftp-server")]
    public async Task<int> Run(string? cfgPath = null)
    {
        var configFilePath = cfgPath ?? DefaultConfigFilePath;

        AnsiConsole.MarkupLine($"[blue]info[/]: Check config file exist: [green]{configFilePath}[/]");

        if (!File.Exists(configFilePath))
        {
            AnsiConsole.MarkupLine($"[yellow]warn[/]: Creating default config file: {configFilePath}");
            var json = JsonConvert.SerializeObject(VirtualFtpServerConfig.Default, Formatting.Indented);
            await File.WriteAllTextAsync(configFilePath, json);
        }

        var fileTextContent = await File.ReadAllTextAsync(configFilePath);
        var cfg = JsonConvert.DeserializeObject<VirtualFtpServerConfig>(fileTextContent);
        if (cfg is null)
        {
            AnsiConsole.MarkupLine("[red]error[/]: Unable to load cfg[/]");
            return 1;
        }
        
        AnsiConsole.MarkupLine(
            $"[blue]info[/]: Check root directory exist: [green]{cfg.FtpServerExConfig.RootDirectory}[/]");

        if (!Directory.Exists(cfg.FtpServerExConfig.RootDirectory))
        {
            AnsiConsole.MarkupLine($"[yellow]warn[/]: Creating root directory: {cfg.FtpServerExConfig.RootDirectory}");
            Directory.CreateDirectory(cfg.FtpServerExConfig.RootDirectory);
        }

        await using var router = Protocol.Create(builder => { builder.RegisterMavlinkV2Protocol(); })
            .CreateRouter("ROUTER");
        
        AnsiConsole.MarkupLine("[green]note[/]: The actual connection strings may differ from those shown below"); 

        foreach (var port in cfg.Ports)
        {
            AnsiConsole.MarkupLine($"[blue]info[/]: Add connection port: [yellow]{port}[/]");
            router.AddPort(port);
        }

        var core = new CoreServices(router);
        await using var device = ServerDevice.Create(
            new MavlinkIdentity(cfg.SystemId, cfg.ComponentId), core, config =>
            {
                config.RegisterFtp(cfg.FtpServerConfig);
                config.RegisterFtpEx(cfg.FtpServerExConfig);
            });

        ConsoleAppHelper.WaitCancelPressOrProcessExit();
        return 0;
    }
}