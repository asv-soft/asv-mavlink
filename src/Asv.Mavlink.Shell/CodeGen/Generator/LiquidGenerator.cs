using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DotLiquid;
using DotLiquid.NamingConventions;

namespace Asv.Mavlink.Shell
{
    public class LiquidGenerator:IMavlinkGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        static LiquidGenerator()
        {
            Template.NamingConvention = new CSharpNamingConvention();
        }

        public string Generate(string template, MavlinkProtocolModel model)
        {
            var args = Hash.FromAnonymousObject(
                new
                {
                    GenerateTime = DateTime.Now.ToString("yy-MM-dd"),
                    Tool = typeof(Program).Assembly.GetTitle(),
                    ToolVersion = typeof(Program).Assembly.GetInformationalVersion(),
                    Dialect = model.Dialect,
                    Version = model.Version,
                    Namespace = NameConverter(Path.GetFileNameWithoutExtension(model.FileName) ?? throw new InvalidOperationException()),
                    Enums = model.Enums.Select(en =>
                                                   new
                                                   {
                                                       Value = en.Value,
                                                       Desc = en.Desc,
                                                       Name = en.Name,
                                                       IsFlag = en.IsFlag,
                                                       CamelCaseName = NameConverter(en.Name ?? throw new InvalidOperationException()),
                                                       Entries = en.Entries.Select(
                                                           enEntry => new
                                                                      {
                                                                          Name = enEntry.Name,
                                                                          CamelCaseName = NameConverter(enEntry.Name),
                                                                          Desc =  enEntry.Desc,
                                                                          Value = enEntry.Value,
                                                                          HasMetadataDescription = enEntry.HasMetadataDescription,
                                                                          MetadataDescription = enEntry.MetadataDescription,
                                                                          Params = enEntry.Params.Select(p => new
                                                                                                              {
                                                                                                                  p.Index,
                                                                                                                  p.Desc,
                                                                                                              })
                                                                      })
                                                   }),
                    Messages = model.Messages.Select(msg =>
                                                         new
                                                         {
                                                             Name = msg.Name,
                                                             CamelCaseName = NameConverter(msg.Name ?? throw new InvalidOperationException()),
                                                             Desc = msg.Desc,
                                                             Id = msg.Id,
                                                             CrcExtra = msg.CrcExtra,
                                                             HasArrayFields = msg.HasArrayFields,
                                                             PayloadByteSize = msg.GetAllFields().Sum(_=>_.FieldByteSize),
                                                             ExtendedFieldsLength = msg.ExtendedFields.Sum(_=>_.FieldByteSize),
                                                             WrapToV2Extension = msg.WrapToV2Extension,
                                                             Fields = msg.GetAllFields().Select((field,i) =>
                                                                                            new
                                                                                            {
                                                                                                Index = i,
                                                                                                Name = field.Name,
                                                                                                CamelCaseName = NameConverter(field.Name ?? throw new InvalidOperationException()),
                                                                                                Units = field.Units,
                                                                                                IsExtended = field.IsExtended,
                                                                                                FieldTypeByteSize = field.FieldTypeByteSize,
                                                                                                ArrayLength = field.ArrayLength,
                                                                                                Desc =  field.Desc,
                                                                                                EscDesc = EscapeCSharpString(string.Join(" ",field.Desc)),
                                                                                                Display = EscapeCSharpString(field.Display),
                                                                                                PrintFormat = EscapeCSharpString(field.PrintFormat),
                                                                                                InvalidValue = field.Inavlid,
                                                                                                IsArray = field.IsArray,
                                                                                                IsTheLargestArrayInMessage = field.IsTheLargestArrayInMessage,
                                                                                                IsEnum = field.Enum != null,
                                                                                                Type = ConvertTypeName(field.Type),
                                                                                                ULogTypeName = ConvertUlogTypeName(field),
                                                                                                TypeEnumName = field.Type.ToString("G"),
                                                                                                EnumCamelCaseName = NameConverter(field.Enum ?? String.Empty),
                                                                                            })
                                                         })
                });

            var result = Template.Parse(template);
            return result.Render(args);
        }

        private static string ConvertUlogTypeName(MessageFieldModel fieldType)
        {
            var type = fieldType.Type switch
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
                _ => throw new ArgumentOutOfRangeException(nameof(fieldType), fieldType, null)
            };
            if (fieldType.IsArray == false)
            {
                return type;
            }
            
            return $"{type}[{fieldType.ArrayLength}]";
        }

        public static string? EscapeCSharpString(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return input; // Возвращаем пустую строку или null без изменений

            var result = new System.Text.StringBuilder(input.Length * 2); // Предполагаем, что длина может удвоиться

            foreach (char c in input)
            {
                switch (c)
                {
                    case '\\':
                        result.Append("\\\\"); // Экранируем обратный слеш
                        break;
                    case '"':
                        result.Append("\\\""); // Экранируем кавычки
                        break;
                    case '\n':
                        result.Append("\\n"); // Экранируем перевод строки
                        break;
                    case '\r':
                        result.Append("\\r"); // Экранируем возврат каретки
                        break;
                    case '\t':
                        result.Append("\\t"); // Экранируем табуляцию
                        break;
                    case '\b':
                        result.Append("\\b"); // Экранируем backspace
                        break;
                    case '\f':
                        result.Append("\\f"); // Экранируем form feed
                        break;
                    case '\0':
                        result.Append("\\0"); // Экранируем нулевой символ
                        break;
                    default:
                        if (char.IsControl(c) || c < 32 || c > 126) // Экранируем непечатаемые символы и не-ASCII
                        {
                            result.Append($"\\u{(int)c:X4}"); // Преобразуем в Unicode-последовательность
                        }
                        else
                        {
                            result.Append(c); // Оставляем символ как есть
                        }
                        break;
                }
            }

            return result.ToString();
        }
        
        private string? NameConverter(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            var a = Regex.Replace(name.ToLower(), "_([a-z0-9])", _=>_.Value.Substring(1).ToUpper(), RegexOptions.Compiled);
            a = a.Substring(0, 1).ToUpper() + a.Substring(1);
            return a;
        }

        private string ConvertTypeName(MessageFieldType fieldType)
        {
            switch (fieldType)
            {
                case MessageFieldType.Int8:
                    return "sbyte";
                case MessageFieldType.Int16:
                    return "short";
                case MessageFieldType.Int32:
                    return "int";
                case MessageFieldType.Int64:
                    return "long";
                case MessageFieldType.Uint8:
                    return "byte";
                case MessageFieldType.Uint16:
                    return "ushort";
                case MessageFieldType.Uint32:
                    return "uint";
                case MessageFieldType.Float32:
                    return "float";
                case MessageFieldType.Uint64:
                    return "ulong";
                case MessageFieldType.Char:
                    return "char";
                case MessageFieldType.Double:
                    return "double";
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldType), fieldType, null);
            }
        }
    }
}
