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
            var (client,server) = MavlinkTestHelper.CreateServerAndClientDevices();

            var mode = AsvGbsCustomMode.AsvGbsCustomModeAuto;

            server.Heartbeat.Set(_ => _.CustomMode = (uint)mode);
            server.Gbs.Set(_ =>
            {
                _.Lat = lat;
                _.Lng = lon;
                _.Alt = alt;
            });
            server.Gbs.Start();
            server.Heartbeat.Start();
            
            MavlinkTestHelper.WaitUntilConnect(client);
            await Task.Delay(2000);
            var clientState = await client.Heartbeat.RawHeartbeat.FirstAsync();
            Assert.Equal((uint)mode,clientState.CustomMode);

            var status = await client.Gbs.RawStatus.FirstAsync();
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
            var (client,server) = MavlinkTestHelper.CreateServerAndClientDevices();
            var mock = new Mock<IGbsClientDevice>();
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
            var serverDevice = new GbsServerDevice(mock.Object, server, TODO);
            var clientDevice = new GbsClientDevice(client, TODO, TODO, TODO);
            MavlinkTestHelper.WaitUntilConnect(client);
            await Task.Delay(2000);
            
            var res = await clientDevice.StartAutoMode(duration, accuracy, CancellationToken.None);
            Assert.Equal(result,res);
            Assert.True(called);

            
            Assert.Equal(0.15,clientDevice.AccuracyMeter.Value);
            Assert.Equal(1,clientDevice.ObservationSec.Value);
            Assert.Equal(2,clientDevice.DgpsRate.Value);
            Assert.Equal(3,clientDevice.AllSatellites.Value);
            Assert.Equal(4,clientDevice.GalSatellites.Value);
            Assert.Equal(5,clientDevice.BeidouSatellites.Value);
            Assert.Equal(6,clientDevice.GlonassSatellites.Value);
            Assert.Equal(7,clientDevice.GpsSatellites.Value);
            Assert.Equal(8,clientDevice.QzssSatellites.Value);
            Assert.Equal(9,clientDevice.SbasSatellites.Value);
            Assert.Equal(10,clientDevice.ImesSatellites.Value);
            Assert.Equal(11,clientDevice.VehicleCount.Value);
            

        }
       

    }
}