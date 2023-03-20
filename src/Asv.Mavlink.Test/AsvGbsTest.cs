using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Server;
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
        [InlineData(12.12345,  12.12345, 12345)]
        [InlineData(0,  0, 0)]
        public async Task StatusTest(double lat,double lon,double alt)
        {
            var (client,server) = await MavlinkTestHelper.CreateServerAndClientDevices();
            var mock = new Mock<IAsvGbsClient>();
            var serverState = new RxValue<AsvGbsState>(AsvGbsState.AsvGbsStateIdleMode);
            var serverPos = new RxValue<GeoPoint>(new GeoPoint(lat,lon,alt));
            
            mock.Setup(_ => _.State).Returns(()=>serverState);
            mock.Setup(_ => _.Position).Returns(()=>serverPos);
            
            server.Gbs.Init(TimeSpan.FromSeconds(1),mock.Object);
           
            MavlinkTestHelper.WaitUntilConnect(client);
            await Task.Delay(2000);
            
            var clientState = await client.Gbs.State.FirstAsync();
            Assert.Equal(serverState.Value,clientState);
            
            var clientPos= await client.Gbs.Position.FirstAsync();
            
            Assert.Equal(serverPos.Value,clientPos);
            
        }

        [Theory]
        [InlineData(1,   1, MavResult.MavResultAccepted)]
        [InlineData(1000,   1000, MavResult.MavResultAccepted)]
        [InlineData(float.NaN,   float.NaN, MavResult.MavResultFailed)]
        public async Task CommandTest(float duration,float accuracy, MavResult result)
        {
            var (client,server) = await MavlinkTestHelper.CreateServerAndClientDevices();
            var mock = new Mock<IAsvGbsClient>();
            var serverState = new RxValue<AsvGbsState>(AsvGbsState.AsvGbsStateIdleMode);
            var serverPos = new RxValue<GeoPoint>(GeoPoint.Zero);
            var called = false;
            mock.Setup(_ => _.State).Returns(serverState);
            mock.Setup(_ => _.Position).Returns(serverPos);
            mock.Setup(_ => _.StartAutoMode(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((float dur,float acc,CancellationToken cancel)=>
                {
                    called = true;
                    Assert.Equal(duration,dur);
                    Assert.Equal(accuracy,acc);
                    return result;
                });
            
            server.Gbs.Init(TimeSpan.FromSeconds(1),mock.Object);

            MavlinkTestHelper.WaitUntilConnect(client);
            await Task.Delay(2000);
            
            var res = await client.Gbs.StartAutoMode(duration, accuracy, CancellationToken.None);
            Assert.Equal(result,res);
            Assert.True(called);
            
            
        }

    }
}