using System;

using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.AsvGbs;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Microsoft.Extensions.Logging;
using R3;
using ZLogger;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;
using MavType = Asv.Mavlink.V2.Minimal.MavType;

namespace Asv.Mavlink;

public class AsvGbsExServer: IAsvGbsServerEx, IDisposable,IAsyncDisposable
{
    private readonly IHeartbeatServer _heartbeatServer;
    private readonly int _maxMessageLength = new GpsRtcmDataPayload().Data.Length;
    public static readonly int MaxDgpsMessageLength = new GpsRtcmDataPayload().Data.Length;
    public static readonly int MaxRtcmMessageLength = MaxDgpsMessageLength * 4;
    
    private readonly ILogger _logger;
    private uint _seqNumber;

    public AsvGbsExServer(IAsvGbsServer server, 
        IHeartbeatServer heartbeatServer, 
        ICommandServerEx<CommandLongPacket> commands)
    {
        Base = server;
        _logger = server.Core.Log.CreateLogger<AsvGbsExServer>();
        _heartbeatServer = heartbeatServer;
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

        Position = new ReactiveProperty<GeoPoint>();
        _sub1 = Position.Subscribe(x => server.Set(status =>
        {
            status.Lat = MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(x.Latitude);
            status.Lng = MavlinkTypesHelper.LatLonDegDoubleToFromInt32E7To(x.Longitude);
            status.Alt = MavlinkTypesHelper.AltFromDoubleMeterToInt32Mm(x.Altitude);
        }));

        AccuracyMeter = new ReactiveProperty<double>();
        _sub2 =AccuracyMeter.Select(d=>(ushort)Math.Round(d*100,0)).Subscribe(x => server.Set(status =>
        {
            status.Accuracy = x;
        }));
        ObservationSec = new ReactiveProperty<ushort>();
        _sub3 = ObservationSec.Subscribe(x => server.Set(status =>
        {
            status.Observation = x;
        }));
        DgpsRate = new ReactiveProperty<ushort>();
        _sub4 = DgpsRate.Subscribe(x => server.Set(status =>
        {
            status.DgpsRate = x;
        }));
        AllSatellites = new ReactiveProperty<byte>();
        _sub5 = AllSatellites.Subscribe(b => server.Set(status =>
        {
            status.SatAll = b;
        }));
        GalSatellites = new ReactiveProperty<byte>();
        _sub6 = GalSatellites.Subscribe(b => server.Set(status =>
        {
            status.SatGal = b;
        }));
        BeidouSatellites = new ReactiveProperty<byte>();
        _sub7 = BeidouSatellites.Subscribe(b => server.Set(status =>
        {
            status.SatBdu = b;
        }));
        GlonassSatellites = new ReactiveProperty<byte>();
        _sub8 = GlonassSatellites.Subscribe(b => server.Set(status =>
        {
            status.SatGlo = b;
        }));
        GpsSatellites = new ReactiveProperty<byte>();
        _sub9 = GpsSatellites.Subscribe(b => server.Set(status =>
        {
            status.SatGps = b;
        }));
        QzssSatellites = new ReactiveProperty<byte>();
        _sub10 = QzssSatellites.Subscribe(b => server.Set(status =>
        {
            status.SatQzs = b;
        }));
        SbasSatellites = new ReactiveProperty<byte>();
        _sub11 = SbasSatellites.Subscribe(b => server.Set(status =>
        {
            status.SatSbs = b;
        }));
        ImesSatellites = new ReactiveProperty<byte>();
        _sub12 =ImesSatellites.Subscribe(b => server.Set(status =>
        {
            status.SatIme = b;
        }));

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
        CustomMode = new ReactiveProperty<AsvGbsCustomMode>();
        _sub13 = CustomMode.Subscribe(mode => heartbeatServer.Set(p =>
        {
            p.CustomMode = (uint)mode;
        }));

        #endregion
    }

    public void Start()
    {
        Base.Start();
        _heartbeatServer.Start();
    }

    public IAsvGbsServer Base { get; }
    public ReactiveProperty<AsvGbsCustomMode> CustomMode { get; }
    public ReactiveProperty<GeoPoint> Position { get; }
    public ReactiveProperty<double> AccuracyMeter { get; }
    public ReactiveProperty<ushort> ObservationSec { get; }
    public ReactiveProperty<ushort> DgpsRate { get; }
    public ReactiveProperty<byte> AllSatellites { get; }
    public ReactiveProperty<byte> GalSatellites { get; }
    public ReactiveProperty<byte> BeidouSatellites { get; }
    public ReactiveProperty<byte> GlonassSatellites { get; }
    public ReactiveProperty<byte> GpsSatellites { get; }
    public ReactiveProperty<byte> QzssSatellites { get; }
    public ReactiveProperty<byte> SbasSatellites { get; }
    public ReactiveProperty<byte> ImesSatellites { get; }
    public StartAutoModeDelegate? StartAutoMode { get; set; }
    public StartFixedModeDelegate? StartFixedMode { get; set; }
    public StartIdleModeDelegate? StartIdleMode { get; set; }
    
    public async Task SendRtcmData(byte[] data, int length, CancellationToken cancel)
    {
        if (length > MaxRtcmMessageLength)
        {
            _logger.ZLogError($"RTCM message for DGPS is too large '{length}'");
            return;
        }

        // number of packets we need, including a termination packet if needed
        var pktCount = (int)Math.Ceiling((double)length / MaxDgpsMessageLength);
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

                var dataLength = Math.Min(length - i1 * MaxDgpsMessageLength, MaxDgpsMessageLength);
                var dataArray = new byte[dataLength];
                Array.Copy(data, i1 * MaxDgpsMessageLength, dataArray, 0, dataLength);
                pkt.Payload.Data = dataArray;

                pkt.Payload.Len = (byte)dataLength;
            }, cancel).ConfigureAwait(false);
        }
    }

    #region Dispose
    
    private readonly IDisposable _sub1;
    private readonly IDisposable _sub2;
    private readonly IDisposable _sub3;
    private readonly IDisposable _sub4;
    private readonly IDisposable _sub5;
    private readonly IDisposable _sub6;
    private readonly IDisposable _sub7;
    private readonly IDisposable _sub8;
    private readonly IDisposable _sub9;
    private readonly IDisposable _sub10;
    private readonly IDisposable _sub11;
    private readonly IDisposable _sub12;
    private readonly IDisposable _sub13;

    public void Dispose()
    {
        _sub1.Dispose();
        _sub2.Dispose();
        _sub3.Dispose();
        _sub4.Dispose();
        _sub5.Dispose();
        _sub6.Dispose();
        _sub7.Dispose();
        _sub8.Dispose();
        _sub9.Dispose();
        _sub10.Dispose();
        _sub11.Dispose();
        _sub12.Dispose();
        _sub13.Dispose();
        CustomMode.Dispose();
        Position.Dispose();
        AccuracyMeter.Dispose();
        ObservationSec.Dispose();
        DgpsRate.Dispose();
        AllSatellites.Dispose();
        GalSatellites.Dispose();
        BeidouSatellites.Dispose();
        GlonassSatellites.Dispose();
        GpsSatellites.Dispose();
        QzssSatellites.Dispose();
        SbasSatellites.Dispose();
        ImesSatellites.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await CastAndDispose(_sub1).ConfigureAwait(false);
        await CastAndDispose(_sub2).ConfigureAwait(false);
        await CastAndDispose(_sub3).ConfigureAwait(false);
        await CastAndDispose(_sub4).ConfigureAwait(false);
        await CastAndDispose(_sub5).ConfigureAwait(false);
        await CastAndDispose(_sub6).ConfigureAwait(false);
        await CastAndDispose(_sub7).ConfigureAwait(false);
        await CastAndDispose(_sub8).ConfigureAwait(false);
        await CastAndDispose(_sub9).ConfigureAwait(false);
        await CastAndDispose(_sub10).ConfigureAwait(false);
        await CastAndDispose(_sub11).ConfigureAwait(false);
        await CastAndDispose(_sub12).ConfigureAwait(false);
        await CastAndDispose(_sub13).ConfigureAwait(false);
        await CastAndDispose(CustomMode).ConfigureAwait(false);
        await CastAndDispose(Position).ConfigureAwait(false);
        await CastAndDispose(AccuracyMeter).ConfigureAwait(false);
        await CastAndDispose(ObservationSec).ConfigureAwait(false);
        await CastAndDispose(DgpsRate).ConfigureAwait(false);
        await CastAndDispose(AllSatellites).ConfigureAwait(false);
        await CastAndDispose(GalSatellites).ConfigureAwait(false);
        await CastAndDispose(BeidouSatellites).ConfigureAwait(false);
        await CastAndDispose(GlonassSatellites).ConfigureAwait(false);
        await CastAndDispose(GpsSatellites).ConfigureAwait(false);
        await CastAndDispose(QzssSatellites).ConfigureAwait(false);
        await CastAndDispose(SbasSatellites).ConfigureAwait(false);
        await CastAndDispose(ImesSatellites).ConfigureAwait(false);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync().ConfigureAwait(false);
            else
                resource.Dispose();
        }
    }

    #endregion
}