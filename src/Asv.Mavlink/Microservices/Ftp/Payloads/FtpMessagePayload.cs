using System;
using Asv.IO;

namespace Asv.Mavlink;

    /// <summary>
    /// Ftp message payload
    /// Max size = 251
    /// https://mavlink.io/en/services/ftp.html
    /// </summary>
    public class FtpMessagePayload
    {
        /// <summary>
        /// FTP message payload max size in bytes
        /// </summary>
        internal const int MAX_PAYLOAD_SIZE = 251;
        
        /// <summary>
        /// Offset of "Data" buffer in bytes
        /// </summary>
        internal const int DATA_BYTES_OFFSET = 12;

        public FtpMessagePayload()
        {
            
        }
        
        public FtpMessagePayload(ReadOnlySpan<byte> buffer)
        {
            Deserialize(ref buffer);
        }
        
        public virtual void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            SequenceNumber = BinSerialize.ReadUShort(ref buffer);
            Session = BinSerialize.ReadByte(ref buffer);
            OpCodeId = (OpCode)BinSerialize.ReadByte(ref buffer);
            Size = BinSerialize.ReadByte(ref buffer);
            ReqOpCodeId = (OpCode)BinSerialize.ReadByte(ref buffer);
            BurstComplete = BinSerialize.ReadByte(ref buffer) != 0;
            Padding = BinSerialize.ReadByte(ref buffer);
            Offset = BinSerialize.ReadUInt(ref buffer);
            Data = BinSerialize.ReadBlock(ref buffer, MAX_PAYLOAD_SIZE - DATA_BYTES_OFFSET);
        }

        public virtual void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteUShort(ref buffer, SequenceNumber);
            BinSerialize.WriteByte(ref buffer, Session);
            BinSerialize.WriteByte(ref buffer, (byte)OpCodeId);
            BinSerialize.WriteByte(ref buffer, Size);
            BinSerialize.WriteByte(ref buffer, (byte)ReqOpCodeId);
            BinSerialize.WriteByte(ref buffer, (byte)(BurstComplete ? 1:0));
            BinSerialize.WriteByte(ref buffer, Padding);
            BinSerialize.WriteUInt(ref buffer, Offset);
            BinSerialize.WriteBlock(ref buffer, Data);
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

        public byte GetMaxByteSize() => 251;
        public byte GetMinByteSize() => 251;
    }
    