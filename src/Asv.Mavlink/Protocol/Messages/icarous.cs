// MIT License
//
// Copyright (c) 2024 asv-soft (https://github.com/asv-soft)
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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.11+22841a669900eb4c494a7e77e2d4b5fee4e474db

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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("status",
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+= 1; // Status
            return (byte)sum;
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
        
        public override MavlinkFieldInfo[] Fields => StaticFields;
                
        public static readonly MavlinkFieldInfo[] StaticFields =
        [
            new("min1",
"min angle (degrees)",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("max1",
"max angle (degrees)",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("min2",
"min angle (degrees)",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("max2",
"max angle (degrees)",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("min3",
"min angle (degrees)",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("max3",
"max angle (degrees)",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("min4",
"min angle (degrees)",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("max4",
"max angle (degrees)",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("min5",
"min angle (degrees)",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("max5",
"max angle (degrees)",
string.Empty, 
@"deg", 
string.Empty, 
string.Empty, 
            MessageFieldType.Float32, 
            0, 
false),
            new("numBands",
"Number of track bands",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Int8, 
            0, 
false),
            new("type1",
"See the TRACK_BAND_TYPES enum.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("type2",
"See the TRACK_BAND_TYPES enum.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("type3",
"See the TRACK_BAND_TYPES enum.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("type4",
"See the TRACK_BAND_TYPES enum.",
string.Empty, 
string.Empty, 
string.Empty, 
string.Empty, 
            MessageFieldType.Uint8, 
            0, 
false),
            new("type5",
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
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //Min1
            sum+=4; //Max1
            sum+=4; //Min2
            sum+=4; //Max2
            sum+=4; //Min3
            sum+=4; //Max3
            sum+=4; //Min4
            sum+=4; //Max4
            sum+=4; //Min5
            sum+=4; //Max5
            sum+=1; //Numbands
            sum+= 1; // Type1
            sum+= 1; // Type2
            sum+= 1; // Type3
            sum+= 1; // Type4
            sum+= 1; // Type5
            return (byte)sum;
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
