using System;
using System.Threading;
using System.Threading.Tasks;

namespace Asv.Mavlink;

/// <summary>
/// Represents an FTP server.
/// </summary>
public interface IFtpServer
{
    /// <summary>
    /// Sends an FTP packet with the specified payload, device identity, and cancellation token.
    /// </summary>
    /// <param name="payload">The payload of the FTP packet.</param>
    /// <param name="identity">The identity of the device.</param>
    /// <param name="cancel">The cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation of sending the FTP packet.
    /// </returns>
    public Task SendFtpPacket(FtpMessagePayload payload, DeviceIdentity identity, CancellationToken cancel);

    /// <summary>
    /// Gets the observable sequence that represents any request made on the FTP server.
    /// </summary>
    /// <value>
    /// An <see cref="IObservable{T}"/> of a tuple that contains the device identity and FTP message payload.
    /// </value>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> AnyRequest { get; }

    /// <summary>
    /// Gets or sets the ResetSessionsRequest property.
    /// </summary>
    /// <remarks>
    /// This property represents an <see cref="IObservable"/> that emits tuples consisting of a <see cref="DeviceIdentity"/> and a <see cref="FtpMessagePayload"/> for resetting sessions
    /// .
    /// </remarks>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> ResetSessionsRequest { get; set; }

    /// <summary>
    /// Gets or sets the TerminateSessionRequest property.
    /// </summary>
    /// <value>
    /// The TerminateSessionRequest property is an observable sequence of tuples.
    /// Each tuple contains a DeviceIdentity object representing the device information
    /// and an FtpMessagePayload object representing the message payload for terminating a session.
    /// </value>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> TerminateSessionRequest { get; set; }

    /// <summary>
    /// The observable property representing a request to list the directories on a device.
    /// </summary>
    /// <value>
    /// An observable sequence of tuples, where each tuple contains a <see cref="DeviceIdentity"/> object representing
    /// the device identity and a <see cref="FtpMessagePayload"/> object representing the FTP message payload.
    /// </value>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> ListDirectoryRequest { get; set; }

    /// <summary>
    /// Gets or sets the observable sequence of open file read-only requests.
    /// </summary>
    /// <value>
    /// The observable sequence of open file read-only requests. Each request contains a <see cref="DeviceIdentity"/> and a <see cref="FtpMessagePayload"/>.
    /// </value>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> OpenFileRORequest { get; set; }

    /// <summary>
    /// Gets or sets the ReadFileRequest property.
    /// This property is responsible for handling requests for reading a file.
    /// It is an IObservable that emits a tuple containing the DeviceIdentity and FtpMessagePayload when a request is made.
    /// </summary>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> ReadFileRequest { get; set; }

    /// <summary>
    /// Represents an observable property that allows the user to create file requests.
    /// </summary>
    /// <value>
    /// An observable sequence of tuples where the first element is the device identity and the second element is the FTP message payload.
    /// </value>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> CreateFileRequest { get; set; }

    /// <summary>
    /// Gets or sets the observable sequence for WriteFileRequest events.
    /// </summary>
    /// <value>
    /// The observable sequence of tuples representing the device identity and the FTP message payload associated with a WriteFileRequest.
    /// </value>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> WriteFileRequest { get; set; }

    /// <summary>
    /// Gets or sets the remove file request event.
    /// </summary>
    /// <value>
    /// An observable sequence of tuples consisting of DeviceIdentity and FtpMessagePayload.
    /// </value>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> RemoveFileRequest { get; set; }

    /// <summary>
    /// Gets or sets the property used to send a create directory request to an FTP server.
    /// </summary>
    /// <value>
    /// An <see cref="IObservable"/> that emits tuples containing a <see cref="DeviceIdentity"/>
    /// and a <see cref="FtpMessagePayload"/> in response to the create directory request.
    /// </value>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> CreateDirectoryRequest { get; set; }

    /// <summary>
    /// Gets or sets an observable sequence of tuples containing the device identity and FTP message payload for the "RemoveDirectory" request.
    /// </summary>
    /// <remarks>
    /// This property represents an asynchronous stream of messages for the "RemoveDirectory" request. Each message in the stream contains
    /// the device identity and the FTP message payload associated with the request. Subscribers can listen to this property to receive
    /// notifications whenever a "RemoveDirectory" request is made.
    /// </remarks>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> RemoveDirectoryRequest { get; set; }

    /// <summary>
    /// Gets or sets the property representing an IObservable that emits a tuple containing the device identity and FTP message payload when an OpenFileWORequest is made.
    /// </summary>
    /// <value>
    /// An IObservable of tuples containing the device identity and FTP message payload.
    /// </value>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> OpenFileWORequest { get; set; }

    /// <summary>
    /// Gets or sets the observable stream of TruncateFileRequest messages.
    /// Each message contains a DeviceIdentity and FtpMessagePayload.
    /// </summary>
    /// <remarks>
    /// The TruncateFileRequest message is used to request truncation of a file on a device.
    /// Truncation is the process of reducing the size of a file to a specified length.
    /// </remarks>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> TruncateFileRequest { get; set; }

    /// <summary>
    /// This property represents the request to rename a file or directory.
    /// </summary>
    /// <remarks>
    /// It is an <see cref="IObservable{T}"/> that emits a tuple containing the <see cref="DeviceIdentity"/>
    /// and <see cref="FtpMessagePayload"/> representing the file or directory to be renamed.
    /// </remarks>
    /// <value>
    /// The value of this property is an <see cref="IObservable{T}"/> that emits a tuple containing the
    /// <see cref="DeviceIdentity"/> and <see cref="FtpMessagePayload"/> when a rename request is made.
    /// </value>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> RenameRequest { get; set; }

    /// <summary>
    /// Gets or sets the observable representing the calculation request to obtain the CRC32 value of a file.
    /// The observer should emit a tuple containing the device identity and the FTP message payload.
    /// </summary>
    /// <value>
    /// An observable that emits tuples of type <see cref="DeviceIdentity"/> and <see cref="FtpMessagePayload"/>.
    /// </value>
    public IObservable<(DeviceIdentity, FtpMessagePayload)> CalcFileCRC32Request { get; set; }

    /// <summary>
    /// Gets or sets the burst read file request observable. </summary>
    /// <value>
    /// The burst read file request observable. The observable emits a tuple containing the DeviceIdentity and the FtpMessagePayload. </value>
    /// /
    public IObservable<(DeviceIdentity, FtpMessagePayload)> BurstReadFileRequest { get; set; }
}