using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Moq;
using R3;
using TimeProviderExtensions;
using Xunit;

namespace Asv.Mavlink.Test;

public class MavlinkDeviceExplorerTest
{
    [Fact]
    public void Factory_EveryGeneratedPacket_UsesCanonicalSourceAddress()
    {
        using var fixture = new Fixture();
        var ids = fixture.Messages.GetSupportedIds().ToArray();
        Assert.NotEmpty(ids);
        foreach (var id in ids)
        {
            var packet = Assert.IsAssignableFrom<MavlinkMessage>(fixture.Messages.Create(id));
            packet.SystemId = 1;
            packet.ComponentId = 1;
            Assert.True(fixture.Factory.TryGetSourceAddress(packet, out var address), packet.Name);
            Assert.Equal(new DeviceAddress("mavlink", "1:1"), address);
        }
    }

    [Theory]
    [InlineData(1, 2, "1:2")]
    [InlineData(2, 1, "2:1")]
    [InlineData(0, 0, "0:0")]
    [InlineData(255, 255, "255:255")]
    public void Factory_SystemAndComponent_DefineDistinctAddresses(byte systemId, byte componentId, string expected)
    {
        using var fixture = new Fixture();
        Assert.True(fixture.Factory.TryGetSourceAddress(new StatustextPacket
        {
            SystemId = systemId,
            ComponentId = componentId,
        }, out var address));
        Assert.Equal(new DeviceAddress("mavlink", expected), address);
        Assert.NotEqual(new DeviceAddress("mavlink", "1:1"), address);
    }

    [Fact]
    public void Factory_V1AndV2Headers_UseSameInvariantSourceAddress()
    {
        using var fixture = new Fixture();
        var previousCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ar-SA");
            Assert.True(fixture.Factory.TryGetSourceAddress(new HeaderOnlyV1Message
            {
                SystemId = 123,
                ComponentId = 245,
            }, out var v1));
            Assert.True(fixture.Factory.TryGetSourceAddress(new StatustextPacket
            {
                SystemId = 123,
                ComponentId = 245,
            }, out var v2));
            Assert.Equal(new DeviceAddress("mavlink", "123:245"), v1);
            Assert.Equal(v1, v2);
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
        }
    }

    [Fact]
    public void Factory_DeviceAddress_UsesRemoteTargetRegardlessOfSelfOrClass()
    {
        using var fixture = new Fixture();
        var first = new MavlinkClientDeviceId("GENERIC", new MavlinkClientIdentity(255, 190, 1, 1));
        var second = new MavlinkClientDeviceId("OTHER", new MavlinkClientIdentity(22, 44, 1, 1));
        var otherFactory = new GenericClientDeviceFactory(new MavlinkIdentity(22, 44),
            new PacketSequenceCalculator(), new GenericDeviceConfig(), fixture.Messages);
        Assert.True(fixture.Factory.TryGetDeviceAddress(first, out var firstAddress));
        Assert.True(otherFactory.TryGetDeviceAddress(second, out var secondAddress));
        Assert.Equal(new DeviceAddress("mavlink", "1:1"), firstAddress);
        Assert.Equal(firstAddress, secondAddress);
        Assert.True(otherFactory.TryGetSourceAddress(Heartbeat(1, 1), out var source));
        Assert.Equal(firstAddress, source);
    }

    [Fact]
    public void Factory_UnsupportedInputs_ReturnFalseAndDefault()
    {
        using var fixture = new Fixture();
        Assert.False(fixture.Factory.TryGetSourceAddress(Mock.Of<IProtocolMessage>(), out var source));
        Assert.Equal(default, source);
        Assert.False(fixture.Factory.TryGetDeviceAddress(new ForeignDeviceId(), out var device));
        Assert.Equal(default, device);
        var unsupported = Mock.Of<IProtocolMessage>();
        Assert.False(fixture.Explorer.TryGetSourceAddress(unsupported, out source));
        Assert.Equal(default, source);
        Assert.False(fixture.Explorer.TryGetDeviceAddress(new ForeignDeviceId(), out device));
        Assert.Equal(default, device);
        Assert.False(fixture.Explorer.TryIdentify(unsupported, out var identified));
        Assert.Null(identified);
    }

    [Fact]
    public void Explorer_AddressFacades_WorkBeforeDiscoveryAndAfterDisposal()
    {
        using var fixture = new Fixture();
        var packet = new CommandLongPacket { SystemId = 1, ComponentId = 1 };
        packet.Payload.TargetSystem = 50;
        packet.Payload.TargetComponent = 60;
        packet.Tags.Add("port", "first");
        var deviceId = new MavlinkClientDeviceId("GENERIC", new MavlinkClientIdentity(255, 190, 1, 1));
        Assert.True(fixture.Explorer.TryGetSourceAddress(packet, out var source));
        Assert.True(fixture.Explorer.TryGetDeviceAddress(deviceId, out var device));
        Assert.Equal(new DeviceAddress("mavlink", "1:1"), source);
        Assert.Equal(source, device);
        Assert.False(fixture.Explorer.TryIdentify(packet, out var unknown));
        Assert.Null(unknown);
        Assert.Empty(fixture.Explorer.Devices);

        fixture.Explorer.Dispose();
        packet.Tags["port"] = "second";
        Assert.True(fixture.Explorer.TryGetSourceAddress(packet, out source));
        Assert.True(fixture.Explorer.TryGetDeviceAddress(deviceId, out device));
        Assert.Equal(new DeviceAddress("mavlink", "1:1"), source);
        Assert.Equal(source, device);
        Assert.False(fixture.Explorer.TryIdentify(packet, out unknown));
        Assert.Null(unknown);
    }

    [Fact]
    public async Task Explorer_NonHeartbeat_DoesNotDiscoverDevice()
    {
        using var fixture = new Fixture();
        await fixture.First.Server.Send(new StatustextPacket { SystemId = 1, ComponentId = 1 },
            Xunit.TestContext.Current.CancellationToken);
        Assert.Empty(fixture.Explorer.Devices);
        Assert.False(fixture.Explorer.TryIdentify(new StatustextPacket { SystemId = 1, ComponentId = 1 }, out var id));
        Assert.Null(id);
    }

    [Fact]
    public async Task Explorer_EveryGeneratedPacket_ResolvesDiscoveredSenderOnly()
    {
        using var fixture = new Fixture();
        await fixture.First.Server.Send(Heartbeat(1, 1), Xunit.TestContext.Current.CancellationToken);
        var discovered = Assert.Single(fixture.Explorer.Devices).Key;
        var ids = fixture.Messages.GetSupportedIds().ToArray();
        Assert.NotEmpty(ids);
        foreach (var id in ids)
        {
            var packet = Assert.IsAssignableFrom<MavlinkMessage>(fixture.Messages.Create(id));
            packet.SystemId = 1;
            packet.ComponentId = 1;
            Assert.True(fixture.Explorer.TryIdentify(packet, out var actual), packet.Name);
            Assert.Same(discovered, actual);
            packet.SystemId = 2;
            Assert.False(fixture.Explorer.TryIdentify(packet, out actual), packet.Name);
            Assert.Null(actual);
            packet.SystemId = 1;
            packet.ComponentId = 2;
            Assert.False(fixture.Explorer.TryIdentify(packet, out actual), packet.Name);
            Assert.Null(actual);
        }
        Assert.Single(fixture.Explorer.Devices);
    }

    [Fact]
    public async Task Explorer_DifferentSystemsAndComponents_CreateSeparateDevices()
    {
        using var fixture = new Fixture();
        foreach (var heartbeat in new[] { Heartbeat(1, 1), Heartbeat(1, 2), Heartbeat(2, 1) })
        {
            await fixture.First.Server.Send(heartbeat, Xunit.TestContext.Current.CancellationToken);
        }
        Assert.Equal(3, fixture.Explorer.Devices.Count);
        foreach (var pair in fixture.Explorer.Devices)
        {
            var id = Assert.IsType<MavlinkClientDeviceId>(pair.Key);
            var packet = new StatustextPacket
            {
                SystemId = id.Id.Target.SystemId,
                ComponentId = id.Id.Target.ComponentId,
            };
            Assert.True(fixture.Explorer.TryIdentify(packet, out var actual));
            Assert.Same(pair.Key, actual);
        }
    }

    [Fact]
    public async Task Explorer_TargetedPacket_ResolvesSourceRatherThanDestination()
    {
        using var fixture = new Fixture();
        await fixture.First.Server.Send(Heartbeat(1, 1), Xunit.TestContext.Current.CancellationToken);
        await fixture.First.Server.Send(Heartbeat(2, 2), Xunit.TestContext.Current.CancellationToken);
        var packet = new CommandLongPacket { SystemId = 1, ComponentId = 1 };
        packet.Payload.TargetSystem = 2;
        packet.Payload.TargetComponent = 2;
        Assert.True(fixture.Explorer.TryIdentify(packet, out var actual));
        var id = Assert.IsType<MavlinkClientDeviceId>(actual);
        Assert.Equal(new MavlinkIdentity(1, 1), id.Id.Target);
    }

    [Fact]
    public async Task Explorer_SameSenderAcrossTwoVirtualEndpoints_IsOneLiveDevice()
    {
        using var fixture = new Fixture();
        await fixture.First.Server.Send(Heartbeat(1, 1), Xunit.TestContext.Current.CancellationToken);
        var discovered = Assert.Single(fixture.Explorer.Devices);
        await fixture.Second.Server.Send(Heartbeat(1, 1), Xunit.TestContext.Current.CancellationToken);
        Assert.Same(discovered.Value, Assert.Single(fixture.Explorer.Devices).Value);
        foreach (var link in new[] { fixture.First, fixture.Second })
        {
            IProtocolMessage? received = null;
            using var subscription = link.Client.OnRxMessage.Subscribe(message => received = message);
            await link.Server.Send(new StatustextPacket { SystemId = 1, ComponentId = 1 },
                Xunit.TestContext.Current.CancellationToken);
            Assert.NotNull(received);
            Assert.Same(link.Client, received.Tags["endpoint"]);
            Assert.True(fixture.Explorer.TryGetSourceAddress(received, out var address));
            Assert.Equal(new DeviceAddress("mavlink", "1:1"), address);
            Assert.True(fixture.Explorer.TryIdentify(received, out var actual));
            Assert.Same(discovered.Key, actual);
        }
    }

    [Fact]
    public async Task Explorer_TimeoutAndReacquisition_ResolveOnlyNewRegistration()
    {
        using var fixture = new Fixture();
        await fixture.First.Server.Send(Heartbeat(1, 1), Xunit.TestContext.Current.CancellationToken);
        var original = Assert.Single(fixture.Explorer.Devices);
        var packet = new StatustextPacket { SystemId = 1, ComponentId = 1 };
        fixture.Time.Advance(TimeSpan.FromMilliseconds(60));
        Assert.True(fixture.Explorer.TryIdentify(packet, out var beforeTimeout));
        Assert.Same(original.Key, beforeTimeout);
        fixture.Time.Advance(TimeSpan.FromMilliseconds(50));
        Assert.Empty(fixture.Explorer.Devices);
        Assert.False(fixture.Explorer.TryIdentify(packet, out var removed));
        Assert.Null(removed);

        await fixture.Second.Server.Send(Heartbeat(1, 1), Xunit.TestContext.Current.CancellationToken);
        var replacement = Assert.Single(fixture.Explorer.Devices);
        Assert.NotSame(original.Value, replacement.Value);
        Assert.NotSame(original.Key, replacement.Key);
        Assert.True(fixture.Explorer.TryIdentify(packet, out var reacquired));
        Assert.Same(replacement.Key, reacquired);
    }

    private static HeartbeatPacket Heartbeat(byte systemId, byte componentId) => new()
    {
        SystemId = systemId,
        ComponentId = componentId,
    };

    private sealed class Fixture : IDisposable
    {
        private readonly EndpointConnection _connection;
        public ManualTimeProvider Time { get; } = new();
        public IProtocolMessageFactory<MavlinkMessage, int> Messages { get; } = MavlinkV2Protocol.CreateMessageFactory();
        public GenericClientDeviceFactory Factory { get; }
        public IVirtualConnection First { get; }
        public IVirtualConnection Second { get; }
        public IDeviceExplorer Explorer { get; }

        public Fixture()
        {
            var protocol = Protocol.Create(builder =>
            {
                builder.SetTimeProvider(Time);
                builder.RegisterMavlinkV2Protocol(Messages);
            });
            First = protocol.CreateVirtualConnection(printMessagesToLog: false);
            Second = protocol.CreateVirtualConnection(printMessagesToLog: false);
            _connection = new EndpointConnection((IProtocolContext)protocol, First.Client, Second.Client);
            Factory = new GenericClientDeviceFactory(new MavlinkIdentity(255, 190),
                new PacketSequenceCalculator(), new GenericDeviceConfig(), Messages);
            Explorer = DeviceExplorer.Create(_connection, builder =>
            {
                builder.SetTimeProvider(Time);
                builder.SetConfig(new ClientDeviceBrowserConfig
                {
                    DeviceTimeoutMs = 100,
                    DeviceCheckIntervalMs = 10,
                });
                builder.Factories.Register(Factory);
            });
        }

        public void Dispose()
        {
            Explorer.Dispose();
            _connection.Dispose();
            First.Dispose();
            Second.Dispose();
        }
    }

    // Merge real virtual endpoints after parsing, preserving each receiver as transport metadata.
    private sealed class EndpointConnection : ProtocolConnection
    {
        private readonly IProtocolConnection[] _endpoints;
        private readonly IDisposable[] _subscriptions;

        public EndpointConnection(IProtocolContext context, params IProtocolConnection[] endpoints)
            : base("two-endpoints", context)
        {
            _endpoints = endpoints;
            _subscriptions = endpoints.Select(endpoint => endpoint.OnRxMessage.Subscribe(message =>
            {
                message.Tags.Add("endpoint", endpoint);
                InternalPublishRxMessage(message);
            })).ToArray();
        }

        public override async ValueTask Send(IProtocolMessage message, CancellationToken cancel = default)
        {
            foreach (var endpoint in _endpoints)
            {
                await endpoint.Send(message, cancel);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var subscription in _subscriptions)
                {
                    subscription.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }

    private sealed class ForeignDeviceId() : DeviceId("foreign")
    {
        public override string AsString() => "foreign";
    }

    // V1 wire generation is currently a stub; only source-header extraction is under test.
    private sealed class HeaderOnlyV1Message : MavlinkV1Message
    {
        public override ProtocolInfo Protocol => MavlinkV1Protocol.Info;
        public override string Name => "V1_HEADER_ONLY";
        public override int Id => 0;
        public override byte GetCrcExtra() => throw new NotSupportedException();
        public override IPayload GetPayload() => throw new NotSupportedException();
        public override int GetByteSize() => throw new NotSupportedException();
        public override void Deserialize(ref ReadOnlySpan<byte> buffer) => throw new NotSupportedException();
        public override void Serialize(ref Span<byte> buffer) => throw new NotSupportedException();
    }
}
