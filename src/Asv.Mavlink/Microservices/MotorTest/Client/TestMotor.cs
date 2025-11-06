using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using R3;

namespace Asv.Mavlink;

internal sealed class TestMotor : ITestMotor, IDisposable
{
	private const double ArduCopterTimeoutScale = 10.0;

	private readonly SerialDisposable _timerSubscription = new();
	private readonly ReactiveProperty<bool> _isRun;
	private readonly ICommandClient _commandClient;
	public int Id { get; }
	public int ServoChannel { get; }

	public TestMotor(int id, int servoChannel, Observable<ushort> pwm, ICommandClient commandClient)
	{
		_commandClient = commandClient ?? throw new ArgumentNullException(nameof(commandClient));
		Id = id;
		ServoChannel = servoChannel;
		Pwm = pwm.ToReadOnlyReactiveProperty();

		_isRun = new ReactiveProperty<bool>(false);
		IsTestRun = _isRun.ToReadOnlyReactiveProperty();
	}

	public ReadOnlyReactiveProperty<bool> IsTestRun { get; }
	public ReadOnlyReactiveProperty<ushort> Pwm { get; }

	public async Task<MavResult> StartTest(int throttleValue, int timeout, CancellationToken cancel = default)
	{
		var ack = await _commandClient.CommandLong(
				MavCmd.MavCmdDoMotorTest,
				Id,
				(float)MotorTestThrottleType.MotorTestThrottlePercent,
				throttleValue,
				(float)(ArduCopterTimeoutScale * timeout),
				0, 0, 0,
				cancel
			)
			.ConfigureAwait(false);

		if (ack.Result != MavResult.MavResultAccepted)
			return ack.Result;

		_isRun.Value = true;
		_timerSubscription.Disposable = Observable.Timer(TimeSpan.FromSeconds(timeout), cancel)
			.Subscribe(t => _isRun.Value = false);

		return ack.Result;
	}

	public async Task<MavResult> StopTest(CancellationToken cancel = default)
	{
		var ack = await _commandClient.CommandLong(
				MavCmd.MavCmdDoMotorTest,
				Id,
				(float)MotorTestThrottleType.MotorTestThrottlePercent,
				0,
				0,
				0, 0, 0,
				cancel
			)
			.ConfigureAwait(false);

		_isRun.Value = false;
		_timerSubscription.Dispose();

		return ack.Result;
	}

	public void Dispose()
	{
		Pwm.Dispose();
		IsTestRun.Dispose();
		_isRun.Dispose();
		_timerSubscription.Dispose();
	}
}