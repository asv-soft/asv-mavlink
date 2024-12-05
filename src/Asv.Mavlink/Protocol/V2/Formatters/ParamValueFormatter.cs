using System;
using System.Diagnostics;
using Asv.IO;
using Asv.Mavlink.Common;
using DotNext.Patterns;


namespace Asv.Mavlink;

public class ParamValueFormatter : IProtocolMessageFormatter, ISingleton<ParamValueFormatter>
{
    public static ParamValueFormatter Instance { get; } = new();

    private ParamValueFormatter()
    {
        
    }
    public int Order => int.MaxValue/2;
    
    public bool CanPrint(IProtocolMessage packet)
    {
        return packet is ParamValuePacket;
    }

    public string Print(IProtocolMessage packet, PacketFormatting formatting)
    {
        var param = packet as ParamValuePacket;
        Debug.Assert(param != null, nameof(param) + " != null");
        
        var name = new Span<char>(param.Payload.ParamId).TrimEnd('\0').ToString();
        var cValue = MavParamCStyleEncoding.Instance.ConvertFromMavlinkUnion(param.Payload.ParamValue, param.Payload.ParamType);
        var bValue = MavParamByteWiseEncoding.Instance.ConvertFromMavlinkUnion(param.Payload.ParamValue, param.Payload.ParamType);
        return $"{name}[{param.Payload.ParamIndex} of {param.Payload.ParamCount}]=cstyle({cValue}) or byteWise({bValue})";
    }

    public string Name => "ParamValue";
    
}