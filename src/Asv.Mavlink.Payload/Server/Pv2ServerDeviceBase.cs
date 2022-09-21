using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.Payload.Digits;
using Asv.Mavlink.Server;

namespace Asv.Mavlink.Payload
{
    public class Pv2ServerDeviceBase : DisposableOnceWithCancel, IPv2ServerDevice
    {
        private readonly Pv2ServerBaseInterface _base;
        private readonly Pv2ServerMissionInterface _mission;
        private readonly Pv2ServerParamsInterface _params;
        private readonly PayloadV2Server _payloadServer;
        private readonly Pv2ServerPowerInterface _power;
        private readonly Pv2ServerRttInterface _rtt;

        


        public Pv2ServerDeviceBase(IMavlinkV2Connection connection, IConfiguration cfgSvc,
            IEnumerable<Pv2ParamType> paramList, Pv2DeviceClass @class, IEnumerable<IWorkModeFactory> workModes,
            IChunkStore rttStore,
            IEnumerable<Pv2RttRecordDesc> rttRecords,
            IEnumerable<Pv2RttFieldDesc> rttFields, string configSuffix = "PV2")
        {
            var systemId = (byte)Pv2DeviceParams.SystemId.ReadFromConfigValue(cfgSvc, configSuffix);
            var componentId = (byte)Pv2DeviceParams.ComponentId.ReadFromConfigValue(cfgSvc, configSuffix);
            var networkId = (byte)Pv2DeviceParams.NetworkId.ReadFromConfigValue(cfgSvc, configSuffix);
            Seq = new PacketSequenceCalculator();
            var mavlinkServer = new MavlinkServerBase(connection, new MavlinkServerIdentity
            {
                ComponentId = componentId,
                SystemId = systemId
            }, Seq).DisposeItWith(Disposable);

            _payloadServer = new PayloadV2Server(mavlinkServer, networkId).DisposeItWith(Disposable);

            var defaultParamsList =
                Pv2DeviceParams.Params.Concat(paramList)
                    .Concat(Pv2BaseInterface.Params)
                    .Concat(Pv2PowerInterface.Params)
                    .Concat(Pv2RttInterface.Params)
                    .Concat(Pv2MissionInterface.Params);

            _params = new Pv2ServerParamsInterface(_payloadServer, cfgSvc, defaultParamsList, configSuffix)
                .DisposeItWith(Disposable);

            _base = new Pv2ServerBaseInterface(_payloadServer, @class, workModes).DisposeItWith(Disposable);

            _power = new Pv2ServerPowerInterface(_payloadServer, _params).DisposeItWith(Disposable);

            _rtt = new Pv2ServerRttInterface(_payloadServer, _base, _params, rttStore, rttRecords, rttFields)
                .DisposeItWith(Disposable);

            _mission = new Pv2ServerMissionInterface(_payloadServer, _rtt, _base).DisposeItWith(Disposable);

            _base.OnLogMessage.Merge(_params.OnLogMessage).Merge(_power.OnLogMessage).Merge(_mission.OnLogMessage)
                .Subscribe(_ => Server.Server.StatusText.Log(_)).DisposeItWith(Disposable);
        }

        public IPacketSequenceCalculator Seq { get; }

        public IPayloadV2Server Server => _payloadServer;

        public IPv2ServerParamsInterface Params => _params;

        public IPv2ServerBaseInterface Base => _base;

        public IPv2ServerPowerInterface Power => _power;

        public IPv2ServerRttInterface Rtt => _rtt;

        public IPv2ServerMissionInterface Mission => _mission;
    }
}
