using System;
using System.Threading;
using R3;

namespace Asv.Mavlink;

internal sealed class ArduMotorTestTimer : IDisposable
{
	private readonly TimeProvider _timeProvider;
	private readonly SynchronizedReactiveProperty<bool> _isAnyRun = new(false);
	private readonly SerialDisposable _timerSubscription = new();

	public ArduMotorTestTimer(TimeProvider? timeProvider = null)
	{
		_timeProvider = timeProvider ?? TimeProvider.System;
		IsAnyTestRunning = _isAnyRun.ToReadOnlyReactiveProperty();
	}

	public ReadOnlyReactiveProperty<bool> IsAnyTestRunning { get; }

	public void RestartTimer(TimeSpan timeout, CancellationToken cancellationToken)
	{
		if (!_isAnyRun.Value)
			_isAnyRun.Value = true;

		_timerSubscription.Disposable = Observable
			.Timer(timeout, _timeProvider, cancellationToken)
			.Subscribe(_ => { _isAnyRun.Value = false; }
			);
	}

	public void StopTimer()
	{
		_timerSubscription.Disposable = null;
		_isAnyRun.Value = false;
	}

	public void Dispose()
	{
		_timerSubscription.Dispose();
		IsAnyTestRunning.Dispose();
		_isAnyRun.Dispose();
	}
}
