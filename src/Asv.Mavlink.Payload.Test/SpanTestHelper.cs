using System;
using Asv.IO;
using DeepEqual.Syntax;
using Xunit;

namespace Asv.Mavlink.Payload.Test
{
    public class SpanTestHelper
    {

        public static void SerializeDeserializeTestBegin(Action<string> output = null)
        {
            output?.Invoke($"{"#",-4} | {"NAME",-25} | {"VALUE",-50} | {"SIZE",-4} | COMMENT ");
            output?.Invoke($"----------------------------------------------------------------------------------------------------------------");
        }

        public static void TestType<T>(T type, Action<string> output = null, string comment = null)
            where T : ISizedSpanSerializable, new()
        {
            TestType(type,()=>new T(),output,comment);
        }

        public static void TestType<T>(T type, Func<T> typeFactory, Action<string> output = null, string comment = null)
            where T : ISizedSpanSerializable
        {
            var arr = new byte[type.GetByteSize()];
            var span = new Span<byte>(arr);
            type.Serialize(ref span);
            Assert.Equal(0, span.Length);

            var compare = typeFactory();
            var readSpan = new ReadOnlySpan<byte>(arr, 0, type.GetByteSize());
            compare.Deserialize(ref readSpan);
            Assert.Equal(0, readSpan.Length);
            var result = type.WithDeepEqual(compare).Compare();
            output?.Invoke(
                $"{(result ? "OK" : "ERR"),-4} | {type.GetType().Name,-25} | {type.ToString().Substring(0,50),-50} | {type.GetByteSize(),-4} | {comment ?? string.Empty}");
            type.WithDeepEqual(compare).Assert();
        }

    }
}
