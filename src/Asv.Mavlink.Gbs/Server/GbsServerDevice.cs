using System;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.Server;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink;

public class GbsServerDevice:DisposableOnceWithCancel, IGbsServerDevice
{
    private readonly IGbsClientDevice _interfaceImplementation;
    public static Logger Logger = LogManager.GetCurrentClassLogger();
    public GbsServerDevice(IGbsClientDevice interfaceImplementation, IMavlinkServer server, int statusRateMs = 1000, bool disposeServer = true)
    {
        _interfaceImplementation = interfaceImplementation ?? throw new ArgumentNullException(nameof(interfaceImplementation));
        Server = server ?? throw new ArgumentNullException(nameof(server));
        if (disposeServer)
        {
            server.DisposeItWith(Disposable);
        }
        Server.Heartbeat.Set(_ =>
        {
            _.Autopilot = MavAutopilot.MavAutopilotInvalid;
            _.Type = (MavType)V2.AsvGbs.MavType.MavTypeAsvGbs;
            _.SystemStatus = MavState.MavStateActive;
            _.BaseMode = MavModeFlag.MavModeFlagCustomModeEnabled;
            _.MavlinkVersion = 3;
            _.CustomMode = (uint)V2.AsvGbs.AsvGbsCustomMode.AsvGbsCustomModeLoading;
        });
        Server.Heartbeat.Start();
        
        _interfaceImplementation.Position.Subscribe(_ => Server.Gbs.Set(status =>
        {
            status.Lat = (int)(_.Latitude * 10000000D);
            status.Lng = (int)(_.Longitude * 10000000D);
            status.Alt = (int)(_.Altitude * 1000D);
        })).DisposeItWith(Disposable);
        
        _interfaceImplementation.VehicleCount.DistinctUntilChanged().Subscribe(_ => Server.Gbs.Set(status =>
        {
            status.VehicleCount = _;
        })).DisposeItWith(Disposable);
        
        _interfaceImplementation.AccuracyMeter.Select(_=>(ushort)Math.Round(_*100,0)).DistinctUntilChanged().Subscribe(_ => Server.Gbs.Set(status =>
        {
            status.Accuracy = _;
        })).DisposeItWith(Disposable);
        
        _interfaceImplementation.ObservationSec.DistinctUntilChanged().Subscribe(_ => Server.Gbs.Set(status =>
        {
            status.Observation = _;
        })).DisposeItWith(Disposable);
        
        _interfaceImplementation.DgpsRate.DistinctUntilChanged().Subscribe(_ => Server.Gbs.Set(status =>
        {
            status.DgpsRate = _;
        })).DisposeItWith(Disposable);

        #region Sattelites statistics

        _interfaceImplementation.AllSatellites.DistinctUntilChanged().Subscribe(_ => Server.Gbs.Set(status =>
        {
            status.SatAll = _;
        })).DisposeItWith(Disposable);
        _interfaceImplementation.GalSatellites.DistinctUntilChanged().Subscribe(_ => Server.Gbs.Set(status =>
        {
            status.SatGal = _;
        })).DisposeItWith(Disposable);
        
        _interfaceImplementation.BeidouSatellites.DistinctUntilChanged().Subscribe(_ => Server.Gbs.Set(status =>
        {
            status.SatBdu = _;
        })).DisposeItWith(Disposable);
        
        _interfaceImplementation.GlonassSatellites.DistinctUntilChanged().Subscribe(_ => Server.Gbs.Set(status =>
        {
            status.SatGlo = _;
        })).DisposeItWith(Disposable);
        _interfaceImplementation.GpsSatellites.DistinctUntilChanged().Subscribe(_ => Server.Gbs.Set(status =>
        {
            status.SatGps = _;
        })).DisposeItWith(Disposable);
        _interfaceImplementation.QzssSatellites.DistinctUntilChanged().Subscribe(_ => Server.Gbs.Set(status =>
        {
            status.SatQzs = _;
        })).DisposeItWith(Disposable);
        _interfaceImplementation.SbasSatellites.DistinctUntilChanged().Subscribe(_ => Server.Gbs.Set(status =>
        {
            status.SatSbs = _;
        })).DisposeItWith(Disposable);
        _interfaceImplementation.ImesSatellites.DistinctUntilChanged().Subscribe(_ => Server.Gbs.Set(status =>
        {
            status.SatIme = _;
        })).DisposeItWith(Disposable);
        

        #endregion

        _interfaceImplementation.CustomMode.Subscribe(mode => Server.Heartbeat.Set(_ =>
        {
            _.CustomMode = (uint)mode;
        })).DisposeItWith(Disposable);
        
        Server.Gbs.Start(TimeSpan.FromMilliseconds(statusRateMs));
        
        Server.CommandLong[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunAutoMode] = async (args, cancel) =>
        {
            var result = await _interfaceImplementation.StartAutoMode(args.Param1, args.Param2, cancel);
            return new CommandLongResult(result);
        }; 
        Server.CommandLong[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunFixedMode] = async (args, cancel) =>
        {
            var lat = BitConverter.ToInt32(BitConverter.GetBytes(args.Param1),0) / 10000000D;
            var lon = BitConverter.ToInt32(BitConverter.GetBytes(args.Param2),0) / 10000000D;
            var alt = BitConverter.ToInt32(BitConverter.GetBytes(args.Param3),0) / 1000D;
            var result = await _interfaceImplementation.StartFixedMode(new GeoPoint(lat,lon,alt),0.1f, cancel);
            return new CommandLongResult(result);
        };
        Server.CommandLong[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunIdleMode] = async (args, cancel) =>
        {
            var result = await _interfaceImplementation.StartIdleMode(cancel);
            return new CommandLongResult(result);
        };
    }
    public IMavlinkServer Server { get; }
}