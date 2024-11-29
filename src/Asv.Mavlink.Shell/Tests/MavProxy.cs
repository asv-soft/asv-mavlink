using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

using ConsoleAppFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Spectre.Console;
using R3;

namespace Asv.Mavlink.Shell;

public class MavProxy
{
    private readonly CancellationTokenSource _cancel = new();
    private const string Key = "DIR";
    private StreamWriter? _out;
    private readonly Subject<ConsoleKeyInfo> _userInput = new();
    private IProtocolRouter? _router;
    private readonly List<int> _sysId = new();
    private readonly List<int> _msgId = new();
    private Regex? _nameFilter;
    private bool _silentMode;
    private Regex? _textFilter;
    private int _count;

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
    [Command("proxy")]
    public async Task RunMavProxy(string[] links, string? outputFile = null, bool silent = false, int[]? sysIds = null,
        int[]? msgIds = null, string? namePattern = null, string? textPattern = null)
    {
        _silentMode = silent;
        _sysId.AddRange(sysIds ?? []);
        _msgId.AddRange(msgIds ?? []);
        _nameFilter = string.IsNullOrEmpty(namePattern) ? null : new Regex(namePattern, RegexOptions.Compiled);
        _textFilter = string.IsNullOrEmpty(textPattern) ? null : new Regex(textPattern, RegexOptions.Compiled);

        if (!string.IsNullOrEmpty(outputFile))
        {
            _out = new StreamWriter(File.OpenWrite(outputFile));
        }

        _router = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();

        }).CreateRouter("ROUTER");
        
        foreach (var link in links)
        {
            AddLink(link);
        }
        _router.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(OnPacket);
        using var c = ConsoleAppHelper.WaitCancelPressOrProcessExit();

        _cancel.Cancel();
    }

    private void AddLink(string connectionString)
    {
        var port = _router.AddPort(connectionString);
        port.Tags.Add(Key, _count++);
    }

    

    private void OnPacket(MavlinkMessage packetV2)
    {
        if (Filter(packetV2, out var txt))
        {
            if (!_silentMode) AnsiConsole.WriteLine(txt);
            _out?.WriteLine(txt);
        }

    }

    private bool Filter(MavlinkMessage packetV2, out string txt)
    {
        txt = JsonConvert.SerializeObject(packetV2, Formatting.None, new StringEnumConverter());

        if (_sysId.Count != 0 && !_sysId.Contains(packetV2.SystemId)) return false;
        if (_msgId.Count != 0 && !_msgId.Contains(packetV2.Id)) return false;
        if (_nameFilter != null && !_nameFilter.IsMatch(packetV2.Name)) return false;
        if (_textFilter != null && !_textFilter.IsMatch(txt)) return false;
        return true;
    }
}