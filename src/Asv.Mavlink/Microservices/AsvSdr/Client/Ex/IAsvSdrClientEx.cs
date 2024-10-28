#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using DynamicData;
using R3;

namespace Asv.Mavlink;

public interface IAsvSdrClientEx:IMavlinkMicroserviceClient
{
    /// <summary>
    /// Gets an instance of IAsvSdrClient.
    /// </summary>
    /// <remarks>
    /// This property represents the base client for the ASV SDR functionality.
    IAsvSdrClient Base { get; }

    /// <summary>
    /// Gets the supported modes of the IRxValue object.
    /// </summary>
    /// <value>
    /// The supported
    ReadOnlyReactiveProperty<AsvSdrCustomModeFlag> SupportedModes { get; }

    /// <summary>
    /// Gets the custom mode value.
    /// </summary>
    /// <returns>The custom mode value.</returns>
    ReadOnlyReactiveProperty<AsvSdrCustomMode> CustomMode { get; }

    /// <summary>
    /// Gets the value representing the count of records.
    /// </summary>
    /// <remarks>
    /// This property provides access to the number of records
    ReadOnlyReactiveProperty<ushort> RecordsCount { get; }
    IObservable<IChangeSet<IAsvSdrClientRecord,Guid>> Records { get; }

    /// <summary>
    /// Gets the current record value.
    /// </summary>
    /// <returns>
    /// An interface representing a reactive value of type Guid
    ReadOnlyReactiveProperty<Guid> CurrentRecord { get; }

    /// <summary>
    /// Gets the value indicating whether the record has started.
    /// </summary>
    /// <returns>An IRxValue representing the current status of the recording. True if recording has started
    ReadOnlyReactiveProperty<bool> IsRecordStarted { get; }

    /// <summary>
    /// Deletes the specified record.
    /// </summary>
    /// <param name="recordName">The name of the record to delete.</param
    Task DeleteRecord(Guid recordName, CancellationToken cancel = default);

    /// <summary>
    /// Downloads the record list.
    /// </summary>
    /// <param name="progress">The progress object used to report the download progress. This parameter is
    Task<bool> DownloadRecordList(IProgress<double> progress = null, CancellationToken cancel = default);

    /// <summary>
    /// Sets the mode of the ASV/SDR device.
    /// </summary>
    /// <param name="mode">The desired mode to set.</param>
    /// <param name
    Task<MavResult> SetMode(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate, uint sendingThinningRatio, float refPower, CancellationToken cancel = default);

    /// <summary>
    /// Starts recording with the provided record name.
    /// </summary>
    /// <param name="recordName">The name of the record to start.</param>
    /// <param name="cancel">
    Task<MavResult> StartRecord(string recordName, CancellationToken cancel = default);

    /// <summary>
    /// Stops the recording process.
    /// </summary>
    /// <param name="cancel">The cancellation token to cancel the operation (default is <see cref="CancellationToken.None"/>).</param>
    /// <returns>A <see cref
    Task<MavResult> StopRecord(CancellationToken cancel = default);
    Task<MavResult> CurrentRecordSetTag(string tagName, AsvSdrRecordTagType type, byte[] rawValue , CancellationToken cancel = default);

    /// <summary>
    /// Represents a method for performing
    Task<MavResult> SystemControlAction(AsvSdrSystemControlAction action, CancellationToken cancel = default);

    /// <summary>
    /// Starts a mission with the specified mission index and cancellation token.
    /// </summary>
    /// <param name="missionIndex">The index of the mission to start.
    Task<MavResult> StartMission(ushort missionIndex = 0, CancellationToken cancel = default);

    /// <summary>
    /// Stops the active mission.
    /// </summary>
    /// <param name="cancel">The cancellation token to stop
    Task<MavResult> StopMission(CancellationToken cancel = default);
    
    #region Calibration

    /// <summary>
    /// Starts the calibration process.
    /// </summary>
    /// <param name="cancel">A cancellation token that can be used to cancel the calibration process (optional).</
    Task<MavResult> StartCalibration(CancellationToken cancel = default);
    Task<MavResult> StopCalibration(CancellationToken cancel = default);

    /// <summary>
    /// Gets the calibration state of the property.
    /// </summary>
    /// <returns>
    /// An
    ReadOnlyReactiveProperty<AsvSdrCalibState> CalibrationState { get; }
    ReadOnlyReactiveProperty<ushort> CalibrationTableRemoteCount { get; }

    /// <summary>
    /// Reads the calibration table list.
    /// </summary>
    /// <param name="progress">An optional progress instance that can be used to report the progress
    Task ReadCalibrationTableList(IProgress<double>? progress = null,CancellationToken cancel = default);

    /// <summary>
    /// Retrieves the calibration table for a given name.
    /// </summary>
    /// <param name="name">The name of the calibration table to retrieve.</param>
    Task<AsvSdrClientCalibrationTable?> GetCalibrationTable(string name, CancellationToken cancel = default);

    /// <summary>
    /// Get the observable collection of calibration tables.
    /// </summary>
    /// <remarks>
    /// The CalibrationTables property returns an
    IObservable<IChangeSet<AsvSdrClientCalibrationTable,string>> CalibrationTables { get; }
        
    #endregion

    /// <summary>
    public async Task StartCalibrationAndCheckResult( CancellationToken cancel = default)
    {
        var result = await StartCalibration(cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"Start calibration failed. Result: {result}");
    }

    /// <summary>
    /// Stops the calibration process and checks the result.
    /// </summary>
    /// <param name="cancel">Cancellation token to
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
    /// <param name="cancel">The
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
    /// <param
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
    /// </
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
    /// <param name="tagName">The name
    public async Task CurrentRecordSetTagAndCheckResult( string tagName, long value, CancellationToken cancel)
    {
        var result = await CurrentRecordSetTag(tagName, value, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"Set tag failed. Result: {result}");
    }

    /// <summary>
    /// Sets the tag value for the current record and checks the result.
    /// </summary>
    /// <param name
    public async Task CurrentRecordSetTagAndCheckResult( string tagName, double value, CancellationToken cancel)
    {
        var result = await CurrentRecordSetTag(tagName, value, cancel).ConfigureAwait(false);
        if (result != MavResult.MavResultAccepted) throw new AsvSdrException($"Set tag failed. Result: {result}");
    }

    /// <summary>
    /// Sets the current record tag with the specified name and value.
    /// </summary>
    /// <param name="
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
    /// <param name="tagName">The name of the tag
    public Task<MavResult> CurrentRecordSetTag( string tagName, double value, CancellationToken cancel)
    {
        return CurrentRecordSetTag(tagName, AsvSdrRecordTagType.AsvSdrRecordTagTypeReal64, BitConverter.GetBytes(value), cancel);
    }
}
