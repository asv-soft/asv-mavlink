#nullable enable
using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink;

public class SdrClientDeviceConfig:ClientDeviceConfig
{
    public CommandProtocolConfig Command { get; set; } = new();
    public AsvSdrClientExConfig SdrEx { get; set; } = new();
    public MissionClientConfig Missions { get; set; } = new();
    public MissionClientExConfig MissionsEx { get; set; } = new();
    public ParameterClientConfig Params { get; set; } = new();
    public ParamsClientExConfig ParamsEx { get; set; } = new();
}
public class SdrClientDevice : ClientDevice, ISdrClientDevice
{
    private readonly ParamsClientEx _params;

    public SdrClientDevice(IMavlinkV2Connection connection, MavlinkClientIdentity identity, SdrClientDeviceConfig config, IPacketSequenceCalculator seq, IScheduler? scheduler = null) : base(connection, identity, config, seq, scheduler)
    {
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

    protected override Task<string> GetCustomName(CancellationToken cancel)
    {
        return Task.FromResult("SdrPayload");
    }
    public override DeviceClass Class => DeviceClass.SdrPayload;
    public IAsvSdrClientEx Sdr { get; }
    public ICommandClient Command { get; }
    public IMissionClientEx Missions { get; }

    public IParamsClientEx Params => _params;
}