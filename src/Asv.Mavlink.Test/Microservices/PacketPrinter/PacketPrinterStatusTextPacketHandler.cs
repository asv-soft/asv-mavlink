using System.Text;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Moq;
using Xunit;

namespace Asv.Mavlink.Test;

public class PacketPrinterStatusTextPacketHandler
{
    private readonly StatusTextHandler _handler;
    private readonly StatustextPacket _packet;

    public PacketPrinterStatusTextPacketHandler()
    {
        _handler = new StatusTextHandler();
        var text = "StatusText";
        _packet = new StatustextPacket()
        {
            Payload =
            {
                Severity = MavSeverity.MavSeverityAlert,
                ChunkSeq = 1,
                Id = 1,
                Text = text.ToCharArray()
            }
        };
    }

    [Fact]
    public void Order_ShouldReturnHalfOfMaxValue_Success()
    {
        // Act
        var order = _handler.Order;

        // Assert
        Assert.Equal(int.MaxValue / 2, order);
    }

    [Fact]
    public void CanPrint_ShouldReturnTrue_WhenMessageIdMatches_Success()
    {
        // Arrange
        var mockPacket = new Mock<IPacketV2<IPayload>>();
        mockPacket.Setup(p => p.MessageId).Returns(ParamValuePacket.PacketMessageId);

        // Act
        var canPrint = _handler.CanPrint(mockPacket.Object);

        // Assert
        Assert.True(canPrint);
    }

    [Fact]
    public void CanPrint_ShouldReturnFalse_WhenMessageIdDoesNotMatch_Fail()
    {
        // Arrange
        var mockPacket = new HeartbeatPacket();

        // Act
        var canPrint = _handler.CanPrint(mockPacket);

        // Assert
        Assert.False(canPrint);
    }

    [Fact]
    public void Print_ShouldReturnCorrectStringRepresentation_Success()
    {
        // Arrange

        var expectedResult = new StringBuilder();
        expectedResult.Append("{");
        expectedResult.Append("\"Severity\":");
        expectedResult.Append($"{_packet.Payload.Severity},");
        expectedResult.Append("\"Text\":");
        expectedResult.Append($"\"{MavlinkTypesHelper.GetString(_packet.Payload.Text)}\"");
        expectedResult.Append("}");

        // Act
        var result = _handler.Print(_packet);

        // Assert
        Assert.Equal(expectedResult.ToString(), result);
    }
}