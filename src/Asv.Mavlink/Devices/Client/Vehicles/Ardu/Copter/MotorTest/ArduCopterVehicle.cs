namespace Asv.Mavlink;

internal class ArduCopterVehicle
{
	private ArduCopterVehicle(ArdupilotFrameClassEnum frameClass, ArdupilotFrameTypeEnum frameType)
	{
		FrameClass = frameClass;
		FrameType = frameType;
	}

	public static ArduCopterVehicle CreateVehicle(int frameClass, int frameType) =>
		new((ArdupilotFrameClassEnum)frameClass, (ArdupilotFrameTypeEnum)frameType);

	public static ArduCopterVehicle CreateUnknownVehicle() => new(ArdupilotFrameClassEnum.Undefined, ArdupilotFrameTypeEnum.Unknown);

	public ArdupilotFrameClassEnum FrameClass { get; init; }
	public ArdupilotFrameTypeEnum FrameType { get; init; }
}