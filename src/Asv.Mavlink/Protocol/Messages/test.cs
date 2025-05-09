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

namespace Asv.Mavlink.Test
{

    public static class TestHelper
    {
        public static void RegisterTestDialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            src.Add(TestTypesPacket.MessageId, ()=>new TestTypesPacket());
        }
    }

#region Enums


#endregion

#region Messages

    /// <summary>
    /// Test all field types
    ///  TEST_TYPES
    /// </summary>
    public class TestTypesPacket : MavlinkV2Message<TestTypesPayload>
    {
        public const int MessageId = 17000;
        
        public const byte CrcExtra = 103;
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => false;

        public override TestTypesPayload Payload { get; } = new();

        public override string Name => "TEST_TYPES";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
            new("u64",
            "uint64_t",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            0, 
            false),
            new("s64",
            "int64_t",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int64, 
            0, 
            false),
            new("d",
            "double",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Double, 
            0, 
            false),
            new("u64_array",
            "uint64_t_array",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint64, 
            3, 
            false),
            new("s64_array",
            "int64_t_array",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int64, 
            3, 
            false),
            new("d_array",
            "double_array",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Double, 
            3, 
            false),
            new("u32",
            "uint32_t",
            "0x%08x", 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            0, 
            false),
            new("s32",
            "int32_t",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            0, 
            false),
            new("f",
            "float",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            0, 
            false),
            new("u32_array",
            "uint32_t_array",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint32, 
            3, 
            false),
            new("s32_array",
            "int32_t_array",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int32, 
            3, 
            false),
            new("f_array",
            "float_array",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Float32, 
            3, 
            false),
            new("u16",
            "uint16_t",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            0, 
            false),
            new("s16",
            "int16_t",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            0, 
            false),
            new("u16_array",
            "uint16_t_array",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint16, 
            3, 
            false),
            new("s16_array",
            "int16_t_array",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int16, 
            3, 
            false),
            new("c",
            "char",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            0, 
            false),
            new("s",
            "string",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Char, 
            10, 
            false),
            new("u8",
            "uint8_t",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            0, 
            false),
            new("s8",
            "int8_t",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int8, 
            0, 
            false),
            new("u8_array",
            "uint8_t_array",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Uint8, 
            3, 
            false),
            new("s8_array",
            "int8_t_array",
            string.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            MessageFieldType.Int8, 
            3, 
            false),
        ];
        public const string FormatMessage = "TEST_TYPES:"
        + "uint64_t u64;"
        + "int64_t s64;"
        + "double d;"
        + "uint64_t[3] u64_array;"
        + "int64_t[3] s64_array;"
        + "double[3] d_array;"
        + "uint32_t u32;"
        + "int32_t s32;"
        + "float f;"
        + "uint32_t[3] u32_array;"
        + "int32_t[3] s32_array;"
        + "float[3] f_array;"
        + "uint16_t u16;"
        + "int16_t s16;"
        + "uint16_t[3] u16_array;"
        + "int16_t[3] s16_array;"
        + "char c;"
        + "char[10] s;"
        + "uint8_t u8;"
        + "int8_t s8;"
        + "uint8_t[3] u8_array;"
        + "int8_t[3] s8_array;"
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
    }

    /// <summary>
    ///  TEST_TYPES
    /// </summary>
    public class TestTypesPayload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => 179; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => 179; // of byte sized of fields (exclude extended)
        
        public int GetByteSize()
        {
            var sum = 0;
            sum+=8; //U64
            sum+=8; //S64
            sum+=8; //D
            sum+=U64Array.Length * 8; //U64Array
            sum+=S64Array.Length * 8; //S64Array
            sum+=DArray.Length * 8; //DArray
            sum+=4; //U32
            sum+=4; //S32
            sum+=4; //F
            sum+=U32Array.Length * 4; //U32Array
            sum+=S32Array.Length * 4; //S32Array
            sum+=FArray.Length * 4; //FArray
            sum+=2; //U16
            sum+=2; //S16
            sum+=U16Array.Length * 2; //U16Array
            sum+=S16Array.Length * 2; //S16Array
            sum+=1; //C
            sum+=S.Length; //S
            sum+=1; //U8
            sum+=1; //S8
            sum+=U8Array.Length; //U8Array
            sum+=S8Array.Length; //S8Array
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            U64 = BinSerialize.ReadULong(ref buffer);
            S64 = BinSerialize.ReadLong(ref buffer);
            D = BinSerialize.ReadDouble(ref buffer);
            arraySize = /*ArrayLength*/3 - Math.Max(0,((/*PayloadByteSize*/179 - payloadSize - /*ExtendedFieldsLength*/0)/8 /*FieldTypeByteSize*/));
            U64Array = new ulong[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                U64Array[i] = BinSerialize.ReadULong(ref buffer);
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                S64Array[i] = BinSerialize.ReadLong(ref buffer);
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                DArray[i] = BinSerialize.ReadDouble(ref buffer);
            }
            U32 = BinSerialize.ReadUInt(ref buffer);
            S32 = BinSerialize.ReadInt(ref buffer);
            F = BinSerialize.ReadFloat(ref buffer);
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                U32Array[i] = BinSerialize.ReadUInt(ref buffer);
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                S32Array[i] = BinSerialize.ReadInt(ref buffer);
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                FArray[i] = BinSerialize.ReadFloat(ref buffer);
            }
            U16 = BinSerialize.ReadUShort(ref buffer);
            S16 = BinSerialize.ReadShort(ref buffer);
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                U16Array[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                S16Array[i] = BinSerialize.ReadShort(ref buffer);
            }
            C = (char)buffer[0];
            buffer = buffer.Slice(1);
            
            arraySize = 10;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = S)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, S.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           
            U8 = (byte)BinSerialize.ReadByte(ref buffer);
            S8 = (sbyte)BinSerialize.ReadByte(ref buffer);
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                U8Array[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                S8Array[i] = (sbyte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteULong(ref buffer,U64);
            BinSerialize.WriteLong(ref buffer,S64);
            BinSerialize.WriteDouble(ref buffer,D);
            for(var i=0;i<U64Array.Length;i++)
            {
                BinSerialize.WriteULong(ref buffer,U64Array[i]);
            }
            for(var i=0;i<S64Array.Length;i++)
            {
                BinSerialize.WriteLong(ref buffer,S64Array[i]);
            }
            for(var i=0;i<DArray.Length;i++)
            {
                BinSerialize.WriteDouble(ref buffer,DArray[i]);
            }
            BinSerialize.WriteUInt(ref buffer,U32);
            BinSerialize.WriteInt(ref buffer,S32);
            BinSerialize.WriteFloat(ref buffer,F);
            for(var i=0;i<U32Array.Length;i++)
            {
                BinSerialize.WriteUInt(ref buffer,U32Array[i]);
            }
            for(var i=0;i<S32Array.Length;i++)
            {
                BinSerialize.WriteInt(ref buffer,S32Array[i]);
            }
            for(var i=0;i<FArray.Length;i++)
            {
                BinSerialize.WriteFloat(ref buffer,FArray[i]);
            }
            BinSerialize.WriteUShort(ref buffer,U16);
            BinSerialize.WriteShort(ref buffer,S16);
            for(var i=0;i<U16Array.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,U16Array[i]);
            }
            for(var i=0;i<S16Array.Length;i++)
            {
                BinSerialize.WriteShort(ref buffer,S16Array[i]);
            }
            BinSerialize.WriteByte(ref buffer,(byte)C);
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = S)
                {
                    Encoding.ASCII.GetBytes(charPointer, S.Length, bytePointer, S.Length);
                }
            }
            buffer = buffer.Slice(S.Length);
            
            BinSerialize.WriteByte(ref buffer,(byte)U8);
            BinSerialize.WriteByte(ref buffer,(byte)S8);
            for(var i=0;i<U8Array.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)U8Array[i]);
            }
            for(var i=0;i<S8Array.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)S8Array[i]);
            }
            /* PayloadByteSize = 179 */;
        }
        
        



        /// <summary>
        /// uint64_t
        /// OriginName: u64, Units: , IsExtended: false
        /// </summary>
        public ulong U64 { get; set; }
        /// <summary>
        /// int64_t
        /// OriginName: s64, Units: , IsExtended: false
        /// </summary>
        public long S64 { get; set; }
        /// <summary>
        /// double
        /// OriginName: d, Units: , IsExtended: false
        /// </summary>
        public double D { get; set; }
        /// <summary>
        /// uint64_t_array
        /// OriginName: u64_array, Units: , IsExtended: false
        /// </summary>
        public const int U64ArrayMaxItemsCount = 3;
        public ulong[] U64Array { get; set; } = new ulong[3];
        [Obsolete("This method is deprecated. Use GetU64ArrayMaxItemsCount instead.")]
        public byte GetU64ArrayMaxItemsCount() => 3;
        /// <summary>
        /// int64_t_array
        /// OriginName: s64_array, Units: , IsExtended: false
        /// </summary>
        public const int S64ArrayMaxItemsCount = 3;
        public long[] S64Array { get; } = new long[3];
        /// <summary>
        /// double_array
        /// OriginName: d_array, Units: , IsExtended: false
        /// </summary>
        public const int DArrayMaxItemsCount = 3;
        public double[] DArray { get; } = new double[3];
        /// <summary>
        /// uint32_t
        /// OriginName: u32, Units: , IsExtended: false
        /// </summary>
        public uint U32 { get; set; }
        /// <summary>
        /// int32_t
        /// OriginName: s32, Units: , IsExtended: false
        /// </summary>
        public int S32 { get; set; }
        /// <summary>
        /// float
        /// OriginName: f, Units: , IsExtended: false
        /// </summary>
        public float F { get; set; }
        /// <summary>
        /// uint32_t_array
        /// OriginName: u32_array, Units: , IsExtended: false
        /// </summary>
        public const int U32ArrayMaxItemsCount = 3;
        public uint[] U32Array { get; } = new uint[3];
        /// <summary>
        /// int32_t_array
        /// OriginName: s32_array, Units: , IsExtended: false
        /// </summary>
        public const int S32ArrayMaxItemsCount = 3;
        public int[] S32Array { get; } = new int[3];
        /// <summary>
        /// float_array
        /// OriginName: f_array, Units: , IsExtended: false
        /// </summary>
        public const int FArrayMaxItemsCount = 3;
        public float[] FArray { get; } = new float[3];
        /// <summary>
        /// uint16_t
        /// OriginName: u16, Units: , IsExtended: false
        /// </summary>
        public ushort U16 { get; set; }
        /// <summary>
        /// int16_t
        /// OriginName: s16, Units: , IsExtended: false
        /// </summary>
        public short S16 { get; set; }
        /// <summary>
        /// uint16_t_array
        /// OriginName: u16_array, Units: , IsExtended: false
        /// </summary>
        public const int U16ArrayMaxItemsCount = 3;
        public ushort[] U16Array { get; } = new ushort[3];
        /// <summary>
        /// int16_t_array
        /// OriginName: s16_array, Units: , IsExtended: false
        /// </summary>
        public const int S16ArrayMaxItemsCount = 3;
        public short[] S16Array { get; } = new short[3];
        /// <summary>
        /// char
        /// OriginName: c, Units: , IsExtended: false
        /// </summary>
        public char C { get; set; }
        /// <summary>
        /// string
        /// OriginName: s, Units: , IsExtended: false
        /// </summary>
        public const int SMaxItemsCount = 10;
        public char[] S { get; } = new char[10];
        /// <summary>
        /// uint8_t
        /// OriginName: u8, Units: , IsExtended: false
        /// </summary>
        public byte U8 { get; set; }
        /// <summary>
        /// int8_t
        /// OriginName: s8, Units: , IsExtended: false
        /// </summary>
        public sbyte S8 { get; set; }
        /// <summary>
        /// uint8_t_array
        /// OriginName: u8_array, Units: , IsExtended: false
        /// </summary>
        public const int U8ArrayMaxItemsCount = 3;
        public byte[] U8Array { get; } = new byte[3];
        /// <summary>
        /// int8_t_array
        /// OriginName: s8_array, Units: , IsExtended: false
        /// </summary>
        public const int S8ArrayMaxItemsCount = 3;
        public sbyte[] S8Array { get; } = new sbyte[3];
    }


#endregion


}
