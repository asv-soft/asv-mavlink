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
    public async Task<int> RunFtpBrowser(
        string connection,
        int timeoutMs = 1000,
        int commandAttemptCount = 5,
        byte targetNetworkId = 0,
        int burstTimeoutMs = 1000)
    {
        var token = new CancellationTokenSource();

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            token.Cancel();
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
                    "[yellow]Warning:[/] some folders could not be loaded, likely due to invalid encoding");
                AnsiConsole.WriteException(e);
            }
            
            AnsiConsole.MarkupLine(string.Empty);
            
            await CreateFtpBrowser(token: token.Token);
            return 0;
        }
        catch (OperationCanceledException)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[blue]info[/] The Command was stopped by the user");
            return 1;
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            return -1;
        }
        finally
        {
            await _ftpClient.DisposeAsync();
            await _ftpClientEx.DisposeAsync();
            token.Dispose();
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
            token.ThrowIfCancellationRequested();
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

            var selection = await AnsiConsole.PromptAsync(
                new SelectionPrompt<IFtpEntry>()
                    .Title($"[blue]Current Directory:[/] [yellow]{currentDirectory?.Path ?? "Root"}[/]")
                    .AddChoices(choices)
                    .UseConverter(entry => entry.Name + (entry.Type == FtpEntryType.Directory ? 
                        MavlinkFtpHelper.DirectorySeparator : string.Empty)), token);

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
            
            AnsiConsole.WriteLine("Press any key to continue...");
            await AnsiConsole.Console.Input.ReadKeyAsync(true, token);
            AnsiConsole.Clear();
        }
    }

    public async Task FileOperationsMenu(IFtpEntry file, CancellationToken token)
    {
        var fileOperation = await AnsiConsole.PromptAsync(
            new SelectionPrompt<string>()
                .Title($"[blue]Select the operation for the file[/] [yellow]{file.Path}[/]:")
                .AddChoices(
                    FileOperation.Remove, FileOperation.CalculateCRC32,
                    FileOperation.Truncate, FileOperation.Download,
                    FileOperation.BurstDownload, FileOperation.Rename,
                    CancelOperationVariant), token);

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
                AnsiConsole.MarkupLine("[yellow]Warning[/]: unsupported.");
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
        var directoryOperation = await AnsiConsole.PromptAsync(
            new SelectionPrompt<string>()
                .Title($"[blue]Select the operation for the directory[/] [yellow]{directory.Path}[/]:")
                .AddChoices(
                    DirectoryOperation.Open, DirectoryOperation.Create,
                    DirectoryOperation.Remove, DirectoryOperation.RemoveRecursive, 
                    DirectoryOperation.Rename, CancelOperationVariant), token);

        switch (directoryOperation)
        {
            case DirectoryOperation.Create:
                await CreateDirectoryImpl(directory, token);
                await RefreshFtpEntries(token);
                break;
            case DirectoryOperation.Remove:
                await _ftpClientEx.RemoveDirectory(directory.Path, false, token);
                AnsiConsole.MarkupLine("[green]Directory was removed![/]");
                break;
            case DirectoryOperation.RemoveRecursive:
                await _ftpClientEx.RemoveDirectory(directory.Path, true, token);
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

        var lastIndex = item.Path
            .TrimEnd(MavlinkFtpHelper.DirectorySeparator)
            .LastIndexOf(MavlinkFtpHelper.DirectorySeparator);
        var directoryPath = item.Path[..(lastIndex + 1)];
        await _ftpClient.Rename(item.Path, directoryPath + fileName, cancellationToken);
        AnsiConsole.MarkupLine("[green]Item was renamed![/]");
    }
    
    // TODO: fix implementation
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

        var pathToCreate = item.Path.TrimEnd(MavlinkFtpHelper.DirectorySeparator) + MavlinkFtpHelper.DirectorySeparator;
        await _ftpClient.CreateDirectory(pathToCreate + dirName, cancellationToken);
        AnsiConsole.MarkupLine("[green]Directory was created![/]");
    }

    private async Task DownloadFile(IFtpEntry file, CancellationToken cancellationToken)
    {
        var prompt = new TextPrompt<string>("[blue]Local path to save[/]:").AllowEmpty();
        var pathToSave = await prompt.ShowAsync(AnsiConsole.Console, cancellationToken);
        if (string.IsNullOrWhiteSpace(pathToSave)) return;

        await using var stream = new FileStream(
            Path.Combine(pathToSave, file.Name),
            FileMode.Create, FileAccess.Write, FileShare.None);

        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task = ctx.AddTask($"[green]Downloading {file.Name}[/]");
                var progress = new Progress<double>(value => { task.Value = value * 100; });
                await _ftpClientEx.DownloadFile(file.Path, stream, progress, cancel: cancellationToken);
                task.StopTask();
            });
        
        AnsiConsole.MarkupLine("[green]Success![/]");
    }

    private async Task BurstDownloadFile(IFtpEntry file, CancellationToken cancellationToken)
    {
        var prompt = new TextPrompt<string>("[blue]Local path to save[/]:").AllowEmpty();
        var pathToSave = await prompt.ShowAsync(AnsiConsole.Console, cancellationToken);
        if (string.IsNullOrWhiteSpace(pathToSave)) return;

        await using var stream = new FileStream(
            Path.Combine(pathToSave, file.Name),
            FileMode.Create, FileAccess.Write, FileShare.None);

        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task = ctx.AddTask($"[green]Downloading {file.Name}[/]");
                var progress = new Progress<double>(value => { task.Value = value * 100; });
                await _ftpClientEx.BurstDownloadFile(
                    file.Path, stream, progress, MavlinkFtpHelper.MaxDataSize, cancellationToken);
                task.StopTask();
            });
        
        AnsiConsole.MarkupLine("[green]Success![/]");
    }

    private async Task RefreshFtpEntries(CancellationToken token)
    {
        await _ftpClientEx.Refresh(MavlinkFtpHelper.DirectorySeparator.ToString(), cancel: token);

        if (_sysFolderIsMissing) return;

        try
        {
            await _ftpClientEx.Refresh("@SYS", cancel: token);
        }
        catch (FtpNackException e)
        {
            if (e.NackError != NackError.FileNotFound) throw;

            _sysFolderIsMissing = true;
            AnsiConsole.MarkupLine("[yellow]Warning:[/] @SYS folder is likely missing on device");
        }
    }
}