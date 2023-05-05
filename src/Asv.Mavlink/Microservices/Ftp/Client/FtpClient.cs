using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    
    /// <summary>
    /// Contains information about items from ListDirectory.
    /// It has the following format: 
    /// "[type][file_or_folder_name]\t[file_size_in_bytes]\0", 
    /// where type is one of the letters F(ile), D(irectory), S(skip)
    /// </summary>
    public class FtpFileListItem
    {
        public int Index { get; set; }
        public string FileName { get; set; }
        public FileItemType Type { get; set; }
    }
    
    public class FtpClientConfig
    {
        /// <summary>
        /// Network ID (0 for broadcast)
        /// </summary>
        public byte TargetNetwork { get; set; } = 0;
    }
    
    public class FtpClient: MavlinkMicroserviceClient, IFtpClient
    {
        private readonly MavlinkClientIdentity _identity;
        private readonly IPacketSequenceCalculator _seq;
        private readonly FtpClientConfig _cfg;
        private int _sessionCounter;
        
        public FtpClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, FtpClientConfig cfg, 
            IPacketSequenceCalculator seq, IScheduler scheduler) : base("FTP", connection, identity, seq, scheduler)
        {
            _cfg = cfg;
            _identity = identity;
        }
        
        public async Task<FtpMessagePayload> None(CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload { OpCodeId = OpCode.None };
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                        fillPacket: _ =>
                        {
                            _.Payload.TargetComponent = _identity.TargetComponentId;
                            _.Payload.TargetSystem = _identity.TargetSystemId;
                            _.Payload.TargetNetwork = _cfg.TargetNetwork;
                            var spanBuffer = new Span<byte>(_.Payload.Payload);
                            messagePayload.Serialize(ref spanBuffer);
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork,
                        timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> TerminateSession(byte sessionNumber, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload { OpCodeId = OpCode.TerminateSession, Session = sessionNumber };
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                        fillPacket: _ =>
                        {
                            _.Payload.TargetComponent = _identity.TargetComponentId;
                            _.Payload.TargetSystem = _identity.TargetSystemId;
                            _.Payload.TargetNetwork = _cfg.TargetNetwork;
                            var spanBuffer = new Span<byte>(_.Payload.Payload);
                            messagePayload.Serialize(ref spanBuffer);
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork & 
                                        _.Payload.Payload[2] == sessionNumber &
                                        (OpCode)_.Payload.Payload[3] == OpCode.TerminateSession,
                        timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> ResetSessions(CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload { OpCodeId = OpCode.ResetSessions };
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                        fillPacket: _ =>
                        {
                            _.Payload.TargetComponent = _identity.TargetComponentId;
                            _.Payload.TargetSystem = _identity.TargetSystemId;
                            _.Payload.TargetNetwork = _cfg.TargetNetwork;
                            
                            var spanBuffer = new Span<byte>(_.Payload.Payload);
                            messagePayload.Serialize(ref spanBuffer);

                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork &
                                        (OpCode)_.Payload.Payload[3] == OpCode.ResetSessions,
                        timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> OpenFileRO(string path, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.OpenFileRO,
                Size = (byte)path.Length,
                Data = MavlinkTypesHelper.GetBytes(path)
            };
            
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                        fillPacket: _ =>
                        {
                            _.Payload.TargetComponent = _identity.TargetComponentId;
                            _.Payload.TargetSystem = _identity.TargetSystemId;
                            _.Payload.TargetNetwork = _cfg.TargetNetwork;
                            var spanBuffer = new Span<byte>(_.Payload.Payload);
                            messagePayload.Serialize(ref spanBuffer);
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork,
                        timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> ReadFile(byte size, uint offset, byte sessionNumber, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.ReadFile,
                Size = size,
                Offset = offset,
                Session = sessionNumber
            };
            
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                        fillPacket: _ =>
                        {
                            _.Payload.TargetComponent = _identity.TargetComponentId;
                            _.Payload.TargetSystem = _identity.TargetSystemId;
                            _.Payload.TargetNetwork = _cfg.TargetNetwork;
                            var spanBuffer = new Span<byte>(_.Payload.Payload);
                            messagePayload.Serialize(ref spanBuffer);
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork & _.Payload.Payload[2] == sessionNumber,
                        timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> CreateFile(string path, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.CreateFile,
                Size = (byte)path.Length,
                Data = MavlinkTypesHelper.GetBytes(path)
            };
            
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                        fillPacket: _ =>
                        {
                            _.Payload.TargetComponent = _identity.TargetComponentId;
                            _.Payload.TargetSystem = _identity.TargetSystemId;
                            _.Payload.TargetNetwork = _cfg.TargetNetwork;
                            var spanBuffer = new Span<byte>(_.Payload.Payload);
                            messagePayload.Serialize(ref spanBuffer);
                            
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork &
                                        (OpCode)_.Payload.Payload[3] == OpCode.CreateFile,
                        timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> WriteFile(byte[] writeBuffer, uint offset, byte sessionNumber, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.WriteFile,
                Size = (byte)writeBuffer.Length,
                Offset = offset,
                Session = sessionNumber,
                Data = writeBuffer
            };
            
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                        fillPacket: _ =>
                        {
                            _.Payload.TargetComponent = _identity.TargetComponentId;
                            _.Payload.TargetSystem = _identity.TargetSystemId;
                            _.Payload.TargetNetwork = _cfg.TargetNetwork;
                            var spanBuffer = new Span<byte>(_.Payload.Payload);
                            messagePayload.Serialize(ref spanBuffer);
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork & _.Payload.Payload[2] == sessionNumber,
                        timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> RemoveFile(string path, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.RemoveFile,
                Size = (byte)path.Length,
                Data = MavlinkTypesHelper.GetBytes(path)
            };
            
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                        fillPacket: _ =>
                        {
                            _.Payload.TargetComponent = _identity.TargetComponentId;
                            _.Payload.TargetSystem = _identity.TargetSystemId;
                            _.Payload.TargetNetwork = _cfg.TargetNetwork;
                            var spanBuffer = new Span<byte>(_.Payload.Payload);
                            messagePayload.Serialize(ref spanBuffer);
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork,
                        timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> CreateDirectory(string path, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.CreateDirectory,
                Size = (byte)path.Length,
                Data = MavlinkTypesHelper.GetBytes(path)
            };
            
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                        fillPacket: _ =>
                        {
                            _.Payload.TargetComponent = _identity.TargetComponentId;
                            _.Payload.TargetSystem = _identity.TargetSystemId;
                            _.Payload.TargetNetwork = _cfg.TargetNetwork;
                            var spanBuffer = new Span<byte>(_.Payload.Payload);
                            messagePayload.Serialize(ref spanBuffer);
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork &
                                        (OpCode)_.Payload.Payload[3] == OpCode.CreateDirectory,
                        timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> RemoveDirectory(string path, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.RemoveDirectory,
                Size = (byte)path.Length,
                Data = MavlinkTypesHelper.GetBytes(path)
            };
            
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                        fillPacket: _ =>
                        {
                            _.Payload.TargetComponent = _identity.TargetComponentId;
                            _.Payload.TargetSystem = _identity.TargetSystemId;
                            _.Payload.TargetNetwork = _cfg.TargetNetwork;
                            var spanBuffer = new Span<byte>(_.Payload.Payload);
                            messagePayload.Serialize(ref spanBuffer);
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork,
                        timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> OpenFileWO(string path, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.OpenFileWO,
                Size = (byte)path.Length,
                Data = MavlinkTypesHelper.GetBytes(path)
            };
            
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                        fillPacket: _ =>
                        {
                            _.Payload.TargetComponent = _identity.TargetComponentId;
                            _.Payload.TargetSystem = _identity.TargetSystemId;
                            _.Payload.TargetNetwork = _cfg.TargetNetwork;
                            var spanBuffer = new Span<byte>(_.Payload.Payload);
                            messagePayload.Serialize(ref spanBuffer);
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork,
                        timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> TruncateFile(string path, uint offset, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.TruncateFile,
                Size = (byte)path.Length,
                Offset = offset,
                Data = MavlinkTypesHelper.GetBytes(path)
            };
            
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                        fillPacket: _ =>
                        {
                            _.Payload.TargetComponent = _identity.TargetComponentId;
                            _.Payload.TargetSystem = _identity.TargetSystemId;
                            _.Payload.TargetNetwork = _cfg.TargetNetwork;
                            var spanBuffer = new Span<byte>(_.Payload.Payload);
                            messagePayload.Serialize(ref spanBuffer);
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork,
                        timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> Rename(string pathToRename, string newPath, CancellationToken cancel)
        {
            return new FtpMessagePayload();
        }

        public async Task<FtpMessagePayload> CalcFileCRC32(string path, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.CalcFileCRC32,
                Size = (byte)path.Length,
                Data = MavlinkTypesHelper.GetBytes(path)
            };
            
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                        fillPacket: _ =>
                        {
                            _.Payload.TargetComponent = _identity.TargetComponentId;
                            _.Payload.TargetSystem = _identity.TargetSystemId;
                            _.Payload.TargetNetwork = _cfg.TargetNetwork;
                            var spanBuffer = new Span<byte>(_.Payload.Payload);
                            messagePayload.Serialize(ref spanBuffer);
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork,
                        timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        // public async Task BurstReadFile(byte size, uint offset, byte sessionNumber, CancellationToken cancel)
        // {
        //     var messagePayload = new FtpMessagePayload
        //     {
        //         OpCodeId = OpCode.OpenFileWO,
        //         Size = (byte)path.Length,
        //         Offset = offset,
        //         Data = MavlinkTypesHelper.GetBytes(path)
        //     };
        //     
        //     await InternalSend<FileTransferProtocolPacket>(
        //             fillPacket: _ =>
        //             {
        //                 _.Payload.TargetComponent = _identity.TargetComponentId;
        //                 _.Payload.TargetSystem = _identity.TargetSystemId;
        //                 _.Payload.TargetNetwork = _cfg.TargetNetwork;
        //                 var buffer = new Span<byte>(new byte[251]);
        //                 messagePayload.Serialize(ref buffer);
        //                 _.Payload.Payload = buffer.ToArray();
        //             }, cancel)
        //         .ConfigureAwait(false);
        //     return new FtpMessagePayload(result.Payload);
        // }
        
        public async Task<FtpMessagePayload> ListDirectory(string path, uint offset, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.ListDirectory,
                Size = (byte)path.Length,
                Offset = offset,
                Data = MavlinkTypesHelper.GetBytes(path)
            };
            byte[] buffer = new byte[251];
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                        fillPacket: _ =>
                        {
                            _.Payload.TargetComponent = _identity.TargetComponentId;
                            _.Payload.TargetSystem = _identity.TargetSystemId;
                            _.Payload.TargetNetwork = _cfg.TargetNetwork;
                            var spanBuffer = new Span<byte>(_.Payload.Payload);
                            messagePayload.Serialize(ref spanBuffer);
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork &
                                        (OpCode)_.Payload.Payload[3] == OpCode.ListDirectory,
                        timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }
        
        private byte GenerateSession()
        {
            return (byte)(Interlocked.Increment(ref _sessionCounter) % byte.MaxValue);
        }
    } 
}
