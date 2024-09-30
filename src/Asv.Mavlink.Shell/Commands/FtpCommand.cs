using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Asv.IO;
using DynamicData;
using ManyConsole;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class FtpCommand : ConsoleCommand
{
    private string _connectionString = "tcp://127.0.0.1:5762";
    private ReadOnlyObservableCollection<FtpEntry> _tree;

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
        using var ftpClient = new FtpClient(new MavlinkFtpClientConfig(), conn, identity, seq, TimeProvider.System);
        var ftpEx = new FtpClientEx(ftpClient);
        try
        {
            await ftpEx.Refresh("/");
            await ftpEx.Refresh("@SYS");
            ftpEx.Entries.TransformToTree(x => x.ParentPath).Transform(x => new FtpEntry(x)).DisposeMany()
                .Bind(out _tree).Subscribe();

            var rootNode = CreateFtpTree(_tree);
            AnsiConsole.Write(rootNode);
            
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

    private TreeNode CreateTreeNode(FtpEntry node)
    {
        var treeNode = new TreeNode(new Markup(node.Item.Name));
        foreach (var child in node.Items)
        {
            treeNode.AddNode(CreateTreeNode(child));
        }

        return treeNode;
    }

    private Tree CreateFtpTree(ReadOnlyObservableCollection<FtpEntry> ftpEntries)
    {
        var rootNode = new Tree("FTP Directory").Guide(TreeGuide.BoldLine).Style("green");
        var rootEntries = ftpEntries.Where(e => e.Depth == 0).ToList();

        foreach (var rootEntry in rootEntries)
        {
            rootNode.AddNode(CreateTreeNode(rootEntry));
        }

        return rootNode;
    }
}