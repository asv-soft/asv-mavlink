using System;
using Asv.Cfg.ImMemory;
using Asv.Common;
using Asv.Mavlink.Payload.Digits;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Payload.Test
{
    public class Pv2ParamsTypeValueValidation
    {
        private readonly ITestOutputHelper _output;

        public Pv2ParamsTypeValueValidation(ITestOutputHelper output)
        {
            _output = output;
        }

        private static void ValidateParamType<TValue, TType, TParamValue>(TType type, Action<string> output = null)
            where TType : Pv2ParamType<TValue, TParamValue>, new() where TParamValue : Pv2ParamValue
        {
            type.ValidateSize();
            SpanTestHelper.TestType(type, output);
        }

        private static void ValidateParamValue<TValue, TType, TParamValue>(TType type, TValue value, Action<string> output = null)
            where TType : Pv2ParamType<TValue, TParamValue>, new()
            where TParamValue : Pv2ParamValue, new()
        {
            var config = new InMemoryConfiguration();
            var val = type.CreateValue();
            val.Index = (uint)new Random().Next(0, 999);
            var realValue = value;
            type.SetValue(val, realValue);
            type.ValidateValue(val);
            var toString = $"{value, 5} => {type.ConvertToString(val)}";
            type.WriteToConfig(config,"TEST", val);
            Assert.Equal(realValue, type.GetValue(type.ReadFromConfig(config, "TEST")));
            Assert.Equal(realValue, type.GetValue(val));
            SpanTestHelper.TestType((TParamValue)val, output, toString);
        }



        [Theory]
        [InlineData("MinMax", "Description 11", "Gr1", "{0:D1}", "12345", uint.MinValue, uint.MaxValue, 0, Pv2ParamFlags.RebootRequired | Pv2ParamFlags.ForAdvancedUsers)]
        [InlineData("Param1", "", "Gr1", "{0:D1}", "12345", 0, 100, 50, Pv2ParamFlags.RebootRequired | Pv2ParamFlags.ForAdvancedUsers)]
        [InlineData("Param1", "Description 11", "Gr1", "{0:D1}", "12345", 300, 400, 350, Pv2ParamFlags.NoFlags)]
        public void TestPv2UIntParamType(string paramName, string description, string Gr1, string formatString, string units, uint min = uint.MinValue, uint max = uint.MaxValue, uint defaultValue = 0, Pv2ParamFlags flags = Pv2ParamFlags.NoFlags)
        {
            SpanTestHelper.SerializeDeserializeTestBegin(_output.WriteLine);
            var random = new Random();
            var type = new Pv2UIntParamType(paramName, description, Gr1, formatString, units, min, max, defaultValue, flags);
            ValidateParamType<uint, Pv2UIntParamType,Pv2UIntParamValue>(type, _output.WriteLine);

            ValidateParamValue<uint, Pv2UIntParamType, Pv2UIntParamValue>(type, type.Max,_output.WriteLine);
            ValidateParamValue<uint, Pv2UIntParamType, Pv2UIntParamValue>(type, type.Min, _output.WriteLine);
            ValidateParamValue<uint, Pv2UIntParamType, Pv2UIntParamValue>(type, type.DefaultValue, _output.WriteLine);

            for (var i = 0; i < 10; i++)
            {
                var realValue = random.Next((int)(type.Min + int.MinValue), (int)(type.Max - int.MaxValue)) + int.MinValue;
                ValidateParamValue<uint, Pv2UIntParamType, Pv2UIntParamValue>(type, (uint)realValue, _output.WriteLine);
            }
        }



        [Theory]
        [InlineData("Param1", "Description 11", "Gr1", "{0:D1}", "12345", -100, 100, 0, Pv2ParamFlags.RebootRequired | Pv2ParamFlags.ForAdvancedUsers)]
        [InlineData("Param1", "Description 11", "Gr1", "{0:D1}", "12345", 300, 400, 350, Pv2ParamFlags.NoFlags)]
        [InlineData("Param1", "Description 11", "Gr1", "{0:D1}", "12345", -1000, -10, -500, Pv2ParamFlags.NoFlags)]
        [InlineData("Param1", "Description 11", "Gr1", "{0:D1}", "12345", -1000, 1000, 100, Pv2ParamFlags.NoFlags)]
        public void TestPv2IntParamType(string paramName, string description, string Gr1, string formatString, string units, int min = int.MinValue, int max = int.MaxValue, int defaultValue = 0, Pv2ParamFlags flags = Pv2ParamFlags.NoFlags)
        {
            SpanTestHelper.SerializeDeserializeTestBegin(_output.WriteLine);
            var random = new Random();
            var type = new Pv2IntParamType(paramName, description, Gr1, formatString, units, min, max, defaultValue, flags);
            ValidateParamType<int, Pv2IntParamType, Pv2IntParamValue>(type, _output.WriteLine);

            ValidateParamValue<int, Pv2IntParamType, Pv2IntParamValue>(type, type.Max, _output.WriteLine);
            ValidateParamValue<int, Pv2IntParamType, Pv2IntParamValue>(type, type.Min, _output.WriteLine);
            ValidateParamValue<int, Pv2IntParamType, Pv2IntParamValue>(type, type.DefaultValue, _output.WriteLine);

            for (var i = 0; i < 10; i++)
            {
                var realValue = random.Next(type.Min,type.Max);
                ValidateParamValue<int, Pv2IntParamType, Pv2IntParamValue>(type, realValue, _output.WriteLine);
            }
        }

        [Theory]
        [InlineData("Param1", "Description 11", "Gr1", "{0:F2}", "12345", -100, 100, 0, Pv2ParamFlags.RebootRequired | Pv2ParamFlags.ForAdvancedUsers)]
        [InlineData("Param1", "Description 11", "Gr1", "{0:F2}", "12345", 300, 400, 350, Pv2ParamFlags.NoFlags)]
        [InlineData("Param1", "Description 11", "Gr1", "{0:F2}", "12345", -1000, -10, -500, Pv2ParamFlags.NoFlags)]
        [InlineData("Param1", "Description 11", "Gr1", "{0:F2}", "12345", -1000, 1000, 100, Pv2ParamFlags.NoFlags)]
        public void TestPv2DoubleParamType(string paramName, string description, string Gr1, string formatString, string units, double min , double max , double defaultValue, Pv2ParamFlags flags = Pv2ParamFlags.NoFlags)
        {
            SpanTestHelper.SerializeDeserializeTestBegin(_output.WriteLine);
            var random = new Random();
            var type = new Pv2DoubleParamType(paramName, description, Gr1, formatString, units, min, max, defaultValue, flags);
            ValidateParamType<double, Pv2DoubleParamType, Pv2DoubleParamValue>(type, _output.WriteLine);

            ValidateParamValue<double, Pv2DoubleParamType, Pv2DoubleParamValue>(type, type.Max, _output.WriteLine);
            ValidateParamValue<double, Pv2DoubleParamType, Pv2DoubleParamValue>(type, type.Min, _output.WriteLine);
            ValidateParamValue<double, Pv2DoubleParamType, Pv2DoubleParamValue>(type, type.DefaultValue, _output.WriteLine);

            for (var i = 0; i < 10; i++)
            {
                var realValue = type.Min + Math.Abs(type.Max - type.Min) * random.NextDouble();
                ValidateParamValue<double, Pv2DoubleParamType, Pv2DoubleParamValue>(type, realValue, _output.WriteLine);
            }
        }

        [Theory]
        [InlineData("Param1", "Description 11", "Gr1", "{0:F2}", "12345", -100, 100, 0, Pv2ParamFlags.RebootRequired | Pv2ParamFlags.ForAdvancedUsers)]
        [InlineData("Param1", "Description 11", "Gr1", "{0:F2}", "12345", 300, 400, 350, Pv2ParamFlags.NoFlags)]
        [InlineData("Param1", "Description 11", "Gr1", "{0:F2}", "12345", -1000, -10, -500, Pv2ParamFlags.NoFlags)]
        [InlineData("Param1", "Description 11", "Gr1", "{0:F2}", "12345", -1000, 1000, 100, Pv2ParamFlags.NoFlags)]
        public void TestPv2FloatParamType(string paramName, string description, string Gr1, string formatString, string units, float min, float max, float defaultValue, Pv2ParamFlags flags = Pv2ParamFlags.NoFlags)
        {
            SpanTestHelper.SerializeDeserializeTestBegin(_output.WriteLine);
            var random = new Random();
            var type = new Pv2FloatParamType(paramName, description, Gr1, formatString, units, min, max, defaultValue, flags);
            ValidateParamType<float, Pv2FloatParamType, Pv2FloatParamValue>(type, _output.WriteLine);

            ValidateParamValue<float, Pv2FloatParamType, Pv2FloatParamValue>(type, type.Max, _output.WriteLine);
            ValidateParamValue<float, Pv2FloatParamType, Pv2FloatParamValue>(type, type.Min, _output.WriteLine);
            ValidateParamValue<float, Pv2FloatParamType, Pv2FloatParamValue>(type, type.DefaultValue, _output.WriteLine);

            for (var i = 0; i < 10; i++)
            {
                var realValue = type.Min + Math.Abs(type.Max - type.Min) * random.NextDouble();
                ValidateParamValue<float, Pv2FloatParamType, Pv2FloatParamValue>(type, (float)realValue, _output.WriteLine);
            }
        }

        [Theory]
        [InlineData("Param1", "Description 11", "Gr1","asdas",  100, Pv2ParamFlags.RebootRequired | Pv2ParamFlags.ForAdvancedUsers)]
        [InlineData("Param1", "Description 11", "Gr1", "", 200, Pv2ParamFlags.NoFlags)]
        [InlineData("Param1", "Description 11", "Gr1", "asdas", 50, Pv2ParamFlags.NoFlags)]
        public void TestPv2StringParamType(string paramName, string description, string @group, string defaultValue, uint maxLength, Pv2ParamFlags flags = Pv2ParamFlags.NoFlags)
        {
            SpanTestHelper.SerializeDeserializeTestBegin(_output.WriteLine);
            var random = new Random();
            var type = new Pv2StringParamType(paramName, description, @group, defaultValue, maxLength, flags);
            ValidateParamType<string, Pv2StringParamType, Pv2StringParamValue>(type, _output.WriteLine);

            ValidateParamValue<string, Pv2StringParamType, Pv2StringParamValue>(type, string.Empty.LeftMargin((int)type.MaxLength), _output.WriteLine);
            ValidateParamValue<string, Pv2StringParamType, Pv2StringParamValue>(type, string.Empty, _output.WriteLine);
            ValidateParamValue<string, Pv2StringParamType, Pv2StringParamValue>(type, type.DefaultValue, _output.WriteLine);

            for (var i = 0; i < 10; i++)
            {
                var realValue = random.Next().ToString();
                ValidateParamValue<string, Pv2StringParamType, Pv2StringParamValue>(type, realValue, _output.WriteLine);
            }
        }

        [Theory]
        [InlineData("Param1", "Description 11", "Gr1", "item1", Pv2ParamFlags.RebootRequired | Pv2ParamFlags.ForAdvancedUsers, new[] {"item1"})]
        [InlineData("Param1", "Description 11", "Gr1", "item2", Pv2ParamFlags.RebootRequired | Pv2ParamFlags.ForAdvancedUsers, new[] { "item1", "item2" })]
        [InlineData("Param1", "Description 11", "Gr1", "item3", Pv2ParamFlags.RebootRequired | Pv2ParamFlags.ForAdvancedUsers, new[] { "item1", "item2", "item3" })]
        [InlineData("Param1", "Description 11", "Gr1", "item4", Pv2ParamFlags.RebootRequired | Pv2ParamFlags.ForAdvancedUsers, new[] { "item1", "item2", "item3", "item4" })]
        public void TestPv2EnumParamValue(string paramName, string description, string @group, string defaultValue, Pv2ParamFlags flags, string[] items)
        {
            SpanTestHelper.SerializeDeserializeTestBegin(_output.WriteLine);
            var type = new Pv2EnumParamType(paramName, description, @group, defaultValue, flags, items);
            ValidateParamType<string, Pv2EnumParamType, Pv2EnumParamValue>(type, _output.WriteLine);
            ValidateParamValue<string, Pv2EnumParamType, Pv2EnumParamValue>(type,type.DefaultValue , _output.WriteLine);
            ValidateParamValue<string, Pv2EnumParamType, Pv2EnumParamValue>(type, "item1", _output.WriteLine);
        }

        [Theory]
        [InlineData("Param1", "Description 11", "Gr1", true, "on","off", Pv2ParamFlags.RebootRequired | Pv2ParamFlags.ForAdvancedUsers)]
        [InlineData("Param1", "Description 11", "Gr1", false, "yes","no",Pv2ParamFlags.RebootRequired | Pv2ParamFlags.ForAdvancedUsers)]
        public void TestPv2BoolParamType(string paramName, string description, string @group, bool defaultValue, string trueString, string falseString, Pv2ParamFlags flags)
        {
            SpanTestHelper.SerializeDeserializeTestBegin(_output.WriteLine);
            var type = new Pv2BoolParamType(paramName, description, @group, defaultValue, trueString,falseString, flags);
            ValidateParamType<bool, Pv2BoolParamType, Pv2BoolParamValue>(type, _output.WriteLine);
            ValidateParamValue<bool, Pv2BoolParamType, Pv2BoolParamValue>(type, true, _output.WriteLine);
            ValidateParamValue<bool, Pv2BoolParamType, Pv2BoolParamValue>(type, false, _output.WriteLine);
        }

        [Theory]
        [InlineData("Param1", "Description 11", "Gr1", "on", "off", Pv2ParamFlags.RebootRequired)]
        public void TestPv2FlagsParamType(string paramName, string description, string groupName, string trueTitle, string falseTitle, Pv2ParamFlags flags)
        {
            SpanTestHelper.SerializeDeserializeTestBegin(_output.WriteLine);
            var size = new Random().Next(5, 10);
            var items = new (string, bool)[size];
            for (int i = 0; i < size; i++)
            {
                items[i] = ("flag"+i, i%3 == 0);
            }
            var type = new Pv2FlagsParamType(paramName, description, groupName, trueTitle, falseTitle, flags, items);
            ValidateParamType<UintBitArray, Pv2FlagsParamType, Pv2FlagsParamValue>(type, _output.WriteLine);

            ValidateParamValue<UintBitArray, Pv2FlagsParamType, Pv2FlagsParamValue>(type, type.DefaultValue, _output.WriteLine);
            
            for (int i = 0; i < size; i++)
            {
                var value = type.GetValue(type.CreateValue());
                value[i] = true;
                ValidateParamValue<UintBitArray, Pv2FlagsParamType, Pv2FlagsParamValue>(type, value, _output.WriteLine);
            }
            
            
        }
    }
}
