using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink.Server
{
    public interface IStatusTextServer
    {
        bool Log((MavSeverity severity, string message) msg);
        bool Log(MavSeverity severity, string message);
    }

    public static class StatusTextLoggerHelper
    {
        public static bool Alert(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityAlert, message);
        }
        public static bool Critical(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityCritical, message);
        }
        public static bool Debug(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityDebug, message);
        }
        public static bool Emergency(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityEmergency, message);
        }
        public static bool Error(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityError, message);
        }
        public static bool Info(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityInfo, message);
        }
        public static bool Notice(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityNotice, message);
        }
        public static bool Warning(this IStatusTextServer src, string message)
        {
            return src.Log(MavSeverity.MavSeverityWarning, message);
        }
    }
}
