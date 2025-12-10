namespace Asv.Mavlink;

public class ArduCopterMotorTestClient(IHeartbeatClient heartbeat, ICommandClient commandClient, IParamsClientEx paramsClientEx)
	: ArduMotorTestClient(heartbeat, commandClient, paramsClientEx)
{
	protected override string FrameClassParam => "FRAME_CLASS";
	protected override string FrameTypeParam => "FRAME_TYPE";
}