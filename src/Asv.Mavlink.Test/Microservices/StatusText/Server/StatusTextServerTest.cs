using System;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(StatusTextServer))]
public class StatusTextServerTest(ITestOutputHelper log)
    : ServerTestBase<StatusTextServer>(log)
{
    private readonly StatusTextLoggerConfig _config = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 10
    };

    protected override StatusTextServer CreateServer(MavlinkIdentity identity, CoreServices core) =>
        new(identity, _config, core);

    [Fact]
    public void Ctor_IdentityArgIsNull_Fail()
    {
        Assert.Throws<ArgumentNullException>(() => { _ = new StatusTextServer(null!, _config, Core); });
    }

    [Fact]
    public void Ctor_CoreArgIsNull_Fail()
    {
        Assert.Throws<ArgumentNullException>(() => { _ = new StatusTextServer(Identity, _config, null!); });
    }

    [Fact]
    public void Ctor_ConfigArgIsNull_Fail()
    {
        Assert.Throws<ArgumentNullException>(() => { _ = new StatusTextServer(Identity, null!, Core); });
    }

    [Fact]
    public void Log_ValidMessage_Success()
    {
        // Arrange
        const MavSeverity severity = MavSeverity.MavSeverityInfo;
        const string message = "Test message";

        // Act
        var result = Server.Log(severity, message);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Log_NullOrEmptyMessage_Fail()
    {
        // Act
        var result1 = Server.Log(MavSeverity.MavSeverityInfo, null!);
        var result2 = Server.Log(MavSeverity.MavSeverityInfo, string.Empty);

        // Assert
        Assert.False(result1);
        Assert.False(result2);
    }

    [Fact]
    public void Log_MessageTooLong_Truncating_Success()
    {
        // Arrange
        var longMessage = new string('A', 500);

        // Act
        var result = Server.Log(MavSeverity.MavSeverityWarning, longMessage);

        // Assert
        Assert.True(result);
    }
}