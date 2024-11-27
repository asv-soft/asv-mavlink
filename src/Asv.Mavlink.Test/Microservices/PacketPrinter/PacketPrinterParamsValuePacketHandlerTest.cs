using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Moq;
using Xunit;

namespace Asv.Mavlink.Test;

public class PacketPrinterParamsValuePacketHandlerTest
{
    private readonly ParamValuePacketHandler _handler;
    private readonly ParamValuePacket _packet;
    IMavParamEncoding _cstyleEncoding = new MavParamCStyleEncoding();
    IMavParamEncoding _byteWiseEncoding = new MavParamByteWiseEncoding();

    public PacketPrinterParamsValuePacketHandlerTest()
    {
        _handler = new ParamValuePacketHandler();
        var name = "Name";
        _packet = new ParamValuePacket()
        {
            Payload =
            {
                ParamType = MavParamType.MavParamTypeInt8, ParamValue = 1,
                ParamId = name.ToCharArray(),
                ParamIndex = 1,
                ParamCount = 2
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
        var cValue = _cstyleEncoding.ConvertFromMavlinkUnion(_packet.Payload.ParamValue, _packet.Payload.ParamType);
        var bValue =
            _byteWiseEncoding.ConvertFromMavlinkUnion(_packet.Payload.ParamValue, _packet.Payload.ParamType);

        var expectedResult = $"Name[1 of 2]=cstyle({cValue}) or byteWise({bValue})";

        // Act
        var result = _handler.Print(_packet);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Print_ShouldHandleEmptyParamId_Success()
    {
        // Arrange
        var packet = new ParamValuePacket()
        {
            Payload =
            {
                ParamType = MavParamType.MavParamTypeInt8, ParamValue = 1,
                ParamIndex = 1,
                ParamCount = 2
            }
        };

        var cValue = _cstyleEncoding.ConvertFromMavlinkUnion(packet.Payload.ParamValue, packet.Payload.ParamType);
        var bValue = _byteWiseEncoding.ConvertFromMavlinkUnion(packet.Payload.ParamValue, packet.Payload.ParamType);

        var expectedResult = $"{string.Empty}[1 of 2]=cstyle({cValue}) or byteWise({bValue})";

        // Act
        var result = _handler.Print(packet);

        // Assert
        Assert.Equal(expectedResult, result);
    }
}