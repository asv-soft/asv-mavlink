using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DotLiquid;
using DotLiquid.NamingConventions;

namespace Asv.Mavlink.Shell;

public class LiquidGenerator:IMavlinkGenerator
{
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
                        HasTargetSystemIdComponentId = msg.HasTargetSystemAndComponentIdFields,
                        Fields = msg.GetAllFields().Select((field,i) =>
                            new
                            {
                                Index = i,
                                Name = field.Name,
                                CamelCaseName = NameConverter(field.Name ?? throw new InvalidOperationException()),
                                FieldCaseName = FieldNameConverter(field.Name),
                                Units = field.Units,
                                IsExtended = field.IsExtended,
                                FieldTypeByteSize = field.FieldTypeByteSize,
                                ArrayLength = field.ArrayLength,
                                Desc =  field.Desc,
                                EscDesc = EscapeCSharpString(string.Join(" ",field.Desc)),
                                Display = EscapeCSharpString(field.Display),
                                PrintFormat = EscapeCSharpString(field.PrintFormat),
                                InvalidValue = field.Invalid,
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
        
        if (!fieldType.IsArray)
        {
            return type;
        }
            
        return $"{type}[{fieldType.ArrayLength}]";
    }

    public static string? EscapeCSharpString(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new System.Text.StringBuilder(input.Length * 2); // Assume the length might double

        foreach (var c in input)
        {
            switch (c)
            {
                case '\\':
                    result.Append("\\\\"); // Escape backslash
                    break;
                case '"':
                    result.Append("\\\""); // Escape double quote
                    break;
                case '\n':
                    result.Append("\\n"); // Escape newline
                    break;
                case '\r':
                    result.Append("\\r"); // Escape carriage return
                    break;
                case '\t':
                    result.Append("\\t"); // Escape tab
                    break;
                case '\b':
                    result.Append("\\b"); // Escape backspace
                    break;
                case '\f':
                    result.Append("\\f"); // Escape form feed
                    break;
                case '\0':
                    result.Append("\\0"); // Escape null character
                    break;
                default:
                    if (char.IsControl(c) || c < 32 || c > 126) // Escape non-printable and non-ASCII characters
                    {
                        result.Append($"\\u{(int)c:X4}"); // Convert to Unicode sequence
                    }
                    else
                    {
                        result.Append(c); // Leave character as is
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
        
    private string? FieldNameConverter(string name)
    {
        var cname = NameConverter(name);

        if (string.IsNullOrEmpty(cname))
        {
            return cname;
        }
        
        // Name -> _name
        return "_" + char.ToLower(cname[0]) + cname[1..];
    }

    private string ConvertTypeName(MessageFieldType fieldType)
    {
        return fieldType switch
        {
            MessageFieldType.Int8 => "sbyte",
            MessageFieldType.Int16 => "short",
            MessageFieldType.Int32 => "int",
            MessageFieldType.Int64 => "long",
            MessageFieldType.Uint8 => "byte",
            MessageFieldType.Uint16 => "ushort",
            MessageFieldType.Uint32 => "uint",
            MessageFieldType.Float32 => "float",
            MessageFieldType.Uint64 => "ulong",
            MessageFieldType.Char => "char",
            MessageFieldType.Double => "double",
            _ => throw new ArgumentOutOfRangeException(nameof(fieldType), fieldType, null)
        };
    }
}