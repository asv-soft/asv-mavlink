using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using R3;

namespace Asv.Mavlink;

internal sealed class TestMotor : ITestMotor, IDisposable
{
	private readonly IMotorTestRunner _motorTestRunner;
	private readonly ReactiveProperty<bool> _isRun;
	private readonly IDisposable _testStateSubscription;

	public int Id { get; }
	public int ServoChannel { get; }

	public TestMotor(int id, int servoChannel, Observable<ushort> pwm, IMotorTestRunner motorTestRunner)
	{
		_motorTestRunner = motorTestRunner;

		Id = id;
		ServoChannel = servoChannel;
		Pwm = pwm.ToReadOnlyReactiveProperty();
		_isRun = new ReactiveProperty<bool>(false);
		IsTestRun = _isRun.ToReadOnlyReactiveProperty();

		_testStateSubscription = _motorTestRunner.IsTestRun
			.Where(state => !state)
			.Merge(_isRun)
			.Subscribe(state => _isRun.Value = state);
	}

	public ReadOnlyReactiveProperty<bool> IsTestRun { get; }
	public ReadOnlyReactiveProperty<ushort> Pwm { get; }

	public async Task<MavResult> StartTest(int throttleValue, int timeout, CancellationToken cancel = default)
	{
		var ack = await _motorTestRunner.StartTest(Id, throttleValue, timeout, cancel).ConfigureAwait(false);

		_isRun.Value = true;

		return ack;
	}

	public async Task<MavResult> StopTest(CancellationToken cancel = default)
	{
		var ack = await _motorTestRunner.StopTest(Id, cancel).ConfigureAwait(false);

		return ack;
	}

	public void Dispose()
	{
		Pwm.Dispose();
		IsTestRun.Dispose();
		_isRun.Dispose();
		_testStateSubscription.Dispose();
	}
}