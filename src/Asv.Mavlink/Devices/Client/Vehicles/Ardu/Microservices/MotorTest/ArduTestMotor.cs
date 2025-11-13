using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using Asv.Mavlink.MotorTest;
using R3;

namespace Asv.Mavlink;

internal sealed class ArduTestMotor : ITestMotor, IDisposable
{
	private readonly SynchronizedReactiveProperty<bool> _isRun = new(false);
	private readonly ICommandClient _commandClient;
	private readonly ArduMotorTestTimer _testTimer;
	private readonly IDisposable _testRunSubscription;
	private readonly SemaphoreSlim _semaphore = new(1, 1);

	public ArduTestMotor(int id, int servoChannel, Observable<ushort> pwm, ICommandClient commandClient, ArduMotorTestTimer testTimer)
	{
		Id = id;
		ServoChannel = servoChannel;
		Pwm = pwm.ToReadOnlyReactiveProperty();

		IsTestRun = _isRun.ToReadOnlyReactiveProperty();
		_commandClient = commandClient;
		_testTimer = testTimer;
		_testRunSubscription = _testTimer.IsAnyTestRunning
			.DistinctUntilChanged()
			.Where(x => !x)
			.Subscribe(_ => _isRun.Value = false);
	}

	public int Id { get; }
	public int ServoChannel { get; }
	public ReadOnlyReactiveProperty<ushort> Pwm { get; }
	public ReadOnlyReactiveProperty<bool> IsTestRun { get; }

	public async Task<MavResult> StartTest(int throttleValue, int timeout, CancellationToken cancel = default)
	{
		await _semaphore.WaitAsync(cancel).ConfigureAwait(false);

		try
		{
			var ack = await _commandClient.CommandLong(
					MavCmd.MavCmdDoMotorTest,
					Id,
					(float)MotorTestThrottleType.MotorTestThrottlePercent,
					throttleValue,
					timeout,
					0, 
					0, 
					0,
					cancel
				)
				.ConfigureAwait(false);

			if (ack.Result != MavResult.MavResultAccepted)
				return ack.Result;

			_isRun.Value = true;
			await _testTimer.RestartTimer(TimeSpan.FromSeconds(timeout), cancel).ConfigureAwait(false);

			return ack.Result;
		}
		finally
		{
			_semaphore.Release();
		}
	}

	public async Task<MavResult> StopTest(CancellationToken cancel = default)
	{
		await _semaphore.WaitAsync(cancel).ConfigureAwait(false);

		try
		{
			await _testTimer.StopTimer(cancel).ConfigureAwait(false);

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

			return ack.Result;
		}
		finally
		{
			_semaphore.Release();
		}
	}

	public void Dispose()
	{
		Pwm.Dispose();
		IsTestRun.Dispose();
		_isRun.Dispose();
		_testRunSubscription.Dispose();
		_semaphore.Dispose();
	}
}