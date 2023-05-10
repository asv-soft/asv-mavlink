using System;

namespace Asv.Mavlink;

public interface IFtpServer
{
    public IObservable<FtpMessagePayload> ResetSessionsRequest { get; }
    public IObservable<FtpMessagePayload> TerminateSessionRequest { get; }
    public IObservable<FtpMessagePayload> ListDirectoryRequest { get; }
    public IObservable<FtpMessagePayload> OpenFileRORequest { get; }
    public IObservable<FtpMessagePayload> ReadFileRequest { get; }
    public IObservable<FtpMessagePayload> CreateFileRequest { get; }
    public IObservable<FtpMessagePayload> WriteFileRequest { get; }
    public IObservable<FtpMessagePayload> RemoveFileRequest { get; }
    public IObservable<FtpMessagePayload> CreateDirectoryRequest { get; }
    public IObservable<FtpMessagePayload> RemoveDirectoryRequest { get; }
    public IObservable<FtpMessagePayload> OpenFileWORequest { get; }
    public IObservable<FtpMessagePayload> TruncateFileRequest { get; }
    public IObservable<FtpMessagePayload> RenameRequest { get; }
    public IObservable<FtpMessagePayload> CalcFileCRC32Request { get; }
    public IObservable<FtpMessagePayload> BurstReadFileRequest { get; }
}