using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using ConsoleAppFramework;
using DynamicData;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class FtpBrowserDirectory
{
    private readonly string _connectionString = "tcp://127.0.0.1:5762";
    private ReadOnlyObservableCollection<FtpEntry> _tree;
    private FtpClient _ftpClient;

    [Command("ftp-browser")]
    public async Task RunFtpBrowser()
    {
        using var port = PortFactory.Create(_connectionString);
        port.Enable();
        using var conn = MavlinkV2Connection.Create(port);
        var identity = new MavlinkClientIdentity(255, 255, 1, 1);
        var seq = new PacketSequenceCalculator();
        _ftpClient = new FtpClient(new MavlinkFtpClientConfig(), conn, identity, seq, TimeProvider.System);
        var ftpEx = new FtpClientEx(_ftpClient);
        try
        {
            await ftpEx.Refresh("/");
            await ftpEx.Refresh("@SYS");
            ftpEx.Entries.TransformToTree(x => x.ParentPath).Transform(x => new FtpEntry(x)).DisposeMany()
                .Bind(out _tree).Subscribe();
            
            AnsiConsole.MarkupLine("Keymap: Reload: [red]F8[/]; Delete: [red]F10[/]; Copy: [red]F10[/];");
            await CreateFtpBrowser(_tree);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private async Task CreateFtpBrowser(ReadOnlyObservableCollection<FtpEntry> tree,
        Stack<FtpEntry> stack = null)
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

        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<FtpEntry>()
                .Title($"[blue]Current Directory:[/] [yellow]{(currentDirectory?.Key ?? "Root")}[/]")
                .AddChoices(choices)
                .UseConverter(entry => entry.Item.Name + (entry.Item.Type == FtpEntryType.Directory ? "/" : "")));

        switch (selection.Item)
        {
            case { Name: "..." }:
                stack.Pop();
                await CreateFtpBrowser(tree, stack);
                break;

            case { Type: FtpEntryType.Directory }:
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.F1)
                    {
                        await DirectoryOperationsMenu(_ftpClient, selection);
                        break;
                    }
                }
                stack.Push(selection);
                await CreateFtpBrowser(tree, stack);
                break;
            case { Type: FtpEntryType.File }:
                AnsiConsole.MarkupLine($"[blue]Selected file:[/] [yellow]{selection.Key}[/]");
                await FileOperationsMenu(_ftpClient, selection);
                break;
            default:
                throw new Exception("Missing directory");
        }
    }
    
    public async Task FileOperationsMenu(IFtpClient ftpClient, FtpEntry file)
    {
        var fileOperation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[blue]Select the operation for the file[/] [yellow]{file.Item.Path}[/]:")
                .AddChoices(["Remove file", "Calculate CRC32", "Truncate file", "Read file", "Rename file", "Close"])
        );

        switch (fileOperation)
        {
            case "Remove file":
                await _ftpClient.RemoveFile(file.Item.Path);
                break;
            case "Calculate CRC32":
                await ftpClient.CalcFileCrc32(file.Item.Path);
                AnsiConsole.MarkupLine("[green]File was removed![/]");
                break;
            case "Truncate file":
                await _ftpClient.TruncateFile(new TruncateRequest());
                break;
            case "Read file":
                await _ftpClient.ReadFile(new ReadRequest());
                break; 
            case "Rename file":
                await RenameIml(file, new CancellationToken());
                break; 
            case "Close":
                return;
        }
    }

    
    private async Task DirectoryOperationsMenu(IFtpClient ftpClient, FtpEntry directory)
    {
        var fileOperation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[blue]Select the operation for the file[/] [yellow]{directory.Item.Path}[/]:")
                .AddChoices(["Create directory", "Remove directory", "List directory", "Rename directory", "Close"])
        );

        switch (fileOperation)
        {
            case "Create directory":
                await _ftpClient.CreateDirectory(directory.Item.Path);
                AnsiConsole.MarkupLine("[green]File was created![/]");
                break;
            case "Remove directory":
                await ftpClient.RemoveDirectory(directory.Item.Path);
                AnsiConsole.MarkupLine("[green]File was removed![/]");
                break;
            case "List directory":
                await _ftpClient.ListDirectory(directory.Item.Path, 0);
                AnsiConsole.MarkupLine($"[green]List directory[/] [yellow] {directory.Item.Path}[/]");
                break;
            case "Rename directory":
                await RenameIml(directory, new CancellationToken());
                break; 
            case "Close":
                return;
        }
    }
    private async Task RenameIml(FtpEntry item, CancellationToken cancellationToken)
    {
        var fileName = AnsiConsole.Prompt(
            new TextPrompt<string>("New name: "));
        var lastIndex = item.Item.Path.LastIndexOf('/');
        var directoryPath = item.Item.Path[..(lastIndex + 1)];
        await _ftpClient.Rename(item.Item.Path, directoryPath + fileName, new CancellationToken());
        AnsiConsole.MarkupLine($"[green]Directory was renamed![/]");
    }
}
