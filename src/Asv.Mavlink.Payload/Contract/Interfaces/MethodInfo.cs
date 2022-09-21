using System.Collections.Generic;

namespace Asv.Mavlink.Payload
{
    public class MethodInfo<TOut>
    {
        public MethodInfo(string methodName, ushort methodId, string interfaceName, ushort interfaceId)
        {
            InterfaceName = interfaceName;
            MethodName = methodName;
            InterfaceId = interfaceId;
            MethodId = methodId;
            Id = PayloadV2Helper.GetMessageId(InterfaceId, MethodId);
            HeaderByteSize = PayloadV2Helper.GetHeaderByteSize(InterfaceId, MethodId, byte.MaxValue);
            FullName = $"{InterfaceName}.{MethodName}";
            DiagTag = new KeyValuePair<string, object>("METHOD", FullName);
        }

        internal KeyValuePair<string, object> DiagTag { get; }

        public string InterfaceName { get; }
        public string MethodName { get; }
        public ushort InterfaceId { get; }
        public ushort MethodId { get; }

        public uint Id { get; }
        public int HeaderByteSize { get; }
        public string FullName { get; }

        public override string ToString()
        {
            return FullName;
        }
    }

    public class MethodInfo<TIn, TOut> : MethodInfo<TOut>
    {
        public MethodInfo(string methodName, ushort methodId, string interfaceName, ushort interfaceId) : base(
            methodName, methodId, interfaceName, interfaceId)
        {
        }
    }
}
