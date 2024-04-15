using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvAudio;
using Asv.Mavlink.V2.AsvRadio;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using NLog;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;


public delegate Task<MavResult> EnableRadioDelegate(uint frequencyHz, AsvRadioModulation modulation,
    float referenceRxPowerDbm, float txPowerDbm, AsvAudioCodec codec, byte codecConfig, CancellationToken cancel);

public delegate Task<MavResult> DisableRadioDelegate(CancellationToken cancel);

public interface IAsvRadioServerEx
{
    IAsvRadioServer Base { get; }
    IRxEditableValue<AsvRadioCustomMode> CustomMode { get; }
    EnableRadioDelegate EnableRadio { set; }
    DisableRadioDelegate DisableRadio { set; }

    void Start();
}



public class AsvRadioServerEx: DisposableOnceWithCancel, IAsvRadioServerEx
{
    private readonly AsvRadioCapabilities _capabilities;
    private readonly IHeartbeatServer _heartbeat;
    private readonly IStatusTextServer _statusText;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private int _capRequestInProgress;

    public AsvRadioServerEx(AsvRadioCapabilities capabilities, IAsvRadioServer server, IHeartbeatServer heartbeat, ICommandServerEx<CommandLongPacket> commands, IStatusTextServer statusText)
    {
        if (commands == null) throw new ArgumentNullException(nameof(commands));
        _capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));
        _heartbeat = heartbeat ?? throw new ArgumentNullException(nameof(heartbeat));
        _statusText = statusText ?? throw new ArgumentNullException(nameof(statusText));
        Base = server ?? throw new ArgumentNullException(nameof(server));
        
        heartbeat.Set(x =>
        {
            x.Autopilot = MavAutopilot.MavAutopilotInvalid;
            x.Type = (V2.Minimal.MavType)V2.AsvRadio.MavType.MavTypeAsvRadio;
            x.SystemStatus = MavState.MavStateActive;
            x.BaseMode = MavModeFlag.MavModeFlagCustomModeEnabled;
            x.MavlinkVersion = 3;
            x.CustomMode = (uint)AsvRadioCustomMode.AsvRadioCustomModeIdle;
        });
        CustomMode = new RxValue<AsvRadioCustomMode>().DisposeItWith(Disposable);
        CustomMode.DistinctUntilChanged().Subscribe(mode => heartbeat.Set(_ =>
        {
            _.CustomMode = (uint)mode;
        })).DisposeItWith(Disposable);
        
        commands[(MavCmd)V2.AsvRadio.MavCmd.MavCmdAsvRadioOn] = async (id,args, cancel) =>
        {
            if (EnableRadio == null) return new CommandResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            AsvRadioHelper.GetArgsForRadioOn(args.Payload, out var freq, out var mode, out var referencePower, out var txPower, out var codec, out var codecConfig);
            if (freq > capabilities.MaxFrequencyHz || freq < capabilities.MinFrequencyHz)
            {
                Logger.Warn($"Frequency {freq} out of range {capabilities.MinFrequencyHz}-{capabilities.MaxFrequencyHz}");
                _statusText.Error("Frequency out of range");
                return new CommandResult(MavResult.MavResultFailed);
            }

            if (referencePower > capabilities.MaxReferencePowerDbm || referencePower < capabilities.MinReferencePowerDbm)
            {
                Logger.Warn($"Reference power {referencePower} out of range {capabilities.MinReferencePowerDbm}-{capabilities.MaxReferencePowerDbm}");
                _statusText.Error("Reference power out of range");
                return new CommandResult(MavResult.MavResultFailed);
            }
            if (capabilities.SupportedModulations.Contains(mode) == false)
            {
                Logger.Warn($"Modulation {mode} not supported. Available: {string.Join(",", capabilities.SupportedModulations)}");
                _statusText.Error("Modulation not supported");
            }

            if (capabilities.SupportedCodecs.ContainsKey(codec) == false)
            {
                Logger.Warn($"Codec {codec} not supported. Available: {string.Join(",", capabilities.SupportedCodecs.Keys)}");
                _statusText.Error("Codec not supported");
            }
            
            if (capabilities.SupportedCodecs[codec].Contains(codecConfig) == false)
            {
                Logger.Warn($"Codec {codec} config {codecConfig} not supported. Available: {string.Join(",", capabilities.SupportedCodecs[codec])}");
                _statusText.Error("Codec config not supported");
            }
            
            var result = await EnableRadio(freq, mode, referencePower, txPower, codec, codecConfig, cs.Token).ConfigureAwait(false);
            return new CommandResult(result);
        };
        commands[(MavCmd)V2.AsvRadio.MavCmd.MavCmdAsvRadioOff] = async (id,args, cancel) =>
        {
            if (DisableRadio == null) return new CommandResult(MavResult.MavResultUnsupported);
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            AsvRadioHelper.GetArgsForRadioOff(args.Payload);
            var result = await DisableRadio(cs.Token).ConfigureAwait(false);
            return new CommandResult(result);
        };

        Base.OnCapabilitiesRequest.Subscribe(OnCapabilitiesRequest).DisposeItWith(Disposable);
        Base.OnCodecCfgRequest.Subscribe(OnCodecCfgRequest).DisposeItWith(Disposable);
    }

    private async void OnCodecCfgRequest(AsvRadioCodecCfgRequestPayload request)
    {
        try
        {
            if (_capabilities.SupportedCodecs.TryGetValue(request.TargetCodec, out var options) == false)
            {
                Logger.Warn($"Codec {request.TargetCodec} not supported");
                // send empty response
                await Base.SendCodecCfgResponse(x =>
                {
                    x.TargetCodec = request.TargetCodec;
                },DisposeCancel).ConfigureAwait(false);
                return;
            }

            await Base.SendCodecCfgResponse(x =>
            {
                x.TargetCodec = request.TargetCodec;
                AsvRadioHelper.SetCodecsOptions(x, options);
            }, DisposeCancel).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }

    private async void OnCapabilitiesRequest(AsvRadioCapabilitiesRequestPayload request)
    {
        try
        {
            if (Interlocked.CompareExchange(ref _capRequestInProgress, 1, 0) != 0)
            {
                Logger.Warn($"Duplicate request capabilities");
                return;
            }

            await Base.SendCapabilitiesResponse(x =>
            {
                AsvRadioHelper.SetModulation(x, _capabilities.SupportedModulations);
                AsvRadioHelper.SetCodecs(x, _capabilities.SupportedCodecs.Keys);
                x.MinRfFreq = _capabilities.MinFrequencyHz;
                x.MaxRfFreq = _capabilities.MaxFrequencyHz;
                x.MinRxPower = _capabilities.MinReferencePowerDbm;
                x.MaxRxPower = _capabilities.MaxReferencePowerDbm;
                x.MinTxPower = _capabilities.MinTxPowerDbm;
                x.MaxTxPower = _capabilities.MaxTxPowerDbm;
                
            }, DisposeCancel).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        finally
        {
            Interlocked.Exchange(ref _capRequestInProgress, 0);
        }
    }

    public IRxEditableValue<AsvRadioCustomMode> CustomMode { get; }
    public IAsvRadioServer Base { get; }
    public EnableRadioDelegate EnableRadio { get; set; }
    public DisableRadioDelegate DisableRadio { get; set; }
    public void Start()
    {
        Base.Start();
        _heartbeat.Start();
    }
}