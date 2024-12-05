using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink.Common;

using DeepEqual.Syntax;
using JetBrains.Annotations;
using R3;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(TelemetryClient))]
public class TelemetryClientTest : ClientTestBase<TelemetryClient>
{
    private readonly TaskCompletionSource<MavlinkMessage> _taskCompletionSource;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TelemetryClient _client;
    
    public TelemetryClientTest(ITestOutputHelper log) : base(log)
    {
        _client = Client;
        _taskCompletionSource = new TaskCompletionSource<MavlinkMessage>();
        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        _cancellationTokenSource.Token.Register(() => _taskCompletionSource.TrySetCanceled());
    }
    
    protected override TelemetryClient CreateClient(MavlinkClientIdentity identity, CoreServices core) 
        => new(identity, core);

    [Fact]
    public void Constructor_WithNull_Throws()
    {
        // ReSharper disable once NullableWarningSuppressionIsUsed
        Assert.Throws<ArgumentNullException>(() => new TelemetryClient(null!, Core));
        // ReSharper disable once NullableWarningSuppressionIsUsed
        Assert.Throws<ArgumentNullException>(() => new TelemetryClient(Identity, null!));
    }

    [Theory]
    [InlineData(byte.MinValue, ushort.MinValue, false)]
    [InlineData(byte.MinValue, byte.MaxValue, false)]
    [InlineData(byte.MaxValue, byte.MinValue, false)]
    [InlineData(byte.MinValue, byte.MinValue, true)]
    [InlineData(byte.MaxValue, byte.MaxValue, false)]
    [InlineData(byte.MaxValue, byte.MaxValue, true)]
    public async Task RequestDataStream_WithDifferentInput_Success(
        byte streamId, 
        ushort rateHz, 
        bool startStop
    )
    {
        // Arrange
        var called = 0;
        RequestDataStreamPacket? packetFromClient = null;
        using var sub1 = Link.Server.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p =>
        {
            called++;

            _taskCompletionSource.TrySetResult(p);
        });
        using var sub2 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as RequestDataStreamPacket;
        });
        
        // Act
        await _client.RequestDataStream(streamId, rateHz, startStop, _cancellationTokenSource.Token);
        
        // Assert
        var result = await _taskCompletionSource.Task as RequestDataStreamPacket;
        Assert.Equal(1, called);
        Assert.Equal(called, (int) Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(0, (int) Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.NotNull(result);
        Assert.NotNull(packetFromClient);
        Assert.True(packetFromClient.IsDeepEqual(result));
    }
    
    [Fact(Skip = "Wait for pr from main")]
    public async Task RequestDataStream_Cancel_Throws()
    {
        // Arrange
        var called = 0;
        RequestDataStreamPacket? packetFromClient = null;
        await _cancellationTokenSource.CancelAsync();
        using var sub1 = Link.Server.OnRxMessage.RxFilterByType<MavlinkMessage>().Subscribe(p =>
        {
            called++;

            _taskCompletionSource.TrySetResult(p);
        });
        using var sub2 = Link.Client.OnTxMessage.Subscribe(p =>
        {
            packetFromClient = p as RequestDataStreamPacket;
        });
        
        // Act + Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            async () => 
                await _client.RequestDataStream(
                    0, 
                    3, 
                    true, 
                    _cancellationTokenSource.Token
                )
        );
        Assert.Equal(0, called);
        Assert.Equal(called, (int) Link.Client.Statistic.TxMessages);
        Assert.Equal(Link.Client.Statistic.TxMessages, Link.Server.Statistic.RxMessages);
        Assert.Equal(0, (int) Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Null(packetFromClient);
    }

    #region SendRadioStatusPacket

    [Theory]
    [InlineData(
        ushort.MinValue, 
        byte.MinValue, 
        byte.MinValue,
        byte.MinValue,
        byte.MinValue,
        ushort.MinValue,
        byte.MinValue
    )]
    [InlineData(
        ushort.MaxValue, 
        byte.MaxValue, 
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        ushort.MaxValue,
        byte.MaxValue
    )]
    public async Task SendRadioStatusPacket_SinglePacket_Success
    (
        ushort fixedValue,
        byte noise, 
        byte remnoise,
        byte remrssi,
        byte rssi,
        ushort rxerrors,
        byte txbuf
    )
    {
        var tcs = new TaskCompletionSource<RadioStatusPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var called = 0;
        var isInit = true;
        var packet = new RadioStatusPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                Fixed = fixedValue,
                Noise = noise,
                Remnoise = remnoise,
                Remrssi = remrssi,
                Rssi = rssi,
                Rxerrors = rxerrors,
                Txbuf = txbuf
            }
        };
        using var sub1 = _client.Radio.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            // ReSharper disable once NullableWarningSuppressionIsUsed
            tcs.TrySetResult(p!);
        });

        // Act
        await Link.Server.Send(packet, cancel.Token);
        
        // Assert
        var payloadFromServer = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int) Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.RxMessages, Link.Client.Statistic.TxMessages);
        Assert.NotNull(payloadFromServer);
        Assert.True(packet.Payload.IsDeepEqual(payloadFromServer));
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(1000)]
    [InlineData(10_000)]
    public async Task SendRadioStatusPacket_SeveralPackets_Success(int packetsCount)
    {
        var tcs = new TaskCompletionSource<RadioStatusPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var called = 0;
        var isInit = true;
        var payloads = new List<RadioStatusPayload>();
        var packet = new RadioStatusPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                Fixed = 0,
                Noise = 2,
                Remnoise = 3,
                Remrssi = 34,
                Rssi = 3,
                Rxerrors = 32,
                Txbuf = 100
            }
        };
        using var sub1 = _client.Radio.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            
            called++;
            payloads.Add(p!);

            if (called >= packetsCount)
            {
                tcs.TrySetResult(p!);
            }
        });

        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Link.Server.Send(packet, cancel.Token);
        }
        
        // Assert
        await tcs.Task;
        Assert.Equal(packetsCount, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int) Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.RxMessages, Link.Client.Statistic.TxMessages);
        Assert.NotEmpty(payloads);
        Assert.True(payloads.All(p => p.IsDeepEqual(packet.Payload)));
    }
    
    [Fact]
    public async Task SendRadioStatusPacket_NoAddress_Throws()
    {
        var tcs = new TaskCompletionSource<RadioStatusPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromMilliseconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var called = 0;
        var isInit = true;
        var packet = new RadioStatusPacket
        {
            Payload =
            {
                Fixed = 0,
                Noise = 2,
                Remnoise = 3,
                Remrssi = 34,
                Rssi = 3,
                Rxerrors = 32,
                Txbuf = 100
            }
        };
        using var sub1 = _client.Radio.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            tcs.TrySetResult(p!);
        });

        // Act
        await Link.Server.Send(packet, cancel.Token);
        
        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(async () => await tcs.Task);
        Assert.Equal(0, called);
    }
    
    #endregion
    
    #region SysStatusPacket
    
    [Theory]
    [InlineData(
        MavSysStatusSensor.MavSysStatusSensor3dGyro, 
        MavSysStatusSensor.MavSysStatusSensor3dGyro, 
        MavSysStatusSensor.MavSysStatusSensor3dGyro, 
        ushort.MinValue, 
        ushort.MinValue, 
        short.MinValue, 
        ushort.MinValue, 
        ushort.MinValue, 
        ushort.MinValue, 
        ushort.MinValue, 
        ushort.MinValue, 
        ushort.MinValue, 
        sbyte.MinValue, 
        MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
        MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
        MavSysStatusSensorExtended.MavSysStatusRecoverySystem
    )]
    [InlineData(
        MavSysStatusSensor.MavSysStatusExtensionUsed, 
        MavSysStatusSensor.MavSysStatusExtensionUsed, 
        MavSysStatusSensor.MavSysStatusExtensionUsed, 
        ushort.MaxValue, 
        ushort.MaxValue, 
        short.MaxValue, 
        ushort.MaxValue, 
        ushort.MaxValue, 
        ushort.MaxValue, 
        ushort.MaxValue, 
        ushort.MaxValue, 
        ushort.MaxValue, 
        sbyte.MaxValue, 
        MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
        MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
        MavSysStatusSensorExtended.MavSysStatusRecoverySystem
    )]
    public async Task SendSysStatusPacket_SinglePacket_Success
    (
       MavSysStatusSensor onboardControlSensorsPresent, 
       MavSysStatusSensor onboardControlSensorsEnabled, 
       MavSysStatusSensor onboardControlSensorsHealth, 
       ushort load, 
       ushort voltageBattery, 
       short currentBattery, 
       ushort dropRateComm, 
       ushort errorsComm, 
       ushort errorsCount1, 
       ushort errorsCount2, 
       ushort errorsCount3, 
       ushort errorsCount4, 
       sbyte batteryRemaining, 
       MavSysStatusSensorExtended onboardControlSensorsPresentExtended, 
       MavSysStatusSensorExtended onboardControlSensorsEnabledExtended, 
       MavSysStatusSensorExtended onboardControlSensorsHealthExtended
    )
    {
        var tcs = new TaskCompletionSource<SysStatusPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var called = 0;
        var isInit = true;
        var packet = new SysStatusPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                OnboardControlSensorsPresent = onboardControlSensorsPresent,
                OnboardControlSensorsEnabled = onboardControlSensorsEnabled, 
                OnboardControlSensorsHealth = onboardControlSensorsHealth, 
                Load = load,
                VoltageBattery = voltageBattery,
                CurrentBattery = currentBattery,
                DropRateComm = dropRateComm,
                ErrorsComm = errorsComm,
                ErrorsCount1 = errorsCount1, 
                ErrorsCount2 = errorsCount2, 
                ErrorsCount3 = errorsCount3, 
                ErrorsCount4 = errorsCount4, 
                BatteryRemaining = batteryRemaining, 
                OnboardControlSensorsPresentExtended = onboardControlSensorsPresentExtended, 
                OnboardControlSensorsEnabledExtended = onboardControlSensorsEnabledExtended, 
                OnboardControlSensorsHealthExtended = onboardControlSensorsHealthExtended,
            }
        };
        using var sub1 = _client.SystemStatus.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            tcs.TrySetResult(p!);
        });

        // Act
        await Link.Server.Send(packet, cancel.Token);
        
        // Assert
        var payloadFromServer = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int) Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.RxMessages, Link.Client.Statistic.TxMessages);
        Assert.NotNull(payloadFromServer);
        Assert.True(packet.Payload.IsDeepEqual(payloadFromServer));
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(1000)]
    [InlineData(10_000)]
    public async Task SendSysStatusPacket_SeveralPackets_Success(int packetsCount)
    {
        var tcs = new TaskCompletionSource<SysStatusPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var called = 0;
        var isInit = true;
        var payloads = new List<SysStatusPayload>();
        var packet = new SysStatusPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                OnboardControlSensorsPresent = MavSysStatusSensor.MavSysStatusAhrs,
                OnboardControlSensorsEnabled = MavSysStatusSensor.MavSysStatusLogging, 
                OnboardControlSensorsHealth = MavSysStatusSensor.MavSysStatusSensor3dAccel2, 
                Load = 10,
                VoltageBattery = 11,
                CurrentBattery = 12,
                DropRateComm = 13,
                ErrorsComm = 14,
                ErrorsCount1 = 15, 
                ErrorsCount2 = 16, 
                ErrorsCount3 = 17, 
                ErrorsCount4 = 18, 
                BatteryRemaining = 19, 
                OnboardControlSensorsPresentExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
                OnboardControlSensorsEnabledExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
                OnboardControlSensorsHealthExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem,
            }
        };
        using var sub1 = _client.SystemStatus.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            payloads.Add(p!);

            if (called >= packetsCount)
            {
                tcs.TrySetResult(p!);
            }
        });

        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Link.Server.Send(packet, cancel.Token);
        }
        
        // Assert
        await tcs.Task;
        Assert.Equal(packetsCount, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int) Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.RxMessages, Link.Client.Statistic.TxMessages);
        Assert.NotEmpty(payloads);
        Assert.True(payloads.All(p => p.IsDeepEqual(packet.Payload)));
    }
    
    [Fact]
    public async Task SendSysStatusPacket_NoAddress_Throws()
    {
        var tcs = new TaskCompletionSource<SysStatusPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromMilliseconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var called = 0;
        var isInit = true;
        var packet = new SysStatusPacket
        {
            Payload =
            {
                OnboardControlSensorsPresent = MavSysStatusSensor.MavSysStatusAhrs,
                OnboardControlSensorsEnabled = MavSysStatusSensor.MavSysStatusLogging, 
                OnboardControlSensorsHealth = MavSysStatusSensor.MavSysStatusSensor3dAccel2, 
                Load = 10,
                VoltageBattery = 11,
                CurrentBattery = 12,
                DropRateComm = 13,
                ErrorsComm = 14,
                ErrorsCount1 = 15, 
                ErrorsCount2 = 16, 
                ErrorsCount3 = 17, 
                ErrorsCount4 = 18, 
                BatteryRemaining = 19, 
                OnboardControlSensorsPresentExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
                OnboardControlSensorsEnabledExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem, 
                OnboardControlSensorsHealthExtended = MavSysStatusSensorExtended.MavSysStatusRecoverySystem,
            }
        };
        using var sub1 = _client.SystemStatus.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            tcs.TrySetResult(p!);
        });

        // Act
        await Link.Server.Send(packet, cancel.Token);
        
        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(async () => await tcs.Task);
        Assert.Equal(0, called);
    }
    
    #endregion

    #region SendExtendedSysStatePacket

    [Theory]
    [InlineData(
        MavVtolState.MavVtolStateUndefined,
        MavLandedState.MavLandedStateUndefined
    )]
    [InlineData(
        MavVtolState.MavVtolStateFw,
        MavLandedState.MavLandedStateLanding
    )]
    public async Task SendExtendedSysStatePacket_SinglePacket_Success
    (
        MavVtolState vtolState, 
        MavLandedState landedState
    )
    {
        var tcs = new TaskCompletionSource<ExtendedSysStatePayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var called = 0;
        var isInit = true;
        var packet = new ExtendedSysStatePacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                VtolState = vtolState,
                LandedState = landedState,
            }
        };
        using var sub1 = _client.ExtendedSystemState.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            tcs.TrySetResult(p);
        });

        // Act
        await Link.Server.Send(packet, cancel.Token);
        
        // Assert
        var payloadFromServer = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int) Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.RxMessages, Link.Client.Statistic.TxMessages);
        Assert.NotNull(payloadFromServer);
        Assert.True(packet.Payload.IsDeepEqual(payloadFromServer));
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(1000)]
    [InlineData(10_000)]
    public async Task SendExtendedSysStatePacket_SeveralPackets_Success(int packetsCount)
    {
        var tcs = new TaskCompletionSource<ExtendedSysStatePayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var called = 0;
        var isInit = true;
        var payloads = new List<ExtendedSysStatePayload>();
        var packet = new ExtendedSysStatePacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                VtolState = MavVtolState.MavVtolStateFw,
                LandedState = MavLandedState.MavLandedStateInAir,
            }
        };
        using var sub1 = _client.ExtendedSystemState.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            payloads.Add(p!);

            if (called >= packetsCount)
            {
                tcs.TrySetResult(p!);
            }
        });

        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Link.Server.Send(packet, cancel.Token);
        }
        
        // Assert
        await tcs.Task;
        Assert.Equal(packetsCount, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int) Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.RxMessages, Link.Client.Statistic.TxMessages);
        Assert.NotEmpty(payloads);
        Assert.True(payloads.All(p => p.IsDeepEqual(packet.Payload)));
    }
    
    [Fact]
    public async Task SendExtendedSysStatePacket_NoAddress_Throws()
    {
        var tcs = new TaskCompletionSource<ExtendedSysStatePayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromMilliseconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var called = 0;
        var isInit = true;
        var packet = new ExtendedSysStatePacket
        {
            Payload =
            {
                VtolState = MavVtolState.MavVtolStateFw,
                LandedState = MavLandedState.MavLandedStateInAir,
            }
        };
        using var sub1 = _client.ExtendedSystemState.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            tcs.TrySetResult(p!);
        });

        // Act
        await Link.Server.Send(packet, cancel.Token);
        
        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(async () => await tcs.Task);
        Assert.Equal(0, called);
    }

    #endregion

    #region SendBatteryStatusPacket

    [Theory]
    [InlineData(
        int.MinValue, 
        int.MinValue, 
        short.MinValue, 
        short.MinValue, 
        byte.MinValue, 
        MavBatteryFunction.MavBatteryFunctionUnknown, 
        MavBatteryType.MavBatteryTypeUnknown, 
        sbyte.MinValue, 
        int.MinValue, 
        MavBatteryChargeState.MavBatteryChargeStateUndefined, 
        MavBatteryMode.MavBatteryModeUnknown, 
        MavBatteryFault.MavBatteryFaultDeepDischarge
    )]
    [InlineData(
        int.MaxValue, 
        int.MaxValue, 
        short.MaxValue, 
        short.MaxValue, 
        byte.MaxValue, 
        MavBatteryFunction.MavBatteryFunctionPayload, 
        MavBatteryType.MavBatteryTypeNimh, 
        sbyte.MaxValue, 
        int.MaxValue, 
        MavBatteryChargeState.MavBatteryChargeStateCharging, 
        MavBatteryMode.MavBatteryModeHotSwap, 
        MavBatteryFault.BatteryFaultIncompatibleCellsConfiguration
    )]
    public async Task SendBatteryStatusPacket_SinglePacket_Success
    (
        int currentConsumed,
        int energyConsumed,
        short temperature,
        short currentBattery,
        byte id,
        MavBatteryFunction batteryFunction,
        MavBatteryType type, 
        sbyte batteryRemaining, 
        int timeRemaining, 
        MavBatteryChargeState chargeState, 
        MavBatteryMode mode, 
        MavBatteryFault faultBitmask
    )
    {
        var tcs = new TaskCompletionSource<BatteryStatusPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var called = 0;
        var isInit = true;
        var packet = new BatteryStatusPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                CurrentConsumed = currentConsumed,
                EnergyConsumed = energyConsumed,
                Temperature = temperature,
                CurrentBattery = currentBattery,
                Id = id,
                BatteryFunction = batteryFunction,
                Type = type,
                BatteryRemaining = batteryRemaining,
                TimeRemaining = timeRemaining,
                ChargeState = chargeState,
                Mode = mode,
                FaultBitmask = faultBitmask,
            }
        };
        using var sub1 = _client.Battery.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            tcs.TrySetResult(p!);
        });

        // Act
        await Link.Server.Send(packet, cancel.Token);
        
        // Assert
        var payloadFromServer = await tcs.Task;
        Assert.Equal(1, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int) Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.RxMessages, Link.Client.Statistic.TxMessages);
        Assert.NotNull(payloadFromServer);
        Assert.True(packet.Payload.IsDeepEqual(payloadFromServer));
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(1000)]
    [InlineData(10_000)]
    public async Task SendBatteryStatusPacket_SeveralPackets_Success(int packetsCount)
    {
        var tcs = new TaskCompletionSource<BatteryStatusPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var called = 0;
        var isInit = true;
        var payloads = new List<BatteryStatusPayload>();
        var packet = new BatteryStatusPacket
        {
            SystemId = Identity.Target.SystemId,
            ComponentId = Identity.Target.ComponentId,
            Payload =
            {
                CurrentConsumed = 10,
                EnergyConsumed = 300,
                Temperature = 112,
                CurrentBattery = 98,
                Id = 23,
                BatteryFunction = MavBatteryFunction.MavBatteryFunctionAvionics,
                Type = MavBatteryType.MavBatteryTypeNimh,
                BatteryRemaining = 45,
                TimeRemaining = 2000,
                ChargeState = MavBatteryChargeState.MavBatteryChargeStateCharging,
                Mode = MavBatteryMode.MavBatteryModeUnknown,
                FaultBitmask = MavBatteryFault.MavBatteryFaultIncompatibleFirmware,
            }
        };
        using var sub1 = _client.Battery.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;
            payloads.Add(p!);

            if (called >= packetsCount)
            {
                tcs.TrySetResult(p!);
            }
        });

        // Act
        for (var i = 0; i < packetsCount; i++)
        {
            await Link.Server.Send(packet, cancel.Token);
        }
        
        // Assert
        await tcs.Task;
        Assert.Equal(packetsCount, called);
        Assert.Equal(called, (int)Link.Server.Statistic.TxMessages);
        Assert.Equal(Link.Server.Statistic.TxMessages, Link.Client.Statistic.RxMessages);
        Assert.Equal(0, (int) Link.Server.Statistic.RxMessages);
        Assert.Equal(Link.Server.Statistic.RxMessages, Link.Client.Statistic.TxMessages);
        Assert.NotEmpty(payloads);
        Assert.True(payloads.All(p => p.IsDeepEqual(packet.Payload)));
    }
    
    [Fact]
    public async Task SendBatteryStatusPacket_NoAddress_Throws()
    {
        var tcs = new TaskCompletionSource<BatteryStatusPayload>();
        var cancel = new CancellationTokenSource(TimeSpan.FromMilliseconds(200), TimeProvider.System);
        cancel.Token.Register(() => tcs.TrySetCanceled());

        var called = 0;
        var isInit = true;
        var packet = new BatteryStatusPacket
        {
            Payload =
            {
                CurrentConsumed = 10,
                EnergyConsumed = 300,
                Temperature = 112,
                CurrentBattery = 98,
                Id = 23,
                BatteryFunction = MavBatteryFunction.MavBatteryFunctionAvionics,
                Type = MavBatteryType.MavBatteryTypeNimh,
                BatteryRemaining = 45,
                TimeRemaining = 2000,
                ChargeState = MavBatteryChargeState.MavBatteryChargeStateCharging,
                Mode = MavBatteryMode.MavBatteryModeUnknown,
                FaultBitmask = MavBatteryFault.MavBatteryFaultIncompatibleFirmware,
            }
        };
        using var sub1 = _client.Battery.Subscribe(p =>
        {
            if (isInit)
            {
                isInit = false;
                return;
            }
            called++;

            tcs.TrySetResult(p!);
        });

        // Act
        await Link.Server.Send(packet, cancel.Token);
        
        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(async () => await tcs.Task);
        Assert.Equal(0, called);
    }

    #endregion
}