using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using DynamicData;
using ManyConsole;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class FtpCommand : ConsoleCommand
{
    private string _connectionString = "tcp://127.0.0.1:5762";
    private readonly CancellationTokenSource _cancel = new CancellationTokenSource();
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
            var tree = new List<Node<IFtpEntry, string>>();
            await ftpEx.Refresh("/");
            await ftpEx.Refresh("@SYS");
            ftpEx.Entries.TransformToTree(x => x.ParentPath).Transform(x => new FtpEntry(x)).DisposeMany().Bind(out _tree).Subscribe();
            
            var rootNode = new Tree("FTP Directory").Guide(TreeGuide.BoldLine).Style("green");
            var treeNode = rootNode.AddNode("root");
            foreach (var node in _tree)
            {
                AddNodesToTree(treeNode, node);
            }
            AnsiConsole.Write(rootNode);
            
            var root = _tree.FirstOrDefault(e => e.IsRoot);
            if (root != null)
            {
                await CreateNavigateDirectory(root);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Root directory not found.[/red]");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private void AddNodesToTree(TreeNode tree,  FtpEntry node)
    {
        var treeNode = tree.AddNode(node.Key);
        foreach (var childrenItem in node.Items)
        {
            AddNodesToTree(treeNode, childrenItem);
        }
    }
    private async Task CreateNavigateDirectory(FtpEntry currentDirectory, FtpEntry? parentDirectory = null)
    {
        var choices = new List<FtpEntry>();
        if (parentDirectory != null)
        {
            var parentEntry = new FtpEntry(new Node<IFtpEntry, string>(new FtpEntryModel
            {
                Name = "...",
                Path = parentDirectory.Key,
                ParentPath = currentDirectory.Item.ParentPath,
                Type = FtpEntryType.Directory
            }, currentDirectory.Node.Key));
            choices.Add(parentEntry);
        }

        choices.AddRange(currentDirectory.Items);
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<FtpEntry>()
                .Title($"[green]Current Directory:[/] [yellow]{currentDirectory.Key}[/]")
                .AddChoices(choices)
                .UseConverter(entry => entry.Item.Name + (entry.Item.Type == FtpEntryType.Directory ? "/" : "")));
        if (selection.Item.Name == "...")
        {
            await CreateNavigateDirectory(parentDirectory!, new FtpEntry(parentDirectory.Node));
        }
        else if (selection.Item.Type == FtpEntryType.Directory)
        {
            await CreateNavigateDirectory(selection, currentDirectory);
        }
        else
        {
            AnsiConsole.MarkupLine($"[green]Selected file:[/] [yellow]{selection.Key}[/]");
        }
    }
}
