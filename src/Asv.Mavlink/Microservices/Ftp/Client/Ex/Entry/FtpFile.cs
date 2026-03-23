namespace Asv.Mavlink;

/// <inheritdoc/>
public class FtpFile : IFtpEntry
{
    /// <summary>
    /// Initializes a new <see cref="FtpFile"/> instance.
    /// </summary>
    /// <param name="name">The file name (without the parent path).</param>
    /// <param name="size">The file size.</param>
    /// <param name="parentPath">The parent directory path that contains this file.</param>
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
    
    /// <summary>
    /// Gets the file size in bytes.
    /// </summary>
    public uint Size { get; }
    
    public override string ToString() => $"[F] {Path} (size: {Size})";
}