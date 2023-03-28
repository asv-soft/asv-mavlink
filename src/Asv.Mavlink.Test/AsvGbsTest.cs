using System;
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
        [InlineData(0,  0, 0)]
        public async Task StatusTest(int lat,int lon,int alt)
        {
            var (client,server) = await MavlinkTestHelper.CreateServerAndClientDevices();

            var mode = AsvGbsCustomMode.AsvGbsCustomModeAuto;

            server.Heartbeat.Set(_ => _.CustomMode = (uint)mode);
            await server.Gbs.Set(_ =>
            {
                _.Lat = lat;
                _.Lng = lon;
                _.Alt = alt;
            });
            server.Gbs.Start(TimeSpan.FromSeconds(1));
            server.Heartbeat.Start();
            
            MavlinkTestHelper.WaitUntilConnect(client);
            await Task.Delay(2000);
            var clientState = await client.Heartbeat.RawHeartbeat.FirstAsync();
            Assert.Equal((uint)mode,clientState.CustomMode);

            var status = await client.Gbs.Status.FirstAsync();
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
            var (client,server) = await MavlinkTestHelper.CreateServerAndClientDevices();
            var mock = new Mock<IGbsClientDevice>();
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
            var serverDevice = new GbsServerDevice(mock.Object, server);
            var clientDevice = new GbsClientDevice(client);

       
            MavlinkTestHelper.WaitUntilConnect(client);
            await Task.Delay(2000);
            
            var res = await clientDevice.StartAutoMode(duration, accuracy, CancellationToken.None);
            Assert.Equal(result,res);
            Assert.True(called);


        }
       

    }
}