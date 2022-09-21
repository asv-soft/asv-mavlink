using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public interface IPayloadV2Client : IDisposable
    {
        IMavlinkClient Client { get; }

        byte GenerateRequestId();

        IObservable<StreamResult<TOut>> Subscribe<TOut>(MethodInfo<TOut> method, DeserializeDelegate<TOut> deserialize,
            byte requestId = 0);

        Task<TOut> Call<TIn, TOut>(MethodInfo<TIn, TOut> method, TIn args, SerializeDelegate<TIn> serialize,
            SerializeSizeDelegate<TIn> size, DeserializeDelegate<TOut> deserialize, CancellationToken cancel = default,
            int attempt = 3, Action<double> progress = null);

        Task Send<TIn, TOut>(MethodInfo<TIn, TOut> method, TIn args, SerializeDelegate<TIn> serialize,
            SerializeSizeDelegate<TIn> size, byte requestId, CancellationToken cancel = default);
    }

    public static class PayloadV2ClientHelper
    {
        public static IObservable<StreamResult<TOut>> Subscribe<TOut>(this IPayloadV2Client src,
            MethodInfo<TOut> method)
            where TOut : ISizedSpanSerializable, new()
        {
            return src.Subscribe(method, SpanSerializableHelper.Deserialize<TOut>);
        }

        public static IObservable<StreamResult<TOut>> Subscribe<TIn, TOut>(this IPayloadV2Client src,
            MethodInfo<TIn, TOut> method, byte requestId = 0)
            where TOut : ISizedSpanSerializable, new()
            where TIn : ISizedSpanSerializable, new()
        {
            return src.Subscribe(method, SpanSerializableHelper.Deserialize<TOut>, requestId);
        }

        public static Task<TOut> Call<TIn, TOut>(this IPayloadV2Client src, MethodInfo<TIn, TOut> method, TIn args,
            CancellationToken cancel = default, int attempt = 3,
            Action<double> progress = null)
            where TIn : ISizedSpanSerializable, new()
            where TOut : ISizedSpanSerializable, new()
        {
            return src.Call(method, args, SpanSerializableHelper.Serialize, SpanSerializableHelper.SerializeSize,
                SpanSerializableHelper.Deserialize<TOut>, cancel, attempt, progress);
        }

        public static async Task<(TIn, TOut)> CallWithArgs<TIn, TOut>(this IPayloadV2Client src,
            MethodInfo<TIn, TOut> method, TIn args,
            CancellationToken cancel = default, int attempt = 3,
            Action<double> progress = null)
            where TIn : ISizedSpanSerializable, new()
            where TOut : ISizedSpanSerializable, new()
        {
            var result = await src.Call(method, args, SpanSerializableHelper.Serialize,
                SpanSerializableHelper.SerializeSize, SpanSerializableHelper.Deserialize<TOut>, cancel, attempt,
                progress);
            return (args, result);
        }

        public static Task Send<TIn, TOut>(this IPayloadV2Client src, MethodInfo<TIn, TOut> method, TIn args,
            byte requestId = 0, CancellationToken cancel = default)
            where TIn : ISizedSpanSerializable, new()
            where TOut : ISizedSpanSerializable, new()
        {
            return src.Send(method, args, SpanSerializableHelper.Serialize, SpanSerializableHelper.SerializeSize,
                requestId, cancel);
        }
    }
}
