using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.XUnit;
using JetBrains.Annotations;
using R3;
using Xunit;

namespace Asv.Mavlink.Test.Features;

[TestSubject(typeof(MavlinkIpBasedIdMapperFeature))]
public class MavlinkIpBasedIdMapperFeatureTest(ITestOutputHelper log)
{

    [Fact]
    public async Task MavlinkIpBasedIdMapperFeature_ReverseProxy_RxTest()
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(
            Xunit.TestContext.Current.CancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(10));

        var loggerFactory = new TestLoggerFactory(log, TimeProvider.System, "TEST");
        var messageFactory = MavlinkV2Protocol.CreateMessageFactory();
        var protocol = Protocol.Create(builder =>
        {
            builder.SetLog(loggerFactory);
            builder.RegisterMavlinkV2Protocol(messageFactory);
            builder.Formatters.RegisterSimpleFormatter();
            builder.Features.RegisterBroadcastAllFeature();
            builder.Features.RegisterMavlinkIpBasedIdMapperFeature((ip, originId) =>
                new MavlinkIdentity((ushort)ip.Port));
        });
        
        var proxyRouter = protocol.CreateRouter("ReverseProxyRouter");
        var inPort = proxyRouter.AddPort("udp://127.0.0.1:14540?reconnect=0");
        var drone1Router = protocol.CreateRouter("Drone1Router");
        var drone1Port = drone1Router.AddPort("udp://127.0.0.1:14541?remote=127.0.0.1:14540&reconnect=0");
        var drone2Router = protocol.CreateRouter("Drone2Router");
        var drone2Port = drone2Router.AddPort("udp://127.0.0.1:14542?remote=127.0.0.1:14540&reconnect=0");

        await drone1Port.Status.FirstAsync(x => x == ProtocolPortStatus.Connected, timeout.Token);
        await drone2Port.Status.FirstAsync(x => x == ProtocolPortStatus.Connected, timeout.Token);

        await Task.Delay(1000, timeout.Token);
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        
        // We expect to receive two packets with different FullId values
        var idPackets = new HashSet<ushort>([14541,14542]);
        
        using var sub1 = proxyRouter.OnRxMessage.FilterByType<HeartbeatPacket>().Subscribe(x =>
        {
            idPackets.Remove(x.FullId.FullId);
            if (idPackets.Count == 0)
            {
                tcs.TrySetResult();
            }
        });

        await drone1Port.Send(new HeartbeatPacket
        {
            SystemId = 1,
            ComponentId = 1,
        }, timeout.Token);
        await drone2Port.Send(new HeartbeatPacket
        {
            SystemId = 1,
            ComponentId = 1,
        }, timeout.Token);
        
        await tcs.Task.WaitAsync(timeout.Token);
        
        
        // ok rx working!!! Now try to send a packet back to the drones
        var droneIdConvertedFullId = new MavlinkIdentity(14541);
        var tcs2 = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var sub2 = drone1Router.OnRxMessage.FilterByType<CommandIntPacket>().Subscribe(x =>
        {
            if (x.Payload is { TargetSystem: 1, TargetComponent: 1 })
            {
                tcs2.TrySetResult();
            }
        });
        using var sub3 = drone2Router.OnRxMessage.FilterByType<CommandIntPacket>().Subscribe(x =>
        {
            tcs2.TrySetException(new Exception("Drone 2 should not receive this packet!"));
        });
        
        await proxyRouter.Send(new CommandIntPacket
        {
            SystemId = 254,
            ComponentId = 254,
            Payload =
            {
                TargetSystem = droneIdConvertedFullId.SystemId,
                TargetComponent = droneIdConvertedFullId.ComponentId,
            }
        }, timeout.Token);


        await tcs2.Task.WaitAsync(timeout.Token);
    }
    
}
