using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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

            AnsiConsole.MarkupLine("Menu for directory: [red]F1[/]");
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

        while (true)
        {
            AnsiConsole.MarkupLine("Display menu?");
            if (Console.ReadKey(true).Key == ConsoleKey.F1)
            {
                if (currentDirectory != null && currentDirectory.Item.Type == FtpEntryType.Directory)
                {
                    await DirectoryOperationsMenu(currentDirectory);
                }
                continue;
            }

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
                    stack.Push(selection);
                    await CreateFtpBrowser(tree, stack);
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
                .AddChoices([".../", "Remove file", "Calculate CRC32", "Truncate file", "Read file", "Rename file"])
        );

        switch (fileOperation)
        {
            case "Remove file":
                await _ftpClient.RemoveFile(file.Item.Path, token.Token);
                AnsiConsole.MarkupLine("[green]File was removed![/]");
                break;
            case "Calculate CRC32":
                var crc32 = await _ftpClient.CalcFileCrc32(file.Item.Path, token.Token);
                AnsiConsole.MarkupLine($"[yellow]CRC32[/]: [green]{crc32.ReadDataAsInt():X}[/]");
                break;
            case "Truncate file":
                await Truncate(_ftpClient, file.Item.Path, token.Token); //TODO: Поправить, сейчас не отрабатывает
                break;
            case "Read file":
                await ReadFile(_ftpClient, file.Item.Path); //TODO: Поправить, сейчас не отрабатывает
                break;
            case "Rename file":
                await RenameImpl(file, token.Token); //TODO: Поправить, сейчас не отрабатывает
                break;
            case ".../":
                await token.CancelAsync();
                return;
        }
    }

    private async Task DirectoryOperationsMenu(FtpEntry directory)
    {
        var token = new CancellationTokenSource();
        var fileOperation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[blue]Select the operation for the file[/] [yellow]{directory.Item.Path}[/]:")
                .AddChoices([".../", "Create directory", "Remove directory", "List directory", "Rename directory"])
        );

        switch (fileOperation)
        {
            case "Create directory":
                await CreateDirectoryImpl(_ftpClient, directory, token.Token);
                break;
            case "Remove directory":
                await _ftpClient.RemoveDirectory(directory.Item.Path, token.Token);
                AnsiConsole.MarkupLine("[green]File was removed![/]");
                break;
            case "List directory":
                await _ftpClient.ListDirectory(directory.Item.Path, 0, token.Token);
                AnsiConsole.MarkupLine($"[green]List directory[/] [yellow] {directory.Item.Path}[/]");
                break;
            case "Rename directory":
                await RenameImpl(directory, token.Token); //TODO: Поправить, сейчас не отрабатывает
                break;
            case ".../":
                await token.CancelAsync();
                break;
        }
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

    private async Task ReadFile(IFtpClient ftpClient, string filePath)
    {
        var handle = await ftpClient.OpenFileRead(filePath);
        AnsiConsole.MarkupLine($"Открыт файл: {filePath}, Размер: {handle.Size} байт.");

        var buffer = new byte[1024];
        var request = new ReadRequest(handle.Session, 0, (byte)buffer.Length);
        var result = await ftpClient.ReadFile(request, buffer);

        AnsiConsole.MarkupLine($"Прочитано: {result.ReadCount} байт.");
        AnsiConsole.WriteLine(Encoding.Default.GetString(buffer, 0, result.ReadCount));
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
}