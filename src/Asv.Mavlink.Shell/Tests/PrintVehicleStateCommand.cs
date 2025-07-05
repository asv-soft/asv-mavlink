using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Common;
using ConsoleAppFramework;
using R3;
using Spectre.Console;


namespace Asv.Mavlink.Shell
{
    public class PrintVehicleStateCommand : IDisposable
    {
        private readonly CancellationTokenSource _cancel = new();
        private readonly TaskCompletionSource _exitTcs = new();
        private readonly KeyListener _keyListener;
        
        private IClientDevice? _device;
        private IDeviceExplorer _deviceExplorer;
        private IDisposable? _stateSub;
        private IDisposable? _intervalSub;
        private IDisposable? _keySub;
        private VehicleStateRenderer? _renderer;
        private VehicleTelemetryProvider? _telemetryProvider;
        private IParamsClient _paramsClient;
        private IPositionClient _positionClient;
        private IControlClient _controlClient;
        private IModeClient _mode;
        private ReadOnlyReactiveProperty<GeoPoint> _dronePosition;

        public PrintVehicleStateCommand()
        {
            _keyListener = new KeyListener();
        }
        
        /// <summary>
        /// Vehicle state real time monitoring
        /// </summary>
        /// <param name="connection">-c, Connection string. Default "tcp://127.0.0.1:7341"</param>
        [Command("print-vehicle-state")]
        public async Task<int> Run(string connection)
        {
            try
            {
                ShellCommandsHelper.CreateDeviceExplorer(connection, out _deviceExplorer);

                _device = await GetCompatibleDeviceAsync();
                if (_device is null) return 0;
                
                var tcs1 = new TaskCompletionSource();
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                using var registration = timeoutCts.Token.Register(() => tcs1.TrySetCanceled());

                _stateSub = _device.State
                    .Where(state => state != ClientDeviceState.Uninitialized)
                    .Subscribe(_ => tcs1.TrySetResult());

                await tcs1.Task;
                
                InitMicroservices();
                
                var cts = new CancellationTokenSource();
                _telemetryProvider = new VehicleTelemetryProvider(_device);
                _renderer = new VehicleStateRenderer(_telemetryProvider, cts.Token);

                _renderer.Print();
                var displayTask = _renderer.DisplayTableAsync(_cancel.Token);

                _intervalSub = R3.Observable
                    .Interval(TimeSpan.FromSeconds(1))
                    .TakeWhile(_ => !_cancel.Token.IsCancellationRequested)
                    .Subscribe(_ => _renderer.Print());

                _keySub = _keyListener.DirectionStream
                    .Subscribe(async direction => await KeyPressedAsync(direction));
                
                _ = _keyListener.ListenAsync();

                await Task.WhenAll(
                    _exitTcs.Task,
                    displayTask
                );
                
                return 0;
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return -1;
            }
            finally
            {
                Dispose();
            }
        }
        
        /// <summary>
        /// Initializes required microservice clients (control, parameters, and position) from the connected device.
        /// Throws an exception if any of the required microservices are not available or incompatible.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if one or more required microservices are missing or cannot be cast to the expected types.
        /// </exception>
        private void InitMicroservices()
        {
            _controlClient = _device?.GetMicroservice<IControlClient>();
            _paramsClient = _device?.GetMicroservice<IParamsClient>();
            _positionClient = _device?.GetMicroservice<IPositionClient>();
            _mode = _device?.GetMicroservice<IModeClient>();
            
            if (_paramsClient is null || _positionClient is null || _controlClient is null || _mode is null)
            {
                throw new InvalidOperationException("One or more required microservices are missing");
            }

            _dronePosition = _positionClient.GlobalPosition
                .WhereNotNull()
                .Select(pos => new GeoPoint(pos.Lat, pos.Lon, pos.RelativeAlt))
                .ToReadOnlyReactiveProperty();
        }


        /// <summary>
        /// Continuously waits for and attempts to retrieve a compatible <see cref="IClientDevice"/> from the device explorer.
        /// Displays a message and exits gracefully if no compatible device is found or the user chooses to cancel.
        /// </summary>
        /// <returns>
        /// A task that returns a compatible <see cref="IClientDevice"/> if available; otherwise, <c>null</c>.
        /// </returns>
        private async Task<IClientDevice?> GetCompatibleDeviceAsync()
        {
            while (_device is null)
            {
                var device = await ShellCommandsHelper.DeviceAwaiter(_deviceExplorer, 3000);
                if (device is null)
                {
                    throw new InvalidOperationException("No device found");
                }

                _device = device as ArduCopterClientDevice;
                if (_device is not null) return _device;

                AnsiConsole.Clear();
                AnsiConsole.WriteLine("This command available only to MavType = 2 devices");
                AnsiConsole.WriteLine("Press R to repeat or any key to exit");

                var key = await _keyListener.WaitForKeyAsync([ConsoleKey.R]);
                if (key == ConsoleKey.R) continue;

                return null;
            }

            return _device;
        }
        
        /// <summary>
        /// Handles vehicle control logic based on the specified keyboard direction input.
        /// Performs actions such as directional movement, altitude change, parameter tuning, takeoff, or cancellation.
        /// </summary>
        /// <param name="direction">The direction or command input from the user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task KeyPressedAsync(VehicleDirection direction)
        {
            var commandInfo = new VehicleStateRenderer.CommandInfo { Direction = direction };

            switch (direction)
            {
                case VehicleDirection.Right:
                    await _controlClient.GoTo(
                        _dronePosition.CurrentValue.RadialPoint(_telemetryProvider.DeltaXyValue, _telemetryProvider.RedialRight),
                        _cancel.Token);
                    break;

                case VehicleDirection.Left:
                    await _controlClient.GoTo(
                        _dronePosition.CurrentValue.RadialPoint(_telemetryProvider.DeltaXyValue, _telemetryProvider.RedialLeft),
                        _cancel.Token);
                    break;

                case VehicleDirection.Up:
                    await _controlClient.GoTo(
                        _dronePosition.CurrentValue.RadialPoint(_telemetryProvider.DeltaXyValue, _telemetryProvider.RedialUp),
                        _cancel.Token);
                    break;

                case VehicleDirection.Down:
                    await _controlClient.GoTo(
                        _dronePosition.CurrentValue.RadialPoint(_telemetryProvider.DeltaXyValue, _telemetryProvider.RedialDown),
                        _cancel.Token);
                    break;

                case VehicleDirection.U:
                    var currentUp = _dronePosition.CurrentValue;
                    commandInfo.AltitudeBeforeChange = currentUp.Altitude;
                    await _controlClient.GoTo(currentUp.AddAltitude(_telemetryProvider.DeltaZValue), _cancel.Token);
                    break;

                case VehicleDirection.D:
                    var currentDown = _dronePosition.CurrentValue;
                    commandInfo.AltitudeBeforeChange = currentDown.Altitude;
                    await _controlClient.GoTo(currentDown.AddAltitude(-_telemetryProvider.DeltaZValue), _cancel.Token);
                    break;

                case VehicleDirection.PageUp:
                    var valueUp = await _paramsClient.Read(_telemetryProvider.VelocityMaxParam, _cancel.Token);
                    if (valueUp is not null)
                    {
                        commandInfo.VelocityParam = valueUp.ParamValue;
                        await _paramsClient.Write(
                            _telemetryProvider.VelocityMaxParam,
                            MavParamType.MavParamTypeReal32,
                            valueUp.ParamValue + _telemetryProvider.VelocityPageStep,
                            _cancel.Token);
                    }
                    break;

                case VehicleDirection.PageDown:
                    var valueDown = await _paramsClient.Read(_telemetryProvider.VelocityMaxParam, _cancel.Token);
                    if (valueDown is not null)
                    {
                        commandInfo.VelocityParam = valueDown.ParamValue;
                        await _paramsClient.Write(
                            _telemetryProvider.VelocityMaxParam,
                            MavParamType.MavParamTypeReal32,
                            valueDown.ParamValue - _telemetryProvider.VelocityPageStep,
                            _cancel.Token);
                    }
                    break;

                case VehicleDirection.Q:
                    _cancel.Cancel(false);
                    _exitTcs.TrySetResult();
                    _keyListener.Stop();
                    break;

                case VehicleDirection.T:
                {
                    await _controlClient.SetGuidedMode(_cancel.Token);
    
                    using var sub = _mode.CurrentMode
                        .Where(mode => mode == ArduCopterMode.Guided)
                        .Take(1)
                        .SubscribeAwait(async (_, ct) => await _controlClient.TakeOff(
                            _dronePosition.CurrentValue.Altitude + _telemetryProvider.TakeOffAltitudeDelta,
                            _cancel.Token));

                    break;
                }
            }
            _renderer?.WriteJournal(commandInfo);
        }
        
        #region IDisposable Implementation
        public void Dispose()
        {
            _stateSub?.Dispose();
            _intervalSub?.Dispose();
            _cancel?.Dispose();
            _keySub?.Dispose();
        }
        #endregion
    }

    #region VehicleDirection
    public enum VehicleDirection
    {
        Left, 
        Right,
        Down,
        Up,
        U,
        D,
        PageUp,
        PageDown,   
        Q,
        T
    }
    #endregion
}
