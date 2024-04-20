#nullable enable
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Common;
using NLog;

namespace Asv.Mavlink;


public class RadioClientDeviceConfig:ClientDeviceConfig
{
    public CommandProtocolConfig Command { get; set; } = new();
    public ParamsClientExConfig Params { get; set; } = new();
    public string SerialNumberParamName { get; set; } = "BRD_SERIAL_NUM";
}

public class RadioClientDevice : ClientDevice, IRadioClientDevice
{
    private readonly RadioClientDeviceConfig _config;
    private readonly ParamsClientEx _params;
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public RadioClientDevice(IAudioCodecFactory factory, IMavlinkV2Connection connection, MavlinkClientIdentity identity, IPacketSequenceCalculator seq,RadioClientDeviceConfig config, IScheduler? scheduler = null)
        : base(connection, identity, config, seq, scheduler)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));

        Command = new CommandClient(connection, identity, seq, config.Command).DisposeItWith(Disposable);
        var client = new AsvRadioClient(connection, identity,seq).DisposeItWith(Disposable);
        Radio = new AsvRadioClientEx(client, Heartbeat, Command).DisposeItWith(Disposable);
        var param = new ParamsClient(connection, identity, seq, config.Params).DisposeItWith(Disposable);
        _params = new ParamsClientEx(param, config.Params).DisposeItWith(Disposable);
    }
    
    public IAsvRadioClientEx Radio { get; }
    
    protected override string DefaultName => $"RADIO [{Identity.TargetSystemId:00},{Identity.TargetComponentId:00}]";
    protected override async Task InternalInit()
    {
        _params.Init(MavParamHelper.ByteWiseEncoding, ArraySegment<ParamDescription>.Empty);
        try
        {
            _logger.Trace($"Try to read serial number from param {_config.SerialNumberParamName}");
            Params.Filter(_config.SerialNumberParamName)
                .Select(serial => $"RADIO [{(int)serial:D5}]")
                .Subscribe(EditableName)
                .DisposeItWith(Disposable);
            await Params.ReadOnce(_config.SerialNumberParamName, DisposeCancel).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.Warn($"Error to get RADIO serial number:{e.Message}");
        }
    }

    public override DeviceClass Class => DeviceClass.Radio;
    public ICommandClient Command { get; }
    public IParamsClientEx Params => _params;
}