using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using DotNext.Collections.Generic;
using ObservableCollections;
using R3;

namespace Asv.Mavlink.MotorTest;

public abstract class ArduMotorTestClient : MotorTestClient
{
	private readonly IParamsClientEx _paramsClientEx;
	private readonly ICommandClient _commandClient;
	private readonly CompositeDisposable _disposable = new();
	private readonly ObservableList<ITestMotor> _testMotors = [];
	private readonly SerialDisposable _testMotorsSubscription = new();
	private readonly ArduMotorTestTimer _arduMotorTestTimer = new();

	private ArduFrame _frame;

	protected abstract string FrameClassParam { get; }
	protected abstract string FrameTypeParam { get; }

	protected ArduMotorTestClient(IHeartbeatClient heartbeat,
		ICommandClient commandClient,
		IParamsClientEx paramsClientEx)
		: base(heartbeat.Identity, heartbeat.Core)
	{
		_commandClient = commandClient ?? throw new ArgumentNullException(nameof(commandClient));
		_paramsClientEx = paramsClientEx ?? throw new ArgumentNullException(nameof(paramsClientEx));
		
		_frame = new ArduFrame((int)ArduFrameClass.Undefined, null);
	}

	public override IReadOnlyObservableList<ITestMotor> TestMotors => _testMotors;

	public override async Task Refresh(CancellationToken cancellationToken = default)
	{
		await _paramsClientEx.ReadOnce(FrameClassParam, cancellationToken).ConfigureAwait(false);
		await _paramsClientEx.ReadOnce(FrameTypeParam, cancellationToken).ConfigureAwait(false);
	}

	protected override Task Initialize(CancellationToken token = default)
	{
		_testMotorsSubscription.Disposable = _paramsClientEx
			.Filter(FrameTypeParam)
			.CombineLatest(_paramsClientEx.Filter(FrameClassParam),
				(frameType, frameClass) => new ArduFrame(frameClass, frameType))
			.Where(frame => frame != _frame)
			.Subscribe(frame =>
			{
				_frame = frame;
				var motors = CreateTestMotors(frame);
				_testMotors.Clear();
				_testMotors.AddRange(motors);

				_disposable.Clear();
				_disposable.AddAll(motors);
			});

		return Task.CompletedTask;
	}

	private List<ArduTestMotor> CreateTestMotors(ArduFrame frame)
	{
		var motorsLayout = ArduPilotMotorsLayout.Layouts.FirstOrDefault(layout =>
			layout.Class == frame.FrameClass && layout.Type == frame.FrameType);

		if (motorsLayout is null)
			throw new UnsupportedVehicleException("Unsupported ArduCopter frame");

		var motors = new List<ArduTestMotor>();

		foreach (var motor in motorsLayout.Motors)
		{
			var pwm = InternalFilter<ServoOutputRawPacket>()
				.Select(packet => GetPwm(packet.Payload, motor.Number));

			var testMotor = new ArduTestMotor(motor.TestOrder, motor.Number, pwm, _commandClient, _arduMotorTestTimer);
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

	private record ArduFrame(int FrameClass, int? FrameType);
}