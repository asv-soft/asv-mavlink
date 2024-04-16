using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.V2.AsvAudio;
using Asv.Mavlink.V2.AsvRadio;
using Asv.Mavlink.V2.Common;
using DynamicData;
using NLog;

namespace Asv.Mavlink;


public class RadioServerDeviceConfig:ServerDeviceConfig
{
    public AudioServiceConfig Audio { get; set; } = new();
    public ParamsServerExConfig Params { get; set; } = new();
    public AsvRadioServerConfig Radio { get; set; } = new();
}

public class RadioServerDevice : ServerDevice, IRadioServerDevice
{
    private const string DefaultName = "Radio"; 
    private readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private int _busy;

    public RadioServerDevice(IMavlinkV2Connection connection,
        IPacketSequenceCalculator seq, MavlinkIdentity identity, RadioServerDeviceConfig config, IScheduler scheduler,
        IEnumerable<IMavParamTypeMetadata> paramList,
        IMavParamEncoding encoding,
        IConfiguration paramStore, IAudioCodecFactory factory, AsvRadioCapabilities capabilities)
        : base(connection, seq, identity, config, scheduler)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));
        
        var cmd = new CommandServer(connection, seq, identity, scheduler).DisposeItWith(Disposable);
        CommandLongEx = new CommandLongServerEx(cmd).DisposeItWith(Disposable);
        Audio = new AudioService(factory, connection, identity, seq, config.Audio, scheduler).DisposeItWith(Disposable);
        var paramsBase = new ParamsServer(connection, seq, identity, scheduler).DisposeItWith(Disposable);
        Params = new ParamsServerEx(paramsBase,StatusText,paramList,encoding,paramStore,config.Params).DisposeItWith(Disposable);
        var radio = new AsvRadioServer(connection, identity, config.Radio, seq,scheduler).DisposeItWith(Disposable);
        Radio = new AsvRadioServerEx(capabilities, factory.AvailableCodecs.ToImmutableHashSet(), radio, Heartbeat,CommandLongEx,StatusText).DisposeItWith(Disposable);
        
        AudioName = new RxValue<string>(DefaultName);
        
        Radio.EnableRadio = EnableRadio;
        Radio.DisableRadio = DisableRadio;

    }

    private async Task<MavResult> DisableRadio(CancellationToken cancel)
    {
        if (Interlocked.CompareExchange(ref _busy, 1, 0) != 0)
        {
            Logger.Warn("Duplicate enable radio request");
            return MavResult.MavResultInProgress;
        }
        try
        {
            Logger.Info("Disable radio");
            Radio.CustomMode.OnNext(AsvRadioCustomMode.AsvRadioCustomModeIdle);
            Audio.GoOffline();
            return MavResult.MavResultAccepted;
        }
        catch (Exception e)
        {
            Logger.Error(e,"Error to disable radio:{0}",e.Message);
            StatusText.Error("Error to disable radio");
            Radio.CustomMode.OnNext(AsvRadioCustomMode.AsvRadioCustomModeIdle);
            return MavResult.MavResultFailed;
        }
        finally
        {
            Interlocked.Exchange(ref _busy, 0);
        }
    }

    private async Task<MavResult> EnableRadio(uint frequencyhz, AsvRadioModulation modulation, float referencerxpowerdbm, float txpowerdbm, AsvAudioCodec codec,  CancellationToken cancel)
    {
        if (Interlocked.CompareExchange(ref _busy, 1, 0) != 0)
        {
            Logger.Warn("Duplicate enable radio request");
            return MavResult.MavResultInProgress;
        }
        try
        {
            Logger.Info($"Enable radio {frequencyhz}Hz {modulation:G} {referencerxpowerdbm:F2}dBm {txpowerdbm:F2}dBm {AsvAudioHelper.GetConfigName(codec)}]");
            
            Radio.CustomMode.OnNext(AsvRadioCustomMode.AsvRadioCustomModeOnair);
            if (Audio.AvailableCodecs.Contains(codec) == false)
            {
                Logger.Error("Codec not found");
                StatusText.Error("Codec not found");
                return MavResult.MavResultFailed;
            }
            Audio.GoOnline(AudioName.Value, codec, true, true);
            return MavResult.MavResultAccepted;
        }
        catch (Exception e)
        {
            Radio.CustomMode.OnNext(AsvRadioCustomMode.AsvRadioCustomModeIdle);
            Logger.Error(e,"Error to enable radio:{0}",e.Message);
            StatusText.Error("Error to enable radio");
            return MavResult.MavResultFailed;
        }
        finally
        {
            Interlocked.Exchange(ref _busy, 0);
        }
        
    }

    public IRxEditableValue<string> AudioName { get; }
    
    

    public override void Start()
    {
        base.Start();
        Radio.Start();
    }
    
    public ICommandServerEx<CommandLongPacket> CommandLongEx { get; }
    public IParamsServerEx Params { get; }
    public IAudioService Audio { get; }
    public IAsvRadioServerEx Radio { get; }
}