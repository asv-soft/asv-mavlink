using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink
{
    public static class VehicleDoRepositionHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
//
//        /// <summary>
//        /// Reposition the vehicle to a specific WGS84 global position.
//        /// </summary>
//        /// <returns></returns>
//        public static Task<CommandAckPayload> DoReposition(this IMavlinkV2Protocol src, float groundSpeed, GeoPoint newPosition, CancellationToken cancel)
//        {
//            return src.DoReposition(groundSpeed, true, -1, (float)newPosition.Latitude, (float)newPosition.Longitude, (float)newPosition.Altitude.Value, cancel);
//        }
//
//        /// <summary>
//        /// Reposition the vehicle to a specific WGS84 global position. Altitude - relative from home altitude.
//        /// </summary>
//        /// <returns></returns>
//        public static Task<CommandAckPayload> DoRepositionRelative(this IMavlinkV2Protocol src, float groundSpeed, GeoPoint newRelativePosition, CancellationToken cancel)
//        {
//            return src.DoReposition(groundSpeed, true, -1, (float)newRelativePosition.Latitude, (float)newRelativePosition.Longitude, (float)newRelativePosition.Altitude.Value + (float)src.Rtt.Home.Value.Altitude, cancel);
//        }
//
//        public static async Task DoRepositionAndWait(this IVehicle vehicle, GeoPoint geoPoint, double velocity, double precisionMet, int checkTimeMs, CancellationToken cancel, IProgress<double> progress)
//        {
//            progress = progress ?? new Progress<double>();
//            var startLocation = vehicle.Rtt.RelGps.Value;
//            var startDistance = GeoMath.Distance(geoPoint, startLocation);
//
//            Logger.Info("DoRepositionAndWait {0} with V={1:F1} m/sec and precision {2:F1} m. Distance to target {3:F1}", geoPoint, velocity, precisionMet, startDistance);
//            progress.Report(0);
//            if (startDistance <= precisionMet)
//            {
//                Logger.Debug("Already in target, nothing to do", startLocation);
//                progress.Report(1);
//                return;
//            }
//
//            var sw = new Stopwatch();
//            sw.Start();
//            Logger.Debug("Send command DoReposition to vehicle", startLocation);
//            await vehicle.DoReposition((float)velocity, geoPoint, cancel).ConfigureAwait(false);
//            double dist = 0;
//            while (!cancel.IsCancellationRequested)
//            {
//                var loc = vehicle.Rtt.RelGps.Value;
//                dist = Math.Abs(GeoMath.Distance(geoPoint, loc));
//                var prog = 1 - dist / startDistance;
//                Logger.Trace("Distance to target {0:F1}, location: {1}, progress {2:P2}", dist, loc, prog);
//                progress.Report(prog);
//                if (dist <= precisionMet) break;
//                await Task.Delay(checkTimeMs, cancel).ConfigureAwait(false);
//            }
//            sw.Stop();
//            Logger.Info($"Complete {sw.Elapsed:hh\\:mm\\:ss} location error {dist:F1} m");
//            progress.Report(1);
//        }

        /// <summary>
        /// Reposition the vehicle to a specific WGS84 global position.
        /// </summary>
        /// <returns></returns>
        public static Task<CommandAckPayload> DoReposition(this IMavlinkClient src, float groundSpeed, bool switchToGuided, float yaw, float lat, float lon, float alt, CancellationToken cancel)
        {
            return src.Commands.CommandLong(MavCmd.MavCmdDoReposition, groundSpeed, switchToGuided ? (float)MavDoRepositionFlags.MavDoRepositionFlagsChangeMode : 0, float.NaN, yaw, lat, lon, alt, 3, cancel);
        }
//
//        public static async Task DoRepositionMoveUp(this IMavlinkV2Protocol vehicle, double moveDistance, double moveVelocity, CancellationToken cancel)
//        {
//            var loc = vehicle.Rtt.RelGps.Value;
//            loc = loc.AddAltitude(moveDistance);
//            await vehicle.DoReposition((float)moveVelocity, loc, cancel);
//        }
//
//        public static async Task DoRepositionDown(this IMavlinkV2Protocol vehicle, double moveDistance, double moveVelocity, CancellationToken cancel)
//        {
//            var loc = vehicle.Rtt.RelGps.Value;
//            loc = loc.AddAltitude(-moveDistance);
//            await vehicle.DoReposition((float)moveVelocity, loc, cancel);
//        }
//
//        public static Task DoRepositionMoveN(this IMavlinkV2Protocol vehicle, double moveDistance, double moveVelocity, CancellationToken cancel)
//        {
//            return DoRepositionMoveToRadialPoint(vehicle, moveDistance, moveVelocity, 0, cancel);
//        }
//
//        public static Task DoRepositionMoveE(this IMavlinkV2Protocol vehicle, double moveDistance, double moveVelocity, CancellationToken cancel)
//        {
//            return DoRepositionMoveToRadialPoint(vehicle, moveDistance, moveVelocity, 90, cancel);
//        }
//
//        public static Task DoRepositionMoveW(this IMavlinkV2Protocol vehicle, double moveDistance, double moveVelocity, CancellationToken cancel)
//        {
//            return DoRepositionMoveToRadialPoint(vehicle, moveDistance, moveVelocity, 270, cancel);
//        }
//
//        public static Task DoRepositionMoveS(this IMavlinkV2Protocol vehicle, double moveDistance, double moveVelocity, CancellationToken cancel)
//        {
//            return DoRepositionMoveToRadialPoint(vehicle, moveDistance, moveVelocity, 180, cancel);
//        }
//
//        public static async Task DoRepositionMoveToRadialPoint(this IMavlinkV2Protocol vehicle, double moveDistance, double moveVelocity, int radial, CancellationToken cancel)
//        {
//            var loc = vehicle.Rtt.RelGps.Value;
//            var alt = loc.Altitude ?? 0;
//            loc = GeoMath.RadialPoint(loc.Latitude, loc.Longitude, moveDistance, radial);
//            loc = loc.AddAltitude(alt);
//            await vehicle.DoReposition((float)moveVelocity, loc, cancel).ConfigureAwait(false);
//        }

    }
}