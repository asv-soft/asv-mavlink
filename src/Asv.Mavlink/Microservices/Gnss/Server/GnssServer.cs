namespace Asv.Mavlink;

public class GnssServer : MavlinkMicroserviceServer, IGnssServer
{
    public GnssServer(MavlinkIdentity identity, IMavlinkContext core) 
        : base(GnssHelper.MicroserviceName, identity, core)
    {
        
    }
}