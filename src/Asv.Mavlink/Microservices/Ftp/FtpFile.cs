namespace Asv.Mavlink;

public class FtpFile : IFtpEntry
{
    public FtpFile(string name, uint size, string parentPath)
    {
        Name = name;
        Size = size;
        ParentPath = parentPath;
        Path = $"{ParentPath}{Name}";
    }
    public string ParentPath { get; }
    public string Path { get; }
    public string Name { get; }
    public FtpEntryType Type => FtpEntryType.File;
    public uint Size { get; }
    public override string ToString() => $"[F] {Path} (size: {Size})";
}