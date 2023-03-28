#nullable enable
using Asv.Common;
using Asv.IO;
using System;
using System.Reactive.Disposables;
using System.Threading;

namespace Asv.Mavlink
{
    public class MavlinkPortConfig
    {
        public string? ConnectionString { get; set; }
        public string? Name { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class MavlinkPortInfo
    {
        public Guid Id { get; internal set; }
        public string? Name { get; internal set; }
        public string? ConnectionString { get; internal set; }
        public PortState State { get; internal set; }
        public long RxBytes { get; internal set; }
        public long TxBytes { get; internal set; }
        public long RxPackets { get; internal set; }
        public long TxPackets { get; internal set; }
        public long SkipPackets { get; internal set; }
        public int DeserializationErrors { get; internal set; }
        public string? Description { get; internal set; }
        public PortType Type { get; internal set; }
        public Exception? LastException { get; internal set; }
        public bool? IsEnabled { get; internal set; }
    }

    

    public interface IMavlinkRouter: IMavlinkV2Connection
    {
        long RxBytes { get; }
        long TxBytes { get; }

        Guid AddPort(MavlinkPortConfig settings);
        IObservable<Guid> OnAddPort { get; }

        bool RemovePort(Guid id);
        IObservable<Guid> OnRemovePort { get; }

        Guid[] GetPorts();
        bool SetEnabled(Guid id, bool enabled);
        MavlinkPortInfo? GetInfo(Guid id);
        MavlinkPortConfig? GetConfig(Guid id);
        MavlinkPortConfig[] GetConfig();

        IObservable<Guid> OnConfigChanged { get; }
    }
}