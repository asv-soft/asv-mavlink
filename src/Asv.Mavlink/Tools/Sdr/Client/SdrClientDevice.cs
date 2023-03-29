using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvGbs;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink;

public class SdrClientDevice : DisposableOnceWithCancel, ISdrClientDevice
{
    private readonly IMavlinkClient _client;
    private readonly RxValue<AsvSdrCustomMode> _customMode;
    private const int DefaultAttemptCount = 3;

    public SdrClientDevice(IMavlinkClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _customMode = new RxValue<AsvSdrCustomMode>();
        _client.Heartbeat.RawHeartbeat.Select(_ => (AsvSdrCustomMode)_.CustomMode).Subscribe(_customMode)
            .DisposeItWith(Disposable);
    }

    public IRxValue<AsvSdrCustomMode> CustomMode => _customMode;

   
}