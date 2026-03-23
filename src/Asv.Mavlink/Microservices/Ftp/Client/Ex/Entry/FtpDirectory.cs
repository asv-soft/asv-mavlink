namespace Asv.Mavlink;

/// <inheritdoc/>
public class FtpDirectory : IFtpEntry
{
    /// <summary>
    /// Initializes a new <see cref="FtpDirectory"/> instance.
    /// </summary>
    /// <param name="name">The directory name (without the parent path).</param>
    /// <param name="parentPath">The parent directory path that contains this directory.</param>
    public FtpDirectory(string name, string parentPath)
    {
        ParentPath = parentPath;
        Name = name;
        Path = $"{ParentPath}{Name}{MavlinkFtpHelper.DirectorySeparator}";
    }

    /// <summary>
    /// Initializes a new <see cref="FtpDirectory"/> instance with an empty parent path.
    /// </summary>
    /// <param name="name">The directory name (without the parent path).</param>
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