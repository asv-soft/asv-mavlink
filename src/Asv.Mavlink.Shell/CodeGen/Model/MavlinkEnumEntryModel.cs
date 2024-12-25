using System.Collections.Generic;

namespace Asv.Mavlink.Shell
{
    public class MavlinkEnumEntryModel: MavlinkModelBase
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public long Value { get; set; }
        public IList<MavlinkEnumEntryParamModel> Params { get; } = new List<MavlinkEnumEntryParamModel>();
        
        public bool HasMetadataDescription { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string MetadataDescription { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}
