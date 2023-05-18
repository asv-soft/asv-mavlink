using System;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public interface IFtpServer
{
    public Task SendFtpPacket(FtpMessagePayload payload, DeviceIdentity identity, CancellationToken cancel);
    public IObservable<(DeviceIdentity, FtpMessagePayload)> AnyRequest { get; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> ResetSessionsRequest { get; set; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> TerminateSessionRequest { get; set; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> ListDirectoryRequest { get; set; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> OpenFileRORequest { get; set; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> ReadFileRequest { get; set; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> CreateFileRequest { get; set; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> WriteFileRequest { get; set; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> RemoveFileRequest { get; set; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> CreateDirectoryRequest { get; set; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> RemoveDirectoryRequest { get; set; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> OpenFileWORequest { get; set; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> TruncateFileRequest { get; set; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> RenameRequest { get; set; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> CalcFileCRC32Request { get; set; }
    public IObservable<(DeviceIdentity, FtpMessagePayload)> BurstReadFileRequest { get; set; }
}