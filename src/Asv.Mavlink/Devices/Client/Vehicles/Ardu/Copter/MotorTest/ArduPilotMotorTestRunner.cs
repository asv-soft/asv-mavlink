using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using R3;

namespace Asv.Mavlink;

internal class ArduPilotMotorTestRunner : IMotorTestRunner, IDisposable
{
	private const double ArduCopterTimeoutScale = 10.0;

	private readonly SerialDisposable _timerSubscription = new();
	private readonly ReactiveProperty<bool> _isRun;
	private readonly ICommandClient _commandClient;
	
	public ArduPilotMotorTestRunner(ICommandClient commandClient)
	{
		_commandClient = commandClient;
		
		_isRun = new ReactiveProperty<bool>(false);
		IsTestRun = _isRun.ToReadOnlyReactiveProperty();
	}
	
	public ReadOnlyReactiveProperty<bool> IsTestRun { get; }


	public async Task<MavResult> StartTest(int motorId, int throttleValue, int timeout, CancellationToken cancel = default)
	{
		return await RunMotorTest(motorId, throttleValue, timeout, cancel).ConfigureAwait(false);
	}

	private async Task<MavResult> RunMotorTest(int motorId, int throttleValue, int timeout, CancellationToken cancel)
	{
		var ack = await _commandClient.CommandLong(
				MavCmd.MavCmdDoMotorTest,
				motorId,
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

	public async Task<MavResult> StopTest(int motorId, CancellationToken cancel = default)
	{
		return await RunMotorTest(motorId, 0, 0, cancel).ConfigureAwait(false);

	
	}

	public void Dispose()
	{
		IsTestRun.Dispose();
		_isRun.Dispose();
		_timerSubscription.Dispose();
	}
}