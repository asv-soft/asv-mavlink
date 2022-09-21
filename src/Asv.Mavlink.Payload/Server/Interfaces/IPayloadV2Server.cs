using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Server;

namespace Asv.Mavlink.Payload
{
    public delegate Task<(TOut, DeviceIdentity)> PayloadV2Delegate<in TIn, TOut>(DeviceIdentity devId, TIn data,
        CancellationToken cancel);

    public interface IPayloadV2Server : IDisposable
    {
        IMavlinkServer Server { get; }

        void Register<TIn, TOut>(MethodInfo<TIn, TOut> method, PayloadV2Delegate<TIn, TOut> callback,
            DeserializeDelegate<TIn> deserialize, SerializeDelegate<TOut> serialize, SerializeSizeDelegate<TOut> size);

        Task SendResult<TOut>(DeviceIdentity devId, MethodInfo<TOut> method, TOut msg,
            SerializeDelegate<TOut> serialize,
            SerializeSizeDelegate<TOut> size, byte sequenceId = 0, CancellationToken cancel = default);

        Task SendError<TOut>(DeviceIdentity devId, MethodInfo<TOut> method, string message, byte sequenceId = 0,
            CancellationToken cancel = default);
    }

    public static class PayloadV2ServerHelper
    {
        public static Task SendResult<TIn, TOut>(this IPayloadV2Server src, DeviceIdentity devId,
            MethodInfo<TIn, TOut> method, TOut msg,
            SerializeDelegate<TOut> serialize, SerializeSizeDelegate<TOut> size, byte sequenceId = 0,
            CancellationToken cancel = default)
        {
            return src.SendResult(devId, method, msg, serialize, size, sequenceId, cancel);
        }

        public static void Register<TIn, TOut>(this IPayloadV2Server src, MethodInfo<TIn, TOut> method,
            PayloadV2Delegate<TIn, TOut> callback)
            where TIn : ISizedSpanSerializable, new()
            where TOut : ISizedSpanSerializable, new()
        {
            src.Register(method, callback, SpanSerializableHelper.Deserialize<TIn>, SpanSerializableHelper.Serialize,
                SpanSerializableHelper.SerializeSize);
        }

        public static Task SendResult<TIn, TOut>(this IPayloadV2Server src, DeviceIdentity devId,
            MethodInfo<TIn, TOut> method, TOut msg, byte sequenceId = 0,
            CancellationToken cancel = default)
            where TIn : ISizedSpanSerializable, new()
            where TOut : ISizedSpanSerializable, new()
        {
            return src.SendResult(devId, method, msg, SpanSerializableHelper.Serialize,
                SpanSerializableHelper.SerializeSize, sequenceId, cancel);
        }

        public static Task SendResult<TOut>(this IPayloadV2Server src, DeviceIdentity devId, MethodInfo<TOut> method,
            TOut msg, byte sequenceId = 0, CancellationToken cancel = default)
            where TOut : ISizedSpanSerializable, new()
        {
            return src.SendResult(devId, method, msg, SpanSerializableHelper.Serialize,
                SpanSerializableHelper.SerializeSize, sequenceId, cancel);
        }

        public static Task SendError<TIn, TOut>(this IPayloadV2Server src, DeviceIdentity devId,
            MethodInfo<TIn, TOut> method, string message,
            byte sequenceId = 0, CancellationToken cancel = default)
        {
            return src.SendError(devId, method, message, sequenceId, cancel);
        }
    }
}
