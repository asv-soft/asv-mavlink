using System;
using System.Linq;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public enum BitSizeEnum
    {
        S03Bits = 03,
        S04Bits = 04,
        S05Bits = 05,
        S06Bits = 06,
        S07Bits = 07,
        S08Bits = 08,
        S09Bits = 09,
        S10Bits = 10,
        S11Bits = 11,
        S12Bits = 12,
        S13Bits = 13,
        S14Bits = 14,
        S15Bits = 15,
        S16Bits = 16,
        S17Bits = 17,
        S18Bits = 18,
        S19Bits = 19,
        S20Bits = 20,
        S21Bits = 21,
        S22Bits = 22,
        S23Bits = 23,
        S24Bits = 24,
        S25Bits = 25,
        S26Bits = 26,
        S27Bits = 27,
        S28Bits = 28,
        S29Bits = 29,
        S30Bits = 30,
        S31Bits = 31,
        S32Bits = 32
    }

    public class Pv2RttFixedPointDesc : Pv2RttFieldDesc
    {
        public Pv2RttFixedPointDesc()
        {
        }

        public Pv2RttFixedPointDesc(byte id, string name, string description, string units, string formatString,
            Pv2RttFieldFlags flags, BitSizeEnum bitSize, double offset, double fraction, double defaultValue,
            Pv2RttRecordDesc group) : base(id, name, description, units, formatString, flags, defaultValue, group)
        {
            BitSize = bitSize;
            Offset = offset;
            Fraction = fraction;
            try
            {
                FormatString.FormatWith(defaultValue);
            }
            catch (Exception e)
            {
                throw new Exception($"RTT field {name}[{id}] has wrong format string '{formatString}':{e.Message}");
            }

            if (Enum.GetValues(typeof(BitSizeEnum)).Cast<BitSizeEnum>().Contains(bitSize) == false)
                throw new ArgumentOutOfRangeException(nameof(bitSize), bitSize,
                    $"Unknwon value of enum {nameof(BitSizeEnum)}");
        }

        public override Pv2RttFieldType Type => Pv2RttFieldType.FixedPoint;

        public BitSizeEnum BitSize { get; set; }
        public double Offset { get; set; }
        public double Fraction { get; set; }

        public double MinValue => BitSize switch
        {
            BitSizeEnum.S03Bits => SpanBitHelper.FixedPointS3Min * Fraction + Offset,
            BitSizeEnum.S04Bits => SpanBitHelper.FixedPointS4Min * Fraction + Offset,
            BitSizeEnum.S05Bits => SpanBitHelper.FixedPointS5Min * Fraction + Offset,
            BitSizeEnum.S06Bits => SpanBitHelper.FixedPointS6Min * Fraction + Offset,
            BitSizeEnum.S07Bits => SpanBitHelper.FixedPointS7Min * Fraction + Offset,
            BitSizeEnum.S08Bits => SpanBitHelper.FixedPointS8Min * Fraction + Offset,
            BitSizeEnum.S09Bits => SpanBitHelper.FixedPointS9Min * Fraction + Offset,
            BitSizeEnum.S10Bits => SpanBitHelper.FixedPointS10Min * Fraction + Offset,
            BitSizeEnum.S11Bits => SpanBitHelper.FixedPointS11Min * Fraction + Offset,
            BitSizeEnum.S12Bits => SpanBitHelper.FixedPointS12Min * Fraction + Offset,
            BitSizeEnum.S13Bits => SpanBitHelper.FixedPointS13Min * Fraction + Offset,
            BitSizeEnum.S14Bits => SpanBitHelper.FixedPointS14Min * Fraction + Offset,
            BitSizeEnum.S15Bits => SpanBitHelper.FixedPointS15Min * Fraction + Offset,
            BitSizeEnum.S16Bits => SpanBitHelper.FixedPointS16Min * Fraction + Offset,
            BitSizeEnum.S17Bits => SpanBitHelper.FixedPointS17Min * Fraction + Offset,
            BitSizeEnum.S18Bits => SpanBitHelper.FixedPointS18Min * Fraction + Offset,
            BitSizeEnum.S19Bits => SpanBitHelper.FixedPointS19Min * Fraction + Offset,
            BitSizeEnum.S20Bits => SpanBitHelper.FixedPointS20Min * Fraction + Offset,
            BitSizeEnum.S21Bits => SpanBitHelper.FixedPointS21Min * Fraction + Offset,
            BitSizeEnum.S22Bits => SpanBitHelper.FixedPointS22Min * Fraction + Offset,
            BitSizeEnum.S23Bits => SpanBitHelper.FixedPointS23Min * Fraction + Offset,
            BitSizeEnum.S24Bits => SpanBitHelper.FixedPointS24Min * Fraction + Offset,
            BitSizeEnum.S25Bits => SpanBitHelper.FixedPointS25Min * Fraction + Offset,
            BitSizeEnum.S26Bits => SpanBitHelper.FixedPointS26Min * Fraction + Offset,
            BitSizeEnum.S27Bits => SpanBitHelper.FixedPointS27Min * Fraction + Offset,
            BitSizeEnum.S28Bits => SpanBitHelper.FixedPointS28Min * Fraction + Offset,
            BitSizeEnum.S29Bits => SpanBitHelper.FixedPointS29Min * Fraction + Offset,
            BitSizeEnum.S30Bits => SpanBitHelper.FixedPointS30Min * Fraction + Offset,
            BitSizeEnum.S31Bits => SpanBitHelper.FixedPointS31Min * Fraction + Offset,
            BitSizeEnum.S32Bits => SpanBitHelper.FixedPointS32Min * Fraction + Offset,
            _ => throw new ArgumentOutOfRangeException()
        };

        public double MaxValue => BitSize switch
        {
            BitSizeEnum.S03Bits => SpanBitHelper.FixedPointS3Max * Fraction + Offset,
            BitSizeEnum.S04Bits => SpanBitHelper.FixedPointS4Max * Fraction + Offset,
            BitSizeEnum.S05Bits => SpanBitHelper.FixedPointS5Max * Fraction + Offset,
            BitSizeEnum.S06Bits => SpanBitHelper.FixedPointS6Max * Fraction + Offset,
            BitSizeEnum.S07Bits => SpanBitHelper.FixedPointS7Max * Fraction + Offset,
            BitSizeEnum.S08Bits => SpanBitHelper.FixedPointS8Max * Fraction + Offset,
            BitSizeEnum.S09Bits => SpanBitHelper.FixedPointS9Max * Fraction + Offset,
            BitSizeEnum.S10Bits => SpanBitHelper.FixedPointS10Max * Fraction + Offset,
            BitSizeEnum.S11Bits => SpanBitHelper.FixedPointS11Max * Fraction + Offset,
            BitSizeEnum.S12Bits => SpanBitHelper.FixedPointS12Max * Fraction + Offset,
            BitSizeEnum.S13Bits => SpanBitHelper.FixedPointS13Max * Fraction + Offset,
            BitSizeEnum.S14Bits => SpanBitHelper.FixedPointS14Max * Fraction + Offset,
            BitSizeEnum.S15Bits => SpanBitHelper.FixedPointS15Max * Fraction + Offset,
            BitSizeEnum.S16Bits => SpanBitHelper.FixedPointS16Max * Fraction + Offset,
            BitSizeEnum.S17Bits => SpanBitHelper.FixedPointS17Max * Fraction + Offset,
            BitSizeEnum.S18Bits => SpanBitHelper.FixedPointS18Max * Fraction + Offset,
            BitSizeEnum.S19Bits => SpanBitHelper.FixedPointS19Max * Fraction + Offset,
            BitSizeEnum.S20Bits => SpanBitHelper.FixedPointS20Max * Fraction + Offset,
            BitSizeEnum.S21Bits => SpanBitHelper.FixedPointS21Max * Fraction + Offset,
            BitSizeEnum.S22Bits => SpanBitHelper.FixedPointS22Max * Fraction + Offset,
            BitSizeEnum.S23Bits => SpanBitHelper.FixedPointS23Max * Fraction + Offset,
            BitSizeEnum.S24Bits => SpanBitHelper.FixedPointS24Max * Fraction + Offset,
            BitSizeEnum.S25Bits => SpanBitHelper.FixedPointS25Max * Fraction + Offset,
            BitSizeEnum.S26Bits => SpanBitHelper.FixedPointS26Max * Fraction + Offset,
            BitSizeEnum.S27Bits => SpanBitHelper.FixedPointS27Max * Fraction + Offset,
            BitSizeEnum.S28Bits => SpanBitHelper.FixedPointS28Max * Fraction + Offset,
            BitSizeEnum.S29Bits => SpanBitHelper.FixedPointS29Max * Fraction + Offset,
            BitSizeEnum.S30Bits => SpanBitHelper.FixedPointS30Max * Fraction + Offset,
            BitSizeEnum.S31Bits => SpanBitHelper.FixedPointS31Max * Fraction + Offset,
            BitSizeEnum.S32Bits => SpanBitHelper.FixedPointS32Max * Fraction + Offset,
            _ => throw new ArgumentOutOfRangeException()
        };

        public override void ValidateValue(object value)
        {
            if (value is not double)
                throw new ArgumentException("Value must be 'double' type", nameof(value));
        }

        public override int GetValueMaxByteSize()
        {
            var bitSize = (int)BitSize;
            return bitSize % 8 == 0 ? bitSize / 8 : bitSize / 8 + 1;
        }

        public override void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer, (byte)BitSize);
            BinSerialize.WriteDouble(ref buffer, Offset);
            BinSerialize.WriteDouble(ref buffer, Fraction);
            base.Serialize(ref buffer); // <----- !!! at the end of serialzation
        }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            BitSize = (BitSizeEnum)BinSerialize.ReadByte(ref buffer);
            Offset = BinSerialize.ReadDouble(ref buffer);
            Fraction = BinSerialize.ReadDouble(ref buffer);
            base.Deserialize(ref buffer); // <----- !!! at the end of serialzation
        }

        public override int GetByteSize()
        {
            return base.GetByteSize() + sizeof(byte) + sizeof(double) * 2;
        }

        public override object DeserializeValue(ReadOnlySpan<byte> data, ref int bitIndex)
        {
            return BitSize switch
            {
                BitSizeEnum.S03Bits => SpanBitHelper.GetFixedPointS3Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S04Bits => SpanBitHelper.GetFixedPointS4Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S05Bits => SpanBitHelper.GetFixedPointS5Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S06Bits => SpanBitHelper.GetFixedPointS6Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S07Bits => SpanBitHelper.GetFixedPointS7Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S08Bits => SpanBitHelper.GetFixedPointS8Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S09Bits => SpanBitHelper.GetFixedPointS9Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S10Bits => SpanBitHelper.GetFixedPointS10Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S11Bits => SpanBitHelper.GetFixedPointS11Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S12Bits => SpanBitHelper.GetFixedPointS12Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S13Bits => SpanBitHelper.GetFixedPointS13Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S14Bits => SpanBitHelper.GetFixedPointS14Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S15Bits => SpanBitHelper.GetFixedPointS15Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S16Bits => SpanBitHelper.GetFixedPointS16Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S17Bits => SpanBitHelper.GetFixedPointS17Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S18Bits => SpanBitHelper.GetFixedPointS18Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S19Bits => SpanBitHelper.GetFixedPointS19Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S20Bits => SpanBitHelper.GetFixedPointS20Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S21Bits => SpanBitHelper.GetFixedPointS21Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S22Bits => SpanBitHelper.GetFixedPointS22Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S23Bits => SpanBitHelper.GetFixedPointS23Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S24Bits => SpanBitHelper.GetFixedPointS24Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S25Bits => SpanBitHelper.GetFixedPointS25Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S26Bits => SpanBitHelper.GetFixedPointS26Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S27Bits => SpanBitHelper.GetFixedPointS27Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S28Bits => SpanBitHelper.GetFixedPointS28Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S29Bits => SpanBitHelper.GetFixedPointS29Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S30Bits => SpanBitHelper.GetFixedPointS30Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S31Bits => SpanBitHelper.GetFixedPointS31Bit(data, ref bitIndex, Fraction, Offset),
                BitSizeEnum.S32Bits => SpanBitHelper.GetFixedPointS32Bit(data, ref bitIndex, Fraction, Offset),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override void SerializeValue(Span<byte> data, object value, ref int bitIndex)
        {
            switch (BitSize)
            {
                case BitSizeEnum.S03Bits:
                    SpanBitHelper.SetFixedPointS3Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S04Bits:
                    SpanBitHelper.SetFixedPointS4Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S05Bits:
                    SpanBitHelper.SetFixedPointS5Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S06Bits:
                    SpanBitHelper.SetFixedPointS6Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S07Bits:
                    SpanBitHelper.SetFixedPointS7Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S08Bits:
                    SpanBitHelper.SetFixedPointS8Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S09Bits:
                    SpanBitHelper.SetFixedPointS9Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S10Bits:
                    SpanBitHelper.SetFixedPointS10Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S11Bits:
                    SpanBitHelper.SetFixedPointS11Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S12Bits:
                    SpanBitHelper.SetFixedPointS12Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S13Bits:
                    SpanBitHelper.SetFixedPointS13Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S14Bits:
                    SpanBitHelper.SetFixedPointS14Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S15Bits:
                    SpanBitHelper.SetFixedPointS15Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S16Bits:
                    SpanBitHelper.SetFixedPointS16Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S17Bits:
                    SpanBitHelper.SetFixedPointS17Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S18Bits:
                    SpanBitHelper.SetFixedPointS18Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S19Bits:
                    SpanBitHelper.SetFixedPointS19Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S20Bits:
                    SpanBitHelper.SetFixedPointS20Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S21Bits:
                    SpanBitHelper.SetFixedPointS21Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S22Bits:
                    SpanBitHelper.SetFixedPointS22Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S23Bits:
                    SpanBitHelper.SetFixedPointS23Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S24Bits:
                    SpanBitHelper.SetFixedPointS24Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S25Bits:
                    SpanBitHelper.SetFixedPointS25Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S26Bits:
                    SpanBitHelper.SetFixedPointS26Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S27Bits:
                    SpanBitHelper.SetFixedPointS27Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S28Bits:
                    SpanBitHelper.SetFixedPointS28Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S29Bits:
                    SpanBitHelper.SetFixedPointS29Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S30Bits:
                    SpanBitHelper.SetFixedPointS30Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S31Bits:
                    SpanBitHelper.SetFixedPointS31Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                case BitSizeEnum.S32Bits:
                    SpanBitHelper.SetFixedPointS32Bit(data, ref bitIndex, (double)value, Fraction, Offset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override int GetValueBitSize(object value)
        {
            return (int)BitSize;
        }

        public override string ToString()
        {
            return $"{FullId}[{GroupId}.{Id}] {Name} {Type},{Flags:F},{BitSize:G})";
        }
    }
}
