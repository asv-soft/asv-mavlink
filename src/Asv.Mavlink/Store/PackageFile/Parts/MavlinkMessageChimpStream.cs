using System;
using System.IO.Packaging;
using Asv.IO;

namespace Asv.Mavlink.PackageFile.Parts;

public interface IMavlinkStreamIdSelector
{
    string ToString(MavlinkMessage msg);
    MavlinkMessage? FromString(string id);
}

public class MavlinkSimpleStreamIdConverter : IMavlinkStreamIdSelector
{
    public string ToString(MavlinkMessage msg)
    {
        return msg.GetIdAsString();
    }

    public MavlinkMessage? FromString(string id)
    {
        return MavlinkV2MessageFactory.Instance.Create(int.Parse(id));
    }
}

public class MavlinkMessageChimpAsvPackagePart(
    Uri uriPart,
    string contentType,
    uint flushEvery,
    AsvPackageContext context,
    IMavlinkStreamIdSelector? idSelector = null,
    CompressionOption compression = CompressionOption.Maximum,
    bool useZstdForBatch = true,
    AsvPackagePart? parent = null)
    : VisitableTimeSeriesAsvPackagePart(uriPart, contentType,flushEvery,context, compression, useZstdForBatch, parent) 
{
    private readonly IMavlinkStreamIdSelector _idSelector = idSelector ?? new MavlinkSimpleStreamIdConverter();
    
    public void Write(MavlinkMessageRecord msg)
    {
        var id = _idSelector.ToString(msg.Data);
        base.Write(new TableRow(msg.Index, msg.Timestamp, id, msg.Data.GetPayload()));
    }
    
    public void Read(Action<MavlinkMessageRecord> visitor)
    {
        Read(x=> visitor(new MavlinkMessageRecord(x.Item1.Index, x.Item1.Timestamp, (MavlinkMessage)x.Item2)), id=>
        {
            var msg = _idSelector.FromString(id);
            if (msg == null) return null;
            return (msg.GetPayload(), msg);
        });
    }
}

public record MavlinkMessageRecord(uint Index, DateTime Timestamp, MavlinkMessage Data);