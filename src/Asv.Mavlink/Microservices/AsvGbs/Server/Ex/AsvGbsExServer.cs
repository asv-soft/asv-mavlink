using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZLogger;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;
using MavType = Asv.Mavlink.V2.Minimal.MavType;

namespace Asv.Mavlink;

public class AsvGbsExServer: DisposableOnceWithCancel,IAsvGbsServerEx
{
    private readonly int _maxMessageLength = new GpsRtcmDataPayload().Data.Length;
    private readonly ILogger _logger;
    private uint _seqNumber;
    public AsvGbsExServer(IAsvGbsServer server, 
        IHeartbeatServer heartbeatServer, 
        ICommandServerEx<CommandLongPacket> commands,
        ILogger? logger = null)
    {
        Base = server;
        _logger ??= NullLogger.Instance;
        #region Commands

        commands[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunAutoMode] = async (id,args, cancel) =>
        {
            if (StartAutoMode == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            var result = await StartAutoMode(args.Payload.Param1, args.Payload.Param2, cancel).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        }; 
        commands[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunFixedMode] = async (id,args, cancel) =>
        {
            if (StartFixedMode == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            var lat = BitConverter.ToInt32(BitConverter.GetBytes(args.Payload.Param1),0) / 10000000.0;
            var lon = BitConverter.ToInt32(BitConverter.GetBytes(args.Payload.Param2),0) / 10000000.0;
            var alt = BitConverter.ToInt32(BitConverter.GetBytes(args.Payload.Param3),0) / 1000.0;
            var ac = args.Payload.Param4;
            var result = await StartFixedMode(new GeoPoint(lat,lon,alt),ac, cancel).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };
        commands[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunIdleMode] = async (id,args, cancel) =>
        {
            if (StartIdleMode == null) return CommandResult.FromResult(MavResult.MavResultUnsupported);
            var result = await StartIdleMode(cancel).ConfigureAwait(false);
            return CommandResult.FromResult(result);
        };

        #endregion

        #region Telemetry

        Position = new RxValue<GeoPoint>().DisposeItWith(Disposable);
        Position.Subscribe(x => server.Set(status =>
        {
            status.Lat = MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(x.Latitude);
            status.Lng = MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(x.Longitude);
            status.Alt = MavlinkTypesHelper.AltFromDoubleMeterToInt32Mm(x.Altitude);
        })).DisposeItWith(Disposable);

        AccuracyMeter = new RxValue<double>().DisposeItWith(Disposable);
        AccuracyMeter.Select(d=>(ushort)Math.Round(d*100,0)).DistinctUntilChanged().Subscribe(x => server.Set(status =>
        {
            status.Accuracy = x;
        })).DisposeItWith(Disposable);
        ObservationSec = new RxValue<ushort>().DisposeItWith(Disposable);
        ObservationSec.DistinctUntilChanged().Subscribe(x => server.Set(status =>
        {
            status.Observation = x;
        })).DisposeItWith(Disposable);
        DgpsRate = new RxValue<ushort>().DisposeItWith(Disposable);
        DgpsRate.DistinctUntilChanged().Subscribe(x => server.Set(status =>
        {
            status.DgpsRate = x;
        })).DisposeItWith(Disposable);
        AllSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        AllSatellites.DistinctUntilChanged().Subscribe(b => server.Set(status =>
        {
            status.SatAll = b;
        })).DisposeItWith(Disposable);
        GalSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        GalSatellites.DistinctUntilChanged().Subscribe(b => server.Set(status =>
        {
            status.SatGal = b;
        })).DisposeItWith(Disposable);
        BeidouSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        BeidouSatellites.DistinctUntilChanged().Subscribe(b => server.Set(status =>
        {
            status.SatBdu = b;
        })).DisposeItWith(Disposable);
        GlonassSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        GlonassSatellites.DistinctUntilChanged().Subscribe(b => server.Set(status =>
        {
            status.SatGlo = b;
        })).DisposeItWith(Disposable);
        GpsSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        GpsSatellites.DistinctUntilChanged().Subscribe(b => server.Set(status =>
        {
            status.SatGps = b;
        })).DisposeItWith(Disposable);
        QzssSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        QzssSatellites.DistinctUntilChanged().Subscribe(b => server.Set(status =>
        {
            status.SatQzs = b;
        })).DisposeItWith(Disposable);
        SbasSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        SbasSatellites.DistinctUntilChanged().Subscribe(b => server.Set(status =>
        {
            status.SatSbs = b;
        })).DisposeItWith(Disposable);
        ImesSatellites = new RxValue<byte>().DisposeItWith(Disposable);
        ImesSatellites.DistinctUntilChanged().Subscribe(b => server.Set(status =>
        {
            status.SatIme = b;
        })).DisposeItWith(Disposable);

        #endregion

        #region Custom mode

        heartbeatServer.Set(p =>
        {
            p.Autopilot = MavAutopilot.MavAutopilotInvalid;
            p.Type = (MavType)V2.AsvGbs.MavType.MavTypeAsvGbs;
            p.SystemStatus = MavState.MavStateActive;
            p.BaseMode = MavModeFlag.MavModeFlagCustomModeEnabled;
            p.MavlinkVersion = 3;
            p.CustomMode = (uint)V2.AsvGbs.AsvGbsCustomMode.AsvGbsCustomModeLoading;
        });
        CustomMode = new RxValue<AsvGbsCustomMode>().DisposeItWith(Disposable);
        CustomMode.DistinctUntilChanged().Subscribe(mode => heartbeatServer.Set(p =>
        {
            p.CustomMode = (uint)mode;
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
            _logger.ZLogError($"RTCM message for DGPS is too large '{length}'");
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