namespace Asv.Mavlink;

public class DroneFrameIsNotAvailableException() : FrameMicroserviceException("This frame is not supported by the current device.") { }