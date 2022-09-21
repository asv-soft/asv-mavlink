using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Client;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.Vehicle;
using Geodesy;
using NLog;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink
{

   


    public abstract class VehicleArdupilot : VehicleBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const int DefaultAttemptToReadCount = 3;


        protected VehicleArdupilot(IMavlinkClient mavlink, VehicleBaseConfig config, bool disposeClient) : base(mavlink, config, disposeClient)
        {
        }

        protected override async Task InternalRequestInitialDataAfterDisconnect(CancellationToken cancel = default)
        {
            await base.InternalRequestInitialDataAfterDisconnect(cancel).ConfigureAwait(false);
            try
            {
                // load custom params

            }
            catch (Exception)
            {

            }
        }

        public override async Task SetRoi(GlobalPosition location, CancellationToken cancel)
        {
            _roi.OnNext(location);
            try
            {
                // just send, because ARDUPILOT does not send mavcmd ack
                await Mavlink.Commands.SendCommandLong(MavCmd.MavCmdDoSetRoiLocation, 0, 0, 0, 0, 0, 0, 0, CancellationToken.None).ConfigureAwait(false);
                await Mavlink.Commands.SendCommandLong(MavCmd.MavCmdDoSetRoi, (int)MavRoi.MavRoiLocation, 0, 0, 0, (float)location.Latitude.Degrees, (float)location.Longitude.Degrees, (float)location.Elevation, CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception)
            {
                _roi.OnNext(null);
                throw;
            }
            
        }

        public override IEnumerable<VehicleParamDescription> GetParamDescription()
        {
            return ArdupilotParamParser.Parse(Files.apm_pdef);
        }

        public override async Task ClearRoi(CancellationToken cancel)
        {
            var old = _roi.Value;
            _roi.OnNext(null);
            try
            {
                // just send, because ARDUPILOT does not send mavcmd ack
                await Mavlink.Commands.SendCommandLong(MavCmd.MavCmdDoSetRoi, 0, 0, 0, 0, 0, 0, 0, CancellationToken.None).ConfigureAwait(false);
                //await _mavlink.Commands.SendCommandLong(MavCmd.MavCmdDoSetRoiNone, (int)MavRoi.MavRoiLocation, 0, 0, 0, 0, 0, 0, 1, CancellationToken.None);
            }
            catch (Exception)
            {
                _roi.OnNext(old);
                throw;
            }

        }

        public override async Task ArmDisarm(bool isArming, CancellationToken cancel, bool force = false)
        {
            const float magic_force_arm_value = 2989.0f;
            const float magic_force_disarm_value = 21196.0f;

            if (force)
            {
                var result = await Mavlink.Commands.CommandLong(MavCmd.MavCmdComponentArmDisarm, isArming ? 1 : 0, isArming? magic_force_arm_value : magic_force_disarm_value, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, 3, cancel).ConfigureAwait(false);
                Logger.Info($"=> Ardupilot force ArmDisarm(isArming:{isArming}, force={force})): '{result.Result}'(porgress:{result.Progress};resultParam2:{result.ResultParam2})");
                ValidateCommandResult(result);
                await WaitCompleteWithDefaultTimeout(() => IsArmed.Value, "Arm/Disarm", cancel).ConfigureAwait(false);
            }
            else
            {
                await base.ArmDisarm(isArming, cancel).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// https://ardupilot.org/copter/docs/common-flight-time-recorder.html
        /// ArduPilot includes a flight time recorder which records the board’s total flight time, total run time and number of times the board has been rebooted. These are stored in user resettable parameters meaning they are not protected from being tampered with.
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public override async Task<FlightTimeStatistic> GetFlightTimeStatistic(CancellationToken cancel = default(CancellationToken))
        {
            var stat = new FlightTimeStatistic
            {
                BootCount = (await Params.ReadParam("STAT_BOOTCNT", DefaultAttemptToReadCount,cancel).ConfigureAwait(false)).IntegerValue.Value,
                FlightTime = TimeSpan.FromSeconds((double) (await Params.ReadParam("STAT_FLTTIME", DefaultAttemptToReadCount, cancel).ConfigureAwait(false)).IntegerValue),
                RunTime = TimeSpan.FromSeconds((double) (await Params.ReadParam("STAT_RUNTIME", DefaultAttemptToReadCount, cancel).ConfigureAwait(false)).IntegerValue),
            };
            return stat;
        }

        public override async Task ResetFlightStatistic(CancellationToken cancel)
        {
            var result = await Params.ReadParam("STAT_BOOTCNT", DefaultAttemptToReadCount, cancel).ConfigureAwait(false);
            result.IntegerValue = 0;
            await Params.WriteParam(result, DefaultAttemptToReadCount, cancel).ConfigureAwait(false);
        }

        

        public override async Task TakeOff(double altitude, CancellationToken cancel)
        {
            try
            {
                await base.TakeOff(altitude, cancel).ConfigureAwait(false);
            }
            catch (CommandException e)
            {
                if (e.Result.Result == MavResult.MavResultFailed)
                {
                    // IGNORE this. I don't know why, but the Ardupilot always send failed
                    return;
                }

                throw;
            }
            
        }

        protected override async Task<ushort> InternalReadSerialNumber(CancellationToken cancel)
        {
            //User-defined serial number of this vehicle, it can be any arbitrary number you want and has no effect on the autopilot
            var result = await Params.GetOrReadFromVehicleParam("BRD_SERIAL_NUM", cancel).ConfigureAwait(false);
            if (result.IntegerValue != null) return (ushort)(result.IntegerValue.Value + 32768);
            return 0;
        }

        protected override async Task InternalWriteSerialNumber(ushort serialNumber, CancellationToken cancel = default)
        {
            await Params.WriteParam("BRD_SERIAL_NUM", serialNumber, cancel).ConfigureAwait(false);
        }

        public async Task<ArdupilotFrameClassEnum> ReadFrameClass(CancellationToken cancel = default)
        {
            var frameClassParam = await Params.GetOrReadFromVehicleParam("FRAME_CLASS", cancel).ConfigureAwait(false);
            return ArdupilotFrameTypeHelper.ParseFrameClass(frameClassParam.IntegerValue);
        }

        public async Task<ArdupilotFrameTypeEnum> ReadFrameType(CancellationToken cancel = default)
        {
            var frameTypeParam = await Params.GetOrReadFromVehicleParam("FRAME_TYPE", cancel).ConfigureAwait(false);
            return ArdupilotFrameTypeHelper.ParseFrameType(frameTypeParam.IntegerValue);
        }

        protected override async Task<string> GetCustomName(CancellationToken cancel)
        {
            try
            {
                return ArdupilotFrameTypeHelper.GenerateName(await ReadFrameClass(cancel).ConfigureAwait(false),
                    await ReadFrameType(cancel).ConfigureAwait(false),
                    await ReadSerialNumber(cancel).ConfigureAwait(false));
            }
            catch (Exception e)
            {
                _logger.Error($"Error to read FRAME_TYPE or FRAME_CLASS or SerialNumber params:{e.Message}");
                return await base.GetCustomName(cancel).ConfigureAwait(false);
            }
        }
    }

    
}
