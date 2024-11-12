using System.Linq;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Test.Utils;

public class AdsbVehiclePacketComparer
{
    public static bool IsEqual(AdsbVehiclePacket? left, AdsbVehiclePacket? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        return left?.Name == right?.Name
               && left?.MessageId == right?.MessageId
               && left?.WrapToV2Extension == right?.WrapToV2Extension
               && left?.Magic == right?.Magic
               && left?.Sequence == right?.Sequence
               && left?.Tag == right?.Tag
               && left?.CompatFlags == right?.CompatFlags
               && left?.ComponentId == right?.ComponentId
               && left?.FullId == right?.FullId
               && left?.IncompatFlags == right?.IncompatFlags
               && left?.SystemId == right?.SystemId
               && IsEqualPayload(left?.Payload, right?.Payload);
    }
    
    public static bool IsEqualPayload(AdsbVehiclePayload? left, AdsbVehiclePayload? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        return left?.IcaoAddress == right?.IcaoAddress
               && left?.Callsign.SequenceEqual(right.Callsign) == true
               && left?.Lat == right?.Lat
               && left?.Lon == right?.Lon
               && left?.Altitude == right?.Altitude
               && left?.Heading == right?.Heading
               && left?.EmitterType == right?.EmitterType
               && left?.AltitudeType == right?.AltitudeType
               && left?.Tslc == right?.Tslc
               && left?.HorVelocity == right?.HorVelocity
               && left?.VerVelocity == right?.VerVelocity
               && left?.Flags == right?.Flags
               && left?.Squawk == right?.Squawk;
    }
}