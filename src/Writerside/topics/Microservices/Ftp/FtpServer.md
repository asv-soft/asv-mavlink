# Ftp Server

[MAVLink FTP](https://mavlink.io/en/services/ftp.html) (File Transfer Protocol) server abstraction.

`FtpServer` is an abstraction. 
To define its behavior, assign handlers (delegates) for the FTP operations. 
Once the handlers are set, `FtpServer` can process client requests.

## Use case

1. Get the FtpServer instance from the server device.
```C#
var ftpServer = device.GetFtp();
```

2. Implement the delegates of the FtpServer.
```C#
ftpServer.CalcFileCrc32 = async (path, cancel) =>
{
    // Implement your own logic to calculate the CRC32 of a file.
    return crc32;
};
```

Now you can use the FtpServer to handle the client’s CRC32 calculation requests.

## [Delegates](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Server/IFtpServer.cs#L13) {collapsible="true"}

| Delegate                                                                                                   | Return Type             | Description                                                                       |
|------------------------------------------------------------------------------------------------------------|-------------------------|-----------------------------------------------------------------------------------|
| `OpenFileReadDelegate(string path, CancellationToken cancel = default)`                                    | `Task<ReadHandle>`      | Delegate that opens a remote file for reading and returns a read session handle.  |
| `OpenFileWriteDelegate(string path, CancellationToken cancel = default)`                                   | `Task<WriteHandle>`     | Delegate that opens a remote file for writing and returns a write session handle. |
| `FileReadDelegate(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default)`           | `Task<ReadResult>`      | Delegate that reads file data from an opened read session.                        |
| `RenameDelegate(string path1, string path2, CancellationToken cancel = default)`                           | `Task`                  | Delegate that renames (or moves) a file or directory.                             |
| `TerminateSessionDelegate(byte session, CancellationToken cancel = default)`                               | `Task`                  | Delegate that terminates a specific session.                                      |
| `ResetSessionsDelegate(CancellationToken cancel = default)`                                                | `Task`                  | Delegate that resets (terminates) all active sessions.                            |
| `ListDirectoryDelegate(string path, uint offset, Memory<char> buffer, CancellationToken cancel = default)` | `Task<byte>`            | Delegate that lists entries in a directory.                                       |
| `CreateDirectory(string path, CancellationToken cancel = default)`                                         | `Task`                  | Delegate that creates a new directory.                                            |
| `CreateFile(string path, CancellationToken cancel = default)`                                              | `Task<byte>`            | Delegate that creates a new file and returns its session handle.                  |
| `RemoveFile(string path, CancellationToken cancel = default)`                                              | `Task`                  | Delegate that removes a file.                                                     |
| `RemoveDirectory(string path, CancellationToken cancel = default)`                                         | `Task`                  | Delegate that removes a directory.                                                |
| `CalcFileCrc32(string path, CancellationToken cancel = default)`                                           | `Task<uint>`            | Delegate that calculates CRC32 checksum for a file.                               |
| `TruncateFile(TruncateRequest request, CancellationToken cancel = default)`                                | `Task`                  | Delegate that truncates a file to a specified length.                             |
| `BurstReadFileDelegate(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default)`      | `Task<BurstReadResult>` | Delegate that performs a burst read from an opened read session.                  |
| `WriteFile(WriteRequest request, Memory<byte> buffer, CancellationToken cancel = default)`                 | `Task`                  | Delegate that writes data to an opened write session.                             |

### `OpenFileReadDelegate`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | Remote file path.               |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

### `OpenFileWriteDelegate`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | Remote file path.               |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |
 
### `FileReadDelegate`
| Parameter | Type                | Description                                   |
|-----------|---------------------|-----------------------------------------------|
| `request` | `ReadRequest`       | Read request details (session, offset, etc.). |
| `buffer`  | `Memory<byte>`      | Memory buffer to read data into.              |
| `cancel`  | `CancellationToken` | Optional cancel token argument.               |

### `RenameDelegate`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path1`   | `string`            | Original path.                  |
| `path2`   | `string`            | New path.                       |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

### `TerminateSessionDelegate`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `session` | `byte`              | Session handle to terminate.    |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

### `ResetSessionsDelegate`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

### `ListDirectoryDelegate`
| Parameter | Type                | Description                             |
|-----------|---------------------|-----------------------------------------|
| `path`    | `string`            | Directory path.                         |
| `offset`  | `uint`              | Offset to start listing from.           |
| `buffer`  | `Memory<char>`      | Memory buffer to write entry list into. |
| `cancel`  | `CancellationToken` | Optional cancel token argument.         |

### `CreateDirectory`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | Directory path to create.       |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

### `CreateFile`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | File path to create.            |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

### `RemoveFile`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | File path to remove.            |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

### `RemoveDirectory`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | Directory path to remove.       |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

### `CalcFileCrc32`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | File path.                      |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

### `TruncateFile`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `request` | `TruncateRequest`   | Truncate request details.       |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

### `BurstReadFileDelegate`
| Parameter | Type                | Description                      |
|-----------|---------------------|----------------------------------|
| `request` | `ReadRequest`       | Read request details.            |
| `buffer`  | `Memory<byte>`      | Memory buffer to read data into. |
| `cancel`  | `CancellationToken` | Optional cancel token argument.  |

### `WriteFile`
| Parameter | Type                | Description                             |
|-----------|---------------------|-----------------------------------------|
| `request` | `WriteRequest`      | Write request details.                  |
| `buffer`  | `Memory<byte>`      | Memory buffer containing data to write. |
| `cancel`  | `CancellationToken` | Optional cancel token argument.         |

## Api { collapsible="true" }

### [MavlinkFtpServerConfig](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Server/FtpServer.cs#L17)

Configuration for the MAVLink FTP server.

| Property                | Type   | Default | Description                                                       |
|-------------------------|--------|---------|-------------------------------------------------------------------|
| `NetworkId`             | `byte` | `0`     | Gets or sets the target network ID.                               |
| `BurstReadChunkDelayMs` | `int`  | `30`    | Gets or sets the delay in milliseconds between burst read chunks. |

### [IFtpServer](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Server/IFtpServer.cs#L139)

MAVLink FTP (File Transfer Protocol) server abstraction.

| Property           | Type                        | Description                                                        |
|--------------------|-----------------------------|--------------------------------------------------------------------|
| `Rename`           | `RenameDelegate?`           | Sets the handler that renames (or moves) a file/directory.         |
| `OpenFileRead`     | `OpenFileReadDelegate?`     | Sets the handler that opens a file for reading.                    |
| `OpenFileWrite`    | `OpenFileWriteDelegate?`    | Sets the handler that opens a file for writing.                    |
| `FileRead`         | `FileReadDelegate?`         | Sets the handler that reads file data from an opened read session. |
| `TerminateSession` | `TerminateSessionDelegate?` | Sets the handler that terminates a session.                        |
| `ListDirectory`    | `ListDirectoryDelegate?`    | Sets the handler that lists directory entries.                     |
| `ResetSessions`    | `ResetSessionsDelegate?`    | Sets the handler that resets (terminates) all sessions.            |
| `CreateDirectory`  | `CreateDirectory?`          | Sets the handler that creates a directory.                         |
| `CreateFile`       | `CreateFile?`               | Sets the handler that creates a file.                              |
| `RemoveFile`       | `RemoveFile?`               | Gets or sets the handler that removes a file.                      |
| `RemoveDirectory`  | `RemoveDirectory?`          | Sets the handler that removes a directory.                         |
| `CalcFileCrc32`    | `CalcFileCrc32?`            | Sets the handler that calculates CRC32 for a file.                 |
| `TruncateFile`     | `TruncateFile?`             | Sets the handler that truncates a file.                            |
| `BurstReadFile`    | `BurstReadFileDelegate?`    | Sets the handler that performs burst reads.                        |
| `WriteFile`        | `WriteFile?`                | Sets the handler that writes file data to an open write session.   |
