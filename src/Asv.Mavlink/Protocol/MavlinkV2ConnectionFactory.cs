using Asv.IO;
using Asv.Mavlink.V2.Ardupilotmega;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Icarous;
using Asv.Mavlink.V2.Uavionix;

namespace Asv.Mavlink
{
    public class MavlinkV2ConnectionFactory
    {
        public static IMavlinkV2Connection Create(IDataStream dataStream, bool disposeDataStream = false)
        {
            return new MavlinkV2Connection(dataStream, RegisterDefaultDialects,disposeDataStream);
        }
        
        public static IMavlinkV2Connection CreateDefault(string connectionString)
        {
            return new MavlinkV2Connection(connectionString, RegisterDefaultDialects);
        }

        public static void RegisterDefaultDialects(IPacketDecoder<IPacketV2<IPayload>> decoder)
        {
            decoder.RegisterCommonDialect();
            decoder.RegisterArdupilotmegaDialect();
            decoder.RegisterIcarousDialect();
            decoder.RegisterUavionixDialect();
            decoder.RegisterAsvGbsDialect();
            decoder.RegisterAsvSdrDialect();
        }
    }
}