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

// This code was generate by tool {{Tool}} version {{ToolVersion}}

using System;
using System.Text;
using Asv.Mavlink.V2.Common;
using Asv.IO;

namespace Asv.Mavlink.V2.{{ Namespace }}
{

    public static class {{ Namespace }}Helper
    {
        public static void Register{{ Namespace }}Dialect(this IPacketDecoder<IPacketV2<IPayload>> src)
        {
            {%- for msg in Messages -%}
            src.Register(()=>new {{ msg.CamelCaseName }}Packet());
            {%- endfor -%}
        }
    }

#region Enums

{%- for en in Enums -%}
    /// <summary>
    {%- for line in en.Desc -%}
    /// {{ line }}
    {%- endfor -%}
    ///  {{ en.Name }}
    /// </summary>
    public enum {{ en.CamelCaseName }}:uint
    {
    {%- for entry in en.Entries -%}
        /// <summary>
        {%- for line in entry.Desc -%}
        /// {{ line }}
        {%- endfor -%}
        {%- for param in entry.Params -%}
        /// Param {{ param.Index }} - {{ param.Desc }}
        {%- endfor -%}
        /// {{ entry.Name }}
        /// </summary>
        {{ entry.CamelCaseName }} = {{ entry.Value }},
    {%- endfor -%}
    }

{%- endfor -%}

#endregion

#region Messages

{%- for msg in Messages -%}
    /// <summary>
    {%- for line in msg.Desc -%}
    /// {{ line }}
    {%- endfor -%}
    ///  {{ msg.Name }}
    /// </summary>
    public class {{ msg.CamelCaseName }}Packet: PacketV2<{{ msg.CamelCaseName }}Payload>
    {
	    public const int PacketMessageId = {{ msg.Id }};
        public override int MessageId => PacketMessageId;
        public override byte GetCrcEtra() => {{ msg.CrcExtra }};

        public override {{ msg.CamelCaseName }}Payload Payload { get; } = new {{ msg.CamelCaseName }}Payload();

        public override string Name => "{{ msg.Name }}";
    }

    /// <summary>
    ///  {{ msg.Name }}
    /// </summary>
    public class {{ msg.CamelCaseName }}Payload : IPayload
    {
        public byte GetMaxByteSize() => {{ msg.PayloadByteSize }}; // Summ of byte sized of all fields (include extended)
        public byte GetMinByteSize() => {{ msg.PayloadByteSize - msg.ExtendedFieldsLength }}; // of byte sized of fields (exclude extended)

        public void Deserialize(ref ReadOnlySpan<byte> buffer, int payloadSize)
        {
            var index = 0;
            var endIndex = payloadSize;
            var arraySize = 0;
{%- for field in msg.Fields -%}
            {%- if field.IsExtended -%}
            // extended field '{{ field.CamelCaseName }}' can be empty
            if (index >= endIndex) return;
            {%- endif -%}
    {%- if field.IsEnum -%}
        {%- if field.IsArray -%}
            {%- if field.IsTheLargestArrayInMessage -%}
            arraySize = /*ArrayLength*/{{ field.ArrayLength }} - Math.Max(0,((/*PayloadByteSize*/{{ msg.PayloadByteSize }} - payloadSize - /*ExtendedFieldsLength*/{{ msg.ExtendedFieldsLength }})//*FieldTypeByteSize*/{{ field.FieldTypeByteSize }}));
            {{ field.CamelCaseName }} = new {{ field.Type }}[arraySize];
            {%- else -%}
            arraySize = {{ field.ArrayLength }};
            {%- endif -%}
            for(var i=0;i<arraySize;i++)
            {
                {%- case field.Type -%}
                {%- when 'sbyte' or 'byte'-%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BinSerialize.ReadByte(ref buffer);index+=1;
                {%- when 'short' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BinSerialize.ReadShort(ref buffer);index+=2;
                {%- when 'ushort' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BinSerialize.ReadUShort(ref buffer);index+=2;
                {%- when 'int' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BinSerialize.ReadInt(ref buffer);index+=4;
                {%- when 'uint' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BinSerialize.ReadUInt(ref buffer);index+=4;
                {%- when 'long' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BinSerialize.ReadLong(ref buffer);index+=8;
                {%- when 'ulong' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BinSerialize.ReadULong(ref buffer);index+=8;
                {%- endcase -%}
            }

        {%- else -%}
            {%- case field.Type -%}
            {%- when 'sbyte' or 'byte' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BinSerialize.ReadByte(ref buffer);index+=1;
            {%- when 'short' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BinSerialize.ReadShort(ref buffer);index+=2;
            {%- when 'ushort' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BinSerialize.ReadUShort(ref buffer);index+=2;
            {%- when 'int' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BinSerialize.ReadInt(ref buffer);index+=4;
            {%- when 'uint' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BinSerialize.ReadUInt(ref buffer);index+=4;
            {%- when 'long' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BinSerialize.ReadLong(ref buffer);index+=8;
            {%- when 'ulong' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BinSerialize.ReadULong(ref buffer);index+=8;
            {%- endcase -%}
        {%- endif -%}
    {%- else -%}
        {%- if field.IsArray -%}
            {%- if field.IsTheLargestArrayInMessage -%}
            arraySize = /*ArrayLength*/{{ field.ArrayLength }} - Math.Max(0,((/*PayloadByteSize*/{{ msg.PayloadByteSize }} - payloadSize - /*ExtendedFieldsLength*/{{ msg.ExtendedFieldsLength }})/{{ field.FieldTypeByteSize }} /*FieldTypeByteSize*/));
            {{ field.CamelCaseName }} = new {{ field.Type }}[arraySize];
            {%- else -%}
            arraySize = {{ field.ArrayLength }};
            {%- endif -%}
            {%- if field.Type == 'char' -%}
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = {{ field.CamelCaseName }})
                {
                    Encoding.ASCII.GetChars(bytePointer, arraySize, charPointer, {{ field.CamelCaseName }}.Length);
                }
            }
            buffer = buffer.Slice(arraySize);
            index+=arraySize;
            {%- else -%}
            for(var i=0;i<arraySize;i++)
            {
                {%- case field.Type -%}
                {%- when 'sbyte' or 'byte' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.Type }})BinSerialize.ReadByte(ref buffer);index+=1;
                {%- when 'short' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadShort(ref buffer);index+=2;
                {%- when 'ushort' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadUShort(ref buffer);index+=2;
                {%- when 'int' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadInt(ref buffer);index+=4;
                {%- when 'uint' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadUInt(ref buffer);index+=4;
                {%- when 'long' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadLong(ref buffer);index+=8;
                {%- when 'ulong' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadULong(ref buffer);index+=8;
                {%- when 'float' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadFloat(ref buffer);index+=4;
                {%- when 'double' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadDouble(ref buffer);index+=8;
                {%- endcase -%}
            }
            {%- endif -%}
        {%- else -%}
            {%- if field.Type == 'char' -%}
            {{ field.CamelCaseName }} = (char)buffer[0];
            buffer = buffer.Slice(1);
            index+=1;
            {%- else -%}
            {%- case field.Type -%}
            {%- when 'sbyte' or 'byte' -%}
            {{ field.CamelCaseName }} = ({{ field.Type }})BinSerialize.ReadByte(ref buffer);index+=1;
            {%- when 'short' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadShort(ref buffer);index+=2;
            {%- when 'ushort' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadUShort(ref buffer);index+=2;
            {%- when 'int' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadInt(ref buffer);index+=4;
            {%- when 'uint' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadUInt(ref buffer);index+=4;
            {%- when 'long' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadLong(ref buffer);index+=8;
            {%- when 'ulong' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadULong(ref buffer);index+=8;
            {%- when 'float' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadFloat(ref buffer);index+=4;
            {%- when 'double' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadDouble(ref buffer);index+=8;
            {%- endcase -%}
            {%- endif -%}
        {%- endif -%}
    {%- endif -%}
{%- endfor -%}

        }

        public int Serialize(ref Span<byte> buffer)
        {
            var index = 0;
{%- for field in msg.Fields -%}
    {%- if field.IsEnum -%}
        {%- if field.IsArray -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                {%- case field.Type -%}
                {%- when 'char' or 'double' or 'float'-%}
                ERROR => ENUM as 'char' or 'double' or 'float' ???????
                {%- when 'sbyte' or 'byte' -%}
                BinSerialize.WriteByte(ref buffer,(byte){{ field.CamelCaseName }}[i]);index+=1;
                {%- when 'short' -%}
                BinSerialize.WriteShort(ref buffer,(short){{ field.CamelCaseName }}[i]);index+=2;
                {%- when 'ushort' -%}
                BinSerialize.WriteUShort(ref buffer,(ushort){{ field.CamelCaseName }}[i]);index+=2;
                {%- when 'int' -%}
                BinSerialize.WriteInt(ref buffer,(int){{ field.CamelCaseName }}[i]);index+=4;
                {%- when 'uint' -%}
                BinSerialize.WriteUInt(ref buffer,(uint){{ field.CamelCaseName }}[i]);index+=4;
                {%- when 'long' -%}
                BinSerialize.WriteLong(ref buffer,(long){{ field.CamelCaseName }}[i]);index+=8;
                {%- when 'ulong' -%}
                BinSerialize.WriteULong(ref buffer,(ulong){{ field.CamelCaseName }}[i]);index+=8;
                
                {%- endcase -%}
            }
        {%- else -%}
            {%- case field.Type -%}
            {%- when 'char' or 'double' or 'float'-%}
            ERROR => ENUM as 'char' or 'double' or 'float' ???????
            {%- when 'sbyte' or 'byte' -%}
            BinSerialize.WriteByte(ref buffer,(byte){{ field.CamelCaseName }});index+=1;
            {%- when 'short' -%}
            BinSerialize.WriteShort(ref buffer,(short){{ field.CamelCaseName }});index+=2;
            {%- when 'ushort' -%}
            BinSerialize.WriteUShort(ref buffer,(ushort){{ field.CamelCaseName }});index+=2;
            {%- when 'int' -%}
            BinSerialize.WriteInt(ref buffer,(int){{ field.CamelCaseName }});index+=4;
            {%- when 'uint' -%}
            BinSerialize.WriteUInt(ref buffer,(uint){{ field.CamelCaseName }});index+=4;
            {%- when 'long' -%}
            BinSerialize.WriteLong(ref buffer,(long){{ field.CamelCaseName }});index+=8;
            {%- when 'ulong' -%}
            BinSerialize.WriteULong(ref buffer,(ulong){{ field.CamelCaseName }});index+=8;
            {%- endcase -%}
        {%- endif -%}
    {%- else -%}
        {%- if field.IsArray -%}
            {%- case field.Type -%}
            {%- when 'char' -%}
            unsafe
            {
                fixed (byte* bytePointer = buffer)
                fixed (char* charPointer = {{ field.CamelCaseName }})
                {
                    Encoding.ASCII.GetBytes(charPointer, {{ field.CamelCaseName }}.Length, bytePointer, {{ field.CamelCaseName }}.Length);
                }
            }
            buffer = buffer.Slice({{ field.CamelCaseName }}.Length);
            index+={{ field.CamelCaseName }}.Length;
            {%- when 'sbyte' or 'byte' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte){{ field.CamelCaseName }}[i]);index+=1;
            }
            {%- when 'short' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteShort(ref buffer,{{ field.CamelCaseName }}[i]);index+=2;
            }
            {%- when 'ushort' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,{{ field.CamelCaseName }}[i]);index+=2;
            }
            {%- when 'int' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteInt(ref buffer,{{ field.CamelCaseName }}[i]);index+=4;
            }
            {%- when 'uint' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteUInt(ref buffer,{{ field.CamelCaseName }}[i]);index+=4;
            }
            {%- when 'long' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteLong(ref buffer,{{ field.CamelCaseName }}[i]);index+=8;
            }
            {%- when 'ulong' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteULong(ref buffer,{{ field.CamelCaseName }}[i]);index+=8;
            }
            {%- when 'float' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteFloat(ref buffer,{{ field.CamelCaseName }}[i]);index+=4;
            }
            {%- when 'double' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteDouble(ref buffer,{{ field.CamelCaseName }}[i]);index+=8;
            }
            {%- endcase -%}
        {%- else -%}
            {%- case field.Type -%}
            {%- when 'char' -%}
            BinSerialize.WriteByte(ref buffer,(byte){{ field.CamelCaseName }});index+=1;
            {%- when 'sbyte' or 'byte' -%}
            BinSerialize.WriteByte(ref buffer,(byte){{ field.CamelCaseName }});index+=1;
            {%- when 'short' -%}
            BinSerialize.WriteShort(ref buffer,{{ field.CamelCaseName }});index+=2;
            {%- when 'ushort' -%}
            BinSerialize.WriteUShort(ref buffer,{{ field.CamelCaseName }});index+=2;
            {%- when 'int' -%}
            BinSerialize.WriteInt(ref buffer,{{ field.CamelCaseName }});index+=4;
            {%- when 'uint' -%}
            BinSerialize.WriteUInt(ref buffer,{{ field.CamelCaseName }});index+=4;
            {%- when 'long' -%}
            BinSerialize.WriteLong(ref buffer,{{ field.CamelCaseName }});index+=8;
            {%- when 'ulong' -%}
            BinSerialize.WriteULong(ref buffer,{{ field.CamelCaseName }});index+=8;
            {%- when 'float' -%}
            BinSerialize.WriteFloat(ref buffer,{{ field.CamelCaseName }});index+=4;
            {%- when 'double' -%}
            BinSerialize.WriteDouble(ref buffer,{{ field.CamelCaseName }});index+=8;
            {%- endcase -%}
        {%- endif -%}
    {%- endif -%}
{%- endfor -%}
            return index; // /*PayloadByteSize*/{{ msg.PayloadByteSize }};
        }



        public void Deserialize(byte[] buffer, int offset, int payloadSize)
        {
            var index = offset;
            var endIndex = offset + payloadSize;
            var arraySize = 0;
{%- for field in msg.Fields -%}
            {%- if field.IsExtended -%}
            // extended field '{{ field.CamelCaseName }}' can be empty
            if (index >= endIndex) return;
            {%- endif -%}
    {%- if field.IsEnum -%}
        {%- if field.IsArray -%}
            {%- if field.IsTheLargestArrayInMessage -%}
            arraySize = /*ArrayLength*/{{ field.ArrayLength }} - Math.Max(0,((/*PayloadByteSize*/{{ msg.PayloadByteSize }} - payloadSize - /*ExtendedFieldsLength*/{{ msg.ExtendedFieldsLength }})//*FieldTypeByteSize*/{{ field.FieldTypeByteSize }}));
            {{ field.CamelCaseName }} = new {{ field.Type }}[arraySize];
            {%- else -%}
            arraySize = {{ field.ArrayLength }};
            {%- endif -%}
            for(var i=0;i<arraySize;i++)
            {
                {%- case field.Type -%}
                {%- when 'sbyte' or 'byte'-%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})buffer[index++];
                {%- when 'short' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BitConverter.ToInt16(buffer,index);index+=2;
                {%- when 'ushort' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BitConverter.ToUInt16(buffer,index);index+=2;
                {%- when 'int' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BitConverter.ToInt32(buffer,index);index+=4;
                {%- when 'uint' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BitConverter.ToUInt32(buffer,index);index+=4;
                {%- when 'long' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BitConverter.ToInt64(buffer,index);index+=8;
                {%- when 'ulong' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BitConverter.ToUInt64(buffer,index);index+=8;
                {%- endcase -%}
            }

        {%- else -%}
            {%- case field.Type -%}
            {%- when 'sbyte' or 'byte' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})buffer[index++];
            {%- when 'short' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BitConverter.ToInt16(buffer,index);index+=2;
            {%- when 'ushort' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BitConverter.ToUInt16(buffer,index);index+=2;
            {%- when 'int' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BitConverter.ToInt32(buffer,index);index+=4;
            {%- when 'uint' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BitConverter.ToUInt32(buffer,index);index+=4;
            {%- when 'long' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BitConverter.ToInt64(buffer,index);index+=8;
            {%- when 'ulong' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BitConverter.ToUInt64(buffer,index);index+=8;
            {%- endcase -%}
        {%- endif -%}
    {%- else -%}
        {%- if field.IsArray -%}
            {%- if field.IsTheLargestArrayInMessage -%}
            arraySize = /*ArrayLength*/{{ field.ArrayLength }} - Math.Max(0,((/*PayloadByteSize*/{{ msg.PayloadByteSize }} - payloadSize - /*ExtendedFieldsLength*/{{ msg.ExtendedFieldsLength }})/{{ field.FieldTypeByteSize }} /*FieldTypeByteSize*/));
            {{ field.CamelCaseName }} = new {{ field.Type }}[arraySize];
            {%- else -%}
            arraySize = {{ field.ArrayLength }};
            {%- endif -%}
            {%- if field.Type == 'char' -%}
            Encoding.ASCII.GetChars(buffer, index,arraySize,{{ field.CamelCaseName }},0);
            index+=arraySize;
            {%- else -%}
            for(var i=0;i<arraySize;i++)
            {
                {%- case field.Type -%}
                {%- when 'sbyte' or 'byte' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.Type }})buffer[index++];
                {%- when 'short' -%}
                {{ field.CamelCaseName }}[i] = BitConverter.ToInt16(buffer,index);index+=2;
                {%- when 'ushort' -%}
                {{ field.CamelCaseName }}[i] = BitConverter.ToUInt16(buffer,index);index+=2;
                {%- when 'int' -%}
                {{ field.CamelCaseName }}[i] = BitConverter.ToInt32(buffer,index);index+=4;
                {%- when 'uint' -%}
                {{ field.CamelCaseName }}[i] = BitConverter.ToUInt32(buffer,index);index+=4;
                {%- when 'long' -%}
                {{ field.CamelCaseName }}[i] = BitConverter.ToInt64(buffer,index);index+=8;
                {%- when 'ulong' -%}
                {{ field.CamelCaseName }}[i] = BitConverter.ToUInt64(buffer,index);index+=8;
                {%- when 'float' -%}
                {{ field.CamelCaseName }}[i] = BitConverter.ToSingle(buffer, index);index+=4;
                {%- when 'double' -%}
                {{ field.CamelCaseName }}[i] = BitConverter.ToDouble(buffer, index);index+=8;
                {%- endcase -%}
            }
            {%- endif -%}
        {%- else -%}
            {%- if field.Type == 'char' -%}
            {{ field.CamelCaseName }} = Encoding.ASCII.GetChars(buffer,index,1)[0];
            index+=1;
            {%- else -%}
            {%- case field.Type -%}
            {%- when 'sbyte' or 'byte' -%}
            {{ field.CamelCaseName }} = ({{ field.Type }})buffer[index++];
            {%- when 'short' -%}
            {{ field.CamelCaseName }} = BitConverter.ToInt16(buffer,index);index+=2;
            {%- when 'ushort' -%}
            {{ field.CamelCaseName }} = BitConverter.ToUInt16(buffer,index);index+=2;
            {%- when 'int' -%}
            {{ field.CamelCaseName }} = BitConverter.ToInt32(buffer,index);index+=4;
            {%- when 'uint' -%}
            {{ field.CamelCaseName }} = BitConverter.ToUInt32(buffer,index);index+=4;
            {%- when 'long' -%}
            {{ field.CamelCaseName }} = BitConverter.ToInt64(buffer,index);index+=8;
            {%- when 'ulong' -%}
            {{ field.CamelCaseName }} = BitConverter.ToUInt64(buffer,index);index+=8;
            {%- when 'float' -%}
            {{ field.CamelCaseName }} = BitConverter.ToSingle(buffer, index);index+=4;
            {%- when 'double' -%}
            {{ field.CamelCaseName }} = BitConverter.ToDouble(buffer, index);index+=8;
            {%- endcase -%}
            {%- endif -%}
        {%- endif -%}
    {%- endif -%}
{%- endfor -%}
        }

        public int Serialize(byte[] buffer, int index)
        {
		var start = index;
{%- for field in msg.Fields -%}
    {%- if field.IsEnum -%}
        {%- if field.IsArray -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                {%- case field.Type -%}
                {%- when 'char' or 'double' or 'float'-%}
                ERROR => ENUM as 'char' or 'double' or 'float' ???????
                {%- when 'sbyte' or 'byte' -%}
                buffer[index] = (byte){{ field.CamelCaseName }}[i];index+={{ field.FieldTypeByteSize }};
                {%- when 'ulong' or 'long' or 'uint' or 'int' or 'ushort' or 'short' -%}
                BitConverter.GetBytes(({{ field.Type }}){{ field.CamelCaseName }}[i]).CopyTo(buffer, index);index+={{ field.FieldTypeByteSize }};
                {%- endcase -%}
            }
        {%- else -%}
            {%- case field.Type -%}
            {%- when 'char' or 'double' or 'float'-%}
            ERROR => ENUM as 'char' or 'double' or 'float' ???????
            {%- when 'sbyte' or 'byte' -%}
            buffer[index] = (byte){{ field.CamelCaseName }};index+={{ field.FieldTypeByteSize }};
            {%- when 'ulong' or 'long' or 'uint' or 'int' or 'ushort' or 'short'  -%}
            BitConverter.GetBytes(({{ field.Type }}){{ field.CamelCaseName }}).CopyTo(buffer, index);index+={{ field.FieldTypeByteSize }};
            {%- endcase -%}
        {%- endif -%}
    {%- else -%}
        {%- if field.IsArray -%}
            {%- case field.Type -%}
            {%- when 'char' -%}
            index+=Encoding.ASCII.GetBytes({{ field.CamelCaseName }},0,{{ field.CamelCaseName }}.Length,buffer,index);
            {%- when 'sbyte' or 'byte' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                buffer[index] = (byte){{ field.CamelCaseName }}[i];index+={{ field.FieldTypeByteSize }};
            }
            {%- when 'double' or 'float' or 'ulong' or 'long' or 'uint' or 'int' or 'ushort' or 'short'  -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BitConverter.GetBytes({{ field.CamelCaseName }}[i]).CopyTo(buffer, index);index+={{ field.FieldTypeByteSize }};
            }
            {%- endcase -%}
        {%- else -%}
            BitConverter.GetBytes({{ field.CamelCaseName }}).CopyTo(buffer, index);index+={{ field.FieldTypeByteSize }};
        {%- endif -%}
    {%- endif -%}
{%- endfor -%}
            return index - start; // /*PayloadByteSize*/{{ msg.PayloadByteSize }};
        }

    {%- for field in msg.Fields -%}
        /// <summary>
        {%- for line in field.Desc -%}
        /// {{ line }}
        {%- endfor -%}
        /// OriginName: {{ field.Name }}, Units: {{ field.Units }}, IsExtended: {{ field.IsExtended }}
        /// </summary>
    {%- if field.IsEnum -%}
        {%- if field.IsArray -%}
            {%- if field.IsTheLargestArrayInMessage -%}
        public {{ field.EnumCamelCaseName }}[] {{ field.CamelCaseName }} { get; set; } = new {{ field.Type }}[{{ field.ArrayLength }}];
            {%- else -%}
        public {{ field.EnumCamelCaseName }}[] {{ field.CamelCaseName }} { get; } = new {{ field.Type }}[{{ field.ArrayLength }}];
            {%- endif -%}
        {%- else -%}
        public {{ field.EnumCamelCaseName }} {{ field.CamelCaseName }} { get; set; }
       {%- endif -%}
    {%- else -%}
        {%- if field.IsArray -%}
            {%- if field.IsTheLargestArrayInMessage -%}
        public {{ field.Type }}[] {{ field.CamelCaseName }} { get; set; } = new {{ field.Type }}[{{ field.ArrayLength }}];
        public byte Get{{ field.CamelCaseName }}MaxItemsCount() => {{ field.ArrayLength }};
            {%- else -%}
        public {{ field.Type }}[] {{ field.CamelCaseName }} { get; } = new {{ field.Type }}[{{ field.ArrayLength }}];
            {%- endif -%}
        {%- else -%}
        public {{ field.Type }} {{ field.CamelCaseName }} { get; set; }
        {%- endif -%}
    {%- endif -%}
    {%- endfor -%}
    }
{%- endfor -%}


#endregion


}
