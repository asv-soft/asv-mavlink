using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvSdr;
using Xunit;

namespace Asv.Mavlink.Test.AsvSdr;

public class AsvSdrTest
{
    [Fact]
    public async Task InterfaceTest()
    {
        var link = new VirtualLink();
        var client = new AsvSdrClient(link.Client, new MavlinkClientIdentity(), new PacketSequenceCalculator(), Scheduler.Default);
        var server = new AsvSdrServer(link.Server, new MavlinkServerIdentity(), new AsvSdrServerConfig(), new PacketSequenceCalculator(), Scheduler.Default);
        server.Set(_ =>
        {
            _.Size = 123;
            _.RecordCount = 456;
            _.SupportedModes = AsvSdrSupportModeFlag.AsvSdrSupportModeGp | AsvSdrSupportModeFlag.AsvSdrSupportModeLlz;
        });
        server.Start();
        var result = await client.Status.FirstAsync();
        Assert.Equal(123,(int)result.Size);
        Assert.Equal(456,(int)result.RecordCount);
        Assert.Equal(AsvSdrSupportModeFlag.AsvSdrSupportModeGp | AsvSdrSupportModeFlag.AsvSdrSupportModeLlz,result.SupportedModes);
        
        server.OnGetRecord.Subscribe(_=>
        {
            Assert.Equal(1, _.RecordIndex);
            server.SendRecord(val =>
            {
                val.Index = 1;
                val.Size = 12;
                val.Frequency = 1232;
                val.DataCount = 1234;
                val.DurationSec = 12;
                val.State = AsvSdrRecordStateFlag.AsvSdrRecordFlagStarted;
                val.RecordMode = AsvSdrCustomMode.AsvSdrCustomModeGp;
                val.TagCount = 12;
                val.CreatedUnixUs = 123321;
            }).Wait();
        });
        var res1 = await client.GetRecord(1,default);
        Assert.Equal(1,(int)res1.Index);
        Assert.Equal(12,(int)res1.Size);
        Assert.Equal(1232,(int)res1.Frequency);
        Assert.Equal(1234,(int)res1.DataCount);
        Assert.Equal(12,(int)res1.DurationSec);
        Assert.Equal(AsvSdrRecordStateFlag.AsvSdrRecordFlagStarted,res1.State);
        Assert.Equal(AsvSdrCustomMode.AsvSdrCustomModeGp,res1.RecordMode);
        Assert.Equal(12,(int)res1.TagCount);
        Assert.Equal(123321,(int)res1.CreatedUnixUs);
        
        
    }
}