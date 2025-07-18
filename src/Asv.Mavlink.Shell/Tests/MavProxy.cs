using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

using ConsoleAppFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Spectre.Console;
using R3;

namespace Asv.Mavlink.Shell;

public class MavProxy  : IDisposable
{
    private const string Key = "DIR";
    private readonly CancellationTokenSource _cancel = new();
    private readonly List<int> _sysId = new();
    private readonly List<int> _msgId = new();
    
    private IProtocolRouter? _router;
    private IDisposable? _subscription;
    private StreamWriter? _out;
    private Regex? _nameFilter;
    private Regex? _textFilter;
    
    private bool _silentMode;
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
    public Task RunMavProxy(string[] links, string? outputFile = null, bool silent = false, int[]? sysIds = null,
        int[]? msgIds = null, string? namePattern = null, string? textPattern = null)
    {
        _silentMode = silent;
        _sysId.AddRange(sysIds ?? []);
        _msgId.AddRange(msgIds ?? []);
        _nameFilter = string.IsNullOrEmpty(namePattern) ? null : new Regex(namePattern, RegexOptions.Compiled);
        _textFilter = string.IsNullOrEmpty(textPattern) ? null : new Regex(textPattern, RegexOptions.Compiled);

        try
        {
            if (!string.IsNullOrEmpty(outputFile))
            {
                _out = new StreamWriter(File.OpenWrite(outputFile));
            }

            var protocol = Protocol.Create(builder => { builder.RegisterMavlinkV2Protocol(); });
            _router = protocol.CreateRouter("ROUTER");

            links = new string[] { "tcp://127.0.0.1:7341", "tcp://127.0.0.1:5670" };
            foreach (var link in links)
            {
                AddLink(link);
            }

            _subscription = _router.OnRxMessage
                .FilterByType<MavlinkMessage>()
                .Subscribe(OnPacket);

            using var c = ConsoleAppHelper.WaitCancelPressOrProcessExit();
        }
        finally
        {
            _subscription?.Dispose();
            _router?.Dispose();
            _out?.Dispose();
        }

        return Task.CompletedTask;
    }

    private void AddLink(string connectionString)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        var port = _router.AddPort(connectionString);
#pragma warning restore CS8604 // Possible null reference argument.
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

    public void Dispose()
    {
        _cancel.Cancel();
        _out?.Dispose();
        _subscription?.Dispose();
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
                var pi = type.GetProperty(prop.UnderlyingName ?? prop.PropertyName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (pi != null)
                {
                    if (pi.PropertyType.IsByRef) continue; 
                    if (pi.PropertyType.FullName?.Contains("&") == true) continue;
                }

                safeProps.Add(prop);
            }
            catch
            {
                
            }
        }

        return safeProps;
    }
}
