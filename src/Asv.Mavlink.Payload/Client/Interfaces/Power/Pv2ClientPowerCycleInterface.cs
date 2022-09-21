using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;

namespace Asv.Mavlink.Payload
{
    public class Pv2ClientPowerCycleInterface : Pv2ClientInterfaceBase, IPv2PowerCycle
    {
        private readonly RxValue<bool> _canPowerOffSubject = new();
        private readonly RxValue<bool> _canRebootSubject = new();
        private readonly RxValue<bool> _isBootRequiredSubject = new();
        private readonly IPv2ClientParamsInterface _paramSvc;
        private readonly RxValue<PowerCycleFlags> _powerCycleFlags = new();


        public Pv2ClientPowerCycleInterface(IPayloadV2Client client, IPv2ClientParamsInterface paramSvc) : base(client,
            Pv2PowerInterface.InterfaceName)
        {
            _paramSvc = paramSvc;

            _paramSvc.CurrentAndThenOnUpdated
                .FilterFlags(Pv2PowerInterface.PowerCycleCompatibility)
                .Select(Pv2PowerInterface.ConvertPowerCycleCompatibility)
                .Subscribe(_powerCycleFlags.OnNext).DisposeItWith(Disposable);
            Disposable.Add(_powerCycleFlags);
            // _powerCycleFlags.OnNext(Pv2PowerInterface.ConvertPowerCycleCompatibility(_paramSvc.ReadFlagValue(Pv2PowerInterface.PowerCycleCompatibility)));
            _powerCycleFlags.Select(_ => (_ & PowerCycleFlags.IsRebootRequired) != 0).Subscribe(_isBootRequiredSubject)
                .DisposeItWith(Disposable);
            _powerCycleFlags.Select(_ => (_ & PowerCycleFlags.CanReboot) != 0).Subscribe(_canRebootSubject)
                .DisposeItWith(Disposable);
            _powerCycleFlags.Select(_ => (_ & PowerCycleFlags.CanPowerOff) != 0).Subscribe(_canPowerOffSubject)
                .DisposeItWith(Disposable);

            Disposable.Add(_isBootRequiredSubject);
            Disposable.Add(_canRebootSubject);
            Disposable.Add(_canPowerOffSubject);
        }

        public IRxValue<bool> IsRebootRequired => _isBootRequiredSubject;

        public IRxValue<bool> CanPowerOff => _canRebootSubject;

        public Task PowerOff(CancellationToken none)
        {
            return Client.Call(Pv2PowerInterface.PowerOff, SpanVoidType.Default, none);
        }

        public IRxValue<bool> CanReboot => _canPowerOffSubject;

        public Task Reboot(CancellationToken none)
        {
            return Client.Call(Pv2PowerInterface.Reboot, SpanVoidType.Default, none);
        }
    }
}
