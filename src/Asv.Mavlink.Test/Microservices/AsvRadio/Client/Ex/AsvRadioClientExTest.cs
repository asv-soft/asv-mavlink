using Asv.Mavlink.AsvRadio;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Client.Ex;

[TestSubject(typeof(AsvRadioClientEx))]
public class AsvRadioClientExTest(ITestOutputHelper log) : ClientTestBase<AsvRadioClientEx>(log)
{
    private readonly HeartbeatClientConfig _heartbeatConfig = new()
    {
        HeartbeatTimeoutMs = 2000,
        LinkQualityWarningSkipCount = 3,
        RateMovingAverageFilter = 3,
        PrintStatisticsToLogDelayMs = 1000,
        PrintLinkStateToLog = true
    };

    private CommandProtocolConfig _commandConfig = new()
    {
        CommandTimeoutMs = 1000,
        CommandAttempt = 5
    };

    [Fact]
    public void Ctor_CreatesClientCorrect_Success()
    {
        //Arrange & Act
        var client = Client;
        
        //Assert
        Assert.NotNull(client.Identity);
        Assert.NotNull(client.TypeName);
        Assert.NotNull(client.Capabilities);
        Assert.NotNull(client.Base);
        Assert.NotNull(client.Core);
        Assert.Equal(AsvRadioCustomMode.AsvRadioCustomModeIdle, client.CustomMode.CurrentValue);
    }

    protected override AsvRadioClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new AsvRadioClientEx(new AsvRadioClient(identity, core),
            new HeartbeatClient(identity, _heartbeatConfig, core), new CommandClient(identity, _commandConfig, core));
    }
}