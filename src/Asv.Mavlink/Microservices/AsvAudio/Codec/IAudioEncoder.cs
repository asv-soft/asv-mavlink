using System;
using Asv.Mavlink.V2.AsvAudio;

namespace Asv.Mavlink;

public interface IAudioEncoder: IObservable<ReadOnlyMemory<byte>>, IDisposable
{   
    AsvAudioCodec Codec { get; }
    
}