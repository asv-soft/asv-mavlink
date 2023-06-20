#nullable enable
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IVehicleMode
{
    public string Name { get; }
    public string Description { get; }
}

public class VehicleMode : IVehicleMode
{
    public static VehicleMode Unknown = new("Unknown", "Unknown mode");
        
    public VehicleMode(string name, string description)
    {
        Name = name;
        Description = description;
    }
    public string Name { get; }
    public string Description { get; }
}