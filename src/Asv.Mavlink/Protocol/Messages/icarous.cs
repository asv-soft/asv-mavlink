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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.17-dev.8+356100e330ee3351d1c0a76be38f09294117ae6a 25-09-26.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.AsvAudio;
using System.Linq;
using System.Collections.Generic;
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
    public enum IcarousTrackBandTypes : ulong
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
    public static class IcarousTrackBandTypesHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ICAROUS_TRACK_BAND_TYPE_NONE");
            yield return new EnumValue<T>(converter(1),"ICAROUS_TRACK_BAND_TYPE_NEAR");
            yield return new EnumValue<T>(converter(2),"ICAROUS_TRACK_BAND_TYPE_RECOVERY");
        }
    }
    /// <summary>
    ///  ICAROUS_FMS_STATE
    /// </summary>
    public enum IcarousFmsState : ulong
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
    public static class IcarousFmsStateHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(0);
            yield return converter(1);
            yield return converter(2);
            yield return converter(3);
            yield return converter(4);
            yield return converter(5);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(0),"ICAROUS_FMS_STATE_IDLE");
            yield return new EnumValue<T>(converter(1),"ICAROUS_FMS_STATE_TAKEOFF");
            yield return new EnumValue<T>(converter(2),"ICAROUS_FMS_STATE_CLIMB");
            yield return new EnumValue<T>(converter(3),"ICAROUS_FMS_STATE_CRUISE");
            yield return new EnumValue<T>(converter(4),"ICAROUS_FMS_STATE_APPROACH");
            yield return new EnumValue<T>(converter(5),"ICAROUS_FMS_STATE_LAND");
        }
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

        public void Accept(IVisitor visitor)
        {
            var tmpStatus = (byte)Status;
            UInt8Type.Accept(visitor,StatusField, ref tmpStatus);
            Status = (IcarousFmsState)tmpStatus;

        }

        /// <summary>
        /// See the FMS_STATE enum.
        /// OriginName: status, Units: , IsExtended: false
        /// </summary>
        public static readonly Field StatusField = new Field.Builder()
            .Name(nameof(Status))
            .Title("status")
            .Description("See the FMS_STATE enum.")
            .DataType(new UInt8Type(IcarousFmsStateHelper.GetValues(x=>(byte)x).Min(),IcarousFmsStateHelper.GetValues(x=>(byte)x).Max()))
            .Enum(IcarousFmsStateHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private IcarousFmsState _status;
        public IcarousFmsState Status { get => _status; set => _status = value; } 
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

        public void Accept(IVisitor visitor)
        {
            FloatType.Accept(visitor,Min1Field, ref _min1);    
            FloatType.Accept(visitor,Max1Field, ref _max1);    
            FloatType.Accept(visitor,Min2Field, ref _min2);    
            FloatType.Accept(visitor,Max2Field, ref _max2);    
            FloatType.Accept(visitor,Min3Field, ref _min3);    
            FloatType.Accept(visitor,Max3Field, ref _max3);    
            FloatType.Accept(visitor,Min4Field, ref _min4);    
            FloatType.Accept(visitor,Max4Field, ref _max4);    
            FloatType.Accept(visitor,Min5Field, ref _min5);    
            FloatType.Accept(visitor,Max5Field, ref _max5);    
            Int8Type.Accept(visitor,NumbandsField, ref _numbands);                
            var tmpType1 = (byte)Type1;
            UInt8Type.Accept(visitor,Type1Field, ref tmpType1);
            Type1 = (IcarousTrackBandTypes)tmpType1;
            var tmpType2 = (byte)Type2;
            UInt8Type.Accept(visitor,Type2Field, ref tmpType2);
            Type2 = (IcarousTrackBandTypes)tmpType2;
            var tmpType3 = (byte)Type3;
            UInt8Type.Accept(visitor,Type3Field, ref tmpType3);
            Type3 = (IcarousTrackBandTypes)tmpType3;
            var tmpType4 = (byte)Type4;
            UInt8Type.Accept(visitor,Type4Field, ref tmpType4);
            Type4 = (IcarousTrackBandTypes)tmpType4;
            var tmpType5 = (byte)Type5;
            UInt8Type.Accept(visitor,Type5Field, ref tmpType5);
            Type5 = (IcarousTrackBandTypes)tmpType5;

        }

        /// <summary>
        /// min angle (degrees)
        /// OriginName: min1, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Min1Field = new Field.Builder()
            .Name(nameof(Min1))
            .Title("min1")
            .Description("min angle (degrees)")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _min1;
        public float Min1 { get => _min1; set => _min1 = value; }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max1, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Max1Field = new Field.Builder()
            .Name(nameof(Max1))
            .Title("max1")
            .Description("max angle (degrees)")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _max1;
        public float Max1 { get => _max1; set => _max1 = value; }
        /// <summary>
        /// min angle (degrees)
        /// OriginName: min2, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Min2Field = new Field.Builder()
            .Name(nameof(Min2))
            .Title("min2")
            .Description("min angle (degrees)")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _min2;
        public float Min2 { get => _min2; set => _min2 = value; }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max2, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Max2Field = new Field.Builder()
            .Name(nameof(Max2))
            .Title("max2")
            .Description("max angle (degrees)")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _max2;
        public float Max2 { get => _max2; set => _max2 = value; }
        /// <summary>
        /// min angle (degrees)
        /// OriginName: min3, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Min3Field = new Field.Builder()
            .Name(nameof(Min3))
            .Title("min3")
            .Description("min angle (degrees)")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _min3;
        public float Min3 { get => _min3; set => _min3 = value; }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max3, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Max3Field = new Field.Builder()
            .Name(nameof(Max3))
            .Title("max3")
            .Description("max angle (degrees)")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _max3;
        public float Max3 { get => _max3; set => _max3 = value; }
        /// <summary>
        /// min angle (degrees)
        /// OriginName: min4, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Min4Field = new Field.Builder()
            .Name(nameof(Min4))
            .Title("min4")
            .Description("min angle (degrees)")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _min4;
        public float Min4 { get => _min4; set => _min4 = value; }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max4, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Max4Field = new Field.Builder()
            .Name(nameof(Max4))
            .Title("max4")
            .Description("max angle (degrees)")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _max4;
        public float Max4 { get => _max4; set => _max4 = value; }
        /// <summary>
        /// min angle (degrees)
        /// OriginName: min5, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Min5Field = new Field.Builder()
            .Name(nameof(Min5))
            .Title("min5")
            .Description("min angle (degrees)")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _min5;
        public float Min5 { get => _min5; set => _min5 = value; }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max5, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Max5Field = new Field.Builder()
            .Name(nameof(Max5))
            .Title("max5")
            .Description("max angle (degrees)")
.Units(@"deg")
            .DataType(FloatType.Default)
        .Build();
        private float _max5;
        public float Max5 { get => _max5; set => _max5 = value; }
        /// <summary>
        /// Number of track bands
        /// OriginName: numBands, Units: , IsExtended: false
        /// </summary>
        public static readonly Field NumbandsField = new Field.Builder()
            .Name(nameof(Numbands))
            .Title("numBands")
            .Description("Number of track bands")

            .DataType(Int8Type.Default)
        .Build();
        private sbyte _numbands;
        public sbyte Numbands { get => _numbands; set => _numbands = value; }
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Type1Field = new Field.Builder()
            .Name(nameof(Type1))
            .Title("type1")
            .Description("See the TRACK_BAND_TYPES enum.")
            .DataType(new UInt8Type(IcarousTrackBandTypesHelper.GetValues(x=>(byte)x).Min(),IcarousTrackBandTypesHelper.GetValues(x=>(byte)x).Max()))
            .Enum(IcarousTrackBandTypesHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private IcarousTrackBandTypes _type1;
        public IcarousTrackBandTypes Type1 { get => _type1; set => _type1 = value; } 
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Type2Field = new Field.Builder()
            .Name(nameof(Type2))
            .Title("type2")
            .Description("See the TRACK_BAND_TYPES enum.")
            .DataType(new UInt8Type(IcarousTrackBandTypesHelper.GetValues(x=>(byte)x).Min(),IcarousTrackBandTypesHelper.GetValues(x=>(byte)x).Max()))
            .Enum(IcarousTrackBandTypesHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private IcarousTrackBandTypes _type2;
        public IcarousTrackBandTypes Type2 { get => _type2; set => _type2 = value; } 
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type3, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Type3Field = new Field.Builder()
            .Name(nameof(Type3))
            .Title("type3")
            .Description("See the TRACK_BAND_TYPES enum.")
            .DataType(new UInt8Type(IcarousTrackBandTypesHelper.GetValues(x=>(byte)x).Min(),IcarousTrackBandTypesHelper.GetValues(x=>(byte)x).Max()))
            .Enum(IcarousTrackBandTypesHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private IcarousTrackBandTypes _type3;
        public IcarousTrackBandTypes Type3 { get => _type3; set => _type3 = value; } 
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type4, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Type4Field = new Field.Builder()
            .Name(nameof(Type4))
            .Title("type4")
            .Description("See the TRACK_BAND_TYPES enum.")
            .DataType(new UInt8Type(IcarousTrackBandTypesHelper.GetValues(x=>(byte)x).Min(),IcarousTrackBandTypesHelper.GetValues(x=>(byte)x).Max()))
            .Enum(IcarousTrackBandTypesHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private IcarousTrackBandTypes _type4;
        public IcarousTrackBandTypes Type4 { get => _type4; set => _type4 = value; } 
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type5, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Type5Field = new Field.Builder()
            .Name(nameof(Type5))
            .Title("type5")
            .Description("See the TRACK_BAND_TYPES enum.")
            .DataType(new UInt8Type(IcarousTrackBandTypesHelper.GetValues(x=>(byte)x).Min(),IcarousTrackBandTypesHelper.GetValues(x=>(byte)x).Max()))
            .Enum(IcarousTrackBandTypesHelper.GetEnumValues(x=>(byte)x))
            .Build();
        private IcarousTrackBandTypes _type5;
        public IcarousTrackBandTypes Type5 { get => _type5; set => _type5 = value; } 
    }




        


#endregion


}
