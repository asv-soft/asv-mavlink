using System.Collections.Generic;

namespace Asv.Mavlink.Shell;

public class FtpEntry
{
    public FtpEntry(string key, IFtpEntry item, int depth)
    {
        Key = key;
        Item = item;
        Depth = depth;
    }

    public string Key { get; set; }
    public IFtpEntry Item { get; set; }
    public int Depth { get; set; }
    public bool IsRoot => Depth == 0;
    public ICollection<FtpEntry> Children { get; } = [];
}