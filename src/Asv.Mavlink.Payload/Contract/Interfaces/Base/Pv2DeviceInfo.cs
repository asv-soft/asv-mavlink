using System;

namespace Asv.Mavlink.Payload
{
    public enum Pv2DeviceClass : byte
    {
        Unknown = 0,
        PayloadSdr = 1,
        RtkBaseStation = 2,
        Reserved3 = 3,
        Reserved4 = 4,
        Reserved5 = 5,
        Reserved6 = 6,
        Reserved7 = 7,
        Reserved8 = 8,
        Reserved9 = 9,
        Reserved10 = 10,
        Reserved11 = 11,
        Reserved12 = 12,
        Reserved13 = 13,
        Reserved14 = 14,
        Reserved15 = 15,
        Reserved16 = 16,
        Reserved17 = 17,
        Reserved18 = 18,
        Reserved19 = 19,
        Reserved20 = 20,
        Reserved21 = 21,
        Reserved22 = 22,
        Reserved23 = 23,
        Reserved24 = 24,
        Reserved25 = 25,
        Reserved26 = 26,
        Reserved27 = 27,
        Reserved28 = 28,
        Reserved29 = 29,
        Reserved30 = 30,
        Reserved31 = 31,

        Reserved255 = 255
    }

    [Flags]
    public enum Pv2DeviceCompatibilityFlags : byte
    {
        NoFlags = 0b0000_0000,
        Reserved1 = 0b0000_0001,
        Reserved2 = 0b0000_0010,
        Reserved3 = 0b0000_0100,
        Reserved4 = 0b0000_1000,
        Reserved5 = 0b0001_0000,
        Reserved6 = 0b0010_0000,
        Reserved7 = 0b0100_0000,
        Reserved8 = 0b1000_0000
    }


    public class Pv2DeviceInfo:IEquatable<Pv2DeviceInfo>
    {
        

        public uint CustomMode { get; private set; }

        public Pv2DeviceInfo(uint customMode)
        {
            CustomMode = customMode;
        }

        private const uint ClassMask = 0b0000_0000_0000_0000_0000_0000_1111_1111;
        private const int ClassOffset = 0;

        public Pv2DeviceClass Class
        {
            get => (Pv2DeviceClass)((CustomMode & ClassMask) >> ClassOffset);
            set => CustomMode = (CustomMode & ~ClassMask) | ((uint)value << ClassOffset) & ClassMask;
        }

        private const uint WorkModeMask = 0b0000_0000_0000_0000_0001_1111_0000_0000;
        private const int WorkModeOffset = 8;
        public byte WorkMode
        {
            get => (byte)((CustomMode & WorkModeMask) >> WorkModeOffset);
            set => CustomMode = (CustomMode & ~WorkModeMask) | ((uint)value << WorkModeOffset) & WorkModeMask;
        }

        private const uint WorkModeStatusMask = 0b0000_0000_0000_0000_1110_0000_0000_0000;
        private const int WorkModeStatusOffset = 13;
        public byte WorkModeStatus
        {
            get => (byte)((CustomMode & WorkModeStatusMask) >> WorkModeStatusOffset);
            set => CustomMode = (CustomMode & ~WorkModeStatusMask) | ((uint)value << WorkModeStatusOffset) & WorkModeStatusMask;
        }

        private const uint CompatibilityMask = 0b0000_0000_1111_1111_0000_0000_0000_0000;
        private const int CompatibilityOffset = 16;
        public Pv2DeviceCompatibilityFlags Compatibility
        {
            get => (Pv2DeviceCompatibilityFlags)((CustomMode & CompatibilityMask) >> CompatibilityOffset);
            set => CustomMode = (CustomMode & ~CompatibilityMask) | ((uint)value << CompatibilityOffset) & CompatibilityMask;
        }

        private const uint RttRecordEnabledMask = 0b0000_0001_0000_0000_0000_0000_0000_0000;
        private const int RttRecordEnabledOffset = 17;
        public bool RttRecordEnabled
        {
            get => ((CustomMode & RttRecordEnabledMask) >> RttRecordEnabledOffset) != 0;
            set => CustomMode = (CustomMode & ~RttRecordEnabledMask) | ((value ? 0U:1U) << RttRecordEnabledOffset) & RttRecordEnabledMask;
        }

        private const uint MissionStartedMask = 0b0000_0010_0000_0000_0000_0000_0000_0000;
        private const int MissionStartedOffset = 18;
        public bool MissionStarted
        {
            get => ((CustomMode & MissionStartedMask) >> MissionStartedOffset) != 0;
            set => CustomMode = (CustomMode & ~MissionStartedMask) | ((value ? 0U : 1U) << MissionStartedOffset) & MissionStartedMask;
        }


        public bool Equals(Pv2DeviceInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return CustomMode == other.CustomMode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pv2DeviceInfo)obj);
        }

        public override int GetHashCode()
        {
            return (int)CustomMode;
        }

        public static bool operator ==(Pv2DeviceInfo left, Pv2DeviceInfo right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Pv2DeviceInfo left, Pv2DeviceInfo right)
        {
            return !Equals(left, right);
        }
    }

}
