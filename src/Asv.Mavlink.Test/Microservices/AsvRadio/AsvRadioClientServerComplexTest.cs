using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvAudio;
using Asv.Mavlink.V2.AsvRadio;
using Asv.Mavlink.V2.Common;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class AsvRadioClientServerComplexTest
    : ComplexTestBase<AsvRadioClientEx, AsvRadioServerEx>
{
    private readonly HeartbeatClientConfig _heartbeatConfigClient = new()
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

    private readonly AsvRadioCapabilities _capabilities = AsvRadioCapabilities.Empty;

    private readonly IReadOnlySet<AsvAudioCodec> _codecs =
        new HashSet<AsvAudioCodec>((byte)AsvRadioCodecCapabilitiesResponsePayload.CodecsMaxItemsCount);

    private AsvAudioCodec[] _codecsCollection ;
    
    private readonly AsvRadioServerConfig _radioConfig = new()
    {
        StatusRateMs = 1000
    };

    private readonly MavlinkHeartbeatServerConfig _heartbeatConfigServer = new()
    {
        HeartbeatRateMs = 1000
    };

    private readonly StatusTextLoggerConfig _statusConfig = new()
    {
        MaxQueueSize = 100,
        MaxSendRateHz = 100
    };
    private readonly TaskCompletionSource<IPacketV2<IPayload>> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    public AsvRadioClientServerComplexTest(ITestOutputHelper output): base(output)
    {
        _taskCompletionSource = new TaskCompletionSource<IPacketV2<IPayload>>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5), TimeProvider.System);
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    [Fact]
    public async Task Client_EnableRadio_Success()
    {
        //Arrange
        var client = Client;
        var server = Server;
        server.EnableRadio += async (hz, modulation, dbm, powerDbm, codec, cancel) =>
        {
            return await Task.Run(() => MavResult.MavResultAccepted, cancel);
        };
        server.Start();
        //Act
        var result = await client.EnableRadio(10000001, AsvRadioModulation.AsvRadioModulationFm, (float)-90.0,
            (float)90.0,
            AsvAudioCodec.AsvAudioCodecAac,_cancellationTokenSource.Token);
        //Assert
        Assert.True(result == MavResult.MavResultAccepted);
        Assert.Equal(AsvRadioCustomMode.AsvRadioCustomModeOnair, server.CustomMode.CurrentValue);
     }

    [Fact]
    public async Task Client_EnableRadio_Failure()
    {
        //Arrange
        var client = Client;
        var server = Server;
        server.EnableRadio += async (hz, modulation, dbm, powerDbm, codec, cancel) =>
        {
            return await Task.Run(() => MavResult.MavResultAccepted, cancel);
        };
        server.Start();

        //Act
        var result = await client.EnableRadio(10000001, AsvRadioModulation.AsvRadioModulationFm, (float)-100.99,
            (float)90.0,
            AsvAudioCodec.AsvAudioCodecAac, _cancellationTokenSource.Token);

        //Assert
        Assert.True(result == MavResult.MavResultFailed);
        
        Assert.Equal(AsvRadioCustomMode.AsvRadioCustomModeIdle, server.CustomMode.CurrentValue);
    }

    [Fact]
    public async Task Client_DisableRadio_Success()
    {
        //Arrange
        Server.EnableRadio += async (hz, modulation, dbm, powerDbm, codec, cancel) =>
        {
            return await Task.Run(() => MavResult.MavResultAccepted, cancel);
        };
        Server.DisableRadio += async cancel => { return await Task.Run(() => MavResult.MavResultAccepted, cancel); };
        Server.Start();

        //Act
        ClientTime.Advance(TimeSpan.FromSeconds(1));
        ServerTime.Advance(TimeSpan.FromSeconds(1));
        var result = await Client.EnableRadio(10000001, AsvRadioModulation.AsvRadioModulationFm, (float)-99.0,
            (float)90.0,
            AsvAudioCodec.AsvAudioCodecAac, _cancellationTokenSource.Token);

        //Assert
        Assert.True(result == MavResult.MavResultAccepted);
        Assert.Equal(AsvRadioCustomMode.AsvRadioCustomModeOnair, Server.CustomMode.CurrentValue);
        result = await Client.DisableRadio(_cancellationTokenSource.Token);
        Assert.True(result == MavResult.MavResultAccepted);
        Assert.Equal(AsvRadioCustomMode.AsvRadioCustomModeIdle, Server.CustomMode.CurrentValue);
    }

    [Fact]
    public async Task Client_RequestCodecCapabilities_Success()
    {
        //Arrange
        _codecsCollection = new AsvAudioCodec[100];
        for (int i = 0; i < 99; i++)
        {
            _codecsCollection[i] = AsvAudioCodec.AsvAudioCodecUnknown;
        }
        var client = Client;
        var server = Server;
        var payload = new AsvRadioCodecCapabilitiesResponsePayload();
        server.Start();

        //Act
        var result = await client.Base.RequestCodecCapabilities();

        //Assert
        Assert.Equal(result.Codecs, _codecsCollection);
        Assert.Equal(result.Count, payload.Count);
    }

    [Fact]
    public async Task Client_RequestCapabilities_Success()
    {
        //Arrange
        var client = Client;
        var server = Server;
        server.Start();

        //Act
        var result = await client.Base.RequestCapabilities();

        //Assert
        Assert.Equal(result.MaxRfFreq, _capabilities.MaxFrequencyHz);
        Assert.Equal(result.MaxRxPower, _capabilities.MaxReferencePowerDbm);
        Assert.Equal(result.MinRxPower, _capabilities.MinReferencePowerDbm);
        Assert.Equal(result.MaxTxPower, _capabilities.MaxTxPowerDbm);
        Assert.Equal(result.MinTxPower, _capabilities.MinTxPowerDbm);
        Assert.Equal(result.MinRfFreq, _capabilities.MinFrequencyHz);
    }

    protected override AsvRadioServerEx CreateServer(MavlinkIdentity identity, ICoreServices core)
    {
        var srv = new AsvRadioServer(identity, _radioConfig, core);
        var hb = new HeartbeatServer(identity, _heartbeatConfigServer, core);
        var cmd = new CommandLongServerEx(new CommandServer(identity, core));
        var status = new StatusTextServer(identity, _statusConfig, core);
        return new AsvRadioServerEx(_capabilities, _codecs, srv, hb, cmd, status);
    }

    protected override AsvRadioClientEx CreateClient(MavlinkClientIdentity identity, ICoreServices core)
    {
        return new AsvRadioClientEx(new AsvRadioClient(identity, core),
            new HeartbeatClient(identity, _heartbeatConfigClient, core),
            new CommandClient(identity, _commandConfig, core));
    }
}