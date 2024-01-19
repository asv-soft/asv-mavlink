using System;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

/// <summary>
/// A factory class for creating instances of <see cref="IMavParamTypeMetadata"/> objects.
/// </summary>
public static class MavParamFactory
{
    /// <summary>
    /// Creates an instance of IMavParamTypeMetadata with a data type of Int32.
    /// </summary>
    /// <param name="name">The name of the Int32 parameter.</param>
    /// <param name="shortDesc">A short description of the Int32 parameter.</param>
    /// <returns>An instance of IMavParamTypeMetadata with the specified name, short description, and default values.</returns>
    /// <remarks>
    /// This method creates a new instance of MavParamTypeMetadata using the specified name and sets the data type to MavParamTypeInt32.
    /// The ShortDesc property is initialized with the specified shortDesc value.
    /// The MinValue property is set to the minimum value of Int32 using int.MinValue.
    /// The MaxValue property is set to the maximum value of Int32 using int.MaxValue.
    /// The DefaultValue property is set to the default value of Int32 which is 0.
    /// </remarks>
    public static IMavParamTypeMetadata CreateInt32(string name,string shortDesc)
    {
        return new MavParamTypeMetadata(name, MavParamType.MavParamTypeInt32)
        {
            ShortDesc = shortDesc,
            MinValue = new(int.MinValue),
            MaxValue = new(int.MaxValue),
            DefaultValue = new((int)0),
        };
    }

    /// <summary>
    /// Creates a Real32 metadata object for MAVLink parameters. </summary> <param name="name">The name of the parameter.</param> <param name="shortDesc">A short description of the parameter.</param> <returns>A metadata object of type IMavParamTypeMetadata.</returns>
    /// /
    public static IMavParamTypeMetadata CreateReal32(string name,string shortDesc)
    {
        return new MavParamTypeMetadata(name, MavParamType.MavParamTypeReal32)
        {
            ShortDesc = shortDesc,
            MinValue = new(float.MinValue),
            MaxValue = new(float.MaxValue),
            DefaultValue = new((float)0),
        };
    }
}

public interface IMavParamTypeMetadata
{
    /// <summary>
    /// Parameter Name
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Parameter type
    /// </summary>
    MavParamType Type { get; }
    /// <summary>
    /// User readable name for a group of parameters which are commonly modified together. For example a GCS can shows params in a hierarchical display based on group 
    /// </summary>
    string Group { get; }
    /// <summary>
    /// User readable name for a 'type' of parameter. For example 'Developer', 'System', or 'Advanced'.
    /// </summary>
    string Category { get; }
    /// <summary>
    /// Short user facing description/name for parameter. Used in UI instead of internal parameter name.
    /// </summary>
    string ShortDesc { get; }
    /// <summary>
    /// Long user facing documentation of how the parameters works.
    /// </summary>
    string LongDesc { get; }
    /// <summary>
    /// Units for parameter value.
    /// </summary>
    string Units { get; }
    /// <summary>
    /// true: Vehicle must be rebooted if this value is changed
    /// </summary>
    bool RebootRequired { get; }
    /// <summary>
    /// true: value is volatile. Should not be included in creation of a CRC over param values for example.
    /// </summary>
    bool Volatile { get; }
    /// <summary>
    /// Minimum valid value
    /// If 'min' is not specified the minimum value is the minimum numeric value which can be represented by the 
    /// </summary>
    MavParamValue MinValue { get; }
    /// <summary>
    /// Maximum valid value
    /// If 'max' is not specified the minimum value is the maximum numeric value which can be represented by the 
    /// </summary>
    MavParamValue MaxValue { get; }
    /// <summary>
    /// Default value for parameter.
    /// If a defaultValue is not specified, there is no default for the parameter available at all. A GCS should not provide an option to reset this parameter to default.
    /// </summary>
    MavParamValue DefaultValue { get; }
    /// <summary>
    /// Increment to use for user facing UI which increments a value.
    /// </summary>
    MavParamValue Increment { get; }
    /// <summary>
    /// Array of values and textual descriptions for use by GCS ui.
    /// </summary>
    (MavParamValue,string)[] Values { get; }
    /// <summary>
    /// Bitmask of allowed values.
    /// </summary>
    (uint,MavParamValue)[] Bitmask { get; }

    /// <summary>
    /// Validates the given MavParamValue.
    /// </summary>
    /// <param name="newValue">The MavParamValue to validate.</param>
    /// <returns>True if the MavParamValue is valid, False otherwise.</returns>
    bool IsValid(MavParamValue newValue);

    /// <summary>
    /// Validates a MAV parameter value and returns an error message if validation fails. </summary> <param name="newValue">The new value to be validated.</param> <returns>A string containing the error message if validation fails; otherwise, an empty string.</returns>
    /// /
    string GetValidationError(MavParamValue newValue);
    
    private static string CombineConfigKey(string prefix, string name)
    {
        return prefix.IsNullOrWhiteSpace() ? name : $"{prefix}{name}";
    }
    
    public MavParamValue ReadFromConfig(IConfiguration config, string prefix = null)
    {
        var key = CombineConfigKey(prefix, Name);
        switch (Type)
        {
            case MavParamType.MavParamTypeUint8:
                return new MavParamValue(config.Get(key, (byte)DefaultValue));
            case MavParamType.MavParamTypeInt8:
                return new MavParamValue(config.Get(key, (sbyte)DefaultValue));
            case MavParamType.MavParamTypeUint16:
                return new MavParamValue(config.Get(key, (ushort)DefaultValue));
            case MavParamType.MavParamTypeInt16:
                return new MavParamValue(config.Get(key, (short)DefaultValue));
            case MavParamType.MavParamTypeUint32:
                return new MavParamValue(config.Get(key, (uint)DefaultValue));
            case MavParamType.MavParamTypeInt32:
                return new MavParamValue(config.Get(key, (int)DefaultValue));
            case MavParamType.MavParamTypeReal32:
                return new MavParamValue(config.Get(key, (float)DefaultValue));
            case MavParamType.MavParamTypeUint64:
            case MavParamType.MavParamTypeInt64:
            case MavParamType.MavParamTypeReal64:
            default:
                throw new ArgumentOutOfRangeException(nameof(Type));
        }
    }
    
    public void WriteToConfig(IConfiguration config, MavParamValue value,string prefix = null)
    {
        var key = CombineConfigKey(prefix, Name);
        switch (value.Type)
        {
            case MavParamType.MavParamTypeUint8:
                config.Set(key, (byte)value);
                break;
            case MavParamType.MavParamTypeInt8:
                config.Set(key, (sbyte)value);
                break;
            case MavParamType.MavParamTypeUint16:
                config.Set(key, (ushort)value);
                break;
            case MavParamType.MavParamTypeInt16:
                config.Set(key, (short)value);
                break;
            case MavParamType.MavParamTypeUint32:
                config.Set(key, (uint)value);
                break;
            case MavParamType.MavParamTypeInt32:
                config.Set(key, (int)value);
                break;
            case MavParamType.MavParamTypeReal32:
                config.Set(key, (float)value);
                break;
            case MavParamType.MavParamTypeUint64:
            case MavParamType.MavParamTypeInt64:
            case MavParamType.MavParamTypeReal64:
            default:
                throw new ArgumentOutOfRangeException(nameof(value), value, null);
        }
    }
}

/// <summary>
/// Represents the metadata associated with a MAVLink parameter 
/// </summary>
public class MavParamTypeMetadata : IMavParamTypeMetadata
{
    /// <summary>
    /// Represents the metadata for a MAVLink parameter 
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="type">The type of the parameter.</param>
    public MavParamTypeMetadata(string name, MavParamType type)
    {
        MavParamHelper.CheckParamName(name);
        Name = name;
        Type = type;
    }

    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    /// <value>
    /// The name of the property.
    /// </value>
    public string Name { get; }

    /// <summary>
    /// Gets the type of the property.
    /// </summary>
    /// <returns>The type of the property.</returns>
    public MavParamType Type { get; }

    /// <summary>
    /// Gets or sets the group of the object.
    /// </summary>
    /// <value>
    /// The group of the object as a string.
    /// </value>
    public string Group { get; set; }

    /// <summary>
    /// Gets or sets the category of the property.
    /// </summary>
    /// <value>
    /// The category of the property.
    /// </value>
    public string Category { get; set; }

    /// <summary>
    /// Gets or sets the short description of a property.
    /// </summary>
    /// <value>
    /// The short description.
    /// </value>
    public string ShortDesc { get; set; }

    /// <summary>
    /// Gets or sets the long description of the property.
    /// </summary>
    /// <value>
    /// The long description of the property.
    /// </value>
    public string LongDesc { get; set; }

    /// <summary>
    /// Gets or sets the units of the property.
    /// </summary>
    /// <value>
    /// The units of the property.
    /// </value>
    public string Units { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a reboot is required.
    /// </summary>
    /// <value>
    /// <c>true</c> if a reboot is required; otherwise, <c>false</c>.
    /// </value>
    public bool RebootRequired { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the property is volatile.
    /// </summary>
    /// <value>
    /// <c>true</c> if the property is volatile; otherwise, <c>false</c>.
    /// </value>
    public bool Volatile { get; set; }

    /// <summary>
    /// Gets or sets the minimum value for the MavParamValue.
    /// </summary>
    public MavParamValue MinValue { get; set; }

    /// <summary>
    /// Gets or sets the maximum value for the MavParamValue.
    /// </summary>
    /// <value>
    /// The maximum value allowed for the MavParamValue.
    /// </value>
    public MavParamValue MaxValue { get; set; }

    /// <summary>
    /// Gets or sets the default value for <see cref="DefaultValue"/>.
    /// </summary>
    /// <value>
    /// The default value.
    /// </value>
    public MavParamValue DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets the value of the Increment property.
    /// </summary>
    /// <value>
    /// The value of the Increment property.
    /// </value>
    public MavParamValue Increment { get; set; }

    /// <summary>
    /// Gets or sets the array of property values.
    /// </summary>
    /// <value>
    /// The array of property values.
    /// </value>
    public (MavParamValue, string)[] Values { get; set; }

    /// <summary>
    /// Gets or sets the bitmask property.
    /// </summary>
    /// <value>
    /// An array of tuples containing a 32-bit unsigned integer and a MavParamValue.
    /// </value>
    public (uint, MavParamValue)[] Bitmask { get; set; }

    /// <summary>
    /// Determines whether the specified MavParamValue is valid.
    /// </summary>
    /// <param name="value">The MavParamValue to check.</param>
    /// <returns>True if the MavParamValue is valid; otherwise, false.</returns>
    public bool IsValid(MavParamValue value)
    {
        if (value.Type != Type) return false; 
        switch (Type)
        {
            case MavParamType.MavParamTypeUint8:
            case MavParamType.MavParamTypeInt8:
            case MavParamType.MavParamTypeUint16:
            case MavParamType.MavParamTypeInt16:
            case MavParamType.MavParamTypeUint32:
            case MavParamType.MavParamTypeInt32:
            case MavParamType.MavParamTypeReal32:
                if (value > MaxValue)
                {
                    return false;
                }
                if (value < MinValue)
                {
                    return false;
                }
                break;
            case MavParamType.MavParamTypeUint64:
            case MavParamType.MavParamTypeInt64:
            case MavParamType.MavParamTypeReal64:
            default:
                return false;
        }
        return true;
    }

    /// <summary>
    /// Returns a validation error message if the provided <paramref name="value"/> is not valid for the current instance of <see cref="MavParamValue"/>.
    /// </summary>
    /// <param name="value">The <see cref="MavParamValue"/> instance to be validated.</param>
    /// <returns>
    /// Returns a validation error message if the provided <paramref name="value"/> is not valid, or null if the value is valid.
    /// </returns>
    public string GetValidationError(MavParamValue value)
    {
        if (value.Type != Type) return $"Type not equals {value.Type:G} != {Type:G}";
        switch (Type)
        {
            case MavParamType.MavParamTypeUint8:
            case MavParamType.MavParamTypeInt8:
            case MavParamType.MavParamTypeUint16:
            case MavParamType.MavParamTypeInt16:
            case MavParamType.MavParamTypeUint32:
            case MavParamType.MavParamTypeInt32:
            case MavParamType.MavParamTypeReal32:
                if (value > MaxValue)
                {
                    return $"must be <'{MaxValue}'";
                }
                if (value < MinValue)
                {
                    return $"must be >'{MinValue}'";
                }
                break;
            case MavParamType.MavParamTypeUint64:
            case MavParamType.MavParamTypeInt64:
            case MavParamType.MavParamTypeReal64:
            default:
                return $"wrong type {Type}";
        }
        return null;
    }
}