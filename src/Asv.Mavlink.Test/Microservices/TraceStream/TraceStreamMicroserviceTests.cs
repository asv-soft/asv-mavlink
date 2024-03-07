using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class TraceStreamMicroserviceTests
{
    private readonly ITestOutputHelper _output;

    public TraceStreamMicroserviceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    #region Initialization Methods

    private static TraceStreamClient CreateTraceStreamClient(VirtualMavlinkConnection link)
    {
        var identity = new MavlinkClientIdentity
        {
            SystemId = 13,
            ComponentId = 13,
            TargetSystemId = 1,
            TargetComponentId = 2
        };
        var seqCalculator = new PacketSequenceCalculator();
        return new TraceStreamClient(link.Client, identity, seqCalculator);
    }

    private static TraceStreamServer CreateTraceStreamServer(VirtualMavlinkConnection link)
    {
        var seqCalculator = new PacketSequenceCalculator();
        var identity = new MavlinkIdentity(1, 2);
        var config = new TraceStreamConfig
        {
            MaxQueueSize = 100,
            MaxSendRateHz = 10
        };
        return new TraceStreamServer(link.Server, seqCalculator, identity, config, Scheduler.Default);
    }

    #endregion

    #region Client

    [Fact]
    public void TraceStreamClient_Should_Initialize_With_Correct_Name()
    {
        var identity = new MavlinkClientIdentity
        {
            TargetSystemId = 1,
            TargetComponentId = 2
        };
        var link = new VirtualMavlinkConnection();
        var seqCalculator = new PacketSequenceCalculator();
        var client = new TraceStreamClient(link.Client, identity, seqCalculator);

        Assert.Equal("[1,2]", client.Name.Value);
    }

    [Fact]
    public async Task TraceStreamClient_Should_Handle_DebugVectorMessage_Correctly()
    {
        var link = new VirtualMavlinkConnection();

        var client = CreateTraceStreamClient(link);
        var server = CreateTraceStreamServer(link);

        var debugVectorMessage = new DebugVectorMessage
        {
            Name = "test",
            TimeUsec = 123456789,
            X = 1.0f,
            Y = 2.0f,
            Z = 3.0f
        };

        Assert.True(server.AddMessage(debugVectorMessage));

        var resultMessage = await client.OnDebugVectorMessage.FirstAsync();

        Assert.NotNull(resultMessage);
        Assert.Equal(debugVectorMessage.Name, resultMessage.Name);
        Assert.Equal(debugVectorMessage.TimeUsec, resultMessage.TimeUsec);
        Assert.Equal(resultMessage.X, resultMessage.X);
        Assert.Equal(resultMessage.Y, resultMessage.Y);
        Assert.Equal(resultMessage.Z, resultMessage.Z);
    }
    
    [Fact]
    public async Task TraceStreamClient_Should_Handle_NamedValueFloatMessage_Correctly()
    {
        var link = new VirtualMavlinkConnection();

        var client = CreateTraceStreamClient(link);
        var server = CreateTraceStreamServer(link);

        var namedValueFloatMessage = new NamedValueFloatMessage()
        {
            Name = "test",
            TimeBoot = 1234,
            Value = 1.1f
        };

        Assert.True(server.AddMessage(namedValueFloatMessage));

        var resultMessage = await client.OnNamedValueFloatMessage.FirstAsync();

        Assert.NotNull(resultMessage);
        Assert.Equal(namedValueFloatMessage.Name, resultMessage.Name);
        Assert.Equal(namedValueFloatMessage.TimeBoot, resultMessage.TimeBoot);
        Assert.Equal(namedValueFloatMessage.Value, resultMessage.Value);
    }
    
    [Fact]
    public async Task TraceStreamClient_Should_Handle_NamedValueIntMessage_Correctly()
    {
        var link = new VirtualMavlinkConnection();

        var client = CreateTraceStreamClient(link);
        var server = CreateTraceStreamServer(link);

        var namedValueIntMessage = new NamedValueIntMessage()
        {
            Name = "test",
            TimeBoot = 1234,
            Value = 1
        };

        Assert.True(server.AddMessage(namedValueIntMessage));

        var resultMessage = await client.OnNamedValueIntMessage.FirstAsync();

        Assert.NotNull(resultMessage);
        Assert.Equal(namedValueIntMessage.Name, resultMessage.Name);
        Assert.Equal(namedValueIntMessage.TimeBoot, resultMessage.TimeBoot);
        Assert.Equal(namedValueIntMessage.Value, resultMessage.Value);
    }
    
    [Fact]
    public async Task TraceStreamClient_Should_Handle_MemoryVectorMessage_Correctly()
    {
        var link = new VirtualMavlinkConnection();

        var client = CreateTraceStreamClient(link);
        var server = CreateTraceStreamServer(link);

        var memoryVectorMessage = new MemoryVectorMessage()
        {
            Address = 1,
            Type = 1,
            Value = new sbyte[32] ,
            Version = 1
        };

        Assert.True(server.AddMessage(memoryVectorMessage));

        var resultMessage = await client.OnMemoryVectorMessage.FirstAsync();

        Assert.NotNull(resultMessage);
        Assert.Equal(memoryVectorMessage.Address, resultMessage.Address);
        Assert.Equal(memoryVectorMessage.Type, resultMessage.Address);
        Assert.Equal(memoryVectorMessage.Value, resultMessage.Value);
        Assert.Equal(memoryVectorMessage.Version, resultMessage.Version);
    }
    
    [Fact]
     public async Task TraceStreamClient_Should_Handle_DebugVectorMessage_Bad_Values()
    {
        var link = new VirtualMavlinkConnection();

        var client = CreateTraceStreamClient(link);
        var server = CreateTraceStreamServer(link);

        var debugVectorMessage = new DebugVectorMessage
        {
            Name = "",
            TimeUsec = 12345678912345678912,
            X = 1234567890f,
            Y = 1234567891f,
            Z = 1234567892f
        };

        Assert.True(server.AddMessage(debugVectorMessage));

        var resultMessage = await client.OnDebugVectorMessage.FirstAsync();

        Assert.NotNull(resultMessage);
        Assert.Equal(debugVectorMessage.Name, resultMessage.Name);
        Assert.Equal(debugVectorMessage.TimeUsec, resultMessage.TimeUsec);
        Assert.Equal(resultMessage.X, resultMessage.X);
        Assert.Equal(resultMessage.Y, resultMessage.Y);
        Assert.Equal(resultMessage.Z, resultMessage.Z);
    }
    
    [Fact]
    public async Task TraceStreamClient_Should_Handle_NamedValueFloatMessage_Bad_Values()
    {
        var link = new VirtualMavlinkConnection();

        var client = CreateTraceStreamClient(link);
        var server = CreateTraceStreamServer(link);

        var namedValueFloatMessage = new NamedValueFloatMessage()
        {
            Name = "",
            TimeBoot = 1234567891,
            Value = 123456789.123456789f
        };

        Assert.True(server.AddMessage(namedValueFloatMessage));

        var resultMessage = await client.OnNamedValueFloatMessage.FirstAsync();

        Assert.NotNull(resultMessage);
        Assert.Equal(namedValueFloatMessage.Name, resultMessage.Name);
        Assert.Equal(namedValueFloatMessage.TimeBoot, resultMessage.TimeBoot);
        Assert.Equal(namedValueFloatMessage.Value, resultMessage.Value);
    }
    
    [Fact]
    public async Task TraceStreamClient_Should_Handle_NamedValueIntMessage_Bad_Values()
    {
        var link = new VirtualMavlinkConnection();

        var client = CreateTraceStreamClient(link);
        var server = CreateTraceStreamServer(link);

        var namedValueIntMessage = new NamedValueIntMessage()
        {
            Name = "",
            TimeBoot = 1234567891,
            Value = 1234567891
        };

        Assert.True(server.AddMessage(namedValueIntMessage));

        var resultMessage = await client.OnNamedValueIntMessage.FirstAsync();

        Assert.NotNull(resultMessage);
        Assert.Equal(namedValueIntMessage.Name, resultMessage.Name);
        Assert.Equal(namedValueIntMessage.TimeBoot, resultMessage.TimeBoot);
        Assert.Equal(namedValueIntMessage.Value, resultMessage.Value);
    }
    
    [Fact]
    public async Task TraceStreamClient_Should_Handle_MemoryVectorMessage_Bad_Values()
    {
        var link = new VirtualMavlinkConnection();

        var client = CreateTraceStreamClient(link);
        var server = CreateTraceStreamServer(link);

        var memoryVectorMessage = new MemoryVectorMessage()
        {
            Address = 1,
            Type = 3,
            Value = new sbyte[33] ,
            Version = 9
        };

        Assert.True(server.AddMessage(memoryVectorMessage));

        var resultMessage = await client.OnMemoryVectorMessage.FirstAsync();

        Assert.NotNull(resultMessage);
        Assert.Equal(memoryVectorMessage.Address, resultMessage.Address);
        Assert.Equal(memoryVectorMessage.Type, resultMessage.Address);
        Assert.Equal(memoryVectorMessage.Value, resultMessage.Value);
        Assert.Equal(memoryVectorMessage.Version, resultMessage.Version);
    }
    [Fact]
    public void TraceStreamClient_Should_Throw_ArgumentNullException_If_Connection_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var client = new TraceStreamClient(null, new MavlinkClientIdentity(), new PacketSequenceCalculator());
        });
    }

    [Fact]
    public void TraceStreamClient_Should_Throw_ArgumentNullException_If_Identity_Is_Null()
    {
        var link = new VirtualMavlinkConnection();

        Assert.Throws<ArgumentNullException>(() =>
        {
            var client = new TraceStreamClient(link.Client, null, new PacketSequenceCalculator());
        });
    }

    [Fact]
    public void TraceStreamClient_Should_Throw_ArgumentNullException_If_SequenceCalculator_Is_Null()
    {
        var link = new VirtualMavlinkConnection();

        Assert.Throws<ArgumentNullException>(() =>
        {
            var client = new TraceStreamClient(link.Client, new MavlinkClientIdentity(), null);
        });
    }

    #endregion

    #region Server
    
    [Fact]
    public void TraceStreamServer_Should_Throw_ArgumentNullException_If_Connection_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var server = new TraceStreamServer(null,
                new PacketSequenceCalculator(),
                new MavlinkIdentity(),
                new TraceStreamConfig(),
                Scheduler.Default);
        });
    }

    [Fact]
    public void TraceStreamServer_Should_Throw_ArgumentNullException_If_SequenceCalculator_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();

            var server = new TraceStreamServer(link.Server,
                null,
                new MavlinkIdentity(),
                new TraceStreamConfig(),
                Scheduler.Default);
        });
    }

    [Fact]
    public void TraceStreamServer_Should_Throw_ArgumentNullException_If_Config_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();

            var server = new TraceStreamServer(link.Server,
                new PacketSequenceCalculator(),
                new MavlinkIdentity(),
                null,
                Scheduler.Default);
        });
    }

    [Fact]
    public void TraceStreamServer_Should_Throw_ArgumentNullException_If_Scheduler_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();

            var server = new TraceStreamServer(link.Server,
                new PacketSequenceCalculator(),
                new MavlinkIdentity(),
                new TraceStreamConfig(),
                null);
        });
    }

    [Fact]
    public void TraceStreamServer_Should_Handle_Debug_MemoryVectorMessage_Queue_Overflow()
    {
        var link = new VirtualMavlinkConnection();
        MemoryVectorMessage message = new();
        message.Value = new sbyte[1];
        var server = new TraceStreamServer(link.Server,
            new PacketSequenceCalculator(),
            new MavlinkIdentity(),
            new TraceStreamConfig(), Scheduler.Default);
        for (int i = 0; i < 101; i++)
        {
            server.AddMessage(message);
        }
        Assert.False(server.AddMessage(message));
    }

    #endregion
}