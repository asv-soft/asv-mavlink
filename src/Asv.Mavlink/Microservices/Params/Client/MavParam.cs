using System;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Client
{
    public class MavParam:ICloneable
    {
        public MavParam(ushort index, string name, MavParamType type, float? realValue, long? integerValue)
        {
            Index = index;
            Name = name;
            Type = type;
            RealValue = realValue;
            IntegerValue = integerValue;
        }

        public MavParam(MavParam param)
        {
            Index = param.Index;
            Name = param.Name;
            Type = param.Type;
            RealValue = param.RealValue;
            IntegerValue = param.IntegerValue;
        }

        public MavParam(MavParam param, float newValue)
        {
            Index = param.Index;
            Name = param.Name;
            Type = param.Type;
            RealValue = newValue;
            IntegerValue = null;
        }

        public MavParam(MavParam param, long newValue)
        {
            Index = param.Index;
            Name = param.Name;
            Type = param.Type;
            RealValue = null;
            IntegerValue = newValue;
        }

        public ushort Index { get; private set; }
        public string Name { get; private set; }
        public MavParamType Type { get; private set; }
        public float? RealValue { get; set; }
        public long? IntegerValue { get; set; }

        public override string ToString()
        {
            return $"MAV_PARAM[index:{Index},name:{Name},type:{Type:G},realValue:{RealValue},intValue:{IntegerValue}]";
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}