#nullable enable
namespace Asv.Mavlink;

/// <summary>
/// Represents a vehicle mode.
/// </summary>
public interface IVehicleMode
{
    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    /// <returns>The name of the property.</returns>
    public string Name { get; }

    /// <summary>
    /// Gets the description of the property.
    /// </summary>
    /// <returns>
    /// A string representing the description.
    /// </returns>
    public string Description { get; }

    /// <summary>
    /// This flag indicates whether the vehicle can be set into this mode by the user command.
    /// You shouldn't show this mode in the user interface if this flag is true.
    /// </summary>
    /// <value>
    /// A boolean value that represents whether the vehicle can be set into this mode by the user command.
    /// </value>
    public bool InternalMode { get; }
}

/// <summary>
/// Represents a specific mode of a vehicle.
/// </summary>
public class VehicleMode : IVehicleMode
{
    /// <summary>
    /// Represents an unknown vehicle mode.
    /// </summary>
    public static VehicleMode Unknown = new("Unknown", "Unknown mode");

    /// <summary>
    /// Represents a vehicle mode.
    /// </summary>
    /// <param name="name">The name of the mode.</param>
    /// <param name="description">The description of the mode.</param>
    /// <param name="internalMode">Indicates whether the mode is for internal use only.</param>
    public VehicleMode(string name, string description, bool internalMode = false)
    {
        Name = name;
        Description = description;
        InternalMode = internalMode;
    }

    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    /// <returns>The name of the property.</returns>
    public string Name { get; }

    /// <summary>
    /// Gets the description of the property.
    /// </summary>
    /// <value>
    /// The description of the property.
    /// </value>
    public string Description { get; }

    /// <summary>
    /// Gets a boolean value indicating whether the Internal Mode is enabled or disabled.
    /// </summary>
    /// <remarks>
    /// When the Internal Mode is enabled, certain internal operations and functionalities are available.
    /// When the Internal Mode is disabled, these operations and functionalities are restricted.
    /// </remarks>
    /// <value>
    /// <see langword="true"/> if the Internal Mode is enabled; otherwise, <see langword="false"/>.
    /// </value>
    public bool InternalMode { get; }
}