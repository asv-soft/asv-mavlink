namespace Asv.Mavlink;

/// <summary>
/// Specifies the type of entry returned by the MAVLink FTP directory listing.
/// </summary>
public enum FtpEntryType
{
    /// <summary>
    /// A regular file.
    /// </summary>
    File,
    
    /// <summary>
    /// A directory (folder).
    /// </summary>
    Directory
}

/// <summary>
/// Represents a single file-system entry (file or directory) on a remote MAVLink FTP server.
/// </summary>
public interface IFtpEntry
{
    /// <summary>
    /// Gets the parent directory path of this entry.
    /// </summary>
    string ParentPath { get; }
    
    /// <summary>
    /// Gets the full path of this entry.
    /// </summary>
    string Path { get; }
    
    /// <summary>
    /// Gets the entry name (file or directory name) without its parent path.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Gets the entry type (file or directory).
    /// </summary>
    FtpEntryType Type { get; }
}