using System;

namespace Asv.Mavlink;

public class MotorTestException : Exception
{
	public MotorTestException() { }

	public MotorTestException(string message)
		: base(message) { }

	public MotorTestException(string message, Exception inner)
		: base(message, inner) { }
}