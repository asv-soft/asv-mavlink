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
        private const int DeltaXy = 10;
        private const int DeltaZ = 5;
        private const int RedialDegRight = 90;
        private const int RedialDegLeft = 70;
        private const int RedialDegDown = 0;
        private const int RedialDegUp = 180;
        private const float TakeOffDelta = 50f;
        private const float PageDelta = 1.0f;
        
        private readonly CancellationTokenSource _cancel = new();
        private readonly KeyListener _keyListener;
        private readonly TaskCompletionSource _exitTcs = new();
        
        private ArduCopterClientDevice? _device;
        private IDeviceExplorer _deviceExplorer;
        private IDisposable? _stateSub;
        private IDisposable? _intervalSub;
        private VehicleStateRenderer? _renderer;
        private ArduCopterControlClient _controlClient;
        private ParamsClient _paramsClient;
        private PositionClient _positionClient; 

        public PrintVehicleStateCommand()
        {
            _keyListener = new KeyListener();
            _keyListener.OnKeyPressedAsync += KeyPressedAsync;
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
            _controlClient = _device?.GetMicroservice<IControlClient>() as ArduCopterControlClient;
            _paramsClient = _device?.GetMicroservice<IParamsClient>() as ParamsClient;
            _positionClient = _device?.GetMicroservice<IPositionClient>() as PositionClient;

            if (_controlClient == null || _paramsClient == null || _positionClient == null)
            {
                throw new InvalidOperationException("One or more required microservices are missing");
            }
        }
        
        /// <summary>
        /// Vehicle state real time monitoring
        /// </summary>
        /// <param name="connection">-c, Connection string. Default "tcp://127.0.0.1:7341"</param>
        [Command("print-vehicle-state")]
        public async Task<int> Run(string connection = "tcp://127.0.0.1:7341")
        {
            try
            {
                ShellCommandsHelper.CreateDeviceExplorer(connection, out _deviceExplorer);

                _device = await GetCompatibleDeviceAsync();
                if (_device is null) return 0;

                try
                {
                    InitMicroservices();
                }
                catch (InvalidOperationException ex)
                {
                    AnsiConsole.MarkupLine($"[red]Exception: {ex.Message}[/]");
                    return -1;
                }
                
                var tcs = new CancellationToken();
                var telemetryProvider = new VehicleTelemetryProvider(_device);
                _renderer = new VehicleStateRenderer(telemetryProvider, tcs);

                _renderer.Print();
                var displayTask = _renderer.DisplayTableAsync(_cancel.Token);

                _intervalSub = Observable
                    .Interval(TimeSpan.FromSeconds(1))
                    .TakeWhile(_ => !_cancel.Token.IsCancellationRequested)
                    .Subscribe(_ => _renderer.Print());

                var tcs1 = new TaskCompletionSource();
                _stateSub = _device.State
                    .Where(state => state != ClientDeviceState.Uninitialized)
                    .Subscribe(_ => tcs1.TrySetResult());
                await tcs1.Task;

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
        /// Continuously waits for and attempts to retrieve a compatible <see cref="ArduCopterClientDevice"/> from the device explorer.
        /// Displays a message and exits gracefully if no compatible device is found or the user chooses to cancel.
        /// </summary>
        /// <returns>
        /// A task that returns a compatible <see cref="ArduCopterClientDevice"/> if available; otherwise, <c>null</c>.
        /// </returns>
        private async Task<ArduCopterClientDevice?> GetCompatibleDeviceAsync()
        {
            while (_device is null)
            {
                var device = await ShellCommandsHelper.DeviceAwaiter(_deviceExplorer, 3000);
                if (device is null)
                {
                    _renderer?.ShowMessage("Program interrupted. Exiting gracefully...",
                        VehicleStateRenderer.MessageType.Warning);
                    return null;
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
        /// Retrieves the current global position of the vehicle from the position client.
        /// </summary>
        /// <returns>
        /// A <see cref="GeoPoint"/> representing the current latitude, longitude, and relative altitude.
        /// Returns <see cref="GeoPoint.Zero"/> if the position data is unavailable.
        /// </returns>
        private GeoPoint GetCurrentPosition()
        {
            var pos = _positionClient?.GlobalPosition.CurrentValue;
            if (pos == null) return GeoPoint.Zero;
            return new GeoPoint(pos.Lat, pos.Lon, pos.RelativeAlt); // или Alt (MSL), по ситуации
        }

        /// <summary>
        /// Handles vehicle control logic based on the specified keyboard direction input.
        /// Performs actions such as directional movement, altitude change, parameter tuning, takeoff, or cancellation.
        /// </summary>
        /// <param name="direction">The direction or command input from the user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task KeyPressedAsync(VehicleDirection direction)
        {
            object? param = null;

            switch (direction)
            {
                case VehicleDirection.Right:
                    await _controlClient.GoTo(GetCurrentPosition().RadialPoint(DeltaXy, RedialDegRight), _cancel.Token);
                    break;

                case VehicleDirection.Left:
                    await _controlClient.GoTo(GetCurrentPosition().RadialPoint(DeltaXy, RedialDegLeft), _cancel.Token);
                    break;

                case VehicleDirection.Up:
                    await _controlClient.GoTo(GetCurrentPosition().RadialPoint(DeltaXy, RedialDegUp), _cancel.Token);
                    break;

                case VehicleDirection.Down:
                    await _controlClient.GoTo(GetCurrentPosition().RadialPoint(DeltaXy, RedialDegDown), _cancel.Token);
                    break;

                case VehicleDirection.U:
                    var pUp = GetCurrentPosition();
                    param = pUp.Altitude;
                    await _controlClient.GoTo(pUp.AddAltitude(DeltaZ), _cancel.Token);
                    break;

                case VehicleDirection.D:
                    var pDown = GetCurrentPosition();
                    param = pDown.Altitude;
                    await _controlClient.GoTo(pDown.AddAltitude(-DeltaZ), _cancel.Token);
                    break;

                case VehicleDirection.PageUp:
                    var valueUp = await _paramsClient.Read("MPC_XY_VEL_MAX", CancellationToken.None);
                    if (valueUp is not null)
                    {
                        param = valueUp;
                        await _paramsClient.Write("MPC_XY_VEL_MAX", MavParamType.MavParamTypeReal32, valueUp.ParamValue + PageDelta, _cancel.Token);
                    }
                    break;

                case VehicleDirection.PageDown:
                    var valueDown = await _paramsClient.Read("MPC_XY_VEL_MAX", CancellationToken.None);
                    if (valueDown is not null)
                    {
                        param = valueDown;
                        await _paramsClient.Write("MPC_XY_VEL_MAX", MavParamType.MavParamTypeReal32, valueDown.ParamValue - PageDelta, _cancel.Token);
                    }
                    break;

                case VehicleDirection.Q:
                    _cancel.Cancel(false);        
                    _exitTcs.TrySetResult();       
                    break;

                case VehicleDirection.T:
                    await _controlClient.SetGuidedMode();
                    await _controlClient.TakeOff(GetCurrentPosition().Altitude + TakeOffDelta, CancellationToken.None);
                    break;
            }

            _renderer?.WriteJournal(direction, param);
        }

        #region IDisposable Implementation
        public void Dispose()
        {
            _stateSub?.Dispose();
            _intervalSub?.Dispose();
            _cancel?.Dispose();
            
            _keyListener.OnKeyPressedAsync -= KeyPressedAsync;
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
