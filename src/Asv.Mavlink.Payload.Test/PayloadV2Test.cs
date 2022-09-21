using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Server;
using Xunit;

namespace Asv.Mavlink.Payload.Test
{
    public class PayloadV2Test
    {

        public static MethodInfo<SpanPacketIntegerType, SpanPacketIntegerType> TestMethod1 = new("TestMethod1", 1, "Test0", 0);
        public static MethodInfo<SpanPacketIntegerType, SpanPacketIntegerType> TestMethod2 = new("TestMethod2", 0, "Test1", 1);

       

        [Fact]
        public void TestFailRegistration()
        {
            var server = PayloadV2TestHelper.CreateServer(out var port);
            var client = PayloadV2TestHelper.CreateClient(port);

            client.Client.Heartbeat.Link.Where(_ => _ == LinkState.Connected).FirstAsync().Wait();

            server.Register(TestMethod1, (devId, data, _) => Task.FromResult((new SpanPacketIntegerType { Value = data.Value * 2 },devId)));
            Assert.Throws<Exception>(() =>
            {
                server.Register(TestMethod1, (devId, data, _) => Task.FromResult((new SpanPacketIntegerType { Value = data.Value * 2 }, devId)));
            });
        }

        [Fact]
        public async Task TestCustomMethod()
        {
            var server = PayloadV2TestHelper.CreateServer(out var port);
            var client = PayloadV2TestHelper.CreateClient(port);
            PayloadV2TestHelper.WaitUntilConnect(client);

            server.Register(TestMethod1, (id, data, cancel) => Task.FromResult((new SpanPacketIntegerType { Value = data.Value * 2 }, id)));
            server.Register(TestMethod2, (id, data, cancel) => Task.FromResult((new SpanPacketIntegerType { Value = data.Value * 3 }, id)));

            var result =await  client.Call(TestMethod1, new SpanPacketIntegerType { Value = 10 }, CancellationToken.None);
            var result2 = await client.Call(TestMethod2, new SpanPacketIntegerType { Value = 10 }, CancellationToken.None);
            Assert.Equal(20,result.Value);
            Assert.Equal(30, result2.Value);
        }

        [Fact]
        public async Task TestCustomMethodWithRemoteError()
        {
            var server = PayloadV2TestHelper.CreateServer(out var port);
            var client = PayloadV2TestHelper.CreateClient(port);

            PayloadV2TestHelper.WaitUntilConnect(client);

            server.Register(TestMethod1, (id, data, cancel) => throw new Exception("ExceptionMessage1"));

            await Assert.ThrowsAsync<InternalPv2Exception>(async () =>
            {
                try
                {
                    var result = await client.Call(TestMethod1, new SpanPacketIntegerType { Value = 10 }, CancellationToken.None);
                }
                catch (InternalPv2Exception e)
                {
                    Assert.Equal("ExceptionMessage1", e.RemoteErrorMessage);
                    throw;
                }
                
            });
        }

        [Fact]
        public async Task TestTimeoutWnenWrongTarget()
        {
            var server = PayloadV2TestHelper.CreateServer(out var port);
            var client = PayloadV2TestHelper.CreateClient(port);

            PayloadV2TestHelper.WaitUntilConnect(client);

            server.Register(TestMethod1, (id, data, cancel) => Task.FromResult((new SpanPacketIntegerType { Value = data.Value * 2 }, new DeviceIdentity{ComponentId = 1,SystemId = 3})));

            await Assert.ThrowsAsync<TimeoutException>( async () =>
            {
                var result = await client.Call(TestMethod1, new SpanPacketIntegerType { Value = 10 }, CancellationToken.None);
            });
        }

        [Fact]
        public async Task TestRequestId()
        {
            var server = PayloadV2TestHelper.CreateServer(out var port);
            var client = PayloadV2TestHelper.CreateClient(port);

            PayloadV2TestHelper.WaitUntilConnect(client);

            server.Register(TestMethod1, async (id, data, cancel) =>
            {
                await Task.Delay(data.Value,cancel);
                return (new SpanPacketIntegerType { Value = data.Value }, id);
            });

            var longTimeResult = client.Call(TestMethod1, new SpanPacketIntegerType { Value = 100 }, CancellationToken.None);
            var fastResult = client.Call(TestMethod1, new SpanPacketIntegerType { Value = 10 }, CancellationToken.None);
            await Task.WhenAny(fastResult, longTimeResult);
            var result = fastResult.Result;
            Assert.Equal(10,fastResult.Result.Value);
            Assert.False(longTimeResult.IsCompleted);
            await longTimeResult;
            Assert.Equal(100, longTimeResult.Result.Value);
            
        }


        [Fact]
        public void PingTest()
        {
            var server = PayloadV2TestHelper.CreateServer(out var port);
            var client = PayloadV2TestHelper.CreateClient(port);

            var task = client.Client.Heartbeat.Link.FirstAsync(_ => _ == LinkState.Connected).ToTask();
            task.Wait(TimeSpan.FromSeconds(10));
            Assert.Equal(LinkState.Connected, task.Result);
            server.Dispose();
            task = client.Client.Heartbeat.Link.FirstAsync(_ => _ == LinkState.Disconnected).ToTask();
            task.Wait(TimeSpan.FromSeconds(10));
        }
    }
}
