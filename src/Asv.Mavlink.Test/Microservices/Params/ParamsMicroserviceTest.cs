using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Cfg;
using Asv.Mavlink.V2.Common;
using DynamicData;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class ParamsMicroserviceTest
{
    private readonly ITestOutputHelper _output;

    public ParamsMicroserviceTest(ITestOutputHelper output)
    {
        _output = output;
    }
    
    public static readonly IMavParamTypeMetadata Param1 = MavParam.SysS32("PARAM1", "Param 1",String.Empty);
    public static readonly IMavParamTypeMetadata Param2 = MavParam.SysS32("PARAM2", "Param 2",String.Empty);
    public static readonly IMavParamTypeMetadata Param3 = MavParam.SysS32("PARAM3", "Param 3",String.Empty);
    
    [Fact]
    public async Task Read_All_Param_List()
    {
        var link = new VirtualMavlinkConnection();
        
        var encoding = MavParamHelper.GetEncoding(MavParamEncodingType.CStyleEncoding);
        
        var srvSeq = new PacketSequenceCalculator();
        
        var srvId = new MavlinkIdentity(componentId: 1, systemId: 1);
        
        var srvCfg = new InMemoryConfiguration();
        
        var statusServer = new StatusTextServer(link.Server, 
            srvSeq, 
            srvId, 
            new StatusTextLoggerConfig());
        
        var server = new ParamsServer(link.Server,srvSeq,srvId);
        
        var serverEx = new ParamsServerEx(server, statusServer, 
            new []
            {
                Param1,
                Param2,
                Param3,
            }, 
            encoding, 
            srvCfg, 
            new ParamsServerExConfig { SendingParamItemDelayMs = 50 });
        
        var client = new ParamsClient(link.Client, 
            new(2,2,1,1), 
            new PacketSequenceCalculator(), 
            new ParamsClientExConfig());
        
        var clientEx = new ParamsClientEx(client, new ParamsClientExConfig());
        
        clientEx.Init(encoding, new List<ParamDescription>());

        var readParamNames = new List<string>(); 
        
        clientEx.Base.OnParamValue.Subscribe(_ =>
        {
            readParamNames.Add(MavlinkTypesHelper.GetString(_.ParamId));
        });
        
        await clientEx.ReadAll();
        
        Assert.Equal(Param1.Name, readParamNames[0]);
        Assert.Equal(Param2.Name, readParamNames[1]);
        Assert.Equal(Param3.Name, readParamNames[2]);
    }
    
    [Fact]
    public async Task Update_Param_After_Write_From_Server()
    {
        var link = new VirtualMavlinkConnection();
        
        var encoding = MavParamHelper.GetEncoding(MavParamEncodingType.CStyleEncoding);
        
        var srvSeq = new PacketSequenceCalculator();
        
        var srvId = new MavlinkIdentity(componentId: 1, systemId: 1);
        
        var srvCfg = new InMemoryConfiguration();
        
        var statusServer = new StatusTextServer(link.Server, 
            srvSeq, 
            srvId, 
            new StatusTextLoggerConfig());
        
        var server = new ParamsServer(link.Server,srvSeq,srvId);
        
        var serverEx = new ParamsServerEx(server, statusServer, 
            new []
            {
                Param1,
                Param2,
                Param3,
            }, 
            encoding, 
            srvCfg, 
            new ParamsServerExConfig { SendingParamItemDelayMs = 50 });
        
        var client = new ParamsClient(link.Client, 
            new(2,2,1,1), 
            new PacketSequenceCalculator(), 
            new ParamsClientExConfig());
        
        var clientEx = new ParamsClientEx(client, new ParamsClientExConfig());
        
        clientEx.Init(encoding, new List<ParamDescription>());

        var readParams = new Dictionary<string, ParamValuePayload>(); 
        
        clientEx.Base.OnParamValue.Subscribe(_ =>
        {
            readParams[MavlinkTypesHelper.GetString(_.ParamId)] = _;
        });
        
        await clientEx.ReadAll();

        Assert.Equal(Param1.Name, MavlinkTypesHelper.GetString(readParams[Param1.Name].ParamId));
        Assert.Equal(Param2.Name, MavlinkTypesHelper.GetString(readParams[Param2.Name].ParamId));
        Assert.Equal(Param3.Name, MavlinkTypesHelper.GetString(readParams[Param3.Name].ParamId));

        var oldValue = (int)serverEx[Param1];
        
        serverEx[Param1] = 1;
        
        Assert.Equal(1, readParams[Param1.Name].ParamValue);
        
        _output.WriteLine($"updated: {readParams[Param1.Name].ParamValue}");
        _output.WriteLine($"old: {oldValue}");
    }
    
    [Fact]
    public async Task Update_Param_After_Write_From_Client()
    {
        var link = new VirtualMavlinkConnection();
        
        var encoding = MavParamHelper.GetEncoding(MavParamEncodingType.CStyleEncoding);
        
        var srvSeq = new PacketSequenceCalculator();
        
        var srvId = new MavlinkIdentity(componentId: 1, systemId: 1);
        
        var srvCfg = new InMemoryConfiguration();
        
        var statusServer = new StatusTextServer(link.Server, 
            srvSeq, 
            srvId, 
            new StatusTextLoggerConfig());
        var fake = new FakeTimeProvider();
        var server = new ParamsServer(link.Server, srvSeq, srvId,fake);
        
        var serverEx = new ParamsServerEx(server, statusServer, 
            new []
            {
                MavParam.SysR32("PARAM1", "Param 1",String.Empty),
                Param2,
                Param3,
            }, 
            encoding, srvCfg, new ParamsServerExConfig { SendingParamItemDelayMs = 50 },fake);
        
        var client = new ParamsClient(link.Client, 
            new(2,2,1,1), 
            new PacketSequenceCalculator(), new ParamsClientExConfig(),fake);
        
        var clientEx = new ParamsClientEx(client, new ParamsClientExConfig(),fake);
        
        clientEx.Init(encoding, new List<ParamDescription>());
        
        await clientEx.ReadAll();

        clientEx.Items.Bind(out var paramItems).Subscribe();

        var oldValue = (float)paramItems[0].Value.Value;

        paramItems[0].Value.OnNext(12.3f);

        await paramItems[0].Write();
        
        var result = await clientEx.ReadOnce(paramItems[0].Name);
        
        Assert.Equal(12.3f, (float)result);
        
        _output.WriteLine($"updated: {(float)result}");
        _output.WriteLine($"old: {oldValue}");
    }

    [Fact]
    public async Task Parameters_Loss()
    {
        var link = new VirtualMavlinkConnection(serverToClientFilter: _ =>
        {
            var packet = _ as ParamValuePacket;
            if (packet == null) return false;
            var name = MavlinkTypesHelper.GetString(packet.Payload.ParamId);
            return name != "PARAM1" & name != "PARAM3";
        });
        
        var encoding = MavParamHelper.GetEncoding(MavParamEncodingType.CStyleEncoding);
        
        var srvSeq = new PacketSequenceCalculator();
        
        var srvId = new MavlinkIdentity(componentId: 1, systemId: 1);
        
        var srvCfg = new InMemoryConfiguration();
        
        var statusServer = new StatusTextServer(link.Server, 
            srvSeq, 
            srvId, 
            new StatusTextLoggerConfig());
        
        var server = new ParamsServer(link.Server,srvSeq,srvId);
        
        var serverEx = new ParamsServerEx(server, statusServer, 
            new []
            {
                Param1,
                Param2,
                Param3,
            }, 
            encoding, 
            srvCfg, 
            new ParamsServerExConfig { SendingParamItemDelayMs = 50 });
        
        var client = new ParamsClient(link.Client, 
            new(2,2,1,1), 
            new PacketSequenceCalculator(), 
            new ParamsClientExConfig());
        
        var clientEx = new ParamsClientEx(client, new ParamsClientExConfig());
        
        clientEx.Init(encoding, new List<ParamDescription>());

        var readParams = new Dictionary<string, ParamValuePayload>(); 
        
        clientEx.Base.OnParamValue.Subscribe(_ =>
        {
            readParams[MavlinkTypesHelper.GetString(_.ParamId)] = _;
        });
        
        await clientEx.ReadAll();
        
        Assert.False(readParams.ContainsKey(Param1.Name));
        Assert.True(readParams.ContainsKey(Param2.Name));
        Assert.False(readParams.ContainsKey(Param3.Name));
    }
    
}