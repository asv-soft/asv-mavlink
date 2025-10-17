namespace Asv.Mavlink;

public class ArduQuadPlaneFrameClient(IHeartbeatClient heartbeat, IParamsClientEx paramsClient)
    : ArduFrameClientBase(heartbeat, paramsClient, ArduQuadPlaneFrameCompability.Map)
{
    public static string FrameClassParam => "Q_FRAME_CLASS";
    public static string FrameTypeParam => "Q_FRAME_TYPE";
    
    public override string FrameClassParamName => FrameClassParam;
    public override string FrameTypeParamName => FrameTypeParam;
}