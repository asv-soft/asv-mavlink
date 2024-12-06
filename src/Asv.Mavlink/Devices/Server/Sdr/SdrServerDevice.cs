using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Mavlink.Common;


namespace Asv.Mavlink;

public class SdrServerDeviceConfig : ServerDeviceConfig
{
    public AsvSdrServerConfig Sdr { get; set; } = new();
    public ParamsServerExConfig Params { get; set; } = new();
}

public class SdrServerDevice:ServerDevice, ISdrServerDevice
{
    

    private readonly AsvSdrServer _sdrBase;
    private readonly CommandServer _cmdBase;
    private readonly AsvSdrServerEx _sdrEx;
    private readonly MissionServerEx _missions;
    private readonly CommandServerEx<CommandLongPacket> _commandLongEx;
    private readonly ParamsServerEx _params;
    private readonly ParamsServer _paramsBase;
    private readonly MissionServer _missionBase;

    public SdrServerDevice( 
        MavlinkIdentity identity, 
        SdrServerDeviceConfig config, 
        IEnumerable<IMavParamTypeMetadata> paramList,
        IMavParamEncoding encoding,
        IConfiguration paramStore,
        IMavlinkContext core)
        : base( identity, config, core)
    {
        ArgumentNullException.ThrowIfNull(config);

        _sdrBase = new AsvSdrServer(identity, config.Sdr,core);
        _cmdBase = new CommandServer(identity, core);
        _commandLongEx = new CommandLongServerEx(_cmdBase);
        
        _sdrEx = new AsvSdrServerEx(_sdrBase, StatusText, Heartbeat, CommandLongEx);
        _paramsBase = new ParamsServer(identity, core);
        _params = new ParamsServerEx(_paramsBase,StatusText,paramList,encoding,paramStore,config.Params);
        _missionBase = new MissionServer(identity, core);
        _missions = new MissionServerEx(_missionBase, StatusText);
        
    }

    public override void Start()
    {
        base.Start();
        SdrEx.Base.Start();
    }

    public IAsvSdrServerEx SdrEx => _sdrEx;

    public IMissionServerEx Missions => _missions;

    public ICommandServerEx<CommandLongPacket> CommandLongEx => _commandLongEx;

    public IParamsServerEx Params => _params;

    #region Dispose and dispose async

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _sdrBase.Dispose();
            _cmdBase.Dispose();
            _sdrEx.Dispose();
            _missions.Dispose();
            _commandLongEx.Dispose();
            _params.Dispose();
            _paramsBase.Dispose();
            _missionBase.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_sdrBase).ConfigureAwait(false);
        await CastAndDispose(_cmdBase).ConfigureAwait(false);
        await CastAndDispose(_sdrEx).ConfigureAwait(false);
        await CastAndDispose(_missions).ConfigureAwait(false);
        await CastAndDispose(_commandLongEx).ConfigureAwait(false);
        await CastAndDispose(_params).ConfigureAwait(false);
        await CastAndDispose(_paramsBase).ConfigureAwait(false);
        await CastAndDispose(_missionBase).ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);

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