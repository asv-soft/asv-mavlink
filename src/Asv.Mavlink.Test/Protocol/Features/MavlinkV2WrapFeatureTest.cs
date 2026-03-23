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
    private readonly ITestOutputHelper _log;
    private readonly ManualTimeProvider _time;
    private readonly PacketSequenceCalculator _seq;
    private readonly IVirtualConnection _link;
    private readonly IProtocolMessageFactory<MavlinkMessage, int> _messageFactory;

    public MavlinkV2WrapFeatureTest(ITestOutputHelper log)
    {
        _log = log;
        _time = new ManualTimeProvider();
        _seq = new PacketSequenceCalculator();
        _messageFactory = MavlinkV2Protocol.CreateMessageFactory();
        //var loggerFactory = new TestLoggerFactory(log, _time, "TEST");
        var protocol = Protocol.Create(builder =>
        {
            //builder.SetLog(loggerFactory);
            builder.SetTimeProvider(_time);
            builder.RegisterMavlinkV2Protocol(_messageFactory);
            builder.Features.RegisterMavlinkV2WrapFeature(_messageFactory);
        });
        _link = protocol.CreateVirtualConnection();
    }
    
    [Fact]
    public async Task WrapMessageFeature_Wrap_Success()
    {
        var counter = 0;
        var factory = _messageFactory;
        foreach (var id in factory.GetSupportedIds())
        {
            var origin = factory.Create(id) as MavlinkV2Message;
            Assert.NotNull(origin);
            origin.GetPayload().Randomize();
            
            var tcs = new TaskCompletionSource<MavlinkV2Message>();
            _link.Server.OnTxMessage.Subscribe(x =>
            {

            });
            using var sub = _link.Server.RxFilterByType<MavlinkV2Message>()
                .Subscribe(x => tcs.SetResult(x));
            
#pragma warning disable CS8604 // Possible null reference argument.
            await _link.Client.Send(origin);
#pragma warning restore CS8604 // Possible null reference argument.
            var recv = await tcs.Task;
            if (origin.WrapToV2Extension)
            {
                Assert.IsType<V2ExtensionPacket>(recv);
            }
            else
            {
                Assert.Equal(origin.Id, recv.Id);
            }
            counter++;
        }
        
        _log.WriteLine("Complete {0} messages", counter);

    }
}