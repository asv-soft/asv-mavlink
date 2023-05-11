using System;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

public interface IFtpServer
{
    public Task SendFtpPacket(FtpMessagePayload payload, CancellationToken cancel);
    public Task SendFtpPacket(byte[] payload, CancellationToken cancel);
    public IObservable<FtpMessagePayload> AnyRequest { get; }
    public IObservable<FtpMessagePayload> ResetSessionsRequest { get; set; }
    public IObservable<FtpMessagePayload> TerminateSessionRequest { get; set; }
    public IObservable<FtpMessagePayload> ListDirectoryRequest { get; set; }
    public IObservable<FtpMessagePayload> OpenFileRORequest { get; set; }
    public IObservable<FtpMessagePayload> ReadFileRequest { get; set; }
    public IObservable<FtpMessagePayload> CreateFileRequest { get; set; }
    public IObservable<FtpMessagePayload> WriteFileRequest { get; set; }
    public IObservable<FtpMessagePayload> RemoveFileRequest { get; set; }
    public IObservable<FtpMessagePayload> CreateDirectoryRequest { get; set; }
    public IObservable<FtpMessagePayload> RemoveDirectoryRequest { get; set; }
    public IObservable<FtpMessagePayload> OpenFileWORequest { get; set; }
    public IObservable<FtpMessagePayload> TruncateFileRequest { get; set; }
    public IObservable<FtpMessagePayload> RenameRequest { get; set; }
    public IObservable<FtpMessagePayload> CalcFileCRC32Request { get; set; }
    public IObservable<FtpMessagePayload> BurstReadFileRequest { get; set; }
}