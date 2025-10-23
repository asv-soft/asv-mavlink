using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using Newtonsoft.Json;
using R3;

namespace Asv.Mavlink;

public class ArduCopterMotorTestClient : MavlinkMicroserviceClient, IMotorTestClient
{
	private readonly ICommandClient _commandClient;
	private readonly IParamsClient _paramsClient;

	public ArduCopterMotorTestClient(ICommandClient commandClient, IParamsClient paramsClient, MavlinkClientIdentity identity,
		IMavlinkContext core)
		: base("MOTOR_TEST", identity, core)
	{
		_commandClient = commandClient ?? throw new ArgumentNullException(nameof(commandClient));
		_paramsClient = paramsClient ?? throw new ArgumentNullException(nameof(paramsClient));
	}

	public async Task RunTelemetry()
	{
		var frameClassPayload = await _paramsClient.Read("FRAME_CLASS").ConfigureAwait(false);
		var frameTypePayload = await _paramsClient.Read("FRAME_TYPE").ConfigureAwait(false);
		var layout = FindFrameLayout((int)frameClassPayload.ParamValue, (int)frameTypePayload.ParamValue);

		var motorServos = await GetMotorServos().ConfigureAwait(false);
		var motorCount = motorServos.Count;
		var motorsByServo = new Dictionary<int, int>();
		var isCustomMotorsLayout = layout is null;
		if (!isCustomMotorsLayout)
		{
			motorsByServo = layout!.Motors.ToDictionary(motor => motor.Number, motor => motor.TestOrder);
			motorCount = layout.Motors.Count();
		}

		var servoChannelLimitPayload = await _paramsClient.Read("SERVO_32_ENABLE").ConfigureAwait(false);
		var isServo32Enabled = servoChannelLimitPayload.ParamValue != 0f;
		if (isServo32Enabled)
			throw new NotImplementedException();

		MotorsTelemetry = InternalFilter<ServoOutputRawPacket>()
			.Select(p => p.Payload)
			.Select(payload =>
				new MotorsTelemetry
				{
					MotorCount = motorCount,
					MotorPwms = motorServos.Select(servo => new MotorPwm
					{
						Id = motorsByServo.GetValueOrDefault(servo, 0),
						Servo = servo,
						Pwm = GetPwm(payload, servo)
					})
				})
			!.ToReadOnlyReactiveProperty();
	}

	public async Task<MavResult> StartMotor(int motor, int throttleValue, int timeout, CancellationToken cancel = default)
	{
		var ack = await _commandClient.CommandLong(
				MavCmd.MavCmdDoMotorTest,
				motor,
				(float)MotorTestThrottleType.MotorTestThrottlePercent,
				throttleValue,
				10 * (float)timeout,
				0,
				0, // MOTOR_TEST_ORDER_BOARD	Motor numbers are specified as the output as labeled on the board. 
				0,
				cancel
			)
			.ConfigureAwait(false);

		return ack.Result;
	}

	public async Task<MavResult> StopMotor(int motor, CancellationToken cancel = default)
	{
		var ack = await _commandClient.CommandLong(
				MavCmd.MavCmdDoMotorTest,
				motor,
				(float)MotorTestThrottleType.MotorTestThrottlePercent,
				0,
				0,
				0, 0, 0,
				cancel
			)
			.ConfigureAwait(false);

		return ack.Result;
	}

	public ReadOnlyReactiveProperty<MotorsTelemetry> MotorsTelemetry { get; private set; }

	public Observable<MotorPwm> GetMotorPwm(int motor)
	{
		//TODO: Create or take from cash Observable for motor id
		throw new NotImplementedException();
	}

	protected override async Task InternalInit(CancellationToken cancel)
	{
		await base.InternalInit(cancel).ConfigureAwait(false);
		await RunTelemetry().ConfigureAwait(false);
	}

	private Layout? FindFrameLayout(int frameClass, int frameType)
	{
		var file = Path.GetDirectoryName(Path.GetFullPath(Assembly.GetExecutingAssembly().Location)) + Path.DirectorySeparatorChar +
			"APMotorLayout.json";
		using var streamReader = new StreamReader(file);
		var json = streamReader.ReadToEnd();
		var motorsMatrixConfig = JsonConvert.DeserializeObject<ArduCopterMotorsMatrixConfig>(json);

		return motorsMatrixConfig.Layouts.FirstOrDefault(layout => layout.Class == frameClass && layout.Type == frameType);
	}

	private async Task<List<int>> GetMotorServos()
	{
		var arduPilotServoChannelsLimit = 16;

		var servoChannelLimitPayload = await _paramsClient.Read("SERVO_32_ENABLE").ConfigureAwait(false);
		var isServo32Enabled = servoChannelLimitPayload.ParamValue != 0f;
		if (isServo32Enabled)
			arduPilotServoChannelsLimit = 32;

		var tasks = Enumerable.Range(1, arduPilotServoChannelsLimit).Select(GetServoFunction);
		var servoFunctions = await Task.WhenAll(tasks).ConfigureAwait(false);

		return servoFunctions
			.Where(IsMotorServoFunction)
			.Select(GetMotorByServoFunction)
			.ToList();
	}

	private async Task<int> GetServoFunction(int servoChannel)
	{
		var servoFunctionParameter = $"SERVO{servoChannel}_FUNCTION";
		var payload = await _paramsClient.Read(servoFunctionParameter).ConfigureAwait(false);

		var servoFunction = (int)payload.ParamValue;
		return servoFunction;
	}

	private async Task<bool> IsServo32Enabled()
	{
		var payload = await _paramsClient.Read("SERVO_32_ENABLE").ConfigureAwait(false);
		return payload.ParamValue != 0f;
	}

	private bool IsMotorServoFunction(int servoFunction) => ServoChannelFunction.MotorsByFunction.ContainsKey(servoFunction);
	private int GetMotorByServoFunction(int servoFunction) => ServoChannelFunction.MotorsByFunction[servoFunction];

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
			MotorsTelemetry.Dispose();
		}

		base.Dispose(disposing);
	}

	protected override async ValueTask DisposeAsyncCore()
	{
		await CastAndDispose(MotorsTelemetry).ConfigureAwait(false);

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