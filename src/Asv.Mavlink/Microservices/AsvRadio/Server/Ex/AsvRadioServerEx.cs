using System;
using System.Collections.Generic;
using System.Linq;
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
    float referenceRxPowerDbm, float txPowerDbm, AsvAudioCodec codec, CancellationToken cancel);

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
    private readonly IReadOnlySet<AsvAudioCodec> _codecs;
    private readonly IHeartbeatServer _heartbeat;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private int _capRequestInProgress;

    public AsvRadioServerEx(AsvRadioCapabilities capabilities, IReadOnlySet<AsvAudioCodec> codecs, IAsvRadioServer server, IHeartbeatServer heartbeat, ICommandServerEx<CommandLongPacket> commands, IStatusTextServer statusText)
    {
        if (commands == null) throw new ArgumentNullException(nameof(commands));
        _capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));
        _codecs = codecs ?? throw new ArgumentNullException(nameof(codecs));
        _heartbeat = heartbeat ?? throw new ArgumentNullException(nameof(heartbeat));
        var statusText1 = statusText ?? throw new ArgumentNullException(nameof(statusText));
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
            AsvRadioHelper.GetArgsForRadioOn(args.Payload, out var freq, out var mode, out var referencePower, out var txPower, out var codec);
            if (freq > capabilities.MaxFrequencyHz || freq < capabilities.MinFrequencyHz)
            {
                Logger.Warn($"Frequency {freq} out of range {capabilities.MinFrequencyHz}-{capabilities.MaxFrequencyHz}");
                statusText1.Error("Frequency out of range");
                return new CommandResult(MavResult.MavResultFailed);
            }

            if (referencePower > capabilities.MaxReferencePowerDbm || referencePower < capabilities.MinReferencePowerDbm)
            {
                Logger.Warn($"Reference power {referencePower} out of range {capabilities.MinReferencePowerDbm}-{capabilities.MaxReferencePowerDbm}");
                statusText1.Error("Reference power out of range");
                return new CommandResult(MavResult.MavResultFailed);
            }
            if (capabilities.SupportedModulations.Contains(mode) == false)
            {
                Logger.Warn($"Modulation {mode} not supported. Available: {string.Join(",", capabilities.SupportedModulations)}");
                statusText1.Error("Modulation not supported");
            }

            if (_codecs.Contains(codec) == false)
            {
                Logger.Warn($"Codec {codec} not supported. Available: {string.Join(",", _codecs)}");
                statusText1.Error("Codec not supported");
            }
            
            var result = await EnableRadio(freq, mode, referencePower, txPower, codec, cs.Token).ConfigureAwait(false);
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
        Base.OnCodecCapabilitiesRequest.Subscribe(OnCodecCapabilitiesRequest).DisposeItWith(Disposable);
    }

    private async void OnCodecCapabilitiesRequest(AsvRadioCodecCapabilitiesRequestPayload request)
    {
        try
        {
            var count = Math.Min((byte)request.Count, (byte)AsvRadioCodecCapabilitiesResponsePayload.CodecsMaxItemsCount);
            var items = _codecs.Skip(request.Skip).Take(count);
            await Base.SendCodecCapabilitiesRequest(x =>
            {
                x.Skip = request.Skip;
                x.All = (byte)_codecs.Count;
                foreach (var item in items)
                {
                    x.Codecs[x.Count] = item;
                    ++x.Count;
                }
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