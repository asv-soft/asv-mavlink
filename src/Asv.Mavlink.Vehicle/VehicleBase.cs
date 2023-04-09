using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Client;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.Vehicle;
using DynamicData;
using NLog;

namespace Asv.Mavlink
{
    public class VehicleBaseConfig
    {
        public int HeartbeatTimeoutMs { get; set; } = 2000;
        public int CommandTimeoutMs { get; set; } = 10000;
        public int RequestInitDataDelayAfterFailMs { get; set; } = 5000;
    }

    public abstract class VehicleBase : DisposableOnceWithCancel, IVehicle
    {
        public static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMavlinkClient _mavlink;
        private readonly VehicleBaseConfig _config;
        private readonly RxValue<bool> _isMissionSynced = new();
        private readonly RxValue<double> _allMissionDistance = new();

        protected VehicleBase(IMavlinkClient mavlink, VehicleBaseConfig config, bool disposeClient)
        {
            _mavlink = mavlink;
            _config = config;
            if (disposeClient)
                Disposable.Add(_mavlink);
        }

        public IRxValue<double> PacketRateHz => _mavlink.Heartbeat.PacketRateHz;
        public IRxValue<double> LinkQuality => _mavlink.Heartbeat.LinkQuality;

        public IMavlinkClient Mavlink => _mavlink;

        public virtual void StartListen()
        {
            InitAttitude();
            InitRoi();
            InitGps();
            InitHome();
            InitArmed();
            InitBattery();
            InitAltitude();
            InitGoTo();
            InitStatus();
            InitMode();
            InitVehicleClass();
            InitRadioStatus();
            InitSerialNumber();
            InitAutopilotVersion();
            InitName();
            InitRequestVehicleInfo();
            InitMissions();
        }

        #region Name

        private void InitName()
        {
            Disposable.Add(_name);
        }

        protected virtual Task<string> GetCustomName(CancellationToken cancel) => Task.FromResult("Vehicle");

        private readonly RxValue<string> _name = new();
        public IRxValue<string> Name => _name;

        #endregion

        #region Vehicle class

        public IRxValue<VehicleClass> Class => _vehicleClass;
        private readonly RxValue<VehicleClass> _vehicleClass = new();
        private void InitVehicleClass()
        {
            Disposable.Add(_vehicleClass);
            _mavlink.Heartbeat.RawHeartbeat.Select(InterpretVehicleClass).Subscribe(_vehicleClass).DisposeItWith(Disposable);
        }

        protected abstract VehicleClass InterpretVehicleClass(HeartbeatPayload heartbeatPacket);

        #endregion

        #region Request init info

        private readonly RxValue<VehicleInitState> _initState = new();

        public ushort FullId => (ushort)(_mavlink.Identity.ComponentId | _mavlink.Identity.SystemId << 8);

        public MavlinkClientIdentity Identity => _mavlink.Identity;
        
        public IRxValue<VehicleInitState> InitState => _initState;

        private int _isRequestInfoIsInProgressOrAlreadySuccess;
        private bool _needToRequestAgain = true;
        private void InitRequestVehicleInfo()
        {
            _initState.OnNext(VehicleInitState.WaitConnection);
            Link.DistinctUntilChanged().Where(_ => _ == LinkState.Disconnected).Subscribe(_ => _needToRequestAgain = true).DisposeItWith(Disposable);
            Link.DistinctUntilChanged().Where(_ => _needToRequestAgain).Where(_ => _ == LinkState.Connected)
                // only one time
                .ObserveOn(Scheduler.Default).Subscribe(_ => TryToRequestData()).DisposeItWith(Disposable);
            Disposable.Add(_initState);
        }



        private async void TryToRequestData()
        {
            if (Interlocked.CompareExchange(ref _isRequestInfoIsInProgressOrAlreadySuccess,1,0) == 1) return;
            _initState.OnNext(VehicleInitState.InProgress);
            try
            {
                Logger.Info($"Request ALL data stream from vehicle");
                //await _mavlink.Common.RequestDataStream((int)MavDataStream.MavDataStreamAll, 0, false).DisposeItWith(Disposable);

                await _mavlink.Common.RequestDataStream((int)MavDataStream.MavDataStreamAll, 1, true, DisposeCancel).ConfigureAwait(false);

                await _mavlink.Common.RequestDataStream((int)MavDataStream.MavDataStreamExtendedStatus, 1, true, DisposeCancel).ConfigureAwait(false);

                await _mavlink.Common.RequestDataStream((int)MavDataStream.MavDataStreamPosition, 1, true, DisposeCancel).ConfigureAwait(false);
                // Logger.Info($"Request MavDataStreamRawSensors data stream from vehicle");
                //await _mavlink.Common.RequestDataStream((int)MavDataStream.MavDataStreamRawSensors, 3, true).DisposeItWith(Disposable);
                // Logger.Info($"Request MavDataStreamRawController data stream from vehicle");
                //await _mavlink.Common.RequestDataStream((int)MavDataStream.MavDataStreamRawController,2 , true).DisposeItWith(Disposable);
                // Logger.Info($"Request MavDataStreamPosition data stream from vehicle");
                // await _mavlink.Common.RequestDataStream((int)MavDataStream.MavDataStreamPosition, 2, true).DisposeItWith(Disposable);

                Logger.Info($"Request home position");

                await _mavlink.Commands.GetHomePosition(DisposeCancel).ConfigureAwait(false);

                await InternalRequestInitialDataAfterDisconnect(DisposeCancel).ConfigureAwait(false);

                await ReadSerialNumber(DisposeCancel).ConfigureAwait(false);

                //_autopilotVersion.OnNext(await _mavlink.Commands.GetAutopilotVersion(DisposeCancel).ConfigureAwait(false)); TODO: fix

                _name.OnNext(await GetCustomName(DisposeCancel).ConfigureAwait(false));

                //await _mavlink.Params.ReadAllParams(DisposeCancel,);
                _initState.OnNext(VehicleInitState.Complete);
                _needToRequestAgain = false;
            }
            catch (Exception e)
            {
                if (Disposable.IsDisposed) return; // no need to replay since the instance was already disposed
                Logger.Error( $"Error to read all vehicle info:{e.Message}");
                _initState.OnNext(VehicleInitState.Failed);
                Observable.Timer(TimeSpan.FromMilliseconds(_config.RequestInitDataDelayAfterFailMs))
                    .Subscribe(_ => TryToRequestData()).DisposeItWith(Disposable);
            }
            finally
            {
                Interlocked.Exchange(ref _isRequestInfoIsInProgressOrAlreadySuccess, 0);
            }
            
        }

        protected virtual Task InternalRequestInitialDataAfterDisconnect(CancellationToken cancel = default)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region Helpers

        protected async Task WaitCompleteWithDefaultTimeout(Func<bool> condition, string actionName, CancellationToken cancel)
        {
            var started = DateTime.Now;
            while (!condition())
            {
                await Task.Delay(500, cancel).ConfigureAwait(false);
                if ((DateTime.Now - started).Milliseconds > _config.CommandTimeoutMs)
                {
                    throw new TimeoutException(string.Format(RS.VehicleBase_WaitCompleteWithDefaultTimeout_Timeout_to_execute, actionName, TimeSpan.FromMilliseconds(_config.CommandTimeoutMs))); ;
                }
            }
        }

        protected void ValidateCommandResult(CommandAckPayload result)
        {
            switch (result.Result)
            {
                case MavResult.MavResultTemporarilyRejected:
                case MavResult.MavResultDenied:
                case MavResult.MavResultUnsupported:
                case MavResult.MavResultFailed:
                    throw new CommandException(result);
            }
        }

        #endregion

        #region Mode

        private readonly RxValue<VehicleMode> _mode = new();
        public abstract IEnumerable<VehicleCustomMode> AvailableModes { get; }
        public IRxValue<VehicleMode> Mode => _mode;

        public Task SetMode(VehicleMode mode, CancellationToken cancel)
        {
            return _mavlink.Common.SetMode((uint) mode.BaseMode, mode.CustomMode.Value, cancel);
        }

        public IMavlinkParameterClient Params => _mavlink.Params;

        protected abstract VehicleMode Interpret(HeartbeatPayload heartbeat);

        private void InitMode()
        {
            _mavlink.Heartbeat.RawHeartbeat.Select(Interpret).Subscribe(_mode).DisposeItWith(Disposable);
            Disposable.Add(_dropRateComm);
        }

        /// <summary>
        /// Check is current mode can used for remote control from GCS with GPS positioning. It depend from vehicle types 
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        protected abstract Task<bool> CheckGuidedMode(CancellationToken cancel);

        /// <summary>
        /// CheckGuidedMode and switch, if it not in guided mode. It depend from vehicle types 
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        protected abstract Task EnsureInGuidedMode(CancellationToken cancel);

        

        #endregion

        #region Attitude



        private readonly RxValue<double> _pitch = new();
        private readonly RxValue<double> _roll = new();
        private readonly RxValue<double> _yaw = new();
        private readonly RxValue<double> _pitchSpeed = new();
        private readonly RxValue<double> _rollSpeed = new();
        private readonly RxValue<double> _yawSpeed = new();


        public IRxValue<double> Pitch => _pitch;
        public IRxValue<double> Roll => _roll;
        public IRxValue<double> Yaw => _yaw;

        

        public IRxValue<double> PitchSpeed => _pitchSpeed;
        public IRxValue<double> RollSpeed => _rollSpeed;
        public IRxValue<double> YawSpeed => _yawSpeed;


        protected virtual void InitAttitude()
        {
            _mavlink.Rtt.RawAttitude.Select(_ => (double)GeoMath.RadiansToDegrees(_.Pitch)).Subscribe(_pitch).DisposeItWith(Disposable);
            _mavlink.Rtt.RawAttitude.Select(_ => (double) GeoMath.RadiansToDegrees(_.Roll)).Subscribe(_roll).DisposeItWith(Disposable);
            _mavlink.Rtt.RawAttitude.Select(_ => (double)GeoMath.RadiansToDegrees(_.Yaw)).Subscribe(_yaw).DisposeItWith(Disposable);
            _mavlink.Rtt.RawAttitude.Select(_ => (double)_.Pitchspeed).Subscribe(_pitchSpeed).DisposeItWith(Disposable);
            _mavlink.Rtt.RawAttitude.Select(_ => (double)_.Rollspeed).Subscribe(_rollSpeed).DisposeItWith(Disposable);
            _mavlink.Rtt.RawAttitude.Select(_ => (double)_.Yawspeed).Subscribe(_yawSpeed).DisposeItWith(Disposable);

            Disposable.Add(_pitch);
            Disposable.Add(_roll);
            Disposable.Add(_yaw);
            Disposable.Add(_pitchSpeed);
            Disposable.Add(_rollSpeed);
            Disposable.Add(_yawSpeed);


        }

        #endregion

        #region ROI

        public virtual async Task SetRoi(GeoPoint location, CancellationToken cancel)
        {
            Logger.Info($"=> SetRoi(location:{location.ToString()})");
            var res = await _mavlink.Commands.CommandLong(MavCmd.MavCmdDoSetRoi, (int)MavRoi.MavRoiLocation, 0, 0, 0, (float)location.Latitude, (float)location.Longitude, (float)location.Altitude, 3, CancellationToken.None).ConfigureAwait(false);
            Logger.Info($"<= SetRoi(location:{location.ToString()}): '{res.Result}'(porgress:{res.Progress};resultParam2:{res.ResultParam2})");
            ValidateCommandResult(res);
            _roi.OnNext(location);
        }

        protected virtual void InitRoi()
        {
            Disposable.Add(_roi);
        }

        protected readonly RxValue<GeoPoint?> _roi = new();
        public IRxValue<GeoPoint?> Roi => _roi;

        public virtual async Task ClearRoi(CancellationToken cancel)
        {
            Logger.Info($"=> ClearRoi()");
            var res = await _mavlink.Commands.CommandLong(MavCmd.MavCmdDoSetRoiNone, (int)MavRoi.MavRoiLocation, 0, 0, 0, 0, 0, 0, 3, CancellationToken.None).ConfigureAwait(false);
            Logger.Info($"<= ClearRoi(): '{res.Result}'(porgress:{res.Progress};resultParam2:{res.ResultParam2})");
            ValidateCommandResult(res);
            _roi.OnNext(null);
        }
     
        #endregion

        #region Link

        public IRxValue<LinkState> Link => _mavlink.Heartbeat.Link;

        #endregion

        #region Altitude

        private readonly RxValue<double> _altitudeRelative = new();
        public IRxValue<double> AltitudeAboveHome => _altitudeRelative;

        private void InitAltitude()
        {
            _mavlink.Rtt.RawGlobalPositionInt.Select(_=>_.RelativeAlt/1000D).Subscribe(_altitudeRelative).DisposeItWith(Disposable);
            Disposable.Add(_altitudeRelative);
        }

       

        #endregion

        #region GPS

        private readonly RxValue<GpsInfo>  _gpsInfo = new();
        private readonly RxValue<double> _gVelocity = new();
        private readonly RxValue<GeoPoint> _globalPosition = new();

        private readonly RxValue<GpsInfo> _gps2Info = new();
        private readonly RxValue<double> _g2Velocity = new();

        public IRxValue<GeoPoint> GlobalPosition => _globalPosition;

        public IRxValue<GpsInfo> GpsInfo => _gpsInfo;
        public IRxValue<double> GpsGroundVelocity => _gVelocity;

        
        public IRxValue<GpsInfo> Gps2Info => _gps2Info;
        public IRxValue<double> Gps2GroundVelocity => _g2Velocity;


        protected virtual void InitGps()
        {
            
            _mavlink.Rtt.RawGlobalPositionInt.Select(_ => new GeoPoint(_.Lat / 10000000D, _.Lon / 10000000D, _.Alt / 1000D)).Subscribe(_globalPosition).DisposeItWith(Disposable);
            Disposable.Add(_globalPosition);


            _mavlink.Rtt.RawGpsRawInt.IgnoreObserverExceptions().Select(_ => new GpsInfo(_)).Subscribe(_gpsInfo).DisposeItWith(Disposable);
            _mavlink.Rtt.RawGpsRawInt.Select(_ => _.Vel / 100D).Subscribe(_gVelocity).DisposeItWith(Disposable);
            Disposable.Add(_gVelocity);
            Disposable.Add(_gpsInfo);
            


            _mavlink.Rtt.RawGps2Raw.IgnoreObserverExceptions().Select(_ => new GpsInfo(_)).Subscribe(_gps2Info).DisposeItWith(Disposable);
            _mavlink.Rtt.RawGps2Raw.Select(_ => _.Vel / 100D).Subscribe(_g2Velocity).DisposeItWith(Disposable);
            Disposable.Add(_g2Velocity);
            Disposable.Add(_gps2Info);
        }

        #endregion

        #region Home

        private readonly RxValue<GeoPoint?> _home = new();
        private readonly RxValue<double?> _homeDistance = new();
        public IRxValue<GeoPoint?> Home => _home;
        public IRxValue<double?> HomeDistance => _homeDistance;
        protected virtual void InitHome()
        {
            _mavlink.Rtt.RawHome
                .Select(_ => (GeoPoint?) new GeoPoint(_.Latitude / 10000000D, _.Longitude / 10000000D, _.Altitude / 1000D))
                .Subscribe(_home).DisposeItWith(Disposable);
            Disposable.Add(_homeDistance);

            this.GlobalPosition
                .Where(_=>_home.Value.HasValue)
                // ReSharper disable once PossibleInvalidOperationException
                .Select(_ => (double?)GeoMath.Distance(_home.Value.Value,_))
                .Subscribe(_homeDistance).DisposeItWith(Disposable);
            Disposable.Add(_home);
        }

        #endregion

        #region Arm

        private readonly RxValue<bool> _isArmed = new();
        private readonly RxValue<TimeSpan> _armedTime = new();
        public Task RequestHome(CancellationToken cancel)
        {
            Logger.Info("=> Request home position from vehicle");
            return _mavlink.Commands.GetHomePosition(cancel);
        }

        public IRxValue<bool> IsArmed => _isArmed;
        public IRxValue<TimeSpan> ArmedTime => _armedTime;
        private long _lastArmedTime;
        protected virtual void InitArmed()
        {
            var timer = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).Where(_=>IsArmed.Value).Subscribe(_ =>
            {
                var lastBin = Interlocked.Read(ref _lastArmedTime);
                if (lastBin == 0)
                {
                    _armedTime.OnNext(TimeSpan.Zero);
                    return;
                }
                var last = DateTime.FromBinary(lastBin);
                var now = DateTime.Now;
                var delay = (now - last);
                _armedTime.OnNext(delay);
            });
            _isArmed.DistinctUntilChanged().Where(_ => _isArmed.Value).Subscribe(_ => Interlocked.Exchange(ref _lastArmedTime,DateTime.Now.ToBinary()) ).DisposeItWith(Disposable);
            Disposable.Add(timer);

            _mavlink.Heartbeat.RawHeartbeat.Select(_ => _.BaseMode.HasFlag(MavModeFlag.MavModeFlagSafetyArmed)).Subscribe(_isArmed).DisposeItWith(Disposable);
            Disposable.Add(_isArmed);
        }

        public virtual async Task ArmDisarm(bool isArming,CancellationToken cancel, bool force = false)
        {
            if (_isArmed.Value == isArming)
            {
                Logger.Info($"=> ArmDisarm(isArming:{isArming}, force={force}): already Armed\\Disarmed");
                return;
            }
            else
            {
                Logger.Info($"=> ArmDisarm(isArming:{isArming}, force={force})");
            }
            var result = await _mavlink.Commands.CommandLong(MavCmd.MavCmdComponentArmDisarm, isArming ? 1 : 0, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, 3, cancel).ConfigureAwait(false);
            Logger.Info($"=> ArmDisarm(isArming:{isArming}, force={force})): '{result.Result}'(porgress:{result.Progress};resultParam2:{result.ResultParam2})");
            ValidateCommandResult(result);
            await WaitCompleteWithDefaultTimeout(() => _isArmed.Value, "Arm/Disarm", cancel).ConfigureAwait(false);
        }

        #endregion

        #region Battery

        private readonly RxValue<double?> _batteryCharge = new();
        public IRxValue<double?> BatteryCharge => _batteryCharge;

        private readonly RxValue<double?> _currentBattery = new();
        public IRxValue<double?> CurrentBattery => _currentBattery;

        private readonly RxValue<double> _voltageBattery = new();
        public IRxValue<double> VoltageBattery => _voltageBattery;

        protected virtual void InitBattery()
        {
            // TODO: add _mavlink.Rtt.RawBatteryStatus

            _mavlink.Rtt.RawSysStatus.Select(_ => _.BatteryRemaining < 0 ? default(double?) : (_.BatteryRemaining / 100.0d)).Subscribe(_batteryCharge).DisposeItWith(Disposable);
            _mavlink.Rtt.RawSysStatus.Select(_ => _.CurrentBattery < 0 ? default(double?) : (_.CurrentBattery / 100.0d)).Subscribe(_currentBattery).DisposeItWith(Disposable);
            _mavlink.Rtt.RawSysStatus.Select(_ => _.VoltageBattery / 1000.0d).Subscribe(_voltageBattery).DisposeItWith(Disposable);

            Disposable.Add(_batteryCharge);
            Disposable.Add(_currentBattery);
            Disposable.Add(_voltageBattery);

        }

        #endregion

        #region Takeoff

        public virtual IEnumerable<VehicleParamDescription> GetParamDescription()
        {
            yield break;
        }

        public virtual async Task TakeOff(double altitude, CancellationToken cancel)
        {
            Logger.Info($"=> TakeOff(altitude:{altitude:F2})");
            await EnsureInGuidedMode(cancel).ConfigureAwait(false);
            await ArmDisarm(true, cancel).ConfigureAwait(false);
            var res = await _mavlink.Commands.CommandLong(MavCmd.MavCmdNavTakeoff, 0, 0, 0, 0, 0, 0, (float)altitude, 3, cancel).ConfigureAwait(false);
            Logger.Info($"<= TakeOff(altitude:{altitude:F2}): '{res.Result}'(porgress:{res.Progress};resultParam2:{res.ResultParam2})");
            ValidateCommandResult(res);
        }

        #endregion

        #region GoTo

        public async Task GoToGlob(GeoPoint location, CancellationToken cancel, double? yawDeg = null)
        {
            await EnsureInGuidedMode(cancel).ConfigureAwait(false);
          
            await InternalGoToGlob(location, cancel, yawDeg).ConfigureAwait(false);
            //_goToTarget.OnNext(location);
        }

        protected abstract Task InternalGoToGlob(GeoPoint location,CancellationToken cancel, double? yawDeg);

        public async Task GoToGlobAndWait(GeoPoint location, IProgress<double> progress, double precisionMet, CancellationToken cancel)
        {
            await GoToGlob(location, cancel).ConfigureAwait(false);
            progress = progress ?? new Progress<double>();
            var startLocation = this.GlobalPosition.Value;
            var startDistance = Math.Abs(GeoMath.Distance(location, startLocation));

            Logger.Info("GoToAndWait {0} with precision {2:F1} m. Distance to target {3:F1}", location, precisionMet, startDistance);
            progress.Report(0);
            if (startDistance <= precisionMet)
            {
                Logger.Debug("Already in target, nothing to do", startLocation);
                progress.Report(1);
                return;
            }

            var sw = new Stopwatch();
            sw.Start();
            Logger.Debug("Send command GoTo to vehicle", startLocation);
            await this.GoToGlob(location, cancel).ConfigureAwait(false);
            double dist = 0;
            while (!cancel.IsCancellationRequested)
            {
                var loc = this.GlobalPosition.Value;
                dist = Math.Abs(GeoMath.Distance(location, loc));
                var prog = 1 - dist / startDistance;
                Logger.Trace("Distance to target {0:F1}, location: {1}, progress {2:P2}", dist, loc, prog);
                progress.Report(prog);
                if (dist <= precisionMet) break;
                await Task.Delay(TimeSpan.FromSeconds(1), cancel).ConfigureAwait(false);
            }
            sw.Stop();
            Logger.Info($"Complete {sw.Elapsed:hh\\:mm\\:ss} location error {dist:F1} m");
            progress.Report(1);
        }

        public async Task GoToGlobAndWaitWithoutAltitude(GeoPoint location, IProgress<double> progress, double precisionMet, CancellationToken cancel)
        {
            await GoToGlob(location, cancel).ConfigureAwait(false);
            progress = progress ?? new Progress<double>();
            var startLocation = this.GlobalPosition.Value;
            var startLocation0 = startLocation.SetAltitude(0);
            var startDistance = Math.Abs(GeoMath.Distance(location, startLocation));
            var startDistance0 = Math.Abs(GeoMath.Distance(location.SetAltitude(0), startLocation0));

            Logger.Info("GoToAndWaitWithoutAltitude {0} with precision {2:F1} m. Distance to target {3:F1}", location, precisionMet, startDistance);
            progress.Report(0);
            if (startDistance0 <= precisionMet)
            {
                Logger.Debug("Already in target, nothing to do", startLocation);
                progress.Report(1);
                return;
            }

            var sw = new Stopwatch();
            sw.Start();
            Logger.Debug("Send command GoTo to vehicle", startLocation);
            await this.GoToGlob(location, cancel).ConfigureAwait(false);
            double dist = 0;
            while (!cancel.IsCancellationRequested)
            {
                var loc = this.GlobalPosition.Value;
                var loc0 = loc.SetAltitude(0);
                dist = Math.Abs(GeoMath.Distance(location, loc));
                var dist0 = Math.Abs(GeoMath.Distance(location.SetAltitude(0), loc0));
                var prog = 1 - dist / startDistance;
                Logger.Trace("Distance to target {0:F1}, location: {1}, progress {2:P2}", dist, loc, prog);
                progress.Report(prog);
                if (dist0 <= precisionMet) break;
                await Task.Delay(TimeSpan.FromSeconds(1), cancel).ConfigureAwait(false);
            }
            sw.Stop();
            Logger.Info($"Complete {sw.Elapsed:hh\\:mm\\:ss} location error {dist:F1} m");
            progress.Report(1);
        }

        public abstract Task FlyByLineGlob(GeoPoint start, GeoPoint stop, double precisionMet, CancellationToken cancel, Action firstPointComplete = null);
        public abstract Task DoLand(CancellationToken cancel);
        

        public abstract Task DoRtl(CancellationToken cancel);


        private readonly RxValue<GeoPoint?> _goToTarget = new();
        public IRxValue<GeoPoint?> GoToTarget => _goToTarget;

        private void InitGoTo()
        {
            _mavlink.Rtt.RawPositionTargetGlobalInt.Where(_ => _.CoordinateFrame == MavFrame.MavFrameGlobal)
                .Select(_ =>(GeoPoint?) new GeoPoint(_.LatInt / 10000000.0, _.LonInt / 10000000.0, _.Alt))
                .Subscribe(_goToTarget);
            
            Disposable.Add(_goToTarget);
        }





        #endregion

        #region Radiostatus

        public IRxValue<RadioLinkStatus> RadioStatus => _radioStatus;
        private readonly RxValue<RadioLinkStatus> _radioStatus = new();

        private void InitRadioStatus()
        {
            _mavlink.Rtt.RawRadioStatus.Select(_=>new RadioLinkStatus(_)).Subscribe(_radioStatus).DisposeItWith(Disposable);
            Disposable.Add(_radioStatus);
        }

        #endregion

        #region Status

        private readonly RxValue<VehicleStatusMessage> _textStatus = new();
        public IRxValue<VehicleStatusMessage> TextStatus => _textStatus;

        private readonly RxValue<double> _cpuLoad = new();
        public IRxValue<double> CpuLoad => _cpuLoad;

        private readonly RxValue<double> _dropRateComm = new();
        


        public IRxValue<double> DropRateCommunication => _dropRateComm;

        private void InitStatus()
        {
            _mavlink.Rtt.RawStatusText.Select(_ => new VehicleStatusMessage { Sender = Name?.Value, Text = new string(_.Text), Type = _.Severity  }).Subscribe(_textStatus).DisposeItWith(Disposable);
            _mavlink.Rtt.RawSysStatus.Select(_=>_.Load/1000D).Subscribe(_cpuLoad).DisposeItWith(Disposable);
            _mavlink.Rtt.RawSysStatus.Select(_ => _.DropRateComm / 1000D).Subscribe(_dropRateComm).DisposeItWith(Disposable);
            

            Disposable.Add(_textStatus);
            Disposable.Add(_cpuLoad);
            Disposable.Add(_dropRateComm);
        }

        #endregion

        #region Custom commands

        public async Task PreflightRebootShutdown(AutopilotRebootShutdown ardupilot, CompanionRebootShutdown companion, CancellationToken cancel)
        {
            Logger.Info($"=> PreflightRebootShutdown(Autopilot:{ardupilot:G}, Companion:{companion:G})");
            var res = await Mavlink.Commands.CommandLong(MavCmd.MavCmdPreflightRebootShutdown, (float)ardupilot, (float)companion, 0, 0, 0, 0, 0, 1, cancel).ConfigureAwait(false);
            Logger.Info($"<= PreflightRebootShutdown(Autopilot:{ardupilot:G}, Companion:{companion:G}): '{res.Result}'(porgress:{res.Progress};resultParam2:{res.ResultParam2})");
            ValidateCommandResult(res);
        }

        public abstract Task<FlightTimeStatistic> GetFlightTimeStatistic(CancellationToken cancel);

        public abstract Task ResetFlightStatistic(CancellationToken cancel);

        #endregion

        #region Fail safe

        public abstract IEnumerable<FailSafeInfo> AvailableFailSafe { get; }
        public abstract Task<FailSafeState[]> ReadFailSafe(CancellationToken cancel = default);
        public abstract Task WriteFailSafe(IReadOnlyDictionary<string, bool> values, CancellationToken cancel = default);

        #endregion

        #region User-defined serial number

        private readonly RxValue<ushort> _serialNumber = new();

        public IRxValue<ushort> SerialNumber => _serialNumber;


        private void InitSerialNumber()
        {
            Disposable.Add(_serialNumber);
        }

        public async Task<ushort> ReadSerialNumber(CancellationToken cancel = default)
        {
            var result = await InternalReadSerialNumber(cancel).ConfigureAwait(false);
            _serialNumber.OnNext(result);
            return result;
        }

        protected virtual Task<ushort> InternalReadSerialNumber(CancellationToken cancel)
        {
            return Task.FromResult((ushort)0);
        }

        public async Task<ushort> WriteSerialNumber(ushort serialNumber, CancellationToken cancel = default)
        {
            await InternalWriteSerialNumber(serialNumber, cancel).ConfigureAwait(false);
            return await ReadSerialNumber(cancel).ConfigureAwait(false);
        }

       

        protected abstract Task InternalWriteSerialNumber(ushort serialNumber, CancellationToken cancel);

        #endregion

        #region AutopilotVersion    

        private readonly RxValue<AutopilotVersionPayload> _autopilotVersion = new();
        

        public IRxValue<AutopilotVersionPayload> AutopilotVersion => _autopilotVersion;

        private void InitAutopilotVersion()
        {
            Disposable.Add(_autopilotVersion);
        }

        #endregion

        #region Missions

        private readonly SourceCache<MissionItem, ushort> _missionSource = new(_=>_.Index);

        protected abstract Task<bool> CheckAutoMode(CancellationToken cancel);
        protected abstract Task EnsureInAutoMode(CancellationToken cancel);

        private void InitMissions()
        {
            Disposable.Add(_missionSource);
            Disposable.Add(_isMissionSynced);
            Disposable.Add(_allMissionDistance);
            MissionItems.DisposeMany().Subscribe().DisposeItWith(Disposable); // for disposing, when remove from cache
            _isMissionSynced.Subscribe(_ => UpdateMissionsDistance()).DisposeItWith(Disposable);
            UpdateMissionsDistance();
        }

        public async Task SetCurrentMissionItem(ushort index, CancellationToken cancel = default)
        {
            Logger.Info($"{Name.Value}: Set current mission WP={index}");
            await EnsureInAutoMode(cancel).ConfigureAwait(false);
            await _mavlink.Mission.MissionSetCurrent(index, 3, cancel).ConfigureAwait(false);
        }

        public async Task<MissionItem[]> DownloadMission(int attemptCount, CancellationToken cancel, Action<double> progress)
        {
            Logger.Info($"{Name.Value}: Begin download mission");
            progress?.Invoke(0);
            var count = await _mavlink.Mission.MissionRequestCount(attemptCount, cancel).ConfigureAwait(false);
            var result = new MissionItem[count];
            _missionSource.Clear();
            var current = 0;
            for (ushort i = 0; i < count; i++)
            {
                var item = await _mavlink.Mission.MissionRequestItem(i, attemptCount, cancel).ConfigureAwait(false);
                result[i] = AddMissionItem(item);
                current++;
                progress?.Invoke((double)current / count);
            }
            _isMissionSynced.OnNext(true);
            return result;
        }

        public async Task ClearRemoteMission(int attemptCount, CancellationToken cancel)
        {
            Logger.Info($"{Name.Value}: Begin clear mission");
            _missionSource.Clear();
            await _mavlink.Mission.ClearAll(attemptCount, cancel).ConfigureAwait(false);
            _isMissionSynced.OnNext(true);
        }

        public async Task UploadMission(int attemptCount, CancellationToken cancel, Action<double> progress)
        {
            Logger.Info($"{Name.Value}: Begin upload mission");
            progress?.Invoke(0);
            await _mavlink.Mission.ClearAll(attemptCount, cancel).ConfigureAwait(false);

            using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
            var tcs = new TaskCompletionSource<Unit>();
            using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled());
            var current = 0;
            _mavlink.Mission.OnMissionRequest.Subscribe(req =>
            {
                Logger.Debug($"UAV request {req.Seq} item");
                current++;
                progress?.Invoke((double)(current) / _missionSource.Count);
                var item = _missionSource.Lookup(req.Seq);
                if (item.HasValue == false)
                {
                    tcs.TrySetException(new Exception($"Requested mission item with index '{req.Seq}' not found in local store"));
                    return;
                }
                _mavlink.Mission.WriteMissionItem(item.Value, cancel);

            } , cancel);

            _mavlink.Mission.OnMissionAck.Subscribe(_ =>
            {
                if (_.Type == MavMissionResult.MavMissionAccepted)
                {
                    tcs.TrySetResult(Unit.Default);
                }
                else
                {
                    tcs.TrySetException(new Exception($"Error to upload mission to vehicle:{_.Type:G}"));
                }
                
            } , cancel);

            await _mavlink.Mission.MissionSetCount((ushort)_missionSource.Count, cancel).ConfigureAwait(false);
            await tcs.Task.ConfigureAwait(false);
            _isMissionSynced.OnNext(true);
        }

        public MissionItem AddMissionItem()
        {
            return AddMissionItem(new MissionItemIntPayload
            {
                Seq = (ushort)_missionSource.Count,
                TargetComponent = Identity.ComponentId,
                TargetSystem = Identity.SystemId,
            });
        }


        private MissionItem AddMissionItem(MissionItemIntPayload item)
        {
            var missionItem = new MissionItem(item);
            _missionSource.AddOrUpdate(missionItem);
            missionItem.OnChanged.Subscribe(_ =>
            {
                _isMissionSynced.OnNext(false);
            }).DisposeItWith(Disposable);
            return missionItem;
        }

        public void RemoveMissionItem(ushort index)
        {
            _isMissionSynced.OnNext(false);
            _missionSource.RemoveKey(index);
        }

        public void CleatLocalMission()
        {
            _isMissionSynced.OnNext(false);
            _missionSource.Clear();
        }

        private void UpdateMissionsDistance()
        {
            var missions = _missionSource.Items.Where(_ =>
                    _.Command.Value == MavCmd.MavCmdNavWaypoint || _.Command.Value == MavCmd.MavCmdNavSplineWaypoint)
                .ToArray();
            var dist = 0.0;
            for (var i = 0; i < missions.Length - 1; i++)
            {
                dist += missions[i].Location.Value.DistanceTo(missions[i + 1].Location.Value);
            }
            _allMissionDistance.OnNext(dist / 1000.0);
        }

        public IRxValue<double> AllMissionsDistance => _allMissionDistance;
        public IRxValue<bool> IsMissionSynced => _isMissionSynced;
        public IObservable<IChangeSet<MissionItem, ushort>> MissionItems => _missionSource.Connect().Publish().RefCount();
        public IRxValue<ushort> MissionCurrent => _mavlink.Mission.MissionCurrent;
        public IRxValue<ushort> MissionReached => _mavlink.Mission.MissionReached;

        #endregion
    }
    
}
