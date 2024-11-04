using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.V2.Common;
using ConsoleAppFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Spectre.Console;
using R3;

namespace Asv.Mavlink.Shell;

public class MavProxy
{
    private readonly CancellationTokenSource _cancel = new();
    private StreamWriter? _out;
    private readonly Subject<ConsoleKeyInfo> _userInput = new();
    private readonly List<MavlinkV2Connection> _connections = new();
    private readonly List<int> _sysId = new();
    private readonly List<int> _msgId = new();
    private readonly List<int> _dirIndex = new();
    private Regex? _nameFilter;
    private bool _silentMode;
    private Regex? _textFilter;

    /// <summary>
    /// Used for connecting vehicle and several ground station
    /// Example: proxy -l udp://192.168.0.140:14560 -l udp://192.168.0.140:14550 -o out.txt
    /// </summary>
    /// <param name="links">-l, Add connection to hub. Can be used multiple times. Example: udp://192.168.0.140:45560 or serial://COM5?br=57600</param>
    /// <param name="outputFile">-o, Write filtered message to file</param>
    /// <param name="silent">-silent, Disable print filtered message to screen</param>
    /// <param name="sysIds">-sys, Filter for logging: system id field (Example: -sys 1 -sys 255)</param>
    /// <param name="msgIds">-id, Filter for logging: message id field (Example: -id 1 -mid 255)</param>
    /// <param name="namePattern">-name, Filter for logging: regex message name filter (Example: -name MAV_CMD_D)</param>
    /// <param name="textPattern">-txt, Filter for logging: regex json text filter (Example: -txt MAV_CMD_D)</param>
    /// <param name="directions">-from, Filter for packet direction: select only input packets from the specified direction</param>
    [Command("proxy")]
    public async Task RunMavProxy(string[] links, string? outputFile = null, bool silent = false, int[]? sysIds = null,
        int[]? msgIds = null, string? namePattern = null, string? textPattern = null, int[]? directions = null)
    {
        _silentMode = silent;
        _sysId.AddRange(sysIds ?? []);
        _msgId.AddRange(msgIds ?? []);
        _dirIndex.AddRange(directions ?? []);
        _nameFilter = string.IsNullOrEmpty(namePattern) ? null : new Regex(namePattern, RegexOptions.Compiled);
        _textFilter = string.IsNullOrEmpty(textPattern) ? null : new Regex(textPattern, RegexOptions.Compiled);

        if (!string.IsNullOrEmpty(outputFile))
        {
            _out = new StreamWriter(File.OpenWrite(outputFile));
        }

        foreach (var link in links)
        {
            AddLink(link);
        }

        using var c = ConsoleAppHelper.WaitCancelPressOrProcessExit();

        _cancel.Cancel();
    }

    private void AddLink(string connectionString)
    {
        var dirIndex = _connections.Count;
        var conn = new MavlinkV2Connection(connectionString, _ => { _.RegisterCommonDialect(); });

        conn.RxPipe.Subscribe(_ => OnPacket(conn, _, dirIndex));
        _connections.Add(conn);
    }

    private void OnPacket(MavlinkV2Connection receiveFrom, IPacketV2<IPayload> packetV2, int dirIndex)
    {
        if (Filter(packetV2, dirIndex, out var txt))
        {
            if (!_silentMode) AnsiConsole.WriteLine(txt);
            _out?.WriteLine(txt);
        }

        foreach (var mavlinkV2Connection in _connections)
        {
            // skip source link
            if (mavlinkV2Connection == receiveFrom) continue;
            mavlinkV2Connection.Send(packetV2, _cancel.Token);
        }
    }

    private bool Filter(IPacketV2<IPayload> packetV2, int dirIndex, out string txt)
    {
        txt = JsonConvert.SerializeObject(packetV2, Formatting.None, new StringEnumConverter());

        if (_sysId.Count != 0 && !_sysId.Contains(packetV2.SystemId)) return false;
        if (_dirIndex.Count != 0 && !_dirIndex.Contains(dirIndex)) return false;
        if (_msgId.Count != 0 && !_msgId.Contains(packetV2.MessageId)) return false;
        if (_nameFilter != null && !_nameFilter.IsMatch(packetV2.Name)) return false;
        if (_textFilter != null && !_textFilter.IsMatch(txt)) return false;
        return true;
    }
}