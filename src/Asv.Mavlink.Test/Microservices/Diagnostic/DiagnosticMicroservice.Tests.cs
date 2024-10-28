using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Mavlink.Diagnostic.Client;
using Asv.Mavlink.Diagnostic.Server;
using DynamicData;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Diagnostic;

public class DiagnosticMicroserviceTests
{
    private readonly ITestOutputHelper _output;
    private readonly FakeTimeProvider _fakeTime;

    public DiagnosticMicroserviceTests(ITestOutputHelper output)
    {
        _fakeTime = new FakeTimeProvider();
        _output = output;
    }
    
    private void SetUpMicroservice(out IDiagnosticClient client, out IDiagnosticServer server,
        Func<IPacketV2<IPayload>, bool> clientToServer, Func<IPacketV2<IPayload>, bool> serverToClient)
    {
        var link = new VirtualMavlinkConnection(clientToServer, serverToClient);
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);

        var clientSeq = new PacketSequenceCalculator();
        client = new DiagnosticClient(new DiagnosticClientConfig(), link.Client, clientId, clientSeq, TimeProvider.System,
            TaskPoolScheduler.Default, new TestLoggerFactory(_output, "CLIENT"));

        var serverSeq = new PacketSequenceCalculator();
        server = new DiagnosticServer(new DiagnosticServerConfig(), link.Server, serverId, serverSeq,
            _fakeTime, Scheduler.Default,new TestLoggerFactory(_output, "SERVER")); //TODO: change to FakeTimeProvider
    }

    [Fact]
    public async Task ServerSend_IntValue_ClientSuccessfullyRetrievesMessage()
    {
        // Arrange
        var name = "testInt";
        var value = 10;
        SetUpMicroservice(out var client, out var server, packet => false, packet =>  true);
        
        // Act
        await server.Send(name, value);

        // Assert
        client.IntProbes.Bind(out var values).Subscribe();
        Assert.Single(values);
        Assert.Equal(name, values[0].Name);
        Assert.Equal(value, values[0].Value.Value.Item2);
    }
    
    [Fact]
    public async Task ServerSend_FloatValue_ClientSuccessfullyRetrievesMessage()
    {
        // Arrange
        var name = "testFloat";
        var value = 10.0f;
        SetUpMicroservice(out var client, out var server, packet => false, packet =>  true);
        
        // Act
        await server.Send(name, value);

        // Assert
        client.FloatProbes.Bind(out var values).Subscribe();
        Assert.Single(values);
        Assert.Equal(name, values[0].Name);
        Assert.Equal(value, values[0].Value.Value.Item2, 5);
    }
}