namespace Asv.Mavlink;

public interface IFtpServerEx
{
    public IFtpServer Server { get; }
    public void OnResetSessions(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
    public void OnBurstReadFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
    public void OnCalcFileCRC32(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
    public void OnTruncateFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
    public void OnOpenFileWO(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
    public void OnRemoveDirectory(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
    public void OnCreateDirectory(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
    public void RemoveFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
    public void OnWriteFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
    public void OnCreateFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
    public void OnReadFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
    public void OnOpenFileRO(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
    public void OnListDirectory(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
    public void OnTerminateSession(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
    public void OnRename(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload);
}