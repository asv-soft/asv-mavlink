using System.Collections.Generic;
using System.Linq;
using Asv.Common;

namespace Asv.Mavlink.Shell
{
    public class MavlinkMessageModel: MavlinkModelBase
    {
        public int Id { get; set; }
        public byte CrcExtra { get; set; }
        public string? Name { get; set; }
        public IList<MessageFieldModel> Fields { get; set; } = new List<MessageFieldModel>();
        public IList<MessageFieldModel> ExtendedFields { get; set; } = new List<MessageFieldModel>();

        public bool HasArrayFields => Fields.Any(_ => _.IsArray) || ExtendedFields.Any(_=>_.IsArray);
        public bool WrapToV2Extension { get; set; }

        public override string ToString()
        {
            return $"{Name}[Id:{Id},CrcExtra:{CrcExtra}]";
        }
    }

    public static class MavlinkMessageModelEx
    {
        public static IEnumerable<MessageFieldModel> GetAllFields(this MavlinkMessageModel src)
        {
            return src.Fields.Concat(src.ExtendedFields);
        }
        
        public static void ReorderFieldsAndClacCrc(this MavlinkMessageModel src)
        {
            src.Fields = src.Fields.OrderByDescending(_ => _.FieldTypeByteSize).ToList();

            var crc = X25Crc.Accumulate($"{src.Name} ", X25Crc.CrcSeed);
            crc = src.Fields.Aggregate(crc, (acc, field) => field.CalculateCrc(acc));
            src.CrcExtra = (byte)((crc & 0xFF) ^ (crc >> 8));
        }

        /// <summary>
        /// It's for Variable Length Arrays calculation
        /// </summary>
        /// <param name="src"></param>
        public static void CalculateLargestArray(this MavlinkMessageModel src)
        {
            var largestArray = src.Fields.Where(_ => _.IsArray).OrderByDescending(_ => _.FieldByteSize).FirstOrDefault();
            if (largestArray != null) largestArray.IsTheLargestArrayInMessage = true;
        }

        public static ushort CalculateCrc(this MessageFieldModel field, ushort crc)
        {
            crc = X25Crc.Accumulate($"{field.TypeName} {field.Name} ", crc);
            if (field.IsArray)
            {
                crc = X25Crc.Accumulate(field.ArrayLength, crc);
            }
            return crc;
        }
    }
}
