using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.Common;


namespace Asv.Mavlink;

/// <summary>
/// A factory class for creating instances of <see cref="IMavParamTypeMetadata"/> objects.
/// </summary>
public static class MavParam
{
    public const string Advanced = "Advanced";
    public const string System = "System";
    public const string Developer = "Developer";

    public static IMavParamTypeMetadata SysU8AsCommand(string name, string desc) => U8AsCommand(System, name, desc); 
    public static IMavParamTypeMetadata AdvU8AsCommand(string name, string desc) => U8AsCommand(Advanced, name, desc);
    public static IMavParamTypeMetadata DevU8AsCommand(string name, string desc) => U8AsCommand(Developer, name, desc);
    
    public static IMavParamTypeMetadata U8AsCommand(string category, string name, string desc)
    {
        return new MavParamTypeMetadata(name, MavParamType.MavParamTypeUint8)
        {
            ShortDesc = desc,
            LongDesc = desc,
            Category = category,
            RebootRequired = false,
            Units = string.Empty,
            Increment = new MavParamValue((byte)1),
            MinValue = new((byte)0),
            DefaultValue = new((byte)0),
            MaxValue = new((byte)1),
            Volatile = true,
        };
    }
    
    public static IMavParamTypeMetadata SysU8AsEnum<TEnum>(string name,string desc,TEnum def = default, bool vlt = false)
        where TEnum : struct, Enum => U8AsEnum(Developer, name, desc, def,vlt);
    public static IMavParamTypeMetadata AdvU8AsEnum<TEnum>(string name,string desc,TEnum def = default, bool vlt = false)
        where TEnum : struct, Enum => U8AsEnum(Developer, name, desc, def,vlt);

    public static IMavParamTypeMetadata DevU8AsEnum<TEnum>(string name, string desc, TEnum def = default, bool vlt = false)
        where TEnum : struct, Enum => U8AsEnum(Developer, name, desc, def,vlt);
    public static IMavParamTypeMetadata U8AsEnum<TEnum>(string category,string name,string desc,TEnum def = default, bool vlt = false)
        where TEnum : struct, Enum
    {
        if(!typeof(TEnum).IsEnum)
        {
            throw new ArgumentException(nameof(TEnum));
        }
        
        var list = new List<(MavParamValue, string)>();
        foreach (var value in Enum.GetValues<TEnum>())
        {
            var val = new MavParamValue(Convert.ToByte(value));
            var valueName = value.GetAttributeOfType<DisplayAttribute>()?.Name ?? Enum.GetName(value) ?? value.ToString();
            
            list.Add(new ValueTuple<MavParamValue, string>(val,valueName));
        }
        
        return new MavParamTypeMetadata(name, MavParamType.MavParamTypeUint8)
        {
            ShortDesc = desc,
            LongDesc = desc,
            Category = category,
            RebootRequired = false,
            Units = string.Empty,
            Increment = new MavParamValue((byte)1),
            MinValue = new((byte)list.Min(a => a.Item1)),
            DefaultValue = new MavParamValue(Convert.ToByte(def)),
            MaxValue = new((byte)list.Max(a => a.Item1)),
            Values = list.ToArray(),
            Volatile = vlt,
            
        };
    }
    
    public static IMavParamTypeMetadata SysU8AsBool(string name,string desc,bool def = true, bool vlt = false) 
        => U8AsBool(System, name, desc, def,vlt); 
    public static IMavParamTypeMetadata AdvU8AsBool(string name,string desc,bool def = true, bool vlt = false) 
        => U8AsBool(Advanced, name, desc, def,vlt);
    public static IMavParamTypeMetadata DevU8AsBool(string name,string desc,bool def = true, bool vlt = false) 
        => U8AsBool(Developer, name, desc, def,vlt);
    
    public static IMavParamTypeMetadata U8AsBool(string category,string name,string desc,bool def = true, bool vlt = false)
    {
        return new MavParamTypeMetadata(name, MavParamType.MavParamTypeUint8)
        {
            ShortDesc = desc,
            LongDesc = desc,
            Category = category,
            RebootRequired = false,
            Units = string.Empty,
            Increment = new MavParamValue((byte)1),
            MinValue = new((byte)0),
            DefaultValue = new(def ? (byte)1 : (byte)0),
            MaxValue = new((byte)1),
            Volatile = vlt,
        };
    }

    public static IMavParamTypeMetadata SysS32(string name, string desc, string unit, int def = 0,
        int min = int.MinValue, int max = int.MaxValue, int inc = 1, bool vlt = false)
        => S32(System, name, desc, unit, def, min, max, inc,vlt);
    
    public static IMavParamTypeMetadata AdvS32(string name, string desc, string unit, int def = 0,
        int min = int.MinValue, int max = int.MaxValue, int inc = 1, bool vlt = false)
        => S32(Advanced, name, desc, unit, def, min, max, inc,vlt);
    
    public static IMavParamTypeMetadata DevS32(string name, string desc, string unit, int def = 0,
        int min = int.MinValue, int max = int.MaxValue, int inc = 1, bool vlt = false)
        => S32(Developer, name, desc, unit, def, min, max, inc,vlt);
    
    public static IMavParamTypeMetadata S32(string category,string name,string desc,string unit,int def = 0,
        int min = int.MinValue,int max = int.MaxValue, int inc = 1, bool vlt = false)
    {
        return new MavParamTypeMetadata(name, MavParamType.MavParamTypeInt32)
        {
            ShortDesc = desc,
            LongDesc = desc,
            Category = category,
            RebootRequired = false,
            Units = unit,
            Increment = new MavParamValue(inc),
            MinValue = new(min),
            DefaultValue = new(def),
            MaxValue = new(max),
            Volatile = vlt,
        };
    }

    public static IMavParamTypeMetadata SysU32(string name, string desc, string unit, uint def = 0,
        uint min = uint.MinValue, uint max = uint.MaxValue, uint inc = 1, bool vlt = false)
        => U32(System, name, desc, unit, def, min, max, inc,vlt);
    
    public static IMavParamTypeMetadata AdvU32(string name, string desc, string unit, uint def = 0,
        uint min = uint.MinValue, uint max = uint.MaxValue, uint inc = 1, bool vlt = false)
        => U32(Advanced, name, desc, unit, def, min, max, inc,vlt);
    
    public static IMavParamTypeMetadata DevU32(string name, string desc, string unit, uint def = 0,
        uint min = uint.MinValue, uint max = uint.MaxValue, uint inc = 1, bool vlt = false)
        => U32(Developer, name, desc, unit, def, min, max, inc,vlt);
    
    public static IMavParamTypeMetadata U32(string category,string name,string desc,string unit,
        uint def = 0,uint min = uint.MinValue,uint max = uint.MaxValue, uint inc = 1, bool vlt = false)
    {
        return new MavParamTypeMetadata(name, MavParamType.MavParamTypeUint32)
        {
            ShortDesc = desc,
            LongDesc = desc,
            Category = category,
            RebootRequired = false,
            Units = unit,
            Increment = new(inc),
            MinValue = new(min),
            DefaultValue = new(def),
            MaxValue = new(max),
            Volatile = vlt, 
        };
    }

    public static IMavParamTypeMetadata SysU16(string name, string desc, string unit, ushort def = 0,
        ushort min = ushort.MinValue, ushort max = ushort.MaxValue, ushort inc = 1, bool vlt = false)
        => U16(System, name, desc, unit, def, min, max, inc,vlt);
    public static IMavParamTypeMetadata AdvU16(string name, string desc, string unit, ushort def = 0,
        ushort min = ushort.MinValue, ushort max = ushort.MaxValue, ushort inc = 1, bool vlt = false)
        => U16(Advanced, name, desc, unit, def, min, max, inc,vlt);
    public static IMavParamTypeMetadata DevU16(string name, string desc, string unit, ushort def = 0,
        ushort min = ushort.MinValue, ushort max = ushort.MaxValue, ushort inc = 1, bool vlt = false)
        => U16(Developer, name, desc, unit, def, min, max, inc,vlt);
    public static IMavParamTypeMetadata U16(string category,string name,string desc,string unit,ushort def = 0,
        ushort min = ushort.MinValue,ushort max = ushort.MaxValue, ushort inc = 1, bool vlt = false)
    {
        return new MavParamTypeMetadata(name, MavParamType.MavParamTypeUint16)
        {
            ShortDesc = desc,
            LongDesc = desc,
            Category = category,
            RebootRequired = false,
            Units = unit,
            Increment = new(inc),
            MinValue = new(min),
            DefaultValue = new(def),
            MaxValue = new(max),
            Volatile = vlt,
        };
    }

    public static IMavParamTypeMetadata SysS16(string name, string desc, string unit, short def = 0,
        short min = short.MinValue, short max = short.MaxValue, short inc = 1, bool vlt = false)
        => S16(System, name, desc, unit, def, min, max, inc, vlt);
    public static IMavParamTypeMetadata AdvS16(string name, string desc, string unit, short def = 0,
        short min = short.MinValue, short max = short.MaxValue, short inc = 1, bool vlt = false)
        => S16(Advanced, name, desc, unit, def, min, max, inc, vlt);
    public static IMavParamTypeMetadata DevS16(string name, string desc, string unit, short def = 0,
        short min = short.MinValue, short max = short.MaxValue, short inc = 1, bool vlt = false)
        => S16(Developer, name, desc, unit, def, min, max, inc,vlt);
    public static IMavParamTypeMetadata S16(string category,string name,string desc,string unit,short def = 0,
        short min = short.MinValue,short max = short.MaxValue, short inc = 1, bool vlt = false)
    {
        return new MavParamTypeMetadata(name, MavParamType.MavParamTypeInt16)
        {
            ShortDesc = desc,
            LongDesc = desc,
            Category = category,
            RebootRequired = false,
            Units = unit,
            Increment = new(inc),
            MinValue = new(min),
            DefaultValue = new(def),
            MaxValue = new(max),
            Volatile = vlt,
        };
    }

    public static IMavParamTypeMetadata SysU8(string name, string desc, string unit, byte def = 0,
        byte min = byte.MinValue, byte max = byte.MaxValue, byte inc = 1, bool vlt = false)
        => U8(System, name, desc, unit, def, min, max, inc, vlt);
    public static IMavParamTypeMetadata AdvU8(string name, string desc, string unit, byte def = 0,
        byte min = byte.MinValue, byte max = byte.MaxValue, byte inc = 1, bool vlt = false)
        => U8(Advanced, name, desc, unit, def, min, max, inc, vlt);
    public static IMavParamTypeMetadata DevU8(string name, string desc, string unit, byte def = 0,
        byte min = byte.MinValue, byte max = byte.MaxValue, byte inc = 1, bool vlt = false)
        => U8(Developer, name, desc, unit, def, min, max, inc, vlt);
    public static IMavParamTypeMetadata U8(string category,string name,string desc,string unit,byte def = 0,
        byte min = byte.MinValue,byte max = byte.MaxValue, byte inc = 1, bool vlt = false)
    {
        return new MavParamTypeMetadata(name, MavParamType.MavParamTypeUint8)
        {
            ShortDesc = desc,
            LongDesc = desc,
            Category = category,
            RebootRequired = false,
            Units = unit,
            Increment = new(inc),
            MinValue = new(min),
            DefaultValue = new(def),
            MaxValue = new(max),
            Volatile = vlt,
        };
    }

    public static IMavParamTypeMetadata SysS8(string name, string desc, string unit, sbyte def = 0,
        sbyte min = sbyte.MinValue, sbyte max = sbyte.MaxValue, sbyte inc = 1, bool vlt = false)
        => S8(System, name, desc, unit, def, min, max, inc,vlt);
    public static IMavParamTypeMetadata AdvS8(string name, string desc, string unit, sbyte def = 0,
        sbyte min = sbyte.MinValue, sbyte max = sbyte.MaxValue, sbyte inc = 1, bool vlt = false)
        => S8(Advanced, name, desc, unit, def, min, max, inc,vlt);
    public static IMavParamTypeMetadata DevS8(string name, string desc, string unit, sbyte def = 0,
        sbyte min = sbyte.MinValue, sbyte max = sbyte.MaxValue, sbyte inc = 1, bool vlt = false)
        => S8(Developer, name, desc, unit, def, min, max, inc,vlt);
    public static IMavParamTypeMetadata S8(string category,string name,string desc,string unit,sbyte def = 0,
        sbyte min = sbyte.MinValue,sbyte max = sbyte.MaxValue, sbyte inc = 1, bool vlt = false)
    {
        return new MavParamTypeMetadata(name, MavParamType.MavParamTypeInt8)
        {
            ShortDesc = desc,
            LongDesc = desc,
            Category = category,
            RebootRequired = false,
            Units = unit,
            Increment = new(inc),
            MinValue = new(min),
            DefaultValue = new(def),
            MaxValue = new(max),
            Volatile = vlt,
        };
    }

    public static IMavParamTypeMetadata SysR32(string name, string desc, string unit, float def = 0.0f,
        float min = float.MinValue, float max = float.MaxValue, float inc = 1.0f, bool vlt = false)
        => R32(System, name, desc, unit, def, min, max, inc, vlt);
    public static IMavParamTypeMetadata AdvR32(string name, string desc, string unit, float def = 0.0f,
        float min = float.MinValue, float max = float.MaxValue, float inc = 1.0f, bool vlt = false)
        => R32(Advanced, name, desc, unit, def, min, max, inc, vlt);
    public static IMavParamTypeMetadata DevR32(string name, string desc, string unit, float def = 0.0f,
        float min = float.MinValue, float max = float.MaxValue, float inc = 1.0f, bool vlt = false)
        => R32(Developer, name, desc, unit, def, min, max, inc, vlt);
    public static IMavParamTypeMetadata R32(string category,string name,string desc,string unit,float def = 0.0f,
        float min = float.MinValue,float max = float.MaxValue, float inc = 1.0f, bool vlt = false)
    {
        return new MavParamTypeMetadata(name, MavParamType.MavParamTypeReal32)
        {
            ShortDesc = desc,
            LongDesc = desc,
            Category = category,
            RebootRequired = false,
            Units = unit,
            Increment = new(inc),
            MinValue = new(min),
            DefaultValue = new(def),
            MaxValue = new(max),
            Volatile = vlt,
        };
    }
    
    public static IMavParamTypeMetadata SysU32AsString(string name,string desc, uint def = 0, bool vlt = false)
        => U32AsString(System, name, desc, def,vlt);
    public static IMavParamTypeMetadata AdvU32AsString(string name,string desc, uint def = 0, bool vlt = false)
        => U32AsString(Advanced, name, desc, def,vlt);

    public static IMavParamTypeMetadata DevU32AsString(string name, string desc, uint def = 0, bool vlt = false)
        => U32AsString(Developer, name, desc, def,vlt);
    public static IMavParamTypeMetadata U32AsString(string category,string name,string desc, uint def = 0x00000000, bool vlt = false)
    {
        // ASCII [0x00000000 : 0x7F7F7F7F]  ("[NULL][NULL][NULL][NULL] : [DEL][DEL][DEL][DEL]")
        if (def > 0x7F7F7F7F) throw new ArgumentException(nameof(def));
        
        return new MavParamTypeMetadata(name, MavParamType.MavParamTypeUint32)
        {
            ShortDesc = desc,
            LongDesc = desc,
            Category = category,
            RebootRequired = false,
            Units = "ABCD",
            Increment = new MavParamValue((uint)0x00000001),
            MinValue = new MavParamValue((uint)0x00000000),
            DefaultValue = new MavParamValue(def),
            MaxValue = new MavParamValue((uint)0x7F7F7F7F),
            Volatile = vlt
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
    string? Group { get; }
    /// <summary>
    /// User readable name for a 'type' of parameter. For example 'Developer', 'System', or 'Advanced'.
    /// </summary>
    string? Category { get; }
    /// <summary>
    /// Short user facing description/name for parameter. Used in UI instead of internal parameter name.
    /// </summary>
    string? ShortDesc { get; }
    /// <summary>
    /// Long user facing documentation of how the parameters works.
    /// </summary>
    string? LongDesc { get; }
    /// <summary>
    /// Units for parameter value.
    /// </summary>
    string? Units { get; }
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
    (MavParamValue, string)[]? Values { get; }
    /// <summary>
    /// Bitmask of allowed values.
    /// </summary>
    (uint, MavParamValue)[]? Bitmask { get; }

    /// <summary>
    /// Validates the given MavParamValue.
    /// </summary>
    /// <param name="newValue">The MavParamValue to validate.</param>
    /// <returns>True if the MavParamValue is valid, False otherwise.</returns>
    bool IsValid(MavParamValue newValue);

    /// <summary>
    /// Validates a MAV parameter value and returns an error message if validation fails. </summary> <param name="newValue">The new value to be validated.</param> <returns>A string containing the error message if validation fails; otherwise, an empty string.</returns>
    /// /
    string? GetValidationError(MavParamValue newValue);
    
    private static string CombineConfigKey(string prefix, string name)
    {
        ArgumentNullException.ThrowIfNull(prefix);
        return prefix.IsNullOrWhiteSpace() ? name : $"{prefix}{name}";
    }
    
    public MavParamValue ReadFromConfig(IConfiguration config, string? prefix = null)
    {
        var key = CombineConfigKey(prefix ?? string.Empty, Name);
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
    
    public void WriteToConfig(IConfiguration config, MavParamValue value,string? prefix = null)
    {
        var key = CombineConfigKey(prefix ?? string.Empty, Name);
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
    public string? Group { get; set; }

    /// <summary>
    /// Gets or sets the category of the property.
    /// </summary>
    /// <value>
    /// The category of the property.
    /// </value>
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the short description of a property.
    /// </summary>
    /// <value>
    /// The short description.
    /// </value>
    public string? ShortDesc { get; set; }

    /// <summary>
    /// Gets or sets the long description of the property.
    /// </summary>
    /// <value>
    /// The long description of the property.
    /// </value>
    public string? LongDesc { get; set; }

    /// <summary>
    /// Gets or sets the units of the property.
    /// </summary>
    /// <value>
    /// The units of the property.
    /// </value>
    public string? Units { get; set; }

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
    public (MavParamValue, string)[]? Values { get; set; }

    /// <summary>
    /// Gets or sets the bitmask property.
    /// </summary>
    /// <value>
    /// An array of tuples containing a 32-bit unsigned integer and a MavParamValue.
    /// </value>
    public (uint, MavParamValue)[]? Bitmask { get; set; }

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
    public string? GetValidationError(MavParamValue value)
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

