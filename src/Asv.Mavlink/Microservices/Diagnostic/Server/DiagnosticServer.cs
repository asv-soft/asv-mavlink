using System;
using System.Collections.Concurrent;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink.Diagnostic.Server;

public class DiagnosticServerConfig
{
    public int MaxSendIntervalMs { get; set; } = 100;
    public bool IsEnabled { get; set; } = true;
}

public class DiagnosticServer: MavlinkMicroserviceServer, IDiagnosticServer
{
    private readonly DiagnosticServerConfig _config;
    private readonly DateTime _bootTime;
    private readonly ConcurrentDictionary<string,DateTime> _lastSendFloatTime = new();
    private readonly ConcurrentDictionary<string,DateTime> _lastSendIntTime = new();

    public DiagnosticServer(
        DiagnosticServerConfig config,
        IMavlinkV2Connection connection, 
        MavlinkIdentity identity, 
        IPacketSequenceCalculator seq, 
        IScheduler? rxScheduler = null,
        ILogger? logger = null) : base("DIAG", connection, identity, seq, rxScheduler,logger)
    {
        _config = config;
        _bootTime = DateTime.Now;
        IsEnabled = config.IsEnabled;
    }

    public bool IsEnabled { get; set; }

    public async Task Send(string name, float value,CancellationToken cancel = default)
    {
        if (IsEnabled == false) return;
        
        if (name.Length > NamedValueFloatPayload.NameMaxItemsCount)
        {
            throw new ArgumentException($"Name '{name}' is too long for parameter name (max size {NamedValueFloatPayload.NameMaxItemsCount})", nameof(name));
        }

        if (_lastSendFloatTime.TryGetValue(name, out var lastSendTime))
        {
            if ((DateTime.Now - lastSendTime).TotalMilliseconds < _config.MaxSendIntervalMs)
            {
                return;
            }
        };
        await InternalSend<NamedValueFloatPacket>(packet =>
        {
            MavlinkTypesHelper.SetString(packet.Payload.Name, name);
            packet.Payload.TimeBootMs = (uint)(DateTime.Now - _bootTime).TotalMilliseconds;
            packet.Payload.Value = value;
        }, cancel).ConfigureAwait(false);
        _lastSendFloatTime.AddOrUpdate(name, DateTime.Now, (_, _) => DateTime.Now);
    }
    public async Task Send(string name, int value,CancellationToken cancel = default)
    {
        if (IsEnabled == false) return;
        
        if (name.Length > NamedValueFloatPayload.NameMaxItemsCount)
        {
            throw new ArgumentException($"Name '{name}' is too long for parameter name (max size {NamedValueFloatPayload.NameMaxItemsCount})", nameof(name));
        }

        if (_lastSendIntTime.TryGetValue(name, out var lastSendTime))
        {
            if ((DateTime.Now - lastSendTime).TotalMilliseconds < _config.MaxSendIntervalMs)
            {
                return;
            }
        };
        await InternalSend<NamedValueIntPacket>(packet =>
        {
            MavlinkTypesHelper.SetString(packet.Payload.Name, name);
            packet.Payload.TimeBootMs = (uint)(DateTime.Now - _bootTime).TotalMilliseconds;
            packet.Payload.Value = value;
        }, cancel).ConfigureAwait(false);
        
        _lastSendIntTime.AddOrUpdate(name, DateTime.Now, (_, _) => DateTime.Now);
    }
    
    public Task Send(string name, ushort arrayId, float[] data, CancellationToken cancel = default)
    {
        if (IsEnabled == false) return Task.CompletedTask;
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

        if (name.Length > DebugFloatArrayPayload.NameMaxItemsCount)
        {
            throw new ArgumentException($"Name '{name}' is too long for parameter name (max size {DebugFloatArrayPayload.NameMaxItemsCount})", nameof(name));
        }

        if (data.Length > DebugFloatArrayPayload.DataMaxItemsCount)
        {
            throw new ArgumentException($"Data is too long (max size {DebugFloatArrayPayload.DataMaxItemsCount})", nameof(name));
        }
        return InternalSend<DebugFloatArrayPacket>(packet =>
        {
            packet.Payload.TimeUsec = (uint)(DateTime.Now - _bootTime).TotalMilliseconds;
            packet.Payload.ArrayId = arrayId;
            MavlinkTypesHelper.SetString(packet.Payload.Name, name);
            data.CopyTo(packet.Payload.Data,0);
        }, cancel);
    }

    public Task Send(ushort address, byte version, byte type, sbyte[] value, CancellationToken cancel = default)
    {
        if (IsEnabled == false) return Task.CompletedTask;
        
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (value.Length > MemoryVectPayload.ValueMaxItemsCount)
        {
            throw new ArgumentException($"Value '{value}' is too long (max size {MemoryVectPayload.ValueMaxItemsCount})", nameof(value));
        }
           
        return InternalSend<MemoryVectPacket>(packet =>
        {
            value.CopyTo(packet.Payload.Value, 0);
            packet.Payload.Address = address;
            packet.Payload.Type = type;
            packet.Payload.Ver = version;
        }, cancel);
    }
}