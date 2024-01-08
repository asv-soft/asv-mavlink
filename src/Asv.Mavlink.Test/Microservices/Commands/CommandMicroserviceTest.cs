using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Xunit;

namespace Asv.Mavlink.Test;

public class CommandMicroserviceTest
{
    private static CommandClient CreateCommandClient(VirtualMavlinkConnection link)
    {
        var client = new CommandClient(link.Client,
            new MavlinkClientIdentity { SystemId = 1, ComponentId = 1, TargetComponentId = 13, TargetSystemId = 13 },
            new PacketSequenceCalculator(), new CommandProtocolConfig
            {
                CommandTimeoutMs = 1000,
                CommandAttempt = 5,
            });
        return client;
    }

    private static CommandServer CreateCommandServer(VirtualMavlinkConnection link)
    {
        var server = new CommandServer(link.Server, new PacketSequenceCalculator(),
            new MavlinkIdentity (13,13), TaskPoolScheduler.Default);
        return server;
    }
    
    [Fact]
    public async Task Client_Call_Command_Int_And_Server_Catch_It()
    {
        var link = new VirtualMavlinkConnection();
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
    public async Task Client_Call_Command_Long_And_Server_Catch_It()
    {
        var link = new VirtualMavlinkConnection();
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
    public async Task Client_Call_Command_Int_And_Server_Catch_It_With_Packet_Loss()
    {
        int cnt = 0;
        // Emulation of packet loss
        var link = new VirtualMavlinkConnection(_=> ++cnt>2);
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
    public async Task Client_Call_Command_Long_And_Server_Catch_It_With_Packet_Loss()
    {
        int cnt = 0;
        // Emulation of packet loss
        var link = new VirtualMavlinkConnection(_=> ++cnt>3);
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
    public async Task Client_Call_Command_Long_And_Server_Catch_It_With_Extended_Interface_And_Packet_Loss()
    {
        int cnt = 0;
        // Emulation of packet loss
        var link = new VirtualMavlinkConnection(_=> Interlocked.Increment(ref cnt) >3);
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
        Assert.Equal(MavResult.MavResultAccepted, result.Result);

        called = false;
        Interlocked.Exchange(ref cnt, 0);
        intList[MavCmd.MavCmdUser1] = (from, args, cancel)  =>
        {
            called = true;
            return Task.FromResult(new CommandResult(MavResult.MavResultAccepted));
        };
        result = await client.CommandInt(MavCmd.MavCmdUser1, MavFrame.MavFrameGlobal, true, true, 0, 0, 0, 0, 0, 0, 0,CancellationToken.None);
        Assert.True(called);
        Assert.Equal(MavResult.MavResultAccepted, result.Result);
            
    }

    
}