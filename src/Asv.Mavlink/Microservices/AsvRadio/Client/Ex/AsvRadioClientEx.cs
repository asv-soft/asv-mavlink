#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.AsvRadio;
using Asv.Mavlink.Common;
using R3;

namespace Asv.Mavlink;

public class AsvRadioClientEx:MavlinkMicroserviceClient, IAsvRadioClientEx, IDisposable, IAsyncDisposable
{
    private readonly ICommandClient _commandClient;
    private readonly ReactiveProperty<AsvRadioCapabilities?> _capabilities;

    public AsvRadioClientEx(
        IAsvRadioClient client, 
        IHeartbeatClient heartbeatClient, 
        ICommandClient commandClient) : base(AsvRadioHelper.MicroserviceExName,client.Identity, client.Core)
    {
        _commandClient = commandClient ?? throw new ArgumentNullException(nameof(commandClient));
        Base = client ?? throw new ArgumentNullException(nameof(client));
        CustomMode = heartbeatClient.RawHeartbeat
            .Select(x => (AsvRadioCustomMode)(x?.CustomMode ?? 0))
            .ToReadOnlyReactiveProperty();
        _capabilities = new ReactiveProperty<AsvRadioCapabilities?>(null);
    }
    public ReadOnlyReactiveProperty<AsvRadioCapabilities?> Capabilities => _capabilities;

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
        _capabilities.Value = cap;
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

    public ReadOnlyReactiveProperty<AsvRadioCustomMode> CustomMode { get; }

    public IAsvRadioClient Base { get; }
   
    #region Dispose

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _capabilities.Dispose();
            CustomMode.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_capabilities).ConfigureAwait(false);
        await CastAndDispose(CustomMode).ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    #endregion
}