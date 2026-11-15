using System;
using System.Collections.Immutable;
using Asv.Mavlink.AsvRsga;
using Xunit;

namespace Asv.Mavlink.Test;

public class UvdProtocolTests
{
    [Fact]
    public void WorkMode_HasDedicatedUvdIdentifier()
    {
        Assert.True(Enum.TryParse<AsvRsgaCustomMode>("AsvRsgaCustomModeUvdReq", out var mode));
        Assert.Equal(58, (int)mode);
    }

    [Fact]
    public void Telemetry_HasDedicatedGeneratedPacket()
    {
        var packetType = typeof(AsvRsgaRttModeACReqPacket).Assembly
            .GetType("Asv.Mavlink.AsvRsga.AsvRsgaRttUvdReqPacket");
        Assert.NotNull(packetType);
        Assert.NotNull(Activator.CreateInstance(packetType));
    }

    [Fact]
    public void Telemetry_IsRegisteredAndFitsRsgaTransport()
    {
        var registry = ImmutableDictionary.CreateBuilder<int, Func<MavlinkMessage>>();
        registry.RegisterAsvRsgaDialect();
        var packet = Assert.IsType<AsvRsgaRttUvdReqPacket>(registry[13472]());
        Assert.Equal(76, packet.Payload.GetMaxByteSize());
        Assert.True(packet.Payload.GetMaxByteSize() <= 237);
        Assert.Equal(124, AsvRsgaRttUvdReqPacket.CrcExtra);
    }

    [Fact]
    public void Telemetry_PayloadRoundTripPreservesSignedRawDataAndStatuses()
    {
        var source = new AsvRsgaRttUvdReqPayload
        {
            RunId = 42, RequestCode = 3, InformationWord = 0xABCDE,
            RawReplyDelay = -80, RawFrequencyOffset = -123,
            Frequency = float.NaN, ReplyDelay = -2, Validity = 0x180,
            NormalReplyCount = 0, SlsReplyCount = 0, AttenuatedSlsReplyCount = 200,
        };
        source.Statuses[0] = 2;
        source.Statuses[6] = 4;
        Span<byte> storage = stackalloc byte[source.GetMaxByteSize()];
        var write = storage;
        source.Serialize(ref write);
        ReadOnlySpan<byte> read = storage;
        var result = new AsvRsgaRttUvdReqPayload();
        result.Deserialize(ref read);
        Assert.Equal(source.RunId, result.RunId);
        Assert.Equal(source.InformationWord, result.InformationWord);
        Assert.Equal(-80, result.RawReplyDelay);
        Assert.Equal(-123, result.RawFrequencyOffset);
        Assert.Equal(source.Statuses, result.Statuses);
        Assert.Equal(source.Validity, result.Validity);
        Assert.Equal(0, result.NormalReplyCount);
        Assert.Equal(200, result.AttenuatedSlsReplyCount);
        Assert.True(float.IsNaN(result.Frequency));
    }
}
