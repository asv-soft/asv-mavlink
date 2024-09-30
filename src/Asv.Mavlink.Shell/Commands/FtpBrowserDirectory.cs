using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    [Command("FtpBrowser")]
    public async Task RunFtpBrowser()
    {
        using var port = PortFactory.Create(_connectionString);
        port.Enable();
        using var conn = MavlinkV2Connection.Create(port);
        var identity = new MavlinkClientIdentity(255, 255, 1, 1);
        var seq = new PacketSequenceCalculator();
        using var ftpClient = new FtpClient(new MavlinkFtpClientConfig(), conn, identity, seq, TimeProvider.System);
        var ftpEx = new FtpClientEx(ftpClient);
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
        FtpEntry currentDirectory = stack.Count > 0 ? stack.Peek() : null;
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
                .Title($"[green]Current Directory:[/] [yellow]{(currentDirectory?.Key ?? "Root")}[/]")
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
                AnsiConsole.MarkupLine($"[green]Selected file:[/] [yellow]{selection.Key}[/]");
                break;
            default:
                throw new Exception("Missing directory");
        }
    }
}