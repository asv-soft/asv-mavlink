using System;

using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Asv.Mavlink.Test;

public class PacketPrinterDefaultPacketHandlerTest
{
    private readonly DefaultPacketHandler _handler;

    public PacketPrinterDefaultPacketHandlerTest()
    {
        _handler = new DefaultPacketHandler();
    }

    [Fact]
    public void Order_ShouldReturnMaxValue_Success()
    {
        // Arrange & Act
        var order = _handler.Order;

        // Assert
        Assert.Equal(int.MaxValue, order);
    }

    [Fact]
    public void CanPrint_ShouldAlwaysReturnTrue_Success()
    {
        // Arrange
        var mockPacket = new Mock<MavlinkMessage>();

        // Act
        var canPrint = _handler.CanPrint(mockPacket.Object);

        // Assert
        Assert.True(canPrint);
    }

    [Fact]
    public void Print_ShouldThrowArgumentException_WhenPacketIsNull_Fail()
    {
        // Arrange
        MavlinkMessage packet = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _handler.Print(packet));
        Assert.Equal("Incoming packet was not initialized!", exception.Message);
    }

    [Fact]
    public void Print_ShouldReturnSerializedPayloadWithNoFormatting_Success()
    {
        // Arrange
        var packet = new HeartbeatPacket()
        {
            Payload =
            {
                CustomMode = 1,
                MavlinkVersion = 1,
                SystemStatus = MavState.MavStateActive,
                Autopilot = MavAutopilot.MavAutopilotArdupilotmega,
                BaseMode = MavModeFlag.MavModeFlagAutoEnabled,
                Type = MavType.MavTypeAdsb
            }
        };
        var expectedResult = JsonConvert.SerializeObject(packet.Payload, Formatting.None);

        // Act
        var result = _handler.Print(packet);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Print_ShouldReturnSerializedPayloadWithIntendedPayload_Success()
    {
        // Arrange
        var packet = new HeartbeatPacket()
        {
            Payload =
            {
                CustomMode = 1,
                MavlinkVersion = 1,
                SystemStatus = MavState.MavStateActive,
                Autopilot = MavAutopilot.MavAutopilotArdupilotmega,
                BaseMode = MavModeFlag.MavModeFlagAutoEnabled,
                Type = MavType.MavTypeAdsb
            }
        };
        var expectedResult = JsonConvert.SerializeObject(packet.Payload, Formatting.Indented);

        // Act
        var result = _handler.Print(packet, PacketFormatting.Indented);

        // Assert
        Assert.Equal(expectedResult, result);
    }


    [Fact]
    public void Print_ShouldThrowArgumentException_ForInvalidFormatting_Fail()
    {
        // Arrange
        var payload = new Mock<IPayload>();
        var mockPacket = new Mock<MavlinkMessage>();
        mockPacket.Setup(p => p.Payload).Returns(payload.Object);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _handler.Print(mockPacket.Object, (PacketFormatting)999));
        Assert.Equal("Wrong packet formatting!", exception.Message);
    }
}