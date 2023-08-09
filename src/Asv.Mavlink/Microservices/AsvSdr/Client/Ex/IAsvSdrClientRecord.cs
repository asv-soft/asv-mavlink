using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using DynamicData;

namespace Asv.Mavlink;

public interface IAsvSdrClientRecord
{
    Guid Id { get; }
    IRxValue<AsvSdrCustomMode> DataType { get; }
    IRxValue<string> Name { get; }
    IRxValue<ulong> Frequency { get; }
    IRxValue<DateTime> Created { get; }
    IRxValue<ushort> TagsCount { get; }
    IRxValue<uint> DataCount { get; }
    IRxValue<uint> ByteSize { get; }
    IRxValue<TimeSpan> Duration { get; }
    IObservable<IChangeSet<AsvSdrClientRecordTag, TagId>> Tags { get; }
    Task<bool> DownloadTagList(IProgress<double> progress = null, CancellationToken cancel = default);
    Task DeleteTag(TagId id, CancellationToken cancel);
}

public static class AsvSdrClientRecordHelper
{
    public static void UpdateMetadataFrom(this IListDataFile<AsvSdrRecordFileMetadata> self, IAsvSdrClientRecord src)
    {
        self.EditMetadata(src.CopyTo);
    }
    
    public static void CopyMetadataTo(this IAsvSdrClientRecord self, IListDataFile<AsvSdrRecordFileMetadata> dest)
    {
        dest.EditMetadata(self.CopyTo);
    }
    
    
    public static void CopyTo(this IAsvSdrClientRecord self, AsvSdrRecordFileMetadata dest)
    {
        self.CopyTo(dest.Info);
        var tags = new List<AsvSdrClientRecordTag>();
        self.Tags.Clone(tags);
        foreach (var tag in tags)
        {
            var tagPayload = new AsvSdrRecordTagPayload();
            tag.CopyTo(tagPayload);
            dest.Tags.Add(tagPayload);
        }
    }
    
    public static void CopyTo(this AsvSdrClientRecordTag self, AsvSdrRecordTagPayload dest)
    {
        self.Id.RecordGuid.ToByteArray().CopyTo(dest.RecordGuid,0);
        self.Id.TagGuid.ToByteArray().CopyTo(dest.TagGuid,0);
        self.RawValue.CopyTo(dest.TagValue,0);
        self.Name.CopyTo(dest.TagName);
        dest.TagType = self.Type;
    }
    public static void CopyTo(this IAsvSdrClientRecord self, AsvSdrRecordPayload dest)
    {
        MavlinkTypesHelper.SetGuid(dest.RecordGuid, self.Id);
        MavlinkTypesHelper.SetString(dest.RecordName,self.Name.Value);
        dest.Frequency = self.Frequency.Value;
        dest.CreatedUnixUs = MavlinkTypesHelper.ToUnixTimeUs(self.Created.Value);
        dest.DataType = self.DataType.Value;
        dest.DurationSec = (uint)self.Duration.Value.TotalSeconds;
        dest.DataCount = self.DataCount.Value;
        dest.Size = self.ByteSize.Value;
        dest.TagCount = self.TagsCount.Value;
        
    }
}


