#nullable enable
using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink;

public class ListDataFile<TMetadata> : IListDataFile<TMetadata> 
    where TMetadata : ISpanSerializable, new()
{
    #region File header
    
    private const int FileHeaderMaxSize = 256;
    
    private static ushort CalculateHeaderCrc(ReadOnlySpan<byte> buffer)
    {
        buffer = buffer.Slice(0,FileHeaderMaxSize - sizeof(ushort) /*CRC*/);
        return X25Crc.Accumulate(ref buffer, X25Crc.CrcSeed);
    }
    
    private static ushort ReadHeaderCrc(ReadOnlySpan<byte> buffer)
    {
        buffer = buffer.Slice(FileHeaderMaxSize - sizeof(ushort) /*CRC*/);
        return BinSerialize.ReadUShort(ref buffer);
    }
    
    public static ListDataFileHeader? ReadHeader(Stream stream)
    {
        if (stream.Length < FileHeaderMaxSize) return null;
        var header = new ListDataFileHeader();
        var data = ArrayPool<byte>.Shared.Rent(FileHeaderMaxSize);
        for (var i = 0; i < FileHeaderMaxSize; i++)
        {
            data[i] = 0;
        }

        try
        {
            var readSpan = new Span<byte>(data, 0, FileHeaderMaxSize);
            var crc = CalculateHeaderCrc(readSpan);
            var readCrc = ReadHeaderCrc(readSpan);
            if (crc != readCrc) return null;
            
            stream.Seek(0, SeekOrigin.Begin);
            var readCount = stream.Read(readSpan);
            Debug.Assert(readCount == FileHeaderMaxSize);
            var deserializeSpan = new ReadOnlySpan<byte>(data, 0, FileHeaderMaxSize);
            header.Deserialize(ref deserializeSpan);
            
            return header;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
    }
    
    #endregion

    private const byte StartRowMagicByte = 0x0A;
    private const byte StopRowMagicByte = 0x0D;
    
    private readonly Stream _stream;
    private readonly bool _disposeSteam;
    private readonly object _sync = new();
    private readonly int _startFileMetadataOffset;
    private readonly int _fileMetadataSize;
    private readonly int _startDataOffset;
    private readonly int _rowDataSize;
    private readonly int _rowSize;
    private readonly int _startRowByteIndex;
    private readonly int _stopRowByteIndex;

    public ListDataFile(Stream stream, ListDataFileHeader header, bool disposeSteam)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        if (_stream.CanSeek == false) throw new Exception("Need stream with seek support");
        _disposeSteam = disposeSteam;
        _startFileMetadataOffset = FileHeaderMaxSize;
        _fileMetadataSize = header.MetadataMaxSize;
        _startDataOffset = FileHeaderMaxSize + _fileMetadataSize;
        _rowDataSize = header.ItemMaxSize;
        _rowSize = 1 /*StartRowMagicByte*/ + _rowDataSize /*MAX DATA SIZE*/ + 1 /*StopRowMagicByte*/ + 2 /*CRC*/;
        _startRowByteIndex = 0;
        _stopRowByteIndex = _rowSize - 3;

        var data = ArrayPool<byte>.Shared.Rent(FileHeaderMaxSize);
        for (var i = 0; i < FileHeaderMaxSize; i++)
        {
            data[i] = 0;
        }

        try
        {
            if (_stream.Length < FileHeaderMaxSize)
            {
                // no header => write origin 
                var serializeSpan = new Span<byte>(data, 0, FileHeaderMaxSize);
                header.Serialize(ref serializeSpan);
                _stream.Flush();
            }
            else
            {
                var readSpan = new Span<byte>(data, 0, FileHeaderMaxSize);
                _stream.Seek(0, SeekOrigin.Begin);
                var readCount = _stream.Read(readSpan);
                Debug.Assert(readCount == FileHeaderMaxSize);
                var deserializeSpan = new ReadOnlySpan<byte>(data, 0, FileHeaderMaxSize);
                var readHeader = new ListDataFileHeader();
                readHeader.Deserialize(ref deserializeSpan);
                if (readHeader.Equals(header) == false)
                {
                    throw new Exception($"Unknown file version. Want {header}, got {readHeader}");
                }
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
        
        
        
        
    }

    public long ByteSize
    {
        get
        {
            lock (_sync)    
            {
                return _stream.Length;
            }
        }
    }

    #region Metadata

    private ushort CalculateMetadataCrc(ReadOnlySpan<byte> metadata)
    {
        metadata = metadata.Slice(0,_fileMetadataSize - sizeof(ushort) /*CRC*/);
        return X25Crc.Accumulate(ref metadata, X25Crc.CrcSeed);
    }
    
    private ushort ReadMetadataCrc(ReadOnlySpan<byte> metadata)
    {
        metadata = metadata.Slice(_fileMetadataSize - sizeof(ushort) /*CRC*/);
        return BinSerialize.ReadUShort(ref metadata);
    }
    
    private void WriteMetadataCrc(Span<byte> metadata, ushort crc)
    {
        metadata = metadata.Slice(_fileMetadataSize - sizeof(ushort) /*CRC*/);
        BinSerialize.WriteUShort(ref metadata, crc);
    }
    
    public void EditMetadata(Action<TMetadata> editCallback)
    {
        var metadata = new TMetadata();
        var data = ArrayPool<byte>.Shared.Rent(_fileMetadataSize);
        for (var i = 0; i < _fileMetadataSize; i++)
        {
            data[i] = 0;
        }
        try
        {
            lock (_sync)
            {
                var readSpan = new Span<byte>(data, 0, _fileMetadataSize);
                
                if (_stream.Length >= FileHeaderMaxSize + _fileMetadataSize)
                {
                    _stream.Seek(_startFileMetadataOffset, SeekOrigin.Begin);
                    var readCount = _stream.Read(readSpan);
                    Debug.Assert(readCount == _fileMetadataSize);
                    var calc = CalculateMetadataCrc(readSpan);
                    var readCrc = ReadMetadataCrc(readSpan);
                    if (calc == readCrc)
                    {
                        var deserializeSpan = new ReadOnlySpan<byte>(data, 0, _fileMetadataSize - 2 /* without CRC*/);
                        metadata.Deserialize(ref deserializeSpan);    
                    }
                }
                editCallback(metadata);
                var serializeSpan = new Span<byte>(data, 0, _fileMetadataSize - 2 /* without CRC*/);
                metadata.Serialize(ref serializeSpan);
                
                var writeSpan = new Span<byte>(data, 0, _fileMetadataSize);
                var calcCrc = CalculateMetadataCrc(writeSpan);
                WriteMetadataCrc(writeSpan, calcCrc);
                
                _stream.Seek(_startFileMetadataOffset, SeekOrigin.Begin);
                _stream.Write(writeSpan);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
    }

    public TMetadata ReadMetadata()
    {
        var metadata = new TMetadata();
        var data = ArrayPool<byte>.Shared.Rent(_fileMetadataSize);
        for (var i = 0; i < _fileMetadataSize; i++)
        {
            data[i] = 0;
        }
        lock (_sync)
        {
            var readSpan = new Span<byte>(data, 0, _fileMetadataSize);
                
            if (_stream.Length >= FileHeaderMaxSize + _fileMetadataSize)
            {
                _stream.Seek(_startFileMetadataOffset, SeekOrigin.Begin);
                var readCount = _stream.Read(readSpan);
                Debug.Assert(readCount == _fileMetadataSize);
                var calc = CalculateMetadataCrc(readSpan);
                var readCrc = ReadMetadataCrc(readSpan);
                if (calc == readCrc)
                {
                    var deserializeSpan = new ReadOnlySpan<byte>(data, 0, _fileMetadataSize - 2 /* without CRC*/);
                    metadata.Deserialize(ref deserializeSpan);    
                }
            }
            return metadata;
        }
    }
    

    #endregion
    
    #region Row

    public uint Count
    {
        get
        {
            lock (_sync)    
            {
                return (uint)Math.Max((_stream.Length - _startDataOffset) / _rowSize, 0);
            }
        }
    }
    private void GetStartStopRowAddress(int index, out int start, out int stop)
    {
        start = index * _rowSize + _startDataOffset;
        stop = start + _rowSize;
    }
    
    private ushort CalculateRowCrc(ReadOnlySpan<byte> row)
    {
        row = row.Slice(0, _rowSize - sizeof(ushort) /*CRC*/);
        return X25Crc.Accumulate(ref row, X25Crc.CrcSeed);
    }
    
    private ushort ReadRowCrc(ReadOnlySpan<byte> row)
    {
        row = row.Slice(_rowSize - sizeof(ushort) /*CRC*/);
        return BinSerialize.ReadUShort(ref row);
    }
    
    private void WriteRowCrc(Span<byte> row, ushort crc)
    {
        row = row.Slice(_rowSize - sizeof(ushort) /*CRC*/);
        BinSerialize.WriteUShort(ref row, crc);
    }
    
    private bool ValidateRaw(ReadOnlySpan<byte> row)
    {
        if (row[_startRowByteIndex] != StartRowMagicByte) return false;
        if (row[_stopRowByteIndex] != StopRowMagicByte) return false;
        var calculatedCrc = CalculateRowCrc(row);
        var readCrc = ReadRowCrc(row);
        return calculatedCrc == readCrc;
    }
    
    public bool Exist(int index)
    {
        if (index<0) return false;
        GetStartStopRowAddress(index, out var start, out var stop);
        
        var data = ArrayPool<byte>.Shared.Rent(_rowSize);
        try
        {
            var buffer = new Span<byte>(data, 0, _rowSize);
            lock (_sync)
            {
                if (_stream.Length <= stop) return false;
                _stream.Seek(start, SeekOrigin.Begin);
                var readBytes = _stream.Read(buffer);
                Debug.Assert(readBytes == _rowSize);
            }
            return ValidateRaw(buffer);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
    }

    public void Write(int index, ISpanSerializable payload)
    {
        if (index < 0) throw new Exception($"{nameof(index)} must be positive or 0");
        GetStartStopRowAddress(index, out var start, out var _);
        var data = ArrayPool<byte>.Shared.Rent(_rowSize);
        for (var i = 0; i < _rowSize; i++)
        {
            data[i] = 0;
        }
        try
        {
            var row = new Span<byte>(data, 0, _rowSize);
            var buffer = row;
            BinSerialize.WriteByte(ref buffer, StartRowMagicByte);
            payload.Serialize(ref buffer);
            data[_stopRowByteIndex] = StopRowMagicByte;
            var calcCrc = CalculateRowCrc(row);
            WriteRowCrc(row, calcCrc);
            lock (_sync)
            {
                _stream.Seek(start, SeekOrigin.Begin);
                _stream.Write(row);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
    }

    public bool Read(int index, ISpanSerializable payload)
    {
        if (index < 0) throw new Exception($"{nameof(index)} must be positive or 0");
        
        GetStartStopRowAddress(index, out var start, out var stop);
        var data = ArrayPool<byte>.Shared.Rent(_rowSize);
        try
        {
            var row = new Span<byte>(data, 0, _rowSize);
            lock (_sync)
            {
                // there is no data
                if (_stream.Length < stop) return false;
                _stream.Seek(start, SeekOrigin.Begin);
                var readBytes = _stream.Read(row);
                Debug.Assert(readBytes == _rowSize);
            }
            if (ValidateRaw(row) == false) return false;
            ReadOnlySpan<byte> buffer = row.Slice(1/*START BYTE*/, _rowDataSize);
            payload.Deserialize(ref buffer);
            return true;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
    }

    #endregion
    
    public void Dispose()
    {
        if (_disposeSteam) _stream.Dispose();
    }
}

public class ListDataFileHeader:ISizedSpanSerializable, IEquatable<ListDataFileHeader>
{
    public SemVersion Version { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int MetadataMaxSize { get; set; }
    public int ItemMaxSize { get; set; }

    public void Deserialize(ref ReadOnlySpan<byte> buffer)
    {
        Type = BinSerialize.ReadString(ref buffer);
        Version = BinSerialize.ReadString(ref buffer);
        MetadataMaxSize = BinSerialize.ReadInt(ref buffer);
        ItemMaxSize = BinSerialize.ReadInt(ref buffer);
    }

    public void Serialize(ref Span<byte> buffer)
    {
        BinSerialize.WriteString(ref buffer,Type);
        BinSerialize.WriteString(ref buffer,Version.ToString());
        BinSerialize.WriteInt(ref buffer,MetadataMaxSize);
        BinSerialize.WriteInt(ref buffer,ItemMaxSize);
    }

    public int GetByteSize()
    {
        return BinSerialize.GetSizeForString(Type) + 
               BinSerialize.GetSizeForString(Version.ToString()) + 
               sizeof(int) + 
               sizeof(int);
    }

    public bool Equals(ListDataFileHeader? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Version == other.Version && Type == other.Type && MetadataMaxSize == other.MetadataMaxSize && ItemMaxSize == other.ItemMaxSize;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ListDataFileHeader)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Version, Type, MetadataMaxSize, ItemMaxSize);
    }

    public static bool operator ==(ListDataFileHeader? left, ListDataFileHeader? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ListDataFileHeader? left, ListDataFileHeader? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"{nameof(Version)}: {Version}, {nameof(Type)}: {Type}, {nameof(MetadataMaxSize)}: {MetadataMaxSize}, {nameof(ItemMaxSize)}: {ItemMaxSize}";
    }
}
