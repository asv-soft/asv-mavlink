using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using R3;
using TimeProviderExtensions;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.Features;

[TestSubject(typeof(MavlinkV2WrapFeature))]
public class MavlinkV2WrapFeatureTest
{
    private readonly ManualTimeProvider _time;
    private readonly PacketSequenceCalculator _seq;
    private readonly IVirtualConnection _link;

    public MavlinkV2WrapFeatureTest(ITestOutputHelper log)
    {
        _time = new ManualTimeProvider();
        _seq = new PacketSequenceCalculator();
        var loggerFactory = new TestLoggerFactory(log, _time, "TEST");
        var protocol = Protocol.Create(builder =>
        {
            builder.SetLog(loggerFactory);
            builder.SetTimeProvider(_time);
            builder.RegisterMavlinkV2Protocol();
            builder.Features.RegisterMavlinkV2WrapFeature();
        });
        _link = protocol.CreateVirtualConnection();
    }
    
    [Fact]
    public async Task WrapMessageFeature_Wrap_Success()
    {
        foreach (var id in MavlinkV2MessageFactory.Instance.GetSupportedIds())
        {
            var origin = MavlinkV2MessageFactory.Instance.Create(id) as MavlinkV2Message;
            Assert.NotNull(origin);
            var tcs = new TaskCompletionSource<MavlinkV2Message>();
            using var sub = _link.Server.RxFilterByType<MavlinkV2Message>().Where(x => x.Id == id)
                .Subscribe(x =>
                {
                    tcs.SetResult(x);
                });
            await _link.Client.Send(origin);
            await tcs.Task;
            var recv = tcs.Task.Result;
            if (origin.WrapToV2Extension)
            {
                Assert.IsType<V2ExtensionPacket>(recv);
            }
            else
            {
                Assert.Equal(origin.Id, recv.Id);
            }
        }

    }
}