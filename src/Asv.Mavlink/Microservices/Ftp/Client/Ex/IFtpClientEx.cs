using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public interface IFtpClientEx
{
    public IFtpClient Client { get; }
    /// <summary>
    /// Reads whole file from server path to client path
    /// </summary>
    /// <param name="serverPath">Server path</param>
    /// <param name="filePath">Client path</param>
    /// <param name="cancel">Cancellation token</param>
    /// <returns>Nothing</returns>
    Task ReadFile(string serverPath, string filePath, CancellationToken cancel);
    /// <summary>
    /// Reading whole file from server path to client path by bursts (Recommended to use due to efficiency and speed)
    /// </summary>
    /// <param name="serverPath">Server path</param>
    /// <param name="filePath">Client path</param>
    /// <param name="cancel">Cancellation token</param>
    /// <param name="offset">Preferred start reading offset</param>
    /// <returns>Nothing</returns>
    Task BurstReadFile(string serverPath, string filePath, CancellationToken cancel, uint offset = 0);
    /// <summary>
    /// Uploads file from client path to server path
    /// </summary>
    /// <param name="serverPath">Server path</param>
    /// <param name="filePath">Client path</param>
    /// <param name="cancel">Cancellation token</param>
    /// <returns>Nothing</returns>
    Task UploadFile(string serverPath, string filePath, CancellationToken cancel);
    /// <summary>
    /// Removes file from server
    /// </summary>
    /// <param name="path">File path on server</param>
    /// <param name="cancel">Cancellation token</param>
    /// <returns>Nothing</returns>
    Task RemoveFile(string path, CancellationToken cancel);
    Task TruncateFile(string path, uint offset, CancellationToken cancel);
    /// <summary>
    /// Lists all server directory entries
    /// </summary>
    /// <param name="path">Server directory path</param>
    /// <param name="cancel">Cancellation token</param>
    /// <returns>List of files and directories</returns>
    Task<List<FtpEntryItem>> ListDirectory(string path, CancellationToken cancel);
    /// <summary>
    /// Creates directory on server
    /// </summary>
    /// <param name="path">Directory name</param>
    /// <param name="cancel">Cancellation token</param>
    /// <returns>Nothing</returns>
    Task CreateDirectory(string path, CancellationToken cancel);
    /// <summary>
    /// Removes directory from server
    /// </summary>
    /// <param name="path">Directory name</param>
    /// <param name="cancel">Cancellation token</param>
    /// <returns>Nothing</returns>
    Task RemoveDirectory(string path, CancellationToken cancel);
}