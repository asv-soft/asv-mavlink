using System;

namespace Asv.Mavlink;

public class FrameMicroserviceException(string message) : Exception(message) { }