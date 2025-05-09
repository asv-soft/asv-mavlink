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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.0-dev.14+613eac956231b473246c80e7d407c06ce1728417 25-04-26.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.IO;

namespace Asv.Mavlink.Ualberta
{

    public static class UalbertaHelper
    {
        public static void RegisterUalbertaDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(NavFilterBiasPacket.MessageId, ()=>new NavFilterBiasPacket());
            src.Add(RadioCalibrationPacket.MessageId, ()=>new RadioCalibrationPacket());
            src.Add(UalbertaSysStatusPacket.MessageId, ()=>new UalbertaSysStatusPacket());
        }
    }

#region Enums

    /// <summary>
    /// Available autopilot modes for ualberta uav
    ///  UALBERTA_AUTOPILOT_MODE
    /// </summary>
    public enum UalbertaAutopilotMode:uint
    {
        /// <summary>
        /// Raw input pulse widts sent to output
        /// MODE_MANUAL_DIRECT
        /// </summary>
        ModeManualDirect = 1,
        /// <summary>
        /// Inputs are normalized using calibration, the converted back to raw pulse widths for output
        /// MODE_MANUAL_SCALED
        /// </summary>
        ModeManualScaled = 2,
        /// <summary>
        /// MODE_AUTO_PID_ATT
        /// </summary>
        ModeAutoPidAtt = 3,
        /// <summary>
        /// MODE_AUTO_PID_VEL
        /// </summary>
        ModeAutoPidVel = 4,
        /// <summary>
        /// MODE_AUTO_PID_POS
        /// </summary>
        ModeAutoPidPos = 5,
    }

    /// <summary>
    /// Navigation filter mode
    ///  UALBERTA_NAV_MODE
    /// </summary>
    public enum UalbertaNavMode:uint
    {
        /// <summary>
        /// NAV_AHRS_INIT
        /// </summary>
        NavAhrsInit = 1,
        /// <summary>
        /// AHRS mode
        /// NAV_AHRS
        /// </summary>
        NavAhrs = 2,
        /// <summary>
        /// INS/GPS initialization mode
        /// NAV_INS_GPS_INIT
        /// </summary>
        NavInsGpsInit = 3,
        /// <summary>
        /// INS/GPS mode
        /// NAV_INS_GPS
        /// </summary>
        NavInsGps = 4,
    }

    /// <summary>
    /// Mode currently commanded by pilot
    ///  UALBERTA_PILOT_MODE
    /// </summary>
    public enum UalbertaPilotMode:uint
    {
        /// <summary>
        /// PILOT_MANUAL
        /// </summary>
        PilotManual = 1,
        /// <summary>
        /// PILOT_AUTO
        /// </summary>
        PilotAuto = 2,
        /// <summary>
        ///  Rotomotion mode 
        /// PILOT_ROTO
        /// </summary>
        PilotRoto = 3,
    }


#endregion

#region Messages

    /// <summary>
    /// Accelerometer and Gyro biases from the navigation filter
    ///  NAV_FILTER_BIAS
    /// </summary>
    public class NavFilterBiasPacket : MavlinkV2Message<NavFilterBiasPayload>
    {
        public const int MessageId = 220;
        
        public const byte CrcExtra = 34;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override NavFilterBiasPayload Payload { get; } = new();

        public override string Name => "NAV_FILTER_BIAS";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("usec",
            "Timestamp (microseconds)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("accel_0",
            "b_f[0]",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("accel_1",
            "b_f[1]",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("accel_2",
            "b_f[2]",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("gyro_0",
            "b_f[0]",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("gyro_1",
            "b_f[1]",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("gyro_2",
            "b_f[2]",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
        ];
        public const string FormatMessage = "NAV_FILTER_BIAS:"
        + "uint64_t usec;"
        + "float accel_0;"
        + "float accel_1;"
        + "float accel_2;"
        + "float gyro_0;"
        + "float gyro_1;"
        + "float gyro_2;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  NAV_FILTER_BIAS
    /// </summary>
    public class NavFilterBiasPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 32; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 32; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //Usec
            sum+=4; //Accel0
            sum+=4; //Accel1
            sum+=4; //Accel2
            sum+=4; //Gyro0
            sum+=4; //Gyro1
            sum+=4; //Gyro2
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Usec = BinSerialize.ReadULong(ref buffer);
            Accel0 = BinSerialize.ReadFloat(ref buffer);
            Accel1 = BinSerialize.ReadFloat(ref buffer);
            Accel2 = BinSerialize.ReadFloat(ref buffer);
            Gyro0 = BinSerialize.ReadFloat(ref buffer);
            Gyro1 = BinSerialize.ReadFloat(ref buffer);
            Gyro2 = BinSerialize.ReadFloat(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,Usec);
            BinSerialize.WriteFloat(ref buffer,Accel0);
            BinSerialize.WriteFloat(ref buffer,Accel1);
            BinSerialize.WriteFloat(ref buffer,Accel2);
            BinSerialize.WriteFloat(ref buffer,Gyro0);
            BinSerialize.WriteFloat(ref buffer,Gyro1);
            BinSerialize.WriteFloat(ref buffer,Gyro2);
            /* PayloadByteSize = 32 */;
        }
        
        



        /// <summary>
        /// Timestamp (microseconds)
        /// OriginName: usec, Units: , IsExtended: false
        /// </summary>
        public ulong Usec { get; set; }
        /// <summary>
        /// b_f[0]
        /// OriginName: accel_0, Units: , IsExtended: false
        /// </summary>
        public float Accel0 { get; set; }
        /// <summary>
        /// b_f[1]
        /// OriginName: accel_1, Units: , IsExtended: false
        /// </summary>
        public float Accel1 { get; set; }
        /// <summary>
        /// b_f[2]
        /// OriginName: accel_2, Units: , IsExtended: false
        /// </summary>
        public float Accel2 { get; set; }
        /// <summary>
        /// b_f[0]
        /// OriginName: gyro_0, Units: , IsExtended: false
        /// </summary>
        public float Gyro0 { get; set; }
        /// <summary>
        /// b_f[1]
        /// OriginName: gyro_1, Units: , IsExtended: false
        /// </summary>
        public float Gyro1 { get; set; }
        /// <summary>
        /// b_f[2]
        /// OriginName: gyro_2, Units: , IsExtended: false
        /// </summary>
        public float Gyro2 { get; set; }
    }
    /// <summary>
    /// Complete set of calibration parameters for the radio
    ///  RADIO_CALIBRATION
    /// </summary>
    public class RadioCalibrationPacket : MavlinkV2Message<RadioCalibrationPayload>
    {
        public const int MessageId = 221;
        
        public const byte CrcExtra = 71;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override RadioCalibrationPayload Payload { get; } = new();

        public override string Name => "RADIO_CALIBRATION";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("aileron",
            "Aileron setpoints: left, center, right",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            3, 
            false),
            new("elevator",
            "Elevator setpoints: nose down, center, nose up",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            3, 
            false),
            new("rudder",
            "Rudder setpoints: nose left, center, nose right",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            3, 
            false),
            new("gyro",
            "Tail gyro mode/gain setpoints: heading hold, rate mode",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            2, 
            false),
            new("pitch",
            "Pitch curve setpoints (every 25%)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            5, 
            false),
            new("throttle",
            "Throttle curve setpoints (every 25%)",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            5, 
            false),
        ];
        public const string FormatMessage = "RADIO_CALIBRATION:"
        + "uint16_t[3] aileron;"
        + "uint16_t[3] elevator;"
        + "uint16_t[3] rudder;"
        + "uint16_t[2] gyro;"
        + "uint16_t[5] pitch;"
        + "uint16_t[5] throttle;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  RADIO_CALIBRATION
    /// </summary>
    public class RadioCalibrationPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 42; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 42; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=Aileron.Length * 2; //Aileron
            sum+=Elevator.Length * 2; //Elevator
            sum+=Rudder.Length * 2; //Rudder
            sum+=Gyro.Length * 2; //Gyro
            sum+=Pitch.Length * 2; //Pitch
            sum+=Throttle.Length * 2; //Throttle
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                Aileron[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                Elevator[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                Rudder[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                Gyro[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = /*ArrayLength*/5 - Math.Max(0,((/*PayloadByteSize*/42 - payloadSize - /*ExtendedFieldsLength*/0)/2 /*FieldTypeByteSize*/));
            Pitch = new ushort[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                Pitch[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 5;
            for(var i=0;i<arraySize;i++)
            {
                Throttle[i] = BinSerialize.ReadUShort(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<Aileron.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Aileron[i]);
            }
            for(var i=0;i<Elevator.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Elevator[i]);
            }
            for(var i=0;i<Rudder.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Rudder[i]);
            }
            for(var i=0;i<Gyro.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Gyro[i]);
            }
            for(var i=0;i<Pitch.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Pitch[i]);
            }
            for(var i=0;i<Throttle.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,Throttle[i]);
            }
            /* PayloadByteSize = 42 */;
        }
        
        



        /// <summary>
        /// Aileron setpoints: left, center, right
        /// OriginName: aileron, Units: , IsExtended: false
        /// </summary>
        public const int AileronMaxItemsCount = 3;
        public ushort[] Aileron { get; } = new ushort[3];
        /// <summary>
        /// Elevator setpoints: nose down, center, nose up
        /// OriginName: elevator, Units: , IsExtended: false
        /// </summary>
        public const int ElevatorMaxItemsCount = 3;
        public ushort[] Elevator { get; } = new ushort[3];
        /// <summary>
        /// Rudder setpoints: nose left, center, nose right
        /// OriginName: rudder, Units: , IsExtended: false
        /// </summary>
        public const int RudderMaxItemsCount = 3;
        public ushort[] Rudder { get; } = new ushort[3];
        /// <summary>
        /// Tail gyro mode/gain setpoints: heading hold, rate mode
        /// OriginName: gyro, Units: , IsExtended: false
        /// </summary>
        public const int GyroMaxItemsCount = 2;
        public ushort[] Gyro { get; } = new ushort[2];
        /// <summary>
        /// Pitch curve setpoints (every 25%)
        /// OriginName: pitch, Units: , IsExtended: false
        /// </summary>
        public const int PitchMaxItemsCount = 5;
        public ushort[] Pitch { get; set; } = new ushort[5];
        [Obsolete("This method is deprecated. Use GetPitchMaxItemsCount instead.")]
        public byte GetPitchMaxItemsCount() => 5;
        /// <summary>
        /// Throttle curve setpoints (every 25%)
        /// OriginName: throttle, Units: , IsExtended: false
        /// </summary>
        public const int ThrottleMaxItemsCount = 5;
        public ushort[] Throttle { get; } = new ushort[5];
    }
    /// <summary>
    /// System status specific to ualberta uav
    ///  UALBERTA_SYS_STATUS
    /// </summary>
    public class UalbertaSysStatusPacket : MavlinkV2Message<UalbertaSysStatusPayload>
    {
        public const int MessageId = 222;
        
        public const byte CrcExtra = 15;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override UalbertaSysStatusPayload Payload { get; } = new();

        public override string Name => "UALBERTA_SYS_STATUS";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("mode",
            "System mode, see UALBERTA_AUTOPILOT_MODE ENUM",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("nav_mode",
            "Navigation mode, see UALBERTA_NAV_MODE ENUM",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("pilot",
            "Pilot mode, see UALBERTA_PILOT_MODE",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
        ];
        public const string FormatMessage = "UALBERTA_SYS_STATUS:"
        + "uint8_t mode;"
        + "uint8_t nav_mode;"
        + "uint8_t pilot;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  UALBERTA_SYS_STATUS
    /// </summary>
    public class UalbertaSysStatusPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 3; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 3; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=1; //Mode
            sum+=1; //NavMode
            sum+=1; //Pilot
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Mode = (byte)BinSerialize.ReadByte(ref buffer);
            NavMode = (byte)BinSerialize.ReadByte(ref buffer);
            Pilot = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteByte(ref buffer,(byte)Mode);
            BinSerialize.WriteByte(ref buffer,(byte)NavMode);
            BinSerialize.WriteByte(ref buffer,(byte)Pilot);
            /* PayloadByteSize = 3 */;
        }
        
        



        /// <summary>
        /// System mode, see UALBERTA_AUTOPILOT_MODE ENUM
        /// OriginName: mode, Units: , IsExtended: false
        /// </summary>
        public byte Mode { get; set; }
        /// <summary>
        /// Navigation mode, see UALBERTA_NAV_MODE ENUM
        /// OriginName: nav_mode, Units: , IsExtended: false
        /// </summary>
        public byte NavMode { get; set; }
        /// <summary>
        /// Pilot mode, see UALBERTA_PILOT_MODE
        /// OriginName: pilot, Units: , IsExtended: false
        /// </summary>
        public byte Pilot { get; set; }
    }


#endregion


}
