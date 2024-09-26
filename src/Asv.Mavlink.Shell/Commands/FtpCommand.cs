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
                AddToTree(treeNode, node);
            }
            AnsiConsole.Write(rootNode);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private void AddToTree(TreeNode tree,  FtpEntry node)
    {
        var treeNode = tree.AddNode(node.Key);
        foreach (var childrenItem in node.Items)
        {
            AddToTree(treeNode, childrenItem);
        }
    }
}
