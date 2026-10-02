using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRsga;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using JetBrains.Annotations;
using R3;
using Xunit;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvRsgaClientEx))]
public class AsvRsgaClientExTest : ClientTestBase<AsvRsgaClientEx>, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly AsvRsgaClientEx _client;

    private readonly CommandProtocolConfig _commandCore = new()
    {
        CommandTimeoutMs = 1000,
        CommandAttempt = 5
    };

    private readonly StatusTextLoggerConfig _statusConfig = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 30,
    };

    private readonly HeartbeatClientConfig _heartbeatClientConfig = new();
    private HeartbeatClient? _heartbeatClient;

    protected override AsvRsgaClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        _heartbeatClient = new HeartbeatClient(identity, _heartbeatClientConfig, core);
        return new AsvRsgaClientEx(new AsvRsgaClient(identity, core), new CommandClient(identity, _commandCore, core),
            _heartbeatClient);
    }

    public AsvRsgaClientExTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Fact]
    public void Constructor_Null_Throws()
    {
        if (_heartbeatClient == null)
        {
            throw new NullReferenceException("_heartbeatClient is not initialized");
        }
        // Act
        Assert.Throws<NullReferenceException>(() =>
            new AsvRsgaClientEx(null!, new CommandClient(Client.Base.Identity, _commandCore, Client.Base.Core),_heartbeatClient));
        Assert.Throws<ArgumentNullException>(() =>
            new AsvRsgaClientEx(new AsvRsgaClient(Client.Base.Identity, Client.Base.Core), null!,_heartbeatClient));
        Assert.Throws<ArgumentNullException>(() =>
            new AsvRsgaClientEx(new AsvRsgaClient(Client.Base.Identity, Client.Base.Core), new CommandClient(Client.Base.Identity, _commandCore, Client.Base.Core), null!));
    }

    [Fact]
    public async Task ReadOnce_ShouldThrowTimeout_Exception()
    {
        // Arrange
        var attempts = (uint)5;
        var timeout = 1000;

        // Act
        var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.RefreshInfo(_cancellationTokenSource.Token);
        });

        var t2 = Task.Factory.StartNew(() => { Time.Advance(TimeSpan.FromMilliseconds(timeout * attempts + 1)); },
            Xunit.TestContext.Current.CancellationToken);

        //Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(attempts, Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task SetMode_ShouldThrowTimeout_Exception()
    {
        // Arrange
        var attempts = (uint)5;
        var timeout = 1000;

        // Act
        var t1 = Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await Client.SetMode(AsvRsgaCustomMode.AsvRsgaCustomModeIdle, _cancellationTokenSource.Token);
        });

        var t2 = Task.Factory.StartNew(() => { Time.Advance(TimeSpan.FromMilliseconds(timeout * attempts + 1)); },
            Xunit.TestContext.Current.CancellationToken);

        //Assert
        await Task.WhenAll(t1, t2);
        Assert.Equal(attempts, Link.Client.Statistic.TxMessages);
    }

    [Fact]
    public async Task Heartbeat_WhenModeAndSubModeChanged_ShouldUpdateCurrentModeAndSubMode()
    {
        // Arrange
        var modes = new List<AsvRsgaCustomMode>();
        var subModes = new List<AsvRsgaCustomSubMode>();
        using var modeSub = Client.CurrentMode.Skip(1).Subscribe(modes.Add);
        using var subModeSub = Client.CurrentSubMode.Skip(1).Subscribe(subModes.Add);
        var expectedMode = AsvRsgaCustomMode.AsvRsgaCustomModeRxGnss;
        var expectedSubMode = AsvRsgaCustomSubMode.AsvRsgaCustomSubModeRecord |
                              AsvRsgaCustomSubMode.AsvRsgaCustomSubModeMission;
        var packet = new HeartbeatPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
        };
        RsgaHelper.SetCustomMode(packet.Payload, expectedMode);
        RsgaHelper.SetCustomSubMode(packet.Payload, expectedSubMode);

        // Act
        await Link.Server.Send(packet, _cancellationTokenSource.Token);

        // Assert
        Assert.Equal(expectedMode, Client.CurrentMode.CurrentValue);
        Assert.Equal(expectedSubMode, Client.CurrentSubMode.CurrentValue);
        Assert.Contains(expectedMode, modes);
        Assert.Contains(expectedSubMode, subModes);
    }

    [Fact]
    public async Task GnssRawStream_WhenNewRefIdsReceived_ShouldCreateOneSourcePerRefIdAndUpdateExistingSource()
    {
        // Arrange
        var firstPacket = CreateGnssPacket(1, 10, 20, 30);
        var updatedFirstPacket = CreateGnssPacket(1, 11, 21, 31);
        var secondPacket = CreateGnssPacket(2, 12, 22, 32);

        // Act
        await Link.Server.Send(firstPacket, _cancellationTokenSource.Token);
        await Link.Server.Send(updatedFirstPacket, _cancellationTokenSource.Token);
        await Link.Server.Send(secondPacket, _cancellationTokenSource.Token);

        // Assert
        Assert.Equal(2, Client.GnssSources.Count);

        var firstSource = Client.GnssSources.Single(x => x.RefId == firstPacket.Payload.RefId);
        Assert.Equal("gnss_1", firstSource.NavId);
        Assert.Equal(updatedFirstPacket.Payload.Lat, firstSource.Stream.CurrentValue.Lat);
        Assert.Equal(updatedFirstPacket.Payload.Lon, firstSource.Stream.CurrentValue.Lon);
        Assert.Equal(updatedFirstPacket.Payload.AltMsl, firstSource.Stream.CurrentValue.AltMsl);

        var secondSource = Client.GnssSources.Single(x => x.RefId == secondPacket.Payload.RefId);
        Assert.Equal("gnss_2", secondSource.NavId);
        Assert.Equal(secondPacket.Payload.Lat, secondSource.Stream.CurrentValue.Lat);
        Assert.Equal(secondPacket.Payload.Lon, secondSource.Stream.CurrentValue.Lon);
        Assert.Equal(secondPacket.Payload.AltMsl, secondSource.Stream.CurrentValue.AltMsl);
    }

    [Fact]
    public async Task ChartRawStream_WhenChartPacketsReceived_ShouldPublishFramesAndCreateSourcesByChartType()
    {
        // Arrange
        var frames = new List<RsgaChartFrame>();
        using var sub = Client.ChartFrames.Subscribe(frames.Add);
        var firstPacket = CreateChartPacket(
            AsvRsgaRttChartType.AsvRsgaRttChartTypeDmePulseShapeXChannel,
            1,
            new[] { 1.0, 2.0, 3.0 }
        );
        var updatedFirstPacket = CreateChartPacket(
            AsvRsgaRttChartType.AsvRsgaRttChartTypeDmePulseShapeXChannel,
            2,
            new[] { 4.0, 5.0, 6.0 }
        );
        var secondPacket = CreateChartPacket(
            AsvRsgaRttChartType.AsvRsgaRttChartTypeDmePulseShapeYChannel,
            3,
            new[] { 7.0, 8.0, 9.0 }
        );

        // Act
        await Link.Server.Send(firstPacket, _cancellationTokenSource.Token);
        await Link.Server.Send(updatedFirstPacket, _cancellationTokenSource.Token);
        await Link.Server.Send(secondPacket, _cancellationTokenSource.Token);

        // Assert
        Assert.Equal(3, frames.Count);
        Assert.Equal(2, Client.ChartSources.Count);

        var firstSource = Client.ChartSources.Single(x =>
            x.ChartType == AsvRsgaRttChartType.AsvRsgaRttChartTypeDmePulseShapeXChannel);
        Assert.Equal(2u, firstSource.Stream.CurrentValue.DataIndex);
        Assert.InRange(Math.Abs(6.0 - firstSource.Stream.CurrentValue.Values[^1]), 0, 0.05);

        var secondSource = Client.ChartSources.Single(x =>
            x.ChartType == AsvRsgaRttChartType.AsvRsgaRttChartTypeDmePulseShapeYChannel);
        Assert.Equal(3u, secondSource.Stream.CurrentValue.DataIndex);
        Assert.InRange(Math.Abs(9.0 - secondSource.Stream.CurrentValue.Values[^1]), 0, 0.05);
    }

    [Fact]
    public async Task StartRecord_WhenNoAckReceived_ShouldSendCommandLongWithEncodedRecordNameAndThrowTimeout()
    {
        // Arrange
        const string recordName = "Test_Record";
        var packets = new List<CommandLongPacket>();
        using var sub = Link.Client.OnTxMessage.Subscribe(p =>
        {
            if (p is CommandLongPacket packet)
            {
                packets.Add(packet);
                Time.Advance(TimeSpan.FromMilliseconds(_commandCore.CommandTimeoutMs + 1));
            }
        });

        // Act
        var task = Client.StartRecord(recordName, _cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_commandCore.CommandAttempt, packets.Count);
        Assert.All(packets, packet =>
        {
            RsgaHelper.GetArgsForStartRecord(packet.Payload, out var actualName);
            Assert.Equal(recordName, actualName);
        });
    }

    [Fact]
    public async Task StopRecord_WhenNoAckReceived_ShouldSendCommandLongWithStopRecordCommandAndThrowTimeout()
    {
        // Arrange
        var packets = new List<CommandLongPacket>();
        using var sub = Link.Client.OnTxMessage.Subscribe(p =>
        {
            if (p is CommandLongPacket packet)
            {
                packets.Add(packet);
                Time.Advance(TimeSpan.FromMilliseconds(_commandCore.CommandTimeoutMs + 1));
            }
        });

        // Act
        var task = Client.StopRecord(_cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<TimeoutException>(async () => await task);
        Assert.Equal(_commandCore.CommandAttempt, packets.Count);
        Assert.All(packets, packet => RsgaHelper.GetArgsForStopRecord(packet.Payload));
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("This_Record_Name_Is_Too_Long1")]
    public async Task StartRecord_WhenRecordNameEmptyOrTooLong_ShouldThrowMavlinkExceptionWithoutSendingCommand(
        string recordName)
    {
        // Arrange
        var called = 0;
        using var sub = Link.Client.OnTxMessage.Subscribe(_ => called++);

        // Act
        var task = Client.StartRecord(recordName, _cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<MavlinkException>(async () => await task);
        Assert.Equal(0, called);
        Assert.Equal(0u, Link.Client.Statistic.TxMessages);
    }

    [Theory]
    [InlineData("1StartsWithDigit")]
    [InlineData("ab")]
    public async Task StartRecord_WhenRecordNameHasInvalidFormat_ShouldThrowArgumentExceptionWithoutSendingCommand(
        string recordName)
    {
        // Arrange
        var called = 0;
        using var sub = Link.Client.OnTxMessage.Subscribe(_ => called++);

        // Act
        var task = Client.StartRecord(recordName, _cancellationTokenSource.Token);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await task);
        Assert.Equal(0, called);
        Assert.Equal(0u, Link.Client.Statistic.TxMessages);
    }

    private AsvRsgaRttGnssPacket CreateGnssPacket(ushort refId, int lat, int lon, int altMsl)
    {
        return new AsvRsgaRttGnssPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                RefId = refId,
                Lat = lat,
                Lon = lon,
                AltMsl = altMsl,
            },
        };
    }

    private AsvRsgaRttChartPacket CreateChartPacket(
        AsvRsgaRttChartType chartType,
        uint dataIndex,
        double[] values
    )
    {
        var packet = new AsvRsgaRttChartPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
        };
        RsgaChartHelper.WriteChartData(packet.Payload, values, new RsgaChartSendOptions
        {
            ChartType = chartType,
            DataIndex = dataIndex,
        });
        return packet;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }

        base.Dispose(disposing);
    }
}
