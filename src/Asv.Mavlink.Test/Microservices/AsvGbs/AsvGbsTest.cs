﻿using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.Common;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test
{
    public class AsvGbsTest
    {
        private readonly ITestOutputHelper _output;

        public AsvGbsTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Theory]
        [InlineData(12,  12, 12345)]
        [InlineData(1234567,  7654321, -123)]
        [InlineData(0,  0, 0)]
        public async Task StatusTest(int lat,int lon,int alt)
        {
            var link = new VirtualLink();

            var mode = AsvGbsCustomMode.AsvGbsCustomModeAuto;
            IGbsServerDevice serverDevice = new GbsServerDevice(link.Server, new MavlinkServerIdentity(),new PacketSequenceCalculator(),Scheduler.Default,new GbsServerDeviceConfig());
            serverDevice.Gbs.AccuracyMeter.OnNext(0.15);
            serverDevice.Gbs.ObservationSec.OnNext(1);
            serverDevice.Gbs.DgpsRate.OnNext(2);
            serverDevice.Gbs.AllSatellites.OnNext(3);
            serverDevice.Gbs.GalSatellites.OnNext(4);
            serverDevice.Gbs.BeidouSatellites.OnNext(5);
            serverDevice.Gbs.GlonassSatellites.OnNext(6);
            serverDevice.Gbs.GpsSatellites.OnNext(7);
            serverDevice.Gbs.QzssSatellites.OnNext(8);
            serverDevice.Gbs.SbasSatellites.OnNext(9);
            serverDevice.Gbs.ImesSatellites.OnNext(10);
            serverDevice.Gbs.CustomMode.OnNext(mode);
            serverDevice.Gbs.Position.OnNext(new GeoPoint(lat,lon,alt));
            
            IGbsClientDevice clientDevice = new GbsClientDevice(link.Client,new MavlinkClientIdentity(), new PacketSequenceCalculator(),Scheduler.Default, new GbsClientDeviceConfig());
            serverDevice.Heartbeat.Set(_ => _.CustomMode = (uint)mode);
            serverDevice.Gbs.Base.Set(_ =>
            {
                _.Lat = lat;
                _.Lng = lon;
                _.Alt = alt;
            });
            serverDevice.Start();
            
            clientDevice.WaitUntilConnect();
            await Task.Delay(2000);
            var clientState = await clientDevice.Heartbeat.RawHeartbeat.FirstAsync();
            Assert.Equal((uint)mode,clientState.CustomMode);

            var status = await clientDevice.Gbs.Base.RawStatus.FirstAsync();
            Assert.Equal(lat,status.Lat);            
            Assert.Equal(lon,status.Lng);
            Assert.Equal(alt,status.Alt);
        }

        
        
        [Theory]
        [InlineData(1,   1, MavResult.MavResultAccepted)]
        [InlineData(1000,   1000, MavResult.MavResultAccepted)]
        [InlineData(float.NaN,   float.NaN, MavResult.MavResultFailed)]
        public async Task CommandTest(float duration,float accuracy, MavResult result)
        {
            var link = new VirtualLink();
            var called = false;
            var serverDevice = new GbsServerDevice(link.Server, new MavlinkServerIdentity(),new PacketSequenceCalculator(),Scheduler.Default, new GbsServerDeviceConfig());
            serverDevice.Gbs.StartAutoMode = (dur, acc, cancel) =>
            {
                called = true;
                Assert.Equal(duration, dur);
                Assert.Equal(accuracy, acc);
                return Task.FromResult(result);
            };
            serverDevice.Gbs.AccuracyMeter.OnNext(0.15);
            serverDevice.Gbs.ObservationSec.OnNext(1);
            serverDevice.Gbs.DgpsRate.OnNext(2);
            serverDevice.Gbs.AllSatellites.OnNext(3);
            serverDevice.Gbs.GalSatellites.OnNext(4);
            serverDevice.Gbs.BeidouSatellites.OnNext(5);
            serverDevice.Gbs.GlonassSatellites.OnNext(6);
            serverDevice.Gbs.GpsSatellites.OnNext(7);
            serverDevice.Gbs.QzssSatellites.OnNext(8);
            serverDevice.Gbs.SbasSatellites.OnNext(9);
            serverDevice.Gbs.ImesSatellites.OnNext(10);
            serverDevice.Gbs.CustomMode.OnNext(AsvGbsCustomMode.AsvGbsCustomModeIdle);
            serverDevice.Gbs.Position.OnNext(GeoPoint.Zero);
            var clientDevice = new GbsClientDevice(link.Client,new MavlinkClientIdentity(), new PacketSequenceCalculator(),Scheduler.Default, new GbsClientDeviceConfig());
            serverDevice.Start();
            clientDevice.WaitUntilConnect();
            await Task.Delay(2000);
            
            var res = await clientDevice.Gbs.StartAutoMode(duration, accuracy, CancellationToken.None);
            Assert.Equal(result,res);
            Assert.True(called);

            
            Assert.Equal(0.15,clientDevice.Gbs.AccuracyMeter.Value);
            Assert.Equal(1,clientDevice.Gbs.ObservationSec.Value);
            Assert.Equal(2,clientDevice.Gbs.DgpsRate.Value);
            Assert.Equal(3,clientDevice.Gbs.AllSatellites.Value);
            Assert.Equal(4,clientDevice.Gbs.GalSatellites.Value);
            Assert.Equal(5,clientDevice.Gbs.BeidouSatellites.Value);
            Assert.Equal(6,clientDevice.Gbs.GlonassSatellites.Value);
            Assert.Equal(7,clientDevice.Gbs.GpsSatellites.Value);
            Assert.Equal(8,clientDevice.Gbs.QzssSatellites.Value);
            Assert.Equal(9,clientDevice.Gbs.SbasSatellites.Value);
            Assert.Equal(10,clientDevice.Gbs.ImesSatellites.Value);
            

        }
       

    }
}