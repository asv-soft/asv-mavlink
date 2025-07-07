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
        private VehicleStateRenderer? _renderer;
        private IParamsClient _paramsClient;
        private IPositionClient _positionClient;
        private IControlClient _controlClient;
        private IModeClient _mode;
        private ReadOnlyReactiveProperty<GeoPoint> _dronePosition;
        private Func<VehicleAction, Task>? _directionHandler;

        public PrintVehicleStateCommand()
        {
            _keyListener = new KeyListener();
        }
        
        /// <summary>
        /// Vehicle state real time monitoring
        /// </summary>
        /// <param name="connection">-c, Connection string. Default "tcp://127.0.0.1:7341"</param>
        /// <param name="refreshRate">-r, Refresh rate in milliseconds. Default 3000ms</param>
        [Command("print-vehicle-state")]
        public async Task<int> Run(string connection, uint refreshRate = 3000)
        {
            try
            {
                ShellCommandsHelper.CreateDeviceExplorer(connection, out _deviceExplorer);

                _device = await GetCompatibleDeviceAsync(refreshRate);
                if (_device is null) return 0;
                
                var tcs1 = new TaskCompletionSource();
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await using var registration = timeoutCts.Token.Register(() => tcs1.TrySetCanceled());

                _stateSub = _device.State
                    .Where(state => state == ClientDeviceState.Complete)
                    .Subscribe(_ => tcs1.TrySetResult());

                await tcs1.Task;
                
                InitMicroservices();
                
                using var cts = new CancellationTokenSource();
                _renderer = new VehicleStateRenderer(_device, cts.Token);

                _renderer.UpdateTable();
                var displayTask = _renderer.DisplayTableAsync(_cancel.Token);
                
                _intervalSub = Observable
                    .Interval(TimeSpan.FromSeconds(1))
                    .TakeWhile(_ => !_cancel.Token.IsCancellationRequested)
                    .Subscribe(_ => _renderer.UpdateTable());
                
                _directionHandler = async direction => await KeyPressedAsync(direction);
                _keyListener.OnDirectionPressed += _directionHandler;
                _keyListener.ListenAsync().SafeFireAndForget();
                
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
            if (_device is null)
            {
                throw new InvalidOperationException("Device cannot be null.");
            }

            _controlClient = _device.GetMicroservice<IControlClient>();
            _paramsClient = _device.GetMicroservice<IParamsClient>();
            _positionClient = _device.GetMicroservice<IPositionClient>();
            _mode = _device.GetMicroservice<IModeClient>();
            
            if (_paramsClient is null || _positionClient is null || _controlClient is null || _mode is null)
            {
                throw new InvalidOperationException("One or more required microservices are missing.");
            }
            
            _dronePosition = _positionClient.GlobalPosition
                .WhereNotNull()
                .Select(pos => MavlinkTypesHelper.FromInt32ToGeoPoint(pos.Lat, pos.Lon, pos.RelativeAlt))
                .ToReadOnlyReactiveProperty();
        }
        
        /// <summary>
        /// Continuously waits for and attempts to retrieve a compatible <see cref="IClientDevice"/> from the device explorer.
        /// Displays a message and exits gracefully if no compatible device is found or the user chooses to cancel.
        /// </summary>
        /// <returns>
        /// A task that returns a compatible <see cref="IClientDevice"/> if available; otherwise, <c>null</c>.
        /// </returns>
        private async Task<IClientDevice?> GetCompatibleDeviceAsync(uint refreshRate)        
        {
            while (_device is null)
            {
                var device = await ShellCommandsHelper.DeviceAwaiter(_deviceExplorer, refreshRate);
                _device = device as ArduCopterClientDevice ?? throw new InvalidOperationException("No device found");
                
                if (_device is not null) return _device;

                AnsiConsole.Clear();
                AnsiConsole.WriteLine("This command available only to MavType = 2 devices");
                AnsiConsole.WriteLine("Press R to repeat or any key to exit");

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.R) continue;

                return null;
            }

            return _device;
        }
        
        /// <summary>
        /// Handles vehicle control logic based on the specified keyboard direction input.
        /// Performs actions such as directional movement, altitude change, parameter tuning, takeoff, or cancellation.
        /// </summary>
        /// <param name="action">The action or command input from the user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task KeyPressedAsync(VehicleAction action)
        {
            var commandInfo = new VehicleStateRenderer.CommandInfo { Direction = action };

            switch (action)
            {
                case VehicleAction.GoRight:
                    await _controlClient.GoTo(
                        _dronePosition.CurrentValue.RadialPoint(TelemetryParams.DeltaXy, TelemetryParams.RedialDegRight),
                        _cancel.Token);
                    break;

                case VehicleAction.GoLeft:
                    await _controlClient.GoTo(
                        _dronePosition.CurrentValue.RadialPoint(TelemetryParams.DeltaXy, TelemetryParams.RedialDegLeft),
                        _cancel.Token);
                    break;

                case VehicleAction.GoUp:
                    await _controlClient.GoTo(
                        _dronePosition.CurrentValue.RadialPoint(TelemetryParams.DeltaXy, TelemetryParams.RedialDegUp),
                        _cancel.Token);
                    break;

                case VehicleAction.GoDown:
                    await _controlClient.GoTo(
                        _dronePosition.CurrentValue.RadialPoint(TelemetryParams.DeltaXy, TelemetryParams.RedialDegDown),
                        _cancel.Token);
                    break;

                case VehicleAction.Upward:
                    var currentUp = _dronePosition.CurrentValue;
                    commandInfo.AltitudeBeforeChange = currentUp.Altitude;
                    await _controlClient.GoTo(currentUp.AddAltitude(TelemetryParams.DeltaZ), _cancel.Token);
                    break;

                case VehicleAction.Downward:
                    var currentDown = _dronePosition.CurrentValue;
                    commandInfo.AltitudeBeforeChange = currentDown.Altitude;
                    await _controlClient.GoTo(currentDown.AddAltitude(-TelemetryParams.DeltaZ), _cancel.Token);
                    break;

                case VehicleAction.PageUp:
                    var valueUp = await _paramsClient.Read(TelemetryParams.VelocityMaxParamName, _cancel.Token);
                        commandInfo.VelocityParam = valueUp.ParamValue;
                        await _paramsClient.Write(
                            TelemetryParams.VelocityMaxParamName,
                            MavParamType.MavParamTypeReal32,
                            valueUp.ParamValue + TelemetryParams.PageDelta,
                            _cancel.Token);
                    break;

                case VehicleAction.PageDown:
                    var valueDown = await _paramsClient.Read(TelemetryParams.VelocityMaxParamName, _cancel.Token);
                        commandInfo.VelocityParam = valueDown.ParamValue;
                        await _paramsClient.Write(
                            TelemetryParams.VelocityMaxParamName,
                            MavParamType.MavParamTypeReal32,
                            valueDown.ParamValue - TelemetryParams.PageDelta,
                            _cancel.Token);
                    break;

                case VehicleAction.Quit:
                    _cancel.Cancel(false);
                    _exitTcs.TrySetResult();
                    _keyListener.Stop();
                    break;

                case VehicleAction.TakeOff:
                {
                    await _controlClient.SetGuidedMode(_cancel.Token);
    
                    using var sub = _mode.CurrentMode
                        .Where(mode => mode == ArduCopterMode.Guided)
                        .Take(1)
                        .SubscribeAwait(async (_, ct) => await _controlClient.TakeOff(
                            _dronePosition.CurrentValue.Altitude + TelemetryParams.TakeOffDelta,
                            _cancel.Token));

                    break;
                }
            }
            _renderer?.WriteJournal(commandInfo);
        }
        
        #region IDisposable Implementation
        public void Dispose()
        {
            if (_directionHandler != null)
                _keyListener.OnDirectionPressed -= _directionHandler;
            
            _stateSub?.Dispose();
            _intervalSub?.Dispose();
            _cancel.Dispose();
            
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    #region VehicleAction
    public enum VehicleAction
    {
        GoLeft, 
        GoRight,
        GoDown,
        GoUp,
        Upward,
        Downward,
        PageUp,
        PageDown,   
        Quit,
        TakeOff
    }
    #endregion
}

public static class TaskExtensions
{
    public static void SafeFireAndForget(this Task task, Action<Exception>? onException = null)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                onException?.Invoke(ex);
                throw;
            }
        });
    }
}
