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
        private readonly CancellationTokenSource _cancel;
        private readonly TaskCompletionSource _exitTcs;
        private readonly KeyListener _keyListener;
        
        private IClientDevice _device;
        private IDeviceExplorer _deviceExplorer;
        private VehicleStateRenderer _renderer;
        private IParamsClient _paramsClient;
        private IPositionClient _positionClient;
        private IControlClient _controlClient;
        private IModeClient _mode;
        private ReadOnlyReactiveProperty<GeoPoint> _dronePosition;

        public PrintVehicleStateCommand()
        { 
            _cancel = new CancellationTokenSource(); 
            _exitTcs = new TaskCompletionSource();
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

                _device = await GetCompatibleDeviceAsync(refreshRate) ?? throw new Exception("No device found");

                _controlClient = _device.GetMicroservice<IControlClient>() ??
                                 throw new Exception($"{nameof(IControlClient)} was not found");
                _paramsClient = _device.GetMicroservice<IParamsClient>() ??
                                throw new Exception($"{nameof(IParamsClient)} was not found");
                _positionClient = _device.GetMicroservice<IPositionClient>() ??
                                  throw new Exception($"{nameof(IPositionClient)} was not found");
                _mode = _device.GetMicroservice<IModeClient>() ??
                        throw new Exception($"{nameof(IModeClient)} was not found");

                _dronePosition = _positionClient.GlobalPosition
                    .WhereNotNull()
                    .Select(pos => MavlinkTypesHelper.FromInt32ToGeoPoint(pos.Lat, pos.Lon, pos.Alt))
                    .ToReadOnlyReactiveProperty();

                _renderer = new VehicleStateRenderer(_device);

                var displayTask = _renderer.DisplayTableAsync(_cancel.Token);

                using var sub =
                    _keyListener.KeyPressed.SubscribeAwait(async (key, _) => await KeyPressedAsync(key, _cancel.Token));
                _keyListener.ListenAsync(_cancel.Token).SafeFireAndForget();

                await Task.WhenAll(
                    _exitTcs.Task,
                    displayTask
                );

                return 0;
            }
            catch (TaskCanceledException)
            {
                AnsiConsole.MarkupLine("[blue]info:[/] The program was stopped");
                return 1;
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
        /// Continuously waits for and attempts to retrieve a compatible <see cref="IClientDevice"/> from the device explorer.
        /// Displays a message and exits gracefully if no compatible device is found or the user chooses to cancel.
        /// </summary>
        /// <returns>
        /// A task that returns a compatible <see cref="IClientDevice"/> if available; otherwise, <c>null</c>.
        /// </returns>
        private async Task<IClientDevice?> GetCompatibleDeviceAsync(uint refreshRate)
        {
            ArduCopterClientDevice? device = null;
            while (device is null)
            {
                device = await ShellCommandsHelper.DeviceAwaiter(_deviceExplorer, refreshRate) as ArduCopterClientDevice;

                if (device is not null)
                {
                    break;
                }

                AnsiConsole.Clear();
                AnsiConsole.WriteLine("This command available only to MavType = 2 devices");
                AnsiConsole.WriteLine("Press R to repeat or any key to exit");
                
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.R)
                {
                    continue;
                }
                
                return null;
            }

            return device;
        }

        /// <summary>
        /// Handles vehicle control logic based on the specified keyboard direction input.
        /// Performs actions such as directional movement, altitude change, parameter tuning, takeoff, or cancellation.
        /// </summary>
        /// <param name="action">The action or command input from the user.</param>
        /// <param name="cancel">.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async ValueTask KeyPressedAsync(VehicleAction action, CancellationToken cancel)
        {
            cancel.ThrowIfCancellationRequested();
            var commandInfo = new VehicleStateRenderer.CommandInfo { Action = action };

            switch (action)
            {
                case VehicleAction.GoRight:
                    var newPositionRight = _dronePosition.CurrentValue.RadialPoint(TelemetryParams.DeltaXy, TelemetryParams.RedialDegRight);
                    await _controlClient.GoTo(newPositionRight, cancel);
                    break;

                case VehicleAction.GoLeft:
                    var newPositionLeft = _dronePosition.CurrentValue.RadialPoint(TelemetryParams.DeltaXy, TelemetryParams.RedialDegLeft);
                    await _controlClient.GoTo(newPositionLeft, cancel);
                    break;

                case VehicleAction.GoForward:
                    var newPositionForward = _dronePosition.CurrentValue.RadialPoint(TelemetryParams.DeltaXy, TelemetryParams.RedialDegForward);
                    await _controlClient.GoTo(newPositionForward, cancel);
                    break;

                case VehicleAction.GoBackwards:
                    var newPositionBackward = _dronePosition.CurrentValue.RadialPoint(TelemetryParams.DeltaXy, TelemetryParams.RedialDegBackwards);
                    await _controlClient.GoTo(newPositionBackward, cancel);
                    break;

                case VehicleAction.Upward:
                    var currentUp = _dronePosition.CurrentValue;
                    commandInfo.AltitudeBeforeChange = currentUp.Altitude;
                    var newPositionUp = currentUp.AddAltitude(TelemetryParams.DeltaZ);
                    await _controlClient.GoTo(newPositionUp, cancel);
                    break;

                case VehicleAction.Downward:
                    var currentDown = _dronePosition.CurrentValue;
                    commandInfo.AltitudeBeforeChange = currentDown.Altitude;
                    var newPositionDown = currentDown.AddAltitude(-TelemetryParams.DeltaZ);
                    if (_positionClient.Home.CurrentValue?.Altitude >= newPositionDown.Altitude)
                    {
                        await _controlClient.DoLand(cancel);
                        break;
                    }
                    
                    await _controlClient.GoTo(newPositionDown, cancel);
                    break;

                case VehicleAction.PageUp: //TODO: fix
                    var valueUp = await _paramsClient.Read(TelemetryParams.VelocityMaxParamName, cancel);
                        commandInfo.VelocityParam = valueUp.ParamValue;
                        await _paramsClient.Write(
                            TelemetryParams.VelocityMaxParamName,
                            MavParamType.MavParamTypeReal32,
                            valueUp.ParamValue + TelemetryParams.PageDelta,
                            cancel);
                    break;

                case VehicleAction.PageDown: //TODO: fix
                    var valueDown = await _paramsClient.Read(TelemetryParams.VelocityMaxParamName, cancel);
                        commandInfo.VelocityParam = valueDown.ParamValue;
                        await _paramsClient.Write(
                            TelemetryParams.VelocityMaxParamName,
                            MavParamType.MavParamTypeReal32,
                            valueDown.ParamValue - TelemetryParams.PageDelta,
                            cancel);
                    break;

                case VehicleAction.Quit:
                    _exitTcs.TrySetResult();
                    _cancel.Cancel(false);
                    break;

                case VehicleAction.TakeOff:
                {
                    await _controlClient.SetGuidedMode(cancel);

                    if (_mode.CurrentMode.CurrentValue == ArduCopterMode.Guided)
                    {
                        await _controlClient.TakeOff(TelemetryParams.TakeOffDelta, cancel);
                    }
                    
                    break;
                }
                
                case VehicleAction.Unknown:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
            
            _renderer.WriteJournal(commandInfo);
        }
        
        #region Dispose
        public void Dispose()
        {
            _cancel.Dispose();
            _keyListener.Dispose();
            
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
