using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public interface IFtpClientEx
{
    Task ReadFile(string serverPath, string filePath, CancellationToken cancel);
    Task BurstReadFile(string serverPath, string filePath, CancellationToken cancel, uint offset = 0);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="serverPath"></param>
    /// <param name="filePath"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task UploadFile(string serverPath, string filePath, CancellationToken cancel);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task RemoveFile(string path, CancellationToken cancel);
    Task TruncateFile(string path, uint offset, CancellationToken cancel);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task<List<FtpEntryItem>> ListDirectory(string path, CancellationToken cancel);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task CreateDirectory(string path, CancellationToken cancel);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task RemoveDirectory(string path, CancellationToken cancel);
}