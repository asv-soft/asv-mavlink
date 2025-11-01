using System;

namespace Asv.Mavlink;

public class ArduCopterMotorsMatrixConfigException(string message, Exception inner) : MotorTestException(message, inner);