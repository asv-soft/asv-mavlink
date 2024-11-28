#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using R3;


namespace Asv.Mavlink;

/// <summary>
/// Represents a delegate used to set the mode of an ASV SDR.
/// </summary>
/// <param name="mode">The custom mode to set.</param>
/// <param name="frequencyHz">The frequency in Hertz.</param>
/// <param name="recordRate">The record rate.</param>
/// <param name="sendingThinningRatio">The sending thinning ratio.</param>
/// <param name="referencePower">The reference power.</param>
/// <param name="cancel">A cancellation token to cancel the operation.</param>
/// <returns>A Task that represents the asynchronous set mode operation and returns a MavResult.</returns>
public delegate Task<MavResult> SetModeDelegate(AsvSdrCustomMode mode, ulong frequencyHz, float recordRate,uint sendingThinningRatio, float referencePower, CancellationToken cancel);

/// <summary>
/// Represents a method that starts recording with the given record name.
/// </summary>
/// <param name="recordName">The name of the record.</param>
/// <param name="cancel">The cancellation token to cancel the operation.</param>
/// <returns>A task that represents the asynchronous operation.</returns>
public delegate Task<MavResult> StartRecordDelegate(string recordName, CancellationToken cancel);

/// <summary>
/// Represents a delegate that is used to stop a recording process asynchronously.
/// </summary>
/// <param name="cancel">A cancellation token to stop the recording.</param>
/// <returns>A task representing the asynchronous operation of stopping the recording.</returns>
public delegate Task<MavResult> StopRecordDelegate(CancellationToken cancel);

/// <summary>
/// Delegate for handling the current record set tag event.
/// </summary>
/// <param name="type">The type of the tag.</param>
/// <param name="name">The name of the tag.</param>
/// <param name="value">The value of the tag.</param>
/// <param name="cancel">Cancellation token for cancelling the operation.</param>
/// <returns>A <see cref="Task"/> representing the current record set tag operation.</returns>
public delegate Task<MavResult> CurrentRecordSetTagDelegate(AsvSdrRecordTagType type, string name, byte[] value, CancellationToken cancel);

/// <summary>
/// Delegate representing a system control action.
/// </summary>
/// <param name="action">The system control action to be performed.</param>
/// <param name="cancel">Cancellation token to cancel the action.</param>
/// <returns>A task representing the asynchronous operation of the system control action.</returns>
public delegate Task<MavResult> SystemControlActionDelegate(AsvSdrSystemControlAction action, CancellationToken cancel);

/// <summary>
/// Delegate for starting a mission.
/// </summary>
/// <param name="missionIndex">The index of the mission to start.</param>
/// <param name="cancel">The cancellation token for cancelling the mission start process.</param>
/// <returns>A task representing the result of the mission start operation.</returns>
/// <remarks>
/// The task should return a MavResult object indicating the success or failure of the mission start operation.
/// </remarks>
public delegate Task<MavResult> StartMissionDelegate(ushort missionIndex, CancellationToken cancel);

/// <summary>
/// Represents a delegate for stopping a mission.
/// </summary>
/// <param name="cancel">The cancellation token used to cancel the operation.</param>
/// <returns>A task representing the asynchronous operation. The task result contains the mission stop result.</returns>
public delegate Task<MavResult> StopMissionDelegate(CancellationToken cancel);

/// <summary>
/// Represents a delegate for starting a calibration process.
/// </summary>
/// <param name="cancel">The cancellation token to stop the calibration process.</param>
/// <returns>A task that represents the asynchronous calibration process. The task result is the MavResult of the calibration.</returns>
public delegate Task<MavResult> StartCalibrationDelegate(CancellationToken cancel);

/// <summary>
/// Represents a delegate used to stop a calibration process asynchronously.
/// </summary>
/// <param name="cancel">The cancellation token used to cancel the calibration process.</param>
/// <returns>A task representing the asynchronous operation, which will return an instance of <see cref="MavResult"/> representing the result of the calibration stop operation.</returns>
public delegate Task<MavResult> StopCalibrationDelegate(CancellationToken cancel);

/// <summary>
/// Delegate used to try to read calibration table information.
/// </summary>
/// <param name="tableIndex">The index of the calibration table.</param>
/// <param name="name">The name of the calibration table (output parameter).</param>
/// <param name="size">The size of the calibration table (output parameter).</param>
/// <param name="metadata">The metadata of the calibration table (output parameter).</param>
/// <returns>True if the calibration table information was successfully read, false otherwise.</returns>
/// <remarks>
/// The TryReadCalibrationTableInfoDelegate delegate is used to provide a method to try to read
/// the information of a calibration table specified by its index. The method should populate the
/// output parameters with the corresponding values obtained from the calibration table.
/// </remarks>
public delegate bool TryReadCalibrationTableInfoDelegate(ushort tableIndex, out string? name, out ushort? size, out CalibrationTableMetadata? metadata);

/// <summary>
/// Delegate for trying to read a calibration table row.
/// </summary>
/// <param name="tableIndex">The index of the calibration table.</param>
/// <param name="rowIndex">The index of the row in the calibration table.</param>
/// <param name="row">Out parameter that stores the calibration table row if it is successfully read.</param>
/// <returns>
/// Returns true if the calibration table row is successfully read; otherwise, false.
/// </returns>
public delegate bool TryReadCalibrationTableRowDelegate(ushort tableIndex, ushort rowIndex, out CalibrationTableRow? row);

/// Delegate for writing calibration data to a specific table index.
/// @param tableIndex The index of the table to write the calibration data to.
/// @param metadata The metadata associated with the calibration table.
/// @param items The array of calibration rows to write.
/// /
public delegate void WriteCalibrationDelegate(ushort tableIndex, CalibrationTableMetadata metadata, CalibrationTableRow[] items);

/// <summary>
/// Represents an extended interface for an ASV SDR server.
/// </summary>
public interface IAsvSdrServerEx
{
    /// <summary>
    /// Gets the <see cref="IAsvSdrServer"/> base property.
    /// </summary>
    /// <value>
    /// The <see cref="IAsvSdrServer"/> base property.
    /// </value>
    IAsvSdrServer Base { get; }

    /// <summary>
    /// Gets the custom mode value of the editable property.
    /// </summary>
    /// <value>
    /// The custom mode value of the editable property.
    /// </value>
    /// <remarks>
    /// Use this property to retrieve the custom mode value of the editable property.
    /// </remarks>
    /// <seealso cref="IRxEditableValue{T}" />
    /// <seealso cref="AsvSdrCustomMode" />
    ReactiveProperty<AsvSdrCustomMode> CustomMode { get; }

    /// <summary>
    /// Property to set the mode using a delegate function.
    /// </summary>
    /// <remarks>
    /// The SetMode property is used to set the mode of an object by assigning a delegate function.
    /// This delegate function defines the behavior or functionality of the object's mode.
    /// By assigning a new delegate function to the SetMode property, the mode of the object can be changed.
    /// The SetMode property is a write-only property, meaning it does not provide the current mode or return any value.
    /// Instead, it allows you to assign a new delegate function that defines the desired mode.
    /// </remarks>
    /// <example>
    /// This property can be used to change the mode of an object. Here's an example:
    /// <code>
    /// // Define a class with a delegate for mode change
    /// public class MyObject
    /// {
    /// // Delegate type for mode change
    /// public delegate void SetModeDelegate(int mode);
    /// // Private field to store the mode
    /// private int _mode;
    /// // Public SetMode property
    /// public SetModeDelegate SetMode { set { _mode = value; } }
    /// }
    /// // Create an instance of MyObject
    /// MyObject obj = new MyObject();
    /// // Define a function that sets the mode
    /// void ChangeMode(int newMode)
    /// {
    /// Console.WriteLine("Changing mode to " + newMode);
    /// }
    /// // Assign the ChangeMode function to SetMode property
    /// obj.SetMode = ChangeMode;
    /// </code>
    /// In this example, the ChangeMode function is defined as the delegate type SetModeDelegate.
    /// The SetMode property of the MyObject class is assigned the ChangeMode function, effectively changing the mode of the object.
    /// </example>
    SetModeDelegate? SetMode { set; }

    /// <summary>
    /// Delegate for setting the value of StartRecord.
    /// </summary>
    /// <param name="startRecord">The value to be set for StartRecord.</param>
    StartRecordDelegate? StartRecord { set; }

    /// <summary>
    /// Delegate that sets the stop record function.
    /// </summary>
    /// <param name="stopRecord">
    /// The stop record delegate to set.
    /// </param>
    StopRecordDelegate? StopRecord { set; }

    /// <summary>
    /// Specifies a delegate that is used to set the current record set tag. </summary> <remarks>
    /// This delegate is typically used to set a custom tag on the current record set.
    /// Implementations of this delegate should take a string parameter, which represents the tag to set. </remarks>
    /// /
    CurrentRecordSetTagDelegate? CurrentRecordSetTag { set; }

    /// <summary>
    /// Delegate used to handle system control actions.
    /// </summary>
    /// <param name="action">The system control action to be performed.</param>
    /// <remarks>
    /// The <see cref="SystemControlAction"/> delegate is used to handle various system control actions such as
    /// starting, stopping, or restarting a system service, or performing any other custom system control action.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// SystemControlActionDelegate systemControlHandler = delegate(SystemControlAction action)
    /// {
    /// // Perform the necessary system control action
    /// };
    /// </code>
    /// </example>
    SystemControlActionDelegate? SystemControlAction { set; }

    /// <summary>
    /// Gets or sets the StartMission delegate.
    /// </summary>
    /// <value>
    /// The StartMission delegate.
    /// </value>
    StartMissionDelegate? StartMission { set; }

    /// <summary>
    /// Delegate for stopping a mission.
    /// </summary>
    /// <param name="mission">The mission to be stopped.</param>
    StopMissionDelegate? StopMission { set; }

    /// <summary>
    /// Delegate for starting calibration.
    /// </summary>
    /// <param name="calibrationData">The calibration data to be used for the calibration process.</param>
    StartCalibrationDelegate? StartCalibration { set; }

    /// <summary>
    /// Sets the delegate to be called when the calibration process is stopped.
    /// </summary>
    /// <remarks>
    /// This delegate will be invoked when the calibration process is manually stopped
    /// by the user or when an error occurs during the calibration process.
    /// </remarks>
    /// <param name="StopCalibration">
    /// The delegate to be called when the calibration process is stopped.
    /// </param>
    StopCalibrationDelegate? StopCalibration { set; }

    /// <summary>
    /// Gets or sets the delegate used to try reading calibration table information.
    /// </summary>
    /// <value>
    /// A delegate of type TryReadCalibrationTableInfoDelegate that represents the method used to try reading calibration table information.
    /// </value>
    TryReadCalibrationTableInfoDelegate? TryReadCalibrationTableInfo { set; }

    /// <summary>
    /// Gets or sets the delegate for trying to read a calibration table row.
    /// </summary>
    /// <remarks>
    /// This delegate is used to read a calibration table row. If the operation is successful, it returns
    /// true and assigns the result to the output parameter. If the operation fails, it returns false and
    /// assigns the default value to the output parameter.
    /// </remarks>
    /// <value>
    /// The delegate for trying to read a calibration table row.
    /// </value>
    TryReadCalibrationTableRowDelegate? TryReadCalibrationTableRow { get; set; }

    /// <summary>
    /// Gets or sets the delegate used to write calibration table.
    /// </summary>
    /// <value>
    /// The write calibration table delegate.
    /// </value>
    /// <remarks>
    /// This delegate is used to write a calibration table. The delegate signature should be as follows:
    /// <c>void WriteCalibrationTable(Table table)</c>
    /// </remarks>
    WriteCalibrationDelegate? WriteCalibrationTable { get; set; }

    /// <summary>
    /// Sends a signal with the specified parameters.
    /// </summary>
    /// <param name="unixTime">The Unix time of the signal.</param>
    /// <param name="name">The name of the signal.</param>
    /// <param name="signal">The signal data to send.</param>
    /// <param name="format">The format of the signal data.</param>
    /// <param name="cancel">Optional cancellation token to cancel the signal sending operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result indicates whether the signal was sent successfully.</returns>
    Task<bool> SendSignal(ulong unixTime, string name, ReadOnlyMemory<double> signal, AsvSdrSignalFormat format, CancellationToken cancel = default);
}