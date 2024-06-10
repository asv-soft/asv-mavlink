using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;

namespace Asv.Mavlink;

public interface IRfsaServer
{
    IReadOnlyDictionary<ushort, SignalInfo> Signals { get; }
    Task Send(DateTime time, ReadOnlyMemory<float> data, SignalInfo info, CancellationToken cancel = default);
}

