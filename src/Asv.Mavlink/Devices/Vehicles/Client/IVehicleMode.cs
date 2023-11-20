#nullable enable
namespace Asv.Mavlink;

public interface IVehicleMode
{
    public string Name { get; }
    public string Description { get; }
    /// <summary>
    /// This flag indicates whether the vehicle can be set into this mode by the user command.
    /// You shouldn't show this mode in the user interface if this flag is true.
    /// </summary>
    public bool InternalMode { get; }
}

public class VehicleMode : IVehicleMode
{
    public static VehicleMode Unknown = new("Unknown", "Unknown mode");
        
    public VehicleMode(string name, string description, bool internalMode = false)
    {
        Name = name;
        Description = description;
        InternalMode = internalMode;
    }
    public string Name { get; }
    public string Description { get; }
    public bool InternalMode { get; }
}