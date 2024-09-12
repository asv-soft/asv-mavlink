using System;
using Asv.IO;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class MavlinkFtpMessagePayload
{
    

    public MavlinkFtpMessagePayload(FileTransferProtocolPacket @base)
    {
        Base = @base;    
    }

    public FileTransferProtocolPacket Base { get; }

    public ushort SeqNumber
    {
        get => BitConverter.ToUInt16(Base.Payload.Payload,0);
        set
        {
            
        }
    }
    
    
}