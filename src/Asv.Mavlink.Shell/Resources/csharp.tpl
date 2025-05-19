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

// This code was generate by tool {{Tool}} version {{ToolVersion}} {{GenerateTime}}.

using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.IO;

namespace Asv.Mavlink.{{ Namespace }}
{

    public static class {{ Namespace }}Helper
    {
        public static void Register{{ Namespace }}Dialect(this ImmutableDictionary<int,Func<MavlinkMessage>>.Builder src)
        {
            {%- for msg in Messages -%}
            src.Add({{ msg.CamelCaseName }}Packet.MessageId, ()=>new {{ msg.CamelCaseName }}Packet());
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
    public class {{ msg.CamelCaseName }}Packet : MavlinkV2Message<{{ msg.CamelCaseName }}Payload>
    {
        public const int MessageId = {{ msg.Id }};
        
        public const byte CrcExtra = {{ msg.CrcExtra }};
        
        public override int Id => MessageId;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte GetCrcExtra() => CrcExtra;
        
        public override bool WrapToV2Extension => {{ msg.WrapToV2Extension }};

        public override {{ msg.CamelCaseName }}Payload Payload { get; } = new();

        public override string Name => "{{ msg.Name }}";
        
        public override ImmutableArray<MavlinkFieldInfo> Fields => StaticFields;
                
        public static readonly ImmutableArray<MavlinkFieldInfo> StaticFields =
        [
{%- for field in msg.Fields -%}
            new({{ forloop.index0 }},
            "{{ field.Name }}",
            {% if field.EscDesc %}"{{ field.EscDesc }}"{% else %}string.Empty{% endif %},
            {% if field.PrintFormat %}"{{ field.PrintFormat }}"{% else %}string.Empty{% endif %}, 
            {% if field.Units %}@"{{ field.Units }}"{% else %}string.Empty{% endif %}, 
            {% if field.Display %}"{{ field.Display }}"{% else %}string.Empty{%- endif %}, 
            {% if field.InvalidValue %}@"{{ field.InvalidValue }}"{% else %}string.Empty{% endif %}, 
            MessageFieldType.{{ field.TypeEnumName }}, 
            {{ field.ArrayLength }}, 
            {% if field.IsExtended %}true{%- else -%}false{% endif %}),
{%- endfor -%}
        ];
        public const string FormatMessage = "{{ msg.Name }}:"
{%- for field in msg.Fields -%}
        + "{{ field.ULogTypeName }} {{ field.Name }};"
{%- endfor -%}
        ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string GetFormatMessage() => FormatMessage;
        
        public override void ReadFields(IMavlinkFieldWriter writer)
        {
{%- for field in msg.Fields -%}
    {%- if field.IsEnum -%}
        {%- if field.IsArray -%}
            writer.Write(StaticFields[{{ forloop.index0 }}], Payload.{{ field.CamelCaseName }}.Select(x=>(ulong)x).ToArray());
        {%- else -%}
            writer.Write(StaticFields[{{ forloop.index0 }}], (ulong)Payload.{{ field.CamelCaseName }});
        {%- endif -%}
    {%- else -%}
            writer.Write(StaticFields[{{ forloop.index0 }}], Payload.{{ field.CamelCaseName }});
    {%- endif -%}
{%- endfor -%}
        }
        
        public override void WriteFields(IMavlinkFieldReader reader)
        {
{%- for field in msg.Fields -%}
    {%- if field.IsEnum -%}
        {%- if field.IsArray -%}
            {%- case field.Type -%}
            {%- when 'char' or 'double' or 'float'-%}
            ERROR => ENUM as 'char' or 'double' or 'float' ???????
            {%- when 'sbyte' -%}
            var array = new sbyte[Payload.{{ field.CamelCaseName }}.Length];
            reader.ReadSByteArray(StaticFields[{{ forloop.index0 }}], array);
            {%- when 'byte' -%}
            var array = new byte[Payload.{{ field.CamelCaseName }}.Length];
            reader.ReadByteArray(StaticFields[{{ forloop.index0 }}], array);
            {%- when 'short' -%}
            var array = new short[Payload.{{ field.CamelCaseName }}.Length];
            reader.ReadShortArray(StaticFields[{{ forloop.index0 }}], array);
            {%- when 'ushort' -%}
            var array = new ushort[Payload.{{ field.CamelCaseName }}.Length];
            reader.ReadUShortArray(StaticFields[{{ forloop.index0 }}], array);
            {%- when 'int' -%}
            var array = new int[Payload.{{ field.CamelCaseName }}.Length];
            reader.ReadIntArray(StaticFields[{{ forloop.index0 }}], array);
            {%- when 'uint' -%}
            var array = new uint[Payload.{{ field.CamelCaseName }}.Length];
            reader.ReadUIntArray(StaticFields[{{ forloop.index0 }}], array);
            {%- when 'long' -%}
            var array = new long[Payload.{{ field.CamelCaseName }}.Length];
            reader.ReadLongArray(StaticFields[{{ forloop.index0 }}], array);
            {%- when 'ulong' -%}
            var array = new ulong[Payload.{{ field.CamelCaseName }}.Length];
            reader.ReadULongArray(StaticFields[{{ forloop.index0 }}], array);
            {%- endcase -%}
            for(var i=0;i<array.Length;i++)
            {
                Payload.{{ field.CamelCaseName }}[i] = ({{ field.EnumCamelCaseName }})array[i];
            }
        {%- else -%}
            {%- case field.Type -%}
            {%- when 'char' or 'double' or 'float'-%}
            ERROR => ENUM as 'char' or 'double' or 'float' ???????
            {%- when 'sbyte' -%}
            Payload.{{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})reader.ReadSByte(StaticFields[{{ forloop.index0 }}]);
            {%- when 'byte' -%}
            Payload.{{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})reader.ReadByte(StaticFields[{{ forloop.index0 }}]);
            {%- when 'short' -%}
            Payload.{{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})reader.ReadShort(StaticFields[{{ forloop.index0 }}]);
            {%- when 'ushort' -%}
            Payload.{{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})reader.ReadUShort(StaticFields[{{ forloop.index0 }}]);
            {%- when 'int' -%}
            Payload.{{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})reader.ReadInt(StaticFields[{{ forloop.index0 }}]);
            {%- when 'uint' -%}
            Payload.{{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})reader.ReadUInt(StaticFields[{{ forloop.index0 }}]);
            {%- when 'long' -%}
            Payload.{{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})reader.ReadLong(StaticFields[{{ forloop.index0 }}]);
            {%- when 'ulong' -%}
            Payload.{{ field.CamelCaseName }} = ({{ field.EnumCamelCaseName }})reader.ReadULong(StaticFields[{{ forloop.index0 }}]);
            {%- endcase -%}
        {%- endif -%}
    {%- else -%}
        {%- if field.IsArray -%}
            {%- case field.Type -%}
            {%- when 'char' -%}
            reader.ReadCharArray(StaticFields[{{ forloop.index0 }}], Payload.{{ field.CamelCaseName }});
            {%- when 'sbyte' -%}
            reader.ReadSByteArray(StaticFields[{{ forloop.index0 }}], Payload.{{ field.CamelCaseName }});
            {%- when 'byte' -%}
            reader.ReadByteArray(StaticFields[{{ forloop.index0 }}], Payload.{{ field.CamelCaseName }});
            {%- when 'short' -%}
            reader.ReadShortArray(StaticFields[{{ forloop.index0 }}], Payload.{{ field.CamelCaseName }});
            {%- when 'ushort' -%}
            reader.ReadUShortArray(StaticFields[{{ forloop.index0 }}], Payload.{{ field.CamelCaseName }});
            {%- when 'int' -%}
            reader.ReadIntArray(StaticFields[{{ forloop.index0 }}], Payload.{{ field.CamelCaseName }});
            {%- when 'uint' -%}
            reader.ReadUIntArray(StaticFields[{{ forloop.index0 }}],Payload.{{ field.CamelCaseName }});
            {%- when 'long' -%}
            reader.ReadLongArray(StaticFields[{{ forloop.index0 }}], Payload.{{ field.CamelCaseName }});
            {%- when 'ulong' -%}
            reader.ReadULongArray(StaticFields[{{ forloop.index0 }}], Payload.{{ field.CamelCaseName }});
            {%- when 'float' -%}
            reader.ReadFloatArray(StaticFields[{{ forloop.index0 }}], Payload.{{ field.CamelCaseName }});
            {%- when 'double' -%}
            reader.ReadDoubleArray(StaticFields[{{ forloop.index0 }}], Payload.{{ field.CamelCaseName }});
            {%- endcase -%}
        {%- else -%}
            {%- case field.Type -%}
            {%- when 'char' -%}
            Payload.{{ field.CamelCaseName }} = reader.ReadChar(StaticFields[{{ forloop.index0 }}]);
            {%- when 'sbyte' -%}
            Payload.{{ field.CamelCaseName }} = reader.ReadSByte(StaticFields[{{ forloop.index0 }}]);
            {%- when 'byte' -%}
            Payload.{{ field.CamelCaseName }} = reader.ReadByte(StaticFields[{{ forloop.index0 }}]);
            {%- when 'short' -%}
            Payload.{{ field.CamelCaseName }} = reader.ReadShort(StaticFields[{{ forloop.index0 }}]);
            {%- when 'ushort' -%}
            Payload.{{ field.CamelCaseName }} = reader.ReadUShort(StaticFields[{{ forloop.index0 }}]);
            {%- when 'int' -%}
            Payload.{{ field.CamelCaseName }} = reader.ReadInt(StaticFields[{{ forloop.index0 }}]);
            {%- when 'uint' -%}
            Payload.{{ field.CamelCaseName }} = reader.ReadUInt(StaticFields[{{ forloop.index0 }}]);
            {%- when 'long' -%}
            Payload.{{ field.CamelCaseName }} = reader.ReadLong(StaticFields[{{ forloop.index0 }}]);
            {%- when 'ulong' -%}
            Payload.{{ field.CamelCaseName }} = reader.ReadULong(StaticFields[{{ forloop.index0 }}]);
            {%- when 'float' -%}
            Payload.{{ field.CamelCaseName }} = reader.ReadFloat(StaticFields[{{ forloop.index0 }}]);
            {%- when 'double' -%}
            Payload.{{ field.CamelCaseName }} = reader.ReadDouble(StaticFields[{{ forloop.index0 }}]);
            {%- endcase -%}
        {%- endif -%}
    {%- endif -%}
{%- endfor -%}
        
            
        }
    }

    /// <summary>
    ///  {{ msg.Name }}
    /// </summary>
    public class {{ msg.CamelCaseName }}Payload : IPayload
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaxByteSize() => {{ msg.PayloadByteSize }}; // Sum of byte sized of all fields (include extended)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMinByteSize() => {{ msg.PayloadByteSize - msg.ExtendedFieldsLength }}; // of byte sized of fields (exclude extended)
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public int GetByteSize()
        {
            return (byte)(
{%- for field in msg.Fields -%}
    {%- if field.IsEnum -%}
        {%- if field.IsArray -%}
            {%- case field.Type -%}
            {%- when 'char' or 'double' or 'float'-%}
            ERROR => ENUM as 'char' or 'double' or 'float' ???????
            {%- when 'sbyte' or 'byte' -%}
            + {{ field.CamelCaseName }}.Length // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'short' -%}
            + {{ field.CamelCaseName }}.Length * 2 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'ushort' -%}
            + {{ field.CamelCaseName }}.Length * 2 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'int' -%}
            + {{ field.CamelCaseName }}.Length * 4 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'uint' -%}
            + {{ field.CamelCaseName }}.Length * 4 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'long' -%}
            + {{ field.CamelCaseName }}.Length * 8 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'ulong' -%}
            + {{ field.CamelCaseName }}.Length * 8 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- endcase -%}
            
        {%- else -%}
            {%- case field.Type -%}
            {%- when 'char' or 'double' or 'float'-%}
            ERROR => ENUM as 'char' or 'double' or 'float' ???????
            {%- when 'sbyte' or 'byte' -%}
            + 1 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'short' -%}
            + 2 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'ushort' -%}
            + 2 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'int' -%}
            + 4 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'uint' -%}
            + 4 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'long' -%}
            + 8 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'ulong' -%}
            + 8 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- endcase -%}
        {%- endif -%}
    {%- else -%}
        {%- if field.IsArray -%}
            {%- case field.Type -%}
            {%- when 'char' -%}
            +{{ field.CamelCaseName }}.Length // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'sbyte' or 'byte' -%}
            +{{ field.CamelCaseName }}.Length // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'short' -%}
            +{{ field.CamelCaseName }}.Length * 2 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'ushort' -%}
            +{{ field.CamelCaseName }}.Length * 2 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'int' -%}
            +{{ field.CamelCaseName }}.Length * 4 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'uint' -%}
            +{{ field.CamelCaseName }}.Length * 4 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'long' -%}
            +{{ field.CamelCaseName }}.Length * 8 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'ulong' -%}
            +{{ field.CamelCaseName }}.Length * 8 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'float' -%}
            +{{ field.CamelCaseName }}.Length * 4 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'double' -%}
            +{{ field.CamelCaseName }}.Length * 8 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- endcase -%}
        {%- else -%}
            {%- case field.Type -%}
            {%- when 'char' -%}
            +1 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'sbyte' or 'byte' -%}
            +1 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'short' -%}
            +2 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'ushort' -%}
            +2 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'int' -%}
            +4 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'uint' -%}
            +4 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'long' -%}
            +8 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'ulong' -%}
            +8 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'float' -%}
            +4 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- when 'double' -%}
            +8 // {{ field.ULogTypeName }} {{ field.Name }}
            {%- endcase -%}
        {%- endif -%}
    {%- endif -%}
{%- endfor -%}
            );
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
