using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvAudio;
using DynamicData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using R3;
using ZLogger;

namespace Asv.Mavlink;

public class AudioService : IAudioService,IDisposable
{
    private readonly ILogger _logger; 
    private readonly IAudioCodecFactory _codecFactory;
    private readonly MavlinkIdentity _identity;
    private readonly ICoreServices _core;
    private readonly SourceCache<AudioDevice,ushort> _deviceCache;
    private readonly MavlinkPacketTransponder<AsvAudioOnlinePacket,AsvAudioOnlinePayload> _transponder;
    private readonly RxValue<bool> _isOnline;
    private readonly RxValue<AsvAudioCodec?> _codec;
    private readonly RxValue<bool> _speakerEnabled;
    private readonly RxValue<bool> _micEnabled;
    
    private readonly TimeSpan _deviceTimeout;
    private readonly TimeSpan _onlineRate;
    private readonly IDisposable _disposeIt;


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

        var builder = Disposable.CreateBuilder();
        
        _deviceCache = new SourceCache<AudioDevice,ushort>(x => x.FullId).AddTo(ref builder);
        Devices = _deviceCache.Connect().Transform(x=>(IAudioDevice)x);
        core.Connection.Filter<AsvAudioOnlinePacket>().Where(_=>IsOnline.Value).Subscribe(OnRecvDeviceOnline).AddTo(ref builder);
        core.Connection.Filter<AsvAudioStreamPacket>().Where(_=>IsOnline.Value).Subscribe(OnRecvAudioStream).AddTo(ref builder);
        
        core.TimeProvider.CreateTimer(RemoveOldDevice, null, TimeSpan.FromMilliseconds(config.RemoveDeviceCheckDelayMs), TimeSpan.FromMilliseconds(config.RemoveDeviceCheckDelayMs)).AddTo(ref builder);
        Devices = _deviceCache.Connect().ObserveOn(core.Scheduler).Transform(x => (IAudioDevice)x).RefCount();
        
        _transponder = new MavlinkPacketTransponder<AsvAudioOnlinePacket,AsvAudioOnlinePayload>(identity,core)
            .AddTo(ref builder);
        _isOnline = new RxValue<bool>(false).AddTo(ref builder);
        _codec = new RxValue<AsvAudioCodec?>().AddTo(ref builder);
        _speakerEnabled = new RxValue<bool>(false).AddTo(ref builder);
        _speakerEnabled.Subscribe(speakerEnabled=>_transponder.Set(p=>
        {
            if (speakerEnabled)
            {
                p.Mode |= AsvAudioModeFlag.AsvAudioModeFlagSpeakerOn;
            }
            else
            {
                p.Mode &= ~AsvAudioModeFlag.AsvAudioModeFlagSpeakerOn;
            }
        })).AddTo(ref builder);
        _micEnabled = new RxValue<bool>(false).AddTo(ref builder);
        _micEnabled.Subscribe(micEnabled=>_transponder.Set(p=>
        {
            if (micEnabled)
            {
                p.Mode |= AsvAudioModeFlag.AsvAudioModeFlagMicOn;
            }
            else
            {
                p.Mode &= ~AsvAudioModeFlag.AsvAudioModeFlagMicOn;
            }
        })).AddTo(ref builder);

        _disposeIt = builder.Build();
    }

 


    private void OnRecvAudioStream(AsvAudioStreamPacket pkt)
    {
        if (pkt.Payload.TargetSystem != _identity.SystemId)
        {
            return;
        }
        if (pkt.Payload.TargetComponent != _identity.ComponentId)
        {
            return;
        }
        var device = _deviceCache.Lookup(pkt.FullId);
        if (device.HasValue == false) return;
        device.Value.OnInputAudioStream(pkt.Payload);
    }
    private void OnRecvDeviceOnline(AsvAudioOnlinePacket pkt)
    {
        if (_codec.Value == null)
        {
            return;
        }
        _deviceCache.Edit(update =>
        {
            try
            {
                var item = update.Lookup(pkt.FullId);
                if (item.HasValue)
                {
                    item.Value.Update(pkt.Payload);
                }
                else
                {

                    var newItem = new AudioDevice(_codecFactory, _codec.Value.Value, pkt, SendAudioStream, InternalOnReceiveAudio, _core );
                    update.AddOrUpdate(newItem);
                }
            }
            catch (Exception e)
            {
                _logger.ZLogError(e, $"Error on add device:{e.Message}");
            }
        });
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
        _deviceCache.Edit(update =>
        {
            var itemsToDelete = update.Items.Where(device => _core.TimeProvider.GetElapsedTime(device.GetLastHit()) > _deviceTimeout).ToList();
            foreach (var item in itemsToDelete)
            {
                item.Dispose();
            }
            update.RemoveKeys(itemsToDelete.Select(device=>device.FullId));
        });
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

    public IRxValue<bool> IsOnline => _isOnline;
    public IRxValue<AsvAudioCodec?> Codec => _codec;
    public IRxEditableValue<bool> SpeakerEnabled => _speakerEnabled;
    public IRxEditableValue<bool> MicEnabled => _micEnabled;
    public IObservable<IChangeSet<IAudioDevice, ushort>> Devices { get; }
    public OnRecvAudioDelegate OnReceiveAudio { get; set; }
  
    public void SendAll(ReadOnlyMemory<byte> pcmRawAudioData)
    {
        foreach (var item in _deviceCache.Items)
        {
            item.SendAudio(pcmRawAudioData);
        }
    }

    public void Dispose()
    {
        _disposeIt.Dispose();
    }
}