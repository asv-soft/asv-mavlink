using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using DotNext.Collections.Generic;
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

	private readonly IDisposable _testMotorsSubscription;

	public ArduCopterMotorTestClient(IHeartbeatClient heartbeatClient, ICommandClient commandClient, IParamsClientEx paramsClientEx)
		: base("MOTOR_TEST", heartbeatClient.Identity, heartbeatClient.Core)
	{
		_commandClient = commandClient ?? throw new ArgumentNullException(nameof(commandClient));
		_paramsClientEx = paramsClientEx ?? throw new ArgumentNullException(nameof(paramsClientEx));

		_testMotorsSubscription = paramsClientEx
			.Filter(FrameTypeParam)
			.CombineLatest(paramsClientEx.Filter(FrameClassParam),
				(frameType, frameClass) => new ArduCopterFrame(frameClass, frameType))
			.Subscribe(frame =>
			{
				var motors = CreateTestMotors(frame);
				_testMotors.Clear();
				_testMotors.AddRange(motors);

				_disposable.Clear();
				_disposable.AddAll(motors);
			});
	}

	public IReadOnlyObservableList<ITestMotor> TestMotors => _testMotors;

	public async Task Refresh(CancellationToken cancellationToken = default)
	{
		await _paramsClientEx.ReadOnce(FrameClassParam, cancellationToken).ConfigureAwait(false);
		await _paramsClientEx.ReadOnce(FrameTypeParam, cancellationToken).ConfigureAwait(false);
	}

	protected override async Task InternalInit(CancellationToken cancel)
	{
		await base.InternalInit(cancel).ConfigureAwait(false);
		await Refresh(cancel).ConfigureAwait(false);
	}

	private List<TestMotor> CreateTestMotors(ArduCopterFrame frame)
	{
		var motorsLayout = ArduPilotMotorsLayout.Layouts.FirstOrDefault(layout =>
			layout.Class == (int)frame.FrameClass && layout.Type == (int?)frame.FrameType);
		
		if (motorsLayout is null)
			throw new UnsupportedVehicleException("Unsupported ArduCopter frame");

		var motors = new List<TestMotor>();

		foreach (var motor in motorsLayout.Motors)
		{
			var pwm = InternalFilter<ServoOutputRawPacket>()
				.Select(packet => GetPwm(packet.Payload, motor.Number));

			var testMotor = new TestMotor(motor.TestOrder, motor.Number, pwm, _commandClient);
			motors.Add(testMotor);
		}

		return motors;
	}

	private static ushort GetPwm(ServoOutputRawPayload message, int servoOutput)
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
			_testMotorsSubscription.Dispose();
		}

		base.Dispose(disposing);
	}

	protected override async ValueTask DisposeAsyncCore()
	{
		await CastAndDispose(_disposable).ConfigureAwait(false);
		await CastAndDispose(_testMotorsSubscription).ConfigureAwait(false);

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

	private class ArduCopterFrame
	{
		public ArduCopterFrame(int frameClass, int frameType)
		{
			FrameClass = (ArduFrameClass)frameClass;
			FrameType = (ArduFrameType)frameType;
		}

		public ArduFrameClass FrameClass { get; }
		public ArduFrameType FrameType { get; }
	}
}