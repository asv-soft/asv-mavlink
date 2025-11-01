using System.Collections.Generic;

namespace Asv.Mavlink;

internal record ArduCopterMotorsMatrixConfig(string Version, IEnumerable<Layout> Layouts);

internal record Layout(int Class, int Type, IEnumerable<Motor> Motors);

internal record Motor
{
	public int Number { get; init; }
	public int TestOrder { get; init; }
	public required string Rotation { get; init; }
	public float Roll { get; init; }
	public float Pitch { get; init; }
}