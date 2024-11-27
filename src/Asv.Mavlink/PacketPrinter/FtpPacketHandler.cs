using System.Text;
using Asv.Mavlink.Common;
using Asv.Mavlink.V2.Common;
using Newtonsoft.Json;

namespace Asv.Mavlink;

public class FtpPacketHandler: IPacketPrinterHandler
{
    public int Order  => int.MaxValue/2;
    public bool CanPrint(IPacketV2<IPayload> packet)
    {
        return packet.MessageId == FileTransferProtocolPacket.PacketMessageId;
    }

    public string Print(IPacketV2<IPayload> packet, PacketFormatting formatting = PacketFormatting.None)
    {
        var ftp = packet as FileTransferProtocolPacket;
        var payload =ftp.Payload.Payload;
        var brstcmp =  ftp.ReadBurstComplete();
        var seq = ftp.ReadSequenceNumber();
        var size = ftp.ReadSize();
        var opCode = ftp.ReadOpcode();
        var offset = ftp.ReadOffset();
        var str = Encoding.ASCII.GetString(payload).TrimEnd('\0');
        return JsonConvert.SerializeObject(new{
            brstcmp,
            seq,
            opCode,
            size,
            offset,
            str,
        }, formatting == PacketFormatting.Indented ? Formatting.Indented : Formatting.None);
    }
}