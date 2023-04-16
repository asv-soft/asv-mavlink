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
    IRxValue<double> CpuLoad { get; }
    IRxValue<double> DropRateCommunication { get; }
}



public class TelemetryClientEx : DisposableOnceWithCancel, ITelemetryClientEx
{
    private readonly RxValue<double> _batteryCharge;
    private readonly RxValue<double> _batteryCurrent;
    private readonly RxValue<double> _batteryVoltage;
    private readonly RxValue<double> _cpuLoad;
    private readonly RxValue<double> _dropRateComm;

    public TelemetryClientEx(ITelemetryClient client)
    {
        Base = client;
        
        _batteryCharge = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(_=>_.BatteryRemaining < 0 ? Double.NaN : _.BatteryRemaining / 100.0d).Subscribe(_batteryCharge).DisposeItWith(Disposable);
        _batteryCurrent = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(_=>_.CurrentBattery < 0 ? Double.NaN : _.CurrentBattery / 100.0d).Subscribe(_batteryCurrent).DisposeItWith(Disposable);
        _batteryVoltage = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(_=>_.VoltageBattery / 1000.0d).Subscribe(_batteryVoltage).DisposeItWith(Disposable);
        
        _cpuLoad = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(_=>_.Load/1000D).Subscribe(_cpuLoad).DisposeItWith(Disposable);
        _dropRateComm = new RxValue<double>(double.NaN).DisposeItWith(Disposable);
        client.SystemStatus.Select(_ => _.DropRateComm / 1000D).Subscribe(_dropRateComm).DisposeItWith(Disposable);
    }

    public ITelemetryClient Base { get; }

    
    public IRxValue<double> BatteryCharge => _batteryCharge;

    public IRxValue<double> BatteryCurrent => _batteryCurrent;

    public IRxValue<double> BatteryVoltage => _batteryVoltage;

    public IRxValue<double> CpuLoad => _cpuLoad;
    public IRxValue<double> DropRateCommunication => _dropRateComm;
}