using System;
using System.Linq;
using Asv.IO;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Payload
{
    public enum Pv2MissionTriggerType : uint
    {
        Unknown,
        UavWayPointReached
    }

    public abstract class Pv2MissionTrigger : ISizedSpanSerializable
    {
        public abstract Pv2MissionTriggerType Type { get; }

        public virtual void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            throw new NotImplementedException();
        }

        public virtual void Serialize(ref Span<byte> buffer)
        {
            throw new NotImplementedException();
        }

        public virtual int GetByteSize()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{Type}";
        }

        public abstract Pv2MissionTrigger Clone();

    }

    public class Pv2UnknownMissionTrigger : Pv2MissionTrigger
    {
        public static Pv2MissionTrigger Default { get; } = new Pv2UnknownMissionTrigger();

        public override Pv2MissionTriggerType Type => Pv2MissionTriggerType.Unknown;

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
        }

        public override void Serialize(ref Span<byte> buffer)
        {
        }

        public override int GetByteSize()
        {
            return 0;
        }

        public override Pv2MissionTrigger Clone()
        {
            return (Pv2MissionTrigger)MemberwiseClone();
        }
    }


    public class Pv2UavWayPointReachedTrigger : Pv2MissionTrigger
    {
        public override Pv2MissionTriggerType Type => Pv2MissionTriggerType.UavWayPointReached;

        public bool UavIdFilterEnabled { get; set; }
        public byte UavSystemId { get; set; } = 1;
        public byte UavComponentId { get; set; } = 1;
        public ushort WpIndex { get; set; }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            UavIdFilterEnabled = BinSerialize.ReadBool(ref buffer);
            UavSystemId = BinSerialize.ReadByte(ref buffer);
            UavComponentId = BinSerialize.ReadByte(ref buffer);
            WpIndex = BinSerialize.ReadUShort(ref buffer);
        }

        public override void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteBool(ref buffer, UavIdFilterEnabled);
            BinSerialize.WriteByte(ref buffer, UavSystemId);
            BinSerialize.WriteByte(ref buffer, UavComponentId);
            BinSerialize.WriteUShort(ref buffer, WpIndex);
        }

        public override int GetByteSize()
        {
            return sizeof(byte) * 3 + sizeof(ushort);
        }

        public override string ToString()
        {
            return $"{Type}";
        }

        public override Pv2MissionTrigger Clone()
        {
            return (Pv2MissionTrigger)MemberwiseClone();
        }

        public bool Check(MissionItemReachedPacket[] value)
        {
            if (UavIdFilterEnabled)
            {
                return value.Any(_ => _.Payload.Seq == WpIndex && _.SystemId == UavSystemId && _.ComponenId == UavComponentId);
            }
            else
            {
                return value.Any(_ => _.Payload.Seq == WpIndex);
            }
        }
    }
}
