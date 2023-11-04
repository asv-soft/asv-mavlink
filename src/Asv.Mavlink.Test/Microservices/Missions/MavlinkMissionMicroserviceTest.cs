using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Xunit;

namespace Asv.Mavlink.Test;

public class MavlinkMissionMicroserviceTest
{
    private void CreateClientServer(VirtualMavlinkConnection link, out IMissionServer server, out IMissionClient client)
    {
        var serverSeq = new PacketSequenceCalculator();
        var serverId = new MavlinkServerIdentity{ComponentId = 13, SystemId = 13};
        server = new MissionServer(link.Server, serverId, serverSeq, TaskPoolScheduler.Default);
        
        
        var clientSeq = new PacketSequenceCalculator();
        var clientId = new MavlinkClientIdentity{SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13};
        client = new MissionClient(
            link.Client, 
            clientId, 
            clientSeq, new MissionClientConfig
            {
                AttemptToCallCount = 5,
                CommandTimeoutMs = 300
            });
    }
    
    
    [Fact]
    public async Task Check_Client_MissionSetCurrent()
    {
        var link = new VirtualMavlinkConnection();
        CreateClientServer(link, out var server, out var client);
        server.OnMissionSetCurrent.Subscribe(_=>server.SendMissionCurrent(_.Payload.Seq));
        var waiter = new TaskCompletionSource<MissionSetCurrentPacket>();
        using var subscribe = server.OnMissionSetCurrent.Subscribe(x =>
        {
            waiter.SetResult(x);
        });
        await client.MissionSetCurrent(1234, CancellationToken.None);
        await waiter.Task;
        Assert.Equal(1234, waiter.Task.Result.Payload.Seq);
    }
    
}