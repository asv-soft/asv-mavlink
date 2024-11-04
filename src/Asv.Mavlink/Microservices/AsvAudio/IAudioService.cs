#nullable enable
using System;
using System.Collections.Generic;
using Asv.Common;
using Asv.Mavlink.V2.AsvAudio;
using ObservableCollections;
using R3;

namespace Asv.Mavlink;

public delegate void OnRecvAudioDelegate(IAudioDevice device, ReadOnlyMemory<byte> pcmRawAudioData);

/// <summary>
/// This is not usual mavlink microservice.
/// There are no difference between client and server.
/// </summary>
public interface IAudioService
{
    IEnumerable<AsvAudioCodec> AvailableCodecs { get; }
    void GoOnline(string name, AsvAudioCodec codec, bool speakerEnabled, bool micEnabled);
    void GoOffline();
    ReadOnlyReactiveProperty<bool> IsOnline { get; }
    ReadOnlyReactiveProperty<AsvAudioCodec?> Codec { get; }
    ReactiveProperty<bool> SpeakerEnabled { get; }
    ReactiveProperty<bool> MicEnabled { get; }
    IReadOnlyObservableDictionary<MavlinkIdentity,IAudioDevice> Devices { get; }
    OnRecvAudioDelegate? OnReceiveAudio { get; set; }
    void SendAll(ReadOnlyMemory<byte> pcmRawAudioData);
}

