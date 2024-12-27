using System;
using System.Diagnostics;
using Asv.IO;
using Asv.Mavlink.Common;
using DotNext.Patterns;


namespace Asv.Mavlink;

public class ParamSetFormatter : IProtocolMessageFormatter, ISingleton<ParamSetFormatter>
{
    public static ParamSetFormatter Instance { get; } = new();

    private ParamSetFormatter()
    {
        
    }
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
        
        var cValue = MavParamCStyleEncoding.Instance.IsValidType(param.Payload.ParamType) 
            ? MavParamCStyleEncoding.Instance.ConvertFromMavlinkUnion(param.Payload.ParamValue, param.Payload.ParamType).ToString() 
            : $"Invalid type({param.Payload.ParamType:G})";

        var bValue = MavParamByteWiseEncoding.Instance.IsValidType(param.Payload.ParamType) 
            ? MavParamByteWiseEncoding.Instance.ConvertFromMavlinkUnion(param.Payload.ParamValue, param.Payload.ParamType).ToString() 
            : $"Invalid type({param.Payload.ParamType:G})";
        return $"{name} = cstyle({cValue}) or byteWise({bValue})";
    }

    public string Name => "Param";
    
}