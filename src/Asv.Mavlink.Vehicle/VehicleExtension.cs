using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink.Vehicle
{
    public static class VehicleHelper
    {
        

        

        

        /// <summary>
        /// Число попыток вызвать метод GoToGlob()
        /// </summary>
        private const int DefaultAttemptsCount = 10;
        /// <summary>
        /// Расстоение в метрах, которое будет являться признаком движения судна
        /// </summary>
        private const int FlightSignDistance = 5;
        /// <summary>
        /// Частота проверки начала движения судна
        /// </summary>
        private static readonly TimeSpan FlightCheckFrequency = TimeSpan.FromSeconds(3);



        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static async Task<bool> IsFlightDetected(this IVehicle vehicle, GeoPoint startLocation,
            GeoPoint targetLocation, int attemptsCount, bool isWithoutAltitude, double precisionMet, Action<string> logger,
            CancellationToken cancel)
        {
            logger = logger ?? (_ => { });
            var startDistance = isWithoutAltitude
                ? Math.Abs(GeoMath.Distance(targetLocation.SetAltitude(0), startLocation.SetAltitude(0)))
                : Math.Abs(GeoMath.Distance(targetLocation, startLocation));

            var isFlying = false;
            for (var i = 0; i < attemptsCount; i++)
            {
                if (cancel.IsCancellationRequested) break;
                Logger.Debug($"Send command GoTo to vehicle. Attempt number {i + 1}", startLocation);
                logger($"Send command GoTo to vehicle . Attempt number {i + 1}. {startLocation}");
                await vehicle.GoToGlob(targetLocation, cancel).ConfigureAwait(false);
                await Task.Delay(FlightCheckFrequency, cancel).ConfigureAwait(false);
                var loc = vehicle.GlobalPosition.Value;
                var dist = isWithoutAltitude
                    ? Math.Abs(GeoMath.Distance(targetLocation.SetAltitude(0), loc.SetAltitude(0)))
                    : Math.Abs(GeoMath.Distance(targetLocation, loc));

                if (startDistance - dist > FlightSignDistance || dist <= precisionMet)
                {
                    isFlying = true;
                    break;
                }
            }

            return isFlying;
        }

        private static async Task GoToGlobAndWaitWithConfirmBase(this IVehicle vehicle, GeoPoint location,
            IProgress<double> progress, double precisionMet, CancellationToken cancel, bool isWithoutAltitude = false,
            int attemptsCount = DefaultAttemptsCount, Action<string> logger = null)
        {
            logger = logger ?? (_ => { });


            var startLocation = vehicle.GlobalPosition.Value;
            var startDistance = isWithoutAltitude
                ? Math.Abs(GeoMath.Distance(location.SetAltitude(0), startLocation.SetAltitude(0)))
                : Math.Abs(GeoMath.Distance(location, startLocation));
            await vehicle.GoToGlob(location, cancel).ConfigureAwait(false);
            progress = progress ?? new Progress<double>();
            
            Logger.Info("GoToGlobAndWaitWithConfirm {0} with precision {2:F1} m. Distance to target {3:F1}", location, precisionMet, Math.Abs(GeoMath.Distance(location, startLocation)));
            logger($"GoToGlobAndWaitWithConfirm {location} with precision {precisionMet:F1} m. Distance to target {Math.Abs(GeoMath.Distance(location, startLocation)):F1}");

            progress.Report(0);

            if (startDistance <= precisionMet)
            {
                Logger.Debug("Already in target, nothing to do", startLocation);
                logger($"Already in target, nothing to do {startLocation}");
                progress.Report(1);
                return;
            }

            var sw = new Stopwatch();
            sw.Start();

            double dist = 0;
            var isFlying = false;
            for (var i = 0; i < attemptsCount; i++)
            {
                if (cancel.IsCancellationRequested) break;
                Logger.Debug($"Send command GoTo to vehicle. Attempt number {i + 1}", startLocation);
                logger($"Send command GoTo to vehicle . Attempt number {i + 1}. {startLocation}");
                await vehicle.GoToGlob(location, cancel).ConfigureAwait(false);
                await Task.Delay(FlightCheckFrequency, cancel).ConfigureAwait(false);
                var loc = vehicle.GlobalPosition.Value;
                dist = isWithoutAltitude
                    ? Math.Abs(GeoMath.Distance(location.SetAltitude(0), loc.SetAltitude(0)))
                    : Math.Abs(GeoMath.Distance(location, loc));

                if (startDistance - dist > FlightSignDistance || dist <= precisionMet)
                {
                    isFlying = true;
                    break;
                }
            }

            if (!isFlying)
            {
                Logger.Info($"GoToGlobAndWaitWithConfirm. Command {nameof(vehicle.GoToGlob)} did not reach to UAV");
                logger($"GoToGlobAndWaitWithConfirm. Command {nameof(vehicle.GoToGlob)} did not reach to UAV");
                throw new Exception($"GoToGlobAndWaitWithConfirm. Command {nameof(vehicle.GoToGlob)} did not reach to UAV");
            }

            while (!cancel.IsCancellationRequested)
            {
                var loc = vehicle.GlobalPosition.Value;
                dist = isWithoutAltitude
                    ? Math.Abs(GeoMath.Distance(location.SetAltitude(0), loc.SetAltitude(0)))
                    : Math.Abs(GeoMath.Distance(location, loc));

                var prog = 1 - dist / startDistance;
                Logger.Trace("Distance to target {0:F1}, location: {1}, progress {2:P2}", dist, loc, prog);
                logger($"Distance to target {dist:F1}, location: {loc}, progress {prog:P2}");
                progress.Report(prog);
                if (dist <= precisionMet) break;
                await Task.Delay(TimeSpan.FromSeconds(1), cancel).ConfigureAwait(false);
            }
            sw.Stop();
            Logger.Info($"Complete {sw.Elapsed:hh\\:mm\\:ss} location error {dist:F1} m");
            logger($"Complete {sw.Elapsed:hh\\:mm\\:ss} location error {dist:F1} m");
            progress.Report(1);
        }

        public static Task GoToGlobAndWaitWithConfirm(this IVehicle vehicle, GeoPoint location, IProgress<double> progress, double precisionMet, CancellationToken cancel, int attemptsCount = DefaultAttemptsCount, Action<string> logger = null)
        {
            return GoToGlobAndWaitWithConfirmBase(vehicle, location, progress, precisionMet, cancel, false,
                attemptsCount, logger);
        }

        public static Task GoToGlobAndWaitWithoutAltitudeWithConfirm(this IVehicle vehicle, GeoPoint location, IProgress<double> progress, double precisionMet, CancellationToken cancel, int attemptsCount = DefaultAttemptsCount, Action<string> logger = null)
        {
            return GoToGlobAndWaitWithConfirmBase(vehicle, location, progress, precisionMet, cancel, true,
                attemptsCount, logger);
        }

        public static Task KillSwitch(this IVehicle vehicle,CancellationToken cancel)
        {
            return vehicle.ArmDisarm(false, cancel, true);
        }

        public static MissionItem AddSplineMissionItem(this IVehicle vehicle, GeoPoint point)
        {
            var item = vehicle.AddMissionItem();
            item.Location.OnNext(point);
            item.Autocontinue.OnNext(true);
            item.Command.OnNext(MavCmd.MavCmdNavSplineWaypoint);
            item.Current.OnNext(false);
            item.Frame.OnNext(MavFrame.MavFrameGlobalInt);
            item.MissionType.OnNext(MavMissionType.MavMissionTypeMission);
            item.Param1.OnNext(0);
            item.Param2.OnNext(0);
            item.Param3.OnNext(0);
            item.Param4.OnNext(0);
            return item;
        }
        public static MissionItem AddNavMissionItem(this IVehicle vehicle, GeoPoint point)
        {
            var item = vehicle.AddMissionItem();
            item.Location.OnNext(point);
            item.Autocontinue.OnNext(true);
            item.Command.OnNext(MavCmd.MavCmdNavWaypoint);
            item.Current.OnNext(false);
            item.Frame.OnNext(MavFrame.MavFrameGlobalInt);
            item.MissionType.OnNext(MavMissionType.MavMissionTypeMission);
            item.Param1.OnNext(0);
            item.Param2.OnNext(0);
            item.Param3.OnNext(0);
            item.Param4.OnNext(0);
            return item;
        }

    }
}
