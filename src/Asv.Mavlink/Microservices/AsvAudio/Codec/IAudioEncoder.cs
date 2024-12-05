using System;
using Asv.Mavlink.AsvAudio;

using R3;

namespace Asv.Mavlink;

public interface IAudioEncoder: IDisposable, IAsyncDisposable
{   
    Observable<ReadOnlyMemory<byte>> Output { get; }
    AsvAudioCodec Codec { get; }
    
}