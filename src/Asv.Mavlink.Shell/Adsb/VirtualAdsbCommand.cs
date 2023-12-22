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
    
    private string _cs = "tcp://127.0.0.1:7344?srv=true";
    private string _start1 = "-O54 17 26|63 38 23|500";
    private string _start2 = "-O32 17 26|43 38 23|500";
    private string _stop1 = "-O64 17 26|83 38 23|500";
    private string _stop2 = "-O42 17 26|63 38 23|500";
    private string _callSign = "UFO";
    private double _vectorVelocity = 300;
    private int _updateRate = 1;

    public VirtualAdsbCommand()
    {
        IsCommand("adsb", "Generate virtual ADSB Vehicle");
        HasOption("cs=", $"Mavlink connection string. By default '{_cs}'", _ => _cs = _);
        HasOption("start1=", $"GeoPoint from where vehicle starts flying. Default value is {_start1}", _ => _start1 = _);
        HasOption("start2=", $"GeoPoint from where vehicle starts flying. Default value is {_start2}", _ => _start2 = _);
        HasOption("stop1=", $"GeoPoint where vehicle stops flight. Default value is {_stop1}", _ => _stop1 = _);
        HasOption("stop2=", $"GeoPoint where vehicle stops flight. Default value is {_stop2}", _ => _stop2 = _);
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
        var result = new GeoPoint();

        if (values.Length == 3 &&
            GeoPointLatitude.TryParse(values[0], out var latitude) && 
            GeoPointLongitude.TryParse(values[1], out var longitude) &&
            double.TryParse(values[2], out var altitude))
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
        
        var conn = MavlinkV2Connection.Create(_cs);
        var server1 = new AdsbServerDevice(
            conn,
            new PacketSequenceCalculator(),
            new MavlinkIdentity(13, 13),
            new AdsbServerDeviceConfig(),
            Scheduler.Default);
        
        var server2 = new AdsbServerDevice(
            conn,
            new PacketSequenceCalculator(),
            new MavlinkIdentity(14,  14),
            new AdsbServerDeviceConfig(),
            Scheduler.Default);
            
        server1.Start();
        server2.Start();
        
        while (!_cancel.IsCancellationRequested)
        {
            var start1 = SetGeoPoint(_start1);
            var start2 = SetGeoPoint(_start2);
            var stop1 = SetGeoPoint(_stop1);
            var stop2 = SetGeoPoint(_stop2);

            var totalDistance1 = GeoMath.Distance(start1.Latitude, start1.Longitude, 
                start1.Altitude, stop1.Latitude, stop1.Longitude, stop1.Altitude);
            var totalDistance2 = GeoMath.Distance(start2.Latitude, start2.Longitude, 
                start2.Altitude, stop2.Latitude, stop2.Longitude, stop2.Altitude);
            var totalSeconds1 = totalDistance1 / _vectorVelocity;
            var totalSeconds2 = totalDistance2 / _vectorVelocity;
            var groundDistance1 = GeoMath.Distance(start1, stop1); // TODO: suggest to rename this method to GroundDistance
            var groundDistance2 = GeoMath.Distance(start2, stop2); // TODO: suggest to rename this method to GroundDistance
            var vertVelocity1 = (start1.Altitude - stop1.Altitude) / totalSeconds1;
            var vertVelocity2 = (start2.Altitude - stop2.Altitude) / totalSeconds2;
            var horVelocity1 = groundDistance1 / totalSeconds1;
            var horVelocity2 = groundDistance2 / totalSeconds2;
            var azimuth1 = GeoMath.Azimuth(start1, stop1);
            var azimuth2 = GeoMath.Azimuth(start2, stop2);

            var threads = new Thread[2];

            threads[0] = new Thread(() => {
                for (var i = 0.0; i < groundDistance1; i += horVelocity1)
                {
                    var nextPoint = start1.RadialPoint(i / _updateRate, azimuth1);
                    nextPoint = nextPoint.AddAltitude(vertVelocity1 / _updateRate);
                    
                    server1.Adsb.Send(_ =>
                    {
                        _.Altitude = (int)(nextPoint.Altitude * 1e3);
                        _.Lon = (int)(nextPoint.Longitude * 1e7);
                        _.Lat = (int)(nextPoint.Latitude * 1e7);
                        MavlinkTypesHelper.SetString(_.Callsign,_callSign);
                        _.Flags = AdsbFlags.AdsbFlagsSimulated;
                        _.Squawk = 15;
                        _.Heading = (ushort)(azimuth1 * 1e2);
                        _.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
                        _.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
                        _.HorVelocity = 150;
                        _.VerVelocity = 75;
                        _.IcaoAddress = 1313;
                    });

                    Console.WriteLine($"Current position 1: {nextPoint}");
                        
                    Thread.Sleep(1000 / _updateRate);
                }
                
                Console.WriteLine("\n ARRIVED 1 \n");
            });
            
            threads[1] = new Thread(() => {
                for (var i = 0.0; i < groundDistance2; i += horVelocity2)
                {
                    var nextPoint = start2.RadialPoint(i / _updateRate, azimuth2);
                    nextPoint = nextPoint.AddAltitude(vertVelocity2 / _updateRate);
                    
                    server2.Adsb.Send(_ =>
                    {
                        _.Altitude = (int)(nextPoint.Altitude * 1e3);
                        _.Lon = (int)(nextPoint.Longitude * 1e7);
                        _.Lat = (int)(nextPoint.Latitude * 1e7);
                        MavlinkTypesHelper.SetString(_.Callsign,_callSign);
                        _.Flags = AdsbFlags.AdsbFlagsSimulated;
                        _.Squawk = 15;
                        _.Heading = (ushort)(azimuth2 * 1e2);
                        _.AltitudeType = AdsbAltitudeType.AdsbAltitudeTypeGeometric;
                        _.EmitterType = AdsbEmitterType.AdsbEmitterTypeNoInfo;
                        _.HorVelocity = 150;
                        _.VerVelocity = 75;
                        _.IcaoAddress = 1313;
                    });

                    Console.WriteLine($"Current position 2: {nextPoint}");
                        
                    Thread.Sleep(1000 / _updateRate);
                }
                
                Console.WriteLine("\n ARRIVED 2 \n");
            });
            
            threads[0].Start();
            threads[1].Start();

            threads[0].Join();
            threads[1].Join();
        }

        return -1;
    }
}