#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvAudio;
using Asv.Mavlink.V2.AsvRadio;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Client;

public interface IAsvRadioClientEx
{
    IRxValue<AsvRadioCustomMode> CustomMode { get; }
    IAsvRadioClient Base { get; }

    IRxValue<AsvRadioCapabilities?> Capabilities { get; }
    
    Task<MavResult> EnableRadio(uint frequencyHz, AsvRadioModulation modulation, float referenceRxPowerDbm,
        float txPowerDbm, AsvAudioCodec codec, byte codecConfig, CancellationToken cancel = default);
    Task<MavResult> DisableRadio(CancellationToken cancel = default);
    
    Task<AsvRadioCapabilities> GetCapabilities(CancellationToken cancel = default);

}

public class AsvRadioClientEx:DisposableOnceWithCancel,IAsvRadioClientEx
{
    private readonly ICommandClient _commandClient;
    private readonly RxValue<AsvRadioCustomMode> _customMode;
    private readonly RxValue<AsvRadioCapabilities?> _capabilities;

    public AsvRadioClientEx(IAsvRadioClient client, IHeartbeatClient heartbeatClient, ICommandClient commandClient)
    {
        _commandClient = commandClient ?? throw new ArgumentNullException(nameof(commandClient));
        Base = client ?? throw new ArgumentNullException(nameof(client));
        _customMode = new RxValue<AsvRadioCustomMode>().DisposeItWith(Disposable);;
        heartbeatClient.RawHeartbeat
            .Select(x => (AsvRadioCustomMode)x.CustomMode)
            .Subscribe(_customMode)
            .DisposeItWith(Disposable);
        _capabilities = new RxValue<AsvRadioCapabilities?>(default).DisposeItWith(Disposable);
    }

    public IRxValue<AsvRadioCapabilities?> Capabilities => _capabilities;

    public async Task<MavResult> EnableRadio(uint frequencyHz, AsvRadioModulation modulation,float referenceRxPowerDbm,float txPowerDbm,  AsvAudioCodec codec, byte codecConfig,  CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(item => AsvRadioHelper.SetArgsForRadioOn(item, frequencyHz,modulation,referenceRxPowerDbm,txPowerDbm,codec,codecConfig),cs.Token).ConfigureAwait(false);
        return result.Result;
    }
    public async Task<MavResult> DisableRadio( CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(AsvRadioHelper.SetArgsForRadioOff,cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<AsvRadioCapabilities> GetCapabilities(CancellationToken cancel = default)
    {
        var result = new AsvRadioCapabilities();
        var cap = await Base.RequestCapabilities(cancel).ConfigureAwait(false);
        result.MaxTxPowerDbm = cap.MaxTxPower;
        result.MinTxPowerDbm = cap.MinTxPower;
        result.MinFrequencyHz = cap.MinRfFreq;
        result.MaxFrequencyHz = cap.MaxRfFreq;
        result.MaxReferencePowerDbm = cap.MaxRxPower;
        result.MinReferencePowerDbm = cap.MinRxPower;
        result.SupportedModulations = AsvRadioHelper.GetModulation(cap).ToImmutableHashSet();
        var codecs = AsvRadioHelper.GetCodecs(cap).ToImmutableArray();

        var list = new Dictionary<AsvAudioCodec,AsvRadioCodecCapabilities>(codecs.Length);
        foreach (var codec in codecs)
        {
            var response = await Base.RequestCodecCfg(codec,cancel).ConfigureAwait(false);
            var config = AsvRadioHelper.GetCodecsOptions(response);
            list.Add(codec,new AsvRadioCodecCapabilities()
             {
                 Codec = codec,
                 SupportedOptions = config.ToImmutableHashSet()
             });
        }

        result.SupportedCodecs = list;
        return result;
       
    }

    public IRxValue<AsvRadioCustomMode> CustomMode => _customMode;
    public IAsvRadioClient Base { get; }
}