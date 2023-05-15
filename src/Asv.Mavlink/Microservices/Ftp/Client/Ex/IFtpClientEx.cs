using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public interface IFtpClientEx
{
    Task ReadFile(string serverPath, string filePath, CancellationToken cancel);
    Task BurstReadFile(string serverPath, string filePath, CancellationToken cancel, uint offset = 0);
    Task UploadFile(string serverPath, string filePath, CancellationToken cancel);
    Task RemoveFile(string path, CancellationToken cancel);
    Task TruncateFile(string path, uint offset, CancellationToken cancel);
    Task<List<FtpEntryItem>> ListDirectory(string path, CancellationToken cancel);
    Task CreateDirectory(string path, CancellationToken cancel);
    Task RemoveDirectory(string path, CancellationToken cancel);
}