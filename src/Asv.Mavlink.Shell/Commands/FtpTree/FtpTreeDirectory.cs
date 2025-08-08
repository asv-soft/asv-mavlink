using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using ConsoleAppFramework;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class FtpTreeDirectory
{
    /// <summary>
    ///     Tree representation of all available files and directories on the drone's FTP server
    /// </summary>
    /// <param name="connection">-cs, The address of the connection to the mavlink device, e.g. tcp://127.0.0.1:5760</param>
    /// <param name="timeoutMs">-t, The connection timeout in ms</param>
    /// <param name="commandAttemptCount"> The command attempts count</param>
    /// <param name="targetNetworkId">-tid, The target id of the network</param>
    /// <param name="burstTimeoutMs"> The burst timeout in ms</param>
    [Command("ftp-tree")]
    public async Task RunFtpTree(
        string connection,
        int timeoutMs = 1000,
        int commandAttemptCount = 5,
        byte targetNetworkId = 0,
        int burstTimeoutMs = 1000)
    {
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
            await using var ftpClient = new FtpClient(identity, config, core);
            await using var ftpEx = new FtpClientEx(ftpClient);

            try
            {
                await ftpEx.Refresh(MavlinkFtpHelper.DirectorySeparator.ToString());
            }
            catch (FtpNackException e)
            {
                AnsiConsole.MarkupLine(
                    "[yellow]Warning:[/] some folders could not be loaded, likely due to invalid encoding");
                AnsiConsole.WriteException(e);
            }
            
            try
            {
                await ftpEx.Refresh("@SYS");
            }
            catch (FtpNackException e)
            {
                if (e.NackError != NackError.FileNotFound) throw;
                AnsiConsole.MarkupLine("[yellow]Warning:[/] @SYS folder is likely missing on device");
            }

            AnsiConsole.MarkupLine(string.Empty);

            var treeEntries = BuildFtpEntryTree(ftpEx.Entries.Values);
            var rootNode = CreateFtpTree(treeEntries.Values);

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
        foreach (var child in node.Children) treeNode.AddNode(CreateTreeNode(child));

        return treeNode;
    }

    private Tree CreateFtpTree(IEnumerable<FtpEntry> ftpEntries)
    {
        var rootNode = new Tree("FTP Directory").Guide(TreeGuide.BoldLine).Style("green");
        var rootEntries = ftpEntries.Where(e => e.IsRoot).ToList();

        foreach (var rootEntry in rootEntries) rootNode.AddNode(CreateTreeNode(rootEntry));

        return rootNode;
    }

    private Dictionary<string, FtpEntry> BuildFtpEntryTree(IEnumerable<IFtpEntry> rawFtpEntries)
    {
        var ftpEntryMap = new Dictionary<string, FtpEntry>();
        var sortedEntries = rawFtpEntries
            .OrderBy(e => e.Path.Count(c => c == MavlinkFtpHelper.DirectorySeparator));

        foreach (var entry in sortedEntries)
            if (string.IsNullOrWhiteSpace(entry.ParentPath))
            {
                var ftpEntry = new FtpEntry(entry.Path, entry, 0);
                ftpEntryMap[entry.Path] = ftpEntry;
            }
            else if (ftpEntryMap.TryGetValue(entry.ParentPath, out var parent))
            {
                var ftpEntry = new FtpEntry(entry.Path, entry, parent.Depth + 1);
                ftpEntryMap[entry.Path] = ftpEntry;

                parent.Children.Add(ftpEntry);
            }

        return ftpEntryMap;
    }
}