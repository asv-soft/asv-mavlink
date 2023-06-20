using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    /// <summary>
    /// Contains information about items from ListDirectory.
    /// It has the following format: 
    /// "[type][file_or_folder_name]\t[file_size_in_bytes]\0", 
    /// where type is one of the letters F(ile), D(irectory), S(skip)
    /// </summary>
    public class FtpEntryItem
    {
        public int Size { get; set; }
        public string Name { get; init; }
        public FtpEntryType Type { get; init; }

        public override string ToString()
        {
            return $"{(char)Type}{Name}{(Size == 0 ? "" : $"\t{Size}")}\0\0";
        }
    }
    
    public class FtpConfig
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
        private readonly FtpConfig _cfg;

        public FtpClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, FtpConfig cfg, 
            IPacketSequenceCalculator seq) : base("FTP", connection, identity, seq)
        {
            _cfg = cfg;
            _identity = identity;
            _seq = seq;
            
            OnBurstReadPacket = InternalFilter<FileTransferProtocolPacket>(_ => (OpCode)_.Payload.Payload[3] == OpCode.ACK &&
                    (OpCode)_.Payload.Payload[5] == OpCode.BurstReadFile)
                .Select(_ => new FtpMessagePayload(_.Payload.Payload))
                .Publish().RefCount();
        }
        
        public IObservable<FtpMessagePayload> OnBurstReadPacket { get; }
        
        public async Task<FtpMessagePayload> None(CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.None,
                SequenceNumber = _seq.GetNextSequenceNumber(),
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
                                        (OpCode)_.Payload.Payload[5] == OpCode.None,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> TerminateSession(byte sessionNumber, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.TerminateSession,
                SequenceNumber = _seq.GetNextSequenceNumber(),
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
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork &
                                        (OpCode)_.Payload.Payload[5] == OpCode.TerminateSession,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> ResetSessions(CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.ResetSessions,
                SequenceNumber = _seq.GetNextSequenceNumber(),
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
                                        (OpCode)_.Payload.Payload[5] == OpCode.ResetSessions,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> OpenFileRO(string path, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.OpenFileRO,
                Size = (byte)path.Length,
                SequenceNumber = _seq.GetNextSequenceNumber(),
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
                                        (OpCode)_.Payload.Payload[5] == OpCode.OpenFileRO,
                        resultGetter: _ => _.Payload, cancel: cancel)
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
                SequenceNumber = _seq.GetNextSequenceNumber(),
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
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork & 
                                        _.Payload.Payload[2] == sessionNumber &
                                        (OpCode)_.Payload.Payload[5] == OpCode.ReadFile,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> CreateFile(string path, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.CreateFile,
                Size = (byte)path.Length,
                SequenceNumber = _seq.GetNextSequenceNumber(),
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
                                        (OpCode)_.Payload.Payload[5] == OpCode.CreateFile,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> WriteFile(byte[] writeBuffer, uint offset, byte sessionNumber, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.WriteFile,
                SequenceNumber = _seq.GetNextSequenceNumber(),
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
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork 
                                        & _.Payload.Payload[2] == sessionNumber &
                                        (OpCode)_.Payload.Payload[5] == OpCode.WriteFile,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> RemoveFile(string path, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.RemoveFile,
                SequenceNumber = _seq.GetNextSequenceNumber(),
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
                                        (OpCode)_.Payload.Payload[5] == OpCode.RemoveFile,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> CreateDirectory(string path, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.CreateDirectory,
                SequenceNumber = _seq.GetNextSequenceNumber(),
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
                                        (OpCode)_.Payload.Payload[5] == OpCode.CreateDirectory,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> RemoveDirectory(string path, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.RemoveDirectory,
                SequenceNumber = _seq.GetNextSequenceNumber(),
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
                                        (OpCode)_.Payload.Payload[5] == OpCode.RemoveDirectory,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> OpenFileWO(string path, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.OpenFileWO,
                Size = (byte)path.Length,
                SequenceNumber = _seq.GetNextSequenceNumber(),
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
                                        (OpCode)_.Payload.Payload[5] == OpCode.OpenFileWO,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        //Don't works somehow
        public async Task<FtpMessagePayload> TruncateFile(string path, uint offset, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.TruncateFile,
                Size = (byte)path.Length,
                Offset = offset,
                SequenceNumber = _seq.GetNextSequenceNumber(),
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
                                        (OpCode)_.Payload.Payload[5] == OpCode.TruncateFile,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> Rename(string pathToRename, string newPath, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.Rename,
                Size = (byte)(pathToRename.Length + newPath.Length + 1),
                SequenceNumber = _seq.GetNextSequenceNumber(),
                Data = MavlinkTypesHelper.GetBytes(pathToRename + '\0' + newPath)
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
                                        (OpCode)_.Payload.Payload[5] == OpCode.Rename,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> CalcFileCRC32(string path, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.CalcFileCRC32,
                Size = (byte)path.Length,
                SequenceNumber = _seq.GetNextSequenceNumber(),
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
                                        (OpCode)_.Payload.Payload[5] == OpCode.CalcFileCRC32,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }

        public async Task<FtpMessagePayload> BurstReadFile(byte size, uint offset, byte sessionNumber, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.BurstReadFile,
                Size = size,
                Offset = offset,
                Session = sessionNumber,
                SequenceNumber = _seq.GetNextSequenceNumber()
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
                                        (OpCode)_.Payload.Payload[5] == OpCode.BurstReadFile,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }
        
        public async Task<FtpMessagePayload> ListDirectory(string path, uint offset, CancellationToken cancel)
        {
            var messagePayload = new FtpMessagePayload
            {
                OpCodeId = OpCode.ListDirectory,
                SequenceNumber = _seq.GetNextSequenceNumber(),
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
                        }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork &
                                        (OpCode)_.Payload.Payload[5] == OpCode.ListDirectory,
                         resultGetter: _ => _.Payload, cancel: cancel)
                    .ConfigureAwait(false);
            return new FtpMessagePayload(result.Payload);
        }
    } 
}
