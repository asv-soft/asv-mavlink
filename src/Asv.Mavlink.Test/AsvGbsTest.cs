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
        [InlineData(1,   1, 1)]
        public async Task StatusTest(double lat,double lon,double alt)
        {
            var (client,server) = await MavlinkTestHelper.CreateServerAndClientDevices();
            var mock = new Mock<IAsvGbsClient>();
            var serverState = new RxValue<AsvGbsState>(AsvGbsState.AsvGbsStateIdleMode);
            var serverPos = new RxValue<GeoPoint>(new GeoPoint(lat,lon,alt));
            
            mock.Setup(_ => _.State).Returns(serverState);
            mock.Setup(_ => _.Position).Returns(serverPos);
            
            server.Gbs.Init(TimeSpan.FromSeconds(1),mock.Object);
           
            MavlinkTestHelper.WaitUntilConnect(client);
            await Task.Delay(2000);
            
            var clientState = await client.Gbs.State.Skip(3).FirstAsync();
            Assert.Equal(serverState.Value,clientState);
            
            var clientPos= await client.Gbs.Position.FirstAsync();
            
            Assert.Equal(serverPos.Value,clientPos);
            
        }

        [Theory]
        [InlineData(1,   1)]
        [InlineData(1000,   1000)]
        [InlineData(float.NaN,   float.NaN)]
        public async Task CommandTest(float duration,float accuracy)
        {
            
        }

    }
}