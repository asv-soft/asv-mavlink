using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Xunit;

namespace Asv.Mavlink.Test;

public class DebugTest
{
    [Fact]
    public async Task TestDebug()
    {
        var link = new VirtualMavlinkConnection();
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);
        
        
        var client = new DebugClient(link.Client, clientId, new PacketSequenceCalculator());
        var server = new DebugServer(link.Server, new PacketSequenceCalculator(), serverId);

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