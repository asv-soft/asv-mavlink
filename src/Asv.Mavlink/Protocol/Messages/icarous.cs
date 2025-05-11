// MIT License
//
// Copyright (c) 2025 asv-soft (https://github.com/asv-soft)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.15+3a942e4794bafbc9b7e025a76c610b9704955531 25-05-11.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.IO;

namespace Asv.Mavlink.Icarous
{

    public static class IcarousHelper
    {
        public static void RegisterIcarousDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(IcarousHeartbeatPacket.MessageId, ()=>new IcarousHeartbeatPacket());
            src.Add(IcarousKinematicBandsPacket.MessageId, ()=>new IcarousKinematicBandsPacket());
        }
    }

#region Enums

    /// <summary>
    ///  ICAROUS_TRACK_BAND_TYPES
    /// </summary>
    public enum IcarousTrackBandTypes:uint
    {
        /// <summary>
        /// ICAROUS_TRACK_BAND_TYPE_NONE
        /// </summary>
        IcarousTrackBandTypeNone = 0,
        /// <summary>
        /// ICAROUS_TRACK_BAND_TYPE_NEAR
        /// </summary>
        IcarousTrackBandTypeNear = 1,
        /// <summary>
        /// ICAROUS_TRACK_BAND_TYPE_RECOVERY
        /// </summary>
        IcarousTrackBandTypeRecovery = 2,
    }

    /// <summary>
    ///  ICAROUS_FMS_STATE
    /// </summary>
    public enum IcarousFmsState:uint
    {
        /// <summary>
        /// ICAROUS_FMS_STATE_IDLE
        /// </summary>
        IcarousFmsStateIdle = 0,
        /// <summary>
        /// ICAROUS_FMS_STATE_TAKEOFF
        /// </summary>
        IcarousFmsStateTakeoff = 1,
        /// <summary>
        /// ICAROUS_FMS_STATE_CLIMB
        /// </summary>
        IcarousFmsStateClimb = 2,
        /// <summary>
        /// ICAROUS_FMS_STATE_CRUISE
        /// </summary>
        IcarousFmsStateCruise = 3,
        /// <summary>
        /// ICAROUS_FMS_STATE_APPROACH
        /// </summary>
        IcarousFmsStateApproach = 4,
        /// <summary>
        /// ICAROUS_FMS_STATE_LAND
        /// </summary>
        IcarousFmsStateLand = 5,
    }


#endregion

#region Messages

    /// <summary>
    /// ICAROUS heartbeat
    ///  ICAROUS_HEARTBEAT
    /// </summary>
    public class IcarousHeartbeatPacket : MavlinkV2Message<IcarousHeartbeatPayload>
    {
        public const int MessageId = 42000;
        
        public const byte CrcExtra = 227;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override IcarousHeartbeatPayload Payload { get; } = new();

        public override string Name => "ICAROUS_HEARTBEAT";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "status",
            "See the FMS_STATE enum.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ICAROUS_HEARTBEAT:"
        + "uint8_t status;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Status);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Status = (IcarousFmsState)reader.ReadEnum(StaticFields[0]);
        
            
        }
    }

    /// <summary>
    ///  ICAROUS_HEARTBEAT
    /// </summary>
    public class IcarousHeartbeatPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 1; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 1; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            + 1 // uint8_t status
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Status = (IcarousFmsState)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)Status);
            /* PayloadByteSize = 1 */;
        }
        
        



        /// <summary>
        /// See the FMS_STATE enum.
        /// OriginName: status, Units: , IsExtended: false
        /// </summary>
        public IcarousFmsState Status { get; set; }
    }
    /// <summary>
    /// Kinematic multi bands (track) output from Daidalus
    ///  ICAROUS_KINEMATIC_BANDS
    /// </summary>
    public class IcarousKinematicBandsPacket : MavlinkV2Message<IcarousKinematicBandsPayload>
    {
        public const int MessageId = 42001;
        
        public const byte CrcExtra = 239;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override IcarousKinematicBandsPayload Payload { get; } = new();

        public override string Name => "ICAROUS_KINEMATIC_BANDS";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new(0,
            "min1",
            "min angle (degrees)",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(1,
            "max1",
            "max angle (degrees)",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(2,
            "min2",
            "min angle (degrees)",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(3,
            "max2",
            "max angle (degrees)",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(4,
            "min3",
            "min angle (degrees)",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(5,
            "max3",
            "max angle (degrees)",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(6,
            "min4",
            "min angle (degrees)",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(7,
            "max4",
            "max angle (degrees)",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(8,
            "min5",
            "min angle (degrees)",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(9,
            "max5",
            "max angle (degrees)",
            string.Empty, 
            @"deg", 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new(10,
            "numBands",
            "Number of track bands",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int8, 
            0, 
            false),
            new(11,
            "type1",
            "See the TRACK_BAND_TYPES enum.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(12,
            "type2",
            "See the TRACK_BAND_TYPES enum.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(13,
            "type3",
            "See the TRACK_BAND_TYPES enum.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(14,
            "type4",
            "See the TRACK_BAND_TYPES enum.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new(15,
            "type5",
            "See the TRACK_BAND_TYPES enum.",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "ICAROUS_KINEMATIC_BANDS:"
        + "float min1;"
        + "float max1;"
        + "float min2;"
        + "float max2;"
        + "float min3;"
        + "float max3;"
        + "float min4;"
        + "float max4;"
        + "float min5;"
        + "float max5;"
        + "int8_t numBands;"
        + "uint8_t type1;"
        + "uint8_t type2;"
        + "uint8_t type3;"
        + "uint8_t type4;"
        + "uint8_t type5;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
            writer.Write(StaticFields[0], Payload.Min1);
            writer.Write(StaticFields[1], Payload.Max1);
            writer.Write(StaticFields[2], Payload.Min2);
            writer.Write(StaticFields[3], Payload.Max2);
            writer.Write(StaticFields[4], Payload.Min3);
            writer.Write(StaticFields[5], Payload.Max3);
            writer.Write(StaticFields[6], Payload.Min4);
            writer.Write(StaticFields[7], Payload.Max4);
            writer.Write(StaticFields[8], Payload.Min5);
            writer.Write(StaticFields[9], Payload.Max5);
            writer.Write(StaticFields[10], Payload.Numbands);
            writer.Write(StaticFields[11], Payload.Type1);
            writer.Write(StaticFields[12], Payload.Type2);
            writer.Write(StaticFields[13], Payload.Type3);
            writer.Write(StaticFields[14], Payload.Type4);
            writer.Write(StaticFields[15], Payload.Type5);
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
            Payload.Min1 = reader.ReadFloat(StaticFields[0]);
            Payload.Max1 = reader.ReadFloat(StaticFields[1]);
            Payload.Min2 = reader.ReadFloat(StaticFields[2]);
            Payload.Max2 = reader.ReadFloat(StaticFields[3]);
            Payload.Min3 = reader.ReadFloat(StaticFields[4]);
            Payload.Max3 = reader.ReadFloat(StaticFields[5]);
            Payload.Min4 = reader.ReadFloat(StaticFields[6]);
            Payload.Max4 = reader.ReadFloat(StaticFields[7]);
            Payload.Min5 = reader.ReadFloat(StaticFields[8]);
            Payload.Max5 = reader.ReadFloat(StaticFields[9]);
            Payload.Numbands = reader.ReadSByte(StaticFields[10]);
            Payload.Type1 = (IcarousTrackBandTypes)reader.ReadEnum(StaticFields[11]);
            Payload.Type2 = (IcarousTrackBandTypes)reader.ReadEnum(StaticFields[12]);
            Payload.Type3 = (IcarousTrackBandTypes)reader.ReadEnum(StaticFields[13]);
            Payload.Type4 = (IcarousTrackBandTypes)reader.ReadEnum(StaticFields[14]);
            Payload.Type5 = (IcarousTrackBandTypes)reader.ReadEnum(StaticFields[15]);
        
            
        }
    }

    /// <summary>
    ///  ICAROUS_KINEMATIC_BANDS
    /// </summary>
    public class IcarousKinematicBandsPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 46; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 46; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +4 // float min1
            +4 // float max1
            +4 // float min2
            +4 // float max2
            +4 // float min3
            +4 // float max3
            +4 // float min4
            +4 // float max4
            +4 // float min5
            +4 // float max5
            +1 // int8_t numBands
            + 1 // uint8_t type1
            + 1 // uint8_t type2
            + 1 // uint8_t type3
            + 1 // uint8_t type4
            + 1 // uint8_t type5
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Min1 = BinSerialize.ReadFloat(ref buffer);
            Max1 = BinSerialize.ReadFloat(ref buffer);
            Min2 = BinSerialize.ReadFloat(ref buffer);
            Max2 = BinSerialize.ReadFloat(ref buffer);
            Min3 = BinSerialize.ReadFloat(ref buffer);
            Max3 = BinSerialize.ReadFloat(ref buffer);
            Min4 = BinSerialize.ReadFloat(ref buffer);
            Max4 = BinSerialize.ReadFloat(ref buffer);
            Min5 = BinSerialize.ReadFloat(ref buffer);
            Max5 = BinSerialize.ReadFloat(ref buffer);
            Numbands = (sbyte)BinSerialize.ReadByte(ref buffer);
            Type1 = (IcarousTrackBandTypes)BinSerialize.ReadByte(ref buffer);
            Type2 = (IcarousTrackBandTypes)BinSerialize.ReadByte(ref buffer);
            Type3 = (IcarousTrackBandTypes)BinSerialize.ReadByte(ref buffer);
            Type4 = (IcarousTrackBandTypes)BinSerialize.ReadByte(ref buffer);
            Type5 = (IcarousTrackBandTypes)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteFloat(ref buffer,Min1);
            BinSerialize.WriteFloat(ref buffer,Max1);
            BinSerialize.WriteFloat(ref buffer,Min2);
            BinSerialize.WriteFloat(ref buffer,Max2);
            BinSerialize.WriteFloat(ref buffer,Min3);
            BinSerialize.WriteFloat(ref buffer,Max3);
            BinSerialize.WriteFloat(ref buffer,Min4);
            BinSerialize.WriteFloat(ref buffer,Max4);
            BinSerialize.WriteFloat(ref buffer,Min5);
            BinSerialize.WriteFloat(ref buffer,Max5);
            BinSerialize.WriteByte(ref buffer,(byte)Numbands);
            BinSerialize.WriteByte(ref buffer,(byte)Type1);
            BinSerialize.WriteByte(ref buffer,(byte)Type2);
            BinSerialize.WriteByte(ref buffer,(byte)Type3);
            BinSerialize.WriteByte(ref buffer,(byte)Type4);
            BinSerialize.WriteByte(ref buffer,(byte)Type5);
            /* PayloadByteSize = 46 */;
        }
        
        



        /// <summary>
        /// min angle (degrees)
        /// OriginName: min1, Units: deg, IsExtended: false
        /// </summary>
        public float Min1 { get; set; }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max1, Units: deg, IsExtended: false
        /// </summary>
        public float Max1 { get; set; }
        /// <summary>
        /// min angle (degrees)
        /// OriginName: min2, Units: deg, IsExtended: false
        /// </summary>
        public float Min2 { get; set; }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max2, Units: deg, IsExtended: false
        /// </summary>
        public float Max2 { get; set; }
        /// <summary>
        /// min angle (degrees)
        /// OriginName: min3, Units: deg, IsExtended: false
        /// </summary>
        public float Min3 { get; set; }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max3, Units: deg, IsExtended: false
        /// </summary>
        public float Max3 { get; set; }
        /// <summary>
        /// min angle (degrees)
        /// OriginName: min4, Units: deg, IsExtended: false
        /// </summary>
        public float Min4 { get; set; }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max4, Units: deg, IsExtended: false
        /// </summary>
        public float Max4 { get; set; }
        /// <summary>
        /// min angle (degrees)
        /// OriginName: min5, Units: deg, IsExtended: false
        /// </summary>
        public float Min5 { get; set; }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max5, Units: deg, IsExtended: false
        /// </summary>
        public float Max5 { get; set; }
        /// <summary>
        /// Number of track bands
        /// OriginName: numBands, Units: , IsExtended: false
        /// </summary>
        public sbyte Numbands { get; set; }
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type1, Units: , IsExtended: false
        /// </summary>
        public IcarousTrackBandTypes Type1 { get; set; }
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type2, Units: , IsExtended: false
        /// </summary>
        public IcarousTrackBandTypes Type2 { get; set; }
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type3, Units: , IsExtended: false
        /// </summary>
        public IcarousTrackBandTypes Type3 { get; set; }
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type4, Units: , IsExtended: false
        /// </summary>
        public IcarousTrackBandTypes Type4 { get; set; }
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type5, Units: , IsExtended: false
        /// </summary>
        public IcarousTrackBandTypes Type5 { get; set; }
    }


#endregion


}
