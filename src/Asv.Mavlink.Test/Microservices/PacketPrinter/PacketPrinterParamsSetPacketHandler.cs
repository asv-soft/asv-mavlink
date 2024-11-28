using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Moq;
using Xunit;

namespace Asv.Mavlink.Test;

public class PacketPrinterParamsSetPacketHandler
{
    private readonly ParamSetPacketHandler _handler;
    IMavParamEncoding _cstyleEncoding = new MavParamCStyleEncoding();
    IMavParamEncoding _byteWiseEncoding = new MavParamByteWiseEncoding();

    public PacketPrinterParamsSetPacketHandler()
    {
        _handler = new ParamSetPacketHandler();
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
        mockPacket.Setup(p => p.MessageId).Returns(ParamSetPacket.PacketMessageId);

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
        var name = "Name";
        var setPacket = new ParamSetPacket()
        {
            Payload =
            {
                ParamType = MavParamType.MavParamTypeInt8, TargetSystem = 1, ParamValue = 1,
                ParamId = name.ToCharArray()
            }
        };
        var cValue = _cstyleEncoding.ConvertFromMavlinkUnion(setPacket.Payload.ParamValue, setPacket.Payload.ParamType);
        var bValue =
            _byteWiseEncoding.ConvertFromMavlinkUnion(setPacket.Payload.ParamValue, setPacket.Payload.ParamType);

        var expectedResult = $"{name} = cstyle({cValue}) or byteWise({bValue})";

        // Act
        var result = _handler.Print(setPacket);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Print_ShouldHandleEmptyParamId_Success()
    {
        // Arrange
        var setPacket = new ParamSetPacket()
        {
            Payload =
            {
                ParamType = MavParamType.MavParamTypeInt8, TargetSystem = 1, ParamValue = 1
            }
        };
        var cValue = _cstyleEncoding.ConvertFromMavlinkUnion(setPacket.Payload.ParamValue, setPacket.Payload.ParamType);
        var bValue =
            _byteWiseEncoding.ConvertFromMavlinkUnion(setPacket.Payload.ParamValue, setPacket.Payload.ParamType);

        var expectedResult = $"{string.Empty} = cstyle({cValue}) or byteWise({bValue})";


        // Act
        var result = _handler.Print(setPacket);

        // Assert
        Assert.Equal(expectedResult, result);
    }
}