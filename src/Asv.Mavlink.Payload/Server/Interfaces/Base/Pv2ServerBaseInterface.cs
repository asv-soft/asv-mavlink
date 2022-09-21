using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Server;
using Asv.Mavlink.V2.Minimal;

namespace Asv.Mavlink.Payload
{
    public class Pv2ServerBaseInterface : Pv2ServerInterfaceBase, IPv2ServerBaseInterface
    {
        private const byte IdleModeIndex = 0;

        private readonly Pv2DeviceClass _deviceClass;

        private readonly WorkModeIdle _idleMode = new();
        private readonly List<IWorkModeFactory> _modes;
        private readonly IPayloadV2Server _srv;
        private readonly Dictionary<ushort, byte> _workModeStatusIndexes = new();
        private IWorkMode _currentMode;
        private byte _currentModeIndex;
        private IDisposable _modeStatusSubscribe;
        private int _state;
        private readonly Pv2DeviceCompatibilityFlags _flags;
        private Pv2DeviceInfo _deviceInfo;
        private readonly uint _descriptionHash;

        public Pv2ServerBaseInterface(IPayloadV2Server srv, Pv2DeviceClass deviceClass,
            IEnumerable<IWorkModeFactory> workModes,
            Pv2DeviceCompatibilityFlags flags = Pv2DeviceCompatibilityFlags.NoFlags)
            : base(srv, Pv2BaseInterface.InterfaceName)
        {
            Logger.Info($"{InterfaceName}: Loading device class '{deviceClass:G}' with flags {flags:F}");

            _srv = srv;
            _deviceClass = deviceClass;
            _flags = flags;

            _modes = InitModes(workModes, out _descriptionHash);
            Logger.Info($"Found {_modes.Count} modes. Desc hash:{_descriptionHash}");

            _idleMode.SetOkStatus();
            InternalUpdateMode(_idleMode, IdleModeIndex);
            srv.Server.Heartbeat.Start();

            srv.Register(Pv2BaseInterface.SetWorkMode, SetMode);
            srv.Register(Pv2BaseInterface.GetWorkModeList, GetModes);
            srv.Register(Pv2BaseInterface.GetWorkModeInfo, GetModeInfo);
            srv.Register(Pv2BaseInterface.GetModeStatusInfo, GetModeStatusInfo);
        }

        #region Work mode

        private List<IWorkModeFactory> InitModes(IEnumerable<IWorkModeFactory> workModes, out uint descHash)
        {
            var modes = new List<IWorkModeFactory>
            {
                // default modes : IDLE
                _idleMode
            };
            modes.AddRange(workModes.OrderBy(_=>_.Name));
            

            if (modes.Count > Pv2WorkModeInfo.MaxWorkModeCount)
                throw new Exception($"Work mode count too big. Max count {Pv2WorkModeInfo.MaxWorkModeCount}");


            for (byte modeIndex = 0; modeIndex < modes.Count; modeIndex++)
            {
                var mode = modes[modeIndex];
                
                var notUniqueGroups = mode.AvailableStatus.GroupBy(_ => _.Id).FirstOrDefault(_ => _.Count() > 1);
                if (notUniqueGroups != null)
                    throw new Exception(
                        $"Work mode status items for {mode.Name} not unique [ID={notUniqueGroups.Key}]");

                if (mode.AvailableStatus.Length > Pv2WorkModeInfo.MaxWorkModeStatusCount)
                    throw new Exception(
                        $"Work mode status count for {mode.Name} too big. Max count {Pv2WorkModeInfo.MaxWorkModeStatusCount}");
                for (byte statusIndex = 0; statusIndex < mode.AvailableStatus.Length; statusIndex++)
                {
                    var status = mode.AvailableStatus[statusIndex];
                    _workModeStatusIndexes.Add(Pv2BaseInterface.CombineId(modeIndex, status.Id), statusIndex);
                }

                Logger.Info(
                    $"{InterfaceName}: Loading work mode '{mode.Name}' with status {string.Join(",", mode.AvailableStatus.Select(_ => $"{_.Name}[{_.Class:G}]"))}");
            }

            var modesDesc = modes.Select((modeId, inx) => new Pv2WorkModeInfo((byte)inx, modeId));
            
            var statusDesc = modes.SelectMany(_ => _.AvailableStatus.OrderBy(__ => __.Id)).Select((_, inx) => new Pv2WorkModeStatusInfo
            {
                Class = _.Class,
                Description = _.Description,
                Name = _.Name,
                Id = (byte)inx,
            });
            descHash = Pv2BaseInterface.CalculateHash(modesDesc, statusDesc);

            return modes;
        }

        public void UpdateDeviceInfo(Action<Pv2DeviceInfo> editCallback)
        {
            var status = _workModeStatusIndexes[Pv2BaseInterface.CombineId(_currentModeIndex, _currentMode.Status.Value.Id)];
            _deviceInfo ??= new Pv2DeviceInfo(0)
            {
                Class = _deviceClass,
                WorkMode = _currentModeIndex,
                WorkModeStatus = status,
                Compatibility = _flags
            };
            editCallback?.Invoke(_deviceInfo);
            _srv.Server.Heartbeat.Set(_ =>
            {
                _.Autopilot = MavAutopilot.MavAutopilotInvalid;
                _.Type = MavType.MavTypeOnboardController;
                _.SystemStatus = MavState.MavStateActive;
                _.MavlinkVersion = 3;
                _.BaseMode = MavModeFlag.MavModeFlagCustomModeEnabled;
                _.CustomMode = _deviceInfo.CustomMode;
            });
        }

        private void InternalUpdateMode(IWorkMode mode, byte modeIndex)
        {
            if (_currentMode != null)
                Logger.Info($"{InterfaceName}: Change work mode to '{_currentMode.Name}' => '{mode.Name}'");
            else
                Logger.Info($"{InterfaceName}: Init work mode '{mode.Name}'");

            _currentMode = mode;
            _currentModeIndex = modeIndex;
            UpdateDeviceInfo(_ =>
            {
                _.WorkMode = modeIndex;
                _.WorkModeStatus = mode.Status.Value.Id;
            });
        }

        private Task<(Pv2WorkModeInfo, DeviceIdentity)> GetModeInfo(DeviceIdentity devid, SpanByteType modeId,
            CancellationToken cancel)
        {
            try
            {
                var mode = CheckAndGetModeByIndex(modeId.Value);
                return Task.FromResult((new Pv2WorkModeInfo(modeId.Value,mode), devid));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private Task<(WorkModeListType, DeviceIdentity)> GetModes(DeviceIdentity devid, SpanVoidType data,
            CancellationToken cancel)
        {
            
            
            var result = new WorkModeListType
            {
                DescHash = _descriptionHash,
                WorkModes = new SpanByteArrayType(_modes.Select(_ => (byte)_.AvailableStatus.Length))
            };
            
            return Task.FromResult((result, devid));
        }

        public IEnumerable<IWorkModeFactory> Modes => _modes;

        public async Task SetMode(byte modeIndex, CancellationToken cancel = default)
        {
            var mode = CheckAndGetModeByIndex(modeIndex);
            await InternalChangeModeAsync(mode, modeIndex).ConfigureAwait(false);
        }

        public Task SetMode(IWorkModeFactory mode)
        {
            var inx = _modes.IndexOf(mode);
            if (inx < 0)
                throw new Exception($"Work mode {mode.Name} not found");
            return InternalChangeModeAsync(mode, (byte)inx);
        }
        
        private Task<(SpanVoidType, DeviceIdentity)> SetMode(DeviceIdentity devid, SpanByteType modeId,
            CancellationToken cancel)
        {
            var mode = CheckAndGetModeByIndex(modeId.Value);
            Task.Factory.StartNew(() => InternalChangeModeAsync(mode, modeId.Value), cancel);
            return Task.FromResult((SpanVoidType.Default, devid));
        }

        private Task<(Pv2WorkModeStatusInfo, DeviceIdentity)> GetModeStatusInfo(DeviceIdentity devid,
            SpanDoubleByteType modeId, CancellationToken cancel)
        {
            var mode = CheckAndGetModeByIndex(modeId.Value1);
            if (modeId.Value2 >= mode.AvailableStatus.Length)
                throw new Exception($"Info with id='{modeId.Value2}' not exist in {mode.Name} mode");

            var status = mode.AvailableStatus[modeId.Value2];
            return Task.FromResult((new Pv2WorkModeStatusInfo
            {
                Description = status.Description,
                Name = status.Name,
                Class = status.Class,
                Id = modeId.Value2
            }, devid));
        }

        private IWorkModeFactory CheckAndGetModeByIndex(byte mode)
        {
            if (mode >= _modes.Count) throw new Exception($"Mode with id='{mode}' not exist");
            return _modes[mode];
        }

        private async Task InternalChangeModeAsync(IWorkModeFactory modeFactory, byte modeIndex)
        {
            if (Interlocked.CompareExchange(ref _state, 1, 0) != 0)
            {
                Logger.Warn($"Dublicate set mode {modeFactory.Name} [{modeIndex}]");
                Debug.Assert(false);
                return;
            }

            try
            {
                var oldMode = _currentMode;
                _idleMode.SetLoadingStatus();
                InternalUpdateMode(_idleMode, IdleModeIndex);
                _modeStatusSubscribe?.Dispose();
                oldMode.Dispose();
                var newMode = modeFactory.Create();
                await newMode.Init().ConfigureAwait(false);
                _idleMode.SetOkStatus();
                InternalUpdateMode(newMode, modeIndex);
                _modeStatusSubscribe = newMode.Status.Subscribe(_ => InternalUpdateMode(newMode, modeIndex));
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error to switch mode:{e.Message}");
                _idleMode.SetErrorStatus();
                InternalUpdateMode(_idleMode, IdleModeIndex);
            }
            finally
            {
                Interlocked.Exchange(ref _state, 0);
            }
        }

        #endregion
    }
}
