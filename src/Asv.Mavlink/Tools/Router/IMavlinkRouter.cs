#nullable enable
using Asv.IO;
using System;
using R3;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents the configuration for a MAVLink port.
    /// </summary>
    public class MavlinkPortConfig
    {
        /// <summary>
        /// Gets or sets the connection string used to connect to a database.
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the property is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// Gets or sets chance to lose current package
        /// </summary>
        public int PacketLossChance { get; set; } = 0; // !!!use it only for testing purposes to simulate packet loss!!!
    }

    /// <summary>
    /// Represents information about a MAVLink port.
    /// </summary>
    public class MavlinkPortInfo
    {
        /// <summary>
        /// Gets or sets the Id of the property.
        /// </summary>
        /// <value>
        /// The Id of the property.
        /// </value>
        public Guid Id { get; internal set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string? Name { get; internal set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string? ConnectionString { get; internal set; }

        /// Represents the state of a port.
        /// This property provides access to the current state of a port. It indicates whether the port is open, closed, or in some other state.
        /// @property {PortState} State - The state of the port.
        /// @access internal
        /// @since 1.0.0
        /// /
        public PortState State { get; internal set; }

        /// <summary>
        /// Gets or sets the number of bytes received.
        /// </summary>
        public long RxBytes { get; internal set; }

        /// <summary>
        /// Gets or sets the total number of bytes transmitted.
        /// </summary>
        /// <value>
        /// The total number of bytes transmitted.
        /// </value>
        public long TxBytes { get; internal set; }

        /// <summary>
        /// Gets or sets the number of received packets.
        /// </summary>
        /// <value>
        /// The number of received packets.
        /// </value>
        public long RxPackets { get; internal set; }

        /// <summary>
        /// Gets or sets the number of transmitted packets.
        /// </summary>
        /// <value>
        /// The total number of packets transmitted.
        /// </value>
        public long TxPackets { get; internal set; }

        /// <summary>
        /// Gets or sets the number of packets to skip.
        /// </summary>
        /// <value>
        /// The number of packets to skip.
        /// </value>
        public long SkipPackets { get; internal set; }

        /// <summary>
        /// Gets or sets the number of errors that occurred during deserialization.
        /// </summary>
        /// <remarks>
        /// This property is used to track the number of deserialization errors that occurred during the process.
        /// </remarks>
        /// <value>
        /// The number of deserialization errors.
        /// </value>
        public int DeserializationErrors { get; internal set; }

        /// <summary>
        /// Gets or sets the description of the property.
        /// </summary>
        /// <value>
        /// The description of the property.
        /// </value>
        /// <remarks>
        /// This property is used to provide additional information or context about the property.
        /// </remarks>
        public string? Description { get; internal set; }

        /// <summary>
        /// Gets or sets the type of the port.
        /// </summary>
        /// <value>
        /// The type of the port.
        /// </value>
        public PortType Type { get; internal set; }

        /// <summary>
        /// Gets or sets the last exception that occurred.
        /// </summary>
        /// <value>
        /// The last exception that occurred, or <c>null</c> if there is no exception.
        /// </value>
        public Exception? LastException { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property is enabled.
        /// </summary>
        /// <value>
        /// The value indicating whether the property is enabled. Null indicates that the property state is unknown.
        /// </value>
        public bool? IsEnabled { get; internal set; }
        
        /// <summary>
        /// Gets or sets value of the chance to lose packet 
        /// </summary>
        /// <value>
        /// The value indicating what is the chance to lose a packet. Zero indicates that you don't lose packet. 100 indicates that you lose all the packets
        /// </value>
        public int PacketLossChance { get; internal set; }
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
        Observable<Guid> OnAddPort { get; }
        /// <summary>
        /// Remove port
        /// </summary>
        /// <param name="id">Port id</param>
        /// <returns></returns>
        bool RemovePort(Guid id);
        /// <summary>
        /// Event for port removed
        /// </summary>
        Observable<Guid> OnRemovePort { get; }
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
        Observable<Guid> OnConfigChanged { get; }
    }
}