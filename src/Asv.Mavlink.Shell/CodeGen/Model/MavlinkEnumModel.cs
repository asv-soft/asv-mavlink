using System.Collections.Generic;

namespace Asv.Mavlink.Shell
{
    public class MavlinkEnumModel:MavlinkModelBase
    {
        public string? Name { get; set; }
        public int Value { get; set; }
        public IList<MavlinkEnumEntryModel> Entries { get; set; } = new List<MavlinkEnumEntryModel>();
        public bool IsFlag { get; set; }
    }
}
