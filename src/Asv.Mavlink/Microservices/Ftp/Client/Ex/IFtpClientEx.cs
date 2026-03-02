using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ObservableCollections;

namespace Asv.Mavlink;

/// <summary>
/// Provides a higher-level, convenience API over a low-level MAVLink FTP client.
/// </summary>
/// <remarks>
/// This interface typically adds:
/// <list type="bullet">
/// <item><description>an observable cache of discovered entries (<see cref="Entries"/>),</description></item>
/// <item><description>recursive refresh of directory trees (<see cref="Refresh"/>),</description></item>
/// <item><description>stream/buffer based download and upload helpers with progress reporting.</description></item>
/// </list>
/// </remarks>
public interface IFtpClientEx
{
    /// <summary>
    /// Gets the underlying low-level FTP client.
    /// </summary>
    IFtpClient Base { get; }
    
    /// <summary>
    /// Gets a read-only observable cache of known remote entries.
    /// </summary>
    IReadOnlyObservableDictionary<string, IFtpEntry> Entries { get; }

    /// <summary>
    /// Refreshes the local entry cache.
    /// </summary>
    /// <param name="path">Remote directory path to refresh.</param>
    /// <param name="recursive">
    /// If <see langword="true"/>, refreshes subdirectories recursively;
    /// otherwise refreshes only the specified directory.
    /// </param>
    /// <param name="cancel">A cancellation token that cancels the operation.</param>
    /// <returns><see cref="Task"/></returns>
    Task Refresh(string path, bool recursive = true, CancellationToken cancel = default);
    
    /// <summary>
    /// Downloads a remote file into the specified destination stream.
    /// </summary>
    /// <param name="filePath">Remote file path to download.</param>
    /// <param name="streamToSave">Destination stream that receives the downloaded bytes.</param>
    /// <param name="progress">Optional progress reporter.</param>
    /// <param name="partSize">
    /// Maximum size of each read chunk. Must not exceed <see cref="MavlinkFtpHelper.MaxDataSize"/>.
    /// </param>
    /// <param name="cancel">A cancellation token that cancels the operation.</param>
    /// <returns><see cref="Task"/></returns>
    Task DownloadFile(
        string filePath, 
        Stream streamToSave, 
        IProgress<double>? progress = null, 
        byte partSize = MavlinkFtpHelper.MaxDataSize,
        CancellationToken cancel = default
    );
    
    /// <summary>
    /// Downloads a remote file into the specified <see cref="IBufferWriter{T}"/>.
    /// </summary>
    /// <param name="filePath">Remote file path to download.</param>
    /// <param name="bufferToSave">Destination buffer writer that receives the downloaded bytes.</param>
    /// <param name="progress">Optional progress reporter.</param>
    /// <param name="partSize">
    /// Maximum size of each read chunk. Must not exceed <see cref="MavlinkFtpHelper.MaxDataSize"/>.
    /// </param>
    /// <param name="cancel">A cancellation token that cancels the operation.</param>
    /// <returns><see cref="Task"/></returns>
    Task DownloadFile(
        string filePath, 
        IBufferWriter<byte> bufferToSave, 
        IProgress<double>? progress = null,
        byte partSize = MavlinkFtpHelper.MaxDataSize,
        CancellationToken cancel = default
    );
    
    /// <summary>
    /// Uploads data from the provided stream into a remote file.
    /// </summary>
    /// <param name="filePath">Remote file path to create/overwrite and upload to.</param>
    /// <param name="streamToUpload">Source stream providing bytes to upload.</param>
    /// <param name="progress">Optional progress reporter.</param>
    /// <param name="cancel">A cancellation token that cancels the operation.</param>
    /// <returns><see cref="Task"/></returns>
    Task UploadFile(
        string filePath, 
        Stream streamToUpload, 
        IProgress<double>? progress = null,
        CancellationToken cancel = default
    );
    
    /// <summary>
    /// Downloads a remote file using the MAVLink FTP burst-read mode into the specified destination stream.
    /// </summary>
    /// <remarks>
    /// Burst mode can be faster but may drop packets;
    /// implementations may fall back to manual reads for missing chunks.
    /// </remarks>
    /// <param name="filePath">Remote file path to download.</param>
    /// <param name="streamToSave">Destination stream that receives the downloaded bytes.</param>
    /// <param name="progress">Optional progress reporter.</param>
    /// <param name="partSize">
    /// Maximum size of each burst chunk. Must not exceed <see cref="MavlinkFtpHelper.MaxDataSize"/>.
    /// </param>
    /// <param name="cancel">A cancellation token that cancels the operation.</param>
    /// <returns>
    /// A task that returns the number of chunks that had to be fetched manually (e.g., due to missing burst packets).
    /// </returns>
    Task<int> BurstDownloadFile(
        string filePath, 
        Stream streamToSave, 
        IProgress<double>? progress = null,
        byte partSize = MavlinkFtpHelper.MaxDataSize, 
        CancellationToken cancel = default
    );
    
    /// <summary>
    /// Downloads a remote file using the MAVLink FTP burst-read mode into the specified <see cref="IBufferWriter{T}"/>.
    /// </summary>
    /// <remarks>
    /// Burst mode can be faster but may drop packets;
    /// implementations may fall back to manual reads for missing chunks.
    /// </remarks>
    /// <param name="filePath">Remote file path to download.</param>
    /// <param name="bufferToSave">Destination buffer writer that receives the downloaded bytes.</param>
    /// <param name="progress">Optional progress reporter.</param>
    /// <param name="partSize">
    /// Maximum size of each burst chunk. Must not exceed <see cref="MavlinkFtpHelper.MaxDataSize"/>.
    /// </param>
    /// <param name="cancel">A cancellation token that cancels the operation.</param>
    /// <returns>
    /// A task that returns the number of chunks that had to be fetched manually (e.g., due to missing burst packets).
    /// </returns>
    Task<int> BurstDownloadFile(
        string filePath, 
        IBufferWriter<byte> bufferToSave, 
        IProgress<double>? progress = null,
        byte partSize = MavlinkFtpHelper.MaxDataSize, 
        CancellationToken cancel = default
    );
    
    /// <summary>
    /// Removes a remote directory.
    /// </summary>
    /// <param name="path">Remote directory path to remove.</param>
    /// <param name="recursive">
    /// If <see langword="true"/>, removes all children recursively before removing the directory itself.
    /// </param>
    /// <param name="cancel">A cancellation token that cancels the operation.</param>
    /// <returns><see cref="Task"/></returns>
    Task RemoveDirectory(string path, bool recursive = true, CancellationToken cancel = default);
}