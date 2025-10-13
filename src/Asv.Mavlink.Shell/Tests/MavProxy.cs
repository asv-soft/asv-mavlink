using System;
using System.Collections.Generic;
using System.IO;
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
    private const string Key = "DIR";
    
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly List<int> _sysId = [];
    private readonly List<int> _msgId = [];
    private StreamWriter? _out;
    private FileStream? _outFileStream;
    private Regex? _nameFilter;
    private Regex? _textFilter;
    private bool _silentMode;

    /// <summary>
    /// Used for connecting vehicle and several ground stations
    /// Example: proxy -l udp://192.168.0.140:14560,udp://192.168.0.140:14550 -o out.txt
    /// </summary>
    /// <param name="links">-l, Add connection to hub. Can be used multiple times. Example: udp://192.168.0.140:45560 or serial://COM5?br=57600</param>
    /// <param name="output">-o, Write filtered messages to file</param>
    /// <param name="silent">-s, Disable printing filtered messages to the screen</param>
    /// <param name="sysIds">-s-ids, Filter for logging: system id field (Example: -s-ids 1 -s-ids 255)</param>
    /// <param name="msgIds">-m-ids, Filter for logging: message id field (Example: -m-ids 1 -m-ids 255)</param>
    /// <param name="namePattern">-n-p, Filter for logging: regex message name filter (Example: -n-p MAV_CMD_D)</param>
    /// <param name="textPattern">-t-p, Filter for logging: regex json text filter (Example: -t-p MAV_CMD_D)</param>
    [Command("proxy")]
    public async Task<int> RunMavProxy(
        string[] links, 
        string? output = null, 
        bool silent = false, 
        int[]? sysIds = null,
        int[]? msgIds = null, 
        string? namePattern = null, 
        string? textPattern = null)
    {
        _silentMode = silent;
        _sysId.AddRange(sysIds ?? []);
        _msgId.AddRange(msgIds ?? []);

        try
        {
            _nameFilter = string.IsNullOrEmpty(namePattern) ? null : new Regex(namePattern, RegexOptions.Compiled);
            _textFilter = string.IsNullOrEmpty(textPattern) ? null : new Regex(textPattern, RegexOptions.Compiled);
            
            if (!string.IsNullOrEmpty(output))
            {
                _outFileStream = File.OpenWrite(output);
                _out = new StreamWriter(_outFileStream);
            }

            var protocol = Protocol.Create(builder => { builder.RegisterMavlinkV2Protocol(); });
            await using var router = protocol.CreateRouter("ROUTER");

            var count = 0;

            foreach (var link in links)
            {
                AnsiConsole.MarkupLine($"[blue]info[/]: Add port: [blue]{link}[/]");

                var port = router.AddPort(link);
                port.Tags.Add(Key, count++);
            }

            AnsiConsole.MarkupLine("[blue]info[/]: Proxying...");

            using var subscription = router.OnRxMessage
                .FilterByType<MavlinkMessage>()
                .SubscribeAwait(OnPacketAsync);

            ConsoleAppHelper.WaitCancelPressOrProcessExit();
            
            if (_out is not null)
            {
                await _out.FlushAsync();
            }
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            return 1;
        }
        finally
        {
            if (_out is not null)
            {
                _out.Close();
                await _out.DisposeAsync();
            }
            
            if (_outFileStream is not null)
            { 
                _outFileStream.Close();
                await _outFileStream.DisposeAsync();
            }
        }

        return 0;
    }
    
    private async ValueTask OnPacketAsync(MavlinkMessage packetV2, CancellationToken token)
    {
        await _lock.WaitAsync(token);
        try
        {
            if (Filter(packetV2, out var txt))
            {
                if (!_silentMode)
                {
                    AnsiConsole.WriteLine(txt);
                }

                if (_out is null)
                {
                    return;
                }

                await _out.WriteLineAsync(txt);
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    private bool Filter(MavlinkMessage packetV2, out string txt)
    {
        var t = new StringEnumConverter();
        txt = JsonConvert.SerializeObject(packetV2, Formatting.None, new StringEnumConverter());

        if (_sysId.Count != 0 && !_sysId.Contains(packetV2.SystemId)) return false;
        if (_msgId.Count != 0 && !_msgId.Contains(packetV2.Id)) return false;
        if (_nameFilter != null && !_nameFilter.IsMatch(packetV2.Name)) return false;
        if (_textFilter != null && !_textFilter.IsMatch(txt)) return false;
        
        return true;
    }
}
