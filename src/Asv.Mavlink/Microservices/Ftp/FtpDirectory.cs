namespace Asv.Mavlink;

public class FtpDirectory : IFtpEntry
{
    public FtpDirectory(string name, string parentPath)
    {
        ParentPath = parentPath;
        Name = name;
        Path = $"{ParentPath}{Name}{MavlinkFtpHelper.DirectorySeparator}";
    }
    public FtpDirectory(string name)
        :this(name,string.Empty)
    {
        
    }
    
    public string ParentPath { get; }
    public string Path { get; }
    public string Name { get; }
    public FtpEntryType Type => FtpEntryType.Directory;

    public override string ToString() => $"[D] {Path}";
}