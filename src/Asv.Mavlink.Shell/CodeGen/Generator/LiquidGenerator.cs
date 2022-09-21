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
                    Tool = typeof(Program).Assembly.GetTitle(),
                    ToolVersion = typeof(Program).Assembly.GetInformationalVersion(),
                    Dialect = model.Dialect,
                    Version = model.Version,
                    Namespace = NameConverter(Path.GetFileNameWithoutExtension(model.FileName)),
                    Enums = model.Enums.Select(en =>
                                                   new
                                                   {
                                                       Value = en.Value,
                                                       Desc = en.Desc,
                                                       Name = en.Name,
                                                       CamelCaseName = NameConverter(en.Name),
                                                       Entries = en.Entries.Select(
                                                           enEntry => new
                                                                      {
                                                                          Name = enEntry.Name,
                                                                          CamelCaseName = NameConverter(enEntry.Name),
                                                                          Desc =  enEntry.Desc,
                                                                          Value = enEntry.Value,
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
                                                             CamelCaseName = NameConverter(msg.Name),
                                                             Desc = msg.Desc,
                                                             Id = msg.Id,
                                                             CrcExtra = msg.CrcExtra,
                                                             PayloadByteSize = msg.GetAllFields().Sum(_=>_.FieldByteSize),
                                                             ExtendedFieldsLength = msg.ExtendedFields.Sum(_=>_.FieldByteSize),
                                                             Fields = msg.GetAllFields().Select(field =>
                                                                                            new
                                                                                            {
                                                                                                Name = field.Name,
                                                                                                CamelCaseName = NameConverter(field.Name),
                                                                                                Units = field.Units,
                                                                                                IsExtended = field.IsExtended,
                                                                                                FieldTypeByteSize = field.FieldTypeByteSize,
                                                                                                ArrayLength = field.ArrayLength,
                                                                                                Desc = field.Desc,
                                                                                                IsArray = field.IsArray,
                                                                                                IsTheLargestArrayInMessage = field.IsTheLargestArrayInMessage,
                                                                                                IsEnum = field.Enum != null,
                                                                                                Type = ConvertTypeName(field.Type),
                                                                                                EnumCamelCaseName = NameConverter(field.Enum),
                                                                                            })
                                                         })
                });

            var result = Template.Parse(template);
            return result.Render(args);
        }

        private string NameConverter(string name)
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
