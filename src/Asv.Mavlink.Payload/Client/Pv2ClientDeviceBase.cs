using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Client;
using NLog;

namespace Asv.Mavlink.Payload
{
    public class Pv2DeviceBaseConfig
    {
        public MavlinkClientConfig MavlinkClientConfig { get; } = new();
        public byte NetworkId { get; set; } = 0;
        public Pv2ClientParamsInterfaceConfig Params { get; } = new();
        public Pv2ClientRttInterfaceConfig Rtt { get; } = new();
        public int RequestInitDataDelayAfterFailMs { get; set; } = 5000;
        public string DefaultName { get; set; } = "PayloadV2";
    }

    public class Pv2ClientDeviceBase : DisposableOnceWithCancel, IPv2ClientDevice
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Pv2ClientBaseInterface _base;
        private readonly Pv2DeviceBaseConfig _cfg;
        private readonly MavlinkClient _client;
        private readonly Pv2ClientParamsInterface _params;
        private readonly PayloadV2Client _payloadClient;
        private readonly Pv2ClientPowerCycleInterface _pv2ClientPower;
        private readonly Pv2ClientRttInterface _rtt;
        private readonly Pv2ClientMissionInterface _mission;


        public Pv2ClientDeviceBase(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
            IPacketSequenceCalculator sequence, IPv2CfgDescriptionStore store, IPv2RttDescriptionStore rttStore, IPv2BaseDescriptionStore workModeStore,
            Pv2DeviceBaseConfig cfg, bool disposeConnection = false, IScheduler scheduler = null)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (identity == null) throw new ArgumentNullException(nameof(identity));
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (store == null) throw new ArgumentNullException(nameof(store));
            if (rttStore == null) throw new ArgumentNullException(nameof(rttStore));
            var workModeStore1 = workModeStore ?? throw new ArgumentNullException(nameof(workModeStore));
            _cfg = cfg ?? throw new ArgumentNullException(nameof(cfg));
            if (disposeConnection) Disposable.Add(connection);
            _client = new MavlinkClient(connection, identity, _cfg.MavlinkClientConfig, sequence, disposeConnection, scheduler).DisposeItWith(Disposable);
            InitRawStatus();
            _payloadClient = new PayloadV2Client(_client, _cfg.NetworkId).DisposeItWith(Disposable);
            _params = new Pv2ClientParamsInterface(_payloadClient, store, _cfg.Params).DisposeItWith(Disposable);
            _base = new Pv2ClientBaseInterface(_payloadClient, _params, _cfg.DefaultName, workModeStore1).DisposeItWith(Disposable);
            InitRequestVehicleInfo();
            _pv2ClientPower = new Pv2ClientPowerCycleInterface(_payloadClient, _params).DisposeItWith(Disposable);
            _rtt = new Pv2ClientRttInterface(_payloadClient, _params, rttStore, _cfg.Rtt).DisposeItWith(Disposable);
            _mission = new Pv2ClientMissionInterface(Client, _base).DisposeItWith(Disposable);
        }

        public IRxValue<LinkState> Link => _client.Heartbeat.Link;
        public IRxValue<int> PacketRateHz => _client.Heartbeat.PacketRateHz;
        public IRxValue<double> LinkQuality => _client.Heartbeat.LinkQuality;
        public IPayloadV2Client Client => _payloadClient;

        public IPv2ClientParamsInterface Params => _params;
        public IPv2ClientBaseInterface Base => _base;
        public IPv2PowerCycle Power => _pv2ClientPower;
        public IPv2ClientRttInterface Rtt => _rtt;
        public IPv2ClientMissionInterface Mission => _mission;

        #region Info

        private readonly RxValue<VehicleStatusMessage> _textStatus = new();
        public IRxValue<VehicleStatusMessage> TextStatus => _textStatus;

        private void InitRawStatus()
        {
            _client.Rtt.RawStatusText
                .Select(_ => new VehicleStatusMessage
                    { Sender = Base?.Name?.Value, Text = new string(_.Text), Type = _.Severity }).Subscribe(_textStatus)
                .DisposeItWith(Disposable);
            Disposable.Add(_textStatus);
        }

        #endregion

        #region Request init info

        private readonly RxValue<VehicleInitState> _initState = new();
        public IRxValue<VehicleInitState> InitState => _initState;

        private int _isRequestInfoIsInProgressOrAlreadySuccess;
        private bool _needToRequestAgain = true;

        private void InitRequestVehicleInfo()
        {
            _initState.OnNext(VehicleInitState.WaitConnection);
            Link.DistinctUntilChanged().Where(_ => _ == LinkState.Disconnected)
                .Subscribe(_ => _needToRequestAgain = true).DisposeItWith(Disposable);
            Link.DistinctUntilChanged().ObserveOn(Scheduler.Default).Where(_ => _needToRequestAgain).Where(_ => _ == LinkState.Connected)
                // only one time
                .Subscribe(_ => Task.Factory.StartNew(TryToRequestData, CancellationToken.None, TaskCreationOptions.LongRunning | TaskCreationOptions.DenyChildAttach , TaskScheduler.Default))
                .DisposeItWith(Disposable);
            Disposable.Add(_initState);
        }

        private async void TryToRequestData()
        {
            if (Interlocked.CompareExchange(ref _isRequestInfoIsInProgressOrAlreadySuccess, 1, 0) == 1) return;
            _initState.OnNext(VehicleInitState.InProgress);
            try
            {
                await InternalRequestInitialDataAfterDisconnect(DisposeCancel).ConfigureAwait(false);
                await Params.RequestAll(DisposeCancel).ConfigureAwait(false);
                // !!!! call after all params readed !!!!
                await _base.ReloadWhenDisconnected(DisposeCancel).ConfigureAwait(false);
                await _rtt.RequestAll(DisposeCancel).ConfigureAwait(false);
                _initState.OnNext(VehicleInitState.Complete);
                _needToRequestAgain = false;
            }
            catch (Exception e)
            {
                Logger.Error($"Error to read all vehicle info:{e.Message}");
                _initState.OnNext(VehicleInitState.Failed);
                Observable.Timer(TimeSpan.FromMilliseconds(_cfg.RequestInitDataDelayAfterFailMs))
                    .Subscribe(_ => TryToRequestData()).DisposeItWith(Disposable);
            }
            finally
            {
                Interlocked.Exchange(ref _isRequestInfoIsInProgressOrAlreadySuccess, 0);
            }
        }

        protected virtual Task InternalRequestInitialDataAfterDisconnect(CancellationToken cancel = default)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
