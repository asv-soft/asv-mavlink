#nullable enable
using System;
using Asv.IO;

namespace Asv.Mavlink
{
    public interface IMavlinkMicroserviceClient: IMicroserviceClient
    {
        MavlinkClientIdentity Identity { get; }
        IMavlinkContext Core { get; }
    }
    
    public abstract class MavlinkMicroserviceClient: MicroserviceClient<MavlinkMessage>, IMavlinkMicroserviceClient
    {
        protected MavlinkMicroserviceClient(string microserviceTypeName, MavlinkClientIdentity identity,
            IMavlinkContext core) : base(core, $"{identity}.{microserviceTypeName}")
        {
            ArgumentNullException.ThrowIfNull(microserviceTypeName);
            Identity = identity ?? throw new ArgumentNullException(nameof(identity));
            Core = core ?? throw new ArgumentNullException(nameof(core));
            TypeName = microserviceTypeName;
        }

        public MavlinkClientIdentity Identity { get; }
        public IMavlinkContext Core { get; }
        public override string TypeName { get; }

        protected override bool FilterDeviceMessages(MavlinkMessage arg)
        {
            return arg.SystemId == Identity.Target.SystemId && arg.ComponentId == Identity.Target.ComponentId;
        }

        protected override void FillMessageBeforeSent(MavlinkMessage message)
        {
            message.SystemId = Identity.Self.SystemId;
            message.ComponentId = Identity.Self.ComponentId;
            message.Sequence = Core.Sequence.GetNextSequenceNumber();
        }
    }
}
