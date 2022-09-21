using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink.Server;

namespace Asv.Mavlink.Payload
{
    public class Pv2ServerPowerInterface : Pv2ServerInterfaceBase, IPv2ServerPowerInterface
    {
        private readonly Subject<Unit> _onPowerOffCommand = new();
        private readonly Subject<Unit> _onRebootCommand = new();
        private readonly IPv2ServerParamsInterface _paramsSvc;
        private readonly object _sync = new();

        public Pv2ServerPowerInterface(IPayloadV2Server server, IPv2ServerParamsInterface paramsSvc) : base(server,
            Pv2PowerInterface.InterfaceName)
        {
            _paramsSvc = paramsSvc;
            _paramsSvc.WriteFlagValue(Pv2PowerInterface.PowerCycleCompatibility, new Asv.Common.UintBitArray(0, 3));
            _paramsSvc.OnRemoteUpdated.Where(_ =>
                    _.Type.Flags.HasFlag(Pv2ParamFlags.RebootRequired) &&
                    GetPowerCycleFlag(PowerCycleFlags.IsRebootRequired) == false)
                .Subscribe(_ => SetPowerCycleFlag(PowerCycleFlags.IsRebootRequired, true)).DisposeItWith(Disposable);

            Server.Register(Pv2PowerInterface.PowerOff, PowerOff);
            Server.Register(Pv2PowerInterface.Reboot, Reboot);

            Disposable.Add(_onPowerOffCommand);
            Disposable.Add(_onRebootCommand);
        }

        public void SetCanReboot(bool value)
        {
            SetPowerCycleFlag(PowerCycleFlags.CanReboot, value);
        }

        public void SetCanPowerOff(bool value)
        {
            SetPowerCycleFlag(PowerCycleFlags.CanPowerOff, value);
        }

        public IObservable<Unit> OnRebootCommand => _onRebootCommand;
        public IObservable<Unit> OnPowerOffCommand => _onPowerOffCommand;

        private Task<(SpanVoidType, DeviceIdentity)> Reboot(DeviceIdentity devid, SpanVoidType data,
            CancellationToken cancel)
        {
            if (GetPowerCycleFlag(PowerCycleFlags.CanReboot))
            {
                LogInfo("Reboot...");
                _onRebootCommand.OnNext(Unit.Default);
            }
            else
            {
                LogError("Reboot rejected: not available now");
            }

            return Task.FromResult((SpanVoidType.Default, devid));
        }

        private Task<(SpanVoidType, DeviceIdentity)> PowerOff(DeviceIdentity devid, SpanVoidType data,
            CancellationToken cancel)
        {
            if (GetPowerCycleFlag(PowerCycleFlags.CanPowerOff))
            {
                LogInfo("Power off...");
                _onPowerOffCommand.OnNext(Unit.Default);
            }
            else
            {
                LogError("Power off rejected: not available now");
            }

            return Task.FromResult((SpanVoidType.Default, devid));
        }

        private bool GetPowerCycleFlag(PowerCycleFlags flag)
        {
            var bits = _paramsSvc.ReadFlagValue(Pv2PowerInterface.PowerCycleCompatibility);
            var flags = (PowerCycleFlags)bits.Value;
            return (flags & flag) != 0;
        }

        private void SetPowerCycleFlag(PowerCycleFlags flag, bool value)
        {
            lock (_sync)
            {
                var bits = _paramsSvc.ReadFlagValue(Pv2PowerInterface.PowerCycleCompatibility);
                var flags = (PowerCycleFlags)bits.Value;
                if (value)
                    flags |= flag;
                else
                    flags &= flag;
                _paramsSvc.WriteFlagValue(Pv2PowerInterface.PowerCycleCompatibility,
                    new Asv.Common.UintBitArray((uint)flags, bits.Size));
            }
        }
    }

    public interface IPv2ServerPowerInterface
    {
        IObservable<Unit> OnRebootCommand { get; }
        IObservable<Unit> OnPowerOffCommand { get; }
        void SetCanReboot(bool value);
        void SetCanPowerOff(bool value);
    }
}
