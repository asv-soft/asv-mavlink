using System;
using System.Diagnostics;
using System.Text;
using Asv.IO;
using Asv.Mavlink.Common;

using Newtonsoft.Json;

namespace Asv.Mavlink;

public class FtpPacketFormatter: IProtocolMessageFormatter
{
    public bool CanPrint(IProtocolMessage message)
    {
        return message is FileTransferProtocolPacket;
    }

    public string Print(IProtocolMessage packet, PacketFormatting formatting)
    {
        if (packet is FileTransferProtocolPacket ftp == false)
        {
            Debug.Assert(false,"packet is FileTransferProtocolPacket ftp == false");
            return string.Empty;
        }
        var payload = ftp.Payload.Payload;
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

    public string Name => "FTP";
    public int Order  => int.MaxValue/2;
   
}