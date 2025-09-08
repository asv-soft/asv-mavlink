using System;
using System.Buffers;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Common;
using R3;

namespace Asv.Mavlink;

public class DgpsServer : MavlinkMicroserviceServer, IDgpsServer
{
    
    public DgpsServer(MavlinkIdentity identity, IMavlinkContext core) : base(DgpsMixin.MicroserviceTypeName,identity, core)
    {
        core.Connection.RxFilterByType<GpsRtcmDataPacket>().Subscribe(OnData).RegisterTo(DisposeCancel);
    }

    private void OnData(GpsRtcmDataPacket gpsRtcmDataPacket)
    {
        throw new NotImplementedException();
    }

    public Func<IMemoryOwner<byte>>? OnRtcmData { get; set; }
}

public interface IDgpsServer : IMavlinkMicroserviceServer
{
    public Func<IMemoryOwner<byte>>? OnRtcmData { set; }
}