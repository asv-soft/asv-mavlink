using Asv.IO;
using DotNext.Patterns;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Asv.Mavlink;

public class PayloadAsJsonFormatter : ProtocolMessageFormatter<MavlinkMessage>, ISingleton<PayloadAsJsonFormatter>
{
    private static readonly JsonConverter[] Converters = [new StringEnumConverter(new CamelCaseNamingStrategy())];
    
    protected override string Print(MavlinkMessage packet, PacketFormatting formatting)
    {
        return $"{packet.Name}: {JsonConvert.SerializeObject(packet.GetPayload(), formatting == PacketFormatting.Indented ? Formatting.Indented : Formatting.None,Converters)}";
    }

    public override string Name => "Mavlink Json";
    public override int Order => int.MaxValue - 1;
    public static PayloadAsJsonFormatter Instance { get; } = new();
}