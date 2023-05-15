using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink;

public class FtpServerEx : DisposableOnceWithCancel, IFtpServerEx
{
    private Dictionary<byte, FileStream> _sessions;
    private IFtpServer _server;
    
    public FtpServerEx(IFtpServer server)
    {
        if (server == null) throw new ArgumentNullException(nameof(server));

        _server = server;
        
        _sessions = new Dictionary<byte, FileStream>();
        
        server.TerminateSessionRequest.Subscribe(_ => OnTerminateSession(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
        
        server.ResetSessionsRequest.Subscribe(_ => OnResetSessions(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
        
        server.ListDirectoryRequest.Subscribe(_ => OnListDirectory(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
        
        server.OpenFileRORequest.Subscribe(_ => OnOpenFileRO(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
        
        server.ReadFileRequest.Subscribe(_ => OnReadFile(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
        
        server.CreateFileRequest.Subscribe(_ => OnCreateFile(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
        
        server.WriteFileRequest.Subscribe(_ => OnWriteFile(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
        
        server.RemoveFileRequest.Subscribe(_ => RemoveFile(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
        
        server.CreateDirectoryRequest.Subscribe(_ => OnCreateDirectory(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
        
        server.RemoveDirectoryRequest.Subscribe(_ => OnRemoveDirectory(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
        
        server.OpenFileWORequest.Subscribe(_ => OnOpenFileWO(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
        
        server.TruncateFileRequest.Subscribe(_ => OnTruncateFile(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
        
        server.RenameRequest.Subscribe(_ => OnRename(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
        
        server.CalcFileCRC32Request.Subscribe(_ => OnCalcFileCRC32(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
        
        server.BurstReadFileRequest.Subscribe(_ => OnBurstReadFile(_.Item1, _.Item2))
            .DisposeItWith(Disposable);
    }

    public void OnResetSessions(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        foreach (var sessionNumber in _sessions.Keys)
        {
            _sessions[sessionNumber].Close();
        }
        _sessions.Clear();
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.ResetSessions
        };
        
        _server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnBurstReadFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        var sequence = ftpMessagePayload.SequenceNumber;
        
        var offset = ftpMessagePayload.Offset;
        
        var currentBurst = 0;
        
        if (_sessions.TryGetValue(ftpMessagePayload.Session, out var session))
        {
            using (BinaryReader br = new BinaryReader(session))
            {
                br.BaseStream.Position = offset;
                
                while (true)
                {
                    sequence++;
                    
                    var responsePayload = new FtpMessagePayload
                    {
                        OpCodeId = OpCode.ACK,
                        Session = ftpMessagePayload.Session,
                        ReqOpCodeId = OpCode.BurstReadFile,
                        SequenceNumber = sequence
                    };

                    responsePayload.Data = br.ReadBytes(251 - 12);

                    responsePayload.Offset = offset;
                    
                    responsePayload.Size = (byte)responsePayload.Data.Length;
                    
                    offset += responsePayload.Size;

                    currentBurst += responsePayload.Size;
                    
                    if (currentBurst >= 23900)
                    {
                        responsePayload.BurstComplete = true;

                        _server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);

                        break;
                    }
                    
                    _server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
                }
            }
        }
    }

    public void OnCalcFileCRC32(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        var messagePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.NAK,
            ReqOpCodeId = OpCode.CalcFileCRC32,
            Size = 0,
            Offset = 0,
            Data = new byte[251 - 12]
        };
        
        _server.SendFtpPacket(messagePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnTruncateFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        var messagePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.NAK,
            ReqOpCodeId = OpCode.TruncateFile,
            Size = 0,
            Offset = 0,
            Data = new byte[251 - 12]
        };
        
        _server.SendFtpPacket(messagePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnOpenFileWO(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        _sessions[(byte)(_sessions.Keys.Last() + 1)] = new FileStream(MavlinkTypesHelper.GetString(ftpMessagePayload.Data), 
            FileMode.Truncate, FileAccess.Write);
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.OpenFileWO,
        };
        
        _server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnRemoveDirectory(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        Directory.Delete(MavlinkTypesHelper.GetString(ftpMessagePayload.Data), true);
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.RemoveDirectory,
        };
     
        _server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnCreateDirectory(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        Directory.CreateDirectory(MavlinkTypesHelper.GetString(ftpMessagePayload.Data));
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.CreateDirectory
        };
        
        _server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void RemoveFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        File.Delete(MavlinkTypesHelper.GetString(ftpMessagePayload.Data));
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.RemoveFile,
        };

        _server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnWriteFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        if (_sessions.TryGetValue(ftpMessagePayload.Session, out var session))
        {
            var responsePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.ACK,
                ReqOpCodeId = OpCode.WriteFile,
                Session = ftpMessagePayload.Session
            };
            
            using (BinaryWriter bw = new BinaryWriter(session))
            {
                bw.BaseStream.Position = ftpMessagePayload.Offset;
                
                bw.Write(ftpMessagePayload.Data, 0, ftpMessagePayload.Size);
                
                responsePayload.Size = (byte)responsePayload.Data.Length;
                
                _server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
            }
        }
    }

    public void OnCreateFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        var sessionNumber = _sessions.Keys.Last() + 1;
        _sessions[(byte)sessionNumber] = File.Create(MavlinkTypesHelper.GetString(ftpMessagePayload.Data));
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.CreateFile
        };

        _server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnReadFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        if (_sessions.TryGetValue(ftpMessagePayload.Session, out var session))
        {
            var responsePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.ACK,
                ReqOpCodeId = OpCode.ReadFile,
                Session = ftpMessagePayload.Session
            };
            
            using (BinaryReader br = new BinaryReader(session))
            {
                br.BaseStream.Position = ftpMessagePayload.Offset;
                
                responsePayload.Data = br.ReadBytes(ftpMessagePayload.Size);

                responsePayload.Size = (byte)responsePayload.Data.Length;

                _server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
            }
        }
    }

    public void OnOpenFileRO(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        _sessions[(byte)(_sessions.Keys.Last() + 1)] = new FileStream(MavlinkTypesHelper.GetString(ftpMessagePayload.Data), 
            FileMode.Open, FileAccess.Read);
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.OpenFileRO
        };
     
        _server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnListDirectory(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        var path = MavlinkTypesHelper.GetString(ftpMessagePayload.Data);

        StringBuilder sb = new StringBuilder();
        
        var index = 0;
        
        foreach (var item in Directory.GetFileSystemEntries(path))
        {
            if (Directory.Exists(item))
            {
                DirectoryInfo info = new DirectoryInfo(item);
                
                var ftpDirectory = new FtpFileListItem()
                {
                    FileName = info.Name,
                    Type = FileItemType.Directory
                };
                
                if (index >= ftpMessagePayload.Offset)
                {
                    sb.Append(ftpDirectory);
                }
            }
            else if (File.Exists(item))
            {
                FileInfo info = new FileInfo(item);
            
                var ftpFile = new FtpFileListItem()
                {
                    FileName = info.Name,
                    Size = (int)info.Length,
                    Type = FileItemType.File
                };
                
                if (index >= ftpMessagePayload.Offset)
                {
                    sb.Append(ftpFile);
                }
            }

            if (sb.Length >= 239) break;
            
            index++;
        }
        
        var responsePayload = new FtpMessagePayload
        {
            ReqOpCodeId = OpCode.ListDirectory
        };
        
        if (index < ftpMessagePayload.Offset)
        {
            responsePayload.OpCodeId = OpCode.NAK;
        }
        else
        {
            responsePayload.OpCodeId = OpCode.ACK;

            responsePayload.Data = MavlinkTypesHelper.GetBytes(sb.Length > 239 ? sb.ToString(0, 239) : sb.ToString());
            
            responsePayload.Size = (byte)responsePayload.Data.Length;
        }
        
        _server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnTerminateSession(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        if (_sessions.TryGetValue(ftpMessagePayload.Session, out var stream))
        {
            stream.Close();
            _sessions.Remove(ftpMessagePayload.Session);
        }

        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.TerminateSession
        };
     
        _server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnRename(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        var messagePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.NAK,
            ReqOpCodeId = OpCode.Rename,
            Size = 0,
            Offset = 0,
            Data = new byte[251 - 12]
        };
     
        _server.SendFtpPacket(messagePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }
}