using Asv.Common;

namespace Asv.Mavlink;

public interface IGnssClientEx
{
    IGnssStatusClient Main { get; }
    IGnssStatusClient Additional { get; }
}

public class GnssClientEx : DisposableOnceWithCancel, IGnssClientEx
{
    public GnssClientEx(IGnssClient client)
    {
        Base = client;
        Main = new GnssStatusClient(client.Main).DisposeItWith(Disposable);
        Additional = new GnssStatusClient(client.Additional).DisposeItWith(Disposable);
    }
    public IGnssClient Base { get; }
    public IGnssStatusClient Main { get; }
    public IGnssStatusClient Additional { get; }
}