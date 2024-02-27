using Asv.Common;

namespace Asv.Mavlink;

public interface ITraceStreamClient
{
    /// <summary>
    /// Gets the editable value for Name.
    /// </summary>
    /// <value>
    /// The editable value for Name.
    /// </value>
    IRxEditableValue<string> Name { get; }

    #region Rx Messages

    /// <summary>
    /// Gets the value of the OnDebugVectorMessage property.
    /// </summary>
    /// <remarks>
    /// The OnDebugVectorMessage property is used to obtain a reference to an IRxValue object that emits DebugVectorMessage values.
    /// </remarks>
    /// <returns>
    /// An IRxValue object that emits DebugVectorMessage values.
    /// </returns>
    IRxValue<DebugVectorMessage> OnDebugVectorMessage { get; }

    /// <summary>
    /// Gets the value of the OnMemoryVectorMessage property.
    /// </summary>
    /// <remarks>
    /// The OnMemoryVectorMessage property is used to obtain a reference to an IRxValue object that emits MemoryVectorMessage values.
    /// </remarks>
    /// <returns>
    /// An IRxValue object that emits MemoryVectorMessage values.
    /// </returns>
    IRxValue<MemoryVectorMessage> OnMemoryVectorMessage { get; }

    /// <summary>
    /// Gets the value of the OnNamedValueIntMessage property.
    /// </summary>
    /// <remarks>
    /// The OnNamedValueIntMessage property is used to obtain a reference to an IRxValue object that emits NamedValueIntMessage values.
    /// </remarks>
    /// <returns>
    /// An IRxValue object that emits NamedValueIntMessage values.
    /// </returns>
    IRxValue<NamedValueIntMessage> OnNamedValueIntMessage { get; }

    /// <summary>
    /// Gets the value of the OnNamedValueFloatMessage property.
    /// </summary>
    /// <remarks>
    /// The OnNamedValueFloatMessage property is used to obtain a reference to an IRxValue object that emits NamedValueFloatMessage values.
    /// </remarks>
    /// <returns>
    /// An IRxValue object that emits NamedValueFloatMessage values.
    /// </returns>
    IRxValue<NamedValueFloatMessage> OnNamedValueFloatMessage { get; }

    #endregion
}