using System;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;


public static class MavParamFactory
{
    
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
    /// If 'min' is not specified the minimum value is the minimum numeric value which can be represented by the type.
    /// </summary>
    MavParamValue MinValue { get; }
    /// <summary>
    /// Maximum valid value
    /// If 'max' is not specified the minimum value is the maximum numeric value which can be represented by the type.
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

    bool IsValid(MavParamValue newValue);
    string GetValidationError(MavParamValue newValue);
}

public class MavParamTypeMetadata : IMavParamTypeMetadata
{
    public MavParamTypeMetadata(string name, MavParamType type)
    {
        MavParamHelper.CheckParamName(name);
        Name = name;
        Type = type;
    }

    public string Name { get; }
    public MavParamType Type { get; }
    public string Group { get; set; }
    public string Category { get; set; }
    public string ShortDesc { get; set; }
    public string LongDesc { get; set; }
    public string Units { get; set; }
    public bool RebootRequired { get; set; }
    public bool Volatile { get; set; }
    public MavParamValue MinValue { get; set; }
    public MavParamValue MaxValue { get; set; }
    public MavParamValue DefaultValue { get; set; }
    public MavParamValue Increment { get; set; }
    public (MavParamValue, string)[] Values { get; set; }
    public (uint, MavParamValue)[] Bitmask { get; set; }
    public bool IsValid(MavParamValue newValue)
    {
        // TODO: implement validation
        return true;
    }

    public string GetValidationError(MavParamValue newValue)
    {
        // TODO: implement validation
        return null;
    }
}