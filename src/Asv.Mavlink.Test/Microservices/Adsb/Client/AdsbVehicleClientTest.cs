using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AdsbVehicleClient))]
public class AdsbVehicleClientTest : ClientTestBase<AdsbVehicleClient>
{
    private readonly AdsbVehicleClientConfig _config = new()
    {
        TargetTimeoutMs = 10_000,
        CheckOldDevicesMs = 3_000,
    };

    public AdsbVehicleClientTest(ITestOutputHelper output) : base(output)
    {
        _ = Client;
    }

    [Fact]
    public async Task OnTarget_GetPacketWithNewVehicle_Success()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Token.Register(() => tcs.TrySetCanceled());
        
        var packetToSend = new AdsbVehiclePacket()
        {
            Payload =
            {
                Flags = AdsbFlags.AdsbFlagsSourceUat,
                IcaoAddress = 10,
            }
        };
        AdsbVehiclePayload? receivedPayload = null;
        using var sub = Client.OnTarget.Subscribe(p => 
        {
            receivedPayload = p;
            tcs.TrySetResult();
        });
        using var sub1 = Link.Server.OnTxMessage.Subscribe(_ =>
        {
            Time.Advance(TimeSpan.FromMilliseconds(10));
        });


        // Act
        await Link.Server.Send(packetToSend, cancellationTokenSource.Token);

        // Assert
        await tcs.Task;
        Assert.NotNull(receivedPayload);
        Assert.Single(Client.Targets);
        Assert.Equal(packetToSend.Payload, receivedPayload);
        Assert.Equal(packetToSend.Payload.IcaoAddress, Client.Targets.First().Value.IcaoAddress);
        Assert.Equal(packetToSend.Payload.Flags, Client.Targets.First().Value.Flags.CurrentValue);
    }

    protected override AdsbVehicleClient CreateClient(MavlinkClientIdentity identity, CoreServices core) =>
        new(identity, _config, core);
}