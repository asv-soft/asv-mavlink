#nullable enable
using System;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink;

public interface IListDataFileFormat
{
    SemVersion Version { get; }
    string Type { get; }
    ushort MetadataMaxSize { get; }
    ushort ItemMaxSize { get;  }
}


public class ListDataFileFormat:ISizedSpanSerializable, IEquatable<ListDataFileFormat>,IListDataFileFormat
{
    public const int MaxSize = 256;
    
    public SemVersion Version { get; set; } = null!;
    public string Type { get; set; } = null!;
    public ushort MetadataMaxSize { get; set; }
    public ushort ItemMaxSize { get; set; }

    public void Deserialize(ref ReadOnlySpan<byte> buffer)
    {
        Type = BinSerialize.ReadString(ref buffer);
        Version = BinSerialize.ReadString(ref buffer);
        MetadataMaxSize = BinSerialize.ReadUShort(ref buffer);
        ItemMaxSize = BinSerialize.ReadUShort(ref buffer);
    }

    public void Serialize(ref Span<byte> buffer)
    {
        BinSerialize.WriteString(ref buffer,Type);
        BinSerialize.WriteString(ref buffer,Version.ToString());
        BinSerialize.WriteUShort(ref buffer,MetadataMaxSize);
        BinSerialize.WriteUShort(ref buffer,ItemMaxSize);
    }

    public int GetByteSize()
    {
        return BinSerialize.GetSizeForString(Type) + 
               BinSerialize.GetSizeForString(Version.ToString()) + 
               sizeof(ushort) + 
               sizeof(ushort);
    }

    public bool Equals(ListDataFileFormat? other)
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
        return Equals((ListDataFileFormat)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Version, Type, MetadataMaxSize, ItemMaxSize);
    }

    public static bool operator ==(ListDataFileFormat? left, ListDataFileFormat? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ListDataFileFormat? left, ListDataFileFormat? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"{nameof(Version)}: {Version}, {nameof(Type)}: {Type}, {nameof(MetadataMaxSize)}: {MetadataMaxSize}, {nameof(ItemMaxSize)}: {ItemMaxSize}";
    }

    public void Validate()
    {
        //validate fields
        if (Version == null!) throw new InvalidOperationException("Version is null");
        if (string.IsNullOrWhiteSpace(Type)) throw new InvalidOperationException("Type is null or empty");
        if (MetadataMaxSize == 0) throw new InvalidOperationException("MetadataMaxSize is 0");
        if (ItemMaxSize == 0) throw new InvalidOperationException("ItemMaxSize is 0");
        if (GetByteSize() + 2 /*CRC*/ >= MaxSize) throw new InvalidOperationException("Header size is too big");
    }

    
}