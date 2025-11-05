using System.Collections.Generic;

namespace Asv.Mavlink;

public interface IDroneFrame
{
    /// <summary>
    /// Unique identifier of the frame.
    /// </summary>
    public string Id { get; }
    
    /// <summary>
    /// Metadata with device-specific parameter information.
    /// </summary>
    public IReadOnlyDictionary<string, string>? Meta { get; }
}