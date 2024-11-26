using System;
using System.Text;
using Asv.Mavlink.V2.Common;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Asv.Mavlink.Test;

public class PacketPrinterFtpPacketHandlerTest
{
    private readonly FtpPacketHandler _handler;

    public PacketPrinterFtpPacketHandlerTest()
    {
        _handler = new FtpPacketHandler();
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
        mockPacket.Setup(p => p.MessageId).Returns(FileTransferProtocolPacket.PacketMessageId);

        // Act
        var canPrint = _handler.CanPrint(mockPacket.Object);

        // Assert
        Assert.True(canPrint);
    }

    [Fact]
    public void CanPrint_ShouldReturnFalse_WhenMessageIdDoesNotMatch_Fail()
    {
        // Arrange
        var mockPacket = new Mock<IPacketV2<IPayload>>();

        // Act
        var canPrint = _handler.CanPrint(mockPacket.Object);

        // Assert
        Assert.False(canPrint);
    }

    [Fact]
    public void Print_ShouldReturnSerializedFields_WithNoFormatting_Success()
    {
        // Arrange
        var ftpPacket = new FileTransferProtocolPacket()
        {
            Payload = { TargetSystem = 1, TargetComponent = 2, Payload = [1, 2, 3, 4, 5,6,7,8,9,10,11,12,13,14,15,16] }
        };

        var expectedResult = JsonConvert.SerializeObject(new
        {
            brstcmp = ftpPacket.ReadBurstComplete(),
            seq = ftpPacket.ReadSequenceNumber(),
            opCode = ftpPacket.ReadOpcode(),
            size = ftpPacket.ReadSize(),
            offset = ftpPacket.ReadOffset(),
            str = Encoding.ASCII.GetString(ftpPacket.Payload.Payload).TrimEnd('\0')
        }, Formatting.None);
        // Act
        var result = _handler.Print(ftpPacket);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Print_ShouldReturnSerializedFields_WithIndentedFormatting_Success()
    {
        // Arrange
        var ftpPacket = new FileTransferProtocolPacket()
        {
            Payload = { TargetSystem = 1, TargetComponent = 2, Payload = [1, 2, 3, 4, 5,6,7,8,9,10,11,12,13,14,15,16] }
        };

        var expectedResult = JsonConvert.SerializeObject(new
        {
            brstcmp = ftpPacket.ReadBurstComplete(),
            seq = ftpPacket.ReadSequenceNumber(),
            opCode = ftpPacket.ReadOpcode(),
            size = ftpPacket.ReadSize(),
            offset = ftpPacket.ReadOffset(),
            str = Encoding.ASCII.GetString(ftpPacket.Payload.Payload).TrimEnd('\0')
        }, Formatting.Indented);

        // Act
        var result = _handler.Print(ftpPacket, PacketFormatting.Indented);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Print_ShouldThrowException_WhenPacketIsNotFileTransferProtocolPacket_Fail()
    {
        // Arrange
        var mockPacket = new Mock<IPacketV2<IPayload>>();

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => _handler.Print(mockPacket.Object));
    }
}