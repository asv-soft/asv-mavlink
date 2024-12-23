using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.AsvSdr;
using Asv.Mavlink.Common;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class AsvSdrComplexTests(ITestOutputHelper log) : ComplexTestBase<AsvSdrClientEx, AsvSdrServerEx>(log)
{
    private readonly HeartbeatClientConfig _configHeartbeat = new()
    {
        HeartbeatTimeoutMs = 2000,
        LinkQualityWarningSkipCount = 3,
        RateMovingAverageFilter = 3,
        PrintStatisticsToLogDelayMs = 10_000,
        PrintLinkStateToLog = true
    };

    private readonly MavlinkHeartbeatServerConfig _configHeartbeatServer = new()
    {
        HeartbeatRateMs = 1000,
    };

    private readonly CommandProtocolConfig _commandConfig = new()
    {
        CommandTimeoutMs = 100,
        CommandAttempt = 5
    };

    private readonly AsvSdrClientExConfig _sdrConfig = new()
    {
        MaxTimeToWaitForResponseForListMs = 1000
    };

    private readonly StatusTextLoggerConfig _configStatus = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 100
    };

    private readonly AsvSdrServerConfig _configSdr = new()
    {
        StatusRateMs = 1000,
    };

    protected override AsvSdrServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        var sdr = new AsvSdrServer(identity, _configSdr, core);
        var statusText = new StatusTextServer(identity, _configStatus, core);
        var heartbeat = new HeartbeatServer(identity, _configHeartbeatServer, core);
        var commandsEx = new CommandLongServerEx(new CommandServer(identity, core));
        return new AsvSdrServerEx(sdr, statusText, heartbeat, commandsEx);
    }

    protected override AsvSdrClientEx CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        return new AsvSdrClientEx(new AsvSdrClient(identity, core),
            new HeartbeatClient(identity, _configHeartbeat, core), new CommandClient(identity, _commandConfig, core),
            _sdrConfig);
    }

    [Fact]
    public async Task Client_StartsAndStopsRecordFromClient_Success()
    {
        var cancel = new CancellationTokenSource();
        Server.StartRecord = async (name, cancel) =>
        {
            await Server.Base.SendRecord(r =>
            {
                var cha = name.ToCharArray();
                for (int i = 0; i < cha.Length; i++)
                {
                    r.RecordName[i] = cha[i];
                }
            },cancel);
            return MavResult.MavResultAccepted;
        };
        Server.StopRecord = async _ =>
        {
            await Server.Base.SendRecord(r =>
            {
                r.Frequency = 100;
                r.DataType = AsvSdrCustomMode.AsvSdrCustomModeGp;
            },_);
           return MavResult.MavResultAccepted;
        };
        var res = await Client.StartRecord("record", cancel.Token);
        var res1 = await Client.StopRecord(cancel.Token);
        Assert.Equal(1,Client.Records.Count);
        Assert.Equal(MavResult.MavResultAccepted, res1);
        Assert.Equal(MavResult.MavResultAccepted, res);
    }

    [Fact]
    public async Task Client_SetMode_Success()
    {
        var cancel = new CancellationTokenSource();
        Server.SetMode = (mode, _, _, _, _, token) =>
        {
            if (token.IsCancellationRequested) return Task.FromResult(MavResult.MavResultCancelled);
            Server.CustomMode.Value = mode;
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        var res = await Client.SetMode(AsvSdrCustomMode.AsvSdrCustomModeGp, 100, (float)100.0, 100, (float)100.0,
            cancel.Token);

        Assert.Equal(MavResult.MavResultAccepted, res);
        Assert.Equal(AsvSdrCustomMode.AsvSdrCustomModeGp, Server.CustomMode.CurrentValue);
    }

    [Fact]
    public async Task Client_CancelSetMode_Fail()
    {
        var cancel = new CancellationTokenSource();
        Server.CustomMode.Value = AsvSdrCustomMode.AsvSdrCustomModeIdle;
        Server.SetMode = (mode, _, _, _, _, token) =>
        {
            if (token.IsCancellationRequested) return Task.FromResult(MavResult.MavResultCancelled);
            Server.CustomMode.Value = mode;
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        await cancel.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.SetMode(AsvSdrCustomMode.AsvSdrCustomModeGp, 100, (float)100.0, 100, (float)100.0,
                cancel.Token);
        });
        Assert.Equal(AsvSdrCustomMode.AsvSdrCustomModeIdle, Server.CustomMode.CurrentValue);
    }

    [Fact]
    public async Task Client_StartMission_Success()
    {
        var cancel = new CancellationTokenSource();
        var sendIndex = 1;
        Server.StartMission = (index, token) =>
        {
            if (token.IsCancellationRequested) return Task.FromResult(MavResult.MavResultCancelled);
            Assert.Equal((ushort)sendIndex, index);
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        var res = await Client.StartMission((ushort)sendIndex, cancel.Token);
        Assert.Equal(MavResult.MavResultAccepted, res);
    }

    [Fact]
    public async Task Client_CancelStartMission_Fail()
    {
        var cancel = new CancellationTokenSource();
        var sendIndex = 1;
        Server.StartMission = (index, token) =>
        {
            if (token.IsCancellationRequested) return Task.FromResult(MavResult.MavResultCancelled);
            Assert.Equal((ushort)sendIndex, index);
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        await cancel.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.StartMission((ushort)sendIndex, cancel.Token);
        });
    }

    [Fact]
    public async Task Client_CancelStopMission_Fail()
    {
        var cancel = new CancellationTokenSource();
        Server.StopMission = (token) =>
            Task.FromResult(token.IsCancellationRequested ? MavResult.MavResultCancelled : MavResult.MavResultAccepted);
        await cancel.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () => { await Client.StopMission(cancel.Token); });
    }

    [Fact]
    public async Task Client_StopMission_Success()
    {
        var cancel = new CancellationTokenSource();
        Server.StopMission = (token) =>
            Task.FromResult(token.IsCancellationRequested ? MavResult.MavResultCancelled : MavResult.MavResultAccepted);
        var res = await Client.StopMission(cancel.Token);
        Assert.Equal(MavResult.MavResultAccepted, res);
    }

    [Fact]
    public async Task Client_SystemControlAction_Success()
    {
        var cancel = new CancellationTokenSource();
        var sendAction = AsvSdrSystemControlAction.AsvSdrSystemControlActionRestart;

        Server.SystemControlAction = (action, token) =>
        {
            if (token.IsCancellationRequested) return Task.FromResult(MavResult.MavResultCancelled);
            Assert.Equal(sendAction, action);
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        var res = await Client.SystemControlAction(sendAction, cancel.Token);
        Assert.Equal(MavResult.MavResultAccepted, res);
    }

    [Fact]
    public async Task Client_CancelSystemControlAction_Fail()
    {
        var cancel = new CancellationTokenSource();
        var sendAction = AsvSdrSystemControlAction.AsvSdrSystemControlActionRestart;

        Server.SystemControlAction = (action, token) =>
        {
            if (token.IsCancellationRequested) return Task.FromResult(MavResult.MavResultCancelled);
            Assert.Equal(sendAction, action);
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        await cancel.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.SystemControlAction(sendAction, cancel.Token);
        });
    }

    [Fact]
    public async Task Client_CancelCurrentRecordSetTag_Fail()
    {
        var cancel = new CancellationTokenSource();
        var sendTagType = AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64;
        var sendStringTagName = "GP";
        var sendTagValue = new byte[] { 1, 1, 1, 1 }
            ;
        Server.CurrentRecordSetTag = (tag, _, _, token) =>
        {
            if (token.IsCancellationRequested) return Task.FromResult(MavResult.MavResultCancelled);
            Assert.Equal(sendTagType, tag);
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        await cancel.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await Client.CurrentRecordSetTag(sendStringTagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64,
                sendTagValue, cancel.Token);
        });
    }

    [Theory]
    [InlineData("G", new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 })]
    [InlineData("GP", new byte[] { 1, 1, 1, 1, 1, 1 })]
    [InlineData("1111glidePath", new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 })]
    [InlineData("Normal", new byte[] { 1, 1, })]
    public async Task Client_CurrentRecordSetTagWithBadValues_Fail(string badName, byte[] badValue)
    {
        var cancel = new CancellationTokenSource();

        var sendTagValue = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 };
        Server.CurrentRecordSetTag = (tag, name, value, token) =>
        {
            if (token.IsCancellationRequested) return Task.FromResult(MavResult.MavResultCancelled);
            Assert.Equal(name, name);
            Assert.Equal(sendTagValue, value);
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await Client.CurrentRecordSetTag(badName, AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64,
                badValue, cancel.Token);
        });
    }


    [Fact]
    public async Task Client_CurrentRecordSetTag_Success()
    {
        var cancel = new CancellationTokenSource();
        var sendTagType = AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64;
        var sendStringTagName = "GlidePat";
        var sendTagValue = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 };
        Server.CurrentRecordSetTag = async (tag, name, value, token) =>
        {
            if (token.IsCancellationRequested) return MavResult.MavResultCancelled;
            await Server.Base.SendRecordTag(t =>
            {
                var cha = name.ToCharArray();
                for (int i = 0; i < t.TagValue.Length; i++)
                {
                    t.TagValue[i] = value[i];
                }

                for (int i = 0; i < cha.Length; i++)
                {
                    t.TagName[i] = cha[i];
                }

                t.TagType = tag;
            }, token);
            Assert.Equal(sendStringTagName, name);
            Assert.Equal(sendTagValue, value);
            Assert.Equal(sendTagType, tag);
            return MavResult.MavResultAccepted;
        };
        var res = await Client.CurrentRecordSetTag(sendStringTagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64,
            sendTagValue, cancel.Token);
        Assert.Equal(MavResult.MavResultAccepted, res);
    }
}