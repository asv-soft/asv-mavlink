using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvAudio;
using Microsoft.Extensions.Logging;
using ObservableCollections;
using R3;
using ZLogger;

namespace Asv.Mavlink;


public class AudioServiceConfig
{
    public int DeviceTimeoutMs { get; set; } = 10_000;
    public int OnlineRateMs { get; set; } = 1_000;
    public int RemoveDeviceCheckDelayMs { get; set; } = 3_000;
}

public class AudioService : IAudioService,IDisposable, IAsyncDisposable
{
    private readonly ILogger _logger; 
    private readonly IAudioCodecFactory _codecFactory;
    private readonly MavlinkIdentity _identity;
    private readonly ICoreServices _core;
    private readonly ObservableDictionary<MavlinkIdentity,IAudioDevice> _devices;
    private readonly MavlinkPacketTransponder<AsvAudioOnlinePacket,AsvAudioOnlinePayload> _transponder;
    private readonly ReactiveProperty<bool> _isOnline;
    private readonly ReactiveProperty<AsvAudioCodec?> _codec;
    private readonly ReactiveProperty<bool> _speakerEnabled;
    private readonly ReactiveProperty<bool> _micEnabled;
    private readonly TimeSpan _deviceTimeout;
    private readonly TimeSpan _onlineRate;
    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;
    private readonly ITimer _timer;
    private readonly IDisposable _sub3;
    private readonly IDisposable _sub4;
    
    public AudioService(IAudioCodecFactory codecFactory,MavlinkIdentity identity,
        AudioServiceConfig config, ICoreServices core)
    {
        _logger = core.Log.CreateLogger<AudioService>();
        _codecFactory = codecFactory ?? throw new ArgumentNullException(nameof(codecFactory));
        _identity = identity;
        _core = core;
        var config1 = config ?? throw new ArgumentNullException(nameof(config));
        _deviceTimeout = TimeSpan.FromMilliseconds(config1.DeviceTimeoutMs);
        _onlineRate = TimeSpan.FromMilliseconds(config1.OnlineRateMs);
        
        _devices = new ObservableDictionary<MavlinkIdentity,IAudioDevice>();
        _sub1 = core.Connection.Filter<AsvAudioOnlinePacket>().Where(_=>IsOnline.CurrentValue).Subscribe(OnRecvDeviceOnline);
        _sub2 = core.Connection.Filter<AsvAudioStreamPacket>().Where(_=>IsOnline.CurrentValue).Subscribe(OnRecvAudioStream);
        
        _timer = core.TimeProvider.CreateTimer(RemoveOldDevice, null, TimeSpan.FromMilliseconds(config.RemoveDeviceCheckDelayMs), TimeSpan.FromMilliseconds(config.RemoveDeviceCheckDelayMs));
        _transponder = new MavlinkPacketTransponder<AsvAudioOnlinePacket,AsvAudioOnlinePayload>(identity,core);
        _isOnline = new ReactiveProperty<bool>(false);
        _codec = new ReactiveProperty<AsvAudioCodec?>();
        _speakerEnabled = new ReactiveProperty<bool>(false);
        _sub3 = _speakerEnabled.Subscribe(speakerEnabled=>_transponder.Set(p=>
        {
            if (speakerEnabled)
            {
                p.Mode |= AsvAudioModeFlag.AsvAudioModeFlagSpeakerOn;
            }
            else
            {
                p.Mode &= ~AsvAudioModeFlag.AsvAudioModeFlagSpeakerOn;
            }
        }));
        _micEnabled = new ReactiveProperty<bool>(false);
        _sub4 = _micEnabled.Subscribe(micEnabled=>_transponder.Set(p=>
        {
            if (micEnabled)
            {
                p.Mode |= AsvAudioModeFlag.AsvAudioModeFlagMicOn;
            }
            else
            {
                p.Mode &= ~AsvAudioModeFlag.AsvAudioModeFlagMicOn;
            }
        }));
    }

    private void OnRecvAudioStream(AsvAudioStreamPacket pkt)
    {
        if (pkt.Payload.TargetSystem != _identity.SystemId) return;
        if (pkt.Payload.TargetComponent != _identity.ComponentId) return;
        if (_devices.TryGetValue(pkt.FullId, out var device) == false) return;
        ((AudioDevice)device).OnInputAudioStream(pkt.Payload);
    }
    private void OnRecvDeviceOnline(AsvAudioOnlinePacket pkt)
    {
        if (_codec.Value == null)
        {
            return;
        }
        try
        {
            if (_devices.TryGetValue(pkt.FullId, out var item))
            {
                ((AudioDevice)item).Update(pkt.Payload);
            }
            else
            {
                var newItem = new AudioDevice(_codecFactory, _codec.Value.Value, pkt, SendAudioStream, InternalOnReceiveAudio, _core );
                _devices.Add(newItem.FullId,newItem);  
            }
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"Error on add device:{e.Message}");
        }
    }

    private void InternalOnReceiveAudio(IAudioDevice device, ReadOnlyMemory<byte> pcmRawAudioData)
    {
        OnReceiveAudio?.Invoke(device, pcmRawAudioData);
    }

    private Task SendAudioStream(Action<AsvAudioStreamPacket> packet, CancellationToken cancel)
    {
        var pkt = new AsvAudioStreamPacket
        {
            SystemId = _identity.SystemId,
            ComponentId = _identity.ComponentId,
            Sequence = _core.Sequence.GetNextSequenceNumber(),
        };
        packet(pkt);
        return _core.Connection.Send(pkt, cancel);   
    }

    private void RemoveOldDevice(object? state)
    {
        var itemsToDelete = _devices
            .Select(x => (AudioDevice)x.Value)
            .Where(device => _core.TimeProvider.GetElapsedTime(device.GetLastHit()) > _deviceTimeout)
            .ToImmutableArray();
        foreach (var item in itemsToDelete.RemoveRange(itemsToDelete))
        {
            item.Dispose();
        }

    }
    
    public IEnumerable<AsvAudioCodec> AvailableCodecs => _codecFactory.AvailableCodecs;
    
    public void GoOnline(string name, AsvAudioCodec codec, bool speakerEnabled, bool micEnabled)
    {
        AsvAudioHelper.CheckDeviceName(name);
        _transponder.Set(p =>
        {
            MavlinkTypesHelper.SetString(p.Name,name);
            p.Codec = codec;
            AsvAudioModeFlag mode = 0;
            if (speakerEnabled)
            {
                mode |= AsvAudioModeFlag.AsvAudioModeFlagSpeakerOn;
            }
            if (micEnabled)
            {
                mode |= AsvAudioModeFlag.AsvAudioModeFlagMicOn;
            }
            p.Mode = mode;
        });
        _isOnline.OnNext(true);
        _codec.OnNext(codec);
        _speakerEnabled.OnNext(speakerEnabled);
        _micEnabled.OnNext(micEnabled);
        _transponder.Start(_onlineRate, _onlineRate);
    }
    public void GoOffline()
    {
        _isOnline.OnNext(false);
        _transponder.Stop();
    }

    public ReadOnlyReactiveProperty<bool> IsOnline => _isOnline;
    public ReadOnlyReactiveProperty<AsvAudioCodec?> Codec => _codec;
    public ReactiveProperty<bool> SpeakerEnabled => _speakerEnabled;
    public ReactiveProperty<bool> MicEnabled => _micEnabled;
    public IReadOnlyObservableDictionary<MavlinkIdentity,IAudioDevice> Devices => _devices;
    public OnRecvAudioDelegate? OnReceiveAudio { get; set; }
  
    public void SendAll(ReadOnlyMemory<byte> pcmRawAudioData)
    {
        foreach (var item in _devices.Select(x=>x.Value))
        {
            item.SendAudio(pcmRawAudioData);
        }
    }

    #region Dispose

    public void Dispose()
    {
        _transponder.Dispose();
        _isOnline.Dispose();
        _codec.Dispose();
        _speakerEnabled.Dispose();
        _micEnabled.Dispose();
        _sub1.Dispose();
        _sub2.Dispose();
        _timer.Dispose();
        _sub3.Dispose();
        _sub4.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_transponder).ConfigureAwait(false);
        await CastAndDispose(_isOnline).ConfigureAwait(false);
        await CastAndDispose(_codec).ConfigureAwait(false);
        await CastAndDispose(_speakerEnabled).ConfigureAwait(false);
        await CastAndDispose(_micEnabled).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);
        await _timer.DisposeAsync().ConfigureAwait(false);
        await CastAndDispose(_sub3).ConfigureAwait(false);
        await CastAndDispose(_sub4).ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    #endregion
}