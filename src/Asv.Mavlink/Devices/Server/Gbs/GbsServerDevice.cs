using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Mavlink.Common;


namespace Asv.Mavlink;

public class GbsServerDeviceConfig:ServerDeviceConfig
{
    public AsvGbsServerConfig Gbs { get; set; } = new();
    public ParamsServerExConfig Params { get; set; } = new();
}
public class GbsServerDevice:ServerDevice, IGbsServerDevice
{
    private readonly CommandServer _commandBase;
    private readonly CommandServerEx<CommandLongPacket> _commandLongEx;
    private readonly AsvGbsServer _gbsBase;
    private readonly AsvGbsExServer _gbs;
    private readonly ParamsServerEx _params;
    private readonly ParamsServer _paramsBase;

    public GbsServerDevice(MavlinkIdentity identity, 
        GbsServerDeviceConfig config,
        IEnumerable<IMavParamTypeMetadata> paramList,
        IMavParamEncoding encoding,
        IConfiguration paramStore,
        IMavlinkContext core) : base(identity, config, core)
    {
        _commandBase = new CommandServer(identity,core);
        _commandLongEx = new CommandLongServerEx(_commandBase);
        _gbsBase = new AsvGbsServer(identity, config.Gbs,core);
        _gbs = new AsvGbsExServer(_gbsBase,Heartbeat,CommandLongEx);
        _paramsBase = new ParamsServer(identity, core);
        _params = new ParamsServerEx(_paramsBase,StatusText,paramList,encoding,paramStore,config.Params);
    }

    public override void Start()
    {
        base.Start();
        Gbs.Base.Start();
    }

    public ICommandServerEx<CommandLongPacket> CommandLongEx => _commandLongEx;

    public IAsvGbsServerEx Gbs => _gbs;

    public IParamsServerEx Params => _params;

    #region Dispose and Dispose async   

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _commandBase.Dispose();
            _commandLongEx.Dispose();
            _gbsBase.Dispose();
            _gbs.Dispose();
            _params.Dispose();
            _paramsBase.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await CastAndDispose(_commandBase).ConfigureAwait(false);
        await CastAndDispose(_commandLongEx).ConfigureAwait(false);
        await CastAndDispose(_gbsBase).ConfigureAwait(false);
        await CastAndDispose(_gbs).ConfigureAwait(false);
        await CastAndDispose(_params).ConfigureAwait(false);
        await CastAndDispose(_paramsBase).ConfigureAwait(false);

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