using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.Common;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(TelemetryClientEx))]
public class TelemetryClientExTest : ClientTestBase<TelemetryClientEx>
{
    private readonly TaskCompletionSource<double> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TelemetryClientEx _client;
    
    public TelemetryClientExTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _taskCompletionSource = new TaskCompletionSource<double>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }

    protected override TelemetryClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core)
    {
        return new TelemetryClientEx(new TelemetryClient(identity, core));
    }

    [Fact]
    public async Task Init_ProperInput_Success()
    {
        await _client.Init();
    }

    [Theory]
    [InlineData(sbyte.MaxValue)]
    [InlineData(sbyte.MinValue)]
    public async Task ReceiveBatteryCharge_SinglePacket_Success(sbyte batteryRemaining)
    {
        var called = 0;
        var isInit = true;
        var packet = new SysStatusPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                OnboardControlSensorsPresent = MavSysStatusSensor.MavSysStatusSensorPropulsion,
                OnboardControlSensorsEnabled = MavSysStatusSensor.MavSysStatusSensor3dAccel2, 
                OnboardControlSensorsHealth = MavSysStatusSensor.MavSysStatusExtensionUsed, 
                Load = 1,
                VoltageBattery = 123,
                CurrentBattery = 97,
                DropRateComm = 304,
                ErrorsComm = 99,
                ErrorsCount1 = 120, 
                ErrorsCount3 = 156, 
                ErrorsCount4 = 34, 
                BatteryRemaining = batteryRemaining, 
                OnboardControlSensorsPresentExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
                OnboardControlSensorsEnabledExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
                OnboardControlSensorsHealthExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem,
            }
        };
        using var sub1 = _client.BatteryCharge.Subscribe(v =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            _taskCompletionSource.TrySetResult(v);
        });

        // Act
        await Link.Server.Send(packet, _cancellationTokenSource.Token);
        
        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(0, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
    }
    
    [Theory]
    [InlineData(short.MinValue)]
    [InlineData(short.MaxValue)]
    public async Task ReceiveBatteryCurrent_SinglePacket_Success(short currentBattery)
    {
        var called = 0;
        var isInit = true;
        var packet = new SysStatusPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                OnboardControlSensorsPresent = MavSysStatusSensor.MavSysStatusSensorPropulsion,
                OnboardControlSensorsEnabled = MavSysStatusSensor.MavSysStatusSensor3dAccel2, 
                OnboardControlSensorsHealth = MavSysStatusSensor.MavSysStatusExtensionUsed, 
                Load = 1,
                VoltageBattery = 123,
                CurrentBattery = currentBattery,
                DropRateComm = 304,
                ErrorsComm = 99,
                ErrorsCount1 = 120, 
                ErrorsCount3 = 156, 
                ErrorsCount4 = 34, 
                BatteryRemaining = 10, 
                OnboardControlSensorsPresentExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
                OnboardControlSensorsEnabledExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
                OnboardControlSensorsHealthExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem,
            }
        };
        using var sub1 = _client.BatteryCurrent.Subscribe(v =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            _taskCompletionSource.TrySetResult(v);
        });

        // Act
        await Link.Server.Send(packet, _cancellationTokenSource.Token);
        
        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(0, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(ushort.MaxValue)]
    public async Task ReceiveBatteryVoltage_SinglePacket_Success(ushort voltageBattery)
    {
        var called = 0;
        var isInit = true;
        var packet = new SysStatusPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                OnboardControlSensorsPresent = MavSysStatusSensor.MavSysStatusSensorPropulsion,
                OnboardControlSensorsEnabled = MavSysStatusSensor.MavSysStatusSensor3dAccel2, 
                OnboardControlSensorsHealth = MavSysStatusSensor.MavSysStatusExtensionUsed, 
                Load = 1,
                VoltageBattery = voltageBattery,
                CurrentBattery = 97,
                DropRateComm = 304,
                ErrorsComm = 99,
                ErrorsCount1 = 120, 
                ErrorsCount3 = 156, 
                ErrorsCount4 = 34, 
                BatteryRemaining = 10, 
                OnboardControlSensorsPresentExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
                OnboardControlSensorsEnabledExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
                OnboardControlSensorsHealthExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem,
            }
        };
        using var sub1 = _client.BatteryVoltage.Subscribe(v =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            _taskCompletionSource.TrySetResult(v);
        });

        // Act
        await Link.Server.Send(packet, _cancellationTokenSource.Token);
        
        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(0, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(ushort.MaxValue)]
    public async Task ReceiveCpuLoad_SinglePacket_Success(ushort load)
    {
        var called = 0;
        var isInit = true;
        var packet = new SysStatusPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                OnboardControlSensorsPresent = MavSysStatusSensor.MavSysStatusSensorPropulsion,
                OnboardControlSensorsEnabled = MavSysStatusSensor.MavSysStatusSensor3dAccel2, 
                OnboardControlSensorsHealth = MavSysStatusSensor.MavSysStatusExtensionUsed, 
                Load = load,
                VoltageBattery = 45,
                CurrentBattery = 97,
                DropRateComm = 304,
                ErrorsComm = 99,
                ErrorsCount1 = 120, 
                ErrorsCount3 = 156, 
                ErrorsCount4 = 34, 
                BatteryRemaining = 10, 
                OnboardControlSensorsPresentExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
                OnboardControlSensorsEnabledExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
                OnboardControlSensorsHealthExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem,
            }
        };
        using var sub1 = _client.CpuLoad.Subscribe(v =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            _taskCompletionSource.TrySetResult(v);
        });

        // Act
        await Link.Server.Send(packet, _cancellationTokenSource.Token);
        
        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(0, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(ushort.MaxValue)]
    public async Task ReceiveDropRateCommunication_SinglePacket_Success(ushort dropRateComm)
    {
        var called = 0;
        var isInit = true;
        var packet = new SysStatusPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                OnboardControlSensorsPresent = MavSysStatusSensor.MavSysStatusSensorPropulsion,
                OnboardControlSensorsEnabled = MavSysStatusSensor.MavSysStatusSensor3dAccel2, 
                OnboardControlSensorsHealth = MavSysStatusSensor.MavSysStatusExtensionUsed, 
                Load = 1,
                VoltageBattery = 45,
                CurrentBattery = 97,
                DropRateComm = dropRateComm,
                ErrorsComm = 99,
                ErrorsCount1 = 120, 
                ErrorsCount3 = 156, 
                ErrorsCount4 = 34, 
                BatteryRemaining = 10, 
                OnboardControlSensorsPresentExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
                OnboardControlSensorsEnabledExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
                OnboardControlSensorsHealthExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem,
            }
        };
        using var sub1 = _client.DropRateCommunication.Subscribe(v =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            _taskCompletionSource.TrySetResult(v);
        });

        // Act
        await Link.Server.Send(packet, _cancellationTokenSource.Token);
        
        // Assert
        await _taskCompletionSource.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, Link.Server.TxPackets);
        Assert.Equal(Link.Server.TxPackets, Link.Client.RxPackets);
        Assert.Equal(0, Link.Server.RxPackets);
        Assert.Equal(Link.Server.RxPackets, Link.Client.TxPackets);
    }
}