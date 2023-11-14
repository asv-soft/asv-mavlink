#nullable enable
using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using NLog;

namespace Asv.Mavlink;

public class GbsClientDeviceConfig:ClientDeviceConfig
{
    public CommandProtocolConfig Command { get; set; } = new();
    public ParamsClientExConfig Params { get; set; } = new();
    public string SerialNumberParamName { get; set; } = "BRD_SERIAL_NUM";
}
public class GbsClientDevice : ClientDevice, IGbsClientDevice
{
    private readonly GbsClientDeviceConfig _config;
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly ParamsClientEx _params;
    public GbsClientDevice(IMavlinkV2Connection connection,
        MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq,
        GbsClientDeviceConfig config,
        IScheduler? scheduler = null) : base(connection, identity,config, seq, scheduler)
    {
        _config = config;
        Command = new CommandClient(connection, identity, seq, config.Command).DisposeItWith(Disposable);
        var gbs = new AsvGbsClient(connection,identity,seq,scheduler).DisposeItWith(Disposable);
        Gbs = new AsvGbsExClient(gbs,Heartbeat,Command).DisposeItWith(Disposable);
        var paramBase = new ParamsClient(connection, identity, seq, config.Params).DisposeItWith(Disposable);
        _params = new ParamsClientEx(paramBase, config.Params).DisposeItWith(Disposable);
    }

    public IParamsClientEx Params => _params;
    public ICommandClient Command { get; }
    public IAsvGbsExClient Gbs { get; }
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
                return $"RTK GBS [{serialNumber:D5}]";    
            }
        }
        catch (Exception e)
        {
            _logger.Error($"Error to get GBS serial number:{e.Message}");
        }
        return "RTL GBS";
    }

    public override DeviceClass Class => DeviceClass.GbsRtk;
}