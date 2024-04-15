#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvAudio;
using Asv.Mavlink.V2.AsvRadio;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

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
        
        var cap = await Base.RequestCapabilities(cancel).ConfigureAwait(false);
        var builder = new AsvRadioCapabilitiesBuilder();
        builder.SetFrequencyHz(cap.MinRfFreq,cap.MaxRfFreq);
        builder.SetReferencePowerDbm(cap.MinRxPower,cap.MaxRxPower);
        builder.SetTxPowerDbm(cap.MinTxPower,cap.MaxTxPower);
        builder.SetSupportedModulations(AsvRadioHelper.GetModulation(cap));
        var codecs = AsvRadioHelper.GetCodecs(cap).ToImmutableArray();

        foreach (var codec in codecs)
        {
            var response = await Base.RequestCodecOptions(codec,cancel).ConfigureAwait(false);
            var options = AsvRadioHelper.GetCodecsOptions(response);
            builder.SetSupportedCodecs(codec,options);
        }
        return builder.Build();
       
    }

    public IRxValue<AsvRadioCustomMode> CustomMode => _customMode;
    public IAsvRadioClient Base { get; }
}