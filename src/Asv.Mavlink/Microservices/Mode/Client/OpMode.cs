#nullable enable
using System;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink;

/// <summary>
/// Represents a vehicle mode.
/// </summary>
public class OpMode:IEquatable<OpMode>
{
    public static OpMode Unknown = new("Unknown", "Unknown mode",true, 0, 0,0);
    
    public OpMode(string name, string description, bool internalMode, MavModeFlag mode, uint customMode, uint customSubMode)
    {
        Name = name;
        Description = description;
        InternalMode = internalMode;
        Mode = mode;
        CustomMode = customMode;
        CustomSubMode = customSubMode;
    }

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
    public MavModeFlag Mode { get; }
    public uint CustomMode { get; }
    public uint CustomSubMode { get; }

    public bool Equals(OpMode? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Mode == other.Mode && CustomMode == other.CustomMode && CustomSubMode == other.CustomSubMode;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((OpMode)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Mode, CustomMode, CustomSubMode);
    }

    public static bool operator ==(OpMode? left, OpMode? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(OpMode? left, OpMode? right)
    {
        return !Equals(left, right);
    }
}
