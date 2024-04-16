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
        float txPowerDbm, AsvAudioCodec codec, CancellationToken cancel = default);
    Task<MavResult> DisableRadio(CancellationToken cancel = default);
    
    Task<AsvRadioCapabilities> GetCapabilities(CancellationToken cancel = default);
    Task<ISet<AsvAudioCodec>> GetCodecsCapabilities(CancellationToken cancel = default);

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

    public async Task<MavResult> EnableRadio(uint frequencyHz, AsvRadioModulation modulation,float referenceRxPowerDbm,float txPowerDbm,  AsvAudioCodec codec, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await _commandClient.CommandLong(item => AsvRadioHelper.SetArgsForRadioOn(item, frequencyHz,modulation,referenceRxPowerDbm,txPowerDbm,codec),cs.Token).ConfigureAwait(false);
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
        var result = await Base.RequestCapabilities(cancel).ConfigureAwait(false);
        var builder = new AsvRadioCapabilitiesBuilder();
        builder.SetFrequencyHz(result.MinRfFreq,result.MaxRfFreq);
        builder.SetReferencePowerDbm(result.MinRxPower,result.MaxRxPower);
        builder.SetTxPowerDbm(result.MinTxPower,result.MaxTxPower);
        builder.SetSupportedModulations(AsvRadioHelper.GetModulation(result));
        var cap = builder.Build();
        _capabilities.OnNext(cap);
        return cap;
    }

    public async Task<ISet<AsvAudioCodec>> GetCodecsCapabilities(CancellationToken cancel = default)
    {
        const int count = 50;
        ushort skip = 0;
        ushort all;
        var result = new HashSet<AsvAudioCodec>();
        do
        {
            var next = await Base.RequestCodecCapabilities(skip, count, cancel).ConfigureAwait(false);
            result.UnionWith(next.Codecs);
            skip += next.Count;
            all = next.All;
        } while (result.Count < all);

        return result;
    }

    public IRxValue<AsvRadioCustomMode> CustomMode => _customMode;
    public IAsvRadioClient Base { get; }
}