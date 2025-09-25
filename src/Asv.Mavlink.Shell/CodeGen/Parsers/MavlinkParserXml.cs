using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class MavlinkParserXml : IMavlinkParser
{
    public MavlinkProtocolModel Parse(string fileName, Stream stream)
    {
        var protocol = new MavlinkProtocolModel { FileName = fileName };
            
        XmlReader rdr = new XmlTextReader(stream);
        rdr.Read();
        
        while (rdr.Read())
        {
            switch (rdr.Name)
            {
                case "include":
                    protocol.Include.Add(rdr.ReadElementContentAsString());
                    break;
                case "version":
                    protocol.Version = rdr.ReadElementContentAsInt();
                    break;
                case "dialect":
                    protocol.Dialect = rdr.ReadElementContentAsInt();
                    break;
                case "enums":
                    ParseEnumsSection(rdr, protocol.Enums);
                    break;
                case "messages":
                    ParseMessagesSection(rdr, protocol.Messages);
                    break;
            }
        }
        
        return protocol;
    }

    private static void ParseMessagesSection(XmlReader rdr, IList<MavlinkMessageModel> protocolEnums)
    {
        var depth = rdr.Depth+1;
        
        do
        {
            rdr.Read();
            if (rdr.Name == "message")
            {
                var messageItem = new MavlinkMessageModel
                {
                    Id = int.Parse(rdr.GetAttribute("id") ?? throw new InvalidOperationException()),
                    Name = rdr.GetAttribute("name"),
                };
                protocolEnums.Add(messageItem);
                ParseMessage(rdr, messageItem);
            }

        } while (rdr.Depth >= depth);
    }

    private static void ParseMessage(XmlReader rdr, MavlinkMessageModel messageItem)
    {
        var depth = rdr.Depth + 1;
        var extendedFields = false;
        
        do
        {
            rdr.Read();
            switch (rdr.Name)
            {
                case "description":
                    var content = rdr.ReadElementContentAsString();
                    messageItem.Desc = ConvertDesc(content);
                    messageItem.WrapToV2Extension = content.Contains("[!WRAP_TO_V2_EXTENSION_PACKET!]");
                    break;
                case "field":
                    ParseMessageFields(rdr, messageItem, extendedFields);
                    break;
                case "extensions":
                    extendedFields = true;
                    break;
            }
        } while (rdr.Depth >= depth);

        messageItem.ReorderFieldsAndClacCrc();
        messageItem.CalculateLargestArray();
    }

    private static string[] ConvertDesc(string desc)
    {
        return Regex.Split(desc, "\r\n|\r|\n", RegexOptions.Compiled);
    }

    private static void ParseMessageFields(XmlReader rdr, MavlinkMessageModel msg, bool extendedFields)
    {
        var type = ParseFieldType(
            rdr.GetAttribute("type") ?? throw new InvalidOperationException(), 
            out _, 
            out var isArray, 
            out var arrayLength);
        var fields = extendedFields ?  msg.ExtendedFields : msg.Fields;

        fields.Add(new MessageFieldModel
        {
            IsExtended = extendedFields,
            Type = type,
            TypeName = FieldTypeToString(type),
            FieldTypeByteSize = type.GetFieldTypeByteSize(),
            IsArray = isArray,
            ArrayLength = arrayLength,
            Name = rdr.GetAttribute("name"),
            Units = rdr.GetAttribute("units"),
            Enum = rdr.GetAttribute("enum"),
            Display = rdr.GetAttribute("display"),
            PrintFormat = rdr.GetAttribute("print_format"),
            Invalid = rdr.GetAttribute("invalid"),
            Desc = ConvertDesc(rdr.ReadElementContentAsString()),
        });
    }

    private static string FieldTypeToString(MessageFieldType type)
    {
        return type switch
        {
            MessageFieldType.Int8 => "int8_t",
            MessageFieldType.Int16 => "int16_t",
            MessageFieldType.Int32 => "int32_t",
            MessageFieldType.Int64 => "int64_t",
            MessageFieldType.Uint8 => "uint8_t",
            MessageFieldType.Uint16 => "uint16_t",
            MessageFieldType.Uint32 => "uint32_t",
            MessageFieldType.Float32 => "float",
            MessageFieldType.Uint64 => "uint64_t",
            MessageFieldType.Char => "char",
            MessageFieldType.Double => "double",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private static MessageFieldType ParseFieldType(
        string typeString, 
        out string typeName, 
        out bool isArrayType, 
        out byte arrayLength)
    {
        typeName = CheckFieldTypeForArray(typeString, out isArrayType, out arrayLength);
        
        return typeName switch
        {
            "float" => MessageFieldType.Float32,
            "int8_t" => MessageFieldType.Int8,
            "uint8_t" => MessageFieldType.Uint8,
            "int16_t" => MessageFieldType.Int16,
            "uint16_t" => MessageFieldType.Uint16,
            "int32_t" => MessageFieldType.Int32,
            "uint32_t" => MessageFieldType.Uint32,
            "int64_t" => MessageFieldType.Int64,
            "uint64_t" => MessageFieldType.Uint64,
            "char" => MessageFieldType.Char,
            "double" => MessageFieldType.Double,
            "uint8_t_mavlink_version" => MessageFieldType.Uint8,
            _ => throw new MavlinkException($"Unknown type {typeString}")
        };
    }

    private static string CheckFieldTypeForArray(string typeString, out bool isArrayType, out byte arrayLength)
    {
        var matches = Regex.Match(typeString, @"([a-zA-Z0-9_]*)(\[)([0-9]*)(\])", RegexOptions.Compiled);
        if (matches.Groups.Count == 1)
        {
            isArrayType = false;
            arrayLength = 0;
            return typeString;
        }
        isArrayType = true;
        arrayLength = byte.Parse(matches.Groups[3].Value);
        return matches.Groups[1].Value;
    }

    private static void ParseEnumsSection(XmlReader rdr, ICollection<MavlinkEnumModel> protocolEnums)
    {
        var depth = rdr.Depth+1;
        do
        {
            rdr.Read();
            if (rdr.Name == "enum")
            {
                var enumItem = new MavlinkEnumModel
                {
                    Name = rdr.GetAttribute("name") ?? throw new InvalidOperationException()
                };
                protocolEnums.Add(enumItem);
                ParseEnum(rdr, enumItem);
            }
        } while (rdr.Depth >= depth);
    }

    private static void ParseEnum(XmlReader rdr, MavlinkEnumModel enumItem)
    {
        var depth = rdr.Depth+1;
        do
        {
            rdr.Read();
            switch (rdr.Name)
            {
                case "description":
                    var content = rdr.ReadElementContentAsString();
                    enumItem.Desc = ConvertDesc(content);
                    enumItem.IsFlag = content.Contains("[!THIS_IS_ENUM_FLAG!]");
                    break;
                case "entry":
                    ParseEnumEntry(rdr,enumItem.Entries);
                    break;
            }
        } while (rdr.Depth >= depth);
    }

    private static void ParseEnumEntry(XmlReader rdr, ICollection<MavlinkEnumEntryModel> enumEntry)
    {
        var valueStr = rdr.GetAttribute("value");
        long enumValue;
        try
        {
            enumValue = string.IsNullOrWhiteSpace(valueStr) ? enumEntry.Count : long.Parse(valueStr);
        }
        catch (Exception e)
        {
            AnsiConsole.WriteLine($"Error to parse enum '{rdr.Name}' value '{valueStr}':{e.Message}");
            throw;
        }
            
        var result = new MavlinkEnumEntryModel
        {
            Name = rdr.GetAttribute("name") ?? throw new InvalidOperationException(),
            // value can be empty !
            Value = enumValue
        };
        var depth = rdr.Depth+1;
        do
        {
            rdr.Read();
            switch (rdr.Name)
            {
                case "description":
                    var content = rdr.ReadElementContentAsString();
                    result.Desc = ConvertDesc(content);
                    result.HasMetadataDescription = content.Contains("[!METADATA!]");;
                    if (result.HasMetadataDescription)
                    {
                        result.MetadataDescription = content.Replace("[!METADATA!]", string.Empty);
                    }
                    break;
                case "param":
                    ParseEnumEntryParam(rdr,result.Params);
                    break;
            }
        } while (rdr.Depth >= depth);

        enumEntry.Add(result);
    }

    private static void ParseEnumEntryParam(XmlReader rdr, ICollection<MavlinkEnumEntryParamModel> enumEntryParams)
    {
        var indexStr = rdr.GetAttribute("index");
        var index = string.IsNullOrWhiteSpace(indexStr) ? enumEntryParams.Count : int.Parse(indexStr);

        enumEntryParams.Add(new MavlinkEnumEntryParamModel
        {
            Desc = ConvertDesc(rdr.ReadElementContentAsString()),
            Index = index,
        });
    }
}