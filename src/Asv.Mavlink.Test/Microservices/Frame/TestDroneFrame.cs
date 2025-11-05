using System.Collections.Generic;

namespace Asv.Mavlink.Test;

public class TestDroneFrame: IDroneFrame
{
    public string Id => "Test";
    public IReadOnlyDictionary<string, string> Meta { get; } = new Dictionary<string, string>();
}