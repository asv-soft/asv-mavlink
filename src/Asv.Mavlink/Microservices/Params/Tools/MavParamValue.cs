using System;
using System.Diagnostics;
using System.Globalization;
using Asv.Common;
using Asv.Mavlink.Common;


namespace Asv.Mavlink;

public readonly struct MavParamValue: IComparable<MavParamValue>, IComparable,IEquatable<MavParamValue>
{
    private readonly float? _realValue;
    private readonly long? _intValue;
    public MavParamValue(byte value)
    {
        Type = MavParamType.MavParamTypeUint8;
        _intValue = value;
    }
   
    public MavParamValue(sbyte value)
    {
        Type = MavParamType.MavParamTypeInt8;
        _intValue = value;
    }

    public MavParamValue(short value)
    {
        Type = MavParamType.MavParamTypeInt16;
        _intValue = value;
    }
    
    public MavParamValue(ushort value)
    {
        Type = MavParamType.MavParamTypeUint16;
        _intValue = value;
    }
    
    public MavParamValue(int value)
    {
        Type = MavParamType.MavParamTypeInt32;
        _intValue = value;
    }
    
    public MavParamValue(uint value)
    {
        Type = MavParamType.MavParamTypeUint32;
        _intValue = value;
    }
    
    public MavParamValue(float value)
    {
        Type = MavParamType.MavParamTypeReal32;
        _realValue = value;
    }

    public static implicit operator MavParamValue(int x)
    {
        return new MavParamValue(x);
    }
    
    public static implicit operator MavParamValue(uint x)
    {
        return new MavParamValue(x);
    }
    
    public static implicit operator MavParamValue(short x)
    {
        return new MavParamValue(x);
    }
    
    public static implicit operator MavParamValue(ushort x)
    {
        return new MavParamValue(x);
    }
    
    public static implicit operator MavParamValue(byte x)
    {
        return new MavParamValue(x);
    }
    
    public static implicit operator MavParamValue(sbyte x)
    {
        return new MavParamValue(x);
    }
    
    public static implicit operator MavParamValue(float x)
    {
        return new MavParamValue(x);
    }
    
    public static implicit operator int(MavParamValue x)
    {
        if (x.Type == MavParamType.MavParamTypeReal32) throw new InvalidOperationException();
        if (x._intValue != null) return (int)x._intValue;
        if (x._realValue != null) return (int)x._realValue;
        throw new InvalidOperationException();
    }
    
    public static implicit operator uint(MavParamValue x)
    {
        if (x.Type == MavParamType.MavParamTypeReal32) throw new InvalidOperationException();
        if (x._intValue != null) return (uint)x._intValue;
        if (x._realValue != null) return (uint)x._realValue;
        throw new InvalidOperationException();
    }
    
    public static implicit operator short(MavParamValue x)
    {
        if (x.Type == MavParamType.MavParamTypeReal32) throw new InvalidOperationException();
        if (x._intValue != null) return (short)x._intValue;
        if (x._realValue != null) return (short)x._realValue;
        throw new InvalidOperationException();
    }
    
    public static implicit operator ushort(MavParamValue x)
    {
        if (x.Type == MavParamType.MavParamTypeReal32) throw new InvalidOperationException();
        if (x._intValue != null) return (ushort)x._intValue;
        if (x._realValue != null) return (ushort)x._realValue;
        throw new InvalidOperationException();
    }
    
    public static implicit operator byte(MavParamValue x)
    {
        if (x.Type == MavParamType.MavParamTypeReal32) throw new InvalidOperationException();
        if (x._intValue != null) return (byte)x._intValue;
        if (x._realValue != null) return (byte)x._realValue;
        throw new InvalidOperationException();
    }
    
    public static implicit operator sbyte(MavParamValue x)
    {
        if (x.Type == MavParamType.MavParamTypeReal32) throw new InvalidOperationException();
        if (x._intValue != null) return (sbyte)x._intValue;
        if (x._realValue != null) return (sbyte)x._realValue;
        throw new InvalidOperationException();
    }
    
    public static implicit operator float(MavParamValue x)
    {
        if (x.Type != MavParamType.MavParamTypeReal32) throw new InvalidOperationException();
        if (x._intValue != null) return (float)x._intValue;
        if (x._realValue != null) return (float)x._realValue;
        throw new InvalidOperationException();
    }
    
    public MavParamType Type { get; }

    public int CompareTo(MavParamValue other)
    {
        var realValueComparison = Nullable.Compare(_realValue, other._realValue);
        if (realValueComparison != 0) return realValueComparison;
        var intValueComparison = Nullable.Compare(_intValue, other._intValue);
        if (intValueComparison != 0) return intValueComparison;
        return 0;
    }

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        return obj is MavParamValue other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(MavParamValue)}");
    }

    public static bool operator <(MavParamValue left, MavParamValue right)
    {
        if (left.Type != right.Type &
            (left.Type == MavParamType.MavParamTypeReal32 | 
             right.Type == MavParamType.MavParamTypeReal32)) throw new InvalidOperationException();
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(MavParamValue left, MavParamValue right)
    {
        if (left.Type != right.Type &
            (left.Type == MavParamType.MavParamTypeReal32 | 
             right.Type == MavParamType.MavParamTypeReal32)) throw new InvalidOperationException();
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(MavParamValue left, MavParamValue right)
    {
        if (left.Type != right.Type &
            (left.Type == MavParamType.MavParamTypeReal32 | 
             right.Type == MavParamType.MavParamTypeReal32)) throw new InvalidOperationException();
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(MavParamValue left, MavParamValue right)
    {
        if (left.Type != right.Type &
            (left.Type == MavParamType.MavParamTypeReal32 | 
             right.Type == MavParamType.MavParamTypeReal32)) throw new InvalidOperationException();
        return left.CompareTo(right) >= 0;
    }

    public bool Equals(MavParamValue other)
    {
        return Nullable.Equals(_realValue, other._realValue) && _intValue == other._intValue && Type == other.Type;
    }

    public override bool Equals(object? obj)
    {
        return obj is MavParamValue other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_realValue, _intValue, (int)Type);
    }

    public static bool operator ==(MavParamValue left, MavParamValue right)
    {
        if (left.Type != right.Type &
            (left.Type == MavParamType.MavParamTypeReal32 | 
             right.Type == MavParamType.MavParamTypeReal32)) throw new InvalidOperationException();
        return left.Equals(right);
    }

    public static bool operator !=(MavParamValue left, MavParamValue right)
    {
        if (left.Type != right.Type &
            (left.Type == MavParamType.MavParamTypeReal32 | 
             right.Type == MavParamType.MavParamTypeReal32)) throw new InvalidOperationException();
        return !left.Equals(right);
    }

    public string PrintValue(string? formatString = null)
    {
        if (formatString != null)
        {
            switch (Type)
            {
                case MavParamType.MavParamTypeUint8:
                    Debug.Assert(_intValue != null, nameof(_intValue) + " != null");
                    return ((byte)_intValue).ToString(formatString, CultureInfo.InvariantCulture);
                case MavParamType.MavParamTypeInt8:
                    Debug.Assert(_intValue != null, nameof(_intValue) + " != null");
                    return ((sbyte)_intValue).ToString(formatString, CultureInfo.InvariantCulture);
                case MavParamType.MavParamTypeUint16:
                    Debug.Assert(_intValue != null, nameof(_intValue) + " != null");
                    return ((ushort)_intValue).ToString(formatString, CultureInfo.InvariantCulture);
                case MavParamType.MavParamTypeInt16:
                    Debug.Assert(_intValue != null, nameof(_intValue) + " != null");
                    return ((short)_intValue).ToString(formatString, CultureInfo.InvariantCulture);
                case MavParamType.MavParamTypeUint32:
                    Debug.Assert(_intValue != null, nameof(_intValue) + " != null");
                    return ((uint)_intValue).ToString(formatString, CultureInfo.InvariantCulture);
                case MavParamType.MavParamTypeInt32:
                    Debug.Assert(_intValue != null, nameof(_intValue) + " != null");
                    return ((int)_intValue).ToString(formatString, CultureInfo.InvariantCulture);
                case MavParamType.MavParamTypeReal32:
                    Debug.Assert(_realValue != null, nameof(_realValue) + " != null");
                    return ((float)_realValue).ToString(formatString, CultureInfo.InvariantCulture);
                case MavParamType.MavParamTypeUint64:
                case MavParamType.MavParamTypeInt64:
                case MavParamType.MavParamTypeReal64:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        switch (Type)
        {
            case MavParamType.MavParamTypeUint8:
            case MavParamType.MavParamTypeInt8:
            case MavParamType.MavParamTypeUint16:
            case MavParamType.MavParamTypeInt16:
            case MavParamType.MavParamTypeUint32:
            case MavParamType.MavParamTypeInt32:
                return $"{_intValue}";
            case MavParamType.MavParamTypeReal32:
                return $"{_realValue}";
            case MavParamType.MavParamTypeUint64:
            case MavParamType.MavParamTypeInt64:
            case MavParamType.MavParamTypeReal64:
                return "Not supported";
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    public static string PrintType(MavParamType type)
    {
        return type switch
        {
            MavParamType.MavParamTypeUint8 => nameof(Byte),
            MavParamType.MavParamTypeInt8 => nameof(SByte),
            MavParamType.MavParamTypeUint16 => nameof(UInt16),
            MavParamType.MavParamTypeInt16 => nameof(Int16),
            MavParamType.MavParamTypeUint32 => nameof(UInt32),
            MavParamType.MavParamTypeInt32 => nameof(Int32),
            MavParamType.MavParamTypeReal32 => nameof(Single),
            MavParamType.MavParamTypeUint64 => nameof(UInt64),
            MavParamType.MavParamTypeInt64 => nameof(Int64),
            MavParamType.MavParamTypeReal64 => nameof(Double),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static ValidationResult TryParseValue(string? input, MavParamType type, out MavParamValue? value, MavParamValue min, MavParamValue max)
    {
        value = null;
        switch (type)
        {
            case MavParamType.MavParamTypeUint8:
                var minimum = Math.Max(byte.MinValue,min);
                var maximum = Math.Min(byte.MaxValue,max);
                var rc1 = InvariantNumberParser.TryParse(input, out int res1, minimum, maximum );
                if (rc1.IsSuccess)
                {
                    value = new MavParamValue((byte)res1);
                }

                return rc1;
            case MavParamType.MavParamTypeInt8:
                var minimum2 = Math.Max(sbyte.MinValue,min);
                var maximum2 = Math.Min(sbyte.MaxValue,max);
                var rc2 = InvariantNumberParser.TryParse(input, out int res2, minimum2, maximum2);
                if (rc2.IsSuccess)
                {
                    value = new MavParamValue((sbyte)res2);
                }

                return rc2;
            case MavParamType.MavParamTypeUint16:
                var minimum3 = Math.Max(ushort.MinValue,min);
                var maximum3 = Math.Min(ushort.MaxValue,max);
                var rc3 = InvariantNumberParser.TryParse(input, out int res3, minimum3, maximum3);
                if (rc3.IsSuccess)
                {
                    value = new MavParamValue((ushort)res3);
                }
                return rc3;
            case MavParamType.MavParamTypeInt16:
                var minimum4 = Math.Max(short.MinValue,min);
                var maximum4 = Math.Min(short.MaxValue,max);
                var rc4 = InvariantNumberParser.TryParse(input, out int res4, minimum4, maximum4);
                if (rc4.IsSuccess)
                {
                    value = new MavParamValue((short)res4);
                }
                return rc4;
            case MavParamType.MavParamTypeUint32:
                var minimum5 = Math.Max(uint.MinValue,min);
                var maximum5 = Math.Min(uint.MaxValue,max);
                var rc5 = InvariantNumberParser.TryParse(input, out uint res5, minimum5, maximum5);
                if (rc5.IsSuccess)
                {
                    value = new MavParamValue((uint)res5);
                }
                return rc5;
            case MavParamType.MavParamTypeInt32:
                var minimum6 = Math.Max(int.MinValue,min);
                var maximum6 = Math.Min(int.MaxValue,max);
                var rc6 = InvariantNumberParser.TryParse(input, out int res6, minimum6, maximum6);
                if (rc6.IsSuccess)
                {
                    value = new MavParamValue(res6);
                }

                return rc6;
            case MavParamType.MavParamTypeReal32:
                var rc7 = InvariantNumberParser.TryParse(input, out double res7, min, max);
                if (rc7.IsSuccess)
                {
                    value = new MavParamValue((float)res7);
                }
                return rc7;
            case MavParamType.MavParamTypeReal64:
                var rc8 = InvariantNumberParser.TryParse(input, out double res8, min, max);
                if (rc8.IsSuccess)
                {
                    value = new MavParamValue((float)res8);
                }
                return rc8;
            case MavParamType.MavParamTypeUint64:
            case MavParamType.MavParamTypeInt64:
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    
    public static ValidationResult TryParseValue(string? input, MavParamType type, out MavParamValue? value)
    {
        switch (type)
        {
            case MavParamType.MavParamTypeUint8:
                return TryParseValue(input,type, out value, byte.MinValue,byte.MaxValue);
            case MavParamType.MavParamTypeInt8:
                return TryParseValue(input,type, out value, sbyte.MinValue,sbyte.MaxValue);
            case MavParamType.MavParamTypeUint16:
                return TryParseValue(input,type, out value, ushort.MinValue,ushort.MaxValue);
            case MavParamType.MavParamTypeInt16:
                return TryParseValue(input,type, out value, short.MinValue,short.MaxValue);
            case MavParamType.MavParamTypeUint32:
                return TryParseValue(input,type, out value, uint.MinValue,uint.MaxValue);
            case MavParamType.MavParamTypeInt32:
                return TryParseValue(input,type, out value, int.MinValue, int.MaxValue);
            case MavParamType.MavParamTypeReal32:
                return TryParseValue(input,type, out value, float.MinValue, float.MaxValue);
            case MavParamType.MavParamTypeInt64:
            case MavParamType.MavParamTypeReal64:
            case MavParamType.MavParamTypeUint64:
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        
    }
    
    public static MavParamValue Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input string is null or whitespace.", nameof(input));

        var parts = input.Split(':');
        if (parts.Length != 2)
            throw new FormatException($"Invalid input format. Expected '<Type>:<Value>', got '{input}'.");

        var typeString = parts[0].Trim();
        var valueString = parts[1].Trim();

        var parsedType = typeString switch
        {
            nameof(Byte) => MavParamType.MavParamTypeUint8,
            nameof(SByte) => MavParamType.MavParamTypeInt8,
            nameof(UInt16) => MavParamType.MavParamTypeUint16,
            nameof(Int16) => MavParamType.MavParamTypeInt16,
            nameof(UInt32) => MavParamType.MavParamTypeUint32,
            nameof(Int32) => MavParamType.MavParamTypeInt32,
            nameof(Single) => MavParamType.MavParamTypeReal32,
            nameof(UInt64) => MavParamType.MavParamTypeUint64,
            nameof(Int64) => MavParamType.MavParamTypeInt64,
            nameof(Double) => MavParamType.MavParamTypeReal64,
            _ => throw new ArgumentOutOfRangeException(nameof(typeString),
                $"Unknown or unsupported type '{typeString}' in MavParamValue.Parse.")
        };

        var rx = TryParseValue(valueString, parsedType, out var result);
        if (!rx.IsSuccess)
        {
            throw new FormatException($"Failed to parse value '{valueString}' as {parsedType}.");
        }

        Debug.Assert(result != null, nameof(result) + " != null");
        return result.Value;

    }



    public override string ToString()
    {
        return Type switch
        {
            MavParamType.MavParamTypeUint8 or MavParamType.MavParamTypeInt8 or MavParamType.MavParamTypeUint16
                or MavParamType.MavParamTypeInt16 or MavParamType.MavParamTypeUint32
                or MavParamType.MavParamTypeInt32 => $"{PrintType(Type)}:{_intValue}",
            MavParamType.MavParamTypeReal32 or MavParamType.MavParamTypeUint64 or MavParamType.MavParamTypeInt64
                or MavParamType.MavParamTypeReal64 => $"{PrintType(Type)}:{_realValue}",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
}