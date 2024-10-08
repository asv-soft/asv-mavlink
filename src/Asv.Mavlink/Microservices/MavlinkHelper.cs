using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink
{
    public static class MavlinkHelper
    {
        public static async Task WaitCompleteWithDefaultTimeout(Func<bool> condition, string actionName, int timeoutMs, CancellationToken cancel)
        {
            var started = DateTime.Now;
            while (!condition())
            {
                await Task.Delay(500, cancel).ConfigureAwait(false);
                if ((DateTime.Now - started).TotalMilliseconds > timeoutMs)
                {
                    throw new TimeoutException(string.Format(RS.VehicleBase_WaitCompleteWithDefaultTimeout_Timeout_to_execute, actionName, TimeSpan.FromMilliseconds(timeoutMs))); ;
                }
            }
        }

        public static IObservable<IPacketV2<IPayload>> FilterVehicle(this IObservable<IPacketV2<IPayload>> src, MavlinkClientIdentity identity)
        {
            return src.Where(v => FilterVehicle(v, identity.TargetSystemId,identity.TargetComponentId));
        }

        public static IObservable<IPacketV2<IPayload>> FilterVehicle(this IObservable<IPacketV2<IPayload>> src, byte targetSystemId, byte targetComponentId)
        {
            return src.Where(v => FilterVehicle(v, targetSystemId, targetComponentId));
        }

        public static bool FilterVehicle(IPacketV2<IPayload> packetV2, byte targetSystemId, byte targetComponentId)
        {
            if (targetSystemId != 0 && targetSystemId != packetV2.SystemId) return false;
            if (targetComponentId != 0 && targetComponentId != packetV2.ComponentId) return false;
            return true;
        }

        public static IObservable<TPacket> Filter<TPacket>(this IObservable<IPacketV2<IPayload>> src)
            where TPacket: IPacketV2<IPayload>,new()
        {
            var pkt = new TPacket();
            var id = pkt.MessageId;
            return src.Where(v => v.MessageId == id).Cast<TPacket>();
        }

      
        
        public static ushort ConvertToFullId(byte componentId, byte systemId) => (ushort)(componentId | systemId << 8);

        public static void ConvertFromId(ushort fullId, out byte componentId, out byte systemId)
        {
            componentId = (byte)(fullId & 0xFF);
            systemId = (byte)(fullId >> 8);
        }
        
    }
}
