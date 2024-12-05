using System;
using Asv.IO;
using Asv.Mavlink.AsvAudio;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using R3;
using Xunit;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AudioService))]
public class AudioServiceTest
{
    private readonly Mock<IAudioCodecFactory> _codecFactoryMock;
    private readonly Mock<ICoreServices> _coreServicesMock;
    private readonly MavlinkIdentity _identity;
    private readonly AudioServiceConfig _config;

    public AudioServiceTest()
    {
        Mock<IProtocolConnection> connectionMock = new();
        Subject<IProtocolMessage> rxPipeSubject = new();

        connectionMock.Setup(c => c.OnRxMessage).Returns(rxPipeSubject.AsObservable());

        _coreServicesMock = new Mock<ICoreServices>();
        _coreServicesMock.Setup(c => c.Connection).Returns(connectionMock.Object);
        _coreServicesMock.Setup(c => c.Log).Returns(NullLoggerFactory.Instance);

        _coreServicesMock.Setup(c => c.TimeProvider).Returns(TimeProvider.System);

        _codecFactoryMock = new Mock<IAudioCodecFactory>();

        _identity = new MavlinkIdentity(1, 1);
        _config = new AudioServiceConfig { DeviceTimeoutMs = 1000, OnlineRateMs = 100, RemoveDeviceCheckDelayMs = 500 };
    }

    [Fact]
    public void GoOnline_Success()
    {
        // Arrange
        var audioService = CreateAudioService();
        const AsvAudioCodec codec = AsvAudioCodec.AsvAudioCodecOpus8000Mono;

        // Act
        audioService.GoOnline("TestDevice", codec, true, true);

        // Assert
        Assert.True(audioService.IsOnline.CurrentValue);
        Assert.Equal(codec, audioService.Codec.CurrentValue);
        Assert.True(audioService.SpeakerEnabled.Value);
        Assert.True(audioService.MicEnabled.Value);
    }

    [Fact]
    public void GoOffline_Success()
    {
        // Arrange
        var audioService = CreateAudioService();
        audioService.GoOnline("TestDevice", AsvAudioCodec.AsvAudioCodecOpus8000Mono, true, true);

        // Act
        audioService.GoOffline();

        // Assert
        Assert.False(audioService.IsOnline.CurrentValue);
    }

    private AudioService CreateAudioService()
    {
        return new AudioService(_codecFactoryMock.Object, _identity, _config, _coreServicesMock.Object);
    }
}