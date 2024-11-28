using System;
using System.Diagnostics;
using Asv.IO;
using Asv.Mavlink.Common;


namespace Asv.Mavlink;

public class ParamSetFormatter : IProtocolMessageFormatter
{
    public int Order  => int.MaxValue/2;
    public bool CanPrint(IProtocolMessage message)
    {
        return message is ParamSetPacket;
    }

    public string Print(IProtocolMessage packet, PacketFormatting formatting = PacketFormatting.Inline)
    {
        var param = packet as ParamSetPacket;
        Debug.Assert(param != null, nameof(param) + " != null");
        
        var name = new Span<char>(param.Payload.ParamId).TrimEnd('\0').ToString();
        var cValue = MavParamCStyleEncoding.Instance.ConvertFromMavlinkUnion(param.Payload.ParamValue, param.Payload.ParamType);
        var bValue = MavParamByteWiseEncoding.Instance.ConvertFromMavlinkUnion(param.Payload.ParamValue, param.Payload.ParamType);
        return $"{name} = cstyle({cValue}) or byteWise({bValue})";
    }

    public string Name => "Param";
}