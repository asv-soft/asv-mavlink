using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Threading;
using Asv.IO;
using Asv.Mavlink.PackageFile.Parts;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(MavParamsKvJsonAsvPackagePart))]
public class MavParamsKvJsonAsvPackagePartTest(ITestOutputHelper log)
{
    private static readonly Uri PartUri = new Uri("/meta/kvs.json", UriKind.Relative);
    private const string ContentType = "application/json";


    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(10000)]
    [InlineData(100000)]
    [InlineData(250000)]
    public void SaveLoad_Roundtrip_Works(int count)
    {
        var ms = new MemoryStream();
        var pkg = Package.Open(ms, FileMode.Create, FileAccess.ReadWrite);
        var logger = new TestLogger(log, TimeProvider.System, "AsvFilePartTest");
        var ctx = new AsvPackageContext(new Lock(), pkg, logger);

        var part = new MavParamsKvJsonAsvPackagePart(PartUri, ContentType, CompressionOption.Maximum, ctx);
        var paramArray = new MavlinkParamRecord[count];
        for (var i = 0; i < count; i++)
        {
            switch (i % 6)
            {
                case 0:
                    paramArray[i] = new MavlinkParamRecord($"PARAM_{i:0000}",
                        new MavParamValue((byte)Random.Shared.Next(byte.MinValue, byte.MaxValue)));
                    break;
                case 1:
                    paramArray[i] = new MavlinkParamRecord($"PARAM_{i:0000}",
                        new MavParamValue((short)Random.Shared.Next(short.MinValue, short.MaxValue)));
                    break;
                case 2:
                    paramArray[i] = new MavlinkParamRecord($"PARAM_{i:0000}",
                        new MavParamValue((int)Random.Shared.Next(int.MinValue, int.MaxValue)));
                    break;
                case 3:
                    paramArray[i] = new MavlinkParamRecord($"PARAM_{i:0000}",
                        new MavParamValue((float)Random.Shared.NextSingle() * 1000));
                    break;
                case 4:
                    paramArray[i] = new MavlinkParamRecord($"PARAM_{i:0000}",
                        new MavParamValue((sbyte)Random.Shared.Next(sbyte.MinValue, sbyte.MaxValue)));
                    break;
                case 5:
                    paramArray[i] = new MavlinkParamRecord($"PARAM_{i:0000}",
                        new MavParamValue((uint)Random.Shared.NextInt64(uint.MinValue, uint.MaxValue)));
                    break;
            }
        }

        part.Write(paramArray);
        part.Dispose();
        pkg.Close();
        ms.Position = 0;

        pkg = Package.Open(ms, FileMode.Open, FileAccess.Read);
        ctx = new AsvPackageContext(new Lock(), pkg, logger);
        part = new MavParamsKvJsonAsvPackagePart(PartUri, ContentType, CompressionOption.Maximum, ctx);
        var readArray = new List<MavlinkParamRecord>();
        part.Read(x => readArray.Add(x));

        Assert.Equal(count, readArray.Count);
        for (var i = 0; i < count; i++)
        {
            Assert.Equal(paramArray[i].Name, readArray[i].Name);
            Assert.Equal(paramArray[i].Value, readArray[i].Value);

        }

    }
}