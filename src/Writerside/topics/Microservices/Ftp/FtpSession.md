# Ftp Session

Represents an FTP session on the server.
The FTP server uses sessions to manage access to the filesystem.
Each session represents a client connection context for file operations and provides access to an underlying stream.

## Use case

`FtpServerEx` uses sessions by default, but you can implement your own FTP server logic and use `FtpSession` directly.

1. Create a session and open it.
```C#
var session = new FtpSession(2);
session.Open(FtpSession.SessionMode.OpenReadWrite);
```

2. Open a file and get a stream from it.
```C#
var file = File.Create("/folder/test.txt");
session.Stream = file;
```

3. Now you can use the session to get access to the file and do some work with it.

4. When you are done with the file, close the session.
```C#
session.Close();
```

## Api { collapsible="true" }

### [SessionMode](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Server/Ex/Session/FtpSession.cs#L16)

Specifies the mode of the FTP session.

| Value           | Description                                         |
|-----------------|-----------------------------------------------------|
| `Free`          | The session is free and not in use.                 |
| `Unknown`       | The session mode is unknown.                        |
| `OpenRead`      | The session is opened for reading.                  |
| `OpenWrite`     | The session is opened for writing.                  |
| `OpenReadWrite` | The session is opened for both reading and writing. |

### [FtpSession](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Server/Ex/Session/FtpSession.cs#L11)

| Property | Type          | Description                                               |
|----------|---------------|-----------------------------------------------------------|
| `Stream` | `Stream?`     | Gets or sets the data stream associated with the session. |
| `Id`     | `byte`        | Gets the session identifier.                              |
| `Mode`   | `SessionMode` | Gets the current mode of the session.                     |

| Method                                         | Return Type | Description                                                  |
|------------------------------------------------|-------------|--------------------------------------------------------------|
| `Open(SessionMode mode = SessionMode.Unknown)` | `void`      | Opens the session with the specified mode.                   |
| `Close()`                                      | `void`      | Closes the session and disposes of resources.                |
| `CloseAsync()`                                 | `Task`      | Asynchronously closes the session and disposes of resources. |

#### `FtpSession.Open`
| Parameter | Type          | Description                      |
|-----------|---------------|----------------------------------|
| `mode`    | `SessionMode` | The mode to open the session in. |
