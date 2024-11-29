using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.AsvRadio;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Microsoft.Extensions.Logging;
using R3;
using ZLogger;
using MavCmd = Asv.Mavlink.Common.MavCmd;

namespace Asv.Mavlink;




public class AsvRadioServerEx: IAsvRadioServerEx, IDisposable,IAsyncDisposable
{
    private readonly AsvRadioCapabilities _capabilities;
    private readonly IReadOnlySet<AsvAudioCodec> _codecs;
    private readonly IHeartbeatServer _heartbeat;
    private readonly ILogger _logger;
    private int _capRequestInProgress;
    private readonly ReactiveProperty<AsvRadioCustomMode> _customMode;
    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;

    public AsvRadioServerEx(
        AsvRadioCapabilities capabilities, 
        IReadOnlySet<AsvAudioCodec> codecs, 
        IAsvRadioServer server, 
        IHeartbeatServer heartbeat, 
        ICommandServerEx<CommandLongPacket> commands, 
        IStatusTextServer statusText)
    {
        _logger = server.Core.Log.CreateLogger<AsvRadioServerEx>();
        ArgumentNullException.ThrowIfNull(commands);
        _capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));
        _codecs = codecs ?? throw new ArgumentNullException(nameof(codecs));
        _heartbeat = heartbeat ?? throw new ArgumentNullException(nameof(heartbeat));
        var statusText1 = statusText ?? throw new ArgumentNullException(nameof(statusText));
        Base = server ?? throw new ArgumentNullException(nameof(server));
        
        heartbeat.Set(x =>
        {
            x.Autopilot = MavAutopilot.MavAutopilotInvalid;
            x.Type = (Minimal.MavType)AsvRadio.MavType.MavTypeAsvRadio;
            x.SystemStatus = MavState.MavStateActive;
            x.BaseMode = MavModeFlag.MavModeFlagCustomModeEnabled;
            x.MavlinkVersion = 3;
            x.CustomMode = (uint)AsvRadioCustomMode.AsvRadioCustomModeIdle;
        });
        
        _customMode = new ReactiveProperty<AsvRadioCustomMode>();
        CustomMode.Subscribe(mode => heartbeat.Set(p => p.CustomMode = (uint)mode));
        
        commands[(MavCmd)AsvRadio.MavCmd.MavCmdAsvRadioOn] = async (id,args, cancel) =>
        {
            if (EnableRadio == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            AsvRadioHelper.GetArgsForRadioOn(args.Payload, out var freq, out var mode, out var referencePower, out var txPower, out var codec);
            if (freq > capabilities.MaxFrequencyHz || freq < capabilities.MinFrequencyHz)
            {
                _logger.ZLogWarning($"Frequency {freq} out of range {capabilities.MinFrequencyHz}-{capabilities.MaxFrequencyHz}");
                statusText1.Error("Frequency out of range");
                return CommandResult.FromResult(MavResult.MavResultFailed);
            }

            if (referencePower > capabilities.MaxReferencePowerDbm || referencePower < capabilities.MinReferencePowerDbm)
            {
                _logger.ZLogWarning($"Reference power {referencePower} out of range {capabilities.MinReferencePowerDbm}-{capabilities.MaxReferencePowerDbm}");
                statusText1.Error("Reference power out of range");
                return CommandResult.FromResult(MavResult.MavResultFailed);
            }
            if (capabilities.SupportedModulations.Contains(mode) == false)
            {
                _logger.ZLogWarning($"Modulation {mode} not supported. Available: {string.Join(",", capabilities.SupportedModulations)}");
                statusText1.Error("Modulation not supported");
            }

            if (_codecs.Contains(codec) == false)
            {
                _logger.ZLogWarning($"Codec {codec} not supported. Available: {string.Join(",", _codecs)}");
                statusText1.Error("Codec not supported");
            }
            
            var result = await EnableRadio(freq, mode, referencePower, txPower, codec, cancel).ConfigureAwait(false);
            _customMode.Value = AsvRadioCustomMode.AsvRadioCustomModeOnair;
            return CommandResult.FromResult(result);
        };
        commands[(MavCmd)AsvRadio.MavCmd.MavCmdAsvRadioOff] = async (id,args, cancel) =>
        {
            if (DisableRadio == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            AsvRadioHelper.GetArgsForRadioOff(args.Payload);
            var result = await DisableRadio(cancel).ConfigureAwait(false);
            _customMode.Value = AsvRadioCustomMode.AsvRadioCustomModeIdle;
            return CommandResult.FromResult(result);
        };

        _sub1 = Base.OnCapabilitiesRequest.Subscribe(OnCapabilitiesRequest);
        _sub2 = Base.OnCodecCapabilitiesRequest.Subscribe(OnCodecCapabilitiesRequest);
    }

    private async void OnCodecCapabilitiesRequest(AsvRadioCodecCapabilitiesRequestPayload? request)
    {
        if (request == null) return;
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
            }).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"Error on request codec capabilities:{e.Message}");
        }
    }

    private async void OnCapabilitiesRequest(AsvRadioCapabilitiesRequestPayload? request)
    {
        try
        {
            if (Interlocked.CompareExchange(ref _capRequestInProgress, 1, 0) != 0)
            {
                _logger.ZLogWarning($"Duplicate request capabilities");
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
                
            }).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"Error on request capabilities:{e.Message}");
        }
        finally
        {
            Interlocked.Exchange(ref _capRequestInProgress, 0);
        }
    }

    public ReadOnlyReactiveProperty<AsvRadioCustomMode> CustomMode => _customMode;

    public IAsvRadioServer Base { get; }
    public EnableRadioDelegate? EnableRadio { get; set; }
    public DisableRadioDelegate? DisableRadio { get; set; }
    public void Start()
    {
        Base.Start();
        _heartbeat.Start();
    }

    #region Dispose

    public void Dispose()
    {
        _customMode.Dispose();
        _sub1.Dispose();
        _sub2.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_customMode).ConfigureAwait(false);
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);

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