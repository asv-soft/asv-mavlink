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

// This code was generate by tool Asv.Mavlink.Shell version 4.0.11+05423b76b208fe780abe1cef9f7beeacb19cba77 25-07-04.

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
    public enum UalbertaAutopilotMode : ulong
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
    public static class UalbertaAutopilotModeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
            yield return converter(3);
            yield return converter(4);
            yield return converter(5);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"MODE_MANUAL_DIRECT");
            yield return new EnumValue<T>(converter(2),"MODE_MANUAL_SCALED");
            yield return new EnumValue<T>(converter(3),"MODE_AUTO_PID_ATT");
            yield return new EnumValue<T>(converter(4),"MODE_AUTO_PID_VEL");
            yield return new EnumValue<T>(converter(5),"MODE_AUTO_PID_POS");
        }
    }
    /// <summary>
    /// Navigation filter mode
    ///  UALBERTA_NAV_MODE
    /// </summary>
    public enum UalbertaNavMode : ulong
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
    public static class UalbertaNavModeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
            yield return converter(3);
            yield return converter(4);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"NAV_AHRS_INIT");
            yield return new EnumValue<T>(converter(2),"NAV_AHRS");
            yield return new EnumValue<T>(converter(3),"NAV_INS_GPS_INIT");
            yield return new EnumValue<T>(converter(4),"NAV_INS_GPS");
        }
    }
    /// <summary>
    /// Mode currently commanded by pilot
    ///  UALBERTA_PILOT_MODE
    /// </summary>
    public enum UalbertaPilotMode : ulong
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
    public static class UalbertaPilotModeHelper
    {
        public static IEnumerable<T> GetValues<T>(Func<ulong, T> converter)
        {
            yield return converter(1);
            yield return converter(2);
            yield return converter(3);
        }
        public static IEnumerable<EnumValue<T>> GetEnumValues<T>(Func<ulong,T> converter)
        {
            yield return new EnumValue<T>(converter(1),"PILOT_MANUAL");
            yield return new EnumValue<T>(converter(2),"PILOT_AUTO");
            yield return new EnumValue<T>(converter(3),"PILOT_ROTO");
        }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t usec
            +4 // float accel_0
            +4 // float accel_1
            +4 // float accel_2
            +4 // float gyro_0
            +4 // float gyro_1
            +4 // float gyro_2
            );
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

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,UsecField, UsecField.DataType, ref _usec);    
            FloatType.Accept(visitor,Accel0Field, Accel0Field.DataType, ref _accel0);    
            FloatType.Accept(visitor,Accel1Field, Accel1Field.DataType, ref _accel1);    
            FloatType.Accept(visitor,Accel2Field, Accel2Field.DataType, ref _accel2);    
            FloatType.Accept(visitor,Gyro0Field, Gyro0Field.DataType, ref _gyro0);    
            FloatType.Accept(visitor,Gyro1Field, Gyro1Field.DataType, ref _gyro1);    
            FloatType.Accept(visitor,Gyro2Field, Gyro2Field.DataType, ref _gyro2);    

        }

        /// <summary>
        /// Timestamp (microseconds)
        /// OriginName: usec, Units: , IsExtended: false
        /// </summary>
        public static readonly Field UsecField = new Field.Builder()
            .Name(nameof(Usec))
            .Title("usec")
            .Description("Timestamp (microseconds)")

            .DataType(UInt64Type.Default)
        .Build();
        private ulong _usec;
        public ulong Usec { get => _usec; set => _usec = value; }
        /// <summary>
        /// b_f[0]
        /// OriginName: accel_0, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Accel0Field = new Field.Builder()
            .Name(nameof(Accel0))
            .Title("accel_0")
            .Description("b_f[0]")

            .DataType(FloatType.Default)
        .Build();
        private float _accel0;
        public float Accel0 { get => _accel0; set => _accel0 = value; }
        /// <summary>
        /// b_f[1]
        /// OriginName: accel_1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Accel1Field = new Field.Builder()
            .Name(nameof(Accel1))
            .Title("accel_1")
            .Description("b_f[1]")

            .DataType(FloatType.Default)
        .Build();
        private float _accel1;
        public float Accel1 { get => _accel1; set => _accel1 = value; }
        /// <summary>
        /// b_f[2]
        /// OriginName: accel_2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Accel2Field = new Field.Builder()
            .Name(nameof(Accel2))
            .Title("accel_2")
            .Description("b_f[2]")

            .DataType(FloatType.Default)
        .Build();
        private float _accel2;
        public float Accel2 { get => _accel2; set => _accel2 = value; }
        /// <summary>
        /// b_f[0]
        /// OriginName: gyro_0, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Gyro0Field = new Field.Builder()
            .Name(nameof(Gyro0))
            .Title("gyro_0")
            .Description("b_f[0]")

            .DataType(FloatType.Default)
        .Build();
        private float _gyro0;
        public float Gyro0 { get => _gyro0; set => _gyro0 = value; }
        /// <summary>
        /// b_f[1]
        /// OriginName: gyro_1, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Gyro1Field = new Field.Builder()
            .Name(nameof(Gyro1))
            .Title("gyro_1")
            .Description("b_f[1]")

            .DataType(FloatType.Default)
        .Build();
        private float _gyro1;
        public float Gyro1 { get => _gyro1; set => _gyro1 = value; }
        /// <summary>
        /// b_f[2]
        /// OriginName: gyro_2, Units: , IsExtended: false
        /// </summary>
        public static readonly Field Gyro2Field = new Field.Builder()
            .Name(nameof(Gyro2))
            .Title("gyro_2")
            .Description("b_f[2]")

            .DataType(FloatType.Default)
        .Build();
        private float _gyro2;
        public float Gyro2 { get => _gyro2; set => _gyro2 = value; }
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +Aileron.Length * 2 // uint16_t[3] aileron
            +Elevator.Length * 2 // uint16_t[3] elevator
            +Rudder.Length * 2 // uint16_t[3] rudder
            +Gyro.Length * 2 // uint16_t[2] gyro
            +Pitch.Length * 2 // uint16_t[5] pitch
            +Throttle.Length * 2 // uint16_t[5] throttle
            );
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

        public void Accept(IVisitor visitor)
        {
            ArrayType.Accept(visitor,AileronField, AileronField.DataType, 3,
                (index, v, f, t) => UInt16Type.Accept(v, f, t, ref Aileron[index]));    
            ArrayType.Accept(visitor,ElevatorField, ElevatorField.DataType, 3,
                (index, v, f, t) => UInt16Type.Accept(v, f, t, ref Elevator[index]));    
            ArrayType.Accept(visitor,RudderField, RudderField.DataType, 3,
                (index, v, f, t) => UInt16Type.Accept(v, f, t, ref Rudder[index]));    
            ArrayType.Accept(visitor,GyroField, GyroField.DataType, 2,
                (index, v, f, t) => UInt16Type.Accept(v, f, t, ref Gyro[index]));    
            ArrayType.Accept(visitor,PitchField, PitchField.DataType, 5,
                (index, v, f, t) => UInt16Type.Accept(v, f, t, ref Pitch[index]));    
            ArrayType.Accept(visitor,ThrottleField, ThrottleField.DataType, 5,
                (index, v, f, t) => UInt16Type.Accept(v, f, t, ref Throttle[index]));    

        }

        /// <summary>
        /// Aileron setpoints: left, center, right
        /// OriginName: aileron, Units: , IsExtended: false
        /// </summary>
        public static readonly Field AileronField = new Field.Builder()
            .Name(nameof(Aileron))
            .Title("aileron")
            .Description("Aileron setpoints: left, center, right")

            .DataType(new ArrayType(UInt16Type.Default,3))
        .Build();
        public const int AileronMaxItemsCount = 3;
        public ushort[] Aileron { get; } = new ushort[3];
        /// <summary>
        /// Elevator setpoints: nose down, center, nose up
        /// OriginName: elevator, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ElevatorField = new Field.Builder()
            .Name(nameof(Elevator))
            .Title("elevator")
            .Description("Elevator setpoints: nose down, center, nose up")

            .DataType(new ArrayType(UInt16Type.Default,3))
        .Build();
        public const int ElevatorMaxItemsCount = 3;
        public ushort[] Elevator { get; } = new ushort[3];
        /// <summary>
        /// Rudder setpoints: nose left, center, nose right
        /// OriginName: rudder, Units: , IsExtended: false
        /// </summary>
        public static readonly Field RudderField = new Field.Builder()
            .Name(nameof(Rudder))
            .Title("rudder")
            .Description("Rudder setpoints: nose left, center, nose right")

            .DataType(new ArrayType(UInt16Type.Default,3))
        .Build();
        public const int RudderMaxItemsCount = 3;
        public ushort[] Rudder { get; } = new ushort[3];
        /// <summary>
        /// Tail gyro mode/gain setpoints: heading hold, rate mode
        /// OriginName: gyro, Units: , IsExtended: false
        /// </summary>
        public static readonly Field GyroField = new Field.Builder()
            .Name(nameof(Gyro))
            .Title("gyro")
            .Description("Tail gyro mode/gain setpoints: heading hold, rate mode")

            .DataType(new ArrayType(UInt16Type.Default,2))
        .Build();
        public const int GyroMaxItemsCount = 2;
        public ushort[] Gyro { get; } = new ushort[2];
        /// <summary>
        /// Pitch curve setpoints (every 25%)
        /// OriginName: pitch, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PitchField = new Field.Builder()
            .Name(nameof(Pitch))
            .Title("pitch")
            .Description("Pitch curve setpoints (every 25%)")

            .DataType(new ArrayType(UInt16Type.Default,5))
        .Build();
        public const int PitchMaxItemsCount = 5;
        public ushort[] Pitch { get; } = new ushort[5];
        [Obsolete("This method is deprecated. Use GetPitchMaxItemsCount instead.")]
        public byte GetPitchMaxItemsCount() => 5;
        /// <summary>
        /// Throttle curve setpoints (every 25%)
        /// OriginName: throttle, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ThrottleField = new Field.Builder()
            .Name(nameof(Throttle))
            .Title("throttle")
            .Description("Throttle curve setpoints (every 25%)")

            .DataType(new ArrayType(UInt16Type.Default,5))
        .Build();
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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +1 // uint8_t mode
            +1 // uint8_t nav_mode
            +1 // uint8_t pilot
            );
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

        public void Accept(IVisitor visitor)
        {
            UInt8Type.Accept(visitor,ModeField, ModeField.DataType, ref _mode);    
            UInt8Type.Accept(visitor,NavModeField, NavModeField.DataType, ref _navMode);    
            UInt8Type.Accept(visitor,PilotField, PilotField.DataType, ref _pilot);    

        }

        /// <summary>
        /// System mode, see UALBERTA_AUTOPILOT_MODE ENUM
        /// OriginName: mode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field ModeField = new Field.Builder()
            .Name(nameof(Mode))
            .Title("mode")
            .Description("System mode, see UALBERTA_AUTOPILOT_MODE ENUM")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _mode;
        public byte Mode { get => _mode; set => _mode = value; }
        /// <summary>
        /// Navigation mode, see UALBERTA_NAV_MODE ENUM
        /// OriginName: nav_mode, Units: , IsExtended: false
        /// </summary>
        public static readonly Field NavModeField = new Field.Builder()
            .Name(nameof(NavMode))
            .Title("nav_mode")
            .Description("Navigation mode, see UALBERTA_NAV_MODE ENUM")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _navMode;
        public byte NavMode { get => _navMode; set => _navMode = value; }
        /// <summary>
        /// Pilot mode, see UALBERTA_PILOT_MODE
        /// OriginName: pilot, Units: , IsExtended: false
        /// </summary>
        public static readonly Field PilotField = new Field.Builder()
            .Name(nameof(Pilot))
            .Title("pilot")
            .Description("Pilot mode, see UALBERTA_PILOT_MODE")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _pilot;
        public byte Pilot { get => _pilot; set => _pilot = value; }
    }




        


#endregion


}
