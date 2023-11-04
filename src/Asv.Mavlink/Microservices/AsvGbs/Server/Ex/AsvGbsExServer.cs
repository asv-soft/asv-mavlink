using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using NLog;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;
using MavType = Asv.Mavlink.V2.Minimal.MavType;

namespace Asv.Mavlink;

public class AsvGbsExServer: DisposableOnceWithCancel,IAsvGbsServerEx
{
    private readonly int _maxMessageLength = new GpsRtcmDataPayload().Data.Length;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private uint _seqNumber;
    public AsvGbsExServer(IAsvGbsServer server, 
        IHeartbeatServer heartbeatServer, 
        ICommandServerEx<CommandLongPacket> commands)
    {
        Base = server;

        #region Commands

        commands[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunAutoMode] = async (id,args, cancel) =>
        {
            if (StartAutoMode == null) return new CommandResult(MavResult.MavResultUnsupported);
            var result = await StartAutoMode(args.Payload.Param1, args.Payload.Param2, cancel).ConfigureAwait(false);
            return new CommandResult(result);
        }; 
        commands[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunFixedMode] = async (id,args, cancel) =>
        {
            if (StartFixedMode == null) return new CommandResult(MavResult.MavResultUnsupported);
            var lat = BitConverter.ToInt32(BitConverter.GetBytes(args.Payload.Param1),0) / 10000000.0;
            var lon = BitConverter.ToInt32(BitConverter.GetBytes(args.Payload.Param2),0) / 10000000.0;
            var alt = BitConverter.ToInt32(BitConverter.GetBytes(args.Payload.Param3),0) / 1000.0;
            var ac = args.Payload.Param4;
            var result = await StartFixedMode(new GeoPoint(lat,lon,alt),ac, cancel).ConfigureAwait(false);
            return new CommandResult(result);
        };
        commands[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunIdleMode] = async (id,args, cancel) =>
        {
            if (StartIdleMode == null) return new CommandResult(MavResult.MavResultUnsupported);
            var result = await StartIdleMode(cancel).ConfigureAwait(false);
            return new CommandResult(result);
        };

        #endregion

        #region Telemetry

        Position = new RxValue<GeoPoint>().DisposeItWith(Disposable);
        Position.Subscribe(_ => server.Set(status =>
        {
            status.Lat = (int)(_.Latitude * 10000000D);
            status.Lng = (int)(_.Longitude * 10000000D);
            status.Alt = (int)(_.Altitude * 1000D);
        })).DisposeItWith(Disposable);

        AccuracyMeter = new RxValue<double>().DisposeItWith(Disposable);
        AccuracyMeter.Select(_=>(ushort)Math.Round(_*100,0)).DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.Accuracy = _;
        })).DisposeItWith(Disposable);
        ObservationSec = new RxValue<ushort>().DisposeItWith(Disposable);
        ObservationSec.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.Observation = _;
        })).DisposeItWith(Disposable);
        DgpsRate = new RxValue<ushort>().DisposeItWith(Disposable);
        DgpsRate.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.DgpsRate = _;
        })).DisposeItWith(Disposable);
        AllSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        AllSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatAll = _;
        })).DisposeItWith(Disposable);
        GalSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        GalSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatGal = _;
        })).DisposeItWith(Disposable);
        BeidouSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        BeidouSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatBdu = _;
        })).DisposeItWith(Disposable);
        GlonassSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        GlonassSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatGlo = _;
        })).DisposeItWith(Disposable);
        GpsSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        GpsSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatGps = _;
        })).DisposeItWith(Disposable);
        QzssSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        QzssSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatQzs = _;
        })).DisposeItWith(Disposable);
        SbasSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        SbasSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatSbs = _;
        })).DisposeItWith(Disposable);
        ImesSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        ImesSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatIme = _;
        })).DisposeItWith(Disposable);

        #endregion

        #region Custom mode

        heartbeatServer.Set(_ =>
        {
            _.Autopilot = MavAutopilot.MavAutopilotInvalid;
            _.Type = (MavType)V2.AsvGbs.MavType.MavTypeAsvGbs;
            _.SystemStatus = MavState.MavStateActive;
            _.BaseMode = MavModeFlag.MavModeFlagCustomModeEnabled;
            _.MavlinkVersion = 3;
            _.CustomMode = (uint)V2.AsvGbs.AsvGbsCustomMode.AsvGbsCustomModeLoading;
        });
        CustomMode = new RxValue<AsvGbsCustomMode>().DisposeItWith(Disposable);
        CustomMode.DistinctUntilChanged().Subscribe(mode => heartbeatServer.Set(_ =>
        {
            _.CustomMode = (uint)mode;
        })).DisposeItWith(Disposable);

        #endregion
    }

    public IAsvGbsServer Base { get; }
    public IRxEditableValue<AsvGbsCustomMode> CustomMode { get; }
    public IRxEditableValue<GeoPoint> Position { get; }
    public IRxEditableValue<double> AccuracyMeter { get; }
    public IRxEditableValue<ushort> ObservationSec { get; }
    public IRxEditableValue<ushort> DgpsRate { get; }
    public IRxEditableValue<byte> AllSatellites { get; }
    public IRxEditableValue<byte> GalSatellites { get; }
    public IRxEditableValue<byte> BeidouSatellites { get; }
    public IRxEditableValue<byte> GlonassSatellites { get; }
    public IRxEditableValue<byte> GpsSatellites { get; }
    public IRxEditableValue<byte> QzssSatellites { get; }
    public IRxEditableValue<byte> SbasSatellites { get; }
    public IRxEditableValue<byte> ImesSatellites { get; }
    public StartAutoModeDelegate StartAutoMode { get; set; }
    public StartFixedModeDelegate StartFixedMode { get; set; }
    public StartIdleModeDelegate StartIdleMode { get; set; }
    
    public async Task SendRtcmData(byte[] data, int length, CancellationToken cancel)
    {
        if (length > _maxMessageLength * 4)
        {
            _logger.Error($"RTCM message for DGPS is too large '{length}'");
            return;
        }

        // number of packets we need, including a termination packet if needed
        var pktCount = length / _maxMessageLength + 1;
        if (pktCount >= 4)
        {
            pktCount = 4;
        }

        for (var i = 0; i < pktCount; i++)
        {
            var i1 = i;
            await Base.SendDgps(pkt =>
            {
                // 1 means message is fragmented
                pkt.Payload.Flags = (byte)(pktCount > 1 ? 1 : 0);
                //  next 2 bits are the fragment ID
                pkt.Payload.Flags += (byte)((i1 & 0x3) << 1);
                // the remaining 5 bits are used for the sequence ID
                var increment = Interlocked.Increment(ref _seqNumber) % 31;
                pkt.Payload.Flags += (byte)((increment & 0x1f) << 3);

                var dataLength = Math.Min(length - i1 * _maxMessageLength, _maxMessageLength);
                var dataArray = new byte[dataLength];
                Array.Copy(data, i1 * _maxMessageLength, dataArray, 0, dataLength);
                pkt.Payload.Data = dataArray;

                pkt.Payload.Len = (byte)dataLength;
            }, cancel).ConfigureAwait(false);
        }
    }
}