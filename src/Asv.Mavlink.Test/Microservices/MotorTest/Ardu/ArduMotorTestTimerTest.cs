using System;
using System.Threading;
using JetBrains.Annotations;
using TimeProviderExtensions;
using Xunit;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(ArduMotorTestTimer))]
public class ArduMotorTestTimerTest
{
    private readonly ManualTimeProvider _time = new();
    
    public static TheoryData<DateTimeOffset, TimeSpan> StartTimes { get; } =
    [
        new TheoryDataRow<DateTimeOffset, TimeSpan>(
            DateTimeOffset.MinValue,
            TimeSpan.FromSeconds(5)),
        new TheoryDataRow<DateTimeOffset, TimeSpan>(
            new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
            TimeSpan.FromSeconds(5)),
        new TheoryDataRow<DateTimeOffset, TimeSpan>(
            DateTimeOffset.MaxValue - TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(5))
    ];

    [Theory]
    [MemberData(nameof(StartTimes))]
    public void RestartTimer_TimeoutElapsedAtDifferentStartTimes_IsAnyTestRunningFalse(
        DateTimeOffset startTime,
        TimeSpan timeout)
    {
        // Arrange
        var time = new ManualTimeProvider(startTime);
        using var timer = new ArduMotorTestTimer(time);

        // Act
        timer.RestartTimer(timeout, CancellationToken.None);
        time.Advance(timeout - TimeSpan.FromTicks(1));

        // Assert
        Assert.True(timer.IsAnyTestRunning.CurrentValue);

        // Act
        time.Advance(TimeSpan.FromTicks(1));

        // Assert
        Assert.False(timer.IsAnyTestRunning.CurrentValue);
    }

    [Fact]
    public void IsAnyTestRunning_BeforeRestart_False()
    {
        // Arrange
        using var timer = new ArduMotorTestTimer(_time);

        // Assert
        Assert.False(timer.IsAnyTestRunning.CurrentValue);
    }

    [Fact]
    public void IsAnyTestRunning_DefaultTimeProvider_False()
    {
        // Arrange
        using var timer = new ArduMotorTestTimer();

        // Assert
        Assert.False(timer.IsAnyTestRunning.CurrentValue);
    }

    [Fact]
    public void RestartTimer_AfterCall_IsAnyTestRunningTrue()
    {
        // Arrange
        using var timer = new ArduMotorTestTimer(_time);

        // Act
        timer.RestartTimer(TimeSpan.FromSeconds(5), CancellationToken.None);

        // Assert
        Assert.True(timer.IsAnyTestRunning.CurrentValue);
    }

    [Fact]
    public void StopTimer_AfterRestart_IsAnyTestRunningFalse()
    {
        // Arrange
        using var timer = new ArduMotorTestTimer(_time);
        timer.RestartTimer(TimeSpan.FromSeconds(5), CancellationToken.None);

        // Act
        timer.StopTimer();

        // Assert
        Assert.False(timer.IsAnyTestRunning.CurrentValue);
    }

    [Fact]
    public void RestartTimer_Twice_OnlyLastFires()
    {
        // Arrange
        using var timer = new ArduMotorTestTimer(_time);

        // Act
        timer.RestartTimer(TimeSpan.FromSeconds(1), CancellationToken.None);
        timer.RestartTimer(TimeSpan.FromSeconds(10), CancellationToken.None);
        _time.Advance(TimeSpan.FromSeconds(2));

        // Assert
        Assert.True(timer.IsAnyTestRunning.CurrentValue);
    }
}
