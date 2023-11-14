#nullable enable
using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using NLog;

namespace Asv.Mavlink;

public class SdrClientDeviceConfig:ClientDeviceConfig
{
    public CommandProtocolConfig Command { get; set; } = new();
    public AsvSdrClientExConfig SdrEx { get; set; } = new();
    public MissionClientConfig Missions { get; set; } = new();
    public MissionClientExConfig MissionsEx { get; set; } = new();
    public ParameterClientConfig Params { get; set; } = new();
    public ParamsClientExConfig ParamsEx { get; set; } = new();
    public string SerialNumberParamName { get; set; } = "BRD_SERIAL_NUM";
}
public class SdrClientDevice : ClientDevice, ISdrClientDevice
{
    private readonly SdrClientDeviceConfig _config;
    private readonly ParamsClientEx _params;
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public SdrClientDevice(IMavlinkV2Connection connection, MavlinkClientIdentity identity, SdrClientDeviceConfig config, IPacketSequenceCalculator seq, IScheduler? scheduler = null) : base(connection, identity, config, seq, scheduler)
    {
        _config = config;
        Command = new CommandClient(connection, identity, seq, config.Command).DisposeItWith(Disposable);
        Sdr = new AsvSdrClientEx(new AsvSdrClient(connection, identity, seq), Heartbeat, Command,config.SdrEx).DisposeItWith(Disposable);
        Missions = new MissionClientEx(new MissionClient(connection, identity, seq,config.Missions), config.MissionsEx).DisposeItWith(Disposable);
        _params = new ParamsClientEx(new ParamsClient(connection, identity, seq,config.Params), config.ParamsEx).DisposeItWith(Disposable);
    }
    protected override Task InternalInit()
    {
        _params.Init(MavParamHelper.ByteWiseEncoding, ArraySegment<ParamDescription>.Empty);
        return Task.CompletedTask;
    }

    protected override async Task<string> GetCustomName(CancellationToken cancel)
    {
        try
        {
            if (_config.SerialNumberParamName.IsNullOrWhiteSpace() == false)
            {
                _logger.Trace($"Try to read serial number from param {_config.SerialNumberParamName}");
                var serialNumber  = (int)await Params.ReadOnce(_config.SerialNumberParamName, cancel).ConfigureAwait(false);
                return $"SDR [{serialNumber:D5}]";    
            }
        }
        catch (Exception e)
        {
            _logger.Warn($"Error to get SDR serial number:{e.Message}");
        }
        return "SDR Payload";
    }
    public override DeviceClass Class => DeviceClass.SdrPayload;
    public IAsvSdrClientEx Sdr { get; }
    public ICommandClient Command { get; }
    public IMissionClientEx Missions { get; }

    public IParamsClientEx Params => _params;
}