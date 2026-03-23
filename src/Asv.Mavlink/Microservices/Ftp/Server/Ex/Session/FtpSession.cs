using System;
using System.IO;
using System.Threading.Tasks;

namespace Asv.Mavlink;

/// <summary>
/// Represents an FTP session on the server.
/// </summary>
/// <param name="id">The session identifier.</param>
public sealed class FtpSession(byte id) : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Specifies the mode of the FTP session.
    /// </summary>
    public enum SessionMode
    {
        /// <summary>
        /// The session is free and not in use.
        /// </summary>
        Free,
        /// <summary>
        /// The session mode is unknown.
        /// </summary>
        Unknown,
        /// <summary>
        /// The session is opened for reading.
        /// </summary>
        OpenRead,
        /// <summary>
        /// The session is opened for writing.
        /// </summary>
        OpenWrite,
        /// <summary>
        /// The session is opened for both reading and writing.
        /// </summary>
        OpenReadWrite,
    }
    
    private bool _disposed;
    private Stream? _stream;
    
    /// <summary>
    /// Gets or sets the data stream associated with the session.
    /// </summary>
    /// <exception cref="FtpException">Thrown when trying to set the stream for a free session.</exception>
    public Stream? Stream
    {
        get => _stream;
        set
        {
            if (Mode == SessionMode.Free )
            {
                throw new FtpException("Session is not at work");
            }
            
            _stream = value;
        }
    }
    
    /// <summary>
    /// Gets the session identifier.
    /// </summary>
    public byte Id { get; } = id;
    
    /// <summary>
    /// Gets the current mode of the session.
    /// </summary>
    public SessionMode Mode { get; private set; } = SessionMode.Free;

    /// <summary>
    /// Opens the session with the specified mode.
    /// </summary>
    /// <param name="mode">The mode to open the session in.</param>
    /// <exception cref="FtpException">Thrown when the session is already opened.</exception>
    public void Open(SessionMode mode = SessionMode.Unknown)
    {
        if (Mode != SessionMode.Free)
        {
            throw new FtpException("Session is already opened");
        }
        
        Mode = mode;
    }

    /// <summary>
    /// Closes the session and disposes of resources.
    /// </summary>
    public void Close()
    {
        Mode = SessionMode.Free;
        Dispose();
    }
    
    /// <summary>
    /// Asynchronously closes the session and disposes of resources.
    /// </summary>
    /// <returns>A task that represents the asynchronous close operation.</returns>
    public async Task CloseAsync()
    {
        Mode = SessionMode.Free;
        await DisposeAsync().ConfigureAwait(false);
    }
    
    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            _stream?.Dispose();
        }
        _disposed = true;
    }
        
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    
    private async ValueTask DisposeAsyncCore()
    {
        if (!_disposed)
        {
            if (_stream is not null)
            {
                await _stream.DisposeAsync().ConfigureAwait(false);
            }
            
            _disposed = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    ~FtpSession()
    {
        Dispose(disposing: false);
    }
}
