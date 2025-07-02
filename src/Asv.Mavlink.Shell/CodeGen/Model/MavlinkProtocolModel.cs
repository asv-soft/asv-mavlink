using System.Collections.Generic;

namespace Asv.Mavlink.Shell;

public class MavlinkProtocolModel
{
    public string? FileName { get; set; }
    public int Version { get; set; }
    public int Dialect { get; set; }
    public IList<string> Include { get; } = new List<string>();
    public IList<MavlinkEnumModel> Enums { get; } = new List<MavlinkEnumModel>();
    public IList<MavlinkMessageModel> Messages { get; } = new List<MavlinkMessageModel>();
}
