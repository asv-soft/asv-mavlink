using System;
using System.Buffers;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public static class MavlinkFtpHelper
{
    public const char DirectorySeparator = '/';
    public const byte MaxDataSize = 239;
    public const char FileSizeSeparator = '\t';
    public const char DirectoryChar = 'D';
    public const char FileChar = 'F';
    public const char PathSeparator = '\0';
    public const string SpecialPathCurrent = ".";
    public const string SpecialPathBack = "..";
    public static readonly ImmutableHashSet<string> IgnorePaths = new[] {SpecialPathCurrent, SpecialPathBack}.ToImmutableHashSet();
    
    public static void CheckFilePath(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (FtpEncoding.GetByteCount(path) > MaxDataSize)
        {
            throw new ArgumentOutOfRangeException(nameof(path), $"Max path size is {MaxDataSize}");
        }
    }
    

    public static void CheckFolderPath(ReadOnlySpan<char> path)
    {
        if (FtpEncoding.GetByteCount(path) > MaxDataSize)
        {
            throw new ArgumentOutOfRangeException(nameof(path), $"Max path size is {MaxDataSize}");
        }
    }

    public static bool IsRootPath(ReadOnlySpan<char> path) => path.Trim() is [DirectorySeparator];

    public static IFtpEntry CreateFtpDirectoryFromPath(string path)
    {
        var span = path.AsSpan();
        if (IsRootPath(span))
        {
            return new FtpDirectory(string.Empty);
        }
        span = span.TrimEnd(DirectorySeparator);
        var index = span.LastIndexOf(DirectorySeparator);
        return index == -1 
            ? new FtpDirectory(span.ToString()) 
            : new FtpDirectory(span[(index + 1)..].ToString(), span[..index].ToString());
    }
    public static bool ParseFtpEntry(ref SequenceReader<char> rdr, string parentPath, out IFtpEntry? entry)
    {
        entry = null;
        while (true)
        {
            if (rdr.TryReadTo(out ReadOnlySpan<char> line, PathSeparator) == false) return false;
            if (line.IsEmpty) continue;
            switch (line[0])
            {
                case DirectoryChar:
                    line = line[1..];
                    if (line.IsEmpty) continue;
                    entry = new FtpDirectory(line.Trim().ToString(),parentPath);
                    return true;
                case FileChar:
                    line = line[1..];
                    if (line.IsEmpty) continue;
                    var tabIndex = line.IndexOf(FileSizeSeparator);
                    if (tabIndex == -1) continue;
                    entry = new FtpFile(line[..tabIndex].Trim(DirectorySeparator).ToString(), uint.Parse(line[(tabIndex + 1)..]),parentPath);
                    return true;
            }
        }
    }
    
    public static Encoding FtpEncoding { get; } = Encoding.ASCII;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteSequenceNumber(this FileTransferProtocolPacket packet,in ushort seqNumber)
    {
        Unsafe.As<byte, ushort>(ref packet.Payload.Payload[0]) = seqNumber;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadSequenceNumber(this FileTransferProtocolPacket packet, out ushort seqNumber)
    {
        seqNumber = Unsafe.As<byte, ushort>(ref packet.Payload.Payload[0]);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort ReadSequenceNumber(this FileTransferProtocolPacket packet)
    {
        return Unsafe.As<byte, ushort>(ref packet.Payload.Payload[0]);
    }
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteSession(this FileTransferProtocolPacket packet,in byte session)
    {
        packet.Payload.Payload[2] = session;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadSession(this FileTransferProtocolPacket packet, out byte session)
    {
        session = packet.Payload.Payload[2];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadSession(this FileTransferProtocolPacket packet)
    {
        return packet.Payload.Payload[2];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteOpcode(this FileTransferProtocolPacket packet,in FtpOpcode ftpOpcode)
    {
        packet.Payload.Payload[3] = (byte)ftpOpcode;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadOpcode(this FileTransferProtocolPacket packet, out FtpOpcode ftpOpcode)
    {
        ftpOpcode = (FtpOpcode)packet.Payload.Payload[3];
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FtpOpcode ReadOpcode(this FileTransferProtocolPacket packet)
    {
        return (FtpOpcode)packet.Payload.Payload[3];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteSize(this FileTransferProtocolPacket packet,in byte size)
    {
        packet.Payload.Payload[4] = size;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadSize(this FileTransferProtocolPacket packet, out byte size)
    {
        size = packet.Payload.Payload[4];
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadSize(this FileTransferProtocolPacket packet)
    {
        return packet.Payload.Payload[4];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteOriginOpCode(this FileTransferProtocolPacket packet,in FtpOpcode offset)
    {
        packet.Payload.Payload[5] = (byte)offset;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadOriginOpCode(this FileTransferProtocolPacket packet, out FtpOpcode offset)
    {
        offset = (FtpOpcode)packet.Payload.Payload[5];
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FtpOpcode ReadOriginOpCode(this FileTransferProtocolPacket packet)
    {
        return (FtpOpcode)packet.Payload.Payload[5];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteBurstComplete(this FileTransferProtocolPacket packet, in byte complete)
    {
        packet.Payload.Payload[6] = complete;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadBurstComplete(this FileTransferProtocolPacket packet, out byte complete)
    {
        complete = packet.Payload.Payload[6];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ReadBurstComplete(this FileTransferProtocolPacket packet)
    {
        return packet.Payload.Payload[6] != 0;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WritePadding(this FileTransferProtocolPacket packet, in byte padding)
    {
        packet.Payload.Payload[7] = padding;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadPadding(this FileTransferProtocolPacket packet, out byte padding)
    {
        padding = packet.Payload.Payload[7];
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadPadding(this FileTransferProtocolPacket packet)
    {
        return packet.Payload.Payload[7];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteOffset(this FileTransferProtocolPacket packet, in uint offset)
    {
        Unsafe.As<byte, uint>(ref packet.Payload.Payload[8]) = offset;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadOffset(this FileTransferProtocolPacket packet, out uint offset)
    {
        offset = Unsafe.As<byte, uint>(ref packet.Payload.Payload[8]);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadOffset(this FileTransferProtocolPacket packet)
    {
        return Unsafe.As<byte, uint>(ref packet.Payload.Payload[8]);
    }

    #region Data section

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteData(this FileTransferProtocolPacket packet, in ReadOnlySpan<byte> data)
    {
        data.CopyTo(packet.Payload.Payload.AsSpan(12));
        packet.WriteSize((byte)data.Length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteDataAsByte(this FileTransferProtocolPacket packet, in byte firstByte)
    {
        packet.Payload.Payload[12] = firstByte;
        packet.WriteSize(sizeof(byte));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteDataAsInt(this FileTransferProtocolPacket packet, in int data)
    {
        var byteArr = new ReadOnlySpan<byte>(BitConverter.GetBytes(data));
        byteArr.CopyTo(packet.Payload.Payload.AsSpan(12));
        packet.WriteSize(sizeof(int));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteDataAsTwoByte(this FileTransferProtocolPacket packet, in byte firstByte, in byte secondByte)
    {
        packet.Payload.Payload[12] = firstByte;
        packet.Payload.Payload[12] = secondByte;
        packet.WriteSize(sizeof(byte) * 2);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadData(this FileTransferProtocolPacket packet, out ReadOnlySpan<byte> data)
    {
        var size = packet.ReadSize();
        data = packet.Payload.Payload.AsSpan(12,size);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadData(this FileTransferProtocolPacket packet, IBufferWriter<byte> data)
    {
        var size = packet.ReadSize();
        var buffer = data.GetSpan(size);
        packet.Payload.Payload.AsSpan(12,size).CopyTo(buffer);
        data.Advance(size);
        return size;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadData(this FileTransferProtocolPacket packet, Memory<byte> data)
    {
        var size = packet.ReadSize();
        packet.Payload.Payload.AsSpan(12,size).CopyTo(data.Span);
        return size;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadDataFirstByte(this FileTransferProtocolPacket packet)
    {
        return packet.Payload.Payload[12];
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadDataSecondByte(this FileTransferProtocolPacket packet)
    {
        return packet.Payload.Payload[13];
    }
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ReadDataAsString(this FileTransferProtocolPacket packet)
    {
        var size = packet.ReadSize();
        return FtpEncoding.GetString(new ReadOnlySpan<byte>(packet.Payload.Payload, 12,size));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadDataAsString(this FileTransferProtocolPacket packet, Memory<char> buffer)
    {
        var size = packet.ReadSize();
        return (byte)FtpEncoding.GetChars(new ReadOnlySpan<byte>(packet.Payload.Payload, 12,size), buffer.Span);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadDataAsInt(this FileTransferProtocolPacket packet)
    {
        var size = packet.ReadSize();
        ReadOnlySpan<byte> byteArr = packet.Payload.Payload.AsSpan(12, size);
        return BitConverter.ToInt32(byteArr);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadDataAsString(this FileTransferProtocolPacket packet, Span<char> buffer)
    {
        var size = packet.ReadSize();
        return (byte)FtpEncoding.GetChars(new ReadOnlySpan<byte>(packet.Payload.Payload, 12,size), buffer);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadDataAsString(this FileTransferProtocolPacket packet, IBufferWriter<char> data)
    {
        var size = packet.ReadSize();
        var span = new ReadOnlySpan<byte>(packet.Payload.Payload, 12, size);
        return (byte)FtpEncoding.GetChars(span, data);
    }
    
    public static void WriteDataAsString(this FileTransferProtocolPacket packet,ReadOnlySpan<char> value)
    {
        var byteSize = FtpEncoding.GetByteCount(value);
        var pathArray = ArrayPool<byte>.Shared.Rent(byteSize);
        try
        {
            FtpEncoding.GetBytes(value, pathArray);
            var pathSpan = new ReadOnlySpan<byte>(pathArray, 0, byteSize);
            packet.WriteData(pathSpan);
            packet.WriteSize((byte)byteSize);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(pathArray);
        }
    }

    public static void WriteDataAsUint(this FileTransferProtocolPacket packet,uint value)
    {
        MemoryMarshal.Write(new Span<byte>(packet.Payload.Payload,12,4),value);
        packet.WriteSize(sizeof(uint));
    }
    public static uint ReadDataAsUint(this FileTransferProtocolPacket packet)
    {
        return MemoryMarshal.Read<uint>(new ReadOnlySpan<byte>(packet.Payload.Payload,12,4));
    }
    
    #endregion
   

    public static string GetErrorMessage(NackError errorCode)
    {
        return errorCode switch
        {
            NackError.None => "No error occurred.",
            NackError.Fail => "An unknown failure occurred.",
            NackError.FailErrno => "The command failed, and an error number is sent back in PayloadHeader.data[1]. This is a file-system error number understood by the server's operating system.",
            NackError.InvalidDataSize => "The payload size is invalid.",
            NackError.InvalidSession => "The session is not currently open.",
            NackError.NoSessionsAvailable => "All available sessions are already in use.",
            NackError.EOF => "The offset is past the end of the file for ListDirectory and ReadFile commands.",
            NackError.UnknownCommand => "The command or opcode is unknown.",
            NackError.FileExists => "The file or directory already exists.",
            NackError.FileProtected => "The file or directory is write-protected.",
            NackError.FileNotFound => "The file or directory could not be found.",
            _ => throw new ArgumentOutOfRangeException(nameof(errorCode), errorCode, null)
        };
    }

    
}

/// <summary>
/// Enum representing various Opcodes used in the communication protocol.
/// </summary>
public enum FtpOpcode : byte
{
    /// <summary>
    /// None - Ignored, always ACKed.
    /// </summary>
    None = 0,

    /// <summary>
    /// TerminateSession - Terminates open Read session.
    /// Closes the file associated with the session and frees the session ID for re-use.
    /// </summary>
    TerminateSession = 1,

    /// <summary>
    /// ResetSessions - Terminates all open read sessions.
    /// Clears all state held by the drone (server); closes all open files, etc.
    /// Sends an ACK reply with no data.
    /// </summary>
    ResetSessions = 2,

    /// <summary>
    /// ListDirectory - Lists directory entry information (files, folders, etc.) in the specified path, starting from a specified entry index (offset).
    /// Response is an ACK packet with one or more entries on success, otherwise a NAK packet with an error code.
    /// Completion is indicated by a NACK with EOF in response to a requested index (offset) beyond the list of entries.
    /// The directory is closed after the operation, leaving no state on the server.
    /// </summary>
    ListDirectory = 3,

    /// <summary>
    /// OpenFileRO - Opens the file at the specified path for reading, returns the session ID.
    /// The path is stored in the payload data, and the drone opens the file (path) and allocates a session number.
    /// On success, an ACK packet contains the allocated session ID and the file size; on failure, a NAK packet contains error information.
    /// The file remains open after the operation and must eventually be closed by Reset or Terminate.
    /// </summary>
    OpenFileRO = 4,

    /// <summary>
    /// ReadFile - Reads a specified number of bytes from the file associated with a session.
    /// Seeks to the specified offset in the file and reads the specified number of bytes.
    /// Sends an ACK packet with the result buffer on success, otherwise a NAK packet with an error code.
    /// For short reads or reads beyond the end of the file, the size field in the ACK packet will indicate the actual number of bytes read.
    /// </summary>
    ReadFile = 5,

    /// <summary>
    /// CreateFile - Creates a file at the specified path for writing and returns the session ID.
    /// All parent directories must exist. If the file already exists, it is truncated.
    /// On success, an ACK packet contains the session ID; on failure, a NAK packet contains error information (e.g., FileExists).
    /// </summary>
    CreateFile = 6,

    /// <summary>
    /// WriteFile - Writes a specified number of bytes to the file associated with a session.
    /// Sends an ACK reply with no data on success, otherwise a NAK packet with an error code.
    /// </summary>
    WriteFile = 7,

    /// <summary>
    /// RemoveFile - Removes the file at the specified path.
    /// Sends an ACK reply with no data on success, otherwise a NAK packet with error information on failure.
    /// </summary>
    RemoveFile = 8,

    /// <summary>
    /// CreateDirectory - Creates a directory at the specified path.
    /// Sends an ACK reply with no data on success, otherwise a NAK packet with an error code.
    /// </summary>
    CreateDirectory = 9,

    /// <summary>
    /// RemoveDirectory - Removes a directory at the specified path.
    /// The directory must be empty.
    /// Sends an ACK reply with no data on success, otherwise a NAK packet with an error code.
    /// </summary>
    RemoveDirectory = 10,

    /// <summary>
    /// OpenFileWO - Opens the file at the specified path for writing, returns the session ID.
    /// The file is created if it does not exist.
    /// On success, an ACK packet contains the session ID; on failure, a NAK packet contains error information.
    /// The file remains open after the operation and must eventually be closed by Reset or Terminate.
    /// </summary>
    OpenFileWO = 11,

    /// <summary>
    /// TruncateFile - Truncates the file at the specified path to a given length.
    /// Sends an ACK reply with no data on success, otherwise a NAK packet with an error code.
    /// </summary>
    TruncateFile = 12,

    /// <summary>
    /// Rename - Renames the file from the specified source path to the destination path.
    /// Sends an ACK reply with no data on success, otherwise a NAK packet with an error code (e.g., if the source path does not exist).
    /// </summary>
    Rename = 13,

    /// <summary>
    /// CalcFileCRC32 - Calculates the CRC32 checksum for the file at the specified path.
    /// Sends an ACK reply with the checksum on success, otherwise a NAK packet with an error code.
    /// </summary>
    CalcFileCRC32 = 14,

    /// <summary>
    /// BurstReadFile - Burst-reads parts of a file.
    /// Messages in the burst are streamed (without ACK) until the burst is complete, as indicated by the burst_complete field set to 1.
    /// Dropped parts of the burst can be fetched using ReadFile.
    /// </summary>
    BurstReadFile = 15,
    /// <summary>
    /// Ack response
    /// </summary>
    Ack = 128,
    /// <summary>
    /// Nak response
    /// </summary>
    Nak = 129,
}

/// <summary>
/// Enum representing various error codes for the protocol.
/// NAK responses must include one of the errors codes listed below in the payload data[0] field.
/// An appropriate error code must be used if one is defined. If no appropriate error code exists, the Drone (server) may respond with Fail or FailErrno.
/// If the error code is FailErrno, then data[1] must additionally contain an error number. This error number is a file-system specific error code (understood by the server).
/// The payload size field must be set to either 1 or 2, depending on whether or not FailErrno is specified.
/// </summary>
public enum NackError : ushort
{
    /// <summary>
    /// None - No error occurred.
    /// </summary>
    None = 0,

    /// <summary>
    /// Fail - An unknown failure occurred.
    /// </summary>
    Fail = 1,

    /// <summary>
    /// FailErrno - The command failed, and an error number is sent back in PayloadHeader.data[1].
    /// This is a file-system error number understood by the server's operating system.
    /// </summary>
    FailErrno = 2,

    /// <summary>
    /// InvalidDataSize - The payload size is invalid.
    /// </summary>
    InvalidDataSize = 3,

    /// <summary>
    /// InvalidSession - The session is not currently open.
    /// </summary>
    InvalidSession = 4,

    /// <summary>
    /// NoSessionsAvailable - All available sessions are already in use.
    /// </summary>
    NoSessionsAvailable = 5,

    /// <summary>
    /// EOF - The offset is past the end of the file for ListDirectory and ReadFile commands.
    /// </summary>
    EOF = 6,

    /// <summary>
    /// UnknownCommand - The command or opcode is unknown.
    /// </summary>
    UnknownCommand = 7,

    /// <summary>
    /// FileExists - The file or directory already exists.
    /// </summary>
    FileExists = 8,

    /// <summary>
    /// FileProtected - The file or directory is write-protected.
    /// </summary>
    FileProtected = 9,

    /// <summary>
    /// FileNotFound - The file or directory could not be found.
    /// </summary>
    FileNotFound = 10
}

