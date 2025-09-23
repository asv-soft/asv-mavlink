using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using Asv.IO;

namespace Asv.Mavlink.PackageFile.Parts;

public class MavParamsChangeKvJsonAsvPackagePart(
    Uri uriPart,
    string contentType,
    CompressionOption compression,
    AsvPackageContext context) 
    : KvChangesJsonAsvPackagePart(uriPart, contentType, compression, context)
{
    public void Append(MavlinkParamChangeRecord change)
    {
        Append(new KeyValueChange<string, string>(change.Timestamp, change.Name, change.OldValue.ToString(), change.NewValue.ToString()));
    }

    public void Read(Action<MavlinkParamChangeRecord> visitor)
    {
        Load((in KeyValueChange<string, string> x) => visitor(new MavlinkParamChangeRecord(x.Timestamp, x.Key, MavParamValue.Parse(x.OldValue), MavParamValue.Parse(x.NewValue))));
    }
}

public record MavlinkParamChangeRecord(DateTime Timestamp, string Name, MavParamValue OldValue, MavParamValue NewValue);