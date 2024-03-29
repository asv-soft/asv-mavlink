// MIT License
//
// Copyright (c) 2023 asv-soft (https://github.com/asv-soft)
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

// This code was generate by tool Asv.Mavlink.Shell version 3.2.5-alpha-11

using System;
using System.Text;
using Asv.IO;

namespace Asv.Mavlink.V2.PythonArrayTest
{

    public static class PythonArrayTestHelper
    {
        public static void RegisterPythonArrayTestDialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            src.Register(()=>new ArrayTest0Packet());
            src.Register(()=>new ArrayTest1Packet());
            src.Register(()=>new ArrayTest3Packet());
            src.Register(()=>new ArrayTest4Packet());
            src.Register(()=>new ArrayTest5Packet());
            src.Register(()=>new ArrayTest6Packet());
            src.Register(()=>new ArrayTest7Packet());
            src.Register(()=>new ArrayTest8Packet());
        }
    }

#region Enums


#endregion

#region Messages

    /// <summary>
    /// Array test #0.
    ///  ARRAY_TEST_0
    /// </summary>
    public class ArrayTest0Packet: PacketV2<ArrayTest0Payload>
    {
	    public const int PacketMessageId = 17150;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 26;
        public override bool WrapToV2Extension => false;

        public override ArrayTest0Payload Payload { get; } = new ArrayTest0Payload();

        public override string Name => "ARRAY_TEST_0";
    }

    /// <summary>
    ///  ARRAY_TEST_0
    /// </summary>
    public class ArrayTest0Payload : IPayload
    {
        public byte GetMaxByteSize() => 33; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 33; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=ArU32.Length * 4; //ArU32
            sum+=ArU16.Length * 2; //ArU16
            sum+=1; //V1
            sum+=ArI8.Length; //ArI8
            sum+=ArU8.Length; //ArU8
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/33 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            ArU32 = new uint[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                ArU32[i] = BinSerialize.ReadUInt(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                ArU16[i] = BinSerialize.ReadUShort(ref buffer);
            }
            V1 = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                ArI8[i] = (sbyte)BinSerialize.ReadByte(ref buffer);
            }
            arraySize = 4;
            for(var i=0;i<arraySize;i++)
            {
                ArU8[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<ArU32.Length;i++)
            {
                BinSerialize.WriteUInt(ref buffer,ArU32[i]);
            }
            for(var i=0;i<ArU16.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,ArU16[i]);
            }
            BinSerialize.WriteByte(ref buffer,(byte)V1);
            for(var i=0;i<ArI8.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)ArI8[i]);
            }
            for(var i=0;i<ArU8.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)ArU8[i]);
            }
            /* PayloadByteSize = 33 */;
        }
        
        



        /// <summary>
        /// Value array
        /// OriginName: ar_u32, Units: , IsExtended: false
        /// </summary>
        public uint[] ArU32 { get; set; } = new uint[4];
        public byte GetArU32MaxItemsCount() => 4;
        /// <summary>
        /// Value array
        /// OriginName: ar_u16, Units: , IsExtended: false
        /// </summary>
        public ushort[] ArU16 { get; } = new ushort[4];
        /// <summary>
        /// Stub field
        /// OriginName: v1, Units: , IsExtended: false
        /// </summary>
        public byte V1 { get; set; }
        /// <summary>
        /// Value array
        /// OriginName: ar_i8, Units: , IsExtended: false
        /// </summary>
        public sbyte[] ArI8 { get; } = new sbyte[4];
        /// <summary>
        /// Value array
        /// OriginName: ar_u8, Units: , IsExtended: false
        /// </summary>
        public byte[] ArU8 { get; } = new byte[4];
    }
    /// <summary>
    /// Array test #1.
    ///  ARRAY_TEST_1
    /// </summary>
    public class ArrayTest1Packet: PacketV2<ArrayTest1Payload>
    {
	    public const int PacketMessageId = 17151;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 72;
        public override bool WrapToV2Extension => false;

        public override ArrayTest1Payload Payload { get; } = new ArrayTest1Payload();

        public override string Name => "ARRAY_TEST_1";
    }

    /// <summary>
    ///  ARRAY_TEST_1
    /// </summary>
    public class ArrayTest1Payload : IPayload
    {
        public byte GetMaxByteSize() => 16; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 16; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=ArU32.Length * 4; //ArU32
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/16 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            ArU32 = new uint[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                ArU32[i] = BinSerialize.ReadUInt(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<ArU32.Length;i++)
            {
                BinSerialize.WriteUInt(ref buffer,ArU32[i]);
            }
            /* PayloadByteSize = 16 */;
        }
        
        



        /// <summary>
        /// Value array
        /// OriginName: ar_u32, Units: , IsExtended: false
        /// </summary>
        public uint[] ArU32 { get; set; } = new uint[4];
        public byte GetArU32MaxItemsCount() => 4;
    }
    /// <summary>
    /// Array test #3.
    ///  ARRAY_TEST_3
    /// </summary>
    public class ArrayTest3Packet: PacketV2<ArrayTest3Payload>
    {
	    public const int PacketMessageId = 17153;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 19;
        public override bool WrapToV2Extension => false;

        public override ArrayTest3Payload Payload { get; } = new ArrayTest3Payload();

        public override string Name => "ARRAY_TEST_3";
    }

    /// <summary>
    ///  ARRAY_TEST_3
    /// </summary>
    public class ArrayTest3Payload : IPayload
    {
        public byte GetMaxByteSize() => 17; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 17; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=ArU32.Length * 4; //ArU32
            sum+=1; //V
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/17 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            ArU32 = new uint[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                ArU32[i] = BinSerialize.ReadUInt(ref buffer);
            }
            V = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<ArU32.Length;i++)
            {
                BinSerialize.WriteUInt(ref buffer,ArU32[i]);
            }
            BinSerialize.WriteByte(ref buffer,(byte)V);
            /* PayloadByteSize = 17 */;
        }
        
        



        /// <summary>
        /// Value array
        /// OriginName: ar_u32, Units: , IsExtended: false
        /// </summary>
        public uint[] ArU32 { get; set; } = new uint[4];
        public byte GetArU32MaxItemsCount() => 4;
        /// <summary>
        /// Stub field
        /// OriginName: v, Units: , IsExtended: false
        /// </summary>
        public byte V { get; set; }
    }
    /// <summary>
    /// Array test #4.
    ///  ARRAY_TEST_4
    /// </summary>
    public class ArrayTest4Packet: PacketV2<ArrayTest4Payload>
    {
	    public const int PacketMessageId = 17154;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 89;
        public override bool WrapToV2Extension => false;

        public override ArrayTest4Payload Payload { get; } = new ArrayTest4Payload();

        public override string Name => "ARRAY_TEST_4";
    }

    /// <summary>
    ///  ARRAY_TEST_4
    /// </summary>
    public class ArrayTest4Payload : IPayload
    {
        public byte GetMaxByteSize() => 17; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 17; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=ArU32.Length * 4; //ArU32
            sum+=1; //V
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/4 - Math.Max(0,((/*PayloadByteSize*/17 - payloadSize - /*ExtendedFieldsLength*/0)/4 /*FieldTypeByteSize*/));
            ArU32 = new uint[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                ArU32[i] = BinSerialize.ReadUInt(ref buffer);
            }
            V = (byte)BinSerialize.ReadByte(ref buffer);

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<ArU32.Length;i++)
            {
                BinSerialize.WriteUInt(ref buffer,ArU32[i]);
            }
            BinSerialize.WriteByte(ref buffer,(byte)V);
            /* PayloadByteSize = 17 */;
        }
        
        



        /// <summary>
        /// Value array
        /// OriginName: ar_u32, Units: , IsExtended: false
        /// </summary>
        public uint[] ArU32 { get; set; } = new uint[4];
        public byte GetArU32MaxItemsCount() => 4;
        /// <summary>
        /// Stub field
        /// OriginName: v, Units: , IsExtended: false
        /// </summary>
        public byte V { get; set; }
    }
    /// <summary>
    /// Array test #5.
    ///  ARRAY_TEST_5
    /// </summary>
    public class ArrayTest5Packet: PacketV2<ArrayTest5Payload>
    {
	    public const int PacketMessageId = 17155;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 27;
        public override bool WrapToV2Extension => false;

        public override ArrayTest5Payload Payload { get; } = new ArrayTest5Payload();

        public override string Name => "ARRAY_TEST_5";
    }

    /// <summary>
    ///  ARRAY_TEST_5
    /// </summary>
    public class ArrayTest5Payload : IPayload
    {
        public byte GetMaxByteSize() => 10; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 10; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=C1.Length; //C1
            sum+=C2.Length; //C2
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/5 - Math.Max(0,((/*PayloadByteSize*/10 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            C1 = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = C1)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, C1.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           
            arraySize = 5;
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = C2)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, C2.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           

        }

        public void Serialize(ref Span<byte> buffer)
        {
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = C1)
                {
                    Encoding.ASCII.GetBytes(charPointer, C1.Length, bytePointer, C1.Length);
                }
            }
            buffer = buffer.Slice(C1.Length);
            
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = C2)
                {
                    Encoding.ASCII.GetBytes(charPointer, C2.Length, bytePointer, C2.Length);
                }
            }
            buffer = buffer.Slice(C2.Length);
            
            /* PayloadByteSize = 10 */;
        }
        
        



        /// <summary>
        /// Value array
        /// OriginName: c1, Units: , IsExtended: false
        /// </summary>
        public char[] C1 { get; set; } = new char[5];
        public byte GetC1MaxItemsCount() => 5;
        /// <summary>
        /// Value array
        /// OriginName: c2, Units: , IsExtended: false
        /// </summary>
        public char[] C2 { get; } = new char[5];
    }
    /// <summary>
    /// Array test #6.
    ///  ARRAY_TEST_6
    /// </summary>
    public class ArrayTest6Packet: PacketV2<ArrayTest6Payload>
    {
	    public const int PacketMessageId = 17156;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 14;
        public override bool WrapToV2Extension => false;

        public override ArrayTest6Payload Payload { get; } = new ArrayTest6Payload();

        public override string Name => "ARRAY_TEST_6";
    }

    /// <summary>
    ///  ARRAY_TEST_6
    /// </summary>
    public class ArrayTest6Payload : IPayload
    {
        public byte GetMaxByteSize() => 91; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 91; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=ArD.Length * 8; //ArD
            sum+=4; //V3
            sum+=ArU32.Length * 4; //ArU32
            sum+=ArI32.Length * 4; //ArI32
            sum+=ArF.Length * 4; //ArF
            sum+=2; //V2
            sum+=ArU16.Length * 2; //ArU16
            sum+=ArI16.Length * 2; //ArI16
            sum+=1; //V1
            sum+=ArU8.Length; //ArU8
            sum+=ArI8.Length; //ArI8
            sum+=ArC.Length; //ArC
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArD[i] = BinSerialize.ReadDouble(ref buffer);
            }
            V3 = BinSerialize.ReadUInt(ref buffer);
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArU32[i] = BinSerialize.ReadUInt(ref buffer);
            }
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArI32[i] = BinSerialize.ReadInt(ref buffer);
            }
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArF[i] = BinSerialize.ReadFloat(ref buffer);
            }
            V2 = BinSerialize.ReadUShort(ref buffer);
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArU16[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArI16[i] = BinSerialize.ReadShort(ref buffer);
            }
            V1 = (byte)BinSerialize.ReadByte(ref buffer);
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArU8[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArI8[i] = (sbyte)BinSerialize.ReadByte(ref buffer);
            }
            arraySize = /*ArrayLength*/32 - Math.Max(0,((/*PayloadByteSize*/91 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            ArC = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = ArC)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, ArC.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<ArD.Length;i++)
            {
                BinSerialize.WriteDouble(ref buffer,ArD[i]);
            }
            BinSerialize.WriteUInt(ref buffer,V3);
            for(var i=0;i<ArU32.Length;i++)
            {
                BinSerialize.WriteUInt(ref buffer,ArU32[i]);
            }
            for(var i=0;i<ArI32.Length;i++)
            {
                BinSerialize.WriteInt(ref buffer,ArI32[i]);
            }
            for(var i=0;i<ArF.Length;i++)
            {
                BinSerialize.WriteFloat(ref buffer,ArF[i]);
            }
            BinSerialize.WriteUShort(ref buffer,V2);
            for(var i=0;i<ArU16.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,ArU16[i]);
            }
            for(var i=0;i<ArI16.Length;i++)
            {
                BinSerialize.WriteShort(ref buffer,ArI16[i]);
            }
            BinSerialize.WriteByte(ref buffer,(byte)V1);
            for(var i=0;i<ArU8.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)ArU8[i]);
            }
            for(var i=0;i<ArI8.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)ArI8[i]);
            }
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = ArC)
                {
                    Encoding.ASCII.GetBytes(charPointer, ArC.Length, bytePointer, ArC.Length);
                }
            }
            buffer = buffer.Slice(ArC.Length);
            
            /* PayloadByteSize = 91 */;
        }
        
        



        /// <summary>
        /// Value array
        /// OriginName: ar_d, Units: , IsExtended: false
        /// </summary>
        public double[] ArD { get; } = new double[2];
        /// <summary>
        /// Stub field
        /// OriginName: v3, Units: , IsExtended: false
        /// </summary>
        public uint V3 { get; set; }
        /// <summary>
        /// Value array
        /// OriginName: ar_u32, Units: , IsExtended: false
        /// </summary>
        public uint[] ArU32 { get; } = new uint[2];
        /// <summary>
        /// Value array
        /// OriginName: ar_i32, Units: , IsExtended: false
        /// </summary>
        public int[] ArI32 { get; } = new int[2];
        /// <summary>
        /// Value array
        /// OriginName: ar_f, Units: , IsExtended: false
        /// </summary>
        public float[] ArF { get; } = new float[2];
        /// <summary>
        /// Stub field
        /// OriginName: v2, Units: , IsExtended: false
        /// </summary>
        public ushort V2 { get; set; }
        /// <summary>
        /// Value array
        /// OriginName: ar_u16, Units: , IsExtended: false
        /// </summary>
        public ushort[] ArU16 { get; } = new ushort[2];
        /// <summary>
        /// Value array
        /// OriginName: ar_i16, Units: , IsExtended: false
        /// </summary>
        public short[] ArI16 { get; } = new short[2];
        /// <summary>
        /// Stub field
        /// OriginName: v1, Units: , IsExtended: false
        /// </summary>
        public byte V1 { get; set; }
        /// <summary>
        /// Value array
        /// OriginName: ar_u8, Units: , IsExtended: false
        /// </summary>
        public byte[] ArU8 { get; } = new byte[2];
        /// <summary>
        /// Value array
        /// OriginName: ar_i8, Units: , IsExtended: false
        /// </summary>
        public sbyte[] ArI8 { get; } = new sbyte[2];
        /// <summary>
        /// Value array
        /// OriginName: ar_c, Units: , IsExtended: false
        /// </summary>
        public char[] ArC { get; set; } = new char[32];
        public byte GetArCMaxItemsCount() => 32;
    }
    /// <summary>
    /// Array test #7.
    ///  ARRAY_TEST_7
    /// </summary>
    public class ArrayTest7Packet: PacketV2<ArrayTest7Payload>
    {
	    public const int PacketMessageId = 17157;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 187;
        public override bool WrapToV2Extension => false;

        public override ArrayTest7Payload Payload { get; } = new ArrayTest7Payload();

        public override string Name => "ARRAY_TEST_7";
    }

    /// <summary>
    ///  ARRAY_TEST_7
    /// </summary>
    public class ArrayTest7Payload : IPayload
    {
        public byte GetMaxByteSize() => 84; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 84; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=ArD.Length * 8; //ArD
            sum+=ArF.Length * 4; //ArF
            sum+=ArU32.Length * 4; //ArU32
            sum+=ArI32.Length * 4; //ArI32
            sum+=ArU16.Length * 2; //ArU16
            sum+=ArI16.Length * 2; //ArI16
            sum+=ArU8.Length; //ArU8
            sum+=ArI8.Length; //ArI8
            sum+=ArC.Length; //ArC
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArD[i] = BinSerialize.ReadDouble(ref buffer);
            }
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArF[i] = BinSerialize.ReadFloat(ref buffer);
            }
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArU32[i] = BinSerialize.ReadUInt(ref buffer);
            }
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArI32[i] = BinSerialize.ReadInt(ref buffer);
            }
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArU16[i] = BinSerialize.ReadUShort(ref buffer);
            }
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArI16[i] = BinSerialize.ReadShort(ref buffer);
            }
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArU8[i] = (byte)BinSerialize.ReadByte(ref buffer);
            }
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArI8[i] = (sbyte)BinSerialize.ReadByte(ref buffer);
            }
            arraySize = /*ArrayLength*/32 - Math.Max(0,((/*PayloadByteSize*/84 - payloadSize - /*ExtendedFieldsLength*/0)/1 /*FieldTypeByteSize*/));
            ArC = new char[arraySize];
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = ArC)
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, ArC.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
           

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<ArD.Length;i++)
            {
                BinSerialize.WriteDouble(ref buffer,ArD[i]);
            }
            for(var i=0;i<ArF.Length;i++)
            {
                BinSerialize.WriteFloat(ref buffer,ArF[i]);
            }
            for(var i=0;i<ArU32.Length;i++)
            {
                BinSerialize.WriteUInt(ref buffer,ArU32[i]);
            }
            for(var i=0;i<ArI32.Length;i++)
            {
                BinSerialize.WriteInt(ref buffer,ArI32[i]);
            }
            for(var i=0;i<ArU16.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,ArU16[i]);
            }
            for(var i=0;i<ArI16.Length;i++)
            {
                BinSerialize.WriteShort(ref buffer,ArI16[i]);
            }
            for(var i=0;i<ArU8.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)ArU8[i]);
            }
            for(var i=0;i<ArI8.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte)ArI8[i]);
            }
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = ArC)
                {
                    Encoding.ASCII.GetBytes(charPointer, ArC.Length, bytePointer, ArC.Length);
                }
            }
            buffer = buffer.Slice(ArC.Length);
            
            /* PayloadByteSize = 84 */;
        }
        
        



        /// <summary>
        /// Value array
        /// OriginName: ar_d, Units: , IsExtended: false
        /// </summary>
        public double[] ArD { get; } = new double[2];
        /// <summary>
        /// Value array
        /// OriginName: ar_f, Units: , IsExtended: false
        /// </summary>
        public float[] ArF { get; } = new float[2];
        /// <summary>
        /// Value array
        /// OriginName: ar_u32, Units: , IsExtended: false
        /// </summary>
        public uint[] ArU32 { get; } = new uint[2];
        /// <summary>
        /// Value array
        /// OriginName: ar_i32, Units: , IsExtended: false
        /// </summary>
        public int[] ArI32 { get; } = new int[2];
        /// <summary>
        /// Value array
        /// OriginName: ar_u16, Units: , IsExtended: false
        /// </summary>
        public ushort[] ArU16 { get; } = new ushort[2];
        /// <summary>
        /// Value array
        /// OriginName: ar_i16, Units: , IsExtended: false
        /// </summary>
        public short[] ArI16 { get; } = new short[2];
        /// <summary>
        /// Value array
        /// OriginName: ar_u8, Units: , IsExtended: false
        /// </summary>
        public byte[] ArU8 { get; } = new byte[2];
        /// <summary>
        /// Value array
        /// OriginName: ar_i8, Units: , IsExtended: false
        /// </summary>
        public sbyte[] ArI8 { get; } = new sbyte[2];
        /// <summary>
        /// Value array
        /// OriginName: ar_c, Units: , IsExtended: false
        /// </summary>
        public char[] ArC { get; set; } = new char[32];
        public byte GetArCMaxItemsCount() => 32;
    }
    /// <summary>
    /// Array test #8.
    ///  ARRAY_TEST_8
    /// </summary>
    public class ArrayTest8Packet: PacketV2<ArrayTest8Payload>
    {
	    public const int PacketMessageId = 17158;
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => 106;
        public override bool WrapToV2Extension => false;

        public override ArrayTest8Payload Payload { get; } = new ArrayTest8Payload();

        public override string Name => "ARRAY_TEST_8";
    }

    /// <summary>
    ///  ARRAY_TEST_8
    /// </summary>
    public class ArrayTest8Payload : IPayload
    {
        public byte GetMaxByteSize() => 24; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 24; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=ArD.Length * 8; //ArD
            sum+=4; //V3
            sum+=ArU16.Length * 2; //ArU16
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var arraySize = 0;
            var payloadSize = buffer.Length;
            arraySize = /*ArrayLength*/2 - Math.Max(0,((/*PayloadByteSize*/24 - payloadSize - /*ExtendedFieldsLength*/0)/8 /*FieldTypeByteSize*/));
            ArD = new double[arraySize];
            for(var i=0;i<arraySize;i++)
            {
                ArD[i] = BinSerialize.ReadDouble(ref buffer);
            }
            V3 = BinSerialize.ReadUInt(ref buffer);
            arraySize = 2;
            for(var i=0;i<arraySize;i++)
            {
                ArU16[i] = BinSerialize.ReadUShort(ref buffer);
            }

        }

        public void Serialize(ref Span<byte> buffer)
        {
            for(var i=0;i<ArD.Length;i++)
            {
                BinSerialize.WriteDouble(ref buffer,ArD[i]);
            }
            BinSerialize.WriteUInt(ref buffer,V3);
            for(var i=0;i<ArU16.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,ArU16[i]);
            }
            /* PayloadByteSize = 24 */;
        }
        
        



        /// <summary>
        /// Value array
        /// OriginName: ar_d, Units: , IsExtended: false
        /// </summary>
        public double[] ArD { get; set; } = new double[2];
        public byte GetArDMaxItemsCount() => 2;
        /// <summary>
        /// Stub field
        /// OriginName: v3, Units: , IsExtended: false
        /// </summary>
        public uint V3 { get; set; }
        /// <summary>
        /// Value array
        /// OriginName: ar_u16, Units: , IsExtended: false
        /// </summary>
        public ushort[] ArU16 { get; } = new ushort[2];
    }


#endregion


}
