using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    /// <summary>
    /// Ftp message payload
    /// Max size = 251
    /// https://mavlink.io/en/services/ftp.html
    /// </summary>
    public class FtpMessagePayload
    {
        public FtpMessagePayload()
        {
            
        }

        public FtpMessagePayload(FileTransferProtocolPayload payload)
        {
            Deserialize(payload);
        }
        
        public FtpMessagePayload(FileTransferProtocolPacket packet)
        {
            Deserialize(packet);
        }
        
        /// <summary>
        /// FTP message payload max size in bytes
        /// </summary>
        private const int MAX_PAYLOAD_SIZE = 251;
        
        /// <summary>
        /// Offset of "Data" buffer in bytes
        /// </summary>
        private const int DATA_BYTES_OFFSET = 12;

        public void Serialize(FileTransferProtocolPacket dest)
        {
            using (var strm = new MemoryStream())
            {
                using (var wrt = new StreamWriter(strm))
                {
                    wrt.Write(SequenceNumber);
                    wrt.Write(Session);
                    wrt.Write((byte)OpCodeId);
                    wrt.Write(Size);
                    wrt.Write((byte)ReqOpCodeId);
                    wrt.Write((byte)(BurstComplete ? 1:0));
                    wrt.Write(Padding);
                    wrt.Write(Offset);
                    strm.Write(Data,0,Data.Length);
                    strm.Position = 0;
                    dest.Payload.Payload = strm.ToArray();
                }
            }
        }

        public void Deserialize(FileTransferProtocolPacket src)
        {
            Deserialize(src.Payload);
        }
        
        public void Deserialize(FileTransferProtocolPayload src)
        {
            var buffer = src.Payload;
            int index = 0;
            SequenceNumber = BitConverter.ToUInt16(buffer, index); index += 2;
            Session = buffer[index]; index += 1;
            OpCodeId = (OpCode)buffer[index]; index += 1;
            Size = buffer[index]; index += 1;
            ReqOpCodeId = (OpCode)buffer[index]; index += 1;
            BurstComplete = buffer[index] != 0; index += 1;
            Padding = buffer[index]; index += 1;
            
            Offset = BitConverter.ToUInt32(buffer, index);
            
            Data = new byte[MAX_PAYLOAD_SIZE - DATA_BYTES_OFFSET];
            Buffer.BlockCopy(src.Payload,DATA_BYTES_OFFSET,Data,0,Data.Length);
        }

        /// <summary>
        /// Command/response data. Varies by OpCode. This contains the path for operations that act on a file or directory.
        /// For an ACK for a read or write this is the requested information.
        /// For an ACK for a OpenFileRO operation this is the size of the file that was opened.
        /// For a NAK the first byte is the error code and the (optional) second byte may be an error number.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Offsets into data to be sent for ListDirectory and ReadFile commands.
        /// </summary>
        public uint Offset { get; set; }
        
        /// <summary>
        /// 32 bit alignment padding.
        /// </summary>
        public byte Padding { get; set; }

        /// <summary>
        /// Code to indicate if a burst is complete. 1: set of burst packets complete, 0: More burst packets coming.
        /// - Only used if req_opcode is BurstReadFile.
        /// </summary>
        public bool BurstComplete { get; set; }
        
        /// <summary>
        /// OpCode (of original message) returned in an ACK or NAK response.
        /// </summary>
        public OpCode ReqOpCodeId { get; set; }

        /// <summary>
        /// Depends on OpCode. For Reads/Writes this is the size of the data transported. For NAK it is the number of bytes used for error information (1 or 2).
        /// </summary>
        public byte Size { get; set; }
        /// <summary>
        /// Ids for particular commands and ACK/NAK messages.
        /// </summary>
        public OpCode OpCodeId { get; set; }
        /// <summary>
        /// Session id for read/write operations (the server may use this to reference the file handle and information about the progress of read/write operations).
        /// </summary>
        public byte Session { get; set; }
        /// <summary>
        /// All new messages between the GCS and drone iterate this number. Re-sent commands/ACK/NAK should use the previous response's sequence number.
        /// </summary>
        public ushort SequenceNumber { get; set; }
    }
    
    /// <summary>
    /// DTO for ListDirectory command
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

    public class FtpFileInfo : FileSystemInfo
    {
        public FtpFileInfo(string name, string parent, bool isdirectory = false, ulong size = 0)
        {
            Name = name;
            isDirectory = isdirectory;
            Size = size;
            Parent = parent;
            FullPath = (Parent.EndsWith("/") ? Parent : Parent + '/') + Name;
        }

        public override bool Exists => true;
        public bool isDirectory { get; set; }
        public override string Name { get; }
        public string Parent { get; }
        public ulong Size { get; set; }

        public override void Delete()
        {
            
        }

        public override string ToString()
        {
            if (isDirectory)
                return "Directory: " + Name;
            return "File: " + Name + " " + Size;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
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
            
            OnReceivedPacket = InternalFilteredVehiclePackets.Where(_ => _.Payload is FileTransferProtocolPayload);
        }

        #region Directory methods
        
        public async Task<FtpMessagePayload> ListDirectory(string path, uint offset, byte sequenceNumber, CancellationToken cancel)
        {
            var result =
                await InternalCall<FileTransferProtocolPayload, FileTransferProtocolPacket, FileTransferProtocolPacket>(
                    fillPacket: _ =>
                    {
                        _.Payload.TargetComponent = _identity.TargetComponentId;
                        _.Payload.TargetSystem = _identity.TargetSystemId;
                        _.Payload.TargetNetwork = _cfg.TargetNetwork;

                        var messagePayload = new FtpMessagePayload
                        {
                            Data = MavlinkTypesHelper.GetBytes(path),
                            Size = (byte)path.Length,
                            Offset = offset,
                            OpCodeId = OpCode.ListDirectory,
                            Session = GenerateSession(),
                            SequenceNumber = sequenceNumber
                        };
                        messagePayload.Serialize(_);

                    }, filter: _ => _.Payload.TargetNetwork == _cfg.TargetNetwork,
                    timeoutMs: 50, attemptCount: 6, resultGetter: _ => _.Payload, cancel: cancel)
            .ConfigureAwait(false);
            
            return new FtpMessagePayload(result);
        }
        
        public async Task CreateDirectory(string path, CancellationToken cancel)
        {

        }

        public async Task RemoveDirectory(string path, CancellationToken cancel)
        {
           
        }
        #endregion

        #region File methods
        
        /// <summary>
        /// Reads file from drone to
        /// </summary>
        /// <param name="readPath"></param>
        /// <param name="savePath"></param>
        /// <param name="cancel"></param>
        public async Task ReadFile(string readPath, string savePath, CancellationToken cancel)
        {
            await InternalSend<FileTransferProtocolPacket>(pkt =>
            {
                var messagePayload = new FtpMessagePayload
                {
                    Data = Encoding.UTF8.GetBytes(readPath),
                    ReqOpCodeId = OpCode.OpenFileRO,
                    Size = (byte)readPath.Length,
                    OpCodeId = OpCode.OpenFileRO,
                    Session = GenerateSession(),
                    SequenceNumber = _seq.GetNextSequenceNumber()
                };
                messagePayload.Serialize(pkt);
            }, cancel).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="savePath"></param>
        /// <param name="cancel"></param>
        public async Task BurstReadFile(string path, string savePath, CancellationToken cancel)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancel"></param>
        public async Task UploadFile(string path, CancellationToken cancel)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancel"></param>
        public async Task RemoveFile(string path, CancellationToken cancel)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="offset"></param>
        /// <param name="cancel"></param>
        public async Task TruncateFile(string path, int offset, CancellationToken cancel)
        {
               
        }
        
        #endregion
        
        public IObservable<IPacketV2<IPayload>> OnReceivedPacket { get; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private byte GenerateSession()
        {
            return (byte)(Interlocked.Increment(ref _sessionCounter) % byte.MaxValue);
        }
    }
}
