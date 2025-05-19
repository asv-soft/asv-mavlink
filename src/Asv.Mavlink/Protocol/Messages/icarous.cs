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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.16+a43ef88c0eb6d4725d650c062779442ee3bd78f6 25-05-19.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.AsvAudio;
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

        public void Visit(IVisitor visitor)
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
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public IcarousFmsState _Status;
        public IcarousFmsState Status { get => _Status; set => _Status = value; } 
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

        public void Visit(IVisitor visitor)
        {
            FloatType.Accept(visitor,Min1Field, ref _Min1);    
            FloatType.Accept(visitor,Max1Field, ref _Max1);    
            FloatType.Accept(visitor,Min2Field, ref _Min2);    
            FloatType.Accept(visitor,Max2Field, ref _Max2);    
            FloatType.Accept(visitor,Min3Field, ref _Min3);    
            FloatType.Accept(visitor,Max3Field, ref _Max3);    
            FloatType.Accept(visitor,Min4Field, ref _Min4);    
            FloatType.Accept(visitor,Max4Field, ref _Max4);    
            FloatType.Accept(visitor,Min5Field, ref _Min5);    
            FloatType.Accept(visitor,Max5Field, ref _Max5);    
            Int8Type.Accept(visitor,NumbandsField, ref _Numbands);                
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
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Min1;
        public float Min1 { get => _Min1; set { _Min1 = value; } }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max1, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Max1Field = new Field.Builder()
            .Name(nameof(Max1))
            .Title("max1")
            .Description("max angle (degrees)")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Max1;
        public float Max1 { get => _Max1; set { _Max1 = value; } }
        /// <summary>
        /// min angle (degrees)
        /// OriginName: min2, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Min2Field = new Field.Builder()
            .Name(nameof(Min2))
            .Title("min2")
            .Description("min angle (degrees)")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Min2;
        public float Min2 { get => _Min2; set { _Min2 = value; } }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max2, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Max2Field = new Field.Builder()
            .Name(nameof(Max2))
            .Title("max2")
            .Description("max angle (degrees)")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Max2;
        public float Max2 { get => _Max2; set { _Max2 = value; } }
        /// <summary>
        /// min angle (degrees)
        /// OriginName: min3, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Min3Field = new Field.Builder()
            .Name(nameof(Min3))
            .Title("min3")
            .Description("min angle (degrees)")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Min3;
        public float Min3 { get => _Min3; set { _Min3 = value; } }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max3, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Max3Field = new Field.Builder()
            .Name(nameof(Max3))
            .Title("max3")
            .Description("max angle (degrees)")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Max3;
        public float Max3 { get => _Max3; set { _Max3 = value; } }
        /// <summary>
        /// min angle (degrees)
        /// OriginName: min4, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Min4Field = new Field.Builder()
            .Name(nameof(Min4))
            .Title("min4")
            .Description("min angle (degrees)")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Min4;
        public float Min4 { get => _Min4; set { _Min4 = value; } }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max4, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Max4Field = new Field.Builder()
            .Name(nameof(Max4))
            .Title("max4")
            .Description("max angle (degrees)")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Max4;
        public float Max4 { get => _Max4; set { _Max4 = value; } }
        /// <summary>
        /// min angle (degrees)
        /// OriginName: min5, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Min5Field = new Field.Builder()
            .Name(nameof(Min5))
            .Title("min5")
            .Description("min angle (degrees)")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Min5;
        public float Min5 { get => _Min5; set { _Min5 = value; } }
        /// <summary>
        /// max angle (degrees)
        /// OriginName: max5, Units: deg, IsExtended: false
        /// </summary>
        public static readonly Field Max5Field = new Field.Builder()
            .Name(nameof(Max5))
            .Title("max5")
            .Description("max angle (degrees)")
            .FormatString(string.Empty)
            .Units(@"deg")
            .DataType(FloatType.Default)

            .Build();
        private float _Max5;
        public float Max5 { get => _Max5; set { _Max5 = value; } }
        /// <summary>
        /// Number of track bands
        /// OriginName: numBands, Units: , IsExtended: false
        /// </summary>
        public static readonly Field NumbandsField = new Field.Builder()
            .Name(nameof(Numbands))
            .Title("numBands")
            .Description("Number of track bands")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(Int8Type.Default)

            .Build();
        private sbyte _Numbands;
        public sbyte Numbands { get => _Numbands; set { _Numbands = value; } }
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Type1Field = new Field.Builder()
            .Name(nameof(Type1))
            .Title("type1")
            .Description("See the TRACK_BAND_TYPES enum.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public IcarousTrackBandTypes _Type1;
        public IcarousTrackBandTypes Type1 { get => _Type1; set => _Type1 = value; } 
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Type2Field = new Field.Builder()
            .Name(nameof(Type2))
            .Title("type2")
            .Description("See the TRACK_BAND_TYPES enum.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public IcarousTrackBandTypes _Type2;
        public IcarousTrackBandTypes Type2 { get => _Type2; set => _Type2 = value; } 
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type3, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Type3Field = new Field.Builder()
            .Name(nameof(Type3))
            .Title("type3")
            .Description("See the TRACK_BAND_TYPES enum.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public IcarousTrackBandTypes _Type3;
        public IcarousTrackBandTypes Type3 { get => _Type3; set => _Type3 = value; } 
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type4, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Type4Field = new Field.Builder()
            .Name(nameof(Type4))
            .Title("type4")
            .Description("See the TRACK_BAND_TYPES enum.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public IcarousTrackBandTypes _Type4;
        public IcarousTrackBandTypes Type4 { get => _Type4; set => _Type4 = value; } 
        /// <summary>
        /// See the TRACK_BAND_TYPES enum.
        /// OriginName: type5, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Type5Field = new Field.Builder()
            .Name(nameof(Type5))
            .Title("type5")
            .Description("See the TRACK_BAND_TYPES enum.")
            .FormatString(string.Empty)
            .Units(string.Empty)
            .DataType(UInt8Type.Default)

            .Build();
        public IcarousTrackBandTypes _Type5;
        public IcarousTrackBandTypes Type5 { get => _Type5; set => _Type5 = value; } 
    }




        


#endregion


}
