using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvRsga;
using Xunit;

namespace Asv.Mavlink.Test.AsvRsga;

public class AsvRsgaTest
{
    private void Create(out IAsvRsgaClient client, out IAsvRsgaServer server)
    {
        var link = new VirtualMavlinkConnection();
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var clientSeq = new PacketSequenceCalculator();
        client = new AsvRsgaClient(link.Client,clientId,clientSeq);
        var serverSeq = new PacketSequenceCalculator();
        server = new AsvRsgaServer(link.Server,
            new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId), serverSeq);
    }

    [Fact]
    public async Task Client_request_compatibility_server_respond_it()
    {
        var origin = new HashSet<AsvRsgaCustomMode>(Enum.GetValues<AsvRsgaCustomMode>());
        Create(out var client, out var server);
        var tcs = new TaskCompletionSource<AsvRsgaCompatibilityResponsePayload>();
        var cancel = new CancellationTokenSource();
        cancel.CancelAfter(TimeSpan.FromSeconds(10));
        await using var c1 = cancel.Token.Register(() => tcs.TrySetCanceled(), false);

        server.OnCompatibilityRequest.Subscribe(x =>
        {
            server.SendCompatilityResponse(inner =>
            {
                inner.RequestId = x.RequestId;
                RsgaHelper.SetSupportedModes(inner, origin);
            }).Wait();
        });
        var result = await client.GetCompatibilities(cancel.Token);
        var resultMode = RsgaHelper.GetSupportedModes(result).ToHashSet();
        
        Assert.Equal(origin.Count, resultMode.Count);
        Assert.True(origin.All(x => resultMode.Contains(x)));
        
    }

    
}