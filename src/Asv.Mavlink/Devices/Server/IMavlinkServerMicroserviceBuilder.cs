using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Metrics;
using System.IO.Abstractions;
using Asv.Cfg;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.Common;
using Asv.Mavlink.Diagnostic.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Asv.Mavlink;



public interface IMavlinkServerMicroserviceBuilder
{
    void SetPacketSequence(IPacketSequenceCalculator calculator);
    void SetLog(ILoggerFactory loggerFactory);
    void SetTimeProvider(TimeProvider timeProvider);
    void SetMeterFactory(IMeterFactory meterFactory);
    void SetConfiguration(IConfiguration configuration);
    IMavlinkServerMicroserviceBuilder Register<TMicroservice>(Func<IMavlinkServerMicroserviceFactory,TMicroservice> factory)
        where TMicroservice: IMavlinkMicroserviceServer;
    IMavlinkServerMicroserviceBuilder Register<TMicroservice, TArg>(Func<IMavlinkServerMicroserviceFactory,TArg,TMicroservice> factory, TArg arg1)
        where TMicroservice: IMavlinkMicroserviceServer;
}

public class MavlinkServerMicroserviceBuilder(MavlinkIdentity identity, IProtocolConnection connection) 
    : IMavlinkServerMicroserviceBuilder
{
    private IPacketSequenceCalculator? _seq;
    private ILoggerFactory? _logFactory;
    private TimeProvider? _timeProvider;
    private IMeterFactory? _meterFactory;
    private IConfiguration? _configuration;
    private ImmutableDictionary<Type,Func<IMavlinkServerMicroserviceFactory,IMavlinkMicroserviceServer>>.Builder _builder 
        = ImmutableDictionary.CreateBuilder<Type, Func<IMavlinkServerMicroserviceFactory, IMavlinkMicroserviceServer>>();

    public void SetPacketSequence(IPacketSequenceCalculator calculator)
    {
        ArgumentNullException.ThrowIfNull(calculator);
        _seq = calculator;
    }

    public void SetLog(ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        _logFactory = loggerFactory;
    }

    public void SetTimeProvider(TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        _timeProvider = timeProvider;
    }
    
    public void SetMeterFactory(IMeterFactory meterFactory)
    {
        ArgumentNullException.ThrowIfNull(meterFactory);
        _meterFactory = meterFactory;
    }

    public void SetConfiguration(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        _configuration = configuration;
    }

    public IMavlinkServerMicroserviceBuilder Register<TMicroservice>(Func<IMavlinkServerMicroserviceFactory, IMavlinkMicroserviceServer> factory) 
        where TMicroservice : IMavlinkMicroserviceServer
    {
        _builder.Add(typeof(TMicroservice),factory);
        return this;
    }

    public IMavlinkServerMicroserviceBuilder Register<TMicroservice, TArg>(Func<IMavlinkServerMicroserviceFactory, TArg, TMicroservice> factory, TArg arg1) 
        where TMicroservice : IMavlinkMicroserviceServer
    {
        _builder.Add(typeof(TMicroservice), src => factory(src, arg1));
        return this;
    }


    public IMavlinkServerMicroserviceFactory Build()
    {
        var core = new CoreServices(
            connection,
            _seq ?? new PacketSequenceCalculator(),
            _logFactory ?? NullLoggerFactory.Instance,
            _timeProvider ?? TimeProvider.System,
            _meterFactory ?? new DefaultMeterFactory());
        var config = _configuration ?? new InMemoryConfiguration();
        return new MavlinkServerMicroserviceFactory(
            identity,
            core,
            config,
            _audioCodecFactory ?? EmptyAudioCodecFactory.Instance);
    }
}

public interface IMavlinkServerMicroserviceFactory
{
    MavlinkIdentity Identity { get; }
    IMavlinkContext Context { get; }
    IConfiguration Configuration { get; }
    TMicroservice Get<TMicroservice>() 
        where TMicroservice:IMavlinkMicroserviceServer;
}

public class MavlinkServerMicroserviceFactory : IMavlinkServerMicroserviceFactory
{
    private readonly MavlinkIdentity _identity;
    private readonly IMavlinkContext _context;
    private readonly IConfiguration _config;
    private readonly IAudioCodecFactory _audioCodec;
    private IAdsbVehicleServer? _adsb;
    private AudioService? _audio;
    private AsvChartServer? _charts;
    private AsvGbsServer? _gbs;
    private AsvGbsExServer? _gbsEx;
    private AsvRadioServer? _radio;
    private readonly AsvRadioCapabilities _capabilities;
    private readonly IReadOnlySet<AsvAudioCodec> _codecs;
    private AsvRadioServerEx? _radioEx;
    private AsvRsgaServer? _rsga;
    private AsvRsgaServerEx? _rsgaEx;
    private AsvSdrServer? _sdr;
    private IAsvSdrServerEx? _sdrEx;
    private CommandLongServerEx? _cmdLong;
    private CommandServer? _cmd;
    private CommandIntServerEx? _cmdInt;
    private DiagnosticServer? _diag;
    private FtpServer? _ftp;
    private readonly IFileSystem _ftpFileSystem;
    private FtpServerEx? _ftpEx;
    private HeartbeatServer? _hb;
    private ILoggingServer? _logging;
    private MissionServer? _mission;
    private MissionServerEx? _missionEx;
    private ModeServer? _mode;
    private ICustomMode _idleMode;
    private IEnumerable<ICustomMode> _availableModes;
    private Func<ICustomMode, IWorkModeHandler> _workModeFactory;
    private ParamsServer? _params;
    private IEnumerable<IMavParamTypeMetadata> _paramsDesc;
    private IMavParamEncoding _paramsEncoding;
    private ParamsServerEx? _paramsEx;
    private ParamsExtServer? _paramsExt;
    private ParamsExtServerEx? _paramsExtEx;
    private IEnumerable<IMavParamExtTypeMetadata> _paramsExtDesc;
    private StatusTextServer? _status;
    private V2ExtensionServer? _v2Ext;

    public static IMavlinkServerMicroserviceFactory Create(MavlinkIdentity identity, IProtocolConnection connection, Action<IMavlinkServerMicroserviceBuilder> builder)
    {
        var b = new MavlinkServerMicroserviceBuilder(identity,connection);
        builder(b);
        return b.Build();
    }
    
    internal MavlinkServerMicroserviceFactory(
        MavlinkIdentity identity, 
        IMavlinkContext context, 
        IConfiguration config,
        IAudioCodecFactory audioCodec,
        AsvRadioCapabilities capabilities,
        IReadOnlySet<AsvAudioCodec> codecs,
        IFileSystem ftpFileSystem, ParamsExtServerEx? paramsExtEx)
    {
        ArgumentNullException.ThrowIfNull(identity);
        ArgumentNullException.ThrowIfNull(context);
        _identity = identity;
        _context = context;
        _config = config;
        _audioCodec = audioCodec;
        _capabilities = capabilities;
        _codecs = codecs;
        _ftpFileSystem = ftpFileSystem;
        _paramsExtEx = paramsExtEx;
    }

    public IAdsbVehicleServer Adsb => _adsb ??= new AdsbVehicleServer(_identity, _context);
    public IAudioService Audio => _audio ??= new AudioService(_audioCodec, _identity, _config.Get<AudioServiceConfig>(), _context); 
    public IAsvChartServer Charts => _charts ??= new AsvChartServer(_identity,_config.Get<AsvChartServerConfig>(), _context);
    public IAsvGbsServer Gbs => _gbs ??= new AsvGbsServer(_identity, _config.Get<AsvGbsServerConfig>(), _context);
    public IAsvGbsServerEx GbsEx => _gbsEx ??= new AsvGbsExServer(Gbs, Heartbeat, CommandLong);
    public IAsvRadioServer Radio => _radio ??= new AsvRadioServer(_identity, _config.Get<AsvRadioServerConfig>(), _context);
    public IAsvRadioServerEx RadioEx => _radioEx ??= new AsvRadioServerEx(_capabilities, _codecs, Radio, Heartbeat,CommandLong,Status);
    public IAsvRsgaServer Rsga => _rsga ??= new AsvRsgaServer(_identity, _context);
    public IAsvRsgaServerEx RsgaEx => _rsgaEx ??= new AsvRsgaServerEx(Rsga, Status, CommandLong);
    public IAsvSdrServer Sdr => _sdr ??= new AsvSdrServer(_identity, _config.Get<AsvSdrServerConfig>(), _context);
    public IAsvSdrServerEx SdrEx => _sdrEx ??= new AsvSdrServerEx(Sdr, Status, Heartbeat, CommandLong);
    public ICommandServer Command => _cmd ??= new CommandServer(_identity, _context);
    public ICommandServerEx<CommandLongPacket> CommandLong => _cmdLong ??= new CommandLongServerEx(Command);
    public ICommandServerEx<CommandIntPacket> CommandInt => _cmdInt ??= new CommandIntServerEx(Command);
    public IDiagnosticServer Diagnostic => _diag ??= new DiagnosticServer(_identity,_config.Get<DiagnosticServerConfig>(),_context);
    public IFtpServer Ftp => _ftp ??= new FtpServer(_identity, _config.Get<MavlinkFtpServerExConfig>(), _context);
    public IFtpServerEx FtpEx => _ftpEx ??= new FtpServerEx(Ftp, _config.Get<MavlinkFtpServerExConfig>(), _ftpFileSystem); 
    public IHeartbeatServer Heartbeat => _hb ??= new HeartbeatServer(_identity, _config.Get<MavlinkHeartbeatServerConfig>(), _context);
    public ILoggingServer Logging => _logging ??= new LoggingServer(_identity, _context);
    public IMissionServer Mission => _mission ??= new MissionServer(_identity, _context);
    public IMissionServerEx MissionEx => _missionEx ??= new MissionServerEx(Mission, Status);
    public IModeServer Mode => _mode ??= new ModeServer(_identity,Heartbeat,CommandLong,Status,_idleMode, _availableModes, _workModeFactory);
    public IParamsServer Params => _params ??= new ParamsServer(_identity, _context);
    public IParamsServerEx ParamsEx => _paramsEx ??= new ParamsServerEx(Params,Status, _paramsDesc, _paramsEncoding, _config, _config.Get<ParamsServerExConfig>());
    public IParamsExtServer ParamsExt => _paramsExt ??= new ParamsExtServer(_identity, _context);

    public IParamsExtServerEx ParamsExtEx => _paramsExtEx ??= new ParamsExtServerEx(ParamsExt, Status, _paramsExtDesc,
        _config, _config.Get<ParamsExtServerExConfig>());
    public IStatusTextServer Status => _status ??= new StatusTextServer(_identity,_config.Get<StatusTextLoggerConfig>(),_context);
    public IV2ExtensionServer V2Extention => _v2Ext ??= new V2ExtensionServer(_identity, _context);
    
    
    public TMicroservice Get<TMicroservice>() where TMicroservice : IMavlinkMicroserviceServer
    {
        throw new NotImplementedException();
    }
}