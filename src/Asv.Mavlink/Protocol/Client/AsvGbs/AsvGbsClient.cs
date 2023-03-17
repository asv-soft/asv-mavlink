using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Client;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink
{
    public class AsvGbsClient:MavlinkMicroserviceClient, IAsvGbsClient
    {
        private const int DefaultAttemptCount = 3;
        private readonly IMavlinkCommandClient _cmd;
        private readonly RxValue<AsvGbsState> _state;
        private readonly RxValue<GeoPoint> _position;


        public AsvGbsClient(IMavlinkV2Connection connection, MavlinkClientIdentity identity,
            IPacketSequenceCalculator seq, IScheduler scheduler,IMavlinkCommandClient cmd) 
            : base(connection,identity,seq,"GBS", scheduler)
        {
            _cmd = cmd ?? throw new ArgumentNullException(nameof(cmd));
            _state = new RxValue<AsvGbsState>(AsvGbsState.AsvGbsStateIdleMode).DisposeItWith(Disposable);
            _position = new RxValue<GeoPoint>(GeoPoint.Zero).DisposeItWith(Disposable);
            Filter<AsvGbsOutStatusPacket>().Select(_ => _.Payload.State).Subscribe(_state).DisposeItWith(Disposable);
            Filter<AsvGbsOutStatusPacket>().Select(Convert).Subscribe(_position).DisposeItWith(Disposable);
        }

        private static GeoPoint Convert(AsvGbsOutStatusPacket payload)
        {
            return new GeoPoint(payload.Payload.Lat / 10000000D,payload.Payload.Lng / 10000000D,payload.Payload.Alt / 1000D);
        }

        public IRxValue<AsvGbsState> State => _state;
        public IRxValue<GeoPoint> Position => _position;
        public async Task<MavResult> StartAutoMode(float duration, float accuracy, CancellationToken cancel)
        {
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            var result = await _cmd.CommandLong((MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunAutoMode,
                duration,
                accuracy,
                Single.NaN,
                Single.NaN,
                Single.NaN,
                Single.NaN,
                Single.NaN,
                DefaultAttemptCount, cs.Token);
            return result.Result;
        }

        public async Task<MavResult> StartFixedMode(GeoPoint geoPoint, CancellationToken cancel)
        {
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            var result = await _cmd.CommandLong((MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunAutoMode,
                BitConverter.ToSingle(BitConverter.GetBytes(geoPoint.Latitude),0),
                BitConverter.ToSingle(BitConverter.GetBytes(geoPoint.Longitude),0),
                BitConverter.ToSingle(BitConverter.GetBytes(geoPoint.Altitude),0),
                Single.NaN,
                Single.NaN,
                Single.NaN,
                Single.NaN,
                DefaultAttemptCount, cs.Token);
            return result.Result;
        }

        public async Task<MavResult> StartIdleMode(CancellationToken cancel)
        {
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
            var result = await _cmd.CommandLong((MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunAutoMode,
                Single.NaN,
                Single.NaN,
                Single.NaN,
                Single.NaN,
                Single.NaN,
                Single.NaN,
                Single.NaN,
                DefaultAttemptCount, cs.Token);
            return result.Result;
        }
    }
}