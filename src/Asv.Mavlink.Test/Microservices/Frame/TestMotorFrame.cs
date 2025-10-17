using System.Collections.Generic;

namespace Asv.Mavlink.Test;

public class TestMotorFrame: IMotorFrame
{
    public string Id => "Test";
    public IReadOnlyDictionary<string, string> Meta { get; } = new Dictionary<string, string>();
}