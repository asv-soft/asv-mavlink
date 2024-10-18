using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
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

   

    [Fact]
    public async void ParamsExt_Client_Write()
    {
        var paramValue = new[] { '1', '2' };
        var paramType = MavParamExtType.MavParamExtTypeInt16;
        var paramName = "param1";
        var link = new VirtualMavlinkConnection();
        var server = new ParamsExtServer(link.Server, new PacketSequenceCalculator(), new MavlinkIdentity( 13,13));
        var cts = new CancellationTokenSource();
        var disp = server.OnParamExtSet.Subscribe( async _ =>
        {
            await server.SendParamExtAck(_ =>
            {
                MavlinkTypesHelper.SetString(_.ParamId, paramName);
                _.ParamType = paramType;
                _.ParamValue = paramValue;
                _.ParamResult = ParamAck.ParamAckAccepted;
            }, cts.Token);
        });
        var client = new ParamsExtClient(link.Client,
            new MavlinkClientIdentity()
            {
                SystemId = 1,
                ComponentId = 1,
                TargetComponentId = 13,
                TargetSystemId = 13
            },
            new PacketSequenceCalculator(),
            new ParamsExtClientConfig());
        
        var writeResult = await client.Write(paramName,paramType, paramValue, cts.Token);
        Assert.Equal( ParamAck.ParamAckAccepted, writeResult.ParamResult);
        disp.Dispose();
    }
    [Fact(Skip = "Not implemented")]
    public async void ParamsExt_Client_Read()
    {
        var paramValue = new[] { '1', '2' };
        var paramType = MavParamExtType.MavParamExtTypeInt16;
        var paramName = "param1";
        var link = new VirtualMavlinkConnection();
        var server = new ParamsExtServer(link.Server, new PacketSequenceCalculator(), new MavlinkIdentity( 13,13));
        var cts = new CancellationTokenSource();
        var disp = server.OnParamExtRequestRead.Subscribe( async _ =>
        {
            await server.SendParamExtValue(_ =>
            {
                MavlinkTypesHelper.SetString(_.ParamId, paramName);
                _.ParamType = paramType;
                _.ParamValue = paramValue;
            }, cts.Token);
        });
        var client = new ParamsExtClient(link.Client,
            new MavlinkClientIdentity()
            {
                SystemId = 1,
                ComponentId = 1,
                TargetComponentId = 13,
                TargetSystemId = 13
            },
            new PacketSequenceCalculator(),
            new ParamsExtClientConfig());
        
        var writeResult = await client.Read(paramName, cts.Token);
        Assert.Equal( paramValue , writeResult.ParamValue );
        disp.Dispose();
    }
   
    #endregion

    #region Server

    
    public void ParamsExtServer_Should_Throw_ArgumentNullException_If_Connection_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new ParamsExtServer(null,
                new PacketSequenceCalculator(),
                new MavlinkIdentity());
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
                new MavlinkIdentity());
        });
    }

   

    #endregion
}