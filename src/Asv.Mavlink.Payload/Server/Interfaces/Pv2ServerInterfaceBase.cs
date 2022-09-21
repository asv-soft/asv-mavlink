using System;
using System.Reactive.Subjects;
using Asv.Common;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink.Payload
{
    public class Pv2ServerInterfaceBase : DisposableOnceWithCancel
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Subject<(MavSeverity, string)> _logMessageSubject = new();
        private string _logLocalName;
        private string _logRecv;
        private string _logSend;

        public Pv2ServerInterfaceBase(IPayloadV2Server server, string ifcLogName)
        {
            if (string.IsNullOrWhiteSpace(ifcLogName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(ifcLogName));
            Server = server ?? throw new ArgumentNullException(nameof(server));
            InterfaceName = ifcLogName;
            Disposable.Add(_logMessageSubject);
        }

        protected string InterfaceName { get; }

        protected string LogLocalName =>
            _logLocalName ??= $"{Server.Server.Identity.SystemId}:{Server.Server.Identity.ComponentId}";

        protected string LogSend => _logSend ??= $"[{LogLocalName}.{InterfaceName}]=>";
        protected string LogRecv => _logRecv ??= $"[{LogLocalName}.{InterfaceName}]<=";

        public IPayloadV2Server Server { get; }

        public IObservable<(MavSeverity, string)> OnLogMessage => _logMessageSubject;

        protected void Log(MavSeverity type, string message)
        {
            switch (type)
            {
                case MavSeverity.MavSeverityEmergency:
                case MavSeverity.MavSeverityAlert:
                case MavSeverity.MavSeverityCritical:
                case MavSeverity.MavSeverityError:
                    Logger.Error($"{LogSend}:{message}");
                    break;
                case MavSeverity.MavSeverityWarning:
                    Logger.Warn($"{LogSend}:{message}");
                    break;
                case MavSeverity.MavSeverityNotice:
                case MavSeverity.MavSeverityInfo:
                    Logger.Info($"{LogSend}:{message}");
                    break;
                case MavSeverity.MavSeverityDebug:
                    Logger.Debug($"{LogSend}:{message}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            _logMessageSubject.OnNext((type, $"{InterfaceName}:{message}"));
        }

        protected void LogError(string message)
        {
            Log(MavSeverity.MavSeverityError, message);
        }

        protected void LogInfo(string message)
        {
            Log(MavSeverity.MavSeverityInfo, message);
        }

        protected void LogDebug(string message)
        {
            Log(MavSeverity.MavSeverityDebug, message);
        }
    }
}
