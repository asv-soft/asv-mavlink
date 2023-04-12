using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public interface ITelemetryClientEx
{
    ITelemetryClient Base { get; }
    /// <summary>
    /// Set the interval between messages for a particular MAVLink message ID. This interface replaces REQUEST_DATA_STREAM.
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="intervalUs"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task SetMessageInterval(int messageId, int intervalUs, CancellationToken cancel = default);
    /// <summary>
    /// Request the target system(s) emit a single instance of a specified message (i.e. a "one-shot" version of MAV_CMD_SET_MESSAGE_INTERVAL).
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="intervalUs"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task RequestMessageOnce(int messageId, CancellationToken cancel = default);
    
    IRxValue<double> BatteryCharge { get; }
    IRxValue<double> BatteryCurrent { get; }
    IRxValue<double> BatteryVoltage { get; }
}

public static class TelemetryClientExHelper
{
    public static Task SetMessageInterval<TPacket>(this ITelemetryClientEx src, int intervalUs, CancellationToken cancel = default)
        where TPacket : IPacketV2<IPayload>, new()
    {
        var pkt = new TPacket();
        return src.SetMessageInterval(pkt.MessageId, intervalUs, cancel);
    }
    public static Task RequestMessageOnce<TPacket>(this ITelemetryClientEx src, int intervalUs, CancellationToken cancel = default)
        where TPacket : IPacketV2<IPayload>, new()
    {
        var pkt = new TPacket();
        return src.RequestMessageOnce(pkt.MessageId, cancel);
    }
}

public class TelemetryClientEx : DisposableOnceWithCancel, ITelemetryClientEx
{
    private readonly ICommandClient _commandClient;
    private readonly RxValue<double> _batteryCharge;
    private readonly RxValue<double> _batteryCurrent;
    private readonly RxValue<double> _batteryVoltage;

    public TelemetryClientEx(ITelemetryClient client, ICommandClient commandClient)
    {
        _commandClient = commandClient;
        Base = client;
        
        _batteryCharge = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(_=>_.BatteryRemaining < 0 ? Double.NaN : _.BatteryRemaining / 100.0d).Subscribe(_batteryCharge).DisposeItWith(Disposable);
        _batteryCurrent = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(_=>_.CurrentBattery < 0 ? Double.NaN : _.CurrentBattery / 100.0d).Subscribe(_batteryCurrent).DisposeItWith(Disposable);
        _batteryVoltage = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(_=>_.VoltageBattery / 1000.0d).Subscribe(_batteryVoltage).DisposeItWith(Disposable);
    }

    public ITelemetryClient Base { get; }

    public Task SetMessageInterval(int messageId,int intervalUs, CancellationToken cancel = default)
    {
        return _commandClient.CommandLongAndCheckResult(MavCmd.MavCmdSetMessageInterval, messageId, intervalUs, 0, Single.NaN,
            Single.NaN, Single.NaN, Single.NaN, cancel);
    }

    public Task RequestMessageOnce(int messageId, CancellationToken cancel = default)
    {
        return _commandClient.CommandLongAndCheckResult(MavCmd.MavCmdRequestMessage, messageId, Single.NaN, Single.NaN, Single.NaN,
            Single.NaN, Single.NaN, 0, cancel);
    }

    public IRxValue<double> BatteryCharge => _batteryCharge;

    public IRxValue<double> BatteryCurrent => _batteryCurrent;

    public IRxValue<double> BatteryVoltage => _batteryVoltage;
}