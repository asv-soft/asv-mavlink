using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2MissionItem : ISizedSpanSerializable
    {
        public uint Index { get; internal set; }
        public Pv2MissionTrigger Trigger { get; internal set; }
        public Pv2MissionAction Action { get; internal set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Index = BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            var trigger = (Pv2MissionTriggerType)BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            Trigger = Pv2MissionInterface.CreateTrigger(trigger);
            var action = (Pv2MissionActionType)BinSerialize.ReadPackedUnsignedInteger(ref buffer);
            Action = Pv2MissionInterface.CreateAction(action);
            Trigger.Deserialize(ref buffer);
            Action.Deserialize(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            if (Trigger == null)
                throw new NullReferenceException($"{nameof(Pv2MissionItem)}.{nameof(Trigger)} == null");
            if (Action == null) throw new NullReferenceException($"{nameof(Pv2MissionItem)}.{nameof(Action)} == null");
            BinSerialize.WritePackedUnsignedInteger(ref buffer, Index);
            BinSerialize.WritePackedUnsignedInteger(ref buffer,(uint)Trigger.Type);
            BinSerialize.WritePackedUnsignedInteger(ref buffer, (uint)Action.Type);
            Trigger.Serialize(ref buffer);
            Action.Serialize(ref buffer);
        }

        public int GetByteSize()
        {
            if (Trigger == null)
                throw new NullReferenceException($"{nameof(Pv2MissionItem)}.{nameof(Trigger)} == null");
            if (Action == null)
                throw new NullReferenceException($"{nameof(Pv2MissionItem)}.{nameof(Action)} == null");
            return BinSerialize.GetSizeForPackedUnsignedInteger(Index) +
                   BinSerialize.GetSizeForPackedUnsignedInteger((uint)Trigger.Type) +
                   BinSerialize.GetSizeForPackedUnsignedInteger((uint)Action.Type) +
                   Trigger.GetByteSize() +
                   Action.GetByteSize();
        }
    }
}
