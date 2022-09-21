using Asv.Cfg;

namespace Asv.Mavlink.Payload
{
    public class Pv2ParamTypeUnknown : Pv2ParamType
    {
        public Pv2ParamTypeUnknown(string paramName, string description, string groupName,
            Pv2ParamFlags flags = Pv2ParamFlags.NoFlags) : base(paramName, description, groupName, flags)
        {
        }

        public override Pv2ParamTypeEnum TypeEnum => Pv2ParamTypeEnum.Unknown;

        protected override void InternalReadFromConfig(IConfiguration config, string configSuffix, Pv2ParamValue value)
        {
        }

        protected override void InternalWriteToConfig(IConfiguration config, string configSuffix, Pv2ParamValue value)
        {
        }

        public override void ValidateValue(Pv2ParamValue data)
        {
        }

        public override string ConvertToString(Pv2ParamValue value)
        {
            return "UNKNOWN";
        }
    }

    public class Pv2ParamValueUnknown : Pv2ParamValue
    {
        public override Pv2ParamTypeEnum Type => Pv2ParamTypeEnum.Unknown;

        public override void CopyFrom(Pv2ParamValue data)
        {
        }
    }
}
