using System.Threading.Tasks;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a debugging server interface.
    /// </summary>
    public interface IDebugServer
    {
        /// <summary>
        /// Gets the maximum length of the name for a debug float array.
        /// </summary>
        /// <remarks>
        /// The MaxDebugFloatArrayNameLength property represents the maximum number
        /// of characters allowed for the name of a debug float array. Any name
        /// exceeding this length will be truncated.
        /// </remarks>
        /// <value>
        /// An integer representing the maximum length of the name for a debug
        /// float array.
        /// </value>
        int MaxDebugFloatArrayNameLength { get; }

        /// <summary>
        /// Gets the maximum length of debug float array data.
        /// </summary>
        /// <remarks>
        /// The maximum length of debug float array data is determined by the value of this property.
        /// </remarks>
        /// <value>
        /// The maximum length of debug float array data.
        /// </value>
        int MaxDebugFloatArrayDataLength { get; }

        /// <summary>
        /// Gets the maximum length of the memory vector.
        /// </summary>
        /// <value>
        /// The maximum length of the memory vector.
        /// </value>
        int MaxMemoryVectLength { get; }

        /// <summary>
        /// A property that represents the maximum length of a named value key.
        /// </summary>
        /// <remarks>
        /// This value represents the maximum number of characters a named value key can have.
        /// </remarks>
        int MaxNamedValueKeyLength { get; }

        /// <summary>
        /// Sends a float array for debugging purposes.
        /// </summary>
        /// <param name="name">The name of the float array.</param>
        /// <param name="arrayId">The ID of the float array.</param>
        /// <param name="data">The float data array to send.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method sends a float array to the debug console for debugging purposes.
        /// The <paramref name="name"/> parameter specifies the name of the float array,
        /// which will be displayed in the debug console. The <paramref name="arrayId"/>
        /// parameter is used to identify the float array. The <paramref name="data"/>
        /// parameter contains the actual float data to be sent.
        /// </remarks>
        /// Example Usage:
        /// <code>
        /// float[] myFloatArray = new float[] { 1.0f, 2.0f, 3.0f };
        /// await SendDebugFloatArray("MyArray", 1, myFloatArray);
        /// </code>
        Task SendDebugFloatArray(string name, ushort arrayId, float[] data);

        /// <summary>
        /// Sends a memory vector to a specific address with given version, type, and value.
        /// </summary>
        /// <param name="address">The address to send the memory vector to.</param>
        /// <param name="version">The version of the memory vector.</param>
        /// <param name="type">The type of the memory vector.</param>
        /// <param name="value">The array of signed bytes representing the value of the memory vector.</param>
        /// <returns>A task representing the asynchronous send operation.</returns>
        Task SendMemoryVect(ushort address, byte version, byte type, sbyte[] value);

        /// <summary>
        /// Sends a named value as a float to a specific destination.
        /// </summary>
        /// <param name="name">The name of the value to be sent.</param>
        /// <param name="value">The float value to be sent.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task SendNamedValueFloat(string name, float value);

        /// <summary>
        /// Sends a named integer value.
        /// </summary>
        /// <param name="name">The name of the value to send.</param>
        /// <param name="value">The integer value to send.</param>
        /// <returns>
        /// A task representing the asynchronous send operation.
        /// </returns>
        Task SendNamedValueInteger(string name, int value);
    }
}