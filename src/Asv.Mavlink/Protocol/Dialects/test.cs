// MIT License
//
// Copyright (c) 2018 Alexey (https://github.com/asvol)
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

// This code was generate by tool Asv.Mavlink.Shell version 1.2.2

using System;
using System.Text;
using Asv.Mavlink.V2.Common;
using Asv.IO;

namespace Asv.Mavlink.V2.Test
{

    public static class TestHelper
    {
        public static void RegisterTestDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new TestTypesPacket());
        }
    }

#region Enums


#endregion

#region Messages

    /// <summary>
    /// Test all field types
    ///  TEST_TYPES
    /// </summary>
    public class TestTypesPacket: PacketV2<TestTypesPayload>
    {
	    public const int PacketMessageId = 0;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 103;

        public override TestTypesPayload Payload { get; } = new TestTypesPayload();

        public override string Name => "TEST_TYPES";
    }

    /// <summary>
    ///  TEST_TYPES
    /// </summary>
    public class TestTypesPayload : IPayload
    {
        public byte GetMaxByteSize() => 179; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 179; // of byte sized of fields (exclude extended)

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
        public ulong[] U64Array { get; set; } = new ulong[3];
        public byte GetU64ArrayMaxItemsCount() => 3;
        /// <summary>
        /// int64_t_array
        /// OriginName: s64_array, Units: , IsExtended: false
        /// </summary>
        public long[] S64Array { get; } = new long[3];
        /// <summary>
        /// double_array
        /// OriginName: d_array, Units: , IsExtended: false
        /// </summary>
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
        public uint[] U32Array { get; } = new uint[3];
        /// <summary>
        /// int32_t_array
        /// OriginName: s32_array, Units: , IsExtended: false
        /// </summary>
        public int[] S32Array { get; } = new int[3];
        /// <summary>
        /// float_array
        /// OriginName: f_array, Units: , IsExtended: false
        /// </summary>
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
        public ushort[] U16Array { get; } = new ushort[3];
        /// <summary>
        /// int16_t_array
        /// OriginName: s16_array, Units: , IsExtended: false
        /// </summary>
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
        public byte[] U8Array { get; } = new byte[3];
        /// <summary>
        /// int8_t_array
        /// OriginName: s8_array, Units: , IsExtended: false
        /// </summary>
        public sbyte[] S8Array { get; } = new sbyte[3];
    }


#endregion


}
