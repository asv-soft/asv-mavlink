using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using ConsoleAppFramework;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class FtpBrowserDirectory
{
    private const string PreviousFolderName = "...";
    private const string CancelOperationVariant = ".../";
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private FtpClient _ftpClient;
    private FtpClientEx _ftpClientEx;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private bool _sysFolderIsMissing;

    /// <summary>
    ///     File manager for interacting with a drone's file system via FTP
    /// </summary>
    /// <param name="connection">-cs, The address of the connection to the mavlink device, e.g. tcp://127.0.0.1:5760</param>
    /// <param name="timeoutMs">-t, The connection timeout in ms</param>
    /// <param name="commandAttemptCount"> The command attempts count</param>
    /// <param name="targetNetworkId">-tid, The target id of the network</param>
    /// <param name="burstTimeoutMs"> The burst timeout in ms</param>
    [Command("ftp-browser")]
    public async Task RunFtpBrowser(
        string connection,
        int timeoutMs = 1000,
        int commandAttemptCount = 5,
        byte targetNetworkId = 0,
        int burstTimeoutMs = 1000)
    {
        var token = new CancellationTokenSource();

        Console.CancelKeyPress += (_, _) =>
        {
            try
            {
                token.Cancel();
            }
            finally
            {
                token.Dispose();
            }
        };

        try
        {
            await using var conn = Protocol.Create(builder => { builder.RegisterMavlinkV2Protocol(); })
                .CreateRouter("ROUTER");
            await using var port = conn.AddPort(connection);

            var identity = new MavlinkClientIdentity(255, 255, 1, 1);
            var seq = new PacketSequenceCalculator();
            var core = new CoreServices(conn, seq, null, TimeProvider.System, new DefaultMeterFactory());

            MavlinkFtpClientConfig config = new()
            {
                TimeoutMs = timeoutMs,
                CommandAttemptCount = commandAttemptCount,
                TargetNetworkId = targetNetworkId,
                BurstTimeoutMs = burstTimeoutMs
            };
            _ftpClient = new FtpClient(identity, config, core);
            _ftpClientEx = new FtpClientEx(_ftpClient);

            try
            {
                await RefreshFtpEntries(token.Token);
            }
            catch (FtpNackException e)
            {
                AnsiConsole.MarkupLine(
                    "[red]Warning:[/] some folders could not be loaded, likely due to invalid encoding");
                AnsiConsole.WriteException(e);
            }
            
            AnsiConsole.MarkupLine("");
            
            await CreateFtpBrowser(token: token.Token);
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            throw;
        }
        finally
        {
            await _ftpClient.DisposeAsync();
            await _ftpClientEx.DisposeAsync();
        }
    }

    private async Task CreateFtpBrowser(
        Stack<IFtpEntry>? stack = null,
        CancellationToken token = default)
    {
        stack ??= new Stack<IFtpEntry>();
        var currentDirectory = stack.Count > 0 ? stack.Peek() : null;

        while (true)
        {
            var choices = new List<IFtpEntry>();

            if (stack.Count > 0 && currentDirectory != null)
                choices.Add(new FtpEntryModel
                {
                    Name = PreviousFolderName,
                    Path = currentDirectory.Path,
                    ParentPath = currentDirectory.ParentPath,
                    Type = FtpEntryType.Directory
                });

            var currentItems = currentDirectory == null
                ? _ftpClientEx.Entries.Values.Where(e => string.IsNullOrWhiteSpace(e.ParentPath)).ToList()
                : _ftpClientEx.Entries.Values.Where(e => e.ParentPath == currentDirectory.Path);
            choices.AddRange(currentItems);

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<IFtpEntry>()
                    .Title($"[blue]Current Directory:[/] [yellow]{currentDirectory?.Path ?? "Root"}[/]")
                    .AddChoices(choices)
                    .UseConverter(entry => entry.Name + (entry.Type == FtpEntryType.Directory ? "/" : "")));

            switch (selection)
            {
                case { Name: PreviousFolderName }:
                    stack.Pop();
                    await CreateFtpBrowser(stack, token);
                    break;
                case { Type: FtpEntryType.Directory }:
                    var result = await DirectoryOperationsMenu(selection, token);
                    if (result == DirectoryOperation.Open)
                    {
                        stack.Push(selection);
                        await CreateFtpBrowser(stack, token);
                    }
                    break;
                case { Type: FtpEntryType.File }:
                    AnsiConsole.MarkupLine($"[blue]Selected file:[/] [yellow]{selection.Path}[/]");
                    await FileOperationsMenu(selection, token);
                    break;
                default:
                    throw new Exception("Missing directory");
            }

            // sleep for one sec so user can see messages
            await Task.Delay(500, token);
            AnsiConsole.Clear();
        }
    }

    public async Task FileOperationsMenu(IFtpEntry file, CancellationToken token)
    {
        var fileOperation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[blue]Select the operation for the file[/] [yellow]{file.Path}[/]:")
                .AddChoices(
                    FileOperation.Remove, FileOperation.CalculateCRC32,
                    FileOperation.Truncate, FileOperation.Download,
                    FileOperation.BurstDownload, FileOperation.Rename,
                    CancelOperationVariant)
        );

        switch (fileOperation)
        {
            case FileOperation.Remove:
                await _ftpClient.RemoveFile(file.Path, token);
                await RefreshFtpEntries(token);
                AnsiConsole.MarkupLine("[green]File was removed![/]");
                break;
            case FileOperation.CalculateCRC32:
                var crc32 = await _ftpClient.CalcFileCrc32(file.Path, token);
                AnsiConsole.MarkupLine($"[yellow]CRC32[/]: [green]{crc32}[/]");
                break;
            case FileOperation.Truncate:
                // TODO: fix implementation
                await Truncate(file.Path, token);
                break;
            case FileOperation.Download:
                await DownloadFile(file, token);
                break;
            case FileOperation.BurstDownload:
                await BurstDownloadFile(file, token);
                break;
            case FileOperation.Rename:
                await RenameImpl(file, token);
                await RefreshFtpEntries(token);
                break;
            case CancelOperationVariant:
                break;
        }
    }

    private async Task<string> DirectoryOperationsMenu(IFtpEntry directory, CancellationToken token)
    {
        var directoryOperation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[blue]Select the operation for the directory[/] [yellow]{directory.Path}[/]:")
                .AddChoices(
                    DirectoryOperation.Open, DirectoryOperation.Create,
                    DirectoryOperation.Remove, DirectoryOperation.Rename,
                    CancelOperationVariant)
        );

        switch (directoryOperation)
        {
            case DirectoryOperation.Create:
                await CreateDirectoryImpl(directory, token);
                await RefreshFtpEntries(token);
                break;
            case DirectoryOperation.Remove:
                // TODO: fix implementation, recursive remove does not work
                await _ftpClient.RemoveDirectory(directory.Path, token);
                await RefreshFtpEntries(token);
                AnsiConsole.MarkupLine("[green]Directory was removed![/]");
                break;
            case DirectoryOperation.Rename:
                await RenameImpl(directory, token);
                await RefreshFtpEntries(token);
                break;
            case DirectoryOperation.Open:
                return DirectoryOperation.Open;
            case CancelOperationVariant:
                break;
        }

        return CancelOperationVariant;
    }

    private async Task RenameImpl(IFtpEntry item, CancellationToken cancellationToken)
    {
        var prompt = new TextPrompt<string>("[blue]New name[/]").DefaultValue(item.Name);
        var fileName = await prompt.ShowAsync(AnsiConsole.Console, cancellationToken);
        if (string.IsNullOrWhiteSpace(fileName) || fileName == item.Name) return;

        var lastIndex = item.Path.TrimEnd('/').LastIndexOf('/');
        var directoryPath = item.Path[..(lastIndex + 1)];
        await _ftpClient.Rename(item.Path, directoryPath + fileName, cancellationToken);
        AnsiConsole.MarkupLine("[green]Item was renamed![/]");
    }

    private async Task Truncate(string filePath, CancellationToken cancellationToken)
    {
        var prompt = new TextPrompt<uint?>("Enter the new size for truncating the file:")
            .DefaultValue(null)
            .HideDefaultValue();
        var newSize = await prompt.ShowAsync(AnsiConsole.Console, cancellationToken);
        if (newSize is null) return;

        await _ftpClient.TruncateFile(new TruncateRequest(filePath, newSize.Value), cancellationToken);
        AnsiConsole.MarkupLine($"[green]File {filePath} truncated to {newSize} bytes![/]");
    }

    private async Task CreateDirectoryImpl(IFtpEntry item, CancellationToken cancellationToken)
    {
        var prompt = new TextPrompt<string>("[blue]Name of new folder[/]:").AllowEmpty();
        var dirName = await prompt.ShowAsync(AnsiConsole.Console, cancellationToken);
        if (string.IsNullOrWhiteSpace(dirName)) return;

        await _ftpClient.CreateDirectory(item.Path.TrimEnd('/') + "/" + dirName, cancellationToken);
        AnsiConsole.MarkupLine("[green]Directory was created![/]");
    }

    private async Task DownloadFile(IFtpEntry file, CancellationToken cancellationToken)
    {
        var prompt = new TextPrompt<string>("[blue]Local path to save[/]:").AllowEmpty();
        var pathToSave = await prompt.ShowAsync(AnsiConsole.Console, cancellationToken);
        if (string.IsNullOrWhiteSpace(pathToSave)) return;

        await using var stream = new FileStream(
            Path.Combine(pathToSave.Replace("\"", ""), file.Name),
            FileMode.Create, FileAccess.Write, FileShare.None);

        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task = ctx.AddTask($"[green]Downloading {file.Name}[/]");
                var progress = new Progress<double>(value => { task.Value = value * 100; });
                await _ftpClientEx.DownloadFile(file.Path, stream, progress, cancel: cancellationToken);
                task.StopTask();
            });
    }

    private async Task BurstDownloadFile(IFtpEntry file, CancellationToken cancellationToken)
    {
        var prompt = new TextPrompt<string>("[blue]Local path to save[/]:").AllowEmpty();
        var pathToSave = await prompt.ShowAsync(AnsiConsole.Console, cancellationToken);
        if (string.IsNullOrWhiteSpace(pathToSave)) return;

        await using var stream = new FileStream(
            Path.Combine(pathToSave.Replace("\"", ""), file.Name),
            FileMode.Create, FileAccess.Write, FileShare.None);

        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task = ctx.AddTask($"[green]Downloading {file.Name}[/]");
                var progress = new Progress<double>(value => { task.Value = value * 100; });
                await _ftpClientEx.BurstDownloadFile(file.Path, stream, progress, 239, cancellationToken);
                task.StopTask();
            });
    }

    private async Task RefreshFtpEntries(CancellationToken token)
    {
        await _ftpClientEx.Refresh("/", cancel: token);

        if (_sysFolderIsMissing) return;

        try
        {
            await _ftpClientEx.Refresh("@SYS", cancel: token);
        }
        catch (FtpNackException e)
        {
            if (e.NackError != NackError.FileNotFound) throw;

            _sysFolderIsMissing = true;
            AnsiConsole.MarkupLine("[red]Warning:[/] @SYS folder is missing on device");
        }
    }
}