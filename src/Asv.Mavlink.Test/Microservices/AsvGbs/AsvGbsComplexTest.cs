using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.AsvGbs;
using Asv.Mavlink.Common;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class AsvGbsComplexTest : ComplexTestBase<AsvGbsExClient, AsvGbsExServer>, IDisposable
{
    private readonly AsvGbsServerConfig _serverConfig = new()
    {
        StatusRateMs = 1000
    };
    
    private const byte HeartbeatClientComponentId = 10;
    private const byte HeartbeatServerComponentId = 11;
    private const byte CommandClientComponentId = 14;
    private const byte CommandServerComponentId = 15;
    
    private readonly TaskCompletionSource _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IAsvGbsExClient _client;
    private readonly IAsvGbsServerEx _server;

    public AsvGbsComplexTest(ITestOutputHelper output) : base(output)
    {
        _server = Server;
        _client = Client;
        _taskCompletionSource = new TaskCompletionSource();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override AsvGbsExServer CreateServer(MavlinkIdentity identity, IMavlinkContext core)
    {
        var heartbeatServer = new HeartbeatServer(
            new MavlinkIdentity(Identity.Target.SystemId, HeartbeatServerComponentId), 
            new MavlinkHeartbeatServerConfig
            {
                HeartbeatRateMs = 1000
            }, 
            core
        );
        var gbsServer = new AsvGbsServer(identity, _serverConfig, core);
        var commandBase = new CommandServer(
            new MavlinkIdentity(
                Identity.Target.SystemId, 
                CommandServerComponentId
            ), 
            core
        );
        var commandLongEx = new CommandLongServerEx(commandBase);

        return new AsvGbsExServer(gbsServer, heartbeatServer, commandLongEx);;
    }

    protected override AsvGbsExClient CreateClient(MavlinkClientIdentity identity, IMavlinkContext core)
    {
        var heartbeatClient = new HeartbeatClient(
            new MavlinkClientIdentity(
                Identity.Self.SystemId, 
                HeartbeatClientComponentId, 
                Identity.Target.SystemId,
                HeartbeatServerComponentId
            ), 
            new HeartbeatClientConfig{
                HeartbeatTimeoutMs = 2000,
                LinkQualityWarningSkipCount = 3,
                RateMovingAverageFilter = 3,
                PrintStatisticsToLogDelayMs = 10000,
                PrintLinkStateToLog = true
            }, 
            core
        );
        
        var gbsClient = new AsvGbsClient(identity, core);
        var protocolCfg = new CommandProtocolConfig{
            CommandTimeoutMs = 1000,
            CommandAttempt = 5
        };
        
        var commandBase = new CommandClient(
            new MavlinkClientIdentity(
                Identity.Self.SystemId, 
                CommandClientComponentId,
                Identity.Target.SystemId,
                CommandServerComponentId
            ), 
            protocolCfg, 
            core
        );
        
        return new AsvGbsExClient(gbsClient, heartbeatClient, commandBase);
    }

    [Theory]
    [InlineData(MavResult.MavResultAccepted)]
    [InlineData(MavResult.MavResultCancelled)]
    [InlineData(MavResult.MavResultDenied)]
    [InlineData(MavResult.MavResultFailed)]
    [InlineData(MavResult.MavResultInProgress)]
    [InlineData(MavResult.MavResultUnsupported)]
    [InlineData(MavResult.MavResultTemporarilyRejected)]
    [InlineData(MavResult.MavResultCommandIntOnly)]
    [InlineData(MavResult.MavResultCommandLongOnly)]
    [InlineData(MavResult.MavResultCommandUnsupportedMavFrame)]
    public async Task StartAutoMode_DifferentResults_Success(MavResult expectedResult)
    {
        // Arrange
        const int duration = 10;
        const int accuracy = 12;

        var called = 0;
        var realDuration = 0f;
        var realAccuracy = 0f;
        _server.StartAutoMode = (_, _, _)
            =>
        {
            called++;
            realDuration = duration;
            realAccuracy = accuracy;

            _taskCompletionSource.TrySetResult();
            return Task.FromResult(expectedResult);
        };

        // Act
        var result = await _client.StartAutoMode(duration, accuracy, CancellationToken.None);

        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(duration, realDuration);
        Assert.Equal(accuracy, realAccuracy);
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(MavResult.MavResultAccepted)]
    [InlineData(MavResult.MavResultCancelled)]
    [InlineData(MavResult.MavResultDenied)]
    [InlineData(MavResult.MavResultFailed)]
    [InlineData(MavResult.MavResultInProgress)]
    [InlineData(MavResult.MavResultUnsupported)]
    [InlineData(MavResult.MavResultTemporarilyRejected)]
    [InlineData(MavResult.MavResultCommandIntOnly)]
    [InlineData(MavResult.MavResultCommandLongOnly)]
    [InlineData(MavResult.MavResultCommandUnsupportedMavFrame)]
    public async Task StartIdleMode_DifferentResults_Success(MavResult expectedResult)
    {
        // Arrange
        var called = 0;
        
        _server.StartIdleMode = (_) =>
        {
            called++;

            _taskCompletionSource.TrySetResult();
            return Task.FromResult(expectedResult);
        };
        
        // Act
        var result = await _client.StartIdleMode(_cancellationTokenSource.Token);
        
        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(expectedResult, result);
    }
    
    [Theory]
    [InlineData(MavResult.MavResultAccepted)]
    [InlineData(MavResult.MavResultCancelled)]
    [InlineData(MavResult.MavResultDenied)]
    [InlineData(MavResult.MavResultFailed)]
    [InlineData(MavResult.MavResultInProgress)]
    [InlineData(MavResult.MavResultUnsupported)]
    [InlineData(MavResult.MavResultTemporarilyRejected)]
    [InlineData(MavResult.MavResultCommandIntOnly)]
    [InlineData(MavResult.MavResultCommandLongOnly)]
    [InlineData(MavResult.MavResultCommandUnsupportedMavFrame)]
    public async Task StartFixedMode_DifferentResults_Success(MavResult expectedResult)
    {
        // Arrange
        const int accuracy = 10;
        
        var called = 0;
        var realGeoPoint = GeoPoint.NaN;
        var realAccuracy = 0f;
        
        _server.StartFixedMode = (_, _, _) => 
        {
            called++;
            realGeoPoint = GeoPoint.Zero;
            realAccuracy = accuracy;

            _taskCompletionSource.TrySetResult();
            return Task.FromResult(expectedResult);
        };
        
        // Act
        var result = await _client.StartFixedMode(
            GeoPoint.Zero, 
            accuracy, 
            _cancellationTokenSource.Token
        );
        
        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(GeoPoint.Zero, realGeoPoint);
        Assert.Equal(accuracy, realAccuracy);
        Assert.Equal(expectedResult, result);
    }
    
    [Theory]
    [InlineData(float.MinValue)]
    [InlineData(float.MaxValue)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NaN)]
    [InlineData(float.Epsilon)]
    [InlineData(float.E)]
    [InlineData(float.Tau)]
    [InlineData(float.NegativeZero)]
    [InlineData(float.Pi)]
    public async Task StartFixedMode_DifferentAccuracyValues_Success(float accuracy)
    {
        // Arrange
        var called = 0;
        var realGeoPoint = GeoPoint.NaN;
        var realAccuracy = 0f;
        
        _server.StartFixedMode = (_, _, _) => 
        {
            called++;
            realGeoPoint = GeoPoint.Zero;
            realAccuracy = accuracy;

            _taskCompletionSource.TrySetResult();
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        
        // Act
        var result = await _client.StartFixedMode(
            GeoPoint.Zero, 
            accuracy, 
            _cancellationTokenSource.Token
        );
        
        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(GeoPoint.Zero, realGeoPoint);
        Assert.Equal(accuracy, realAccuracy);
        Assert.Equal(MavResult.MavResultAccepted, result);
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task StartFixedMode_SeveralCalls_Success(int callsCount)
    {
        // Arrange
        var called = 0;
        _server.StartFixedMode = (_, _, _) =>
        {
            called++;
            
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        
        // Act
        var results = new List<MavResult>();
        for (var i = 0; i < callsCount; i++)
        {
            var result = await _client.StartFixedMode(
                GeoPoint.Zero, 
                10,
                _cancellationTokenSource.Token
            );
            
            results.Add(result);
        }
        
        // Assert
        Assert.Equal(callsCount, called);
        Assert.True(results.All(r => r == MavResult.MavResultAccepted));
    }

    [Theory]
    [InlineData(AsvGbsCustomMode.AsvGbsCustomModeLoading)]
    [InlineData(AsvGbsCustomMode.AsvGbsCustomModeAutoInProgress)]
    [InlineData(AsvGbsCustomMode.AsvGbsCustomModeAuto)]
    [InlineData(AsvGbsCustomMode.AsvGbsCustomModeError)]
    [InlineData(AsvGbsCustomMode.AsvGbsCustomModeFixed)]
    [InlineData(AsvGbsCustomMode.AsvGbsCustomModeIdle)]
    [InlineData(AsvGbsCustomMode.AsvGbsCustomModeFixedInProgress)]
    public async Task SetCustomMode_DifferentModes_Success(AsvGbsCustomMode mode)
    {
        // Arrange
        _server.Start();
        var tcs = new TaskCompletionSource<AsvGbsCustomMode>();
        var cancel = new CancellationTokenSource();
        cancel.Token.Register(() => tcs.TrySetCanceled());
        using var sub = _client.CustomMode
            .Subscribe(p =>
            {
                if (p != mode)
                {
                    return;
                }
                
                tcs.TrySetResult(p);
            });
        
        // Act
        _server.CustomMode.OnNext(mode);
        
        ServerTime.Advance(TimeSpan.FromSeconds(10));
        ClientTime.Advance(TimeSpan.FromSeconds(10));
        
        // Assert
        var modeFromServer = await tcs.Task;
        Assert.Equal(mode, modeFromServer);
    }
    
    [Fact]
    public async Task StartAutoMode_Canceled_Throws()
    {
        // Arrange
        var called = 0;
        _server.StartAutoMode = (_, _, _)
            => 
        {
            called++;
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        
        // Act
        await _cancellationTokenSource.CancelAsync();
        var task = _client.StartAutoMode(
            10, 
            10, 
            _cancellationTokenSource.Token
        );
        
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        Assert.Equal(0, called);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Set_DifferentValues_Success(bool isMax)
    {
        // Arrange
        _server.Start();
        var intValue = isMax ? int.MaxValue : int.MinValue;
        var ushortValue = isMax ? ushort.MaxValue : ushort.MinValue;
        var byteValue = isMax ? byte.MaxValue : byte.MinValue;
        var tcs = new TaskCompletionSource<AsvGbsOutStatusPayload>();
        var cancel = new CancellationTokenSource();
        cancel.Token.Register(() => tcs.TrySetCanceled());
        using var sub = _client.Base.RawStatus
            .Subscribe(p =>
            {
                if (p is null)
                {
                    return;
                }
                
                tcs.TrySetResult(p);
            });
        
        // Act
        _server.Base.Set(pld =>
        {
            pld.Accuracy = ushortValue;
            pld.Observation = ushortValue;
            pld.DgpsRate = ushortValue;
            pld.SatAll = byteValue;
            pld.SatGal = byteValue;
            pld.SatBdu = byteValue;
            pld.SatGlo = byteValue;
            pld.SatGps = byteValue;
            pld.SatQzs = byteValue;
            pld.SatSbs = byteValue;
            pld.SatIme = byteValue;
            pld.Lat = intValue;
            pld.Lng = intValue;
            pld.Alt = intValue;
        });
        
        ServerTime.Advance(TimeSpan.FromSeconds(10));
        ClientTime.Advance(TimeSpan.FromSeconds(10));
        
        // Assert
        var status = await tcs.Task;
        Assert.NotNull(status);
        Assert.Equal(ushortValue, status.Accuracy);
        Assert.Equal(ushortValue, status.Observation);
        Assert.Equal(ushortValue, status.DgpsRate);
        Assert.Equal(byteValue, status.SatAll);
        Assert.Equal(byteValue, status.SatGal);
        Assert.Equal(byteValue, status.SatBdu);
        Assert.Equal(byteValue, status.SatGlo);
        Assert.Equal(byteValue, status.SatGps);
        Assert.Equal(byteValue, status.SatQzs);
        Assert.Equal(byteValue, status.SatSbs);
        Assert.Equal(byteValue, status.SatIme);
        Assert.Equal(intValue, status.Lat);            
        Assert.Equal(intValue, status.Lng);
        Assert.Equal(intValue, status.Alt);         
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SetServerStatus_WithoutSet_Success(bool isMax)
    {
        // Arrange
        _server.Start();
        var ushortValue = isMax ? ushort.MaxValue : ushort.MinValue;
        var byteValue = isMax ? byte.MaxValue : byte.MinValue;
        var tcs = new TaskCompletionSource<AsvGbsOutStatusPayload>();
        var tcsMode = new TaskCompletionSource<AsvGbsCustomMode>();
        var cancel = new CancellationTokenSource();
        var cancelMode = new CancellationTokenSource();
        cancel.Token.Register(() => tcs.TrySetCanceled());
        cancelMode.Token.Register(() => tcsMode.TrySetCanceled());
        using var sub1 = _client.Base.RawStatus
            .Subscribe(p =>
            {
                if (p is null)
                {
                    return;
                }
                
                tcs.TrySetResult(p);
            });
        using var sub2 = _client.CustomMode.Subscribe(p =>
        {
            if (p != AsvGbsCustomMode.AsvGbsCustomModeAuto)
            {
                return;
            }
            
            tcsMode.TrySetResult(p);
        });
        
        // Act
        _server.AccuracyMeter.OnNext(65.53);
        _server.ObservationSec.OnNext(ushortValue);
        _server.DgpsRate.OnNext(ushortValue);
        _server.AllSatellites.OnNext(byteValue);
        _server.GalSatellites.OnNext(byteValue);
        _server.BeidouSatellites.OnNext(byteValue);
        _server.GlonassSatellites.OnNext(byteValue);
        _server.GpsSatellites.OnNext(byteValue);
        _server.QzssSatellites.OnNext(byteValue);
        _server.SbasSatellites.OnNext(byteValue);
        _server.ImesSatellites.OnNext(byteValue);
        _server.CustomMode.OnNext(AsvGbsCustomMode.AsvGbsCustomModeAuto);
        _server.Position.OnNext(new GeoPoint(65,-65,1001.0));
        
        ServerTime.Advance(TimeSpan.FromSeconds(10));
        ClientTime.Advance(TimeSpan.FromSeconds(10));
        
        // Assert
        var status = await tcs.Task;
        var modeFromServer = await tcsMode.Task;
        Assert.NotNull(status);
        Assert.Equal(AsvGbsCustomMode.AsvGbsCustomModeAuto, modeFromServer);
        Assert.Equal(65.53*100, status.Accuracy);// this is because of the fixed point conversion m => mm
        Assert.Equal(ushortValue, status.Observation);
        Assert.Equal(ushortValue, status.DgpsRate);
        Assert.Equal(byteValue, status.SatAll);
        Assert.Equal(byteValue, status.SatGal);
        Assert.Equal(byteValue, status.SatBdu);
        Assert.Equal(byteValue, status.SatGlo);
        Assert.Equal(byteValue, status.SatGps);
        Assert.Equal(byteValue, status.SatQzs);
        Assert.Equal(byteValue, status.SatSbs);
        Assert.Equal(byteValue, status.SatIme);
        Assert.Equal(65*10e6, status.Lat);     // this is because of the fixed point conversion         
        Assert.Equal(-65*10e6, status.Lng); // this is because of the fixed point conversion
        Assert.Equal(1001000, status.Alt);          
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}