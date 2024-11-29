#nullable enable
using System;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink;

/// <summary>
/// Represents the data format of a list data file.
/// </summary>
public interface IListDataFileFormat
{
    /// <summary>
    /// Gets the semantic version of the software.
    /// </summary>
    /// <remarks>
    /// The semantic version follows the format MAJOR.MINOR.PATCH.
    /// - MAJOR version indicates incompatible changes.
    /// - MINOR version indicates new features that are backward-compatible.
    /// - PATCH version indicates bug fixes that are backward-compatible.
    /// </remarks>
    /// <returns>
    /// The semantic version of the software.
    /// </returns>
    SemVersion Version { get; }

    /// <summary>
    /// Gets the type of the property.
    /// </summary>
    /// <returns>
    /// A string value representing the type of the property.
    /// </returns>
    string Type { get; }

    /// <summary>
    /// Gets the maximum size of metadata.
    /// </summary>
    /// <value>
    /// The maximum size of metadata.
    /// </value>
    /// <remarks>
    /// This property returns the maximum allowed size for metadata in bytes.
    /// </remarks>
    /// <seealso cref="MetadataManager"/>
    ushort MetadataMaxSize { get; }

    /// <summary>
    /// Gets the maximum size of an item.
    /// </summary>
    /// <value>
    /// The maximum size of an item.
    /// </value>
    ushort ItemMaxSize { get;  }
}

/// <summary>
/// Represents a file format for storing list data.
/// </summary>
public class ListDataFileFormat:ISizedSpanSerializable, IEquatable<ListDataFileFormat>,IListDataFileFormat
{
    /// <summary>
    /// The maximum size allowed for a variable.
    /// </summary>
    public const int MaxSize = 256;

    /// <summary>
    /// Gets or sets the version of the property.
    /// </summary>
    /// <value>
    /// A <see cref="SemVersion"/> object representing the version of the property.
    /// </value>
    /// <remarks>
    /// This property allows you to access or modify the version of the property.
    /// </remarks>
    public SemVersion Version { get; set; } = null!;

    /// <summary>
    /// Gets or sets the type property.
    /// </summary>
    /// <value>
    /// The type property.
    /// </value>
    /// <remarks>
    /// The type property represents the type of an object.
    /// </remarks>
    public string Type { get; set; } = null!;

    /// <summary>
    /// Gets or sets the maximum size of metadata.
    /// </summary>
    /// <value>
    /// The maximum size of metadata.
    /// </value>
    public ushort MetadataMaxSize { get; set; }

    /// <summary>
    /// Gets or sets the maximum size of an item.
    /// </summary>
    /// <value>
    /// The maximum size of an item.
    /// </value>
    public ushort ItemMaxSize { get; set; }

    /// <summary>
    /// Deserializes the data from a byte buffer.
    /// </summary>
    /// <param name="buffer">The byte buffer containing the serialized data.</param>
    public void Deserialize(ref ReadOnlySpan<byte> buffer)
    {
        Type = BinSerialize.ReadString(ref buffer);
        Version = BinSerialize.ReadString(ref buffer);
        MetadataMaxSize = BinSerialize.ReadUShort(ref buffer);
        ItemMaxSize = BinSerialize.ReadUShort(ref buffer);
    }

    /// <summary>
    /// Serializes the data into the provided byte buffer.
    /// </summary>
    /// <param name="buffer">The byte buffer to serialize the data into.</param>
    public void Serialize(ref Span<byte> buffer)
    {
        BinSerialize.WriteString(ref buffer,Type);
        BinSerialize.WriteString(ref buffer,Version.ToString());
        BinSerialize.WriteUShort(ref buffer,MetadataMaxSize);
        BinSerialize.WriteUShort(ref buffer,ItemMaxSize);
    }

    /// <summary>
    /// Gets the byte size of the object.
    /// </summary>
    /// <returns>The byte size of the object.</returns>
    public int GetByteSize()
    {
        return BinSerialize.GetSizeForString(Type) + 
               BinSerialize.GetSizeForString(Version.ToString()) + 
               sizeof(ushort) + 
               sizeof(ushort);
    }

    /// <summary>
    /// Determines whether the current ListDataFileFormat instance is equal to another ListDataFileFormat instance.
    /// </summary>
    /// <param name="other">The ListDataFileFormat instance to compare with the current instance.</param>
    /// <returns>True if the current instance is equal to the other instance; otherwise, false.</returns>
    public bool Equals(ListDataFileFormat? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Version == other.Version && Type == other.Type && MetadataMaxSize == other.MetadataMaxSize && ItemMaxSize == other.ItemMaxSize;
    }

    /// <summary>
    /// Determines whether the current instance and the specified object are equal.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    /// <c>true</c> if the current instance is equal to the specified object;
    /// otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ListDataFileFormat)obj);
    }

    /// <summary>
    /// Overrides the GetHashCode method to provide a unique hash code for the object.
    /// </summary>
    /// <returns>
    /// A unique hash code based on the values of the Version, Type, MetadataMaxSize, and ItemMaxSize properties.
    /// </returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Version, Type, MetadataMaxSize, ItemMaxSize);
    }

    /// <summary>
    /// Checks if two instances of ListDataFileFormat are equal.
    /// </summary>
    /// <param name="left">The left-hand side ListDataFileFormat instance.</param>
    /// <param name="right">The right-hand side ListDataFileFormat instance.</param>
    /// <returns>True if the instances are equal, otherwise false.</returns>
    public static bool operator ==(ListDataFileFormat? left, ListDataFileFormat? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Checks if two ListDataFileFormat objects are not equal.
    /// </summary>
    /// <param name="left">The first ListDataFileFormat object.</param>
    /// <param name="right">The second ListDataFileFormat object.</param>
    /// <returns>True if the two objects are not equal, otherwise false.</returns>
    public static bool operator !=(ListDataFileFormat? left, ListDataFileFormat? right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    /// Returns a string representation of the object.
    /// </summary>
    /// <returns>A string representation of the object.</returns>
    public override string ToString()
    {
        return $"{nameof(Version)}: {Version}, {nameof(Type)}: {Type}, {nameof(MetadataMaxSize)}: {MetadataMaxSize}, {nameof(ItemMaxSize)}: {ItemMaxSize}";
    }

    /// <summary>
    /// Validates the fields of the object.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when any of the fields are invalid.</exception>
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