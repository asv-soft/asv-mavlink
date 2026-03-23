using System;
using System.IO;
using System.IO.Packaging;
using System.Threading;
using Asv.IO;
using Asv.Mavlink.PackageFile.Parts;
using Asv.XUnit;
using DeepEqual.Syntax;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(MavlinkMessageChimpAsvPackagePart))]
public class MavlinkMessageChimpAsvPackagePartTest(ITestOutputHelper log)
{
    private void TestMessageData(uint flushEvery, MavlinkMessageRecord[] data)
    {
        var uriPart = new Uri("/messages/", UriKind.Relative);
        var contentType = "application/bin+chimp";
        
        var ms = new MemoryStream();
        var pkg = Package.Open(ms, FileMode.Create, FileAccess.ReadWrite);
        var logger = new TestLogger(log, TimeProvider.System, "MavlinkMessageChimpAsvPackagePartTest");
        var ctx = new AsvPackageContext(new Lock(), pkg, logger);
        
        var part = new MavlinkMessageChimpAsvPackagePart(uriPart, contentType, flushEvery, ctx);
        foreach (var t in data)
        {
            part.Write(t);
        }
        
        
        
        part.Flush();
        part.Dispose();
        pkg.Close();
        ms.Position = 0;
        
        pkg = Package.Open(ms, FileMode.Open, FileAccess.Read);
        ctx = new AsvPackageContext(new Lock(), pkg, logger);
        part = new MavlinkMessageChimpAsvPackagePart(uriPart, contentType, flushEvery, ctx);

        var count = 0;
        part.Read(x =>
        {
            try
            {
                count++;
                Assert.Equal(data[x.Index].Index, x.Index);
                Assert.Equal(data[x.Index].Timestamp, x.Timestamp);
                var expected = data[x.Index].Data.GetPayload();
                var actual = x.Data.GetPayload();
                expected.ShouldDeepEqual(actual);
            }
            catch (Exception e)
            {
                throw;
            }
            
        });
        Assert.Equal(data.Length, count);
        
    }
    
    [Theory]
    [InlineData(1,300)]
    [InlineData(1_000,300)]
    public void TestMessages(int count, uint flushEvery)
    {
        foreach (var messageId in MavlinkV2MessageFactory.Instance.GetSupportedIds())
        {
            var data = new MavlinkMessageRecord[count];
            var start = DateTime.Now;
            string name = String.Empty;
            for (int i = 0; i < count; i++)
            {
                var packet = MavlinkV2MessageFactory.Instance.Create(messageId);
                packet.GetPayload().Randomize();
                name = packet.Name;
                var rec = new MavlinkMessageRecord((uint)i,start.AddSeconds(i) , packet);
                data[i] = rec;
            
            }
            TestMessageData(flushEvery, data);
            log.WriteLine(name);
        }
    }
}