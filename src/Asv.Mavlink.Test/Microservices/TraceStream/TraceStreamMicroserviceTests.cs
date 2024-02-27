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
        var identity = new MavlinkServerIdentity
        {
            SystemId = 1,
            ComponentId = 2
        };
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
                new MavlinkServerIdentity(),
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
                new MavlinkServerIdentity(),
                new TraceStreamConfig(),
                Scheduler.Default);
        });
    }

    [Fact]
    public void TraceStreamServer_Should_Throw_ArgumentNullException_If_Identity_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();

            var server = new TraceStreamServer(link.Server,
                new PacketSequenceCalculator(),
                null,
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
                new MavlinkServerIdentity(),
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
                new MavlinkServerIdentity(),
                new TraceStreamConfig(),
                null);
        });
    }

    #endregion
}