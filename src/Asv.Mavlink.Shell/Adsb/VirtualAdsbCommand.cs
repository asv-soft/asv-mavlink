using System;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.Vehicle;
using ManyConsole;

namespace Asv.Mavlink.Shell;

public class VirtualAdsbCommand : ConsoleCommand
{
    private readonly CancellationTokenSource _cancel = new ();
    private readonly Subject<ConsoleKeyInfo> _userInput = new ();
    
    private string _cs = "tcp://127.0.0.1:7344?server=true";
    private string _start = "-O54.2905802|63.6063891|500";
    private string _stop = "-O64.2905802|83.6063891|500";
    private string _callSign = "UFO";
    private double _vectorVelocity = 300;
    private int _updateRate = 1;

    public VirtualAdsbCommand()
    {
        IsCommand("adsb", "Generate virtual ADSB Vehicle");
        HasOption("cs=", $"Mavlink connection string. By default '{_cs}'", _ => _cs = _);
        HasOption("start=", $"GeoPoint from where vehicle starts flying. Default value is {_start}", _ => _start = _);
        HasOption("end=", $"GeoPoint where vehicle stops flight. Default value is {_stop}", _ => _stop = _);
        HasOption("callSign=", $"Vehicle call sign. Default value is {_callSign}", _ => _callSign = _);
        HasOption("updateRate=", $"Update rate in Hz. Default value is {_updateRate}", _ =>
        {
            if (int.TryParse(_, out var result))
            {
                _updateRate = result;
            }
        });
        HasOption("vectorSpeed=", $"Vehicle vector velocity. Default value is {_vectorVelocity}", _ =>
        {
            if (double.TryParse(_, out var result))
            {
                _vectorVelocity = result;
            }
        });
    }
    
    private void KeyListen()
    {
        while (!_cancel.IsCancellationRequested)
        {
            var key = Console.ReadKey(true);
            _userInput.OnNext(key);
        }
    }
    
    private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
        if (e.Cancel) _cancel.Cancel(false);
    }

    private static GeoPoint SetGeoPoint(string value)
    {
        var values = value.Split('|');
        var altitude = 500.0;
        var result = new GeoPoint();

        if (values.Length == 3 &&
            GeoPointLatitude.TryParse(values[0], out var latitude) && 
            GeoPointLongitude.TryParse(values[1], out var longitude) &&
            double.TryParse(values[2], out altitude))
        {
            result = new GeoPoint(latitude, longitude, altitude);
        }
        else
        {
            Console.WriteLine($"Incorrect GeoPoint string fromat");
        }

        return result;
    }
    
    public override int Run(string[] remainingArguments)
    {
        Task.Factory.StartNew(_ => KeyListen(), _cancel.Token);
        Console.CancelKeyPress += Console_CancelKeyPress;
        
        while (!_cancel.IsCancellationRequested)
        {
            var start = SetGeoPoint(_start);
            var stop = SetGeoPoint(_stop);

            var totalDistance = GeoMath.Distance(start.Latitude, start.Longitude, 
                start.Altitude, stop.Latitude, stop.Longitude, stop.Altitude);
            var totalSeconds = totalDistance / _vectorVelocity;
            var groundDistance = GeoMath.Distance(start, stop); // TODO: suggest to rename this method to GroundDistance
            var vertVelocity = (start.Altitude - stop.Altitude) / totalSeconds;
            var horVelocity = groundDistance / totalSeconds;
            var azimuth = GeoMath.Azimuth(start, stop);
            
            var server = new AdsbServerDevice(
                new MavlinkV2Connection(_cs, _ =>
                {
                    _.RegisterCommonDialect();
                }),
                new PacketSequenceCalculator(),
                new MavlinkServerIdentity{ComponentId = 13, SystemId = 13},
                new AdsbServerDeviceConfig(),
                Scheduler.Default);

            for (var i = 0; i < totalDistance / _vectorVelocity * _updateRate; i++)
            {
                var nextPoint = start.RadialPoint(horVelocity / _updateRate, azimuth);
                nextPoint = nextPoint.AddAltitude(vertVelocity / _updateRate);
            
                server.Adsb.Send(_ =>
                {
                    _.Altitude = (int)nextPoint.Altitude;
                    _.Lon = (int)nextPoint.Longitude;
                    _.Lat = (int)nextPoint.Latitude;
                    _.Callsign = _callSign.ToCharArray();
                    _.Flags = AdsbFlags.AdsbFlagsSimulated;
                    _.Squawk = 15;
                    _.Heading = 13;
                    _.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
                    _.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
                    _.HorVelocity = 150;
                    _.VerVelocity = 75;
                    _.IcaoAddress = 1313;
                });
                
                Thread.Sleep(1000 / _updateRate);
            }
        }

        return -1;
    }
}