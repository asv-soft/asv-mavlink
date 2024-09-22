using System;
using Asv.Mavlink.V2.Common;

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

    public int CompareTo(object obj)
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

    public override bool Equals(object obj)
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

    public string PrintValue()
    {
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
            MavParamType.MavParamTypeUint8 => "U8",
            MavParamType.MavParamTypeInt8 => "I8",
            MavParamType.MavParamTypeUint16 => "U16",
            MavParamType.MavParamTypeInt16 => "I16",
            MavParamType.MavParamTypeUint32 => "U32",
            MavParamType.MavParamTypeInt32 => "R32",
            MavParamType.MavParamTypeUint64 or MavParamType.MavParamTypeInt64 or MavParamType.MavParamTypeReal32
                or MavParamType.MavParamTypeReal64 => "Not supported",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    
    public override string ToString()
    {
        switch (Type)
        {
            case MavParamType.MavParamTypeUint8:
            case MavParamType.MavParamTypeInt8:
            case MavParamType.MavParamTypeUint16:
            case MavParamType.MavParamTypeInt16:
            case MavParamType.MavParamTypeUint32:
            case MavParamType.MavParamTypeInt32:
                return $"{_intValue}[{PrintType(Type)}]";
            case MavParamType.MavParamTypeReal32:
                return $"{_realValue}[{PrintType(Type)}]";
            case MavParamType.MavParamTypeUint64:
            case MavParamType.MavParamTypeInt64:
            case MavParamType.MavParamTypeReal64:
                return "Not supported";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}