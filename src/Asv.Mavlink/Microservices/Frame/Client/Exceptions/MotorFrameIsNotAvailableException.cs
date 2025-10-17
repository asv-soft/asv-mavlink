namespace Asv.Mavlink;

public class MotorFrameIsNotAvailableException() : FrameMicroserviceException("This frame is not supported by the current device.") { }