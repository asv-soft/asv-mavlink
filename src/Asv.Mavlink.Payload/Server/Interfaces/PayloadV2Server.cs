using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Server;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink.Payload
{
    public class PayloadV2Server : DisposableOnceWithCancel, IPayloadV2Server
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static bool WritePacketsToLogger = false;
        private readonly Dictionary<uint, Action<DeviceIdentity, byte[]>> _dataCallbacks = new();
        private readonly byte _networkId;


        public PayloadV2Server(IMavlinkServer server, byte networkId = 0, bool disposeServer = false)
        {
            Server = server;
            _networkId = networkId;
            server.V2Extension.OnData.Where(CheckPacketTarget).Subscribe(OnData).DisposeItWith(Disposable);
            if (disposeServer)
                Disposable.Add(server);
        }

        public IMavlinkServer Server { get; }

        public void Register<TIn, TOut>(MethodInfo<TIn, TOut> method, PayloadV2Delegate<TIn, TOut> callback,
            DeserializeDelegate<TIn> deserialize,
            SerializeDelegate<TOut> serialize, SerializeSizeDelegate<TOut> size)
        {
            var id = method.Id;
            if (_dataCallbacks.ContainsKey(id))
                throw new Exception(
                    $"PayloadV2 callback for method '{method} already registered");

            async void DataCallback(DeviceIdentity devId, byte[] data)
            {
                TIn messageIn = default;
                byte seq = 0;
                try
                {
                    await Task.Run(() =>
                        {
                            var span = new ReadOnlySpan<byte>(data);
                            PayloadV2Helper.ReadHeader(ref span, out var ifcId, out var mthdId, out seq);
                            messageIn = deserialize(ref span);

                            if (WritePacketsToLogger)
                                Logger.Trace("SERVER [req_id:{0}]<= {1}({2})", seq, method.FullName, messageIn);
                        }, DisposeCancel)
                        .ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Logger.Error($"Deserialization input error for {method}");
                    await SendError(devId, method, e.Message, seq, DisposeCancel).ConfigureAwait(false);
                }

                try
                {
                    var messageOut = await callback(devId, messageIn, DisposeCancel).ConfigureAwait(false);
                    await SendResult(messageOut.Item2, method, messageOut.Item1, serialize, size, seq, DisposeCancel)
                        .ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Server.StatusText.Error($"Error {method.InterfaceName}.{method.MethodName}:{e.Message}");
                    Logger.Warn($"Error to execute '{method}':{e.Message}");
                    await SendError(devId, method, e.Message, seq, DisposeCancel).ConfigureAwait(false);
                }
            }

            _dataCallbacks[id] = DataCallback;
        }

        public async Task SendResult<TOut>(DeviceIdentity devId, MethodInfo<TOut> method, TOut msg,
            SerializeDelegate<TOut> serialize,
            SerializeSizeDelegate<TOut> size, byte sequenceId = 0, CancellationToken cancel = default)
        {
            var data = new byte[method.HeaderByteSize + size(msg)];
            await Task.Run(() =>
            {
                var span = new Span<byte>(data);
                PayloadV2Helper.WriteHeader(ref span, method.InterfaceId, method.MethodId, sequenceId);
                serialize(ref span, msg);
            }, cancel).ConfigureAwait(false);
            await Server.V2Extension.SendData(devId.SystemId, devId.ComponentId, _networkId,
                PayloadV2Helper.DefaultResponseSuccessMessageType, data, cancel).ConfigureAwait(false);

            if (WritePacketsToLogger)
                Logger.Trace("SERVER [req_id:{0}]=> OK {1}({2})", sequenceId, method.FullName, msg);
        }

        public async Task SendError<TOut>(DeviceIdentity devId, MethodInfo<TOut> method, string message,
            byte sequenceId = 0,
            CancellationToken cancel = default)
        {
            try
            {
                var msg = new SpanStringType
                {
                    Value = message.TrimToMaxLength(PayloadV2Helper.MaxErrorMessageStringCharSize)
                };
                var data = new byte[method.HeaderByteSize + msg.GetByteSize()];
                await Task.Run(() =>
                {
                    var span = new Span<byte>(data);
                    PayloadV2Helper.WriteHeader(ref span, method.InterfaceId, method.MethodId, sequenceId);
                    msg.Serialize(ref span);
                }, cancel).ConfigureAwait(false);
                await Server.V2Extension.SendData(devId.SystemId, devId.ComponentId, _networkId,
                    PayloadV2Helper.DefaultErrorMessageType, data, cancel).ConfigureAwait(false);
                Logger.Trace("SERVER [{0}]==> {1}() ERR:{2}", sequenceId, method.FullName, message);
                if (WritePacketsToLogger)
                    Logger.Trace("SERVER [{0}]==> ERR {1}({2})", sequenceId, method.FullName, message);
            }
            catch (Exception e)
            {
                Logger.Error($"Exception occured to send error. Message:{message}:{e.Message}");
            }
        }

        private void OnData(V2ExtensionPacket v2ExtensionPacket)
        {
            try
            {
                var span = new ReadOnlySpan<byte>(v2ExtensionPacket.Payload.Payload);
                PayloadV2Helper.ReadHeader(ref span, out var ifcId, out var methodId, out var seq);
                var messageId = PayloadV2Helper.GetMessageId(ifcId, methodId);
                if (_dataCallbacks.TryGetValue(messageId, out var callback))
                    callback(
                        new DeviceIdentity
                            { ComponentId = v2ExtensionPacket.ComponenId, SystemId = v2ExtensionPacket.SystemId },
                        v2ExtensionPacket.Payload.Payload);
                else
                    Debug.Assert(false, "Unknown message");
            }
            catch (Exception)
            {
                Logger.Error("Fatal error to execute callback");
                Debug.Assert(false, "Error to execute callback");
            }
        }

        private bool CheckPacketTarget(V2ExtensionPacket packet)
        {
            if (packet.Payload.MessageType != PayloadV2Helper.DefaultRequestSuccessMessageType &&
                packet.Payload.MessageType != PayloadV2Helper.DefaultErrorMessageType) return false;
            if ((packet.Payload.TargetNetwork == 0 || _networkId == 0 || packet.Payload.TargetNetwork == _networkId) ==
                false) return false;
            if ((packet.Payload.TargetSystem == 0 || Server.Identity.SystemId == 0 ||
                 packet.Payload.TargetSystem == Server.Identity.SystemId) == false) return false;
            if ((packet.Payload.TargetComponent == 0 || Server.Identity.ComponentId == 0 ||
                 packet.Payload.TargetComponent == Server.Identity.ComponentId) == false) return false;
            return true;
        }

        protected override void InternalDisposeOnce()
        {
            base.InternalDisposeOnce();
            _dataCallbacks.Clear();
        }
    }
}
