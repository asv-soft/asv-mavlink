namespace Asv.Mavlink.Shell;

public class FtpEntryModel : IFtpEntry
{
    public string ParentPath { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public FtpEntryType Type { get; set; }
}