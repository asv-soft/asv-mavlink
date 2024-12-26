using Asv.IO;
using Asv.Mavlink.Common;
using DotNext.Patterns;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Asv.Mavlink;

public class GpsRtcmDataPacketFormatter : ProtocolMessageFormatter<GpsRtcmDataPacket>, ISingleton<GpsRtcmDataPacketFormatter>
{
    private static readonly JsonConverter[] Converters = [new StringEnumConverter(new CamelCaseNamingStrategy())];
    
    protected override string Print(GpsRtcmDataPacket packet, PacketFormatting formatting)
    {
        return $"{packet.Name}: FLAGS:{packet.Payload.Flags:b16} DATA[{packet.Payload.Len}]";
    }

    public override string Name => "Mavlink RtcmData";
    public override int Order => int.MaxValue/2;
    public static GpsRtcmDataPacketFormatter Instance { get; } = new();
}