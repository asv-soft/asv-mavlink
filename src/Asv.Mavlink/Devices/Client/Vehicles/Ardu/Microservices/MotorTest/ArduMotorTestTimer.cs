using System;
using System.Threading;
using System.Threading.Tasks;
using R3;

namespace Asv.Mavlink.MotorTest;

internal sealed class ArduMotorTestTimer : IDisposable
{
	private readonly SynchronizedReactiveProperty<bool> _isAnyRun = new(false);
	private readonly SemaphoreSlim _timerSemaphore = new(1, 1);

	private IDisposable? _timer;

	public ArduMotorTestTimer()
	{
		IsAnyTestRunning = _isAnyRun.ToReadOnlyReactiveProperty();
	}

	public ReadOnlyReactiveProperty<bool> IsAnyTestRunning { get; }

	public async Task RestartTimer(TimeSpan timeout, CancellationToken cancellationToken)
	{
		await _timerSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
		try
		{
			if (!_isAnyRun.Value)
				_isAnyRun.Value = true;

			_timer?.Dispose();
			_timer = Observable
				.Timer(timeout, cancellationToken)
				.Subscribe(_ => _isAnyRun.Value = false);
		}
		finally
		{
			_timerSemaphore.Release();
		}
	}

	public async Task StopTimer(CancellationToken ct = default)
	{
		await _timerSemaphore.WaitAsync(ct).ConfigureAwait(false);
		try
		{
			_timer?.Dispose();
			_timer = null;
			_isAnyRun.Value = false;
		}
		finally
		{
			_timerSemaphore.Release();
		}
	}

	public void Dispose()
	{
		_timer?.Dispose();
		_isAnyRun.Value = false;
		IsAnyTestRunning.Dispose();
		_isAnyRun.Dispose();
		_timerSemaphore.Dispose();
	}
}