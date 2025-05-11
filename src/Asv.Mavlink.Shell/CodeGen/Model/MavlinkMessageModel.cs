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
            // Message payload fields are reordered for transmission as follows:
            // 
            // Fields are sorted according to their native data size:
            // (u)int64_t, double (8 bytes)
            // (u)int32_t, float (4)
            // (u)int16_t (2)
            // (u)int8_t, char (1)
            // If two fields have the same length, their order is preserved as it was present before the data field size ordering
            // Arrays are handled based on the data type they use, not based on the total array size
            // The over-the-air order is the same as for the struct and thus represents the reordered fields

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
