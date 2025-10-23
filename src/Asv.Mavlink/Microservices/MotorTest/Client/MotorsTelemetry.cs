using System.Collections.Generic;

namespace Asv.Mavlink;

public class MotorsTelemetry
{
	public IEnumerable<MotorPwm> MotorPwms { get; init; }
	public int MotorCount { get; init; }
}