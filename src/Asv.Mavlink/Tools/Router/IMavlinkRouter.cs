#nullable enable
using Asv.IO;
using System;

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

    

    /// <summary>
    /// Used for routing mavlink packets between ports
    /// </summary>
    public interface IMavlinkRouter: IMavlinkV2Connection
    {
        /// <summary>
        /// Total received bytes
        /// </summary>
        long RxBytes { get; }
        /// <summary>
        /// Total transmitted bytes
        /// </summary>
        long TxBytes { get; }
        /// <summary>
        /// Add new port
        /// </summary>
        /// <param name="settings">Port settings</param>
        /// <returns></returns>
        Guid AddPort(MavlinkPortConfig settings);
        /// <summary>
        /// Event for new port added
        /// </summary>
        IObservable<Guid> OnAddPort { get; }
        /// <summary>
        /// Remove port
        /// </summary>
        /// <param name="id">Port id</param>
        /// <returns></returns>
        bool RemovePort(Guid id);
        /// <summary>
        /// Event for port removed
        /// </summary>
        IObservable<Guid> OnRemovePort { get; }
        /// <summary>
        /// Return all ports
        /// </summary>
        /// <returns></returns>
        Guid[] GetPorts();
        /// <summary>
        /// Enable/disable port
        /// </summary>
        /// <param name="id">Port id</param>
        /// <param name="enabled">Enable\disable port</param>
        /// <returns></returns>
        bool SetEnabled(Guid id, bool enabled);
        /// <summary>
        /// Return port info
        /// </summary>
        /// <param name="id">Port id</param>
        /// <returns></returns>
        MavlinkPortInfo? GetInfo(Guid id);
        /// <summary>
        /// Return specifed ports config for save it to persistent storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MavlinkPortConfig? GetConfig(Guid id);
        /// <summary>
        /// Return all ports config for save it to persistent storage
        /// </summary>
        /// <returns></returns>
        MavlinkPortConfig[] GetConfig();
        /// <summary>
        /// Event for port config changed
        /// </summary>
        IObservable<Guid> OnConfigChanged { get; }
    }
}