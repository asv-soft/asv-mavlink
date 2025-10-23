using System.Collections.Generic;

namespace Asv.Mavlink;

public static class ServoChannelFunction
{
	public static IReadOnlyDictionary<int, int> MotorsByFunction { get;} = new Dictionary<int, int>
	{
		{
			33, 1 // k_motor1
		},
		{
			34, 2
		},
		{
			35, 3
		},
		{
			36, 4
		},
		{
			37, 5
		},
		{
			38, 6
		},
		{
			39, 7
		},
		{
			40, 8
		},
		{
			82, 9
		},
		{
			83, 10
		},
		{
			84, 11
		},
		{
			85, 12
		},
		{
			160, 13
		},
		{
			161, 14
		},
		{
			162, 15
		},
		{
			163, 16
		},
		{
			164, 17
		},
		{
			165, 18
		},
		{
			166, 19
		},
		{
			167, 20
		},
		{
			168, 21
		},
		{
			169, 22
		},
		{
			170, 23
		},
		{
			171, 24
		},
		{
			172, 25
		},
		{
			173, 26
		},
		{
			174, 27
		},
		{
			175, 28
		},
		{
			176, 29
		},
		{
			177, 30
		},
		{
			178, 31
		},
		{
			179, 32 // k_motor32
		}
	};
}