#nullable enable
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
using NLog;

namespace Asv.Mavlink;

public delegate void OnRecvAudioDelegate(IAudioDevice device, byte[] data, int dataSize);

/// <summary>
/// This is not usual mavlink microservice.
/// There are no difference between client and server.
/// </summary>
public interface IAudioService
{
    IEnumerable<AsvAudioCodec> AvailableCodecs { get; }
    void GoOnline(string name, AsvAudioCodec codec, bool speakerEnabled, bool micEnabled);
    void GoOffline();
    IRxValue<bool> IsOnline { get; }
    IRxValue<AsvAudioCodec?> Codec { get; }
    IRxEditableValue<bool> SpeakerEnabled { get; }
    IRxEditableValue<bool> MicEnabled { get; }
    IObservable<IChangeSet<IAudioDevice, ushort>> Devices { get; }
    OnRecvAudioDelegate OnReceiveAudio { get; set; }
    Task SendAll(byte[] pcmRawAudioData, int dataSize,CancellationToken cancel = default);
}

public class AudioServiceConfig
{
    public int DeviceTimeoutMs { get; set; } = TimeSpan.FromSeconds(10).Milliseconds;
    public int OnlineRateMs { get; set; } = TimeSpan.FromSeconds(1).Milliseconds;
}

public class AudioService : DisposableOnceWithCancel, IAudioService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); 
    private readonly IAudioCodecFactory _codecFactory;
    private readonly IMavlinkV2Connection _connection;
    private readonly MavlinkIdentity _identity;
    private readonly IPacketSequenceCalculator _seq;
    private readonly SourceCache<AudioDevice,ushort> _deviceCache;
    private readonly MavlinkPacketTransponder<AsvAudioOnlinePacket,AsvAudioOnlinePayload> _transponder;
    private readonly RxValue<bool> _isOnline;
    private readonly RxValue<AsvAudioCodec?> _codec;
    private readonly RxValue<bool> _speakerEnabled;
    private readonly RxValue<bool> _micEnabled;
    private IAudioEncoder? _encoder;
    private readonly TimeSpan _deviceTimeout;
    private readonly TimeSpan _onlineRate;


    public AudioService(
        IAudioCodecFactory codecFactory,    
        IMavlinkV2Connection connection, 
        MavlinkIdentity identity, 
        IPacketSequenceCalculator seq,
        AudioServiceConfig config, 
        IScheduler? publishScheduler = null)
    {
        _codecFactory = codecFactory ?? throw new ArgumentNullException(nameof(codecFactory));
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _identity = identity;
        _seq = seq ?? throw new ArgumentNullException(nameof(seq));
        var config1 = config ?? throw new ArgumentNullException(nameof(config));
        _deviceTimeout = TimeSpan.FromMilliseconds(config1.DeviceTimeoutMs);
        _onlineRate = TimeSpan.FromMilliseconds(config1.OnlineRateMs);
        _deviceCache = new SourceCache<AudioDevice,ushort>(x => x.FullId).DisposeItWith(Disposable);
        Devices = _deviceCache.Connect().Transform(x=>(IAudioDevice)x);
        
        connection.Filter<AsvAudioOnlinePacket>().Where(_=>IsOnline.Value).Subscribe(OnRecvDeviceOnline).DisposeItWith(Disposable);
        connection.Filter<AsvAudioStreamPacket>().Where(_=>IsOnline.Value).Subscribe(OnRecvAudioStream).DisposeItWith(Disposable);
        
        Observable
            .Timer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3))
            .Subscribe(RemoveOldDevice)
            .DisposeItWith(Disposable);
        Devices = publishScheduler != null 
            ? _deviceCache.Connect().ObserveOn(publishScheduler).Transform(x => (IAudioDevice)x).RefCount() 
            : _deviceCache.Connect().Transform(x => (IAudioDevice)x).RefCount();
        
        _transponder = new MavlinkPacketTransponder<AsvAudioOnlinePacket,AsvAudioOnlinePayload>(connection, identity, seq)
            .DisposeItWith(Disposable);
        _isOnline = new RxValue<bool>(false).DisposeItWith(Disposable);
        _codec = new RxValue<AsvAudioCodec?>().DisposeItWith(Disposable);
        _speakerEnabled = new RxValue<bool>(false).DisposeItWith(Disposable);
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
        })).DisposeItWith(Disposable);
        _micEnabled = new RxValue<bool>(false).DisposeItWith(Disposable);
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
        })).DisposeItWith(Disposable);

        Disposable.AddAction(() =>
        {
            _encoder?.Dispose();
            _encoder = null;
        });
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

                    var newItem = new AudioDevice(_codecFactory, _codec.Value.Value, pkt, SendAudioStream,
                        InternalOnReceiveAudio);
                    update.AddOrUpdate(newItem);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        });
    }

    private void InternalOnReceiveAudio(IAudioDevice device, byte[] data, int dataSize)
    {
        OnReceiveAudio?.Invoke(device, data, dataSize);
    }

    private Task SendAudioStream(Action<AsvAudioStreamPacket> packet, CancellationToken cancel)
    {
           var pkt = new AsvAudioStreamPacket
           {
               SystemId = _identity.SystemId,
               ComponentId = _identity.ComponentId,
               Sequence = _seq.GetNextSequenceNumber(),
           };
           packet(pkt);
           return _connection.Send(pkt, cancel);   
    }

    private void RemoveOldDevice(long l)
    {
        _deviceCache.Edit(update =>
        {
            var now = DateTime.Now;
            var itemsToDelete = update.Items.Where(device => (now - device.GetLastHit()) > _deviceTimeout).ToList();
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
        _encoder?.Dispose();
        _encoder = _codecFactory.CreateEncoder(codec);
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
        _encoder?.Dispose();
        _encoder = null;
        _isOnline.OnNext(false);
        _transponder.Stop();
    }

    public IRxValue<bool> IsOnline => _isOnline;
    public IRxValue<AsvAudioCodec?> Codec => _codec;
    public IRxEditableValue<bool> SpeakerEnabled => _speakerEnabled;
    public IRxEditableValue<bool> MicEnabled => _micEnabled;
    public IObservable<IChangeSet<IAudioDevice, ushort>> Devices { get; }
    public OnRecvAudioDelegate OnReceiveAudio { get; set; }
    public Task SendAll(byte[] pcmRawAudioData, int dataSize, CancellationToken cancel = default)
    {
        return Task.WhenAll(_deviceCache.Items.Select(device => device.SendAudio(pcmRawAudioData, dataSize, cancel)));
    }

    
}