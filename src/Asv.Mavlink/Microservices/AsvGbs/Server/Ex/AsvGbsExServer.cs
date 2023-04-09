using System;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class AsvGbsExServer: DisposableOnceWithCancel,IAsvGbsServerEx
{
    public AsvGbsExServer(IAsvGbsServer server, 
        IHeartbeatServer heartbeatServer, 
        ICommandServerEx<CommandLongPacket> commands, 
        IAsvGbsExClient gbsImplementation)
    {
        #region Commands

        commands[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunAutoMode] = async (id,args, cancel) =>
        {
            var result = await gbsImplementation.StartAutoMode(args.Payload.Param1, args.Payload.Param2, cancel);
            return new CommandResult(result);
        }; 
        commands[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunFixedMode] = async (id,args, cancel) =>
        {
            var lat = BitConverter.ToInt32(BitConverter.GetBytes(args.Payload.Param1),0) / 10000000D;
            var lon = BitConverter.ToInt32(BitConverter.GetBytes(args.Payload.Param2),0) / 10000000D;
            var alt = BitConverter.ToInt32(BitConverter.GetBytes(args.Payload.Param3),0) / 1000D;
            var result = await gbsImplementation.StartFixedMode(new GeoPoint(lat,lon,alt),0.1f, cancel);
            return new CommandResult(result);
        };
        commands[(MavCmd)V2.AsvGbs.MavCmd.MavCmdAsvGbsRunIdleMode] = async (id,args, cancel) =>
        {
            var result = await gbsImplementation.StartIdleMode(cancel);
            return new CommandResult(result);
        };

        #endregion

        #region Telemetry
        
        gbsImplementation.Position.Subscribe(_ => server.Set(status =>
        {
            status.Lat = (int)(_.Latitude * 10000000D);
            status.Lng = (int)(_.Longitude * 10000000D);
            status.Alt = (int)(_.Altitude * 1000D);
        })).DisposeItWith(Disposable);
        
        gbsImplementation.VehicleCount.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.VehicleCount = _;
        })).DisposeItWith(Disposable);
        
        gbsImplementation.AccuracyMeter.Select(_=>(ushort)Math.Round(_*100,0)).DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.Accuracy = _;
        })).DisposeItWith(Disposable);
        
        gbsImplementation.ObservationSec.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.Observation = _;
        })).DisposeItWith(Disposable);
        
        gbsImplementation.DgpsRate.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.DgpsRate = _;
        })).DisposeItWith(Disposable);

        gbsImplementation.AllSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatAll = _;
        })).DisposeItWith(Disposable);
        gbsImplementation.GalSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatGal = _;
        })).DisposeItWith(Disposable);
        
        gbsImplementation.BeidouSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatBdu = _;
        })).DisposeItWith(Disposable);
        
        gbsImplementation.GlonassSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatGlo = _;
        })).DisposeItWith(Disposable);
        gbsImplementation.GpsSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatGps = _;
        })).DisposeItWith(Disposable);
        gbsImplementation.QzssSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatQzs = _;
        })).DisposeItWith(Disposable);
        gbsImplementation.SbasSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
        {
            status.SatSbs = _;
        })).DisposeItWith(Disposable);
        gbsImplementation.ImesSatellites.DistinctUntilChanged().Subscribe(_ => server.Set(status =>
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
        
        gbsImplementation.CustomMode.DistinctUntilChanged().Subscribe(mode => heartbeatServer.Set(_ =>
        {
            _.CustomMode = (uint)mode;
        })).DisposeItWith(Disposable);

        #endregion
    }


    public IAsvGbsServer Bsae { get; }
}