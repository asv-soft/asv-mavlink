using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using ConsoleAppFramework;
using DynamicData;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class FtpTreeDirectory
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private ReadOnlyObservableCollection<FtpEntry> _tree;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private readonly SourceCache<IFtpEntry,string> _entryCache = new(x => x.Path);
    private IObservable<IChangeSet<IFtpEntry, string>> Entries => _entryCache.Connect();

    /// <summary>
    /// Tree representation of all available files and directories on the drone's FTP server
    /// </summary>
    /// <param name="connection">-cs, The address of the connection to the mavlink device</param>
    [Command("ftp-tree")]
    public async Task RunFtpTree(string connection)
    {
        await using var conn = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
        }).CreateRouter("ROUTER");
        conn.AddPort(connection);
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
        await using var ftpClient = new FtpClient(identity, config, core);
        var ftpEx = new FtpClientEx(ftpClient);
        try
        {
            await ftpEx.Refresh("/");
            await ftpEx.Refresh("@SYS");
            _entryCache.AddOrUpdate(ftpEx.Entries.Values);
            Entries.TransformToTree(x => x.ParentPath)
                .Transform(x => new FtpEntry(x))
                .DisposeMany()
                .Bind(out _tree)
                .Subscribe();

            var rootNode = CreateFtpTree(_tree);
            AnsiConsole.Write(rootNode);
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
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