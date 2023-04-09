using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Client.Ftp
{
    /// <summary>
    /// TODO: implemet FTP protocol
    ///
    /// https://mavlink.io/en/services/ftp.html
    /// </summary>



    public enum OpCode
    {
        /// <summary> 
        /// Ignored, always ACKed
        /// </summary> 
        None = 0,
        /// <summary> 
        /// "Terminates open Read session.
        /// - Closes the file associated with (session) and frees the session ID for re-use."
        /// </summary> 
        TerminateSession = 1,
        /// <summary> 
        /// "Terminates all open read sessions.
        /// - Clears all state held by the drone (server); closes all open files, etc.
        /// - Sends an ACK reply with no data."
        /// </summary> 
        ResetSessions = 2,
        /// <summary> 
        /// "List directory entry information (files, folders etc.) in <path>, starting from a specified entry index (<offset>).
        /// - Response is an ACK packet with one or more entries on success, otherwise a NAK packet with an error code.
        /// - Completion is indicated by a NACK with EOF in response to a requested index (offset) beyond the list of entries.
        /// - The directory is closed after the operation, so this leaves no state on the server."
        /// </summary> 
        ListDirectory = 3,
        /// <summary> 
        /// "Opens file at <path> for reading, returns <session>
        /// - The path is stored in the payload data. The drone opens the file (path) and allocates a session number. The file must exist.
        /// - An ACK packet must include the allocated session and the data size of the file to be opened (size)
        /// - A NAK packet must contain error information . Typical error codes for this command are NoSessionsAvailable, FileExists.
        /// - The file remains open after the operation, and must eventually be closed by Reset or Terminate."
        /// </summary> 
        OpenFileRO = 4,
        /// <summary> 
        /// "Reads <size> bytes from <offset> in <session>.
        /// - Seeks to (offset) in the file opened in (session) and reads (size) bytes into the result buffer.
        /// - Sends an ACK packet with the result buffer on success, otherwise a NAK packet with an error code. For short reads or reads beyond the end of a file, the (size) field in the ACK packet will indicate the actual number of bytes read.
        /// - Reads can be issued to any offset in the file for any number of bytes, so reconstructing portions of the file to deal with lost packets should be easy.
        /// - For best download performance, try to keep two Read packets in flight."
        /// </summary> 
        ReadFile = 5,
        /// <summary> 
        /// "Creates file at <path> for writing, returns <session>.
        /// - Creates the file (path) and allocates a session number. The file must not exist, but all parent directories must exist.
        /// - Sends an ACK packet with the allocated session number on success, or a NAK packet with an error code on error (i.e. FileExists if the path already exists).
        /// - The file remains open after the operation, and must eventually be closed by Reset or Terminate."
        /// </summary> 
        CreateFile = 6,
        /// <summary> 
        /// "Writes <size> bytes to <offset> in <session>.
        /// - Sends an ACK reply with no data on success, otherwise a NAK packet with an error code."
        /// </summary> 
        WriteFile = 7,
        /// <summary> 
        /// "Remove file at <path>.
        /// - ACK reply with no data on success.
        /// - NAK packet with error information on failure."
        /// </summary> 
        RemoveFile = 8,
        /// <summary> 
        /// "Creates directory at <path>.
        /// - Sends an ACK reply with no data on success, otherwise a NAK packet with an error code."
        /// </summary> 
        CreateDirectory = 9,
        /// <summary> 
        /// "Removes directory at <path>. The directory must be empty.
        /// - Sends an ACK reply with no data on success, otherwise a NAK packet with an error code."
        /// </summary> 
        RemoveDirectory = 10,
        /// <summary> 
        /// "Opens file at <path> for writing, returns <session>.
        /// - Opens the file (path) and allocates a session number. The file must exist.
        /// - Sends an ACK packet with the allocated session number on success, otherwise a NAK packet with an error code.
        /// - The file remains open after the operation, and must eventually be closed by Reset or Terminate."
        /// </summary> 
        OpenFileWO = 11,
        /// <summary> 
        /// "Truncate file at <path> to <offset> length.
        /// - Sends an ACK reply with no data on success, otherwise a NAK packet with an error code."
        /// </summary> 
        TruncateFile = 12,
        /// <summary> 
        /// "Rename <path1> to <path2>.
        /// - Sends an ACK reply the no data on success, otherwise a NAK packet with an error code (i.e. if the source path does not exist)."
        /// </summary> 
        Rename = 13,
        /// <summary> 
        /// "Calculate CRC32 for file at <path>.
        /// - Sends an ACK reply with the checksum on success, otherwise a NAK packet with an error code."
        /// </summary> 
        CalcFileCRC32 = 14,
        /// <summary> 
        /// Burst download session file.
        /// </summary> 
        BurstReadFile = 15,

        /// <summary>
        ///  ACK response.
        /// </summary>
        ACK = 128,
        /// <summary>
        ///  NAK response.
        /// </summary>
        NAK = 129,
    }

    /// <summary>
    /// Ftp message payload
    /// Max size = 251
    /// https://mavlink.io/en/services/ftp.html
    /// </summary>
    public class FtpMessagePayload
    {
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
            var buffer = src.Payload.Payload;
            int index = 0;
            SequenceNumber = BitConverter.ToUInt16(buffer, index);index+=2;
            Session = buffer[index]; index+=1;
            OpCodeId = (OpCode)buffer[index]; index += 1;
            Size = buffer[index]; index += 1;
            ReqOpCodeId = (OpCode)buffer[index]; index += 1;
            BurstComplete = buffer[index] != 0; index += 1;
            Padding = buffer[index]; index += 1;
            Offset = BitConverter.ToUInt16(buffer, index); index += 4;
            Data = new byte[239];
            Buffer.BlockCopy(src.Payload.Payload,12,Data,0,Data.Length);

        }

        /// <summary>
        /// Command/response data. Varies by OpCode. This contains the path for operations that act on a file or directory.
        /// For an ACK for a read or write this is the requested information.
        /// For an ACK for a OpenFileRO operation this is the size of the file that was opened.
        /// For a NAK the first byte is the error code and the (optional) second byte may be an error number.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 32 bit alignment padding.
        /// </summary>
        public ushort Offset { get; set; }


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


    public enum NakError
    {
        /// <summary> 
        /// No error 
        /// </summary> 
        None = 0,

        /// <summary> 
        /// Unknown failure 
        /// </summary> 
        Fail = 1,

        /// <summary> 
        /// Command failed, Err number sent back in PayloadHeader.data[1]. This is a file-system error number understood by the server operating system. 
        /// </summary> 
        FailErrno = 2,

        /// <summary> 
        /// Payload size is invalid 
        /// </summary> 
        InvalidDataSize = 3,

        /// <summary> 
        /// Session is not currently open 
        /// </summary> 
        InvalidSession = 4,

        /// <summary> 
        /// All available sessions are already in use. 
        /// </summary> 
        NoSessionsAvailable = 5,

        /// <summary> 
        /// Offset past end of file for ListDirectory and ReadFile commands. 
        /// </summary> 
        EOF = 6,

        /// <summary> 
        /// Unknown command / opcode 
        /// </summary> 
        UnknownCommand = 7,

        /// <summary> 
        /// File/directory already exists 
        /// </summary> 
        FileExists = 8,

        /// <summary> 
        /// File/directory is write protected 
        /// </summary> 
        FileProtected = 9,

        /// <summary> 
        /// File/directory not found 
        /// </summary> 
        FileNotFound = 10,

    }

    public interface IFtpClient
    {
        Task<FtpFileListItem[]> ListDirectory(string path, int index, CancellationToken cancel);
    }

    public class FtpFileListItem
    {
        public FileItemType Type { get; set; }
        public string FileName { get; set; }
        public int Index { get; set; }
    }

    public enum FileItemType
    {
        File,
        Directory,
        Skip,
    }
  
    public class FtpClient:IFtpClient
    {
        private readonly IMavlinkV2Connection _connection;
        private readonly MavlinkClientIdentity _identity;
        private readonly PacketSequenceCalculator _seq;

        public FtpClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity, PacketSequenceCalculator seq)
        {
            _connection = connection;
            _identity = identity;
            _seq = seq;
        }

        public Task SendListDirectory(string path, ushort index, CancellationToken cancel)
        {
            var packet = new FileTransferProtocolPacket
            {
                ComponenId = _identity.ComponentId,
                SystemId = _identity.SystemId,
                Sequence = _seq.GetNextSequenceNumber(),
                Payload =
                {
                    TargetComponent = _identity.TargetComponentId,
                    TargetSystem = _identity.TargetSystemId,
                    TargetNetwork = 0,
                }
            };
            var ftp = new FtpMessagePayload
            {
                Size = (byte) path.Length,
                Offset = index,
                Data = Encoding.ASCII.GetBytes(path)
            };
            
            return _connection.Send(packet, cancel);
        }

        public Task<FtpFileListItem[]> ListDirectory(string path, int index, CancellationToken cancel)
        {
            throw new NotImplementedException();
        }
    }
}
