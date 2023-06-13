using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Asv.Cfg.ImMemory;
using Asv.Mavlink.V2.Common;
using Xunit;

namespace Asv.Mavlink.Test;

public class ParamsMicroserviceTest
{
    public static readonly IMavParamTypeMetadata Param1 = MavParamFactory.CreateInt32("PARAM1", "Param 1");
    public static readonly IMavParamTypeMetadata Param2 = MavParamFactory.CreateInt32("PARAM2", "Param 2");
    public static readonly IMavParamTypeMetadata Param3 = MavParamFactory.CreateInt32("PARAM3", "Param 3");
    
    [Fact]
    public async Task Define_Param_List()
    {
        var link = new VirtualLink();
        var encoding = MavParamHelper.GetEncoding(MavParamEncodingType.CStyleEncoding);
        var srvSeq = new PacketSequenceCalculator();
        var srvId = new MavlinkServerIdentity(componentId: 1, systemId: 1);
        var srvCfg = new InMemoryConfiguration();
        var statusServer = new StatusTextServer(link.Server, srvSeq, srvId, new StatusTextLoggerConfig(),Scheduler.Default);
        var server = new ParamsServer(link.Server,srvSeq,srvId,Scheduler.Default);
        var serverEx = new ParamsServerEx(server, statusServer, new []
        {
            Param1,
            Param2,
            Param3,
        }, encoding, srvCfg, new ParamsServerExConfig { SendingParamItemDelayMs = 50 });

        
        
        
        var client = new ParamsClient(link.Client, new(2,2,1,1), new PacketSequenceCalculator(), new ParamsClientExConfig(), Scheduler.Default);
        var clientEx = new ParamsClientEx(client, new ParamsClientExConfig());
        clientEx.Init(encoding, new List<ParamDescription>());

        await clientEx.ReadAll();
        
        clientEx.Base.OnParamValue.Subscribe(_ =>
        {

        });

        serverEx[Param1] = 1;

        await Task.Delay(1000);





    }
}