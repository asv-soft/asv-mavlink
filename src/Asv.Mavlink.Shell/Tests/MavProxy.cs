using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Asv.IO;
using ConsoleAppFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Spectre.Console;
using R3;

namespace Asv.Mavlink.Shell;

public class MavProxy
{
    private const string Key = "DIR";
    
    private readonly List<int> _sysId = [];
    private readonly List<int> _msgId = [];
    private StreamWriter? _out;
    private FileStream? _outFileStream;
    private Regex? _nameFilter;
    private Regex? _textFilter;
    private bool _silentMode;

    /// <summary>
    /// Used for connecting vehicle and several ground station
    /// Example: proxy -l udp://192.168.0.140:14560,udp://192.168.0.140:14550 -o out.txt
    /// </summary>
    /// <param name="links">-l, Add connection to hub. Can be used multiple times. Example: udp://192.168.0.140:45560 or serial://COM5?br=57600</param>
    /// <param name="outputFile">-o, Write filtered message to file</param>
    /// <param name="silent">-silent, Disable print filtered message to screen</param>
    /// <param name="sysIds">-sys, Filter for logging: system id field (Example: -sys 1 -sys 255)</param>
    /// <param name="msgIds">-id, Filter for logging: message id field (Example: -id 1 -mid 255)</param>
    /// <param name="namePattern">-name, Filter for logging: regex message name filter (Example: -name MAV_CMD_D)</param>
    /// <param name="textPattern">-txt, Filter for logging: regex json text filter (Example: -txt MAV_CMD_D)</param>
    [Command("proxy")]
    public async Task<int> RunMavProxy(
        string[] links, 
        string? outputFile = null, 
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
            
            if (!string.IsNullOrEmpty(outputFile))
            {
                _outFileStream = File.OpenWrite(outputFile);
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
                .Subscribe(OnPacket);

            ConsoleAppHelper.WaitCancelPressOrProcessExit();
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            return 1;
        }
        finally
        {
            await (_outFileStream?.DisposeAsync() ?? ValueTask.CompletedTask);
            await (_out?.DisposeAsync() ?? ValueTask.CompletedTask);
        }

        return 0;
    }
    
    private void OnPacket(MavlinkMessage packetV2)
    {
        if (Filter(packetV2, out var txt))
        {
            if (!_silentMode)
            {
                AnsiConsole.WriteLine(txt);
            }
            
            _out?.WriteLine(txt);
        }
    }

    private bool Filter(MavlinkMessage packetV2, out string txt)
    {
        txt = JsonConvert.SerializeObject(packetV2, Formatting.None, new JsonSerializerSettings
        {
            ContractResolver = new SafeContractResolver(),
            Converters = { new StringEnumConverter() }
        });

        if (_sysId.Count != 0 && !_sysId.Contains(packetV2.SystemId)) return false;
        if (_msgId.Count != 0 && !_msgId.Contains(packetV2.Id)) return false;
        if (_nameFilter != null && !_nameFilter.IsMatch(packetV2.Name)) return false;
        if (_textFilter != null && !_textFilter.IsMatch(txt)) return false;
        
        return true;
    }
}

public class SafeContractResolver : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        var allProps = base.CreateProperties(type, memberSerialization);
        var safeProps = new List<JsonProperty>();

        foreach (var prop in allProps)
        {
            try
            {
                var propertyName = prop.UnderlyingName ?? prop.PropertyName;

                if (propertyName is null)
                {
                    continue;
                }
                
                var pi = type.GetProperty(propertyName, 
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (pi != null)
                {
                    if (pi.PropertyType.IsByRef) continue; 
                    if (pi.PropertyType.FullName?.Contains('&') == true) continue;
                }

                safeProps.Add(prop);
            }
            catch
            {
                // nothing to do
            }
        }

        return safeProps;
    }
}
