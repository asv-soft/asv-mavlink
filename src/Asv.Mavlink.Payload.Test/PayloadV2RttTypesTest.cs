using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Payload.Test
{
    public class PayloadV2RttTypesTest
    {
        private readonly ITestOutputHelper _output;

        public PayloadV2RttTypesTest(ITestOutputHelper output)
        {
            _output = output;
        }

        public static Pv2RttRecordDesc Group = new(0,"Group1","Desc group", Pv2RttRecordFlags.NoFlags);

        [Fact]
        public void TestBitSize()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new Pv2RttFixedPointDesc(0,"param","desc","item","{0:F5}", Pv2RttFieldFlags.Indexed, 0,0,1,0,Group);
            });
        }

        [Fact]
        public void RttTypes()
        {
            var r = new Random();
            SpanTestHelper.SerializeDeserializeTestBegin(_output.WriteLine);
            SpanTestHelper.TestType(new Pv2RttGetFieldsArgs { FieldIndex = 255, SessionId = new SessionId(Guid.NewGuid()) }, _output.WriteLine);
            SpanTestHelper.TestType(new Pv2RttGetFieldsDataArgs { SessionId =new SessionId(Guid.NewGuid()), FieldId = UInt32.MaxValue, StartIndex = UInt32.MaxValue,Take = UInt32.MinValue}, _output.WriteLine);
            var data = new byte[Pv2RttInterface.MaxOnStreamDataSize];
            r.NextBytes(data);
            SpanTestHelper.TestType(new Pv2RttGetFieldsDataResult { Count = uint.MaxValue, Data = data }, _output.WriteLine);
        }

        [Fact]
        public void TestFixedPointTypeValue2()
        {
            var fractions = new[]{10,1,0.1,0.01};
            var offsets = new[] {-100,-10.5,0,9.4,1000 };
            var counter = 0;
            var maxCount = 100;
            foreach (var fraction in fractions)
            {
                foreach (var offset in offsets)
                {
                    foreach (var bitSizeEnum in Enum.GetValues(typeof(BitSizeEnum)).Cast<BitSizeEnum>())
                    {
                        var type = new Pv2RttFixedPointDesc(0, "param1", "desc1", "ads", "{0:F2}",
                            Pv2RttFieldFlags.NoFlags, bitSizeEnum, offset, fraction, offset, Group);
                        var count = 0;
                        for (var value = type.MinValue; value <= type.MaxValue; value+=type.Fraction)
                        {
                            var data = new byte[type.GetValueMaxByteSize()];
                            var span = new Span<byte>(data);
                            var bitIndex = 0;
                            type.SerializeValue(span, value, ref bitIndex);
                            var span2 = new ReadOnlySpan<byte>(data);
                            bitIndex = 0;
                            var value2 = type.DeserializeValue(span2, ref bitIndex);
                            if (Math.Abs((double)value2 - value) > type.Fraction)
                            {
                                Assert.True(false,$"{value2} != {value}");
                            }
                            
                            counter++;
                            if (counter % 1000 == 0) _output.WriteLine($"Count: {counter} value= {value}");
                            if (++count > maxCount) break;
                        }
                    }
                }
            }
            _output.WriteLine($"Total check {counter} ");

        }

        [Theory]
        [InlineData(1,1, BitSizeEnum.S03Bits,0,1,0,1)]
        [InlineData(1, 1, BitSizeEnum.S04Bits, 0, 1, 0, 2)]
        [InlineData(1, 10, BitSizeEnum.S05Bits, 0, 1, 0, 3)]
        [InlineData(10, 1, BitSizeEnum.S06Bits, 0, 1, 0, 4)]
        [InlineData(5, 5, BitSizeEnum.S07Bits, 0, 1, 0, 5)]
        [InlineData(1, 1, BitSizeEnum.S08Bits, 250, 1, 250, 370)]
        [InlineData(1, 1, BitSizeEnum.S09Bits, 25.0, 0.1, 25.1, 37.5)]
        public void TestFixedPointTypeValue(int groupCount,int fieldsInGroupCount, BitSizeEnum bitSize, double offset,double fraction, double defaultValue, double value)
        {
            var recs = new List<Pv2RttRecordDesc>();
            var fields = new List<Pv2RttFixedPointDesc>();
            for (int i = 0; i < groupCount; i++)
            {
                var group = new Pv2RttRecordDesc((ushort)i, "Group" + i, "Group desc" + i, Pv2RttRecordFlags.NoFlags);
                recs.Add(group);
                
                for (int j = 0; j < fieldsInGroupCount; j++)
                {
                    fields.Add(new Pv2RttFixedPointDesc((byte)j,"Param"+j,"Desc para"+j,"it"+j,"{0:F1}", Pv2RttFieldFlags.Indexed, bitSize, offset, fraction, defaultValue,group));
                }
            }

            SpanTestHelper.SerializeDeserializeTestBegin(_output.WriteLine);
            foreach (var rec in recs)
            {
                SpanTestHelper.TestType(rec, _output.WriteLine);
            }
            foreach (var field in fields)
            {
                TestFieldType(field, field.DefaultValue);
                TestFieldType(field, value);
            }
        }

        private void TestFieldType<T>(T field, object value)
            where T:Pv2RttFieldDesc,new()
        {
            
            var data = new byte[field.GetValueMaxByteSize()];
            var span = new Span<byte>(data);
            var bitIndex = 0;
            field.SerializeValue(span, value, ref bitIndex);
            Assert.Equal(field.GetValueBitSize(value), bitIndex);

            var span2 = new ReadOnlySpan<byte>(data);
            bitIndex = 0;
            var value2 = field.DeserializeValue(span2, ref bitIndex);
            Assert.Equal(field.GetValueBitSize(value), bitIndex);

            Assert.Equal(value,value2);

            SpanTestHelper.TestType<T>(field, _output.WriteLine, $"{value}=>{field.ConvertToString(value2)} {bitIndex} bit");

        }
    }
}
