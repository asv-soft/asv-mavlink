using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace Asv.Mavlink
{
    /// <summary>
    /// This is for parsing https://autotest.ardupilot.org/ParamExteters/ xml files
    /// </summary>
    public static class XmlParamExtDescriptionParser
    {
        public static IEnumerable<ParamExtDescription> Parse(string source)
        {
            var a = new XmlDocument();
            a.LoadXml(source);
            var ParamExtfile = a.GetElementsByTagName("ParamExtfile");
            if (ParamExtfile.Count == 0) yield break;
            foreach (var childNode in ParamExtfile.Item(0).ChildNodes.Cast<XmlNode>())
            {
                foreach (var item in ParseGroup(childNode))
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<ParamExtDescription> ParseGroup(XmlNode group)
        {
            foreach (var ParamExtList in group.ChildNodes.Cast<XmlNode>().Where(n => n.Name == "ParamExteters"))
            {
                foreach (var item in ParseParamExtList(group,ParamExtList))
                {
                    yield return item;
                }
            }


        }

        private static IEnumerable<ParamExtDescription> ParseParamExtList(XmlNode @group, XmlNode ParamExtList)
        {
            foreach (var ParamExt in ParamExtList.ChildNodes.Cast<XmlNode>().Where(n => n.Name == "ParamExt"))
            {
                yield return ParseParamExt(group, ParamExtList, ParamExt);
            }

        }

        private static ParamExtDescription ParseParamExt(XmlNode @group, XmlNode ParamExtList, XmlNode ParamExt)
        {
            var result = new ParamExtDescription
            {
                GroupName = ParamExtList.Attributes["name"]?.Value,
                Name = ParamExt.Attributes["name"]?.Value,
                DisplayName = ParamExt.Attributes["humanName"]?.Value,
                Description = ParamExt.Attributes["documentation"]?.Value,
                User = ParamExt.Attributes["user"]?.Value
            };

            if (result.Name.Contains(":"))
            {
                var combinedName = result.Name.Split(':');
                result.BoardType = combinedName[0];
                result.Name = combinedName[1];
            }

            foreach (var field in ParamExt.ChildNodes.Cast<XmlNode>().Where(n => n.Name == "field"))
            {
                switch (field.Attributes["name"].Value)
                {
                       
                    case "Range":
                        ParseRange(result, field);
                        break;
                    case "Increment":
                        ParseIncrement(result, field);
                        break;
                    case "Units":
                        ParseUnits(result, field);
                        break;
                    case "UnitText":
                        ParseUnitText(result, field);
                        break;
                    case "Bitmask":
                        ParseBitmask(result, field);
                        break;
                    case "ReadOnly":
                        result.IsReadOnly = bool.Parse(field.InnerText);
                        break;
                    case "RebootRequired":
                        result.IsRebootRequired = bool.Parse(field.InnerText);
                        break;
                    case "Calibration":
                        ParseCalibrationField(result, field);
                        break;
                    case "Values":
                        ParseValuesField(result, field);
                        break;
                    default:
                        break;
                }
            }
            foreach (var field in ParamExt.ChildNodes.Cast<XmlNode>().Where(n => n.Name == "values"))
            {
                foreach (var valueField in field.ChildNodes.Cast<XmlNode>())
                {
                    result.AvailableValues.Add(ParseValueField(valueField));
                }
            }

            return result;
        }

        private static void ParseCalibrationField(ParamExtDescription result, XmlNode field)
        {
            result.Calibration = int.Parse(field.InnerText, CultureInfo.InvariantCulture);
        }

        private static void ParseValuesField(ParamExtDescription result, XmlNode field)
        {
            result.Values = field.InnerText;
        }

        private static void ParseBitmask(ParamExtDescription result, XmlNode field)
        {
            
        }

        private static ParamExtDescriptionValue ParseValueField(XmlNode valueField)
        {
            return new ParamExtDescriptionValue
            {
                Code = decimal.Parse(valueField.Attributes["code"].Value, CultureInfo.InvariantCulture),
                Description = valueField.InnerText
            };
        }

        private static void ParseUnitText(ParamExtDescription result, XmlNode fieldValue)
        {
            result.UnitsDisplayName = fieldValue.InnerText;
        }

        private static void ParseUnits(ParamExtDescription result, XmlNode fieldValue)
        {
            result.Units = fieldValue.InnerText;
        }

        private static void ParseIncrement(ParamExtDescription result, XmlNode fieldValue)
        {
            result.Increment = decimal.Parse(fieldValue.InnerText, CultureInfo.InvariantCulture);
        }

        private static void ParseRange(ParamExtDescription result, XmlNode fieldValue)
        {
            result.Min = decimal.Parse(Regex.Replace(fieldValue.InnerText, @"([0-9\.\-\+]*) ([0-9\.\-\+]*)", "$1", RegexOptions.Compiled).Trim('+'), CultureInfo.InvariantCulture);
            result.Max = decimal.Parse(Regex.Replace(fieldValue.InnerText, @"([0-9\.\-\+]*) ([0-9\.\-\+]*)", "$2", RegexOptions.Compiled).Trim('+'), CultureInfo.InvariantCulture);
        }
    }
}
