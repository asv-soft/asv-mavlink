using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public interface IFtpClientEx
{
    Task DownloadFile(string fileUri, Stream streamToSave, IProgress<double>? progress = null,
        CancellationToken cancel = default);
}