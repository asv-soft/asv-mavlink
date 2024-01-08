using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Asv.Mavlink.Shell
{
    public class MavlinkParserXml : IMavlinkParser
    {
        public MavlinkProtocolModel Parse(string fileName, Stream strm)
        {

            var protocol = new MavlinkProtocolModel
                           {
                               FileName = fileName,
            };
            XmlReader rdr = new XmlTextReader(strm);
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
                                          Id = int.Parse(rdr.GetAttribute("id")),
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
            bool isArray;
            byte arrayLength;
            string baseTypeName;
            var type = ParseFieldType(rdr.GetAttribute("type"), out baseTypeName, out isArray, out arrayLength);
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
                           Desc = ConvertDesc(rdr.ReadElementContentAsString()),
                       });

        }

        private static string FieldTypeToString(MessageFieldType type)
        {
            switch (type)
            {
                case MessageFieldType.Int8:
                    return "int8_t";
                case MessageFieldType.Int16:
                    return "int16_t";
                case MessageFieldType.Int32:
                    return "int32_t";
                case MessageFieldType.Int64:
                    return "int64_t";
                case MessageFieldType.Uint8:
                    return "uint8_t";
                case MessageFieldType.Uint16:
                    return "uint16_t";
                case MessageFieldType.Uint32:
                    return "uint32_t";
                case MessageFieldType.Float32:
                    return "float";
                case MessageFieldType.Uint64:
                    return "uint64_t";
                case MessageFieldType.Char:
                    return "char";
                case MessageFieldType.Double:
                    return "double";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static MessageFieldType ParseFieldType(string typeString,out string typeName, out bool isArrayType, out byte arrayLength)
        {
            typeName = CheckFieldTypeForArray(typeString, out isArrayType, out arrayLength);
            switch (typeName)
            {
                case "float": return MessageFieldType.Float32;
                case "int8_t": return MessageFieldType.Int8;
                case "uint8_t": return MessageFieldType.Uint8;
                case "int16_t": return MessageFieldType.Int16;
                case "uint16_t": return MessageFieldType.Uint16;
                case "int32_t": return MessageFieldType.Int32;
                case "uint32_t": return MessageFieldType.Uint32;
                case "int64_t": return MessageFieldType.Int64;
                case "uint64_t": return MessageFieldType.Uint64;
                case "char": return MessageFieldType.Char;
                case "double": return MessageFieldType.Double;
                case "uint8_t_mavlink_version": return MessageFieldType.Uint8;
                default:
                    throw new Exception($"Unknown type {typeString}");

            }
        }

        private static string CheckFieldTypeForArray(string typeString, out bool isArrayType, out byte arrayLength)
        {
            var mathes = Regex.Match(typeString, @"([a-zA-Z0-9_]*)(\[)([0-9]*)(\])", RegexOptions.Compiled);
            if (mathes.Groups.Count == 1)
            {
                isArrayType = false;
                arrayLength = 0;
                return typeString;
            }
            isArrayType = true;
            arrayLength = byte.Parse(mathes.Groups[3].Value);
            return mathes.Groups[1].Value;
            
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
                                       Name = rdr.GetAttribute("name")
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
                Console.WriteLine($"Error to parse enum '{rdr.Name}' value '{valueStr}':{e.Message}");
                throw;
            }
            
            var result = new MavlinkEnumEntryModel
                         {
                             Name = rdr.GetAttribute("name"),
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
                        result.Desc = ConvertDesc(rdr.ReadElementContentAsString());;
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
}
