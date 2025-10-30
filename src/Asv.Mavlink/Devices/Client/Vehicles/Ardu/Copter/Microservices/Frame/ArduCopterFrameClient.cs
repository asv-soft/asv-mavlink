namespace Asv.Mavlink;

public class ArduCopterFrameClient(IHeartbeatClient heartbeat, IParamsClientEx paramsClient)
    : ArduFrameClientBase(heartbeat, paramsClient, ArduCopterFrameCompability.Map)
{
    public static string FrameClassParam => "FRAME_CLASS";
    public static string FrameTypeParam => "FRAME_TYPE";
    
    public override string FrameClassParamName => FrameClassParam;
    public override string FrameTypeParamName => FrameTypeParam;
}
