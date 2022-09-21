using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using DynamicData;
using NLog;

namespace Asv.Mavlink.Payload
{
    public class Pv2ClientBaseInterface : Pv2ClientInterfaceBase, IPv2ClientBaseInterface
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly SourceCache<Pv2WorkModeInfo,byte> _allWorkModes = new(_=>_.Id);
        private readonly RxValue<Pv2DeviceCompatibilityFlags> _compatibilityFlags = new();
        private readonly string _defaultName;
        private readonly IPv2BaseDescriptionStore _store;
        private readonly RxValue<Pv2DeviceInfo> _deviceInfo = new();
        private readonly RxValue<Pv2WorkModeInfo> _modeInfo = new();
        private readonly RxValue<string> _nameSubject = new();

        private readonly IPv2ClientParamsInterface _paramSvc;
        private readonly RxValue<Pv2WorkModeStatusInfo> _status = new();
        private readonly ConcurrentDictionary<ushort, Pv2WorkModeStatusInfo> _statusItems = new();

        public Pv2ClientBaseInterface(IPayloadV2Client client, IPv2ClientParamsInterface paramSvc, string defaultName, IPv2BaseDescriptionStore store) :
            base(client, "BASE")
        { 
            if (client == null) throw new ArgumentNullException(nameof(client));
            _paramSvc = paramSvc ?? throw new ArgumentNullException(nameof(paramSvc));
            _defaultName = defaultName ?? throw new ArgumentNullException(nameof(defaultName));
            _store = store ?? throw new ArgumentNullException(nameof(store));

            _paramSvc.OnUpdated.FilterString(Pv2BaseInterface.Name).Subscribe(_ => _nameSubject.OnNext(_))
                .DisposeItWith(Disposable);
            Disposable.Add(_nameSubject);

            Disposable.Add(_modeInfo);
            Disposable.Add(_status);
            Disposable.Add(_compatibilityFlags);
            _deviceInfo.Subscribe(UpdateWorkMode).DisposeItWith(Disposable);
            client.Client.Heartbeat.RawHeartbeat.Select(_ => _.CustomMode).DistinctUntilChanged()
                .Select(_=>new Pv2DeviceInfo(_))
                .Subscribe(_deviceInfo).DisposeItWith(Disposable);
            
            Disposable.Add(_deviceInfo);
        }

        public IRxValue<Pv2DeviceInfo> Info => _deviceInfo;
        public IObservable<IChangeSet<Pv2WorkModeInfo,byte>> AllWorkModes => _allWorkModes.Connect();
        public IRxValue<Pv2WorkModeInfo> WorkMode => _modeInfo;
        public IRxValue<Pv2WorkModeStatusInfo> WorkModeStatus => _status;
        public IRxValue<string> Name => _nameSubject;
        public Task SetWorkMode(byte id, CancellationToken cancel = default)
        {
            return Client.Call(Pv2BaseInterface.SetWorkMode,new SpanByteType(id), cancel);
        }

        private void UpdateWorkMode(Pv2DeviceInfo info)
        {
            var combinedId = Pv2BaseInterface.CombineId(info.WorkMode,info.WorkModeStatus);
            if (_statusItems.TryGetValue(combinedId, out var status))
            {
                var currentMode = _allWorkModes.Items.FirstOrDefault(_ => _.Id == info.WorkMode);
                if (currentMode != null) _modeInfo.OnNext(currentMode);
                _status.OnNext(status);
            }
        }

        public override async Task ReloadWhenDisconnected(CancellationToken cancel)
        {
            _nameSubject.OnNext(GetCustomName());
            Logger.Trace($"{LogSend} read work mode list ");
            _allWorkModes.Clear();
            _statusItems.Clear();

            var info = await Client.Call(Pv2BaseInterface.GetWorkModeList, SpanVoidType.Default, cancel: cancel).ConfigureAwait(false);
            if (_store.TryGetFromCache(info.DescHash, (uint)info.WorkModes.Items.Count, out var descList))
            {
                Logger.Trace($"{LogRecv} loading description from cache: {string.Join(",", info.WorkModes.Items)}");
                for (byte i = 0; i < descList.Count; i++)
                {
                    var tuple = descList[i];
                    _allWorkModes.AddOrUpdate(tuple.Item1);
                    Logger.Trace($"{LogRecv} {tuple.Item1.Name} {tuple.Item1.Id}");
                    for (byte j = 0; j < tuple.Item2.Count; j++)
                    {
                        _statusItems.AddOrUpdate(Pv2BaseInterface.CombineId(i, j), tuple.Item2[j], (_, _) => tuple.Item2[j]);
                        Logger.Trace($"{LogRecv} status {tuple.Item1.Name}:{tuple.Item2[j].Name}");
                    }
                }
            }
            else
            {
                Logger.Trace($"{LogRecv} list: {string.Join(",", info.WorkModes.Items)}");
                
                var storeItems = new List<(Pv2WorkModeInfo, IList<Pv2WorkModeStatusInfo>)>();
                for (byte i = 0; i < info.WorkModes.Items.Count; i++)
                {
                    Logger.Trace($"{LogSend} read work mode {i}");
                    var mode = await Client.Call(Pv2BaseInterface.GetWorkModeInfo, new SpanByteType(i), cancel: cancel)
                        .ConfigureAwait(false);
                    Logger.Trace($"{LogRecv} {mode.Name} {mode.Id}");
                    _allWorkModes.AddOrUpdate(mode);
                    var statusCount = info.WorkModes.Items[i];
                    var items = new List<Pv2WorkModeStatusInfo>();
                    for (byte j = 0; j < statusCount; j++)
                    {
                        Logger.Trace($"{LogSend} read status for work mode {mode.Name} with index {j}");
                        var modeStatus = await Client.Call(Pv2BaseInterface.GetModeStatusInfo, new SpanDoubleByteType(i, j), cancel: cancel)
                            .ConfigureAwait(false);
                        _statusItems.AddOrUpdate(Pv2BaseInterface.CombineId(i, j), modeStatus, (_, _) => modeStatus);
                        items.Add(modeStatus);
                        Logger.Trace($"{LogRecv} status {mode.Name}:{modeStatus.Name}");
                    }
                    storeItems.Add((mode,items));
                }
                _store.Save(info.DescHash, storeItems);

            }
            UpdateWorkMode(new Pv2DeviceInfo(Client.Client.Heartbeat.RawHeartbeat.Value.CustomMode));
        }

        protected virtual string GetCustomName()
        {
            try
            {
                return _paramSvc.ReadString(Pv2BaseInterface.Name);
            }
            catch (Exception e)
            {
                Logger.Error($"Error to read custom payload name:{e.Message}");
                return _defaultName;
            }
        }
    }
}
