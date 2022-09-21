using System;
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
    public interface IPv2ClientMissionInterface
    {
        Task Download(int attempt, CancellationToken cancel = default, Action<double> progress = null);
        Task Upload(int attempt, CancellationToken cancel = default, Action<double> progress = null);
        IObservable<IChangeSet<Pv2MissionItem, uint>> Mission { get; }
        IRxValue<bool> NotSync { get; }
        Pv2MissionItem Add(Pv2MissionTrigger trigger, Pv2MissionAction action);
        void Edit(uint index, Action<Pv2MissionTrigger, Pv2MissionAction> editCallback);
        void Remove(uint index);
        Task Start(ushort index = 0, CancellationToken cancel = default);
        Task Stop(CancellationToken cancel = default);
        IRxValue<bool> IsStarted { get; }
        void ClearLocalMission();
    }

    public class Pv2ClientMissionInterface : Pv2ClientInterfaceBase, IPv2ClientMissionInterface
    {
        private readonly SourceCache<Pv2MissionItem, uint> _missionSource;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly RxValue<bool> _notSync = new();
        private readonly RxValue<bool> _isStarted = new();

        public Pv2ClientMissionInterface(IPayloadV2Client client, IPv2ClientBaseInterface baseIfc) : base(client, Pv2MissionInterface.InterfaceName)
        {
            _missionSource = new SourceCache<Pv2MissionItem, uint>(_ => _.Index).DisposeItWith(Disposable);
            _notSync.DisposeItWith(Disposable);
            _isStarted.DisposeItWith(Disposable);
            baseIfc.Info.Select(_ => _.MissionStarted).Subscribe(_isStarted).DisposeItWith(Disposable);
        }

        public override Task ReloadWhenDisconnected(CancellationToken cancel)
        {
            return Download(3, cancel);
        }

        public async Task Download(int attempt, CancellationToken cancel = default, Action<double> progress = null)
        {
            try
            {
                Logger.Info($"{LogSend} Download payload mission");
                progress?.Invoke(0);
                ClearLocalMission();
                
                var info = await Client.Call(Pv2MissionInterface.GetInfo, SpanVoidType.Default,attempt:attempt,cancel:cancel);
                for (uint i = 0; i < info.Count; i++)
                {
                    var result = await Client.Call(Pv2MissionInterface.ReadMissionItem, new SpanPacketUnsignedIntegerType(i),cancel);
                    _missionSource.AddOrUpdate(result);
                    progress?.Invoke((double)(i+1)/info.Count);
                }
                progress?.Invoke(1);
                _notSync.OnNext(false);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"{LogSend} Error to upload payload mission:{e.Message}");
                _notSync.OnNext(true);
                throw;
            }
        }

        public IRxValue<bool> NotSync => _notSync;

        public Pv2MissionItem Add(Pv2MissionTrigger trigger, Pv2MissionAction action)
        {
            var res = new Pv2MissionItem
            {
                Index = (uint)_missionSource.Count,
                Trigger = trigger.Clone(),
                Action = action.Clone(),
            };
            _missionSource.AddOrUpdate(res);
            _notSync.OnNext(true);
            return res;
        }

        public void Edit(uint index, Action<Pv2MissionTrigger, Pv2MissionAction> editCallback)
        {
            var item = _missionSource.Lookup(index);
            if (item.HasValue == false)
                throw new Exception($"{LogSend} Error to edit: Mission item with index {index} not found");
            editCallback(item.Value.Trigger, item.Value.Action);
            _notSync.OnNext(true);
        }

        public void Remove(uint index)
        {
            _missionSource.RemoveKey(index);
            _notSync.OnNext(true);
        }

        public Task Start(ushort index = 0, CancellationToken cancel = default)
        {
            Logger.Info($"{LogSend} Start payload mission from {index} item");
            return Client.Call(Pv2MissionInterface.Start, new SpanPacketUnsignedIntegerType(index), cancel);
        }

        public Task Stop(CancellationToken cancel = default)
        {
            Logger.Info($"{LogSend} Stop payload mission");
            return Client.Call(Pv2MissionInterface.Stop, SpanVoidType.Default, cancel);
        }

        public IRxValue<bool> IsStarted => _isStarted;

        public void ClearLocalMission()
        {
            Logger.Info($"{LogSend} Clear local mission");
            _missionSource.Clear();
        }

        public async Task Upload(int attempt, CancellationToken cancel = default, Action<double> progress = null)
        {
            try
            {
                Logger.Info($"{LogSend} Upload payload mission");
                progress?.Invoke(0);
                await Client.Call(Pv2MissionInterface.ClearAll, SpanVoidType.Default, attempt: attempt, cancel: cancel);
                var items =  _missionSource.Items.OrderBy(_=>_.Index).ToArray();
                _missionSource.Clear();
                uint index = 0;
                foreach (var item in items)
                {
                    item.Index = index++;
                    var newItem = await Client.Call(Pv2MissionInterface.WriteMissionItem, item, cancel);
                    _missionSource.AddOrUpdate(newItem);
                    progress?.Invoke((double)index/items.Length);
                }
                progress?.Invoke(1);
                _notSync.OnNext(false);
            }
            catch (Exception e)
            {
                Logger.Error(e,$"{LogSend} Error to upload payload mission:{e.Message}");
                _notSync.OnNext(true);
                throw;
            }
        }

        public IObservable<IChangeSet<Pv2MissionItem,uint>> Mission => _missionSource.Connect().RefCount().Publish();
    }
}
