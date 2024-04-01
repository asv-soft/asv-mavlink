using System;
using System.Reactive.Concurrency;
using Xunit;

namespace Asv.Mavlink.Test.ParamsExt;

public class ParamsExtMicroserviceTest
{
    #region Client
    
    [Fact]
    public void ParamsExtClient_Should_Throw_ArgumentNullException_If_Connection_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new ParamsExtClient(null,
                new MavlinkClientIdentity(),
                new PacketSequenceCalculator(),
                new ParamsExtClientConfig());
        });
    }
    
    [Fact]
    public void ParamsExtClient_Should_Throw_ArgumentNullException_If_ClientIdentity_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();

            _ = new ParamsExtClient(link.Client,
                null,
                new PacketSequenceCalculator(),
                new ParamsExtClientConfig());
        });
    }
    
    [Fact]
    public void ParamsExtClient_Should_Throw_ArgumentNullException_If_SequenceCalculator_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();

            _ = new ParamsExtClient(link.Client,
                new MavlinkClientIdentity(),
                null,
                new ParamsExtClientConfig());
        });
    }
    
    [Fact]
    public void ParamsExtClient_Should_Throw_ArgumentNullException_If_Config_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();

            _ = new ParamsExtClient(link.Client,
                new MavlinkClientIdentity(),
                new PacketSequenceCalculator(),
                null);
        });
    }
    
    #endregion

    #region Server
    
    [Fact]
    public void ParamsExtServer_Should_Throw_ArgumentNullException_If_Connection_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new ParamsExtServer(null,
                new PacketSequenceCalculator(),
                new MavlinkIdentity(),
                Scheduler.Default);
        });
    }
    
    [Fact]
    public void ParamsExtServer_Should_Throw_ArgumentNullException_If_SequenceCalculator_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();

            _ = new ParamsExtServer(link.Server,
                null,
                new MavlinkIdentity(),
                Scheduler.Default);
        });
    }
    
    [Fact]
    public void ParamsExtServer_Should_Throw_ArgumentNullException_If_Scheduler_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var link = new VirtualMavlinkConnection();

            _ = new ParamsExtServer(link.Server,
                new PacketSequenceCalculator(),
                new MavlinkIdentity(),
                null);
        });
    }
    
    #endregion
}