using System;
using Asv.Cfg;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;







// public class MavParamDesc
// {
//     public MavParamDesc(string name, MavParamType type, decimal defaultValue)
//     {
//         switch (type)
//         {
//             case MavParamType.MavParamTypeUint8:
//                 MaxValue = byte.MaxValue;
//                 MinValue = byte.MinValue;
//                 break;
//             case MavParamType.MavParamTypeInt8:
//                 MaxValue = sbyte.MaxValue;
//                 MinValue = sbyte.MinValue;
//                 break;
//             case MavParamType.MavParamTypeUint16:
//                 MaxValue = ushort.MaxValue;
//                 MinValue = ushort.MinValue;
//                 break;
//             case MavParamType.MavParamTypeInt16:
//                 MaxValue = short.MaxValue;
//                 MinValue = short.MinValue;
//                 break;
//             case MavParamType.MavParamTypeUint32:
//                 MaxValue = uint.MaxValue;
//                 MinValue = uint.MinValue;
//                 break;
//             case MavParamType.MavParamTypeInt32:
//                 MaxValue = int.MaxValue;
//                 MinValue = int.MinValue;
//                 break;
//             case MavParamType.MavParamTypeUint64:
//                 break;
//             
//             case MavParamType.MavParamTypeReal32:
//                 MaxValue = float.MaxValue;
//                 MinValue = float.MinValue;
//                 break;
//             case MavParamType.MavParamTypeReal64:
//             case MavParamType.MavParamTypeInt64:
//             default:
//                 throw new ArgumentOutOfRangeException(nameof(type), type, null);
//         }
//     }
//     public string Name { get; set; }
//     public MavParamType Type { get; set; }
//     public decimal DefaultValue { get; set; }
//     
//     public decimal MaxValue { get; set; }
//     public decimal MinValue { get; set; }
//
//     public bool Validate(decimal value, out string error)
//     {
//         switch (Type)
//         {
//             case MavParamType.MavParamTypeUint8:
//                 value = decimal.Floor(value);
//                 if (value is > byte.MaxValue or < byte.MinValue)
//                 {
//                     error = $"must be {byte.MinValue}..{byte.MaxValue}";
//                     return false;
//                 }
//                 break;
//             case MavParamType.MavParamTypeInt8:
//                 if (value is > sbyte.MaxValue or < sbyte.MinValue)
//                 {
//                     error = $"must be {sbyte.MinValue}..{sbyte.MaxValue}";
//                     return false;
//                 }
//                 break;
//             case MavParamType.MavParamTypeUint16:
//                 if (value is > ushort.MaxValue or < ushort.MinValue)
//                 {
//                     error = $"must be {ushort.MinValue}..{ushort.MaxValue}";
//                     return false;
//                 }
//                 break;
//             case MavParamType.MavParamTypeInt16:
//                 if (value is > short.MaxValue or < short.MinValue)
//                 {
//                     error = $"must be {short.MinValue}..{short.MaxValue}";
//                     return false;
//                 }
//                 break;
//             case MavParamType.MavParamTypeUint32:
//                 if (value is > uint.MaxValue or < uint.MinValue)
//                 {
//                     error = $"must be {uint.MinValue}..{uint.MaxValue}";
//                     return false;
//                 }
//                 break;
//             case MavParamType.MavParamTypeInt32:
//                 if (value is > int.MaxValue or < int.MinValue)
//                 {
//                     error = $"must be {int.MinValue}..{int.MaxValue}";
//                     return false;
//                 }
//                 break;
//             case MavParamType.MavParamTypeUint64:
//                 if (value is > ulong.MaxValue or < ulong.MinValue)
//                 {
//                     error = $"must be {ulong.MinValue}..{ulong.MaxValue}";
//                     return false;
//                 }
//                 break;
//             case MavParamType.MavParamTypeReal32:
//                 
//                 break;
//             case MavParamType.MavParamTypeReal64:
//             case MavParamType.MavParamTypeInt64:
//             default:
//                 throw new ArgumentOutOfRangeException();
//         }
//         
//         return true;
//     }
// }

