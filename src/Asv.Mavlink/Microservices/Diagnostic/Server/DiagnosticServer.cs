using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Diagnostic.Server;

public class DiagnosticServerConfig
{
    public int MaxSendIntervalMs { get; set; } = 100;
    public bool IsEnabled { get; set; } = true;
}

public class DiagnosticServer: MavlinkMicroserviceServer, IDiagnosticServer
{
    private readonly DiagnosticServerConfig _config;
    private readonly long _bootTime;
    private readonly ConcurrentDictionary<string,long> _lastSendFloatTime = new();
    private readonly ConcurrentDictionary<string,long> _lastSendIntTime = new();

    public DiagnosticServer(MavlinkIdentity identity, DiagnosticServerConfig config,ICoreServices core) 
        : base("DIAG", identity, core)
    {
        _config = config;
        _bootTime = core.TimeProvider.GetTimestamp();
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
            if (Core.TimeProvider.GetElapsedTime(lastSendTime).TotalMilliseconds < _config.MaxSendIntervalMs)
            {
                return;
            }
        };
        await InternalSend<NamedValueFloatPacket>(packet =>
        {
            MavlinkTypesHelper.SetString(packet.Payload.Name, name);
            packet.Payload.TimeBootMs = (uint)Core.TimeProvider.GetElapsedTime(_bootTime).TotalMilliseconds;
            packet.Payload.Value = value;
        }, cancel).ConfigureAwait(false);
        _lastSendFloatTime.AddOrUpdate(name, Core.TimeProvider.GetTimestamp(), (_, _) => Core.TimeProvider.GetTimestamp());
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
            if (Core.TimeProvider.GetElapsedTime(lastSendTime).TotalMilliseconds < _config.MaxSendIntervalMs)
            {
                return;
            }
        };
        await InternalSend<NamedValueIntPacket>(packet =>
        {
            MavlinkTypesHelper.SetString(packet.Payload.Name, name);
            packet.Payload.TimeBootMs = (uint)Core.TimeProvider.GetElapsedTime(_bootTime).TotalMilliseconds;
            packet.Payload.Value = value;
        }, cancel).ConfigureAwait(false);
        
        _lastSendIntTime.AddOrUpdate(name, Core.TimeProvider.GetTimestamp(), (_, _) => Core.TimeProvider.GetTimestamp());
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
            packet.Payload.TimeUsec = (uint)(uint)Core.TimeProvider.GetElapsedTime(_bootTime).TotalMilliseconds;
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