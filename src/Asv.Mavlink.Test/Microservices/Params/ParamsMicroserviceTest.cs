using System.Reactive.Concurrency;
using Asv.Cfg.ImMemory;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Test;

public class ParamsMicroserviceTest
{
    public static readonly MavParamDesc Param1 = new("PARAM1", MavParamType.MavParamTypeInt32,16);
    
    public void Define_Param_List()
    {
        var link = new VirtualLink();
        var server = new ParamsServer(link.Server, new PacketSequenceCalculator(), new(componentId: 1, systemId: 1),Scheduler.Default);
        var serverEx = new ParamsServerEx(server, new []{Param1}, new MavParamValueConverter(), new InMemoryConfiguration(), new ParamsServerExConfig
        {
            ReadListTimeoutMs = 50,
        });
        
        
        var client = new ParamsClient(link.Client, new(2,2,1,1), new PacketSequenceCalculator(), new ParamsClientExConfig(), Scheduler.Default);
        var clientEx = new ParamsClientEx(client, new ParamsClientExConfig());
        
        
    }
}