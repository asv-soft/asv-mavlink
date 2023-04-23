using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvSdr;
using Xunit;

namespace Asv.Mavlink.Test;

public class DebugTest
{
    [Fact]
    public async Task TestDebug()
    {
        var link = new VirtualLink();
        var client = new DebugClient(link.Client, new MavlinkClientIdentity(), new PacketSequenceCalculator(), Scheduler.Default);
        var server = new DebugServer(link.Server, new PacketSequenceCalculator(), new MavlinkServerIdentity(), Scheduler.Default);

        var completed = false;
        client.DebugFloatArray.Subscribe(_=>
        {
            var a = new[] { 1.0f, 2.0f, 3.0f };
            for (var i = 0; i < 3; i++)
            {
                Assert.Equal(a[i], _.Data[i]);
            }
            Assert.Equal("test",MavlinkTypesHelper.GetString(_.Name));
            Assert.Equal(1,(int)_.ArrayId);
            completed = true;
        });
        await server.SendDebugFloatArray("test", 1, new[] { 1.0f, 2.0f, 3.0f });
        await Task.Delay(100);
        Assert.True(completed);
    }
}