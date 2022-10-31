using Asv.Mavlink.Client;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    public static class VehicleFactory
    {
        public static IVehicle CreateVehicle(MavlinkClient client, IMavlinkDeviceInfo info, bool disposeClient)
        {
            switch (info.Autopilot)
            {
               
                case MavAutopilot.MavAutopilotArdupilotmega:
                    return SelectArdupilotmega(client,info, disposeClient);
                case MavAutopilot.MavAutopilotGeneric:
                case MavAutopilot.MavAutopilotReserved:
                case MavAutopilot.MavAutopilotSlugs:
                case MavAutopilot.MavAutopilotOpenpilot:
                case MavAutopilot.MavAutopilotGenericWaypointsOnly:
                case MavAutopilot.MavAutopilotGenericWaypointsAndSimpleNavigationOnly:
                case MavAutopilot.MavAutopilotGenericMissionFull:
                case MavAutopilot.MavAutopilotInvalid:
                case MavAutopilot.MavAutopilotPpz:
                case MavAutopilot.MavAutopilotUdb:
                case MavAutopilot.MavAutopilotFp:
                case MavAutopilot.MavAutopilotPx4:
                case MavAutopilot.MavAutopilotSmaccmpilot:
                case MavAutopilot.MavAutopilotAutoquad:
                case MavAutopilot.MavAutopilotArmazila:
                case MavAutopilot.MavAutopilotAerob:
                case MavAutopilot.MavAutopilotAsluav:
                case MavAutopilot.MavAutopilotSmartap:
                case MavAutopilot.MavAutopilotAirrails:
                default:
                    return null;
            }
        }

        public static VehicleArdupilot SelectArdupilotmega(MavlinkClient client, IMavlinkDeviceInfo info, bool disposeClient)
        {
            switch (info.Type)
            {
                case MavType.MavTypeFixedWing:
                    return new VehicleArdupilotPlane(client,new VehicleBaseConfig(), disposeClient);
                case MavType.MavTypeQuadrotor:
                case MavType.MavTypeTricopter:
                case MavType.MavTypeHexarotor:
                case MavType.MavTypeOctorotor:
                case MavType.MavTypeHelicopter:
                    return new VehicleArdupilotCopter(client, new VehicleBaseConfig());
                case MavType.MavTypeSurfaceBoat:
                case MavType.MavTypeGroundRover:
                case MavType.MavTypeAntennaTracker:
                case MavType.MavTypeSubmarine:
                default:
                    return null;
            }
        }
    }
}