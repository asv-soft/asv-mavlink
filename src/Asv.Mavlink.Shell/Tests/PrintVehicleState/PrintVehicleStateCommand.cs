using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Common;
using ConsoleAppFramework;
using R3;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class PrintVehicleStateCommand
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private CancellationTokenSource _cancel;
    private TaskCompletionSource _exitTcs;
    private KeyListener _keyListener;
    private IClientDevice _device;
    private IDeviceExplorer _deviceExplorer;
    private IParamsClient _paramsClient;
    private IPositionClient _positionClient;
    private IControlClient _controlClient;
    private IModeClient _mode;
    private ReadOnlyReactiveProperty<GeoPoint> _dronePosition;
    private VehicleStateRenderable _vehicleRenderable;
    private LogRenderable _logRenderable;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// Vehicle state real-time monitoring and control
    /// </summary>
    /// <param name="connection">-c, Connection string</param>
    /// <param name="refreshRate">-r, (in ms) States how fast the console should be refreshed</param>
    [Command("print-vehicle-state")]
    public async Task<int> Run(string connection, uint refreshRate = 3000)
    {
        try
        {
            _cancel = new CancellationTokenSource();
            _exitTcs = new TaskCompletionSource();
            _keyListener = new KeyListener();
            
            ShellCommandsHelper.CreateDeviceExplorer(connection, out _deviceExplorer);

            _device = await GetCompatibleDeviceAsync(refreshRate) ?? throw new Exception("No device found");

            _controlClient = _device.GetMicroservice<IControlClient>() ?? throw new Exception($"{nameof(IControlClient)} was not found");
            _paramsClient = _device.GetMicroservice<IParamsClient>() ?? throw new Exception($"{nameof(IParamsClient)} was not found");
            _positionClient = _device.GetMicroservice<IPositionClient>() ?? throw new Exception($"{nameof(IPositionClient)} was not found");
            _mode = _device.GetMicroservice<IModeClient>() ?? throw new Exception($"{nameof(IModeClient)} was not found");

            _dronePosition = _positionClient.GlobalPosition
                .WhereNotNull()
                .Select(pos => MavlinkTypesHelper.FromInt32ToGeoPoint(pos.Lat, pos.Lon, pos.Alt))
                .ToReadOnlyReactiveProperty();

            _vehicleRenderable = new VehicleStateRenderable(_device);
            _logRenderable = new LogRenderable();

            var ui = new UiRenderer(_vehicleRenderable, _logRenderable);
            var uiTask = ui.RunAsync(_cancel.Token);

            await using var sub = _keyListener.KeyPressed.SubscribeAwait(KeyPressedAsync).RegisterTo(_cancel.Token);
            _keyListener.ListenAsync(_cancel.Token).SafeFireAndForget();

            await Task.WhenAll(_exitTcs.Task, uiTask);

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
            _cancel.Dispose();
            _keyListener.Dispose();
        }
    }

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
            AnsiConsole.WriteLine("This command available only to \'MavType = 2\' devices");
            AnsiConsole.WriteLine("Press R to repeat or Q to exit");

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.R) continue;

            return null;
        }

        return device;
    }

    private async ValueTask KeyPressedAsync(VehicleAction action, CancellationToken cancel)
    {
        cancel.ThrowIfCancellationRequested();

        var commandInfo = new CommandInfo { Action = action };

        switch (action)
        {
            case VehicleAction.GoRight:
                var newPositionRight = _dronePosition.CurrentValue.RadialPoint(
                    TelemetryParams.DeltaXy, TelemetryParams.RedialDegRight);
                await _controlClient.GoTo(newPositionRight, cancel);
                break;

            case VehicleAction.GoLeft:
                var newPositionLeft = _dronePosition.CurrentValue.RadialPoint(
                    TelemetryParams.DeltaXy, TelemetryParams.RedialDegLeft);
                await _controlClient.GoTo(newPositionLeft, cancel);
                break;

            case VehicleAction.GoForward:
                var newPositionForward = _dronePosition.CurrentValue.RadialPoint(
                    TelemetryParams.DeltaXy, TelemetryParams.RedialDegForward);
                await _controlClient.GoTo(newPositionForward, cancel);
                break;

            case VehicleAction.GoBackwards:
                var newPositionBackward = _dronePosition.CurrentValue.RadialPoint(
                    TelemetryParams.DeltaXy, TelemetryParams.RedialDegBackwards);
                await _controlClient.GoTo(newPositionBackward, cancel);
                break;

            case VehicleAction.Upward:
                commandInfo.AltitudeBeforeChange = _dronePosition.CurrentValue.Altitude;
                var newPositionUp = _dronePosition.CurrentValue.AddAltitude(TelemetryParams.DeltaZ);
                await _controlClient.GoTo(newPositionUp, cancel);
                break;

            case VehicleAction.Downward:
                commandInfo.AltitudeBeforeChange = _dronePosition.CurrentValue.Altitude;
                var newPositionDown = _dronePosition.CurrentValue.AddAltitude(-TelemetryParams.DeltaZ);
                
                if (_positionClient.Home.CurrentValue?.Altitude >= newPositionDown.Altitude)
                {
                    await _controlClient.DoLand(cancel);
                    break;
                }

                await _controlClient.GoTo(newPositionDown, cancel);
                break;

            case VehicleAction.PageUp:
                var valueUp = await _paramsClient.Read(TelemetryParams.VelocityMaxParamName, cancel);
                commandInfo.VelocityParam = valueUp.ParamValue;
                await _paramsClient.Write(
                    TelemetryParams.VelocityMaxParamName, 
                    MavParamType.MavParamTypeReal32, 
                    Math.Clamp(valueUp.ParamValue + TelemetryParams.VelocityDelta, 100f, 3000f), 
                    cancel); 
                break;

            case VehicleAction.PageDown:
                var valueDown = await _paramsClient.Read(TelemetryParams.VelocityMaxParamName, cancel);
                commandInfo.VelocityParam = valueDown.ParamValue;
                await _paramsClient.Write(
                    TelemetryParams.VelocityMaxParamName, 
                    MavParamType.MavParamTypeReal32, 
                    Math.Clamp(valueDown.ParamValue - TelemetryParams.VelocityDelta, 100f, 3000f), 
                    cancel);
                break;

            case VehicleAction.TakeOff:
                await _controlClient.SetGuidedMode(cancel);
                
                if (_mode.CurrentMode.CurrentValue == ArduCopterMode.Guided)
                {
                    await _controlClient.TakeOff(TelemetryParams.TakeOffDelta, cancel);
                }
                
                break;

            case VehicleAction.Quit:
                _exitTcs.TrySetResult();
                _cancel.Cancel(false);
                break;

            case VehicleAction.Unknown:
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }

        var journal = commandInfo.ToJournalString();
        _vehicleRenderable.WriteJournal(journal);
        _logRenderable.Add(journal);
    }
}
