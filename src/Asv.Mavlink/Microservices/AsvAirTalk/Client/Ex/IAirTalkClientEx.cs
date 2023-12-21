using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvAirTalk;
using DynamicData;

namespace Asv.Mavlink;



public delegate void OnRecvPcmAudioDelegate(byte systemId, byte componentId, ReadOnlyMemory<byte> pcmRawAudioData);

public interface IAirTalkClientEx
{
    IAirTalkClient Base { get; }
    Task UpdateContacts(CancellationToken cancel = default);
    IObservable<IChangeSet<IAirTalkDevice, ushort>> Devices { get; }
    OnRecvPcmAudioDelegate OnReceiveAudio { get; set; }
}

public class AirTalkClientEx : DisposableOnceWithCancel, IAirTalkClientEx
{
    private readonly SourceCache<IAirTalkDevice,ushort> _clientSourceCache;

    public AirTalkClientEx(IAirTalkClient client, IEnumerable<Lazy<IAudioCodec,AsvAirTalkCodec>> codecs, AsvAirTalkCodec defaultEncoding = AsvAirTalkCodec.AsvAirTalkCodecOpus)
    {
        Base = client;
        _clientSourceCache = new SourceCache<IAirTalkDevice, ushort>(_ => _.FullId).DisposeItWith(Disposable);
    }

    public IAirTalkClient Base { get; }

    public Task UpdateContacts(CancellationToken cancel = default)
    {
        throw new NotImplementedException();
    }

    public IObservable<IChangeSet<IAirTalkDevice, ushort>> Devices => _clientSourceCache.Connect();
    
   
    
    public OnRecvPcmAudioDelegate OnReceiveAudio { get; set; }
}