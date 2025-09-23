using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using Asv.IO;

namespace Asv.Mavlink.PackageFile.Parts;

public class MavParamsKvJsonAsvPackagePart(
    Uri uriPart,
    string contentType,
    CompressionOption compression,
    AsvPackageContext context) 
    : KvJsonAsvPackagePart(uriPart, contentType, compression, context)
{
    public void Write(params IEnumerable<MavlinkParamRecord> mavParams)
    {
        Save(mavParams.Select(x=>new KeyValuePair<string, string>(x.Name, x.Value.ToString())));
    }

    public void Read(Action<MavlinkParamRecord> visitor)
    {
        Load(x=>visitor(new MavlinkParamRecord(x.Key, MavParamValue.Parse(x.Value))));
    }
}

public record MavlinkParamRecord(string Name, MavParamValue Value);
