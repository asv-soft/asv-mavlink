using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink.Payload
{
    public class StreamResult<TOut>
    {
        public bool IsError { get; set; }
        public SpanStringType StringType { get; set; }
        public TOut Value { get; set; }
    }

    public class PayloadV2Client : DisposableOnceWithCancel, IPayloadV2Client
    {
        private static readonly TimeSpan DefaultCommandTimeout = TimeSpan.FromMilliseconds(1000);
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly KeyValuePair<string, object> _diagTagId;
        private readonly byte _networkId;
        private readonly Subject<V2ExtensionPacket> _onData = new();
        private int _inc;
        private uint _requestIdCounter;

        public PayloadV2Client(IMavlinkClient client, byte networkId = 0, bool disposeClient = false)
        {
            Client = client;
            _networkId = networkId;
            _diagTagId = new KeyValuePair<string, object>("ID(SYS,COM,NET)",
                $"{client.Identity.SystemId}:{client.Identity.ComponentId},{networkId}");
            Client.V2Extension.OnData.Where(CheckPacketTarget).Subscribe(_onData).DisposeItWith(Disposable);
            Disposable.Add(System.Reactive.Disposables.Disposable.Create(() =>
            {
                _onData.OnCompleted();
                _onData.Dispose();
            }));
            if (disposeClient)
                Disposable.Add(Client);
        }

        public IMavlinkClient Client { get; }

        public IObservable<StreamResult<TOut>> Subscribe<TOut>(MethodInfo<TOut> method,
            DeserializeDelegate<TOut> deserialize, byte requestId)
        {
            return _onData.Where(_ => FilterMethod(_, method, requestId)).Select(_ =>
            {
                try
                {
                    var span = new ReadOnlySpan<byte>(_.Payload.Payload);
                    PayloadV2Helper.ReadHeader(ref span, out var _, out var _, out var temp);
                    if (_.Payload.MessageType == PayloadV2Helper.DefaultErrorMessageType)
                    {
                        var err = new SpanStringType();
                        Logger.Trace("CLIENT  [{0}]<== {1}() ERROR: {2}", method.FullName, requestId, err);
                        err.Deserialize(ref span);
                        return new StreamResult<TOut>
                        {
                            StringType = err,
                            IsError = true
                        };
                    }

                    if (_.Payload.MessageType == PayloadV2Helper.DefaultResponseSuccessMessageType)
                    {
                        var result = deserialize(ref span);
                        Logger.Trace("CLIENT [{0}]<== {1}() OK: {2}", requestId, method.FullName, result);
                        return new StreamResult<TOut>
                        {
                            Value = result,
                            IsError = false
                        };
                    }

                    return default;
                }
                catch (Exception e)
                {
                    Logger.Trace("CLIENT [{0}]<== {1}() FATAL: {2}", requestId, method.FullName, e.Message);
                    Logger.Warn($"Error to deserialize input data '{method}':{e.Message}");
                    return default;
                }
            }).Where(_ => !Equals(_, default(StreamResult<TOut>)));
        }

        public async Task<TOut> Call<TIn, TOut>(MethodInfo<TIn, TOut> method, TIn args,
            SerializeDelegate<TIn> serialize, SerializeSizeDelegate<TIn> size, DeserializeDelegate<TOut> deserialize,
            CancellationToken cancel = default, int attempt = 3, Action<double> progress = null)
        {
            var currentAttempt = 0;
            while (currentAttempt < attempt)
            {
                ++currentAttempt;
                progress?.Invoke((double)currentAttempt / attempt);
                try
                {
                    using var linkedCancel = CancellationTokenSource.CreateLinkedTokenSource(cancel, DisposeCancel);
                    linkedCancel.CancelAfter(DefaultCommandTimeout);
                    var tcs = new TaskCompletionSource<StreamResult<TOut>>();
                    using var c1 = linkedCancel.Token.Register(() => tcs.TrySetCanceled());
                    var requestId = GenerateRequestId();
                    using var subscribe = Subscribe(method, deserialize, requestId).FirstAsync(_ => true)
                        .Subscribe(_ => tcs.TrySetResult(_));
                    await Send(method, args, serialize, size, requestId, linkedCancel.Token).ConfigureAwait(false);
                    await tcs.Task.ConfigureAwait(false);
                    if (tcs.Task.Result.IsError)
                        throw new InternalPv2Exception(tcs.Task.Result.StringType.Value, method.FullName,
                            Client.Identity.ToString());
                    return tcs.Task.Result.Value;
                }
                catch (TaskCanceledException)
                {
                    Logger.Error($"Timeout to call {method}:{currentAttempt}");
                    if (cancel.IsCancellationRequested) throw;
                }
                catch (Exception e)
                {
                    Logger.Error($"Error to call {method}:{e.Message}");
                    throw;
                }
            }

            throw new TimeoutException(
                $"Timeout to execute command '{method}' {args} with '{attempt}' attempts (timeout {DefaultCommandTimeout.TotalMilliseconds} ms)");
        }

        public async Task Send<TIn, TOut>(MethodInfo<TIn, TOut> method, TIn args, SerializeDelegate<TIn> serialize,
            SerializeSizeDelegate<TIn> size, byte requestId, CancellationToken cancel = default)
        {
            var data = new byte[method.HeaderByteSize + size(args)];
            await Task.Run(() =>
            {
                var span = new Span<byte>(data);
                PayloadV2Helper.WriteHeader(ref span, method.InterfaceId, method.MethodId, requestId);
                serialize(ref span, args);
            }, cancel).ConfigureAwait(false);
            await Client.V2Extension
                .SendData(_networkId, PayloadV2Helper.DefaultRequestSuccessMessageType, data, cancel)
                .ConfigureAwait(false);

            Logger.Trace("CLIENT [{0}]==> {1}({2})", requestId, method.FullName, args);
        }

        public byte GenerateRequestId()
        {
            return (byte)(Interlocked.Increment(ref Unsafe.As<uint, int>(ref _requestIdCounter)) % (byte.MaxValue - 1) +
                          1); // sequence 1 to 254
        }

        private bool CheckPacketTarget(V2ExtensionPacket packet)
        {
            if (packet.Payload.MessageType != PayloadV2Helper.DefaultResponseSuccessMessageType &&
                packet.Payload.MessageType != PayloadV2Helper.DefaultErrorMessageType) return false;
            if ((packet.Payload.TargetNetwork == 0 || _networkId == 0 || packet.Payload.TargetNetwork == _networkId) ==
                false) return false;
            if ((packet.Payload.TargetSystem == 0 || Client.Identity.SystemId == 0 ||
                 packet.Payload.TargetSystem == Client.Identity.SystemId) == false) return false;
            if ((packet.Payload.TargetComponent == 0 || Client.Identity.ComponentId == 0 ||
                 packet.Payload.TargetComponent == Client.Identity.ComponentId) == false) return false;
            return true;
        }

        private bool FilterMethod<TOut>(V2ExtensionPacket v2ExtensionPacket, MethodInfo<TOut> method, byte sequence)
        {
            var span = new ReadOnlySpan<byte>(v2ExtensionPacket.Payload.Payload);
            PayloadV2Helper.ReadHeader(ref span, out var interfaceId, out var methodId, out var seq);
            var meterInterfaceTag = new KeyValuePair<string, object>("IFC", method.InterfaceName);
            var meterMethodTag = new KeyValuePair<string, object>("MSG", method.MethodName);
            return method.InterfaceId == interfaceId && method.MethodId == methodId &&
                   (sequence == 0 || seq == sequence);
        }

       
    }
}
