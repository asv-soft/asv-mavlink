using System;
using System.Threading;

namespace Asv.Mavlink;

public static class TimeProviderTaskExtensions
{
    /// <param name="timeProvider">The <see cref="TimeProvider"/> with which to interpret the <paramref name="delay"/>. </param>
    /// <param name="cts">The <see cref="CancellationTokenSource"/> to be canceled after the delay. </param>
    /// <param name="delay">The time interval to wait before canceling this <see cref="CancellationTokenSource"/>. </param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="delay"/>'s <see cref="TimeSpan.TotalMilliseconds"/> is less than -1 or greater than <see cref="uint.MaxValue"/> - 1. </exception>
    /// <remarks>
    /// The countdown for the delay starts during the call to the constructor.  When the delay expires,
    /// the constructed <see cref="CancellationTokenSource"/> is canceled if it has
    /// not been canceled already. Subsequent calls to CancelAfter will reset the delay for the constructed
    /// <see cref="CancellationTokenSource"/> if it has not been canceled already.
    /// </remarks>
    public static void CancelAfter(this CancellationTokenSource cts, TimeSpan delay, TimeProvider  timeProvider)
    {
        if (timeProvider == TimeProvider.System)
        {
            cts.CancelAfter(delay);
        }
        else
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var timer = timeProvider.CreateTimer(s => ((CancellationTokenSource)s).Cancel(), cts, delay, Timeout.InfiniteTimeSpan);
            cts.Token.Register(t => ((ITimer)t).Dispose(), timer);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }    
    }
    public static void CancelAfter(this CancellationTokenSource cts, int delayMs, TimeProvider  timeProvider)
    {
        if (timeProvider == TimeProvider.System)
        {
            cts.CancelAfter(delayMs);
        }
        else
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var timer = timeProvider.CreateTimer(s => ((CancellationTokenSource)s).Cancel(), cts, TimeSpan.FromMilliseconds(delayMs), Timeout.InfiniteTimeSpan);
            cts.Token.Register(t => ((ITimer)t).Dispose(), timer);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }    
    }
}