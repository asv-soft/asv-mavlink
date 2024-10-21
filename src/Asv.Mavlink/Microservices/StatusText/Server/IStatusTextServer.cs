using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink
{
    /// <summary>
    /// Represents a status text server that can be used to log messages with different severity levels.
    /// </summary>
    public interface IStatusTextServer:IMavlinkMicroserviceServer
    {
        /// <summary>
        /// Logs a message with the given severity.
        /// </summary>
        /// <param name="msg">(MavSeverity severity, string message)</param>
        /// <returns>True if the log operation was successful; otherwise, false.</returns>
        /// <remarks>
        /// The Log method logs the specified message with the given severity level. The severity level is defined by
        /// <paramref name="msg"/> which is a tuple containing the severity level and the message to be logged.
        /// This method returns true if the log operation was successful, otherwise false.
        /// </remarks>
        bool Log((MavSeverity severity, string message) msg);

        /// <summary>
        /// Logs a message with the specified severity level.
        /// </summary>
        /// <param name="severity">The severity level of the message.</param>
        /// <param name="message">The message to be logged.</param>
        /// <returns>
        /// Returns true if the message was successfully logged; otherwise, false.
        /// </returns>
        bool Log(MavSeverity severity, string message);
    }

    /// <summary>
    /// Helper class for logging status messages with different severity levels.
    /// </summary>
    public static class StatusTextLoggerHelper
    {
        /// <summary>
        /// Displays an alert message and logs it with the specified severity.
        /// </summary>
        /// <param name="src">The IStatusTextServer object.</param>
        /// <param name="message">The message to display and log.</param>
        /// <returns>True if the message was successfully displayed and logged; otherwise, false.</returns>
        public static bool Alert(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityAlert, message);
        }

        /// <summary>
        /// Logs a critical message using the given <paramref name="message"/> with the severity of MavSeverityCritical.
        /// </summary>
        /// <param name="src">The <see cref="IStatusTextServer"/> object to log the message.</param>
        /// <param name="message">The message to be logged.</param>
        /// <returns>True if the message is successfully logged; otherwise, false.</returns>
        public static bool Critical(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityCritical, message);
        }

        /// <summary>
        /// Logs a debug message using the specified IStatusTextServer instance.
        /// </summary>
        /// <param name="src">The IStatusTextServer instance.</param>
        /// <param name="message">The debug message to log.</param>
        /// <returns>Returns true if the debug message was successfully logged; otherwise, false.</returns>
        public static bool Debug(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityDebug, message);
        }

        /// <summary>
        /// Sends an emergency message with the given message text.
        /// </summary>
        /// <param name="src">The source <see cref="IStatusTextServer"/> object.</param>
        /// <param name="message">The message text to be sent as an emergency.</param>
        /// <returns>
        /// true if the emergency message was sent successfully; otherwise, false.
        /// </returns>
        public static bool Emergency(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityEmergency, message);
        }

        /// <summary>
        /// Logs an error message using the <see cref="IStatusTextServer"/> interface.
        /// </summary>
        /// <param name="src">The <see cref="IStatusTextServer"/> instance to log the error message.</param>
        /// <param name="message">The error message to be logged.</param>
        /// <returns>True if the error message is successfully logged; otherwise, false.</returns>
        public static bool Error(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityError, message);
        }

        /// <summary>
        /// Logs an informational message using the <see cref="MavSeverity.MavSeverityInfo"/> severity level.
        /// </summary>
        /// <param name="src">The source object that implements the <see cref="IStatusTextServer"/> interface.</param>
        /// <param name="message">The message to be logged.</param>
        /// <returns>A boolean value indicating whether the logging operation was successful or not.</returns>
        public static bool Info(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityInfo, message);
        }

        /// <summary>
        /// Sends a notice message to the server's log using the specified message.
        /// </summary>
        /// <param name="src">The <see cref="IStatusTextServer"/> instance.</param>
        /// <param name="message">The notice message to be sent to the log.</param>
        /// <returns>A boolean value indicating if the notice message was successfully sent to the log.</returns>
        public static bool Notice(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityNotice, message);
        }

        /// <summary>
        /// Method to send a warning message using the status text server. </summary> <param name="src">The status text server.</param> <param name="message">The warning message.</param> <returns>True if the message is successfully logged; otherwise, false.</returns>
        /// /
        public static bool Warning(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityWarning, message);
        }
    }
}
