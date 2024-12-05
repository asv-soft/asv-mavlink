using System;
using System.Diagnostics.Metrics;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.AsvAudio;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using R3;
using Xunit;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AudioDevice))]
public class AudioDeviceTest
{
    private readonly Mock<Func<Action<AsvAudioStreamPacket>, CancellationToken, ValueTask>> _sendPacketDelegateMock;
    private readonly Mock<IAudioCodecFactory> _codecFactoryMock;
    private readonly Mock<IAudioEncoder> _encoderMock;
    private readonly Mock<IAudioDecoder> _decoderMock;
    private readonly Subject<ReadOnlyMemory<byte>> _encoderOutputSubject;
    private readonly Subject<ReadOnlyMemory<byte>> _decoderOutputSubject;
    private readonly Mock<OnRecvAudioDelegate> _onRecvAudioMock;

    public AudioDeviceTest()
    {
        _sendPacketDelegateMock = new Mock<Func<Action<AsvAudioStreamPacket>, CancellationToken, ValueTask>>();
        Mock<ICoreServices> coreServicesMock = new();
        _codecFactoryMock = new Mock<IAudioCodecFactory>();
        _encoderMock = new Mock<IAudioEncoder>();
        _decoderMock = new Mock<IAudioDecoder>();
        _encoderOutputSubject = new Subject<ReadOnlyMemory<byte>>();
        _decoderOutputSubject = new Subject<ReadOnlyMemory<byte>>();
        _onRecvAudioMock = new Mock<OnRecvAudioDelegate>();

        coreServicesMock.Setup(c => c.TimeProvider).Returns(TimeProvider.System);

        _encoderMock.Setup(e => e.Output).Returns(_encoderOutputSubject);
        _decoderMock.Setup(d => d.Output).Returns(_decoderOutputSubject);
        _codecFactoryMock.Setup(f =>
                f.CreateEncoder(It.IsAny<AsvAudioCodec>(), It.IsAny<Observable<ReadOnlyMemory<byte>>>()))
            .Returns(_encoderMock.Object);
        _codecFactoryMock.Setup(f =>
                f.CreateDecoder(It.IsAny<AsvAudioCodec>(), It.IsAny<Observable<ReadOnlyMemory<byte>>>()))
            .Returns(_decoderMock.Object);
    }

    [Fact]
    public Task SendAudio_EncodingTrigger_Success()
    {
        // Arrange
        var device = CreateAudioDevice();

        var audioData = new byte[10];
        var audioMemory = new ReadOnlyMemory<byte>(audioData);

        // Act
        device.SendAudio(audioMemory);

        var encodedData = new byte[10];
        _encoderOutputSubject.OnNext(new ReadOnlyMemory<byte>(encodedData));

        // Assert
        _sendPacketDelegateMock.Verify(s => s(It.IsAny<Action<AsvAudioStreamPacket>>(), It.IsAny<CancellationToken>()),
            Times.AtLeastOnce);
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Dispose_Success()
    {
        // Arrange
        var device = CreateAudioDevice();

        // Act
        await device.DisposeAsync();

        // Assert
        _encoderMock.Verify(e => e.DisposeAsync(), Times.Once);
        _decoderMock.Verify(d => d.DisposeAsync(), Times.Once);
    }

    [Fact]
    public Task SendAudio_Empty_Null()
    {
        // Arrange
        var device = CreateAudioDevice();
        var emptyAudioData = ReadOnlyMemory<byte>.Empty;

        // Act
        device.SendAudio(emptyAudioData);

        // Assert
        _sendPacketDelegateMock.Verify(s => s(It.IsAny<Action<AsvAudioStreamPacket>>(), It.IsAny<CancellationToken>()),
            Times.Never);
        return Task.CompletedTask;
    }

    [Fact]
    public async Task SendAudio_Throws()
    {
        // Arrange
        var device = CreateAudioDevice();
        _sendPacketDelegateMock
            .Setup( s => s(It.IsAny<Action<AsvAudioStreamPacket>>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Test exception"));

        var audioData = new byte[10];
        var audioMemory = new ReadOnlyMemory<byte>(audioData);

        // Act and Assert
        var exception = await Record.ExceptionAsync(() =>
        {
            device.SendAudio(audioMemory);
            _encoderOutputSubject.OnNext(audioMemory);
            return Task.CompletedTask;
        });

        Assert.Null(exception);
    }

    [Fact]
    public Task SendAudio_Increment_Success()
    {
        // Arrange
        var device = CreateAudioDevice();
        var audioData = new byte[10];
        var audioMemory = new ReadOnlyMemory<byte>(audioData);

        // Act
        device.SendAudio(audioMemory);
        _encoderOutputSubject.OnNext(audioMemory);
        device.SendAudio(audioMemory);
        _encoderOutputSubject.OnNext(audioMemory);

        // Assert
        _sendPacketDelegateMock.Verify(s => s(It.IsAny<Action<AsvAudioStreamPacket>>(), It.IsAny<CancellationToken>()),
            Times.AtLeast(2));
        return Task.CompletedTask;
    }

    [Fact]
    public async Task SendAudio_LogFailure_Success()
    {
        // Arrange
        var device = CreateAudioDevice();

        _sendPacketDelegateMock
            .Setup(s => s(It.IsAny<Action<AsvAudioStreamPacket>>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Test exception"));

        var audioData = new byte[10];
        var audioMemory = new ReadOnlyMemory<byte>(audioData);

        // Act and Assert
        var exception = await Record.ExceptionAsync(() =>
        {
            device.SendAudio(audioMemory);
            _encoderOutputSubject.OnNext(audioMemory);
            return Task.CompletedTask;
        });

        Assert.Null(exception);
    }

    [Fact]
    public Task OnInputAudioStream_FrameSeqChanges_Success()
    {
        // Arrange
        var device = CreateAudioDevice();
        var initialFrame = new AsvAudioStreamPayload { FrameSeq = 1, PktSeq = 0, PktInFrame = 1, Data = new byte[10] };
        var newFrame = new AsvAudioStreamPayload { FrameSeq = 2, PktSeq = 0, PktInFrame = 1, Data = new byte[10] };

        // Act
        device.OnInputAudioStream(initialFrame);
        device.OnInputAudioStream(newFrame);

        // Assert
        _decoderOutputSubject.Subscribe(data => { Assert.Empty(data.ToArray()); });
        return Task.CompletedTask;
    }

    [Fact]
    public Task SendAudio_NormalData_Success()
    {
        // Arrange
        var device = CreateAudioDevice();
        var audioData = new byte[10];
        var audioMemory = new ReadOnlyMemory<byte>(audioData);

        // Act
        device.SendAudio(audioMemory);
        _encoderOutputSubject.OnNext(audioMemory);

        // Assert
        _sendPacketDelegateMock.Verify(
            s => s(It.IsAny<Action<AsvAudioStreamPacket>>(), It.IsAny<CancellationToken>()),
            Times.AtLeastOnce);
        return Task.CompletedTask;
    }

    private AudioDevice CreateAudioDevice()
    {
        var coreServices = new FakeCoreServices();

        return new AudioDevice(
            _codecFactoryMock.Object,
            AsvAudioCodec.AsvAudioCodecOpus8000Mono,
            new AsvAudioOnlinePacket(),
            _sendPacketDelegateMock.Object,
            _onRecvAudioMock.Object,
            coreServices
        );
    }

    private class FakeCoreServices : ICoreServices
    {
        public IProtocolConnection Connection { get; } = Mock.Of<IProtocolConnection>();
        public IPacketSequenceCalculator Sequence { get; } = Mock.Of<IPacketSequenceCalculator>();
        public ILoggerFactory Log { get; } = new FakeLoggerFactory();
        public TimeProvider TimeProvider { get; } = TimeProvider.System;
        public IMeterFactory Metrics { get; } = Mock.Of<IMeterFactory>();

        private class FakeLoggerFactory : ILoggerFactory
        {
            public ILogger CreateLogger(string categoryName)
            {
                if (categoryName == typeof(AudioDevice).FullName)
                {
                    return NullLogger<AudioDevice>.Instance;
                }

                return NullLogger.Instance;
            }

            public void AddProvider(ILoggerProvider provider)
            {
            }

            public void Dispose()
            {
            }
        }
    }
}