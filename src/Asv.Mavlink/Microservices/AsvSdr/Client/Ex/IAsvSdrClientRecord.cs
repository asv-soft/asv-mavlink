using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.AsvSdr;

using ObservableCollections;
using R3;

namespace Asv.Mavlink;

/// Represents a client record in the ASV SDR system.
/// /
public interface IAsvSdrClientRecord
{
    /// <summary>
    /// Represents the unique identifier of a property. </summary> <value>
    /// The unique identifier of the property. </value>
    /// /
    Guid Id { get; }

    /// <summary>
    /// Gets the data type of the value.
    /// </summary>
    /// <value>
    /// The data type of the value as an <see cref="ReadOnlyReactiveProperty{T}"/> where T is <see cref="AsvSdrCustomMode"/>.
    /// </value>
    ReadOnlyReactiveProperty<AsvSdrCustomMode> DataType { get; }

    /// Gets the value of the Name property. </summary> <value>
    /// The value of the Name property. </value>
    /// /
    ReadOnlyReactiveProperty<string> Name { get; }

    /// <summary>
    /// Gets the frequency of the signal.
    /// </summary>
    /// <returns>
    /// The frequency represented by an <see cref="ReadOnlyReactiveProperty"/> of type <see cref="ulong"/>.
    /// </returns>
    ReadOnlyReactiveProperty<ulong> Frequency { get; }

    /// <summary>
    /// Gets the ReactiveProperty representing the creation date and time of an object.
    /// </summary>
    /// <returns>
    /// The ReactiveProperty object that represents the creation date and time of an object.
    /// </returns>
    ReadOnlyReactiveProperty<DateTime> Created { get; }

    /// <summary>
    /// Gets the number of tags.
    /// </summary>
    /// <remarks>
    /// This property represents the count of tags in a collection.
    /// </remarks>
    /// <returns>
    /// An interface representing a reactive value of type ushort, indicating the number of tags.
    /// </returns>
    ReadOnlyReactiveProperty<ushort> TagsCount { get; }

    /// <summary>
    /// Gets the data count property.
    /// </summary>
    /// <remarks>
    /// This property represents the total count of data.
    /// </remarks>
    /// <value>
    /// An <see cref="ReadOnlyReactiveProperty{T}"/> object representing the data count.
    /// </value>
    ReadOnlyReactiveProperty<uint> DataCount { get; }

    /// <summary>
    /// Gets the byte size of the value.
    /// </summary>
    /// <remarks>
    /// The ByteSize property returns the byte size of the value contained in the ReadOnlyReactiveProperty.
    /// </remarks>
    /// <returns>
    /// An ReadOnlyReactiveProperty of type uint representing the byte size of the value.
    /// </returns>
    ReadOnlyReactiveProperty<uint> ByteSize { get; }

    /// <summary>
    /// Gets the duration of this object.
    /// </summary>
    /// <remarks>
    /// The Duration property returns an <see cref="ReadOnlyReactiveProperty{TimeSpan}"/> that represents the duration of an object.
    /// </remarks>
    /// <returns>
    /// An <see cref="ReadOnlyReactiveProperty{TimeSpan}"/> representing the duration.
    /// </returns>
    ReadOnlyReactiveProperty<TimeSpan> Duration { get; }

    /// <summary>
    /// Gets an observable sequence of changes to the collection of tags.
    /// </summary>
    /// <returns>
    /// An <see cref="Observable{T}"/> sequence where each change represents a collection of tags.
    /// The change is represented by an <see cref="IChangeSet{TObject, TKey}"/> where TObject is <see cref="AsvSdrClientRecordTag"/>
    /// and TKey is <see cref="TagId"/>.
    /// </returns>
    IReadOnlyObservableDictionary<TagId, AsvSdrClientRecordTag> Tags { get; }

    /// <summary>
    /// Downloads the tag list.
    /// </summary>
    /// <param name="progress">An optional progress object to report the download progress.</param>
    /// <param name="cancel">An optional cancellation token to cancel the download operation.</param>
    /// <returns>A task representing the asynchronous operation. The task will be completed with a boolean value indicating whether the download is successful or not.</returns>
    Task<bool> DownloadTagList(IProgress<double>? progress = null, CancellationToken cancel = default);

    /// <summary>
    /// Deletes a tag with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the tag to delete.</param>
    /// <param name="cancel">Cancellation token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteTag(TagId id, CancellationToken cancel);
}

/// <summary>
/// Helper class for copying metadata between <see cref="IAsvSdrClientRecord"/> and <see cref="IListDataFile{TMetadata}"/>.
/// </summary>
public static class AsvSdrClientRecordHelper
{
    /// <summary>
    /// Updates the metadata of the <see cref="IListDataFile{T}"/> from the given <see cref="IAsvSdrClientRecord"/>.
    /// </summary>
    /// <typeparam name="T">The type of metadata of the <see cref="IListDataFile{T}"/>.</typeparam>
    /// <param name="self">The <see cref="IListDataFile{T}"/> instance.</param>
    /// <param name="src">The source <see cref="IAsvSdrClientRecord"/> instance to copy metadata from.</param>
    public static void UpdateMetadataFrom(this IListDataFile<AsvSdrRecordFileMetadata> self, IAsvSdrClientRecord src)
    {
        self.EditMetadata(src.CopyTo);
    }

    /// <summary>
    /// Copies the metadata from the current record to the destination list data file.
    /// </summary>
    /// <param name="self">The current record.</param>
    /// <param name="dest">The destination list data file.</param>
    public static void CopyMetadataTo(this IAsvSdrClientRecord self, IListDataFile<AsvSdrRecordFileMetadata> dest)
    {
        dest.EditMetadata(self.CopyTo);
    }


    /// <summary>
    /// Copies the contents of the <paramref name="self"/> object to the specified <paramref name="dest"/> object.
    /// </summary>
    /// <param name="self">The source <see cref="IAsvSdrClientRecord"/> object to copy from.</param>
    /// <param name="dest">The destination <see cref="AsvSdrRecordFileMetadata"/> object to copy to.</param>
    public static void CopyTo(this IAsvSdrClientRecord self, AsvSdrRecordFileMetadata dest)
    {
        self.CopyTo(dest.Info);
        foreach (var tag in self.Tags.Select(x=>x.Value))
        {
            var tagPayload = new AsvSdrRecordTagPayload();
            tag.CopyTo(tagPayload);
            dest.Tags.Add(tagPayload);
        }
    }

    /// <summary>
    /// Copies the values of the AsvSdrClientRecordTag object to the specified AsvSdrRecordTagPayload object.
    /// </summary>
    /// <param name="self">The source AsvSdrClientRecordTag object.</param>
    /// <param name="dest">The destination AsvSdrRecordTagPayload object.</param>
    public static void CopyTo(this AsvSdrClientRecordTag self, AsvSdrRecordTagPayload dest)
    {
        self.Id.RecordGuid.ToByteArray().CopyTo(dest.RecordGuid,0);
        self.Id.TagGuid.ToByteArray().CopyTo(dest.TagGuid,0);
        self.RawValue.CopyTo(dest.TagValue,0);
        self.Name.CopyTo(dest.TagName);
        dest.TagType = self.Type;
    }

    /// Copies the properties of an IAsvSdrClientRecord object to an AsvSdrRecordPayload object.
    /// @param self The source IAsvSdrClientRecord object.
    /// @param dest The destination AsvSdrRecordPayload object.
    /// /
    public static void CopyTo(this IAsvSdrClientRecord self, AsvSdrRecordPayload dest)
    {
        MavlinkTypesHelper.SetGuid(dest.RecordGuid, self.Id);
        MavlinkTypesHelper.SetString(dest.RecordName,self.Name.CurrentValue);
        dest.Frequency = self.Frequency.CurrentValue;
        dest.CreatedUnixUs = MavlinkTypesHelper.ToUnixTimeUs(self.Created.CurrentValue);
        dest.DataType = self.DataType.CurrentValue;
        dest.DurationSec = (uint)self.Duration.CurrentValue.TotalSeconds;
        dest.DataCount = self.DataCount.CurrentValue;
        dest.Size = self.ByteSize.CurrentValue;
        dest.TagCount = self.TagsCount.CurrentValue;
        
    }
}


