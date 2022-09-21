using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using DynamicData;

namespace Asv.Mavlink.Payload
{
    public interface IPv2ClientBaseInterface
    {
        IRxValue<Pv2DeviceInfo> Info { get; }
        IObservable<IChangeSet<Pv2WorkModeInfo, byte>> AllWorkModes { get; }
        IRxValue<Pv2WorkModeStatusInfo> WorkModeStatus { get; }
        IRxValue<Pv2WorkModeInfo> WorkMode { get; }
        IRxValue<string> Name { get; }

        Task SetWorkMode(byte id, CancellationToken cancel = default);
    }
}
