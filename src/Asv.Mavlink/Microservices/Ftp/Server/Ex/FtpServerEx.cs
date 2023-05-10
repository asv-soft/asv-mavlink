using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink;

public class FtpServerEx : DisposableOnceWithCancel, IFtpServerEx
{
    private Dictionary<byte, FileStream> _sessions;
    public FtpServerEx(IFtpServer server)
    {
        _sessions = new Dictionary<byte, FileStream>();
        
        server.TerminateSessionRequest.Subscribe(OnTerminateSession)
            .DisposeItWith(Disposable);
        server.ListDirectoryRequest.Subscribe(OnListDirectory)
            .DisposeItWith(Disposable);
        server.OpenFileRORequest.Subscribe(OnOpenFileRO)
            .DisposeItWith(Disposable);
        server.ReadFileRequest.Subscribe(OnReadFile)
            .DisposeItWith(Disposable);
        server.CreateFileRequest.Subscribe(OnCreateFile)
            .DisposeItWith(Disposable);
        server.WriteFileRequest.Subscribe(OnWriteFile)
            .DisposeItWith(Disposable);
        server.RemoveFileRequest.Subscribe(RemoveFile)
            .DisposeItWith(Disposable);
        server.CreateDirectoryRequest.Subscribe(OnCreateDirectory)
            .DisposeItWith(Disposable);
        server.RemoveDirectoryRequest.Subscribe(OnRemoveDirectory)
            .DisposeItWith(Disposable);
        server.OpenFileWORequest.Subscribe(OnOpenFileWO)
            .DisposeItWith(Disposable);
        server.TruncateFileRequest.Subscribe(OnTruncateFile)
            .DisposeItWith(Disposable);
        server.RenameRequest.Subscribe(OnRename)
            .DisposeItWith(Disposable);
        server.CalcFileCRC32Request.Subscribe(OnCalcFileCRC32)
            .DisposeItWith(Disposable);
        server.BurstReadFileRequest.Subscribe(OnBurstReadFile)
            .DisposeItWith(Disposable);
    }

    private void OnBurstReadFile(FtpMessagePayload ftpMessagePayload)
    {
        
    }

    private void OnCalcFileCRC32(FtpMessagePayload ftpMessagePayload)
    {
        
    }

    private void OnTruncateFile(FtpMessagePayload ftpMessagePayload)
    {
        
    }

    private void OnOpenFileWO(FtpMessagePayload ftpMessagePayload)
    {
        _sessions[ftpMessagePayload.Session] = new FileStream(MavlinkTypesHelper.GetString(ftpMessagePayload.Data), 
            FileMode.Truncate, FileAccess.Write);
    }

    private void OnRemoveDirectory(FtpMessagePayload ftpMessagePayload)
    {
        
    }

    private void OnCreateDirectory(FtpMessagePayload ftpMessagePayload)
    {
        
    }

    private void RemoveFile(FtpMessagePayload ftpMessagePayload)
    {
        
    }

    private void OnWriteFile(FtpMessagePayload ftpMessagePayload)
    {
        
    }

    private void OnCreateFile(FtpMessagePayload ftpMessagePayload)
    {
        
    }

    private void OnReadFile(FtpMessagePayload ftpMessagePayload)
    {
        
    }

    private void OnOpenFileRO(FtpMessagePayload ftpMessagePayload)
    {
        _sessions[ftpMessagePayload.Session] = new FileStream(MavlinkTypesHelper.GetString(ftpMessagePayload.Data), 
            FileMode.Open, FileAccess.Read);
    }

    private void OnListDirectory(FtpMessagePayload ftpMessagePayload)
    {
        
    }

    private void OnTerminateSession(FtpMessagePayload ftpMessagePayload)
    {
        
    }

    private void OnRename(FtpMessagePayload ftpMessagePayload)
    {
        
    }
}