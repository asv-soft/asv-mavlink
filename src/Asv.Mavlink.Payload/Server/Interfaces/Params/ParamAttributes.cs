using System;

namespace Asv.Mavlink.Payload
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PayloadV2ParamAttribute : Attribute
    {
        public PayloadV2ParamAttribute(Pv2ParamTypeEnum typeEnum, string paramName, ushort paramId, string description)
        {
        }
    }

    [PayloadV2ParamGroup("TestGroup", 0)]
    public class TestGroupParam
    {
        [PayloadV2Param(Pv2ParamTypeEnum.Int, nameof(TestInt), 0, "Params")]
        public uint TestInt { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PayloadV2ParamGroupAttribute : Attribute
    {
        public PayloadV2ParamGroupAttribute(string groupName, ushort groupId)
        {
            GroupName = groupName;
            GroupId = groupId;
        }

        public string GroupName { get; }
        public ushort GroupId { get; }
    }
}
