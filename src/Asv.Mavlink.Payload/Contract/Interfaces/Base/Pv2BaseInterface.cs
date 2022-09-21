using System.Collections.Generic;
using System.Linq;
using Asv.IO;
using Asv.Mavlink.Payload.Digits;

namespace Asv.Mavlink.Payload
{
    public class Pv2BaseInterface
    {
        public const string InterfaceName = "Base";
        public const ushort InterfaceId = 0;

        public static MethodInfo<SpanVoidType, WorkModeListType> GetWorkModeList =
            new(nameof(GetWorkModeList), 0, InterfaceName, InterfaceId);

        public static MethodInfo<SpanByteType, Pv2WorkModeInfo> GetWorkModeInfo =
            new(nameof(GetWorkModeInfo), 1, InterfaceName, InterfaceId);

        public static MethodInfo<SpanByteType, SpanVoidType> SetWorkMode = new(nameof(SetWorkMode), 2, InterfaceName,
            InterfaceId);

        public static MethodInfo<SpanDoubleByteType, Pv2WorkModeStatusInfo> GetModeStatusInfo =
            new(nameof(GetModeStatusInfo), 3, InterfaceName, InterfaceId);

        public static readonly Pv2UIntParamType StartupMode =
            new(nameof(StartupMode), "Payload startup mode", InterfaceName, "{0}", string.Empty, 0, byte.MaxValue);

        public static readonly Pv2StringParamType Name =
            new(nameof(Name), "Payload identification name", InterfaceName, "PayloadV2", 30,
                Pv2ParamFlags.RebootRequired);

        public static IEnumerable<Pv2ParamType> Params
        {
            get
            {
                yield return Name;
                yield return StartupMode;
            }
        }

        public static ushort CombineId(byte modeIndex, byte statusId)
        {
            return (ushort)((modeIndex << 8) | (statusId & 0xF));
        }

        #region Hash calculation

        public static uint CalculateHash(IEnumerable<Pv2WorkModeInfo> workMode, IEnumerable<Pv2WorkModeStatusInfo> records)
        {
            return records.OrderBy(_ => _.Id).CalculateCrc32QHash(workMode.OrderBy(_ => _.Id).CalculateCrc32QHash(0U));
        }


        #endregion
    }
}
