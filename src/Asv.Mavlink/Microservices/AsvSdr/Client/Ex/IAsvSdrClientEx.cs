#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using DynamicData;

namespace Asv.Mavlink;

public interface IAsvSdrClientEx
{
    IAsvSdrClient Base { get; }
    IRxValue<AsvSdrCustomModeFlag> SupportedModes { get; }
    IRxValue<AsvSdrCustomMode> CustomMode { get; }
    IRxValue<ushort> RecordsCount { get; }
    IObservable<IChangeSet<IAsvSdrClientRecord,Guid>> Records { get; }
    IRxValue<Guid> CurrentRecord { get; }
    IRxValue<bool> IsRecordStarted { get; }
    Task DeleteRecord(Guid recordName, CancellationToken cancel = default);
    Task<bool> DownloadRecordList(IProgress<double> progress = null, CancellationToken cancel = default);
    Task<MavResult> SetMode(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate, uint sendingThinningRatio, float refPower, CancellationToken cancel = default);
    Task<MavResult> StartRecord(string recordName, CancellationToken cancel = default);
    Task<MavResult> StopRecord(CancellationToken cancel = default);
    Task<MavResult> CurrentRecordSetTag(string tagName, AsvSdrRecordTagType type, byte[] rawValue , CancellationToken cancel = default);
    Task<MavResult> SystemControlAction(AsvSdrSystemControlAction action, CancellationToken cancel = default);
    Task<MavResult> StartMission(ushort missionIndex = 0, CancellationToken cancel = default);
    Task<MavResult> StopMission(CancellationToken cancel = default);
    
    #region Calibration
    
    Task<MavResult> StartCalibration(CancellationToken cancel = default);
    Task<MavResult> StopCalibration(CancellationToken cancel = default);
    IRxValue<AsvSdrCalibState> CalibrationState { get; }
    IRxValue<ushort> CalibrationTableRemoteCount { get; }
    Task ReadCalibrationTableList(IProgress<double>? progress = null,CancellationToken cancel = default);
    Task<AsvSdrClientCalibrationTable?> GetCalibrationTable(string name, CancellationToken cancel = default);
    IObservable<IChangeSet<AsvSdrClientCalibrationTable,string>> CalibrationTables { get; }
        
    #endregion
    
    public async Task StartCalibrationAndCheckResult( CancellationToken cancel = default)
    {
        var result = await StartCalibration(cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new Exception($"Start calibration failed. Result: {result}");
    }
    
    public async Task StopCalibrationAndCheckResult( CancellationToken cancel = default)
    {
        var result = await StopCalibration(cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new Exception($"Stop calibration failed. Result: {result}");
    }
    
    public async Task StartMissionAndCheckResult( ushort missionIndex, CancellationToken cancel = default)
    {
        var result = await StartMission(missionIndex, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new Exception($"Start mission '{missionIndex:g}' failed. Result: {result}");
    }
    public async Task StopMissionAndCheckResult( CancellationToken cancel = default)
    {
        var result = await StopMission(cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new Exception($"Stop mission failed. Result: {result}");
    }
    public async Task SystemControlActionCheckResult( AsvSdrSystemControlAction action, CancellationToken cancel = default)
    {
        var result = await SystemControlAction(action, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new Exception($"System control action {action:G} failed. Result: {result}");
    }
    public async Task SetModeAndCheckResult( AsvSdrCustomMode mode, ulong frequencyHz, float recordRate, uint sendingThinningRatio, float refPower, CancellationToken cancel)
    {
        var result = await SetMode(mode, frequencyHz, recordRate, sendingThinningRatio, refPower, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new Exception($"Set mode failed. Result: {result}");
    }
    
    public async Task StartRecordAndCheckResult( string recordName, CancellationToken cancel)
    {
        var result = await StartRecord(recordName, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new Exception($"Start record failed. Result: {result}");
    }
    
    public async Task StopRecordAndCheckResult( CancellationToken cancel)
    {
        var result = await StopRecord(cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new Exception($"Stop record failed. Result: {result}");
    }

    public async Task CurrentRecordSetTagAndCheckResult( string tagName, string value, CancellationToken cancel)
    {
        var result = await CurrentRecordSetTag(tagName, value, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new Exception($"Set tag failed. Result: {result}");
    }
    public async Task CurrentRecordSetTagAndCheckResult( string tagName, ulong value, CancellationToken cancel)
    {
        var result = await CurrentRecordSetTag(tagName, value, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new Exception($"Set tag failed. Result: {result}");
    }
    public async Task CurrentRecordSetTagAndCheckResult( string tagName, long value, CancellationToken cancel)
    {
        var result = await CurrentRecordSetTag(tagName, value, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new Exception($"Set tag failed. Result: {result}");
    }
    public async Task CurrentRecordSetTagAndCheckResult( string tagName, double value, CancellationToken cancel)
    {
        var result = await CurrentRecordSetTag(tagName, value, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new Exception($"Set tag failed. Result: {result}");
    }
    
    public Task<MavResult> CurrentRecordSetTag( string tagName, string value, CancellationToken cancel)
    {
        if (value.Length > AsvSdrHelper.RecordTagValueLength) 
            throw new Exception($"Tag string value is too long. Max length is {AsvSdrHelper.RecordTagValueLength}");
        var nameArray = new byte[AsvSdrHelper.RecordTagValueLength];
        MavlinkTypesHelper.SetString(nameArray,value);
        return CurrentRecordSetTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeString8, nameArray, cancel);
    }
    public Task<MavResult> CurrentRecordSetTag( string tagName, ulong value, CancellationToken cancel)
    {
        return CurrentRecordSetTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeUint64, BitConverter.GetBytes(value), cancel);
    }
    public Task<MavResult> CurrentRecordSetTag( string tagName, long value, CancellationToken cancel)
    {
        return CurrentRecordSetTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeInt64, BitConverter.GetBytes(value), cancel);
    }
    
    public Task<MavResult> CurrentRecordSetTag( string tagName, double value, CancellationToken cancel)
    {
        return CurrentRecordSetTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64, BitConverter.GetBytes(value), cancel);
    }
}
