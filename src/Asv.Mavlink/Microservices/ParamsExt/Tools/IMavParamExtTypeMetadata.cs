using System;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface IMavParamExtTypeMetadata
{
    /// <summary>
    /// Parameter Name
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Parameter type
    /// </summary>
    MavParamExtType Type { get; }
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
    MavParamExtValue MinValue { get; }
    /// <summary>
    /// Maximum valid value
    /// If 'max' is not specified the minimum value is the maximum numeric value which can be represented by the 
    /// </summary>
    MavParamExtValue MaxValue { get; }
    /// <summary>
    /// Default value for parameter.
    /// If a defaultValue is not specified, there is no default for the parameter available at all. A GCS should not provide an option to reset this parameter to default.
    /// </summary>
    MavParamExtValue DefaultValue { get; }
    /// <summary>
    /// Increment to use for user facing UI which increments a value.
    /// </summary>
    MavParamExtValue Increment { get; }
    /// <summary>
    /// Array of values and textual descriptions for use by GCS ui.
    /// </summary>
    (MavParamExtValue,string)[] Values { get; }
    /// <summary>
    /// Bitmask of allowed values.
    /// </summary>
    (uint,MavParamExtValue)[] Bitmask { get; }

    /// <summary>
    /// Validates the given MavParamExtValue.
    /// </summary>
    /// <param name="newValue">The MavParamExtValue to validate.</param>
    /// <returns>True if the MavParamExtValue is valid, False otherwise.</returns>
    bool IsValid(MavParamExtValue newValue);

    /// <summary>
    /// Validates a MAV parameter value and returns an error message if validation fails. </summary> <param name="newValue">The new value to be validated.</param> <returns>A string containing the error message if validation fails; otherwise, an empty string.</returns>
    /// /
    string GetValidationError(MavParamExtValue newValue);
    
    private static string CombineConfigKey(string? prefix, string name)
    {
        return string.IsNullOrWhiteSpace(prefix) ? name : $"{prefix}{name}";
    }
    
    public MavParamExtValue ReadFromConfig(IConfiguration config, string? prefix = null)
    {
        var key = CombineConfigKey(prefix, Name);
        switch (Type)
        {
            case MavParamExtType.MavParamExtTypeUint8:
                return new MavParamExtValue(config.Get(key, new System.Lazy<byte>(()=>DefaultValue)));
            case MavParamExtType.MavParamExtTypeInt8:
                return new MavParamExtValue(config.Get(key, new System.Lazy<sbyte>(()=>DefaultValue)));
            case MavParamExtType.MavParamExtTypeUint16:
                return new MavParamExtValue(config.Get(key, new System.Lazy<ushort>(DefaultValue)));
            case MavParamExtType.MavParamExtTypeInt16:
                return new MavParamExtValue(config.Get(key,  new System.Lazy<short>(DefaultValue)));
            case MavParamExtType.MavParamExtTypeUint32:
                return new MavParamExtValue(config.Get(key,  new System.Lazy<uint>(DefaultValue)));
            case MavParamExtType.MavParamExtTypeInt32:
                return new MavParamExtValue(config.Get(key,  new System.Lazy<int>(DefaultValue)));
            case MavParamExtType.MavParamExtTypeReal32:
                return new MavParamExtValue(config.Get(key,  new System.Lazy<float>(DefaultValue)));
            case MavParamExtType.MavParamExtTypeUint64:
                return new MavParamExtValue(config.Get(key, new System.Lazy<ulong>(DefaultValue)));
            case MavParamExtType.MavParamExtTypeInt64:
                return new MavParamExtValue(config.Get(key, new System.Lazy<long>(DefaultValue)));
            case MavParamExtType.MavParamExtTypeReal64:
                return new MavParamExtValue(config.Get(key, new System.Lazy<double>(DefaultValue)));
            case MavParamExtType.MavParamExtTypeCustom:
                return new MavParamExtValue(config.Get(key, new System.Lazy<char[]>(DefaultValue)));
            default:
                throw new ArgumentOutOfRangeException(nameof(Type));
        }
    }
    
    public void WriteToConfig(IConfiguration config, MavParamExtValue value,string? prefix = null)
    {
        var key = CombineConfigKey(prefix, Name);
        switch (value.Type)
        {
            case MavParamExtType.MavParamExtTypeUint8:
                config.Set(key, (byte)value);
                break;
            case MavParamExtType.MavParamExtTypeInt8:
                config.Set(key, (sbyte)value);
                break;
            case MavParamExtType.MavParamExtTypeUint16:
                config.Set(key, (ushort)value);
                break;
            case MavParamExtType.MavParamExtTypeInt16:
                config.Set(key, (short)value);
                break;
            case MavParamExtType.MavParamExtTypeUint32:
                config.Set(key, (uint)value);
                break;
            case MavParamExtType.MavParamExtTypeInt32:
                config.Set(key, (int)value);
                break;
            case MavParamExtType.MavParamExtTypeReal32:
                config.Set(key, (float)value);
                break;
            case MavParamExtType.MavParamExtTypeUint64:
                config.Set(key, (ulong)value);
                break;
            case MavParamExtType.MavParamExtTypeInt64:
                config.Set(key, (long)value);
                break;
            case MavParamExtType.MavParamExtTypeReal64:
                config.Set(key, (double)value);
                break;
            case MavParamExtType.MavParamExtTypeCustom:
                config.Set(key, (char[])value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(value), value, null);
        }
    }
}

/// <summary>
/// Represents the metadata associated with a MAVLink parameter 
/// </summary>
public class MavParamExtTypeMetadata : IMavParamExtTypeMetadata
{
    /// <summary>
    /// Represents the metadata for a MAVLink parameter 
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="type">The type of the parameter.</param>
    public MavParamExtTypeMetadata(string name, MavParamExtType type)
    {
        MavParamExtHelper.CheckParamName(name);
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
    public MavParamExtType Type { get; }

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
    /// Gets or sets the minimum value for the MavParamExtValue.
    /// </summary>
    public MavParamExtValue MinValue { get; set; }

    /// <summary>
    /// Gets or sets the maximum value for the MavParamExtValue.
    /// </summary>
    /// <value>
    /// The maximum value allowed for the MavParamExtValue.
    /// </value>
    public MavParamExtValue MaxValue { get; set; }

    /// <summary>
    /// Gets or sets the default value for <see cref="DefaultValue"/>.
    /// </summary>
    /// <value>
    /// The default value.
    /// </value>
    public MavParamExtValue DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets the value of the Increment property.
    /// </summary>
    /// <value>
    /// The value of the Increment property.
    /// </value>
    public MavParamExtValue Increment { get; set; }

    /// <summary>
    /// Gets or sets the array of property values.
    /// </summary>
    /// <value>
    /// The array of property values.
    /// </value>
    public (MavParamExtValue, string)[] Values { get; set; }

    /// <summary>
    /// Gets or sets the bitmask property.
    /// </summary>
    /// <value>
    /// An array of tuples containing a 32-bit unsigned integer and a MavParamExtValue.
    /// </value>
    public (uint, MavParamExtValue)[] Bitmask { get; set; }

    /// <summary>
    /// Determines whether the specified MavParamExtValue is valid.
    /// </summary>
    /// <param name="value">The MavParamExtValue to check.</param>
    /// <returns>True if the MavParamExtValue is valid; otherwise, false.</returns>
    public bool IsValid(MavParamExtValue value)
    {
        if (value.Type != Type) return false; 
        if (value < MinValue || value > MaxValue)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Returns a validation error message if the provided <paramref name="value"/> is not valid for the current instance of <see cref="MavParamExtValue"/>.
    /// </summary>
    /// <param name="value">The <see cref="MavParamExtValue"/> instance to be validated.</param>
    /// <returns>
    /// Returns a validation error message if the provided <paramref name="value"/> is not valid, or null if the value is valid.
    /// </returns>
    public string GetValidationError(MavParamExtValue value)
    {
        if (value.Type != Type) return $"Type not equals {value.Type:G} != {Type:G}";
        if (value > MaxValue)
        {
            return $"must be <'{MaxValue}'";
        }
        if (value < MinValue)
        {
            return $"must be >'{MinValue}'";
        }
        return null;
    }
}