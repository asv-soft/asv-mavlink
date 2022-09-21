using System;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public enum Pv2MissionActionType : uint
    {
        Unknown,
        StartRecord,
        StopRecord
    }


    public abstract class Pv2MissionAction : ISizedSpanSerializable
    {
        public abstract Pv2MissionActionType Type { get; }

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

        public abstract Pv2MissionAction Clone();

    }

    public class Pv2UnknownAction : Pv2MissionAction
    {
        public static Pv2MissionAction Default { get; } = new Pv2UnknownAction();

        public override Pv2MissionActionType Type => Pv2MissionActionType.Unknown;

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

        public override Pv2MissionAction Clone()
        {
            return (Pv2MissionAction)MemberwiseClone();
        }
    }


    public class Pv2StartRecordAction : Pv2MissionAction
    {
        public override Pv2MissionActionType Type => Pv2MissionActionType.StartRecord;

        public SessionSettings SessionSetting { get; set; }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            SessionSetting = new SessionSettings();
            SessionSetting.Deserialize(ref buffer);
        }

        public override void Serialize(ref Span<byte> buffer)
        {
            if (SessionSetting == null)
                throw new NullReferenceException(
                    $"Error serialize: {nameof(Pv2StartRecordAction)}.{nameof(SessionSetting)} == null");
            SessionSetting.Serialize(ref buffer);
        }

        public override int GetByteSize()
        {
            if (SessionSetting == null)
                throw new NullReferenceException(
                    $"Error serialize: {nameof(Pv2StartRecordAction)}.{nameof(SessionSetting)} == null");
            return SessionSetting.GetByteSize();
        }

        public override string ToString()
        {
            return $"action:{Type}, session:{SessionSetting}";
        }

        public override Pv2MissionAction Clone()
        {
            return (Pv2MissionAction)MemberwiseClone();
        }

        public void DoAction(IPv2ServerRttInterface rtt)
        {
            rtt.StartSession(SessionSetting);
        }
    }

    public class Pv2StopRecordAction : Pv2MissionAction
    {
        public override Pv2MissionActionType Type => Pv2MissionActionType.StopRecord;

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

        public override string ToString()
        {
            return $"action:{Type}";
        }

        public override Pv2MissionAction Clone()
        {
            return (Pv2MissionAction)MemberwiseClone();
        }

        public void DoAction(IPv2ServerRttInterface rtt)
        {
            rtt.StopSession();
        }
    }
}
