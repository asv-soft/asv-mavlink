using System;
using System.Threading.Tasks;
using Asv.Mavlink.Diagnostic.Client;
using Asv.Mavlink.Diagnostic.Server;
using JetBrains.Annotations;
using ObservableCollections;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(DiagnosticClient))]
[TestSubject(typeof(DiagnosticServer))]
public class DiagnosticComplexTest(ITestOutputHelper log)
    : ComplexTestBase<DiagnosticClient, DiagnosticServer>(log)
{
    private readonly DiagnosticServerConfig _serverConfig = new()
    {
        MaxSendIntervalMs = 200,
        IsEnabled = true
    };

    private readonly DiagnosticClientConfig _clientConfig = new()
    {
        DeleteProbesTimeoutMs = 30_000,
        CheckProbesDelayMs = 1000,
        MaxCollectionSize = 100
    };

    protected override DiagnosticServer CreateServer(MavlinkIdentity identity, ICoreServices core)
    {
        return new DiagnosticServer(identity, _serverConfig, core);
    }

    protected override DiagnosticClient CreateClient(MavlinkClientIdentity identity, ICoreServices core)
    {
        return new DiagnosticClient(identity, _clientConfig, core);
    }
   
    [Fact]
    public async Task ServerSend_IntProbes_ClientRetrievesMessageSuccess()
    {
        // Arrange
        var name = "testInt";
        var value = 10;
        // Act

        var t = new TaskCompletionSource();
        using var c1 = Client.IntProbes.ObserveAdd()
            .Subscribe(t,(x,tcs) => tcs.TrySetResult());
        await Server.Send(name, value);
        await t.Task;
        
        // Assert
        Assert.Empty(Client.FloatProbes);
        Assert.Single(Client.IntProbes);
        Assert.Equal(name, Client.IntProbes[name].Name);
        Assert.Equal(value, Client.IntProbes[name].Value.CurrentValue.Item2);
        
        ClientTime.Advance(TimeSpan.FromMilliseconds(_clientConfig.DeleteProbesTimeoutMs + _clientConfig.CheckProbesDelayMs + 1 ));
        
        Assert.Empty(Client.FloatProbes);
        Assert.Empty(Client.IntProbes);
        
    }
    
    [Fact]
    public async Task ServerSend_FloatValue_ClientRetrievesMessageSuccess()
    {
        // Arrange
        var name = "testFloat";
        var value = 10.0f;
        
        var t = new TaskCompletionSource();
        using var c1 = Client.FloatProbes.ObserveAdd()
            .Subscribe(t,(x,tcs) => tcs.TrySetResult());
        
        // Act
        await Server.Send(name, value);
        await t.Task;
        
        // Assert
        Assert.Empty(Client.IntProbes);
        Assert.Single(Client.FloatProbes);
        Assert.Equal(name, Client.FloatProbes[name].Name);
        Assert.Equal(value, Client.FloatProbes[name].Value.CurrentValue.Item2, 5);
        
        ClientTime.Advance(TimeSpan.FromMilliseconds(_clientConfig.DeleteProbesTimeoutMs + _clientConfig.CheckProbesDelayMs + 1 ));
        
        Assert.Empty(Client.IntProbes);
        Assert.Empty(Client.FloatProbes);
    }

    [Fact]
    public async Task ServerSend_IntProbes_ClientReduceMaxCollectionSizeSuccess()
    {
        
        var all = _clientConfig.MaxCollectionSize * 2;
        var counter = all;
        var t = new TaskCompletionSource();
        using var c1 = Client.OnIntProbe
            .Subscribe(t,(x,tcs) =>
            {
                --counter;
                if (counter == 0)
                    tcs.TrySetResult();
            });
        
        for (var i = 0; i < all; i++)
        {
            await Server.Send($"name{i}", i);
        }
        await t.Task;
        
        Assert.Equal(_clientConfig.MaxCollectionSize, Client.IntProbes.Count);
        
    }
    
    [Fact]
    public async Task ServerSend_FloatProbes_ClientReduceMaxCollectionSizeSuccess()
    {
        
        var all = _clientConfig.MaxCollectionSize * 2;
        var counter = all;
        var t = new TaskCompletionSource();
        using var c1 = Client.OnFloatProbe
            .Subscribe(t,(x,tcs) =>
            {
                --counter;
                if (counter == 0)
                    tcs.TrySetResult();
            });
        
        for (var i = 0; i < all; i++)
        {
            await Server.Send($"name{i}", (float)i);
        }
        await t.Task;
        
        Assert.Equal(_clientConfig.MaxCollectionSize, Client.FloatProbes.Count);
        
    }
    
}