using System.Threading.Tasks;

namespace Asv.Mavlink;

public class GenericDevice: ClientDevice
{
    public GenericDevice(MavlinkClientIdentity identity, ClientDeviceConfig config, ICoreServices core) : base(identity,config,core)
    {
        
    }

    protected override Task InternalInit()
    {
        return Task.CompletedTask;
    }

    public override DeviceClass Class => DeviceClass.Unknown;
}