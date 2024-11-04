#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using ObservableCollections;
using R3;

namespace Asv.Mavlink;

public interface IAsvSdrClientEx:IMavlinkMicroserviceClient
{
    /// <summary>
    /// Gets an instance of IAsvSdrClient.
    /// </summary>
    /// This property represents the base client for the ASV SDR functionality.
    IAsvSdrClient Base { get; }

    /// <summary>
    /// Gets the supported modes of the IRxValue object.
    /// </summary>
    /// The supported
    ReadOnlyReactiveProperty<AsvSdrCustomModeFlag> SupportedModes { get; }

    /// <summary>
    /// Gets the custom mode value.
    /// </summary>
    ReadOnlyReactiveProperty<AsvSdrCustomMode> CustomMode { get; }

    /// <summary>
    /// Gets the value representing the count of records.
    /// </summary>
    ReadOnlyReactiveProperty<ushort> RecordsCount { get; }
    IReadOnlyObservableDictionary<Guid, IAsvSdrClientRecord> Records { get; }

    /// <summary>
    /// Gets the current record value.
    /// </summary>
    ReadOnlyReactiveProperty<Guid> CurrentRecord { get; }

    /// <summary>
    /// Gets the value indicating whether the record has started.
    /// </summary>
    ReadOnlyReactiveProperty<bool> IsRecordStarted { get; }

    /// <summary>
    /// Deletes the specified record.
    /// </summary>
    Task DeleteRecord(Guid recordName, CancellationToken cancel = default);

    /// <summary>
    /// Downloads the record list.
    /// </summary>
    Task<bool> DownloadRecordList(IProgress<double> progress = null, CancellationToken cancel = default);

    /// <summary>
    /// Sets the mode of the ASV/SDR device.
    /// </summary>
    Task<MavResult> SetMode(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate, uint sendingThinningRatio, float refPower, CancellationToken cancel = default);

    /// <summary>
    /// Starts recording with the provided record name.
    /// </summary>
    Task<MavResult> StartRecord(string recordName, CancellationToken cancel = default);

    /// <summary>
    /// Stops the recording process.
    /// </summary>
    /// <param name="cancel">The cancellation token to cancel the operation (default is <see cref="CancellationToken.None"/>).</param>
    Task<MavResult> StopRecord(CancellationToken cancel = default);
    Task<MavResult> CurrentRecordSetTag(string tagName, AsvSdrRecordTagType type, byte[] rawValue , CancellationToken cancel = default);
    Task<MavResult> SystemControlAction(AsvSdrSystemControlAction action, CancellationToken cancel = default);

    /// <summary>
    /// Starts a mission with the specified mission index and cancellation token.
    /// </summary>
    Task<MavResult> StartMission(ushort missionIndex = 0, CancellationToken cancel = default);

    /// <summary>
    /// Stops the active mission.
    /// </summary>
    Task<MavResult> StopMission(CancellationToken cancel = default);
    
    #region Calibration

    /// <summary>
    /// Starts the calibration process.
    /// </summary>
    Task<MavResult> StartCalibration(CancellationToken cancel = default);
    Task<MavResult> StopCalibration(CancellationToken cancel = default);

    /// <summary>
    /// Gets the calibration state of the property.
    /// </summary>
    ReadOnlyReactiveProperty<AsvSdrCalibState?> CalibrationState { get; }
    ReadOnlyReactiveProperty<ushort?> CalibrationTableRemoteCount { get; }

    /// <summary>
    /// Reads the calibration table list.
    /// </summary>
    Task ReadCalibrationTableList(IProgress<double>? progress = null,CancellationToken cancel = default);

    /// <summary>
    /// Retrieves the calibration table for a given name.
    /// </summary>
    Task<AsvSdrClientCalibrationTable?> GetCalibrationTable(string name, CancellationToken cancel = default);

    /// <summary>
    /// Get the observable collection of calibration tables.
    /// </summary>
    IReadOnlyObservableDictionary<string, AsvSdrClientCalibrationTable> CalibrationTables { get; }
        
    #endregion

    public async Task StartCalibrationAndCheckResult( CancellationToken cancel = default)
    {
        var result = await StartCalibration(cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"Start calibration failed. Result: {result}");
    }

    /// <summary>
    /// Stops the calibration process and checks the result.
    /// </summary>
    public async Task StopCalibrationAndCheckResult( CancellationToken cancel = default)
    {
        var result = await StopCalibration(cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"Stop calibration failed. Result: {result}");
    }
    
    public async Task StartMissionAndCheckResult( ushort missionIndex, CancellationToken cancel = default)
    {
        var result = await StartMission(missionIndex, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"Start mission '{missionIndex:g}' failed. Result: {result}");
    }

    /// <summary>
    /// Stops the mission and checks the result.
    /// </summary>
    public async Task StopMissionAndCheckResult( CancellationToken cancel = default)
    {
        var result = await StopMission(cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"Stop mission failed. Result: {result}");
    }

    /// <summary>
    /// Checks the result of a system control action and throws an exception if it fails.
    /// </summary>
    /// <param name="action
    public async Task SystemControlActionCheckResult( AsvSdrSystemControlAction action, CancellationToken cancel = default)
    {
        var result = await SystemControlAction(action, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"System control action {action:G} failed. Result: {result}");
    }

    /// <summary>
    /// Sets the mode and checks the result.
    /// </summary>
    public async Task SetModeAndCheckResult( AsvSdrCustomMode mode, ulong frequencyHz, float recordRate, uint sendingThinningRatio, float refPower, CancellationToken cancel)
    {
        var result = await SetMode(mode, frequencyHz, recordRate, sendingThinningRatio, refPower, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"Set mode failed. Result: {result}");
    }
    
    public async Task StartRecordAndCheckResult( string recordName, CancellationToken cancel)
    {
        var result = await StartRecord(recordName, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"Start record failed. Result: {result}");
    }

    /// <summary>
    /// Stops the record operation and checks the result.
    /// </summary>
    public async Task StopRecordAndCheckResult( CancellationToken cancel)
    {
        var result = await StopRecord(cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"Stop record failed. Result: {result}");
    }

    /// <summary>
    /// Sets a tag for the current record and checks the result of the operation.
    /// </summary>
    public async Task CurrentRecordSetTagAndCheckResult( string tagName, string value, CancellationToken cancel)
    {
        var result = await CurrentRecordSetTag(tagName, value, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"Set tag failed. Result: {result}");
    }

    /// <summary
    public async Task CurrentRecordSetTagAndCheckResult( string tagName, ulong value, CancellationToken cancel)
    {
        var result = await CurrentRecordSetTag(tagName, value, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"Set tag failed. Result: {result}");
    }

    /// <summary>
    /// Sets the tag of current record and checks the result.
    /// </summary>
    public async Task CurrentRecordSetTagAndCheckResult( string tagName, long value, CancellationToken cancel)
    {
        var result = await CurrentRecordSetTag(tagName, value, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"Set tag failed. Result: {result}");
    }

    /// <summary>
    /// Sets the tag value for the current record and checks the result.
    /// </summary>
    public async Task CurrentRecordSetTagAndCheckResult( string tagName, double value, CancellationToken cancel)
    {
        var result = await CurrentRecordSetTag(tagName, value, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"Set tag failed. Result: {result}");
    }

    /// <summary>
    /// Sets the current record tag with the specified name and value.
    /// </summary>
    public Task<MavResult> CurrentRecordSetTag( string tagName, string value, CancellationToken cancel)
    {
        if (value.Length > AsvSdrHelper.RecordTagValueLength) 
            throw new ArgumentException($"Tag string value is too long. Max length is {AsvSdrHelper.RecordTagValueLength}");
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

    /// <summary>
    /// Sets the value of a tag in the current record set.
    /// </summary>
    public Task<MavResult> CurrentRecordSetTag( string tagName, double value, CancellationToken cancel)
    {
        return CurrentRecordSetTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64, BitConverter.GetBytes(value), cancel);
    }
}
