using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvSdrClient))]
public class AsvSdrClientTest() : ClientTestBase<AsvSdrClient>(_log)
{
    protected override AsvSdrClient CreateClient(MavlinkClientIdentity identity, CoreServices core) =>
        new(identity, core);

    private readonly Mock<IAsvSdrClient> _mockBaseClient = new();
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
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await Client.DeleteRecord(recordId, cancel.Token));
    }


    [Fact]
    public async Task Client_StartRecordCancelWithToken_Fail()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await Client.GetRecordList(0, ushort.MaxValue, cancel.Token));
    }

    [Fact]
    public async Task Client_GetRecordTagListCancelWithToken_Fail()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await Client.GetRecordTagList(new Guid(), 0, ushort.MaxValue, cancel.Token));
    }

    [Fact]
    public async Task Client_GetRecordListCancelWithToken_Fail()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await Client.GetRecordList(0, ushort.MaxValue, cancel.Token));
    }

    [Fact]
    public async Task Client_DeleteRecordTagCancelWithToken_Fail()
    {
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await Client.DeleteRecordTag(new TagId(new Guid(), new Guid()), cancel.Token));
    }

    [Fact]
    public void Client_CtorArgumentNullWithToken_Fail()
    {
        Assert.ThrowsAsync<ArgumentNullException>(() =>
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var test = new AsvSdrClient(Identity, null);
            return Task.CompletedTask;
        });
        Assert.ThrowsAsync<ArgumentNullException>(() =>
        {
            var test = new AsvSdrClient(null, Context);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            return Task.CompletedTask;
        });
    }
}