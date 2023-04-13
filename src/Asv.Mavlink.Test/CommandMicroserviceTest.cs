using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Xunit;

namespace Asv.Mavlink.Test;

public class CommandMicroserviceTest
{
    private static CommandClient CreateCommandClient(VirtualLink link)
    {
        var client = new CommandClient(link.Client,
            new MavlinkClientIdentity { SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13 },
            new PacketSequenceCalculator(), new CommandProtocolConfig
            {
                CommandTimeoutMs = 500,
            }, TaskPoolScheduler.Default);
        return client;
    }

    private static CommandServer CreateCommandServer(VirtualLink link)
    {
        var server = new CommandServer(link.Server, new PacketSequenceCalculator(),
            new MavlinkServerIdentity { ComponentId = 13, SystemId = 13 }, TaskPoolScheduler.Default);
        return server;
    }
    
    [Fact]
    public async Task TestSimpleCallCommandInt()
    {
        var link = new VirtualLink();
        var server = CreateCommandServer(link);
        var client = CreateCommandClient(link);
        
        var called = false;
        server.OnCommandInt.Subscribe(_ =>
        {
            called = true;
            server.SendCommandAckAccepted(_, MavResult.MavResultAccepted).Wait();
        });
        var result = await client.CommandInt(MavCmd.MavCmdUser1, MavFrame.MavFrameGlobal, true, true, 0, 0, 0, 0, 0, 0, 0, CancellationToken.None);
        Assert.True(called);
    }
    [Fact]
    public async Task TestSimpleCallCommandLong()
    {
        var link = new VirtualLink();
        var server = CreateCommandServer(link);
        var client = CreateCommandClient(link);
        
        var called = false;
        server.OnCommandLong.Subscribe(_ =>
        {
            called = true;
            server.SendCommandAckAccepted(_, MavResult.MavResultAccepted).Wait();
        });
        var result = await client.CommandLong(MavCmd.MavCmdUser1, 0, 0, 0, 0, 0, 0, 0,  CancellationToken.None);
        Assert.True(called);
    }
    
    [Fact]
    public async Task TestConfirmationCallCommandInt()
    {
        int cnt = 0;
        // Emulation of packet loss
        var link = new VirtualLink(_=> ++cnt>2);
        var server = CreateCommandServer(link);
        var client = CreateCommandClient(link);
       
        
        var called = false;
        server.OnCommandInt.Subscribe(_ =>
        {
            called = true;
            server.SendCommandAckAccepted(_, MavResult.MavResultAccepted).Wait();
        });
        var result = await client.CommandInt(MavCmd.MavCmdUser1, MavFrame.MavFrameGlobal, true, true, 0, 0, 0, 0, 0, 0, 0,CancellationToken.None);
        Assert.True(called);
        
            
    }
    
    [Fact]
    public async Task TestConfirmationCallCommandLong()
    {
        int cnt = 0;
        // Emulation of packet loss
        var link = new VirtualLink(_=> ++cnt>3);
        var server = CreateCommandServer(link);
        var client = CreateCommandClient(link);
        
        var called = false;
        server.OnCommandLong.Subscribe(_ =>
        {
            called = true;
            server.SendCommandAckAccepted(_, MavResult.MavResultAccepted).Wait();
        });
        var result = await client.CommandLong(MavCmd.MavCmdUser1, 0, 0, 0, 0, 0, 0, 0, CancellationToken.None);
        Assert.True(called);
        
            
    }
    
    [Fact]
    public async Task TestCommandList()
    {
        int cnt = 0;
        // Emulation of packet loss
        var link = new VirtualLink(_=> ++cnt>3);
        var server = CreateCommandServer(link);
        var client = CreateCommandClient(link);
        var intList = new CommandIntServerEx(server);
        var longList = new CommandLongServerEx(server);
        var called = false;
        longList[MavCmd.MavCmdUser1] = (from, args, cancel)  =>
        {
            called = true;
            return Task.FromResult(new CommandResult(MavResult.MavResultAccepted));
        };
       
        
        var result = await client.CommandLong(MavCmd.MavCmdUser1, 0, 0, 0, 0, 0, 0, 0, CancellationToken.None);
        Assert.True(called);

        called = false;
        intList[MavCmd.MavCmdUser1] = (from, args, cancel)  =>
        {
            called = true;
            return Task.FromResult(new CommandResult(MavResult.MavResultAccepted));
        };
        result = await client.CommandInt(MavCmd.MavCmdUser1, MavFrame.MavFrameGlobal, true, true, 0, 0, 0, 0, 0, 0, 0,CancellationToken.None);
        Assert.True(called);
            
    }

    
}