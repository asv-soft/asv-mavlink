using System.Reactive.Concurrency;
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
            var mock = new Mock<IAsvGbsExClient>();
            mock.Setup(_=>_.AccuracyMeter).Returns(new RxValue<double>(0.15));
            mock.Setup(_=>_.ObservationSec).Returns(new RxValue<ushort>(1));
            mock.Setup(_=>_.DgpsRate).Returns(new RxValue<ushort>(2));
            mock.Setup(_=>_.AllSatellites).Returns(new RxValue<byte>(3));
            mock.Setup(_=>_.GalSatellites).Returns(new RxValue<byte>(4));
            mock.Setup(_=>_.BeidouSatellites).Returns(new RxValue<byte>(5));
            mock.Setup(_=>_.GlonassSatellites).Returns(new RxValue<byte>(6));
            mock.Setup(_=>_.GpsSatellites).Returns(new RxValue<byte>(7));
            mock.Setup(_=>_.QzssSatellites).Returns(new RxValue<byte>(8));
            mock.Setup(_=>_.SbasSatellites).Returns(new RxValue<byte>(9));
            mock.Setup(_=>_.ImesSatellites).Returns(new RxValue<byte>(10));
            mock.Setup(_=>_.VehicleCount).Returns(new RxValue<byte>(11));
            mock.Setup(_=>_.CustomMode).Returns(new RxValue<AsvGbsCustomMode>(mode));
            mock.Setup(_=>_.Position).Returns(new RxValue<GeoPoint>(new GeoPoint(lat,lon,alt)));
            
            IGbsServerDevice serverDevice = new GbsServerDevice(link.Server, new MavlinkServerIdentity(),new PacketSequenceCalculator(),Scheduler.Default,new GbsServerDeviceConfig(),mock.Object); 
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
            
            var mock = new Mock<IAsvGbsExClient>();
            mock.Setup(_=>_.AccuracyMeter).Returns(new RxValue<double>(0.15));
            mock.Setup(_=>_.ObservationSec).Returns(new RxValue<ushort>(1));
            mock.Setup(_=>_.DgpsRate).Returns(new RxValue<ushort>(2));
            mock.Setup(_=>_.AllSatellites).Returns(new RxValue<byte>(3));
            mock.Setup(_=>_.GalSatellites).Returns(new RxValue<byte>(4));
            mock.Setup(_=>_.BeidouSatellites).Returns(new RxValue<byte>(5));
            mock.Setup(_=>_.GlonassSatellites).Returns(new RxValue<byte>(6));
            mock.Setup(_=>_.GpsSatellites).Returns(new RxValue<byte>(7));
            mock.Setup(_=>_.QzssSatellites).Returns(new RxValue<byte>(8));
            mock.Setup(_=>_.SbasSatellites).Returns(new RxValue<byte>(9));
            mock.Setup(_=>_.ImesSatellites).Returns(new RxValue<byte>(10));
            mock.Setup(_=>_.VehicleCount).Returns(new RxValue<byte>(11));
            mock.Setup(_=>_.CustomMode).Returns(new RxValue<AsvGbsCustomMode>(AsvGbsCustomMode.AsvGbsCustomModeIdle));
            mock.Setup(_=>_.Position).Returns(new RxValue<GeoPoint>(GeoPoint.Zero));

            
            
            var serverMode = new RxValue<AsvGbsCustomMode>(AsvGbsCustomMode.AsvGbsCustomModeIdle);
            var serverPos = new RxValue<GeoPoint>(GeoPoint.Zero);
            var called = false;
            mock.Setup(_ => _.CustomMode).Returns(serverMode);
            mock.Setup(_ => _.Position).Returns(serverPos);
            mock.Setup(_ => _.StartAutoMode(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((float dur,float acc,CancellationToken cancel)=>
                {
                    called = true;
                    Assert.Equal(duration,dur);
                    Assert.Equal(accuracy,acc);
                    return result;
                });
            var serverDevice = new GbsServerDevice(link.Server, new MavlinkServerIdentity(),new PacketSequenceCalculator(),Scheduler.Default, new GbsServerDeviceConfig(),mock.Object); 
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
            Assert.Equal(11,clientDevice.Gbs.VehicleCount.Value);
            

        }
       

    }
}