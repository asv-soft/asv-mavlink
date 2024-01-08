using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

/// <summary>
/// Represents a parameter item.
/// </summary>
public interface IParamItem
{
    /// <summary>
    /// Gets the index value.
    /// </summary>
    /// <returns>
    /// The index value.
    /// </returns>
    ushort Index { get;}

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    string Name { get; }

    /// <summary>
    /// Gets the type of the MavParam.
    /// </summary>
    /// <returns>
    /// The MavParamType of the MavParam.
    /// </returns>
    MavParamType Type { get; }

    /// <summary>
    /// Gets the description of the property Info.
    /// </summary>
    /// <value>
    /// The <see cref="ParamDescription"/> of the property Info.
    /// </value>
    ParamDescription Info { get; }

    /// <summary>
    /// Gets the asynchronous Rx value indicating whether the data is synchronized or not.
    /// </summary>
    /// <value>
    /// The <see cref="IRxValue{T}"/> that represents whether the data is synchronized or not.
    /// </value>
    IRxValue<bool> IsSynced { get; }

    /// Gets the value of the property.
    /// @returns The value of the property as an instance of IRxEditableValue<MavParamValue>.
    /// /
    IRxEditableValue<MavParamValue> Value { get; }

    /// <summary>
    /// Reads data asynchronously from a source.
    /// </summary>
    /// <param name="cancel">Cancellation token for aborting the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous read operation.</returns>
    Task Read(CancellationToken cancel = default);

    /// <summary>
    /// Writes the data to a destination according to the specified cancelationToken.
    /// </summary>
    /// <param name="cancel">The cancelationToken to observe cancellation requests.</param>
    /// <returns>A task representing the asynchronous writing operation.</returns>
    Task Write(CancellationToken cancel = default);
    
}