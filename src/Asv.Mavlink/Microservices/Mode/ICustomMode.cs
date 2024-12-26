using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;

namespace Asv.Mavlink;

public interface ICustomMode
{
    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    /// <returns>The name of the property.</returns>
    public string Name { get; }

    /// <summary>
    /// Gets the description of the property.
    /// </summary>
    /// <returns>
    /// A string representing the description.
    /// </returns>
    public string Description { get; }

    /// <summary>
    /// This flag indicates whether the vehicle can be set into this mode by the user command.
    /// You shouldn't show this mode in the user interface if this flag is true.
    /// </summary>
    /// <value>
    /// A boolean value that represents whether the vehicle can be set into this mode by the user command.
    /// </value>
    public bool InternalMode { get; }
    /// <summary>
    /// Get args 
    /// </summary>
    /// <param name="baseMode"></param>
    /// <param name="customMode"></param>
    /// <param name="customSubMode"></param>
    public void GetCommandLongArgs(out uint baseMode, out uint customMode, out uint customSubMode);
    public bool IsCurrentMode(HeartbeatPayload? hb);
    public bool IsCurrentMode(CommandLongPayload payload);
    void Fill(HeartbeatPayload hb);
}