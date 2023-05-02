﻿using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class StatusTextMicroserviceTests
{
    private readonly ITestOutputHelper _output;

    public StatusTextMicroserviceTests(ITestOutputHelper output)
    {
        _output = output;
    }
    
    #region Client

    [Fact]
    public async Task Client_Service_Should_Throw_Argument_Null_Exception_If_Identity_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualLink();

            var client = new StatusTextClient(link.Client,
                null,
                new PacketSequenceCalculator(),
                Scheduler.Default);
        });
    }
    
    [Fact]
    public async Task Client_Service_Should_Throw_Argument_Null_Exception_If_Sequence_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualLink();

            var client = new StatusTextClient(link.Client,
                new MavlinkClientIdentity(),
                null,
                Scheduler.Default);
        });
    }
    
    [Fact]
    public async Task Client_Service_Should_Throw_Argument_Null_Exception_If_Connection_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var client = new StatusTextClient(null,
                new MavlinkClientIdentity(),
                new PacketSequenceCalculator(),
                Scheduler.Default);
        });
    }
    
    [Fact]
    public async Task Client_Service_Should_Throw_Argument_Null_Exception_If_Scheduler_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualLink();

            var client = new StatusTextClient(link.Client,
                new MavlinkClientIdentity(),
                new PacketSequenceCalculator(),
                null);
        });
    }

    #endregion

    #region Server

    [Fact]
    public async Task Server_Service_Should_Throw_Argument_Null_Exception_If_Identity_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualLink();

            var server = new StatusTextServer(link.Client,
                new PacketSequenceCalculator(),
                null,
                new StatusTextLoggerConfig(),
                Scheduler.Default);
        });
    }
    
    [Fact]
    public async Task Server_Service_Should_Throw_Argument_Null_Exception_If_Connection_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var server = new StatusTextServer(null,
                new PacketSequenceCalculator(),
                new MavlinkServerIdentity(),
                new StatusTextLoggerConfig(),
                Scheduler.Default);
        });
    }
    
    [Fact]
    public async Task Server_Service_Should_Throw_Argument_Null_Exception_If_Sequence_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualLink();

            var server = new StatusTextServer(link.Client,
                null,
                new MavlinkServerIdentity(),
                new StatusTextLoggerConfig(),
                Scheduler.Default);
        });
    }
    
    [Fact]
    public async Task Server_Service_Should_Throw_Argument_Null_Exception_If_Config_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualLink();

            var server = new StatusTextServer(link.Client,
                new PacketSequenceCalculator(),
                new MavlinkServerIdentity(),
                null,
                Scheduler.Default);
        });
    }
    
    [Fact]
    public async Task Server_Service_Should_Throw_Argument_Null_Exception_If_Scheduler_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualLink();

            var server = new StatusTextServer(link.Client,
                new PacketSequenceCalculator(),
                new MavlinkServerIdentity(),
                new StatusTextLoggerConfig(),
                null);
        });
    }

    #endregion

    #region Common

    [Theory]
    [InlineData("The quick brown fox jumps over the lazy dog")]
    [InlineData("/><#$@!%^&*()-=+_?.,]{}[~`';:\"|^\\")]
    [InlineData("1234567890")]
    public async Task OnMessage_Property_Should_Return_Expected_Status_Message(string messageText)
    {
        var link = new VirtualLink();

        var server = new StatusTextServer(link.Server, new PacketSequenceCalculator(),
            new MavlinkServerIdentity(), new StatusTextLoggerConfig(), Scheduler.Default);

        var client = new StatusTextClient(link.Client, new MavlinkClientIdentity(),
            new PacketSequenceCalculator(), Scheduler.Default);

        server.Log(MavSeverity.MavSeverityDebug, messageText);

        var message = await client.OnMessage.FirstAsync();
        
        _output.WriteLine($"Client status message: {message.Text}");
        
        Assert.Equal(messageText, message.Text);
    }

    [Fact]
    public async Task Name_Property_Should_Be_Set_To_Expected_Value()
    {
        var link = new VirtualLink();
        var clientIdentity = new MavlinkClientIdentity()
        {
            SystemId = 1,
            ComponentId = 2
        };
        var serverIdentity = new MavlinkServerIdentity()
        {
            SystemId = 1,
            ComponentId = 2
        };

        var server = new StatusTextServer(link.Server, new PacketSequenceCalculator(),
            serverIdentity, new StatusTextLoggerConfig(), Scheduler.Default);

        var client = new StatusTextClient(link.Client, clientIdentity,
            new PacketSequenceCalculator(), Scheduler.Default);

        server.Log(MavSeverity.MavSeverityDebug, "TEST MESSAGE");
        var name = client.Name.Value;
        Assert.Equal(name, $"[{clientIdentity.TargetSystemId},{clientIdentity.TargetComponentId}]");
    }

    [Fact]
    public async Task Check_For_Server_Overload()
    {
        var link = new VirtualLink();

        var server = new StatusTextServer(link.Server, new PacketSequenceCalculator(),
            new MavlinkServerIdentity(), new StatusTextLoggerConfig(), Scheduler.Default);

        var client = new StatusTextClient(link.Client, new MavlinkClientIdentity(),
            new PacketSequenceCalculator(), Scheduler.Default);

        var isOverloaded = false;
        var overloadIndex = 0;

        for (var i = 0; i < 1000; i++)
        {
            if (server.Debug($"Message {i}")) continue;
            
            isOverloaded = true;
            _output.WriteLine($"Breaked on: {i}");
            overloadIndex = i;
            break;
        }
        
        Assert.True(isOverloaded);
        Assert.Equal(101, overloadIndex);
        
        var message = await client.OnMessage.FirstAsync();
        _output.WriteLine($"Client status message: {message.Text}");
        Assert.Equal("Message 0", message.Text);
    }

    [Fact]
    public async Task Check_For_Server_Packet_Loss()
    {
        var packetsCount = 0;
        var lostPacketsCount = 4;
        
        var link = new VirtualLink(_ => true,_ => ++packetsCount > lostPacketsCount);

        var serverIdentity = new MavlinkServerIdentity()
        {
            ComponentId = 13,
            SystemId = 13
        };
        
        var clientIdentity = new MavlinkClientIdentity()
        {
            SystemId = 1,
            ComponentId = 1,
            TargetSystemId = 13,
            TargetComponentId = 13
        };
        
        var server = new StatusTextServer(link.Server, new PacketSequenceCalculator(),
            serverIdentity, new StatusTextLoggerConfig(), Scheduler.Default);

        var client = new StatusTextClient(link.Client, clientIdentity,
            new PacketSequenceCalculator(), Scheduler.Default);
        
        // currently impossible to check if packets were lost because Connection properties are protected
    }

    #endregion
}