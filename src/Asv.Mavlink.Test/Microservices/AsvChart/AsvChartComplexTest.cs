using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvChartClient))]
[TestSubject(typeof(AsvChartServer))]
public class AsvChartComplexTest : ComplexTestBase<AsvChartClient, AsvChartServer>, IDisposable
{
    private readonly TaskCompletionSource<AsvChartInfo> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public AsvChartComplexTest(ITestOutputHelper log) : base(log)
    {
        _taskCompletionSource = new TaskCompletionSource<AsvChartInfo>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    private readonly AsvChartServerConfig _serverConfig = new()
    {
        SendSignalDelayMs = 30,
        SendCollectionUpdateMs = 100,
    };

    private readonly AsvChartClientConfig _clientConfig = new()
    {
        MaxTimeToWaitForResponseForListMs = 1000,
    };

    protected override AsvChartServer CreateServer(MavlinkIdentity identity, ICoreServices core) =>
        new(identity, _serverConfig, core);

    protected override AsvChartClient CreateClient(MavlinkClientIdentity identity, ICoreServices core) =>
        new(identity, _clientConfig, core);


    [Fact]  
    public async Task Send_SendSinglePacket_Success()
    {
        // Arrange
        var info = new AsvChartInfo(1, "TestChart",
            new AsvChartAxisInfo("X", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10),
            new AsvChartAxisInfo("Y", AsvChartUnitType.AsvChartUnitTypeDbm, 0f, 10f, 10),
            AsvChartDataFormat.AsvChartDataFormatFloat);
        var data = new ReadOnlyMemory<float>(new float[info.OneFrameMeasureSize]);
        using var sub = Client.OnChartInfo.Subscribe(
            p => _taskCompletionSource.TrySetResult(p));

        // Act
        await Server.Send(DateTime.Now, data, info, _cancellationTokenSource.Token);
        ServerTime.Advance(TimeSpan.FromMilliseconds(200));

        // Assert
        Assert.Equal(2, Link.Client.RxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Single(Server.Charts); //TODO: Не обновляется коллекция Charts
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}