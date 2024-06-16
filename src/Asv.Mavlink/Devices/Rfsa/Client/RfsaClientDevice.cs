using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Diagnostic.Client;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink;

public class RfsaClientDeviceConfig:ClientDeviceConfig
{
    public ParamsClientExConfig Params { get; set; } = new();
    public AsvChartClientConfig Charts { get; set; } = new();
    public CommandProtocolConfig Command { get; set; } = new();
    public DiagnosticClientConfig Diagnostics { get; set; } = new();
    public string SerialNumberParamName { get; set; } = "BRD_SERIAL_NUM";
    
}

public class RfsaClientDevice:ClientDevice, IRfsaClientDevice
{
    private readonly RfsaClientDeviceConfig _config;
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly ParamsClientEx _params;
    private readonly AsvChartClient _charts;
    private readonly DiagnosticClient _diagnostics;

    public RfsaClientDevice(IMavlinkV2Connection link, MavlinkClientIdentity identity, RfsaClientDeviceConfig config, IPacketSequenceCalculator seq, IScheduler scheduler)
        :base(link,identity,config,seq,scheduler)
    {
        _config = config;
        Command = new CommandClient(link, identity, seq, config.Command).DisposeItWith(Disposable);
        var paramBase = new ParamsClient(link, identity, seq, config.Params).DisposeItWith(Disposable);
        _params = new ParamsClientEx(paramBase, config.Params).DisposeItWith(Disposable);
        _charts = new AsvChartClient(config.Charts, link, identity, seq).DisposeItWith(Disposable);
        _diagnostics = new DiagnosticClient(config.Diagnostics, link, identity, seq, scheduler).DisposeItWith(Disposable);
    }
    
    protected override async Task InternalInit()
    {
        _params.Init(MavParamHelper.ByteWiseEncoding, ArraySegment<ParamDescription>.Empty);
        try
        {
            _logger.Trace($"Try to read serial number from param {_config.SerialNumberParamName}");
            Params.Filter(_config.SerialNumberParamName)
                .Select(serial => $"RFSA [{(int)serial:D5}]")
                .Subscribe(EditableName)
                .DisposeItWith(Disposable);
            await Params.ReadOnce(_config.SerialNumberParamName, DisposeCancel).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.Warn($"Error to get serial number:{e.Message}");
        }
    }
    public IParamsClientEx Params => _params;
    public IAsvChartClient Charts => _charts;
    public ICommandClient Command { get; }
    public IDiagnosticClient Diagnostic => _diagnostics;
    
    public async Task<MavResult> Enable(ulong frequencyHz, uint spanHz, CancellationToken cancel = default)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await Command.CommandLong(item => RfsaHelper.SetArgsForEnableCommand(item, frequencyHz,spanHz),cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    public async Task<MavResult> Disable(CancellationToken cancel = default)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        var result = await Command.CommandLong(item => RfsaHelper.SetArgsForDisableCommand(item),cs.Token).ConfigureAwait(false);
        return result.Result;
    }

    protected override string DefaultName => $"RFSA [{Identity.TargetSystemId:00},{Identity.TargetComponentId:00}]";
    public override DeviceClass Class => DeviceClass.Rfsa;
}