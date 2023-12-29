namespace Asv.Mavlink.Shell
{
    /// <summary>
    /// Represents an interface for generating Mavlink messages.
    /// </summary>
    public interface IMavlinkGenerator
    {
        /// <summary>
        /// Generates a formatted string using the provided template and MavlinkProtocolModel.
        /// </summary>
        /// <param name="template">The template string to use for formatting.</param>
        /// <param name="model">The MavlinkProtocolModel object containing the data for template replacement.</param>
        /// <returns>
        /// The formatted string generated using the template and data from the MavlinkProtocolModel.
        /// </returns>
        string Generate(string template, MavlinkProtocolModel model);
    }
}
