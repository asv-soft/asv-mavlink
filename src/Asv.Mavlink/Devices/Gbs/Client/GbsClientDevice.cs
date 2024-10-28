#nullable enable
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;

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
    private readonly ParamsClientEx _params;
    private readonly ILogger _logger;

    public GbsClientDevice(IMavlinkV2Connection connection,
        MavlinkClientIdentity identity,
        IPacketSequenceCalculator seq,
        GbsClientDeviceConfig config,
        TimeProvider? timeProvider = null,
        IScheduler? scheduler = null, 
        ILoggerFactory? loggerFactory = null) 
        : base(connection, identity,config, seq,timeProvider, scheduler, loggerFactory)
    {
        _config = config;
        loggerFactory ??= NullLoggerFactory.Instance;
        _logger = loggerFactory.CreateLogger<GbsClientDevice>();
        scheduler ??= Scheduler.Default;
        Command = new CommandClient(connection, identity, seq, config.Command,timeProvider, scheduler,loggerFactory).DisposeItWith(Disposable);
        var gbs = new AsvGbsClient(connection,identity,seq,timeProvider, scheduler,loggerFactory).DisposeItWith(Disposable);
        Gbs = new AsvGbsExClient(gbs,Heartbeat,Command,timeProvider, scheduler,loggerFactory).DisposeItWith(Disposable);
        var paramBase = new ParamsClient(connection, identity, seq, config.Params,timeProvider, scheduler,loggerFactory).DisposeItWith(Disposable);
        _params = new ParamsClientEx(paramBase, config.Params,timeProvider, scheduler,loggerFactory).DisposeItWith(Disposable);
    }

    public IParamsClientEx Params => _params;
    public ICommandClient Command { get; }
    public IAsvGbsExClient Gbs { get; }
    
    protected override string DefaultName => $"RTK GBS [{Identity.TargetSystemId:00},{Identity.TargetComponentId:00}]";

    protected override async Task InternalInit()
    {
        _params.Init(MavParamHelper.ByteWiseEncoding, ArraySegment<ParamDescription>.Empty);
        try
        {
            _logger.ZLogInformation($"Try to read serial number from param {_config.SerialNumberParamName}");
            Params.Filter(_config.SerialNumberParamName)
                .Select(serial => $"RTK GBS [{(int)serial:D5}]")
                .Subscribe(EditableName)
                .DisposeItWith(Disposable);
            await Params.ReadOnce(_config.SerialNumberParamName, DisposeCancel).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"Error to get serial number:{e.Message}");
        }
    }

    public override DeviceClass Class => DeviceClass.GbsRtk;
}