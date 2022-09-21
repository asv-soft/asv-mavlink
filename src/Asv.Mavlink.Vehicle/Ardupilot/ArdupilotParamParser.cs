using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace Asv.Mavlink
{
    public static class ArdupilotParamParser
    {
        public static IEnumerable<VehicleParamDescription> Parse(string source)
        {
            var a = new XmlDocument();
            a.LoadXml(source);
            var paramfile = a.GetElementsByTagName("paramfile");
            if (paramfile.Count == 0) yield break;
            foreach (var childNode in paramfile.Item(0).ChildNodes.Cast<XmlNode>())
            {
                foreach (var item in ParseGroup(childNode))
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<VehicleParamDescription> ParseGroup(XmlNode group)
        {
            foreach (var paramList in group.ChildNodes.Cast<XmlNode>().Where(_ => _.Name == "parameters"))
            {
                foreach (var item in ParseParamList(group,paramList))
                {
                    yield return item;
                }
            }


        }

        private static IEnumerable<VehicleParamDescription> ParseParamList(XmlNode @group, XmlNode paramList)
        {
            foreach (var param in paramList.ChildNodes.Cast<XmlNode>().Where(_ => _.Name == "param"))
            {
                yield return ParseParam(group, paramList, param);
            }

        }

        private static VehicleParamDescription ParseParam(XmlNode @group, XmlNode paramList, XmlNode param)
        {
            var result = new VehicleParamDescription
            {
                GroupName = paramList.Attributes["name"]?.Value,
                Name = param.Attributes["name"]?.Value,
                DisplayName = param.Attributes["humanName"]?.Value,
                Description = param.Attributes["documentation"]?.Value,
                User = param.Attributes["user"]?.Value
            };

            if (result.Name.Contains(":"))
            {
                var combinedName = result.Name.Split(':');
                result.BoardType = combinedName[0];
                result.Name = combinedName[1];
            }

            foreach (var field in param.ChildNodes.Cast<XmlNode>().Where(_ => _.Name == "field"))
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
            foreach (var field in param.ChildNodes.Cast<XmlNode>().Where(_ => _.Name == "values"))
            {
                foreach (var valueField in field.ChildNodes.Cast<XmlNode>())
                {
                    result.AvailableValues.Add(ParseValueField(valueField));
                }
            }

            return result;
        }

        private static void ParseCalibrationField(VehicleParamDescription result, XmlNode field)
        {
            result.Calibration = int.Parse(field.InnerText, CultureInfo.InvariantCulture);
        }

        private static void ParseValuesField(VehicleParamDescription result, XmlNode field)
        {
            result.Values = field.InnerText;
        }

        private static void ParseBitmask(VehicleParamDescription result, XmlNode field)
        {
            
        }

        private static VehicleParamValue ParseValueField(XmlNode valueField)
        {
            return new VehicleParamValue
            {
                Code = decimal.Parse(valueField.Attributes["code"].Value, CultureInfo.InvariantCulture),
                Description = valueField.InnerText
            };
        }

        private static void ParseUnitText(VehicleParamDescription result, XmlNode fieldValue)
        {
            result.UnitsDisplayName = fieldValue.InnerText;
        }

        private static void ParseUnits(VehicleParamDescription result, XmlNode fieldValue)
        {
            result.Units = fieldValue.InnerText;
        }

        private static void ParseIncrement(VehicleParamDescription result, XmlNode fieldValue)
        {
            result.Increment = decimal.Parse(fieldValue.InnerText, CultureInfo.InvariantCulture);
        }

        private static void ParseRange(VehicleParamDescription result, XmlNode fieldValue)
        {
            result.Min = decimal.Parse(Regex.Replace(fieldValue.InnerText, @"([0-9\.\-\+]*) ([0-9\.\-\+]*)", "$1", RegexOptions.Compiled).Trim('+'), CultureInfo.InvariantCulture);
            result.Max = decimal.Parse(Regex.Replace(fieldValue.InnerText, @"([0-9\.\-\+]*) ([0-9\.\-\+]*)", "$2", RegexOptions.Compiled).Trim('+'), CultureInfo.InvariantCulture);
        }
    }
}
