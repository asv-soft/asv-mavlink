#nullable enable
using System;
using System.Collections.Generic;
using Asv.Common;
using Asv.Mavlink.V2.AsvAudio;
using DynamicData;

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
    IRxValue<bool> IsOnline { get; }
    IRxValue<AsvAudioCodec?> Codec { get; }
    IRxEditableValue<bool> SpeakerEnabled { get; }
    IRxEditableValue<bool> MicEnabled { get; }
    IObservable<IChangeSet<IAudioDevice, ushort>> Devices { get; }
    OnRecvAudioDelegate OnReceiveAudio { get; set; }
    void SendAll(ReadOnlyMemory<byte> pcmRawAudioData);
}

public class AudioServiceConfig
{
    public int DeviceTimeoutMs { get; set; } = 10_000;
    public int OnlineRateMs { get; set; } = 1_000;
    public int RemoveDeviceCheckDelayMs { get; set; } = 3_000;
}