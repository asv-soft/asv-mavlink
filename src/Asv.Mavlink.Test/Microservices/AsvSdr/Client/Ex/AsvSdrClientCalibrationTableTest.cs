using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AsvSdrClientCalibrationTable))]
public class AsvSdrClientCalibrationTableTest : ComplexTestBase<AsvSdrClientEx, AsvSdrServerEx>
{
    private static ITestOutputHelper _log = new TestOutputHelper();


    private List<CalibrationItem> _tables = [];

    #region Test data

    private class CalibrationItem
    {
        public string Name { get; } = Guid.NewGuid().ToString();
        public ushort Size { get; }
        public bool IsEnabled { get; set; } = false;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public CalibrationTableMetadata Metadata { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }

    #endregion

    #region Configs

    private readonly MavlinkHeartbeatServerConfig _configHeartbeatServer = new()
    {
        HeartbeatRateMs = 1000,
    };

    private readonly AsvSdrServerConfig _configServerSdr = new()
    {
        StatusRateMs = 1000,
    };

    private readonly StatusTextLoggerConfig _configStatus = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 100
    };

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

    #endregion


    public AsvSdrClientCalibrationTableTest() : base(_log)
    {
        _tables = new List<CalibrationItem>();
        for (var i = 0; i < 10; i++)
        {
            _tables.Add(new CalibrationItem()
            {
                Metadata = new CalibrationTableMetadata() { Updated = DateTime.Now }
            });
        }
    }

    [Fact]
    public async Task CalibrationTable_StartCalibration_Success()
    {
        Server.StartCalibration += token =>
        {
            _tables.ForEach(x => x.IsEnabled = false);
            _tables.ForEach(x => x.Metadata.Updated = DateTime.Now);
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await Client.StartCalibration(cancel.Token);
        Assert.True(_tables.All(c => c.IsEnabled == false));
    }

    [Fact]
    public async Task CalibrationTable_StopCalibration_Success()
    {
        Server.StartCalibration += token =>
        {
            _tables.ForEach(x => x.IsEnabled = false);
            _tables.ForEach(x => x.Metadata.Updated = DateTime.Now);
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        Server.StopCalibration += token =>
        {
            _tables.ForEach(x => x.IsEnabled = true);
            _tables.ForEach(x => x.Metadata.Updated = DateTime.Now);
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await Client.StartCalibration(cancel.Token);
        Assert.True(_tables.All(c => c.IsEnabled == false));
        cancel = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await Client.StopCalibration(cancel.Token);
        Assert.True(_tables.All(c => c.IsEnabled));
    }

    protected override AsvSdrServerEx CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        var sdr = new AsvSdrServer(identity, _configServerSdr, core);
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
}