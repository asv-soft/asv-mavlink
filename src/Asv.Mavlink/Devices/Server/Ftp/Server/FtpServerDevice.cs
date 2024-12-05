using System.Threading.Tasks;

namespace Asv.Mavlink;

public class FtpServerDeviceConfig: ServerDeviceConfig
{
    public required MavlinkFtpServerConfig ServerCfg { get; set; }
    public required MavlinkFtpServerExConfig ServerExCfg { get; set; }
}

public sealed class FtpServerDevice : ServerDevice, IFtpServerDevice
{
    

    private readonly FtpServerEx _ftp;
    private readonly FtpServer _serverBase;

    public FtpServerDevice( 
        MavlinkIdentity identity, 
        FtpServerDeviceConfig config, IMavlinkContext core) 
        : base(identity, config,core)
    {
        _serverBase = new FtpServer(identity,config.ServerCfg,core);
        _ftp = new FtpServerEx(_serverBase, config.ServerExCfg);
    }

    public IFtpServerEx Ftp => _ftp;
    
    protected sealed override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _ftp.Dispose();
            _serverBase.Dispose();
        }

        base.Dispose(disposing);
    }

    protected sealed override async ValueTask DisposeAsyncCore()
    {
        await _ftp.DisposeAsync().ConfigureAwait(false);
        await _serverBase.DisposeAsync().ConfigureAwait(false);

        await base.DisposeAsyncCore().ConfigureAwait(false);
    }
}