using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using DotNext.Collections.Generic;
using Newtonsoft.Json;
using ObservableCollections;
using R3;

namespace Asv.Mavlink;

public class ArduCopterMotorTestClient : MavlinkMicroserviceClient, IMotorTestClient
{
	private static string FrameClassParam => "FRAME_CLASS";
	private static string FrameTypeParam => "FRAME_TYPE";

	private readonly ICommandClient _commandClient;
	private readonly IParamsClientEx _paramsClientEx;
	private readonly CompositeDisposable _disposable = new();
	private readonly ObservableList<ITestMotor> _testMotors = [];

	private ArduCopterVehicle _vehicle = ArduCopterVehicle.CreateUnknownVehicle();

	public ArduCopterMotorTestClient(MavlinkClientIdentity identity,
		IMavlinkContext core, ICommandClient commandClient, IParamsClientEx paramsClientEx)
		: base("MOTOR_TEST", identity, core)
	{
		_commandClient = commandClient ?? throw new ArgumentNullException(nameof(commandClient));
		_paramsClientEx = paramsClientEx ?? throw new ArgumentNullException(nameof(paramsClientEx));

		paramsClientEx
			.Filter(FrameTypeParam)
			.CombineLatest(paramsClientEx.Filter(FrameClassParam),
				(frameType, frameClass) => ArduCopterVehicle.CreateVehicle(frameClass, frameType))
			.Subscribe(vehicle =>
			{
				var motors = GetMotors(vehicle);
				_testMotors.Clear();
				_testMotors.AddRange(motors);

				_disposable.Clear();
				_disposable.AddAll(motors);
			});
	}

	public IReadOnlyObservableList<ITestMotor> TestMotors => _testMotors;

	protected override async Task InternalInit(CancellationToken cancel)
	{
		await base.InternalInit(cancel).ConfigureAwait(false);
		_vehicle = await GetVehicle(cancel).ConfigureAwait(false);
	}

	private static Layout? FindFrameLayout(ArduCopterVehicle vehicle)
	{
		try
		{
			var config = GetMotorsMatrixConfig();
			return config.Layouts.FirstOrDefault(layout =>
				layout.Class == (int)vehicle.FrameClass && layout.Type == (int)vehicle.FrameType);
		}
		catch (Exception ex)
		{
			throw new ArduCopterMotorsMatrixConfigException("ArduCopter motors matrix configuration is not provided", ex);
		}
	}

	private static ArduCopterMotorsMatrixConfig GetMotorsMatrixConfig()
	{
		var file = Path.GetDirectoryName(Path.GetFullPath(Assembly.GetExecutingAssembly().Location)) + Path.DirectorySeparatorChar +
			"APMotorLayout.json";
		using var streamReader = new StreamReader(file);
		var json = streamReader.ReadToEnd();
		var motorsMatrixConfig = JsonConvert.DeserializeObject<ArduCopterMotorsMatrixConfig>(json);
		return motorsMatrixConfig ?? throw new NullReferenceException();
	}

	private async Task<ArduCopterVehicle> GetVehicle(CancellationToken cancel)
	{
		var frameClass = await _paramsClientEx.ReadOnce(FrameClassParam, cancel).ConfigureAwait(false);
		var frameType = await _paramsClientEx.ReadOnce(FrameTypeParam, cancel).ConfigureAwait(false);

		return ArduCopterVehicle.CreateVehicle(frameClass, frameType);
	}

	private List<TestMotor> GetMotors(ArduCopterVehicle vehicle)
	{
		var layout = FindFrameLayout(vehicle);
		if (layout is null)
			throw new UnsupportedVehicleException("Unsupported ArduCopter frame");

		var motors = new List<TestMotor>();

		foreach (var motorDef in layout.Motors)
		{
			var pwm = InternalFilter<ServoOutputRawPacket>()
				.Select(packet => GetPwm(packet.Payload, motorDef.Number));

			var motor = new TestMotor(motorDef.TestOrder, motorDef.Number, pwm, _commandClient);
			motors.Add(motor);
		}

		return motors;
	}

	private ushort GetPwm(ServoOutputRawPayload message, int servoOutput)
	{
		return servoOutput switch
		{
			1 => message.Servo1Raw,
			2 => message.Servo2Raw,
			3 => message.Servo3Raw,
			4 => message.Servo4Raw,
			5 => message.Servo5Raw,
			6 => message.Servo6Raw,
			7 => message.Servo7Raw,
			8 => message.Servo8Raw,
			9 => message.Servo9Raw,
			10 => message.Servo10Raw,
			11 => message.Servo11Raw,
			12 => message.Servo12Raw,
			13 => message.Servo13Raw,
			14 => message.Servo14Raw,
			15 => message.Servo15Raw,
			16 => message.Servo16Raw,
			_ => throw new ArgumentOutOfRangeException(nameof(servoOutput), servoOutput, null)
		};
	}

	#region Dispose

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_disposable.Dispose();
		}

		base.Dispose(disposing);
	}

	protected override async ValueTask DisposeAsyncCore()
	{
		await CastAndDispose(_disposable).ConfigureAwait(false);

		await base.DisposeAsyncCore().ConfigureAwait(false);

		return;

		static async ValueTask CastAndDispose(IDisposable resource)
		{
			if (resource is IAsyncDisposable resourceAsyncDisposable)
				await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
			else
				resource.Dispose();
		}
	}

	#endregion
}