using System;
using System.Reactive.Linq;
using Asv.Common;

namespace Asv.Mavlink;

public interface ITelemetryClientEx
{
    ITelemetryClient Base { get; }
    
    
    IRxValue<double> BatteryCharge { get; }
    IRxValue<double> BatteryCurrent { get; }
    IRxValue<double> BatteryVoltage { get; }
}



public class TelemetryClientEx : DisposableOnceWithCancel, ITelemetryClientEx
{
    private readonly RxValue<double> _batteryCharge;
    private readonly RxValue<double> _batteryCurrent;
    private readonly RxValue<double> _batteryVoltage;

    public TelemetryClientEx(ITelemetryClient client)
    {
        Base = client;
        
        _batteryCharge = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(_=>_.BatteryRemaining < 0 ? Double.NaN : _.BatteryRemaining / 100.0d).Subscribe(_batteryCharge).DisposeItWith(Disposable);
        _batteryCurrent = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(_=>_.CurrentBattery < 0 ? Double.NaN : _.CurrentBattery / 100.0d).Subscribe(_batteryCurrent).DisposeItWith(Disposable);
        _batteryVoltage = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(_=>_.VoltageBattery / 1000.0d).Subscribe(_batteryVoltage).DisposeItWith(Disposable);
    }

    public ITelemetryClient Base { get; }

    
    public IRxValue<double> BatteryCharge => _batteryCharge;

    public IRxValue<double> BatteryCurrent => _batteryCurrent;

    public IRxValue<double> BatteryVoltage => _batteryVoltage;
}