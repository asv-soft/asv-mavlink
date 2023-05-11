namespace Asv.Mavlink;

public interface IFtpServerEx
{
    public void OnResetSessions(FtpMessagePayload ftpMessagePayload);
    public void OnBurstReadFile(FtpMessagePayload ftpMessagePayload);
    public void OnCalcFileCRC32(FtpMessagePayload ftpMessagePayload);
    public void OnTruncateFile(FtpMessagePayload ftpMessagePayload);
    public void OnOpenFileWO(FtpMessagePayload ftpMessagePayload);
    public void OnRemoveDirectory(FtpMessagePayload ftpMessagePayload);
    public void OnCreateDirectory(FtpMessagePayload ftpMessagePayload);
    public void RemoveFile(FtpMessagePayload ftpMessagePayload);
    public void OnWriteFile(FtpMessagePayload ftpMessagePayload);
    public void OnCreateFile(FtpMessagePayload ftpMessagePayload);
    public void OnReadFile(FtpMessagePayload ftpMessagePayload);
    public void OnOpenFileRO(FtpMessagePayload ftpMessagePayload);
    public void OnListDirectory(FtpMessagePayload ftpMessagePayload);
    public void OnTerminateSession(FtpMessagePayload ftpMessagePayload);
    public void OnRename(FtpMessagePayload ftpMessagePayload);
}