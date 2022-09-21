// MIT License
//
// Copyright (c) 2018 Alexey Voloshkevich Cursir ltd. (https://github.com/asvol)
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

// This code was generate by tool Asv.Mavlink.Shell version 1.0.0

using System;
using System.Text;
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
	    public const int PacketMessageId = 17000;
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

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
            U64 = BinSerialize.ReadULong(ref buffer);index+=8;
            S64 = BinSerialize.ReadLong(ref buffer);index+=8;
            D = BinSerialize.ReadDouble(ref buffer);index+=8;
            arraySize = /*ArrayLength*/3 - Math.Max(0,((/*PayloadByteSize*/179 - payloadSize - /*ExtendedFieldsLength*/0)/8 /*FieldTypeByteSize*/));
            U64Array = new ulong[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                U64Array[i] = BinSerialize.ReadULong(ref buffer);index+=8;
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                S64Array[i] = BinSerialize.ReadLong(ref buffer);index+=8;
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                DArray[i] = BinSerialize.ReadDouble(ref buffer);index+=8;
            }
            U32 = BinSerialize.ReadUInt(ref buffer);index+=4;
            S32 = BinSerialize.ReadInt(ref buffer);index+=4;
            F = BinSerialize.ReadFloat(ref buffer);index+=4;
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                U32Array[i] = BinSerialize.ReadUInt(ref buffer);index+=4;
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                S32Array[i] = BinSerialize.ReadInt(ref buffer);index+=4;
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                FArray[i] = BinSerialize.ReadFloat(ref buffer);index+=4;
            }
            U16 = BinSerialize.ReadUShort(ref buffer);index+=2;
            S16 = BinSerialize.ReadShort(ref buffer);index+=2;
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                U16Array[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                S16Array[i] = BinSerialize.ReadShort(ref buffer);index+=2;
            }
            C = (char)buffer[0];
            buffer = buffer.Slice(1);
            index+=1;
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
            index+=arraySize;
            U8 = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            S8 = (sbyte)BinSerialize.ReadByte(ref buffer);index+=1;
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                U8Array[i] = (byte)BinSerialize.ReadByte(ref buffer);index+=1;
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                S8Array[i] = (sbyte)BinSerialize.ReadByte(ref buffer);index+=1;
            }

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
            BinSerialize.WriteULong(ref buffer,U64);index+=8;
            BinSerialize.WriteLong(ref buffer,S64);index+=8;
            BinSerialize.WriteDouble(ref buffer,D);index+=8;
            for(var i=0;i<U64Array.Length;i++)
            {
                BinSerialize.WriteULong(ref buffer,U64Array[i]);index+=8;
            }
            for(var i=0;i<S64Array.Length;i++)
            {
                BinSerialize.WriteLong(ref buffer,S64Array[i]);index+=8;
            }
            for(var i=0;i<DArray.Length;i++)
            {
                BinSerialize.WriteDouble(ref buffer,DArray[i]);index+=8;
            }
            BinSerialize.WriteUInt(ref buffer,U32);index+=4;
            BinSerialize.WriteInt(ref buffer,S32);index+=4;
            BinSerialize.WriteFloat(ref buffer,F);index+=4;
            for(var i=0;i<U32Array.Length;i++)
            {
                BinSerialize.WriteUInt(ref buffer,U32Array[i]);index+=4;
            }
            for(var i=0;i<S32Array.Length;i++)
            {
                BinSerialize.WriteInt(ref buffer,S32Array[i]);index+=4;
            }
            for(var i=0;i<FArray.Length;i++)
            {
                BinSerialize.WriteFloat(ref buffer,FArray[i]);index+=4;
            }
            BinSerialize.WriteUShort(ref buffer,U16);index+=2;
            BinSerialize.WriteShort(ref buffer,S16);index+=2;
            for(var i=0;i<U16Array.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,U16Array[i]);index+=2;
            }
            for(var i=0;i<S16Array.Length;i++)
            {
                BinSerialize.WriteShort(ref buffer,S16Array[i]);index+=2;
            }
            BinSerialize.WriteByte(ref buffer,(byte)C);index+=1;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = S)
                {
                    Encoding.ASCII.GetBytes(charPointer, S.Length, bytePointer, S.Length);
                }
            }
            buffer = buffer.Slice(S.Length);
            index+=S.Length;
            BinSerialize.WriteByte(ref buffer,(byte)U8);index+=1;
            BinSerialize.WriteByte(ref buffer,(byte)S8);index+=1;
            for(var i=0;i<U8Array.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)U8Array[i]);index+=1;
            }
            for(var i=0;i<S8Array.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)S8Array[i]);index+=1;
            }
            return index; // /*PayloadByteSize*/179;
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
            U64 = BitConverter.ToUInt64(buffer,index);index+=8;
            S64 = BitConverter.ToInt64(buffer,index);index+=8;
            D = BitConverter.ToDouble(buffer, index);index+=8;
            arraySize = /*ArrayLength*/3 - Math.Max(0,((/*PayloadByteSize*/179 - payloadSize - /*ExtendedFieldsLength*/0)/8 /*FieldTypeByteSize*/));
            U64Array = new ulong[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                U64Array[i] = BitConverter.ToUInt64(buffer,index);index+=8;
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                S64Array[i] = BitConverter.ToInt64(buffer,index);index+=8;
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                DArray[i] = BitConverter.ToDouble(buffer, index);index+=8;
            }
            U32 = BitConverter.ToUInt32(buffer,index);index+=4;
            S32 = BitConverter.ToInt32(buffer,index);index+=4;
            F = BitConverter.ToSingle(buffer, index);index+=4;
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                U32Array[i] = BitConverter.ToUInt32(buffer,index);index+=4;
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                S32Array[i] = BitConverter.ToInt32(buffer,index);index+=4;
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                FArray[i] = BitConverter.ToSingle(buffer, index);index+=4;
            }
            U16 = BitConverter.ToUInt16(buffer,index);index+=2;
            S16 = BitConverter.ToInt16(buffer,index);index+=2;
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                U16Array[i] = BitConverter.ToUInt16(buffer,index);index+=2;
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                S16Array[i] = BitConverter.ToInt16(buffer,index);index+=2;
            }
            C = Encoding.ASCII.GetChars(buffer,index,1)[0];
            index+=1;
            arraySize = 10;
            Encoding.ASCII.GetChars(buffer, index,arraySize,S,0);
            index+=arraySize;
            U8 = (byte)buffer[index++];
            S8 = (sbyte)buffer[index++];
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                U8Array[i] = (byte)buffer[index++];
            }
            arraySize = 3;
            for(var i=0;i<arraySize;i++)
            {
                S8Array[i] = (sbyte)buffer[index++];
            }
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
            BitConverter.GetBytes(U64).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(S64).CopyTo(buffer, index);index+=8;
            BitConverter.GetBytes(D).CopyTo(buffer, index);index+=8;
            for(var i=0;i<U64Array.Length;i++)
            {
                BitConverter.GetBytes(U64Array[i]).CopyTo(buffer, index);index+=8;
            }
            for(var i=0;i<S64Array.Length;i++)
            {
                BitConverter.GetBytes(S64Array[i]).CopyTo(buffer, index);index+=8;
            }
            for(var i=0;i<DArray.Length;i++)
            {
                BitConverter.GetBytes(DArray[i]).CopyTo(buffer, index);index+=8;
            }
            BitConverter.GetBytes(U32).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(S32).CopyTo(buffer, index);index+=4;
            BitConverter.GetBytes(F).CopyTo(buffer, index);index+=4;
            for(var i=0;i<U32Array.Length;i++)
            {
                BitConverter.GetBytes(U32Array[i]).CopyTo(buffer, index);index+=4;
            }
            for(var i=0;i<S32Array.Length;i++)
            {
                BitConverter.GetBytes(S32Array[i]).CopyTo(buffer, index);index+=4;
            }
            for(var i=0;i<FArray.Length;i++)
            {
                BitConverter.GetBytes(FArray[i]).CopyTo(buffer, index);index+=4;
            }
            BitConverter.GetBytes(U16).CopyTo(buffer, index);index+=2;
            BitConverter.GetBytes(S16).CopyTo(buffer, index);index+=2;
            for(var i=0;i<U16Array.Length;i++)
            {
                BitConverter.GetBytes(U16Array[i]).CopyTo(buffer, index);index+=2;
            }
            for(var i=0;i<S16Array.Length;i++)
            {
                BitConverter.GetBytes(S16Array[i]).CopyTo(buffer, index);index+=2;
            }
            BitConverter.GetBytes(C).CopyTo(buffer, index);index+=1;
            index+=Encoding.ASCII.GetBytes(S,0,S.Length,buffer,index);
            BitConverter.GetBytes(U8).CopyTo(buffer, index);index+=1;
            BitConverter.GetBytes(S8).CopyTo(buffer, index);index+=1;
            for(var i=0;i<U8Array.Length;i++)
            {
                buffer[index] = (byte)U8Array[i];index+=1;
            }
            for(var i=0;i<S8Array.Length;i++)
            {
                buffer[index] = (byte)S8Array[i];index+=1;
            }
            return index - start; // /*PayloadByteSize*/179;
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
