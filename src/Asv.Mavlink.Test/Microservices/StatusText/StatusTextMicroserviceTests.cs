using System;
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
    public void Client_Service_Should_Throw_Argument_Null_Exception_If_Identity_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();

            var client = new StatusTextClient(link.Client,
                null,
                new PacketSequenceCalculator());
        });
    }
    
    [Fact]
    public void Client_Service_Should_Throw_Argument_Null_Exception_If_Sequence_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();

            var client = new StatusTextClient(link.Client,
                new MavlinkClientIdentity(),
                null);
        });
    }
    
    [Fact]
    public void Client_Service_Should_Throw_Argument_Null_Exception_If_Connection_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var client = new StatusTextClient(null,
                new MavlinkClientIdentity(),
                new PacketSequenceCalculator());
        });
    }
    
   
    #endregion

    #region Server

   
    
    [Fact]
    public void Server_Service_Should_Throw_Argument_Null_Exception_If_Connection_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var server = new StatusTextServer(null,
                new PacketSequenceCalculator(),
                new MavlinkIdentity(),
                new StatusTextLoggerConfig());
        });
    }
    
    [Fact]
    public void Server_Service_Should_Throw_Argument_Null_Exception_If_Sequence_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();

            var server = new StatusTextServer(link.Client,
                null,
                new MavlinkIdentity(),
                new StatusTextLoggerConfig());
        });
    }
    
    [Fact]
    public void Server_Service_Should_Throw_Argument_Null_Exception_If_Config_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();

            var server = new StatusTextServer(link.Client,
                new PacketSequenceCalculator(),
                new MavlinkIdentity(),
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
        var link = new VirtualMavlinkConnection();
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);

        var server = new StatusTextServer(link.Server, new PacketSequenceCalculator(),
            serverId, new StatusTextLoggerConfig());

        var client = new StatusTextClient(link.Client, clientId,
            new PacketSequenceCalculator());

        server.Log(MavSeverity.MavSeverityDebug, messageText);

        var message = await client.OnMessage.FirstAsync();
        
        _output.WriteLine($"Client status message: {message.Text}");
        
        Assert.Equal(messageText, message.Text);
    }

    [Fact]
    public void Name_Property_Should_Be_Set_To_Expected_Value()
    {
        var link = new VirtualMavlinkConnection();
        var clientIdentity = new MavlinkClientIdentity()
        {
            SystemId = 1,
            ComponentId = 2
        };
        var serverIdentity = new MavlinkIdentity(1, 2);

        var server = new StatusTextServer(link.Server, new PacketSequenceCalculator(),
            serverIdentity, new StatusTextLoggerConfig());

        var client = new StatusTextClient(link.Client, clientIdentity,
            new PacketSequenceCalculator());

        server.Log(MavSeverity.MavSeverityDebug, "TEST MESSAGE");
        var name = client.Name.Value;
        Assert.Equal(name, $"[{clientIdentity.TargetSystemId},{clientIdentity.TargetComponentId}]");
    }

    [Fact]
    public async Task Check_For_Server_Overload()
    {
        var link = new VirtualMavlinkConnection();
        var clientId = new MavlinkClientIdentity
        {
            SystemId = 1,
            ComponentId = 2,
            TargetSystemId = 3,
            TargetComponentId = 4
        };
        var serverId = new MavlinkIdentity(clientId.TargetSystemId, clientId.TargetComponentId);
        
        var server = new StatusTextServer(link.Server, new PacketSequenceCalculator(),
            serverId, new StatusTextLoggerConfig());

        var client = new StatusTextClient(link.Client, clientId,
            new PacketSequenceCalculator());

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
    public void Check_For_Server_Packet_Loss()
    {
        var packetsCount = 0;
        var lostPacketsCount = 4;
        
        var link = new VirtualMavlinkConnection(_ => true,_ => ++packetsCount > lostPacketsCount);

        var serverIdentity = new MavlinkIdentity(13, 13);
        
        
        var clientIdentity = new MavlinkClientIdentity()
        {
            SystemId = 1,
            ComponentId = 1,
            TargetSystemId = 13,
            TargetComponentId = 13
        };
        
        var server = new StatusTextServer(link.Server, new PacketSequenceCalculator(),
            serverIdentity, new StatusTextLoggerConfig());

        var client = new StatusTextClient(link.Client, clientIdentity,
            new PacketSequenceCalculator());
        
        // currently impossible to check if packets were lost because Connection properties are protected
    }

    #endregion
}