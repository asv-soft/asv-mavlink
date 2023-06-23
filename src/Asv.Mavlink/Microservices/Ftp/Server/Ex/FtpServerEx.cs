using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Asv.Common;

namespace Asv.Mavlink;

public class FtpServerEx : DisposableOnceWithCancel, IFtpServerEx
{
    private readonly Dictionary<byte, FileStream> _sessions;
    private readonly Dictionary<byte, BinaryReader> _readers;
    private readonly Dictionary<byte, BinaryWriter> _writers;
    
    public FtpServerEx(IFtpServer server)
    {
        Server = server ?? throw new ArgumentNullException(nameof(server));
        
        _sessions = new Dictionary<byte, FileStream>();
        _readers = new Dictionary<byte, BinaryReader>();
        _writers = new Dictionary<byte, BinaryWriter>();
        
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

    public IFtpServer Server { get; }

    public void OnResetSessions(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = _sessions.Keys.Count > 0 ? OpCode.ACK : OpCode.NAK,
            ReqOpCodeId = OpCode.ResetSessions
        };
        
        foreach (var sessionNumber in _sessions.Keys)
        {
            _sessions[sessionNumber].Close();
            
            if (_readers.TryGetValue(sessionNumber, out var reader))
            {
                reader.Close();
                _readers.Remove(sessionNumber);
            }
        
            if (_writers.TryGetValue(sessionNumber, out var writer))
            {
                writer.Close();
                _writers.Remove(sessionNumber);
            }
        }
        _sessions.Clear();
        
        Server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnBurstReadFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        var sequence = ftpMessagePayload.SequenceNumber;

        var offset = ftpMessagePayload.Offset;

        var currentBurst = 0;

        if (_sessions.TryGetValue(ftpMessagePayload.Session, out var session))
        {
            if (!_readers.TryGetValue(ftpMessagePayload.Session, out _))
            {
                _readers[ftpMessagePayload.Session] = new BinaryReader(session);
            }

            _readers[ftpMessagePayload.Session].BaseStream.Position = offset;

            var burstComplete = false;
            
            while (!burstComplete)
            {
                sequence++;

                var responsePayload = new FtpMessagePayload
                {
                    OpCodeId = OpCode.ACK,
                    Session = ftpMessagePayload.Session,
                    ReqOpCodeId = OpCode.BurstReadFile,
                    SequenceNumber = sequence
                };

                responsePayload.Data = _readers[ftpMessagePayload.Session].ReadBytes(251 - 12);

                responsePayload.Offset = offset;

                responsePayload.Size = (byte)responsePayload.Data.Length;

                offset += responsePayload.Size;

                currentBurst += responsePayload.Size;

                if (responsePayload.Size == 0)
                {
                    break;
                }

                if (currentBurst >= 23900)
                {
                    burstComplete = true;

                    responsePayload.BurstComplete = true;
                    
                    Server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);

                    break;
                }

                Server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
            }
        }
        else
        {
            var responsePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.NAK,
                Session = ftpMessagePayload.Session,
                ReqOpCodeId = OpCode.BurstReadFile,
                SequenceNumber = sequence
            };
   
            responsePayload.Data[0] = (byte)NakError.Fail;
            
            Server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
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
        
        Server.SendFtpPacket(messagePayload, identity, DisposeCancel).Wait(DisposeCancel);
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
        
        Server.SendFtpPacket(messagePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnOpenFileWO(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        var session = (byte)(_sessions.Keys.LastOrDefault() + 1);
        
        _sessions[session] = new FileStream(MavlinkTypesHelper.GetString(ftpMessagePayload.Data), 
            FileMode.Truncate, FileAccess.Write);
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            Session = session,
            ReqOpCodeId = OpCode.OpenFileWO,
        };
        
        Server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnRemoveDirectory(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        Directory.Delete(MavlinkTypesHelper.GetString(ftpMessagePayload.Data), true);
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.RemoveDirectory,
        };
     
        Server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnCreateDirectory(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        Directory.CreateDirectory(MavlinkTypesHelper.GetString(ftpMessagePayload.Data));
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.CreateDirectory
        };
        
        Server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void RemoveFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        File.Delete(MavlinkTypesHelper.GetString(ftpMessagePayload.Data));
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.RemoveFile,
        };

        Server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnWriteFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.NAK,
            ReqOpCodeId = OpCode.WriteFile,
            Session = ftpMessagePayload.Session
        };
        
        if (_sessions.TryGetValue(ftpMessagePayload.Session, out var session))
        {
            responsePayload.OpCodeId = OpCode.ACK;
            
            if (!_writers.TryGetValue(ftpMessagePayload.Session, out _))
            {
                _writers[ftpMessagePayload.Session] = new BinaryWriter(session);
            }
            
            _writers[ftpMessagePayload.Session].BaseStream.Position = ftpMessagePayload.Offset;
                
            _writers[ftpMessagePayload.Session].Write(ftpMessagePayload.Data, 0, ftpMessagePayload.Size);
            
            responsePayload.Size = ftpMessagePayload.Size;
        }
        else
        {
            responsePayload.Data[0] = (byte)NakError.Fail;
        }
        
        Server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnCreateFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
         var sessionNumber = _sessions.Keys.LastOrDefault() + 1;

        _sessions[(byte)sessionNumber] = File.Create(MavlinkTypesHelper.GetString(ftpMessagePayload.Data));
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            Session = (byte)sessionNumber,
            ReqOpCodeId = OpCode.CreateFile
        };

        Server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnReadFile(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.NAK,
            ReqOpCodeId = OpCode.ReadFile,
            Session = ftpMessagePayload.Session
        };
        
        if (_sessions.TryGetValue(ftpMessagePayload.Session, out var session))
        {
            responsePayload.OpCodeId = OpCode.ACK;
            
            if (!_readers.TryGetValue(ftpMessagePayload.Session, out _))
            {
                _readers[ftpMessagePayload.Session] = new BinaryReader(session);
            }
            
            _readers[ftpMessagePayload.Session].BaseStream.Position = ftpMessagePayload.Offset;
            
            responsePayload.Data = _readers[ftpMessagePayload.Session].ReadBytes(ftpMessagePayload.Size);

            responsePayload.Size = (byte)responsePayload.Data.Length;
        }
        else
        {
            responsePayload.Data[0] = (byte)NakError.Fail;
        }
        
        Server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnOpenFileRO(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        var session = (byte)(_sessions.Keys.LastOrDefault() + 1);
        
        _sessions[session] = new FileStream(MavlinkTypesHelper.GetString(ftpMessagePayload.Data), 
            FileMode.Open, FileAccess.Read);
        
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.ACK,
            ReqOpCodeId = OpCode.OpenFileRO,
            Session = session,
            Data = BitConverter.GetBytes(_sessions[session].Length) 
        };
     
        Server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
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
                
                var ftpDirectory = new FtpEntryItem()
                {
                    Name = info.Name,
                    Type = FtpEntryType.Directory
                };
                
                if (index >= ftpMessagePayload.Offset)
                {
                    sb.Append(ftpDirectory);
                }
            }
            else if (File.Exists(item))
            {
                FileInfo info = new FileInfo(item);
            
                var ftpFile = new FtpEntryItem()
                {
                    Name = info.Name,
                    Size = (int)info.Length,
                    Type = FtpEntryType.File
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
        
        if (Directory.GetFileSystemEntries(path).Length <= ftpMessagePayload.Offset || Directory.GetFileSystemEntries(path).Length == 0)
        {
            responsePayload.OpCodeId = OpCode.NAK;
        }
        else
        {
            responsePayload.OpCodeId = OpCode.ACK;

            responsePayload.Data = MavlinkTypesHelper.GetBytes(sb.Length > 239 ? sb.ToString(0, 239) : sb.ToString());
            
            responsePayload.Size = (byte)responsePayload.Data.Length;
        }
        
        Server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }

    public void OnTerminateSession(DeviceIdentity identity, FtpMessagePayload ftpMessagePayload)
    {
        var responsePayload = new FtpMessagePayload
        {
            OpCodeId = OpCode.NAK,
            ReqOpCodeId = OpCode.TerminateSession
        };
        
        if (_readers.Count > 0 && _readers.TryGetValue(ftpMessagePayload.Session, out var reader))
        {
            reader.Close();
            _readers.Remove(ftpMessagePayload.Session);
        }
        
        if (_writers.Count > 0 && _writers.TryGetValue(ftpMessagePayload.Session, out var writer))
        {
            writer.Close();
            _writers.Remove(ftpMessagePayload.Session);
        }

        if (_sessions.Count > 0 && _sessions.TryGetValue(ftpMessagePayload.Session, out var stream))
        {
            responsePayload.OpCodeId = OpCode.ACK;
            stream.Close();
            _sessions.Remove(ftpMessagePayload.Session);
        }
        
        Server.SendFtpPacket(responsePayload, identity, DisposeCancel).Wait(DisposeCancel);
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
     
        Server.SendFtpPacket(messagePayload, identity, DisposeCancel).Wait(DisposeCancel);
    }
}