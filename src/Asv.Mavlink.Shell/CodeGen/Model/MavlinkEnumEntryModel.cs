using System.Collections.Generic;

namespace Asv.Mavlink.Shell;

public class MavlinkEnumEntryModel: MavlinkModelBase
{
    public string? Name { get; set; }
    public long Value { get; set; }
    public IList<MavlinkEnumEntryParamModel> Params { get; } = new List<MavlinkEnumEntryParamModel>();
    public bool HasMetadataDescription { get; set; }
    public string? MetadataDescription { get; set; }
}