using System.Threading;
using System.Threading.Tasks;
using Asv.Common;

namespace Asv.Mavlink.Payload
{
    public abstract class Pv2ClientInterfaceBase : DisposableOnceWithCancel
    {
        private readonly string _ifcLogName;
        private string _locTargetName;
        private string _logLocalName;
        private string _logRecv;
        private string _logSend;

        protected Pv2ClientInterfaceBase(IPayloadV2Client client, string ifcLogName)
        {
            Client = client;
            _ifcLogName = ifcLogName;
        }

        public IPayloadV2Client Client { get; }

        protected string LogTargetName => _locTargetName ??=
            $"{Client.Client.Identity.TargetSystemId}:{Client.Client.Identity.TargetSystemId}";

        protected string LogLocalName =>
            _logLocalName ??= $"{Client.Client.Identity.SystemId}:{Client.Client.Identity.ComponentId}";

        protected string LogSend => _logSend ??= $"[{LogLocalName}]=>[{LogTargetName}][{_ifcLogName}]:";
        protected string LogRecv => _logRecv ??= $"[{LogLocalName}]<=[{LogTargetName}][{_ifcLogName}]:";

        public virtual Task ReloadWhenDisconnected(CancellationToken cancel)
        {
            return Task.CompletedTask;
        }
    }
}
