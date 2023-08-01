using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink;

public class AsvSdrRecordFileSign:ISizedSpanSerializable
{
    
    
    
    public void Deserialize(ref ReadOnlySpan<byte> buffer)
    {
        throw new NotImplementedException();
    }

    public void Serialize(ref Span<byte> buffer)
    {
        throw new NotImplementedException();
    }

    public int GetByteSize()
    {
        throw new NotImplementedException();
    }

    public string Version { get; set; }
    public string Comment { get; set; }
}

public class AsvSdrRecordFileMetadata:ISizedSpanSerializable
{
    
    
    
    public void Deserialize(ref ReadOnlySpan<byte> buffer)
    {
        throw new NotImplementedException();
    }

    public void Serialize(ref Span<byte> buffer)
    {
        throw new NotImplementedException();
    }

    public int GetByteSize()
    {
        throw new NotImplementedException();
    }
}




public class AsvSdrRecordFile: DisposableOnceWithCancel,IAsvSdrRecordFile
{
    public const string SignComment = "This is AsvSdrRecordFile V 1.0";
    public static SemVersion CurrentFileVersion = new(1, 0);
    
    // File sign
    // File metadata
    // Raws: [StartRawMagicByte] [DATA] [CRC] [StopRawMagicByte]

    private const int FileSignSize = 256;
    
    private const int StartFileMetadataOffset = FileSignSize;
    private const int FileMetadataSize = 1024 * 4;
    private const int FileMetadataCrcIndex = FileMetadataSize - 2;
    
    private const int StartDataOffset = FileSignSize + FileMetadataSize;
    private const int RawDataSize = 256;
    private const int RawSize = 1 /*StartRawMagicByte*/ + RawDataSize /*MAX DATA SIZE*/ + 2 /*CRC*/ + 1 /*StopRawMagicByte*/;
    private const int StartRawByteIndex = 0;
    private const byte StartRawMagicByte = 0x0A;
    private const int StopRawByteIndex = RawSize - 1;
    private const byte StopRawMagicByte = 0x0D;
    private const int StartRawCrcIndex = RawSize - 3;
   
    private readonly Stream _stream;
    private readonly object _sync = new();
    
    
    private static void GetStartStopRawAddress(uint index, out uint start, out uint stop)
    {
        start = index * RawSize + StartDataOffset;
        stop = start + RawSize + StartDataOffset;
    }
    
    private static ushort CalculateMetadataCrc(ReadOnlySpan<byte> metadata)
    {
        metadata = metadata.Slice(FileMetadataSize -sizeof(ushort) /*CRC*/);
        return X25Crc.Accumulate(ref metadata, X25Crc.CrcSeed);
    }
    
    private static ushort ReadMetadataCrc(ReadOnlySpan<byte> metadata)
    {
        metadata = metadata.Slice(FileMetadataCrcIndex);
        return BinSerialize.ReadUShort(ref metadata);
    }
    
    private static ushort CalculateRawCrc(ReadOnlySpan<byte> raw)
    {
        raw = raw.Slice(1 /* START BYTE */, RawSize - 1 /*END BYTE*/ - sizeof(ushort) /*CRC*/);
        return X25Crc.Accumulate(ref raw, X25Crc.CrcSeed);
    }
    
    private static ushort ReadRawCrc(ReadOnlySpan<byte> raw)
    {
        raw = raw.Slice(StartRawCrcIndex);
        return BinSerialize.ReadUShort(ref raw);
    }

    public AsvSdrRecordFile(Stream stream, bool disposeStream = true)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        if (disposeStream)
        {
            Disposable.AddAction(stream.Dispose);
        }
        
        
        var sign = new AsvSdrRecordFileSign();
        var data = ArrayPool<byte>.Shared.Rent(FileSignSize);
        try
        {
            if (_stream.Length < FileSignSize)
            {
                sign.Version = CurrentFileVersion.ToString();
                sign.Comment = SignComment;
                var serializeSpan = new Span<byte>(data, 0, FileSignSize);
                sign.Serialize(ref serializeSpan);
                var writeSpan = new ReadOnlySpan<byte>(data, 0, FileSignSize);
                _stream.Write(writeSpan);
            }
            else
            {
                var readSpan = new Span<byte>(data, 0, FileSignSize);
                var readCount = _stream.Read(readSpan);
                Debug.Assert(readCount == FileSignSize);
                var deserializeSpan = new ReadOnlySpan<byte>(data, 0, FileSignSize);
                sign.Deserialize(ref deserializeSpan);
                if (SemVersion.Parse(sign.Version) != CurrentFileVersion)
                {
                    throw new Exception($"Unknown file version. Want {CurrentFileVersion}, got {sign.Version}");
                }
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
    }

    public void EditMetadata(Action<AsvSdrRecordFileMetadata> editCallback)
    {
        var metadata = new AsvSdrRecordFileMetadata();
        var data = ArrayPool<byte>.Shared.Rent(FileMetadataSize);
        try
        {
            lock (_sync)
            {
                var readSpan = new Span<byte>(data, 0, FileMetadataSize);
                var readCount = _stream.Read(readSpan);
                if (readCount == FileMetadataSize)
                {
                    var calc = CalculateMetadataCrc(readSpan);
                    var readCrc = ReadMetadataCrc(readSpan);
                    if (calc == readCrc)
                    {
                        var deserializeSpan = new ReadOnlySpan<byte>(data, 0, FileMetadataSize);
                        metadata.Deserialize(ref deserializeSpan);    
                    }
                }
                editCallback(metadata);
                var serializeSpan = new Span<byte>(data, 0, FileMetadataSize - 2 /* without CRC*/);
                metadata.Serialize(ref serializeSpan);
                
                var writeSpan = new Span<byte>(data, 0, FileMetadataSize);
                var calcCrc = CalculateMetadataCrc(writeSpan);
                
                var crcSpan = new Span<byte>(data, FileMetadataSize - 2,  2);
                BinSerialize.WriteUShort(ref crcSpan,calcCrc);
                
                _stream.Seek(StartFileMetadataOffset, SeekOrigin.Begin);
                _stream.Write(writeSpan);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
       
    }

    public uint Count
    {
        get
        {
            lock (_sync)    
            {
                return (uint)Math.Max((_stream.Length - StartDataOffset) / RawSize, 0);
            }
        }
    }

    public long Size
    {
        get
        {
            lock (_sync)    
            {
                return _stream.Length;
            }
        }
    }

    public bool Exist(uint index)
    {
        GetStartStopRawAddress(index, out var start, out var stop);
        
        var data = ArrayPool<byte>.Shared.Rent(RawSize);
        try
        {
            var buffer = new Span<byte>(data, 0, RawSize);
            lock (_sync)
            {
                if (_stream.Length <= stop) return false;
                _stream.Seek(start, SeekOrigin.Begin);
                var readBytes = _stream.Read(buffer);
                Debug.Assert(readBytes != RawSize);
            }
            return ValidateRecord(buffer);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
    }

    public void Write(uint index, IPayload payload)
    {
        GetStartStopRawAddress(index, out var start, out var _);
        var data = ArrayPool<byte>.Shared.Rent(RawSize);
        try
        {
            var page = new Span<byte>(data, 0, RawSize);
            var buffer = page;
            BinSerialize.WriteByte(ref buffer, StartRawMagicByte);
            payload.Serialize(ref buffer);
            var calcCrc = CalculateRawCrc(page);
            BinSerialize.WriteUInt(ref buffer, calcCrc);
            lock (_sync)
            {
                _stream.Seek(start, SeekOrigin.Begin);
                _stream.Write(page);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
    }

    public bool Read(uint index, ref IPayload payload)
    {
        GetStartStopRawAddress(index, out var start, out var stop);
        var data = ArrayPool<byte>.Shared.Rent(RawSize);
        try
        {
            var page = new Span<byte>(data, 0, RawSize);
            lock (_sync)
            {
                // there is no data
                if (_stream.Length <= stop) return false;
                _stream.Seek(start, SeekOrigin.Begin);
                var readBytes = _stream.Read(page);
                Debug.Assert(readBytes!= RawSize);
            }
            if (ValidateRecord(page) == false) return false;
            ReadOnlySpan<byte> buffer = page.Slice(StartRawByteIndex, RawDataSize);
            payload.Deserialize(ref buffer);
            Debug.Assert(buffer.Length == 0);
            return true;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
    }

    
    private bool ValidateRecord(ReadOnlySpan<byte> page)
    {
        if (page[StartRawByteIndex] != StartRawMagicByte) return false;
        if (page[StopRawByteIndex] != StopRawMagicByte) return false;
        var calculatedCrc = CalculateRawCrc(page);
        var readCrc = ReadRawCrc(page);
        return calculatedCrc == readCrc;
    }
}