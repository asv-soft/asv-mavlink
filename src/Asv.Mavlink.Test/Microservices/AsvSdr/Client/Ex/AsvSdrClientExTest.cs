using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvSdr;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvSdrClientEx))]
public class AsvSdrClientExTest() : ClientTestBase<AsvSdrClientEx>(_log)
{
    private static ITestOutputHelper _log = new TestOutputHelper();

    private readonly HeartbeatClientConfig _configHeartbeat = new()
    {
        HeartbeatTimeoutMs = 2000,
        LinkQualityWarningSkipCount = 3,
        RateMovingAverageFilter = 3,
        PrintStatisticsToLogDelayMs = 10_000,
        PrintLinkStateToLog = true
    };

    private readonly CommandProtocolConfig _commandConfig = new()
    {
        CommandTimeoutMs = 1000,
        CommandAttempt = 5
    };

    private readonly AsvSdrClientExConfig _sdrConfig = new()
    {
        MaxTimeToWaitForResponseForListMs = 1000
    };

    protected override AsvSdrClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new AsvSdrClientEx(new AsvSdrClient(identity, core),
            new HeartbeatClient(identity, _configHeartbeat, core), new CommandClient(identity, _commandConfig, core),
            _sdrConfig);
    }

    [Fact]
    public async Task Client_Init_Success()
    {
        await Client.Init();
        Assert.True(true);
    }

    [Fact]
    public async Task Client_DeleteRecordCancelWithToken_Fail()
    {
        var recordId = Guid.NewGuid();
        var cancel = new CancellationTokenSource();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await Client.DeleteRecord(recordId, cancel.Token));
    }

    [Fact]
    public async Task Client_StartRecordCancelWithToken_Fail()
    {
        var recordName = "Test_Record";
        var cancel = new CancellationTokenSource();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await Client.StartRecord(recordName, cancel.Token));
    }

    [Fact]
    public async Task Client_OnRecordTagCancelWithToken_Fail()
    {
        var tagName = "tag";
        var rawValue = new byte[] { 1, 0, 0, 1, 0, 0, 1, 0 };
        var cancel = new CancellationTokenSource();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await Client.CurrentRecordSetTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64,
                rawValue, cancel.Token));
    }
    [Fact]
    public async Task Client_OnRecordTagExcptWithBadValues_Fail()
    {
        var tagName = "tag";
        var rawValue = new byte[] { 1, 0, 0};
        var cancel = new CancellationTokenSource();
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await Client.CurrentRecordSetTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64,
                rawValue, cancel.Token));
    }

    [Fact]
    public async Task Client_StartCalibrationCancelWithToken_Fail()
    {
        // Arrange
        var cancel = new CancellationTokenSource();
        await cancel.CancelAsync();
        // Act
        var exception = await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await Client.StartCalibration(cancel.Token));
        //Assert
        Assert.True(exception.CancellationToken.IsCancellationRequested);
    }

    [Fact]
    public void Client_CtorArgumentNull_Fail()
    {
        var identity = new MavlinkClientIdentity(1, 2, 3, 4);
        var heartBeat = new HeartbeatClient(identity, _configHeartbeat, Context);
        var command = new CommandClient(identity, _commandConfig, Context);
        Assert.ThrowsAsync<ArgumentNullException>(() =>
        {
            var test = new AsvSdrClientEx(null, heartBeat, command, new AsvSdrClientExConfig());
            return Task.CompletedTask;
        });
        Assert.ThrowsAsync<ArgumentNullException>(() =>
        {
            var test = new AsvSdrClientEx(Client.Base, null, command, new AsvSdrClientExConfig());
            return Task.CompletedTask;
        });
        Assert.ThrowsAsync<ArgumentNullException>(() =>
        {
            var test = new AsvSdrClientEx(Client.Base, heartBeat, null, new AsvSdrClientExConfig());
            return Task.CompletedTask;
        });
        Assert.ThrowsAsync<ArgumentNullException>(() =>
        {
            var test = new AsvSdrClientEx(Client.Base, heartBeat, command, null);
            return Task.CompletedTask;
        });
    }
}