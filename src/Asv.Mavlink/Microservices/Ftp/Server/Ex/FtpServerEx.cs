using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        
        server.TerminateSessionRequest.Subscribe(OnTerminateSession)
            .DisposeItWith(Disposable);
        
        server.ResetSessionsRequest.Subscribe(OnResetSessions)
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

    public void OnResetSessions(FtpMessagePayload ftpMessagePayload)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);

        foreach (var sessionNumber in _sessions.Keys)
        {
            _sessions[sessionNumber].Close();
        }
        _sessions.Clear();
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.ResetSessions,
            SequenceNumber = (ushort)(ftpMessagePayload.SequenceNumber + 1)
        };
        
        Task.WaitAll(_server.SendFtpPacket(responsePayload, cs.Token));
    }

    public void OnBurstReadFile(FtpMessagePayload ftpMessagePayload)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);

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

                        Task.WaitAll(_server.SendFtpPacket(responsePayload, cs.Token));

                        break;
                    }
                    
                    Task.WaitAll(_server.SendFtpPacket(responsePayload, cs.Token));
                }
            }
        }
    }

    public void OnCalcFileCRC32(FtpMessagePayload ftpMessagePayload)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);

        var messagePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.NAK,
            ReqOpCodeId = OpCode.CalcFileCRC32,
            SequenceNumber = (ushort)(ftpMessagePayload.SequenceNumber + 1),
            Size = 0,
            Offset = 0,
            Data = new byte[251 - 12]
        };
        
        Task.WaitAll(_server.SendFtpPacket(messagePayload, cs.Token));
    }

    public void OnTruncateFile(FtpMessagePayload ftpMessagePayload)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);

        var messagePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.NAK,
            ReqOpCodeId = OpCode.TruncateFile,
            SequenceNumber = (ushort)(ftpMessagePayload.SequenceNumber + 1),
            Size = 0,
            Offset = 0,
            Data = new byte[251 - 12]
        };
        
        Task.WaitAll(_server.SendFtpPacket(messagePayload, cs.Token));
    }

    public void OnOpenFileWO(FtpMessagePayload ftpMessagePayload)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);
        
        _sessions[(byte)(_sessions.Keys.Last() + 1)] = new FileStream(MavlinkTypesHelper.GetString(ftpMessagePayload.Data), 
            FileMode.Truncate, FileAccess.Write);
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.OpenFileWO,
            SequenceNumber = (ushort)(ftpMessagePayload.SequenceNumber + 1)
        };
        
        Task.WaitAll(_server.SendFtpPacket(responsePayload, cs.Token));
    }

    public void OnRemoveDirectory(FtpMessagePayload ftpMessagePayload)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);

        Directory.Delete(MavlinkTypesHelper.GetString(ftpMessagePayload.Data), true);
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.RemoveDirectory,
            SequenceNumber = (ushort)(ftpMessagePayload.SequenceNumber + 1)
        };
        
        Task.WaitAll(_server.SendFtpPacket(responsePayload, cs.Token));
    }

    public void OnCreateDirectory(FtpMessagePayload ftpMessagePayload)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);
        
        Directory.CreateDirectory(MavlinkTypesHelper.GetString(ftpMessagePayload.Data));
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.CreateDirectory,
            SequenceNumber = (ushort)(ftpMessagePayload.SequenceNumber + 1)
        };
        
        Task.WaitAll(_server.SendFtpPacket(responsePayload, cs.Token));
    }

    public void RemoveFile(FtpMessagePayload ftpMessagePayload)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);
        
        File.Delete(MavlinkTypesHelper.GetString(ftpMessagePayload.Data));
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.RemoveFile,
            SequenceNumber = (ushort)(ftpMessagePayload.SequenceNumber + 1)
        };
        
        Task.WaitAll(_server.SendFtpPacket(responsePayload, cs.Token));
    }

    public void OnWriteFile(FtpMessagePayload ftpMessagePayload)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);

        if (_sessions.TryGetValue(ftpMessagePayload.Session, out var session))
        {
            var responsePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.ACK,
                ReqOpCodeId = OpCode.WriteFile,
                Session = ftpMessagePayload.Session,
                SequenceNumber = (ushort)(ftpMessagePayload.SequenceNumber + 1)
            };
            
            using (BinaryWriter bw = new BinaryWriter(session))
            {
                bw.BaseStream.Position = ftpMessagePayload.Offset;
                
                bw.Write(ftpMessagePayload.Data, 0, ftpMessagePayload.Size);
                
                responsePayload.Size = (byte)responsePayload.Data.Length;
                
                Task.WaitAll(_server.SendFtpPacket(responsePayload, cs.Token));
            }
        }
    }

    public void OnCreateFile(FtpMessagePayload ftpMessagePayload)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);
        
        var sessionNumber = _sessions.Keys.Last() + 1;
        _sessions[(byte)sessionNumber] = File.Create(MavlinkTypesHelper.GetString(ftpMessagePayload.Data));
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.CreateFile,
            SequenceNumber = (ushort)(ftpMessagePayload.SequenceNumber + 1)
        };
        
        Task.WaitAll(_server.SendFtpPacket(responsePayload, cs.Token));
    }

    public void OnReadFile(FtpMessagePayload ftpMessagePayload)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);

        if (_sessions.TryGetValue(ftpMessagePayload.Session, out var session))
        {
            var responsePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.ACK,
                ReqOpCodeId = OpCode.ReadFile,
                Session = ftpMessagePayload.Session,
                SequenceNumber = (ushort)(ftpMessagePayload.SequenceNumber + 1)
            };
            
            using (BinaryReader br = new BinaryReader(session))
            {
                br.BaseStream.Position = ftpMessagePayload.Offset;
                
                responsePayload.Data = br.ReadBytes(ftpMessagePayload.Size);

                responsePayload.Size = (byte)responsePayload.Data.Length;
                
                Task.WaitAll(_server.SendFtpPacket(responsePayload, cs.Token));
            }
        }
    }

    public void OnOpenFileRO(FtpMessagePayload ftpMessagePayload)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);
        
        _sessions[(byte)(_sessions.Keys.Last() + 1)] = new FileStream(MavlinkTypesHelper.GetString(ftpMessagePayload.Data), 
            FileMode.Open, FileAccess.Read);
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.OpenFileRO,
            SequenceNumber = (ushort)(ftpMessagePayload.SequenceNumber + 1)
        };
        
        Task.WaitAll(_server.SendFtpPacket(responsePayload, cs.Token));
    }

    public void OnListDirectory(FtpMessagePayload ftpMessagePayload)
    {
        var directoryItems = new List<FtpFileListItem>();
        
        var path = MavlinkTypesHelper.GetString(ftpMessagePayload.Data);
        
        foreach (var item in Directory.GetFileSystemEntries(path))
        {
            FileInfo info = new FileInfo(item);
            
            var ftpFile = new FtpFileListItem()
            {
                FileName = info.Name,
                Size = (int)info.Length,
            };
            
            if (info.Directory != null)
            {
                ftpFile.Type = FileItemType.Directory;
            }
            else
            {
                ftpFile.Type = FileItemType.File;
            }
            
            directoryItems.Add(ftpFile);
        }
    }

    public void OnTerminateSession(FtpMessagePayload ftpMessagePayload)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);
        
        _sessions[ftpMessagePayload.Session].Close();
        _sessions.Remove(ftpMessagePayload.Session);

        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.TerminateSession,
            SequenceNumber = (ushort)(ftpMessagePayload.SequenceNumber + 1)
        };
        
        Task.WaitAll(_server.SendFtpPacket(responsePayload, cs.Token));
    }

    public void OnRename(FtpMessagePayload ftpMessagePayload)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);

        var messagePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.NAK,
            ReqOpCodeId = OpCode.Rename,
            SequenceNumber = (ushort)(ftpMessagePayload.SequenceNumber + 1),
            Size = 0,
            Offset = 0,
            Data = new byte[251 - 12]
        };
        
        Task.WaitAll(_server.SendFtpPacket(messagePayload, cs.Token));
    }
}