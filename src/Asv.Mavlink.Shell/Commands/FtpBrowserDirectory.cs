using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using ConsoleAppFramework;
using DynamicData;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class FtpBrowserDirectory
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private ReadOnlyObservableCollection<FtpEntry> _tree;
    private readonly SourceCache<IFtpEntry,string> _entryCache = new(x => x.Path);
    private IObservable<IChangeSet<IFtpEntry, string>> Entries => _entryCache.Connect();
    private FtpClient _ftpClient;
    private FtpClientEx _ftpClientEx;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// File manager for interacting with a drone's file system via FTP
    /// </summary>
    /// <param name="connection">-cs, The address of the connection to the mavlink device</param>
    [Command("ftp-browser")]
    public async Task RunFtpBrowser(string connection)
    {
        await using var conn = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
        }).CreateRouter("ROUTER");
        var identity = new MavlinkClientIdentity(255, 255, 1, 1);
        var seq = new PacketSequenceCalculator();
        var core = new CoreServices(conn, seq, null, TimeProvider.System, new DefaultMeterFactory());
        MavlinkFtpClientConfig config = new()
        {
            TimeoutMs = 1000,
            CommandAttemptCount = 5,
            TargetNetworkId = 0,
            BurstTimeoutMs = 1000
        };
        _ftpClient = new FtpClient(identity, config, core);
        _ftpClientEx = new FtpClientEx(_ftpClient);
        try
        {
            await _ftpClientEx.Refresh("/");
            await _ftpClientEx.Refresh("@SYS");
            
            _entryCache.AddOrUpdate(_ftpClientEx.Entries.Values);
            Entries.TransformToTree(x => x.ParentPath)
                .Transform(x => new FtpEntry(x))
                .DisposeMany()
                .Bind(out _tree)
                .Subscribe();
            
            await CreateFtpBrowser(_tree);
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            throw;
        }
    }

    private async Task CreateFtpBrowser(ReadOnlyObservableCollection<FtpEntry> tree,
        Stack<FtpEntry>? stack = null)
    {
        stack ??= new Stack<FtpEntry>();
        var currentDirectory = stack.Count > 0 ? stack.Peek() : null;
        var choices = new List<FtpEntry>();

        if (stack.Count > 0 && currentDirectory != null)
        {
            choices.Add(new FtpEntry(new Node<IFtpEntry, string>(new FtpEntryModel
            {
                Name = "...",
                Path = currentDirectory.Key,
                ParentPath = currentDirectory.Item.ParentPath,
                Type = FtpEntryType.Directory
            }, currentDirectory.Node.Key)));
        }

        var currentItems = currentDirectory == null ? tree : currentDirectory.Items;
        choices.AddRange(currentItems);

        while (true)
        {
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<FtpEntry>()
                    .Title($"[blue]Current Directory:[/] [yellow]{currentDirectory?.Key ?? "Root"}[/]")
                    .AddChoices(choices)
                    .UseConverter(entry => entry.Item.Name + (entry.Item.Type == FtpEntryType.Directory ? "/" : "")));

            switch (selection.Item)
            {
                case { Name: "..." }:
                    stack.Pop();
                    await CreateFtpBrowser(tree, stack);
                    break;

                case { Type: FtpEntryType.Directory }:
                    var result = await DirectoryOperationsMenu(selection);
                    if (result == "Open directory")
                    {
                        stack.Push(selection);
                        await CreateFtpBrowser(tree, stack);
                    }

                    break;
                case { Type: FtpEntryType.File }:
                    AnsiConsole.MarkupLine($"[blue]Selected file:[/] [yellow]{selection.Key}[/]");
                    await FileOperationsMenu(selection);
                    break;
                default:
                    throw new Exception("Missing directory");
            }
        }
    }

    public async Task FileOperationsMenu(FtpEntry file)
    {
        var token = new CancellationTokenSource();
        var fileOperation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[blue]Select the operation for the file[/] [yellow]{file.Item.Path}[/]:")
                .AddChoices([
                    ".../", "Remove file", "Calculate CRC32", "Truncate file", "Download file", "Burst download file",
                    "Rename file"
                ])
        );

        switch (fileOperation)
        {
            case "Remove file":
                await _ftpClient.RemoveFile(file.Item.Path, token.Token);
                AnsiConsole.MarkupLine("[green]File was removed![/]");
                break;
            case "Calculate CRC32":
                var crc32 = await _ftpClient.CalcFileCrc32(file.Item.Path,
                    token.Token);
                AnsiConsole.MarkupLine($"[yellow]CRC32[/]: [green]{crc32}[/]");
                break;
            case "Truncate file":
                await Truncate(_ftpClient, file.Item.Path,
                    token.Token); //TODO: Поправить реализацию, сейчас не отрабатывает
                break;
            case "Download file":
                await DownloadFile(file, token.Token);
                break;
            case "Burst download file":
                await BurstDownloadFile(file, token.Token);
                break;
            case "Rename file":
                await RenameImpl(file, token.Token); //TODO: Поправить, сейчас не отрабатывает
                break;
            case ".../":
                await token.CancelAsync();
                break;
        }
    }

    private async Task<string> DirectoryOperationsMenu(FtpEntry directory)
    {
        var token = new CancellationTokenSource();
        var fileOperation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[blue]Select the operation for the directory[/] [yellow]{directory.Item.Path}[/]:")
                .AddChoices(".../", "Open directory", "Create directory", "Remove directory", "Rename directory")
        );

        switch (fileOperation)
        {
            case "Create directory":
                await CreateDirectoryImpl(_ftpClient, directory, token.Token);
                break;
            case "Remove directory":
                await _ftpClient.RemoveDirectory(directory.Item.Path, token.Token);
                AnsiConsole.MarkupLine("[green]Directory was removed![/]");
                break;
            case "Rename directory":
                await RenameImpl(directory, token.Token); //TODO: Поправить, сейчас не отрабатывает
                break;
            case "Open directory":
                return "Open directory";
            case ".../":
                break;
        }

        return ".../";
    }

    private async Task RenameImpl(FtpEntry item, CancellationToken cancellationToken)
    {
        var fileName = AnsiConsole.Prompt(
            new TextPrompt<string>("[blue]New name[/]: "));
        var lastIndex = item.Item.Path.LastIndexOf('/');
        var directoryPath = item.Item.Path[..(lastIndex + 1)];
        await _ftpClient.Rename(item.Item.Path, directoryPath + fileName, cancellationToken);
        AnsiConsole.MarkupLine($"[green]Directory was renamed![/]");
    }

    private async Task Truncate(FtpClient ftpClient, string filePath, CancellationToken cancellationToken)
    {
        var newSize = AnsiConsole.Ask<uint>("Enter the new size for truncating the file:");
        await ftpClient.TruncateFile(new TruncateRequest(filePath, newSize), cancellationToken);
        AnsiConsole.MarkupLine($"[green]File {filePath} truncated to {newSize} bytes![/]");
    }

    private async Task CreateDirectoryImpl(IFtpClient ftpClient, FtpEntry item, CancellationToken cancellationToken)
    {
        var dirName = AnsiConsole.Prompt(
            new TextPrompt<string>("[blue]Name of new folder[/]: "));
        var lastIndex = item.Item.Path.LastIndexOf('/');
        var dirPath = item.Item.Path[..(lastIndex + 1)];
        await ftpClient.CreateDirectory(dirPath + dirName, cancellationToken);
        AnsiConsole.MarkupLine("[green]Directory was created![/]");
    }

    private async Task DownloadFile(FtpEntry file, CancellationToken cancellationToken)
    {
        var pathToSave = AnsiConsole.Prompt(
            new TextPrompt<string>("[blue]Path to save[/]: "));
        await using var stream = new FileStream(Path.Combine(pathToSave, file.Item.Name), FileMode.Create,
            FileAccess.Write, FileShare.None);
        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task = ctx.AddTask($"[green]Downloading {file.Item.Name}[/]");
                var progress = new Progress<double>(value => { task.Value = value * 100; });
                await _ftpClientEx.DownloadFile(file.Item.Path, stream, progress:progress, cancel:cancellationToken);
                task.StopTask();
            });
    }

    private async Task BurstDownloadFile(FtpEntry file, CancellationToken cancellationToken)
    {
        var pathToSave = AnsiConsole.Prompt(
            new TextPrompt<string>("[blue]Path to save[/]: "));
        await using var stream = new FileStream(Path.Combine(pathToSave, file.Item.Name), FileMode.Create,
            FileAccess.Write, FileShare.None);
        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task = ctx.AddTask($"[green]Downloading {file.Item.Name}[/]");
                var progress = new Progress<double>(value => { task.Value = value * 100; });
                await _ftpClientEx.BurstDownloadFile(file.Item.Path, stream, progress, 239, cancellationToken);
                task.StopTask();
            });
    }
}