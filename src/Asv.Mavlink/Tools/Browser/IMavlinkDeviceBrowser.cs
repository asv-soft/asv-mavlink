using System;
using ObservableCollections;
using R3;

namespace Asv.Mavlink
{
    /// <summary>
    /// Device browser in mavlink network
    /// </summary>
    public interface IMavlinkDeviceBrowser
    {
        IReadOnlyObservableDictionary<MavlinkIdentity, MavlinkDevice> Devices { get; }
        /// <summary>
        /// Current device timeout
        /// </summary>
        ReactiveProperty<TimeSpan> DeviceTimeout { get; }
    }



    
}