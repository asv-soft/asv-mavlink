using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Time.Testing;
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
    private readonly Mock<IMavlinkContext> _coreServicesMock;
    private readonly MavlinkIdentity _identity;
    private readonly AudioServiceConfig _config;

    public AudioServiceTest()
    {
        Mock<IProtocolConnection> connectionMock = new();
        Subject<IProtocolMessage> rxPipeSubject = new();

        connectionMock.Setup(c => c.OnRxMessage).Returns(rxPipeSubject.AsObservable());

        _coreServicesMock = new Mock<IMavlinkContext>();
        _coreServicesMock.Setup(c => c.Connection).Returns(connectionMock.Object);
        _coreServicesMock.Setup(c => c.LoggerFactory).Returns(NullLoggerFactory.Instance);

        _coreServicesMock.Setup(c => c.TimeProvider).Returns(TimeProvider.System);

        _codecFactoryMock = new Mock<IAudioCodecFactory>();

        _identity = new MavlinkIdentity(1, 1);
        _config = new AudioServiceConfig { DeviceTimeoutMs = 1000, OnlineRateMs = 100, RemoveDeviceCheckDelayMs = 500 };
    }

    [Fact]
    public void GoOnline_Success()
    {
        // Arrange
        using var audioService = CreateAudioService();
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
        using var audioService = CreateAudioService();
        audioService.GoOnline("TestDevice", AsvAudioCodec.AsvAudioCodecOpus8000Mono, true, true);

        // Act
        audioService.GoOffline();

        // Assert
        Assert.False(audioService.IsOnline.CurrentValue);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Dispose_ReleasesAllPeerCodecsAndClearsDevices(bool asynchronous)
    {
        using var fixture = new AudioFixture();
        fixture.Online(2);
        fixture.Online(3);
        Assert.Equal(2, fixture.Service.Devices.Count);

        if (asynchronous) await fixture.Service.DisposeAsync();
        else fixture.Service.Dispose();
        fixture.Service.Dispose();
        await fixture.Service.DisposeAsync();

        Assert.Empty(fixture.Service.Devices);
        Assert.All(fixture.Codecs, codec =>
        {
            Assert.Equal(1, codec.DisposeCount);
            Assert.Equal(asynchronous ? 1 : 0, codec.AsyncDisposeCount);
        });
        fixture.Online(4);
        Assert.Empty(fixture.Service.Devices);
        Assert.Equal(4, fixture.Codecs.Count);
    }

    [Fact]
    public async Task DisposeAsync_AwaitsPendingPeerCodecDisposal()
    {
        using var fixture = new AudioFixture();
        fixture.Online(2);
        var pending = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        fixture.Codecs[0].PendingDisposal = pending.Task;

        var disposal = fixture.Service.DisposeAsync().AsTask();

        Assert.False(disposal.IsCompleted);
        Assert.Equal(1, fixture.Codecs[0].AsyncDisposeCount);
        Assert.Equal(0, fixture.Codecs[0].DisposeCount);
        Assert.Equal(0, fixture.Codecs[1].DisposeCount);
        Assert.Empty(fixture.Service.Devices);
        pending.SetResult();
        await disposal;

        Assert.All(fixture.Codecs, codec => Assert.Equal(1, codec.DisposeCount));
        Assert.All(fixture.Codecs, codec => Assert.Equal(1, codec.AsyncDisposeCount));
    }

    [Fact]
    public void DeviceTimeout_RemovesExpiredPeerAndPreservesRefreshedPeer()
    {
        using var fixture = new AudioFixture();
        fixture.Online(2);
        fixture.Online(3);
        fixture.Time.Advance(TimeSpan.FromMilliseconds(750));
        fixture.Online(3);
        fixture.Time.Advance(TimeSpan.FromMilliseconds(750));

        Assert.Single(fixture.Service.Devices);
        Assert.True(fixture.Service.Devices.ContainsKey(new MavlinkIdentity(3, 1)));
        Assert.Equal(1, fixture.Codecs[0].DisposeCount);
        Assert.Equal(1, fixture.Codecs[1].DisposeCount);
        Assert.Equal(0, fixture.Codecs[2].DisposeCount);
        Assert.Equal(0, fixture.Codecs[3].DisposeCount);

        fixture.Online(2);
        Assert.Equal(2, fixture.Service.Devices.Count);
        Assert.Equal(6, fixture.Codecs.Count);
    }

    private sealed class AudioFixture : IDisposable
    {
        private readonly Subject<IProtocolMessage> _received = new();
        public FakeTimeProvider Time { get; } = new();
        public List<TrackingCodec> Codecs { get; } = [];
        public AudioService Service { get; }

        public AudioFixture()
        {
            var connection = new Mock<IProtocolConnection>();
            connection.Setup(x => x.OnRxMessage).Returns(_received);
            var core = new Mock<IMavlinkContext>();
            core.Setup(x => x.Connection).Returns(connection.Object);
            core.Setup(x => x.LoggerFactory).Returns(NullLoggerFactory.Instance);
            core.Setup(x => x.TimeProvider).Returns(Time);
            core.Setup(x => x.Sequence).Returns(new PacketSequenceCalculator());
            var factory = new Mock<IAudioCodecFactory>();
            factory.Setup(x => x.CreateEncoder(It.IsAny<AsvAudioCodec>(), It.IsAny<Observable<ReadOnlyMemory<byte>>>()))
                .Returns(() => NewCodec());
            factory.Setup(x => x.CreateDecoder(It.IsAny<AsvAudioCodec>(), It.IsAny<Observable<ReadOnlyMemory<byte>>>()))
                .Returns(() => NewCodec());
            Service = new AudioService(factory.Object, new MavlinkIdentity(1, 1),
                new AudioServiceConfig { DeviceTimeoutMs = 1000, OnlineRateMs = 10000, RemoveDeviceCheckDelayMs = 500 }, core.Object);
            Service.GoOnline("Local", AsvAudioCodec.AsvAudioCodecOpus8000Mono, true, true);
        }

        private TrackingCodec NewCodec()
        {
            var codec = new TrackingCodec();
            Codecs.Add(codec);
            return codec;
        }

        public void Online(byte systemId) => _received.OnNext(new AsvAudioOnlinePacket
        {
            SystemId = systemId,
            ComponentId = 1,
            Payload = { Codec = AsvAudioCodec.AsvAudioCodecOpus8000Mono }
        });

        public void Dispose()
        {
            Service.Dispose();
            _received.Dispose();
        }
    }

    private sealed class TrackingCodec : IAudioEncoder, IAudioDecoder
    {
        public Observable<ReadOnlyMemory<byte>> Output => Observable.Empty<ReadOnlyMemory<byte>>();
        public AsvAudioCodec Codec => AsvAudioCodec.AsvAudioCodecOpus8000Mono;
        public int DisposeCount { get; private set; }
        public int AsyncDisposeCount { get; private set; }
        public Task? PendingDisposal { get; set; }
        public void Dispose() => DisposeCount++;
        public async ValueTask DisposeAsync()
        {
            AsyncDisposeCount++;
            if (PendingDisposal != null) await PendingDisposal.ConfigureAwait(false);
            Dispose();
        }
    }

    private AudioService CreateAudioService()
    {
        return new AudioService(_codecFactoryMock.Object, _identity, _config, _coreServicesMock.Object);
    }
}