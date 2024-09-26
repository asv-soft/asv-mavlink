// MIT License
//
// Copyright (c) 2024 asv-soft (https://github.com/asv-soft)
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
using System.ComponentModel;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
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
    {%- if en.IsFlag -%}
    [Flags]
    {%- endif -%}
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
        {%- if entry.HasMetadataDescription -%}
        [Description("{{ entry.MetadataDescription }}")]
        {%- endif -%}
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
        public override bool WrapToV2Extension => {{ msg.WrapToV2Extension }};

        public override {{ msg.CamelCaseName }}Payload Payload { get; } = new {{ msg.CamelCaseName }}Payload();

        public override string Name => "{{ msg.Name }}";
    }

    /// <summary>
    ///  {{ msg.Name }}
    /// </summary>
    public class {{ msg.CamelCaseName }}Payload : IPayload
    {
        public byte GetMaxByteSize() => {{ msg.PayloadByteSize }}; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => {{ msg.PayloadByteSize - msg.ExtendedFieldsLength }}; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
{%- for field in msg.Fields -%}
    {%- if field.IsEnum -%}
        {%- if field.IsArray -%}
            {%- case field.Type -%}
            {%- when 'char' or 'double' or 'float'-%}
            ERROR => ENUM as 'char' or 'double' or 'float' ???????
            {%- when 'sbyte' or 'byte' -%}
            sum+= {{ field.CamelCaseName }}.Length; // {{ field.CamelCaseName }}
            {%- when 'short' -%}
            sum+= {{ field.CamelCaseName }}.Length * 2; // {{ field.CamelCaseName }}
            {%- when 'ushort' -%}
            sum+= {{ field.CamelCaseName }}.Length * 2; // {{ field.CamelCaseName }}
            {%- when 'int' -%}
            sum+= {{ field.CamelCaseName }}.Length * 4; // {{ field.CamelCaseName }}
            {%- when 'uint' -%}
            sum+= {{ field.CamelCaseName }}.Length * 4; // {{ field.CamelCaseName }}
            {%- when 'long' -%}
            sum+= {{ field.CamelCaseName }}.Length * 8; // {{ field.CamelCaseName }}
            {%- when 'ulong' -%}
            sum+= {{ field.CamelCaseName }}.Length * 8; // {{ field.CamelCaseName }}
            {%- endcase -%}
            
        {%- else -%}
            {%- case field.Type -%}
            {%- when 'char' or 'double' or 'float'-%}
            ERROR => ENUM as 'char' or 'double' or 'float' ???????
            {%- when 'sbyte' or 'byte' -%}
            sum+= 1; // {{ field.CamelCaseName }}
            {%- when 'short' -%}
            sum+= 2; // {{ field.CamelCaseName }}
            {%- when 'ushort' -%}
            sum+= 2; // {{ field.CamelCaseName }}
            {%- when 'int' -%}
            sum+= 4; // {{ field.CamelCaseName }}
            {%- when 'uint' -%}
            sum+= 4; // {{ field.CamelCaseName }}
            {%- when 'long' -%}
            sum+= 8; // {{ field.CamelCaseName }}
            {%- when 'ulong' -%}
            sum+= 8; // {{ field.CamelCaseName }}
            {%- endcase -%}
        {%- endif -%}
    {%- else -%}
        {%- if field.IsArray -%}
            {%- case field.Type -%}
            {%- when 'char' -%}
            sum+={{ field.CamelCaseName }}.Length; //{{ field.CamelCaseName }}
            {%- when 'sbyte' or 'byte' -%}
            sum+={{ field.CamelCaseName }}.Length; //{{ field.CamelCaseName }}
            {%- when 'short' -%}
            sum+={{ field.CamelCaseName }}.Length * 2; //{{ field.CamelCaseName }}
            {%- when 'ushort' -%}
            sum+={{ field.CamelCaseName }}.Length * 2; //{{ field.CamelCaseName }}
            {%- when 'int' -%}
            sum+={{ field.CamelCaseName }}.Length * 4; //{{ field.CamelCaseName }}
            {%- when 'uint' -%}
            sum+={{ field.CamelCaseName }}.Length * 4; //{{ field.CamelCaseName }}
            {%- when 'long' -%}
            sum+={{ field.CamelCaseName }}.Length * 8; //{{ field.CamelCaseName }}
            {%- when 'ulong' -%}
            sum+={{ field.CamelCaseName }}.Length * 8; //{{ field.CamelCaseName }}
            {%- when 'float' -%}
            sum+={{ field.CamelCaseName }}.Length * 4; //{{ field.CamelCaseName }}
            {%- when 'double' -%}
            sum+={{ field.CamelCaseName }}.Length * 8; //{{ field.CamelCaseName }}
            {%- endcase -%}
        {%- else -%}
            {%- case field.Type -%}
            {%- when 'char' -%}
            sum+=1; //{{ field.CamelCaseName }}
            {%- when 'sbyte' or 'byte' -%}
            sum+=1; //{{ field.CamelCaseName }}
            {%- when 'short' -%}
            sum+=2; //{{ field.CamelCaseName }}
            {%- when 'ushort' -%}
            sum+=2; //{{ field.CamelCaseName }}
            {%- when 'int' -%}
            sum+=4; //{{ field.CamelCaseName }}
            {%- when 'uint' -%}
            sum+=4; //{{ field.CamelCaseName }}
            {%- when 'long' -%}
            sum+=8; //{{ field.CamelCaseName }}
            {%- when 'ulong' -%}
            sum+=8; //{{ field.CamelCaseName }}
            {%- when 'float' -%}
            sum+=4; //{{ field.CamelCaseName }}
            {%- when 'double' -%}
            sum+=8; //{{ field.CamelCaseName }}
            {%- endcase -%}
        {%- endif -%}
    {%- endif -%}
{%- endfor -%}
            return (byte)sum;
        }



        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
{%- if msg.HasArrayFields -%}
            var arraySize = 0;
            var payloadSize = buffer.Length;
{%- endif -%}
{%- for field in msg.Fields -%}
            {%- if field.IsExtended -%}
            // extended field '{{ field.CamelCaseName }}' can be empty
            if (buffer.IsEmpty) return;
            {%- endif -%}
    {%- if field.IsEnum -%}
        {%- if field.IsArray -%}
            {%- if field.IsTheLargestArrayInMessage -%}
            arraySize = /*ArrayLength*/{{ field.ArrayLength }} - Math.Max(0,((/*PayloadByteSize*/{{ msg.PayloadByteSize }} - payloadSize - /*ExtendedFieldsLength*/{{ msg.ExtendedFieldsLength }})/*FieldTypeByteSize*/ /{{ field.FieldTypeByteSize }}));
            {{ field.CamelCaseName }} = new {{ field.EnumCamelCaseName }}[arraySize];
            {%- else -%}
            arraySize = {{ field.ArrayLength }};
            {%- endif -%}
            for(var i=0;i<arraySize;i++)
            {
                {%- case field.Type -%}
                {%- when 'sbyte' or 'byte'-%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BinSerialize.ReadByte(ref buffer);
                {%- when 'short' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BinSerialize.ReadShort(ref buffer);
                {%- when 'ushort' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BinSerialize.ReadUShort(ref buffer);
                {%- when 'int' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BinSerialize.ReadInt(ref buffer);
                {%- when 'uint' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BinSerialize.ReadUInt(ref buffer);
                {%- when 'long' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BinSerialize.ReadLong(ref buffer);
                {%- when 'ulong' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})BinSerialize.ReadULong(ref buffer);
                {%- endcase -%}
            }

        {%- else -%}
            {%- case field.Type -%}
            {%- when 'sbyte' or 'byte' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BinSerialize.ReadByte(ref buffer);
            {%- when 'short' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BinSerialize.ReadShort(ref buffer);
            {%- when 'ushort' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BinSerialize.ReadUShort(ref buffer);
            {%- when 'int' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BinSerialize.ReadInt(ref buffer);
            {%- when 'uint' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BinSerialize.ReadUInt(ref buffer);
            {%- when 'long' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BinSerialize.ReadLong(ref buffer);
            {%- when 'ulong' -%}
            {{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})BinSerialize.ReadULong(ref buffer);
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
           
            {%- else -%}
            for(var i=0;i<arraySize;i++)
            {
                {%- case field.Type -%}
                {%- when 'sbyte' or 'byte' -%}
                {{ field.CamelCaseName }}[i] = ({{ field.Type }})BinSerialize.ReadByte(ref buffer);
                {%- when 'short' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadShort(ref buffer);
                {%- when 'ushort' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadUShort(ref buffer);
                {%- when 'int' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadInt(ref buffer);
                {%- when 'uint' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadUInt(ref buffer);
                {%- when 'long' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadLong(ref buffer);
                {%- when 'ulong' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadULong(ref buffer);
                {%- when 'float' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadFloat(ref buffer);
                {%- when 'double' -%}
                {{ field.CamelCaseName }}[i] = BinSerialize.ReadDouble(ref buffer);
                {%- endcase -%}
            }
            {%- endif -%}
        {%- else -%}
            {%- if field.Type == 'char' -%}
            {{ field.CamelCaseName }} = (char)buffer[0];
            buffer = buffer.Slice(1);
            
            {%- else -%}
            {%- case field.Type -%}
            {%- when 'sbyte' or 'byte' -%}
            {{ field.CamelCaseName }} = ({{ field.Type }})BinSerialize.ReadByte(ref buffer);
            {%- when 'short' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadShort(ref buffer);
            {%- when 'ushort' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadUShort(ref buffer);
            {%- when 'int' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadInt(ref buffer);
            {%- when 'uint' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadUInt(ref buffer);
            {%- when 'long' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadLong(ref buffer);
            {%- when 'ulong' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadULong(ref buffer);
            {%- when 'float' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadFloat(ref buffer);
            {%- when 'double' -%}
            {{ field.CamelCaseName }} = BinSerialize.ReadDouble(ref buffer);
            {%- endcase -%}
            {%- endif -%}
        {%- endif -%}
    {%- endif -%}
{%- endfor -%}

        }

        public void Serialize(ref Span<byte> buffer)
        {
{%- for field in msg.Fields -%}
    {%- if field.IsEnum -%}
        {%- if field.IsArray -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                {%- case field.Type -%}
                {%- when 'char' or 'double' or 'float'-%}
                ERROR => ENUM as 'char' or 'double' or 'float' ???????
                {%- when 'sbyte' or 'byte' -%}
                BinSerialize.WriteByte(ref buffer,(byte){{ field.CamelCaseName }}[i]);
                {%- when 'short' -%}
                BinSerialize.WriteShort(ref buffer,(short){{ field.CamelCaseName }}[i]);
                {%- when 'ushort' -%}
                BinSerialize.WriteUShort(ref buffer,(ushort){{ field.CamelCaseName }}[i]);
                {%- when 'int' -%}
                BinSerialize.WriteInt(ref buffer,(int){{ field.CamelCaseName }}[i]);
                {%- when 'uint' -%}
                BinSerialize.WriteUInt(ref buffer,(uint){{ field.CamelCaseName }}[i]);
                {%- when 'long' -%}
                BinSerialize.WriteLong(ref buffer,(long){{ field.CamelCaseName }}[i]);
                {%- when 'ulong' -%}
                BinSerialize.WriteULong(ref buffer,(ulong){{ field.CamelCaseName }}[i]);
                
                {%- endcase -%}
            }
        {%- else -%}
            {%- case field.Type -%}
            {%- when 'char' or 'double' or 'float'-%}
            ERROR => ENUM as 'char' or 'double' or 'float' ???????
            {%- when 'sbyte' or 'byte' -%}
            BinSerialize.WriteByte(ref buffer,(byte){{ field.CamelCaseName }});
            {%- when 'short' -%}
            BinSerialize.WriteShort(ref buffer,(short){{ field.CamelCaseName }});
            {%- when 'ushort' -%}
            BinSerialize.WriteUShort(ref buffer,(ushort){{ field.CamelCaseName }});
            {%- when 'int' -%}
            BinSerialize.WriteInt(ref buffer,(int){{ field.CamelCaseName }});
            {%- when 'uint' -%}
            BinSerialize.WriteUInt(ref buffer,(uint){{ field.CamelCaseName }});
            {%- when 'long' -%}
            BinSerialize.WriteLong(ref buffer,(long){{ field.CamelCaseName }});
            {%- when 'ulong' -%}
            BinSerialize.WriteULong(ref buffer,(ulong){{ field.CamelCaseName }});
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
            
            {%- when 'sbyte' or 'byte' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteByte(ref buffer,(byte){{ field.CamelCaseName }}[i]);
            }
            {%- when 'short' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteShort(ref buffer,{{ field.CamelCaseName }}[i]);
            }
            {%- when 'ushort' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteUShort(ref buffer,{{ field.CamelCaseName }}[i]);
            }
            {%- when 'int' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteInt(ref buffer,{{ field.CamelCaseName }}[i]);
            }
            {%- when 'uint' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteUInt(ref buffer,{{ field.CamelCaseName }}[i]);
            }
            {%- when 'long' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteLong(ref buffer,{{ field.CamelCaseName }}[i]);
            }
            {%- when 'ulong' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteULong(ref buffer,{{ field.CamelCaseName }}[i]);
            }
            {%- when 'float' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteFloat(ref buffer,{{ field.CamelCaseName }}[i]);
            }
            {%- when 'double' -%}
            for(var i=0;i<{{ field.CamelCaseName }}.Length;i++)
            {
                BinSerialize.WriteDouble(ref buffer,{{ field.CamelCaseName }}[i]);
            }
            {%- endcase -%}
        {%- else -%}
            {%- case field.Type -%}
            {%- when 'char' -%}
            BinSerialize.WriteByte(ref buffer,(byte){{ field.CamelCaseName }});
            {%- when 'sbyte' or 'byte' -%}
            BinSerialize.WriteByte(ref buffer,(byte){{ field.CamelCaseName }});
            {%- when 'short' -%}
            BinSerialize.WriteShort(ref buffer,{{ field.CamelCaseName }});
            {%- when 'ushort' -%}
            BinSerialize.WriteUShort(ref buffer,{{ field.CamelCaseName }});
            {%- when 'int' -%}
            BinSerialize.WriteInt(ref buffer,{{ field.CamelCaseName }});
            {%- when 'uint' -%}
            BinSerialize.WriteUInt(ref buffer,{{ field.CamelCaseName }});
            {%- when 'long' -%}
            BinSerialize.WriteLong(ref buffer,{{ field.CamelCaseName }});
            {%- when 'ulong' -%}
            BinSerialize.WriteULong(ref buffer,{{ field.CamelCaseName }});
            {%- when 'float' -%}
            BinSerialize.WriteFloat(ref buffer,{{ field.CamelCaseName }});
            {%- when 'double' -%}
            BinSerialize.WriteDouble(ref buffer,{{ field.CamelCaseName }});
            {%- endcase -%}
        {%- endif -%}
    {%- endif -%}
{%- endfor -%}
            /* PayloadByteSize = {{ msg.PayloadByteSize }} */;
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
        public const int {{ field.CamelCaseName }}MaxItemsCount = {{ field.ArrayLength }};    
        public {{ field.EnumCamelCaseName }}[] {{ field.CamelCaseName }} { get; set; } = new {{ field.EnumCamelCaseName }}[{{ field.ArrayLength }}];
            {%- else -%}
        public const int {{ field.CamelCaseName }}MaxItemsCount = {{ field.ArrayLength }};
        public {{ field.EnumCamelCaseName }}[] {{ field.CamelCaseName }} { get; } = new {{ field.EnumCamelCaseName }}[{{ field.ArrayLength }}];
            {%- endif -%}
        {%- else -%}
        public {{ field.EnumCamelCaseName }} {{ field.CamelCaseName }} { get; set; }
       {%- endif -%}
    {%- else -%}
        {%- if field.IsArray -%}
            {%- if field.IsTheLargestArrayInMessage -%}
        public const int {{ field.CamelCaseName }}MaxItemsCount = {{ field.ArrayLength }};
        public {{ field.Type }}[] {{ field.CamelCaseName }} { get; set; } = new {{ field.Type }}[{{ field.ArrayLength }}];
        [Obsolete("This method is deprecated. Use Get{{ field.CamelCaseName }}MaxItemsCount instead.")]
        public byte Get{{ field.CamelCaseName }}MaxItemsCount() => {{ field.ArrayLength }};
            {%- else -%}
        public const int {{ field.CamelCaseName }}MaxItemsCount = {{ field.ArrayLength }};
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
