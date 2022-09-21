using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Server;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink.Payload
{
    public interface IPv2ServerMissionInterface
    {
        IRxValue<bool> IsStarted { get; }
        IRxValue<ushort> CurrentIndex { get; }
    }

    public class Pv2ServerMissionInterface : Pv2ServerInterfaceBase, IPv2ServerMissionInterface
    {
        public const int MissionTickTimeMs = 500;
        private readonly IPv2ServerRttInterface _rtt;
        private readonly SourceCache<Pv2MissionItem, uint> _source = new(_=>_.Index);
        private readonly RxValue<bool> _isStarted = new();
        private readonly object _sync = new();
        private IDisposable _timer;
        private int _isMissionTick;
        private readonly RxValue<ushort> _currentIndex = new ();
        private readonly RxValue<MissionItemReachedPacket> _uavMissionItemReached = new();
        private readonly SourceCache<MissionItemReachedPacket,uint> _reachedWpList = new(_=>(uint)(_.SystemId | (_.ComponenId << 8) | (_.Payload.Seq << 16)));

        public Pv2ServerMissionInterface(IPayloadV2Server server,IPv2ServerRttInterface rtt, IPv2ServerBaseInterface baseIfc) : base(server, Pv2MissionInterface.InterfaceName)
        {
            _rtt = rtt;
            _isStarted.DisposeItWith(Disposable);
            _source.DisposeItWith(Disposable);
            _uavMissionItemReached.DisposeItWith(Disposable);

            _currentIndex.DisposeItWith(Disposable);

            server.Server.MavlinkV2Connection.Where(_ => _.MessageId == MissionItemReachedPacket.PacketMessageId)
                .Cast<MissionItemReachedPacket>().Subscribe(_uavMissionItemReached)
                .DisposeItWith(Disposable);

            _uavMissionItemReached.Subscribe(_=> _reachedWpList.AddOrUpdate(_)).DisposeItWith(Disposable);
            

            _isStarted.DistinctUntilChanged().Subscribe(_=> baseIfc.UpdateDeviceInfo(__ => __.MissionStarted = _)).DisposeItWith(Disposable);

            server.Register(Pv2MissionInterface.GetInfo,GetInfo);
            server.Register(Pv2MissionInterface.ReadMissionItem, ReadMissionItem);
            server.Register(Pv2MissionInterface.WriteMissionItem, WriteMissionItem);
            server.Register(Pv2MissionInterface.ClearAll, ClearAll);
            server.Register(Pv2MissionInterface.Start, Start);
            server.Register(Pv2MissionInterface.Stop, Stop);
        }

        public IRxValue<bool> IsStarted => _isStarted;
        public IRxValue<ushort> CurrentIndex => _currentIndex;

        private Task<(SpanVoidType, DeviceIdentity)> Stop(DeviceIdentity devid, SpanVoidType data, CancellationToken cancel)
        {
            InternalStopMission();
            return Task.FromResult((SpanVoidType.Default, devid));
        }

        private Task<(SpanVoidType, DeviceIdentity)> Start(DeviceIdentity devid, SpanPacketUnsignedIntegerType data, CancellationToken cancel)
        {
            InternalStartMission((ushort)data.Value);
            return Task.FromResult((SpanVoidType.Default, devid));
        }

        private Task<(SpanVoidType, DeviceIdentity)> ClearAll(DeviceIdentity devid, SpanVoidType data, CancellationToken cancel)
        {
            _source.Clear();
            return Task.FromResult((SpanVoidType.Default, devid));
        }

        private Task<(Pv2MissionItem, DeviceIdentity)> WriteMissionItem(DeviceIdentity devid, Pv2MissionItem data, CancellationToken cancel)
        {
            _source.AddOrUpdate(data);
            return Task.FromResult((data, devid));
        }

        private Task<(Pv2MissionItem, DeviceIdentity)> ReadMissionItem(DeviceIdentity devid, SpanPacketUnsignedIntegerType data, CancellationToken cancel)
        {
            var result = _source.Lookup(data.Value);
            if (result.HasValue == false)
                Task.FromException(new Exception($"Mission item '{data.Value}' not found"));
            return Task.FromResult((result.Value, devid));
        }

        private Task<(Pv2MissionInfo, DeviceIdentity)> GetInfo(DeviceIdentity devid, SpanVoidType data, CancellationToken cancel)
        {
            return Task.FromResult((new Pv2MissionInfo
            {
                Count = (ushort)_source.Count
            }, devid));
        }

        #region Mission executing

        private void MissionInit()
        {
            Disposable.AddAction(() =>
            {
                lock (_sync)
                {
                    _timer?.Dispose();
                    _timer = null;
                }
            });
        }

        private void InternalStopMission()
        {
            lock (_sync)
            {
                if (_isStarted.Value == false)
                {
                    LogInfo("Mission already stopped");
                    return;
                }
                _timer?.Dispose();
                LogInfo("Payload mission stopped");
                _isStarted.OnNext(false);
            }
        }

        private void InternalStartMission(ushort startIndex)
        {
            lock (_sync)
            {
                if (_isStarted.Value)
                {
                    LogError("Mission already started");
                    return;
                }

                if (_source.Count == 0)
                {
                    LogError("Mission is empty");
                    return;
                }

                if (_source.Count <= startIndex)
                {
                    LogError($"Unknown mission '{startIndex}'");
                    return;
                }
                _reachedWpList.Clear();
                _currentIndex.OnNext(startIndex);

                _timer?.Dispose();
                _timer = Observable.Timer(TimeSpan.FromMilliseconds(MissionTickTimeMs), TimeSpan.FromMilliseconds(MissionTickTimeMs)).Subscribe(MissionTick);
                LogInfo("Mission started");
                _isStarted.OnNext(true);
            }
            
        }

        private void MissionTick(long l)
        {
            if (Interlocked.CompareExchange(ref _isMissionTick, 1, 0) != 0)
            {
                Logger.Warn($"Tick skiped");
                return;
            }

            try
            {
                lock (_sync)
                {
                    var index = _currentIndex.Value;
                    var missionItem = _source.Lookup(index);
                    if (missionItem.HasValue == false)
                    {
                        throw new Exception($"Mission item with index '{index}' not found. Abort mission");
                    }

                    if (CheckTrigger(missionItem.Value.Trigger))
                    {
                        LogInfo($"{InterfaceName}:Trigger raised {missionItem.Value.Trigger}");
                        DoAction(missionItem.Value.Action);
                        LogInfo($"{InterfaceName}: Go to next {index}=>{index+1}");
                        ++index;
                        if (_source.Lookup(index).HasValue)
                        {
                            _currentIndex.OnNext(index);
                        }
                        else
                        {
                            LogInfo($"{InterfaceName}: Mission complete");
                            InternalStopMission();
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {
                LogError("Mission execution error");
                Logger.Error(e,$"Payload mission tick error: {e.Message}");
                InternalStopMission();
            }
            finally
            {
                Interlocked.Exchange(ref _isMissionTick, 0);
            }
        }

        private void DoAction(Pv2MissionAction action)
        {
            LogInfo($"Do action {action}");
            switch (action.Type)
            {
                case Pv2MissionActionType.Unknown:
                    return;
                case Pv2MissionActionType.StartRecord:
                    ((Pv2StartRecordAction)action).DoAction(_rtt);
                    break;
                case Pv2MissionActionType.StopRecord:
                    ((Pv2StopRecordAction)action).DoAction(_rtt);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool CheckTrigger(Pv2MissionTrigger trigger)
        {
            return trigger.Type switch
            {
                Pv2MissionTriggerType.Unknown => true,
                Pv2MissionTriggerType.UavWayPointReached => ((Pv2UavWayPointReachedTrigger)trigger).Check(_reachedWpList.Items.ToArray()),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        

        #endregion

        
    }
}
