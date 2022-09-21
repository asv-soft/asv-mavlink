using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using Asv.Cfg;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public enum Pv2ParamTypeEnum
    {
        Unknown,
        Int,
        UInt,
        Double,
        Float,
        String,
        Enum,
        Bool,
        Flags
    }

    public abstract class Pv2ParamType : ISizedSpanSerializable, IEquatable<Pv2ParamType>
    {
        private string _description = string.Empty;
        private string _fullName = string.Empty;

        private string _groupName = string.Empty;
        private string _paramName = string.Empty;

        // internal uint Index { get; set; } = ushort.MaxValue;

        internal Pv2ParamType()
        {
        }

        protected Pv2ParamType(string paramName, string description, string groupName, Pv2ParamFlags flags)
        {
            Flags = flags;
            ParamName = paramName;
            GroupName = groupName;
            Description = description;
            _fullName = null;
        }

        public abstract Pv2ParamTypeEnum TypeEnum { get; }
        public Pv2ParamFlags Flags { get; private set; }

        public string FullName
        {
            get
            {
                if (_fullName != null) return _fullName;
                _fullName = string.Concat(GroupName, "_", ParamName);
                return _fullName;
            }
        }

        public string GroupName
        {
            get => _groupName;
            protected set
            {
                Pv2ParamInterface.CheckAndSetName(ref _groupName, value);
                _fullName = null;
            }
        }

        public string ParamName
        {
            get => _paramName;
            protected set
            {
                Pv2ParamInterface.CheckAndSetName(ref _paramName, value);
                _fullName = null;
            }
        }

        public string Description
        {
            get => _description;
            protected set => Pv2ParamInterface.CheckAndSetDescription(ref _description, value);
        }


        public bool Equals(Pv2ParamType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _groupName == other._groupName && _paramName == other._paramName && TypeEnum == other.TypeEnum;
        }

        public virtual void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            // We wouldn't serialize and deserialize 'Type' field here. It would be serialized upper level for selecting class.
            Flags = (Pv2ParamFlags)BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            _groupName = BinSerialize.ReadString(ref buffer);
            _paramName = BinSerialize.ReadString(ref buffer);
            _description = BinSerialize.ReadString(ref buffer);
            _fullName = null;
        }

        public virtual void Serialize(ref Span<byte> buffer)
        {
            // We wouldn't serialize and deserialize 'Type' field here. It would be serialized upper level for selecting class.
            BinSerialize.WritePackedUnsignedInteger(ref buffer, (uint)Flags);
            BinSerialize.WriteString(ref buffer, GroupName);
            BinSerialize.WriteString(ref buffer, ParamName);
            BinSerialize.WriteString(ref buffer, Description);
        }


        public virtual int GetByteSize()
        {
            return BinSerialize.GetSizeForPackedUnsignedInteger((uint)Flags) +
                   BinSerialize.GetSizeForString(GroupName) +
                   BinSerialize.GetSizeForString(ParamName) +
                   BinSerialize.GetSizeForString(Description);
        }

        public void ValidateSize()
        {
            var size = GetByteSize();
            if (size > Pv2ParamInterface.MaxValidByteSize)
                throw new Exception(
                    $"Max size of serialized param info must be less then '{Pv2ParamInterface.MaxValidByteSize}'. '{size}' bytes now.");
        }

        public Pv2ParamValue CreateValue()
        {
            return Pv2ParamInterface.CreateValue(TypeEnum);
        }

        public Pv2ParamValue ReadFromConfig(IConfiguration config, string configSuffix)
        {
            var value = CreateValue();
            if (Flags.HasFlag(Pv2ParamFlags.ReadOnly)) return value;
            InternalReadFromConfig(config, configSuffix, value);
            return value;
        }

        public void WriteToConfig(IConfiguration config, string configSuffix, Pv2ParamValue value)
        {
            if (Flags.HasFlag(Pv2ParamFlags.ReadOnly)) return;
            InternalWriteToConfig(config, configSuffix, value);
        }

        protected abstract void InternalReadFromConfig(IConfiguration config, string configSuffix, Pv2ParamValue value);
        protected abstract void InternalWriteToConfig(IConfiguration config, string configSuffix, Pv2ParamValue value);

        public abstract void ValidateValue(Pv2ParamValue value);
        public abstract string ConvertToString(Pv2ParamValue value);


        public override string ToString()
        {
            return $"{GroupName}.{ParamName} of {TypeEnum:G}";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Pv2ParamType)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _groupName != null ? _groupName.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (_paramName != null ? _paramName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)TypeEnum;
                return hashCode;
            }
        }

        public static bool operator ==(Pv2ParamType left, Pv2ParamType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Pv2ParamType left, Pv2ParamType right)
        {
            return !Equals(left, right);
        }

        #region Static

        public static uint CalculateHash(Pv2ParamType type, uint initValue = 0U)
        {
            var data = ArrayPool<byte>.Shared.Rent(Pv2ParamInterface.MaxValidByteSize);
            try
            {
                var span = new Span<byte>(data, 0, Pv2ParamInterface.MaxValidByteSize);
                var size = span.Length;
                type.Serialize(ref span);
                size -= span.Length;
                return Crc32Q.Calc(data, size, initValue);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(data);
            }
        }

        public static uint CalculateHash(IEnumerable<Pv2ParamType> items)
        {
            return items.Aggregate(0U, (current, item) => CalculateHash(item, current));
        }

        #endregion
    }


    public abstract class Pv2ParamType<TValue, TParamValue> : Pv2ParamType where TParamValue : Pv2ParamValue
    {
        internal Pv2ParamType()
        {
        }

        protected Pv2ParamType(string paramName, string description, string groupName, TValue defaultValue,
            Pv2ParamFlags flags) : base(
            paramName, description, groupName, flags)
        {
            DefaultValue = defaultValue;
        }

        public TValue DefaultValue { get; internal set; }

        protected abstract TValue DeserializeValue(ref ReadOnlySpan<byte> buffer);
        protected abstract void SerializeValue(ref Span<byte> buffer, TValue value);
        protected abstract int GetValueSize(TValue value);

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            base.Deserialize(ref buffer);
            DefaultValue = DeserializeValue(ref buffer);
        }

        public override void Serialize(ref Span<byte> buffer)
        {
            base.Serialize(ref buffer);
            SerializeValue(ref buffer, DefaultValue);
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() +
                   GetValueSize(DefaultValue);
        }

        public TValue GetValue(Pv2ParamValue container)
        {
            return InternalGetValue(Pv2ParamInterface.CheckValueTypeAndCast<TParamValue>(container));
        }

        public void SetValue(Pv2ParamValue container, TValue value)
        {
            Validate(value);
            var paramValue = Pv2ParamInterface.CheckValueTypeAndCast<TParamValue>(container);
            InternalSetValue(paramValue, value);
        }

        public override void ValidateValue(Pv2ParamValue data)
        {
            Validate(GetValue(data));
        }

        protected override void InternalReadFromConfig(IConfiguration config, string configSuffix, Pv2ParamValue value)
        {
            SetValue(value, config.Get(string.Concat(configSuffix, "_", FullName), DefaultValue));
        }

        protected override void InternalWriteToConfig(IConfiguration config, string configSuffix, Pv2ParamValue value)
        {
            config.Set(string.Concat(configSuffix, "_", FullName), GetValue(value));
        }

        protected abstract void InternalSetValue(TParamValue paramValue, TValue value);
        protected abstract TValue InternalGetValue(TParamValue paramValue);
        public abstract bool IsValidValue(TValue value);
        public abstract string GetValidationError(TValue value);

        public void Validate(TValue value)
        {
            if (IsValidValue(value) == false)
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"{this} validation  error: {GetValidationError(value)}");
        }
    }
}
