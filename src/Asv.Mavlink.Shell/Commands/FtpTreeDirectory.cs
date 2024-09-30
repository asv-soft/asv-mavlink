using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Asv.IO;
using ConsoleAppFramework;
using DynamicData;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class FtpTreeDirectory
{
    private readonly string _connectionString = "tcp://127.0.0.1:5762";
    private ReadOnlyObservableCollection<FtpEntry> _tree;

    [Command("FtpTree")]
    public async Task RunFtpTree()
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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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