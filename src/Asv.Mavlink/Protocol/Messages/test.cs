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
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
            +8 // uint64_t u64
            +8 // int64_t s64
            +8 // double d
            +U64Array.Length * 8 // uint64_t[3] u64_array
            +S64Array.Length * 8 // int64_t[3] s64_array
            +DArray.Length * 8 // double[3] d_array
            +4 // uint32_t u32
            +4 // int32_t s32
            +4 // float f
            +U32Array.Length * 4 // uint32_t[3] u32_array
            +S32Array.Length * 4 // int32_t[3] s32_array
            +FArray.Length * 4 // float[3] f_array
            +2 // uint16_t u16
            +2 // int16_t s16
            +U16Array.Length * 2 // uint16_t[3] u16_array
            +S16Array.Length * 2 // int16_t[3] s16_array
            +1 // char c
            +S.Length // char[10] s
            +1 // uint8_t u8
            +1 // int8_t s8
            +U8Array.Length // uint8_t[3] u8_array
            +S8Array.Length // int8_t[3] s8_array
            );
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            U64 = BinSerialize.ReadULong(ref buffer);
            S64 = BinSerialize.ReadLong(ref buffer);
            D = BinSerialize.ReadDouble(ref buffer);
            arraySize = /*ArrayLength*/3 - Math.Max(0,((/*PayloadByteSize*/179 - payloadSize - /*ExtendedFieldsLength*/0)/8 /*FieldTypeByteSize*/));
            
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
            buffer = buffer[arraySize..];
           
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

        public void Accept(IVisitor visitor)
        {
            UInt64Type.Accept(visitor,U64Field, ref _u64);    
            Int64Type.Accept(visitor,S64Field, ref _s64);    
            DoubleType.Accept(visitor,DField, ref _d);
            ArrayType.Accept(visitor,U64ArrayField, 
                (index, v, f, t) => UInt64Type.Accept(v, f, t, ref U64Array[index]));    
            ArrayType.Accept(visitor,S64ArrayField, 
                (index, v, f, t) => Int64Type.Accept(v, f, t, ref S64Array[index]));    
            ArrayType.Accept(visitor,DArrayField, 
                (index, v, f, t) => DoubleType.Accept(v, f, t, ref DArray[index]));    
            UInt32Type.Accept(visitor,U32Field, ref _u32);    
            Int32Type.Accept(visitor,S32Field, ref _s32);    
            FloatType.Accept(visitor,FField, ref _f);    
            ArrayType.Accept(visitor,U32ArrayField, 
                (index, v, f, t) => UInt32Type.Accept(v, f, t, ref U32Array[index]));    
            ArrayType.Accept(visitor,S32ArrayField, 
                (index, v, f, t) => Int32Type.Accept(v, f, t, ref S32Array[index]));
            ArrayType.Accept(visitor,FArrayField, 
                (index, v, f, t) => FloatType.Accept(v, f, t, ref FArray[index]));
            UInt16Type.Accept(visitor,U16Field, ref _u16);    
            Int16Type.Accept(visitor,S16Field, ref _s16);
            ArrayType.Accept(visitor,U16ArrayField, 
                (index, v, f, t) => UInt16Type.Accept(v, f, t, ref U16Array[index]));    
            ArrayType.Accept(visitor,S16ArrayField, 
                (index, v, f, t) => Int16Type.Accept(v, f, t, ref S16Array[index]));    
            CharType.Accept(visitor,CField, ref _c);
            ArrayType.Accept(visitor,SField,  
                (index, v, f, t) => CharType.Accept(v, f, t, ref S[index]));
            UInt8Type.Accept(visitor,U8Field, ref _u8);    
            Int8Type.Accept(visitor,S8Field, ref _s8);                
            ArrayType.Accept(visitor,U8ArrayField, 
                (index, v, f, t) => UInt8Type.Accept(v, f, t, ref U8Array[index]));    
            ArrayType.Accept(visitor,S8ArrayField,  
                (index, v, f, t) => Int8Type.Accept(v, f, t, ref S8Array[index]));

        }

        /// <summary>
        /// uint64_t
        /// OriginName: u64, Units: , IsExtended: false
        /// </summary>
        public static readonly Field U64Field = new Field.Builder()
            .Name(nameof(U64))
            .Title("u64")
            .Description("uint64_t")

            .DataType(UInt64Type.Default)
        .Build();
        private ulong _u64;
        public ulong U64 { get => _u64; set => _u64 = value; }
        /// <summary>
        /// int64_t
        /// OriginName: s64, Units: , IsExtended: false
        /// </summary>
        public static readonly Field S64Field = new Field.Builder()
            .Name(nameof(S64))
            .Title("s64")
            .Description("int64_t")

            .DataType(Int64Type.Default)
        .Build();
        private long _s64;
        public long S64 { get => _s64; set => _s64 = value; }
        /// <summary>
        /// double
        /// OriginName: d, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DField = new Field.Builder()
            .Name(nameof(D))
            .Title("d")
            .Description("double")

            .DataType(DoubleType.Default)
        .Build();
        private double _d;
        public double D { get => _d; set => _d = value; }
        /// <summary>
        /// uint64_t_array
        /// OriginName: u64_array, Units: , IsExtended: false
        /// </summary>
        public static readonly Field U64ArrayField = new Field.Builder()
            .Name(nameof(U64Array))
            .Title("u64_array")
            .Description("uint64_t_array")

            .DataType(new ArrayType(UInt64Type.Default,3))            
        .Build();
        public const int U64ArrayMaxItemsCount = 3;
        public ulong[] U64Array { get; } = new ulong[3];
        [Obsolete("This method is deprecated. Use GetU64ArrayMaxItemsCount instead.")]
        public byte GetU64ArrayMaxItemsCount() => 3;
        /// <summary>
        /// int64_t_array
        /// OriginName: s64_array, Units: , IsExtended: false
        /// </summary>
        public static readonly Field S64ArrayField = new Field.Builder()
            .Name(nameof(S64Array))
            .Title("s64_array")
            .Description("int64_t_array")

            .DataType(new ArrayType(Int64Type.Default,3))        
        .Build();
        public const int S64ArrayMaxItemsCount = 3;
        public long[] S64Array { get; } = new long[3];
        /// <summary>
        /// double_array
        /// OriginName: d_array, Units: , IsExtended: false
        /// </summary>
        public static readonly Field DArrayField = new Field.Builder()
            .Name(nameof(DArray))
            .Title("d_array")
            .Description("double_array")

            .DataType(new ArrayType(DoubleType.Default,3))        
        .Build();
        public const int DArrayMaxItemsCount = 3;
        public double[] DArray { get; } = new double[3];
        /// <summary>
        /// uint32_t
        /// OriginName: u32, Units: , IsExtended: false
        /// </summary>
        public static readonly Field U32Field = new Field.Builder()
            .Name(nameof(U32))
            .Title("u32")
            .Description("uint32_t")
.FormatString("0x%08x")
            .DataType(UInt32Type.Default)
        .Build();
        private uint _u32;
        public uint U32 { get => _u32; set => _u32 = value; }
        /// <summary>
        /// int32_t
        /// OriginName: s32, Units: , IsExtended: false
        /// </summary>
        public static readonly Field S32Field = new Field.Builder()
            .Name(nameof(S32))
            .Title("s32")
            .Description("int32_t")

            .DataType(Int32Type.Default)
        .Build();
        private int _s32;
        public int S32 { get => _s32; set => _s32 = value; }
        /// <summary>
        /// float
        /// OriginName: f, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FField = new Field.Builder()
            .Name(nameof(F))
            .Title("f")
            .Description("float")

            .DataType(FloatType.Default)
        .Build();
        private float _f;
        public float F { get => _f; set => _f = value; }
        /// <summary>
        /// uint32_t_array
        /// OriginName: u32_array, Units: , IsExtended: false
        /// </summary>
        public static readonly Field U32ArrayField = new Field.Builder()
            .Name(nameof(U32Array))
            .Title("u32_array")
            .Description("uint32_t_array")

            .DataType(new ArrayType(UInt32Type.Default,3))        
        .Build();
        public const int U32ArrayMaxItemsCount = 3;
        public uint[] U32Array { get; } = new uint[3];
        /// <summary>
        /// int32_t_array
        /// OriginName: s32_array, Units: , IsExtended: false
        /// </summary>
        public static readonly Field S32ArrayField = new Field.Builder()
            .Name(nameof(S32Array))
            .Title("s32_array")
            .Description("int32_t_array")

            .DataType(new ArrayType(Int32Type.Default,3))        
        .Build();
        public const int S32ArrayMaxItemsCount = 3;
        public int[] S32Array { get; } = new int[3];
        /// <summary>
        /// float_array
        /// OriginName: f_array, Units: , IsExtended: false
        /// </summary>
        public static readonly Field FArrayField = new Field.Builder()
            .Name(nameof(FArray))
            .Title("f_array")
            .Description("float_array")

            .DataType(new ArrayType(FloatType.Default,3))        
        .Build();
        public const int FArrayMaxItemsCount = 3;
        public float[] FArray { get; } = new float[3];
        /// <summary>
        /// uint16_t
        /// OriginName: u16, Units: , IsExtended: false
        /// </summary>
        public static readonly Field U16Field = new Field.Builder()
            .Name(nameof(U16))
            .Title("u16")
            .Description("uint16_t")

            .DataType(UInt16Type.Default)
        .Build();
        private ushort _u16;
        public ushort U16 { get => _u16; set => _u16 = value; }
        /// <summary>
        /// int16_t
        /// OriginName: s16, Units: , IsExtended: false
        /// </summary>
        public static readonly Field S16Field = new Field.Builder()
            .Name(nameof(S16))
            .Title("s16")
            .Description("int16_t")

            .DataType(Int16Type.Default)
        .Build();
        private short _s16;
        public short S16 { get => _s16; set => _s16 = value; }
        /// <summary>
        /// uint16_t_array
        /// OriginName: u16_array, Units: , IsExtended: false
        /// </summary>
        public static readonly Field U16ArrayField = new Field.Builder()
            .Name(nameof(U16Array))
            .Title("u16_array")
            .Description("uint16_t_array")

            .DataType(new ArrayType(UInt16Type.Default,3))
        .Build();
        public const int U16ArrayMaxItemsCount = 3;
        public ushort[] U16Array { get; } = new ushort[3];
        /// <summary>
        /// int16_t_array
        /// OriginName: s16_array, Units: , IsExtended: false
        /// </summary>
        public static readonly Field S16ArrayField = new Field.Builder()
            .Name(nameof(S16Array))
            .Title("s16_array")
            .Description("int16_t_array")

            .DataType(new ArrayType(Int16Type.Default,3))
        .Build();
        public const int S16ArrayMaxItemsCount = 3;
        public short[] S16Array { get; } = new short[3];
        /// <summary>
        /// char
        /// OriginName: c, Units: , IsExtended: false
        /// </summary>
        public static readonly Field CField = new Field.Builder()
            .Name(nameof(C))
            .Title("c")
            .Description("char")

            .DataType(CharType.Ascii)
        .Build();
        private char _c;
        public char C { get => _c; set => _c = value; }
        /// <summary>
        /// string
        /// OriginName: s, Units: , IsExtended: false
        /// </summary>
        public static readonly Field SField = new Field.Builder()
            .Name(nameof(S))
            .Title("s")
            .Description("string")

            .DataType(new ArrayType(CharType.Ascii,10))
        .Build();
        public const int SMaxItemsCount = 10;
        public char[] S { get; } = new char[10];
        /// <summary>
        /// uint8_t
        /// OriginName: u8, Units: , IsExtended: false
        /// </summary>
        public static readonly Field U8Field = new Field.Builder()
            .Name(nameof(U8))
            .Title("u8")
            .Description("uint8_t")

            .DataType(UInt8Type.Default)
        .Build();
        private byte _u8;
        public byte U8 { get => _u8; set => _u8 = value; }
        /// <summary>
        /// int8_t
        /// OriginName: s8, Units: , IsExtended: false
        /// </summary>
        public static readonly Field S8Field = new Field.Builder()
            .Name(nameof(S8))
            .Title("s8")
            .Description("int8_t")

            .DataType(Int8Type.Default)
        .Build();
        private sbyte _s8;
        public sbyte S8 { get => _s8; set => _s8 = value; }
        /// <summary>
        /// uint8_t_array
        /// OriginName: u8_array, Units: , IsExtended: false
        /// </summary>
        public static readonly Field U8ArrayField = new Field.Builder()
            .Name(nameof(U8Array))
            .Title("u8_array")
            .Description("uint8_t_array")

            .DataType(new ArrayType(UInt8Type.Default,3))
        .Build();
        public const int U8ArrayMaxItemsCount = 3;
        public byte[] U8Array { get; } = new byte[3];
        /// <summary>
        /// int8_t_array
        /// OriginName: s8_array, Units: , IsExtended: false
        /// </summary>
        public static readonly Field S8ArrayField = new Field.Builder()
            .Name(nameof(S8Array))
            .Title("s8_array")
            .Description("int8_t_array")

            .DataType(new ArrayType(Int8Type.Default,3))
        .Build();
        public const int S8ArrayMaxItemsCount = 3;
        public sbyte[] S8Array { get; } = new sbyte[3];
    }




        


#endregion


}
