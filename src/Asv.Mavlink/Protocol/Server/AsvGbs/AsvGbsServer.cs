using System;
using Asv.Common;
using Asv.Mavlink.Server;
using Asv.Mavlink.V2.AsvGbs;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink
{
    

    public class AsvGbsServer:DisposableOnceWithCancel, IAsvGbsServer
    {
        private readonly MavlinkPacketTransponder<AsvGbsOutStatusPacket,AsvGbsOutStatusPayload> _transponder;
        private IAsvGbsClient _client;

        public AsvGbsServer(IMavlinkV2Connection connection, IPacketSequenceCalculator seq,MavlinkServerIdentity identity,ICommandLongServer cmd)
        {
            cmd[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunAutoMode] = async (args, cancel) =>
            {
                if (_client == null)
                {
                    throw new NotImplementedException(nameof(V2.AsvGbs.MavCmd.MavCmdAsvGbsRunAutoMode));
                }
                var result = await _client.StartAutoMode(args.Param1, args.Param2, cancel);
                return new CommandLongResult(result);
            };
            cmd[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunFixedMode] = async (args, cancel) =>
            {
                if (_client == null)
                {
                    throw new NotImplementedException(nameof(V2.AsvGbs.MavCmd.MavCmdAsvGbsRunFixedMode));
                }

                var lat = BitConverter.ToInt32(BitConverter.GetBytes(args.Param1),0) / 10000000D;
                var lon = BitConverter.ToInt32(BitConverter.GetBytes(args.Param2),0) / 10000000D;
                var alt = BitConverter.ToInt32(BitConverter.GetBytes(args.Param3),0) / 1000D;
                var result = await _client.StartFixedMode(new GeoPoint(lat,lon,alt), cancel);
                return new CommandLongResult(result);
            };
            cmd[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunIdleMode] = async (args, cancel) =>
            {
                if (_client == null)
                {
                    throw new NotImplementedException(nameof(V2.AsvGbs.MavCmd.MavCmdAsvGbsRunIdleMode));
                }
                var result = await _client.StartIdleMode(cancel);
                return new CommandLongResult(result);
            };
            _transponder =
                new MavlinkPacketTransponder<AsvGbsOutStatusPacket, AsvGbsOutStatusPayload>(connection, identity, seq);
            _transponder.Set(_ => _.State = AsvGbsState.AsvGbsStateIdleMode);
        }

        public void UpdateStatus(Action<AsvGbsOutStatusPayload> changeCallback)
        {
            _transponder.Set(changeCallback);
        }

      
        public void Init(TimeSpan statusRate, IAsvGbsClient localImplementation)
        {
            if (_client != null) throw new Exception($"{nameof(AsvGbsServer)} already init");
            _client = localImplementation ?? throw new ArgumentNullException(nameof(localImplementation));
            _client.DisposeItWith(Disposable);
            _client.State.Subscribe(_ => _transponder.Set(status => status.State = _)).DisposeItWith(Disposable);
            _client.Position.Subscribe(_ => _transponder.Set(status =>
            {
                status.Lat = (int)(_.Latitude * 10000000D);
                status.Lng = (int)(_.Longitude * 10000000D);
                status.Alt = (int)(_.Altitude * 1000D);
            })).DisposeItWith(Disposable);
            _transponder.Start(statusRate);
        }
    }
}