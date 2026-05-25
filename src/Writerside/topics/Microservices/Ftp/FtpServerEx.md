# Ftp ServerEx

Extended MAVLink FTP server microservice.
Provides high-level methods for file and directory operations over MAVLink FTP.
It is a base implementation of the MAVLink FTP server with [sessions](FtpSession.md).

## Use case

This class is just an implementation of delegates from the base [FtpServer](FtpServer.md).
You can call methods manually, but it is not the default way of using FtpServerEx.

## Api { collapsible="true" }

### [MavlinkFtpServerExConfig](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Server/FtpServerEx.cs#L19)

Configuration for the extended MAVLink FTP server.

| Property        | Type     | Default | Description                                                                                                 |
|-----------------|----------|---------|-------------------------------------------------------------------------------------------------------------|
| `RootDirectory` | `string` | `/`     | Gets or sets the root directory for the FTP server. All file operations will be relative to this directory. |

### [IFtpServerEx](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Server/IFtpServerEx.cs#L11)

Extended MAVLink FTP server microservice.

| Property | Type         | Description                        |
|----------|--------------|------------------------------------|
| `Base`   | `IFtpServer` | Gets the base FTP server instance. |

| Method                                                                                             | Return Type             | Description                                  |
|----------------------------------------------------------------------------------------------------|-------------------------|----------------------------------------------|
| `OpenFileRead(string path, CancellationToken cancel = default)`                                    | `Task<ReadHandle>`      | Opens a file for reading.                    |
| `OpenFileWrite(string path, CancellationToken cancel = default)`                                   | `Task<WriteHandle>`     | Opens a file for writing.                    |
| `FileRead(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default)`           | `Task<ReadResult>`      | Reads data from a file using a read request. |
| `Rename(string path1, string path2, CancellationToken cancel = default)`                           | `Task`                  | Renames a file or directory.                 |
| `TerminateSession(byte session, CancellationToken cancel = default)`                               | `Task`                  | Terminates a specific FTP session.           |
| `ResetSessions(CancellationToken cancel = default)`                                                | `Task`                  | Resets all active FTP sessions.              |
| `ListDirectory(string path, uint offset, Memory<char> buffer, CancellationToken cancel = default)` | `Task<byte>`            | Lists the contents of a directory.           |
| `CreateDirectory(string path, CancellationToken cancel = default)`                                 | `Task`                  | Creates a new directory.                     |
| `CreateFile(string path, CancellationToken cancel = default)`                                      | `Task<byte>`            | Creates a new file.                          |
| `RemoveFile(string path, CancellationToken cancel = default)`                                      | `Task`                  | Removes a file.                              |
| `RemoveDirectory(string path, CancellationToken cancel = default)`                                 | `Task`                  | Removes a directory.                         |
| `CalcFileCrc32(string path, CancellationToken cancel = default)`                                   | `Task<uint>`            | Calculates the CRC32 checksum of a file.     |
| `TruncateFile(TruncateRequest request, CancellationToken cancel = default)`                        | `Task`                  | Truncates a file to a specific length.       |
| `BurstReadFile(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default)`      | `Task<BurstReadResult>` | Performs a burst read from a file.           |
| `WriteFile(WriteRequest request, Memory<byte> buffer, CancellationToken cancel = default)`         | `Task`                  | Writes data to a file.                       |

#### `IFtpServerEx.OpenFileRead`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | The path of the file to open.   |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

#### `IFtpServerEx.OpenFileWrite`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | The path of the file to open.   |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

#### `IFtpServerEx.FileRead`
| Parameter | Type                | Description                                                 |
|-----------|---------------------|-------------------------------------------------------------|
| `request` | `ReadRequest`       | The read request containing session and offset information. |
| `buffer`  | `Memory<byte>`      | The buffer to store the read data.                          |
| `cancel`  | `CancellationToken` | Optional cancel token argument.                             |

#### `IFtpServerEx.Rename`
| Parameter | Type                | Description                                |
|-----------|---------------------|--------------------------------------------|
| `path1`   | `string`            | The current path of the file or directory. |
| `path2`   | `string`            | The new path of the file or directory.     |
| `cancel`  | `CancellationToken` | Optional cancel token argument.            |

#### `IFtpServerEx.TerminateSession`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `session` | `byte`              | The session ID to terminate.    |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

#### `IFtpServerEx.ResetSessions`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

#### `IFtpServerEx.ListDirectory`
| Parameter | Type                | Description                                              |
|-----------|---------------------|----------------------------------------------------------|
| `path`    | `string`            | The path of the directory to list.                       |
| `offset`  | `uint`              | The offset within the directory listing.                 |
| `buffer`  | `Memory<char>`      | The buffer to store the directory listing as characters. |
| `cancel`  | `CancellationToken` | Optional cancel token argument.                          |

#### `IFtpServerEx.CreateDirectory`
| Parameter | Type                | Description                          |
|-----------|---------------------|--------------------------------------|
| `path`    | `string`            | The path of the directory to create. |
| `cancel`  | `CancellationToken` | Optional cancel token argument.      |

#### `IFtpServerEx.CreateFile`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | The path of the file to create. |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

#### `IFtpServerEx.RemoveFile`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | The path of the file to remove. |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

#### `IFtpServerEx.RemoveDirectory`
| Parameter | Type                | Description                          |
|-----------|---------------------|--------------------------------------|
| `path`    | `string`            | The path of the directory to remove. |
| `cancel`  | `CancellationToken` | Optional cancel token argument.      |

#### `IFtpServerEx.CalcFileCrc32`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | The path of the file.           |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

#### `IFtpServerEx.TruncateFile`
| Parameter | Type                | Description                                                  |
|-----------|---------------------|--------------------------------------------------------------|
| `request` | `TruncateRequest`   | The truncate request containing path and length information. |
| `cancel`  | `CancellationToken` | Optional cancel token argument.                              |

#### `IFtpServerEx.BurstReadFile`
| Parameter | Type                | Description                                                 |
|-----------|---------------------|-------------------------------------------------------------|
| `request` | `ReadRequest`       | The read request containing session and offset information. |
| `buffer`  | `Memory<byte>`      | The buffer to store the read data.                          |
| `cancel`  | `CancellationToken` | Optional cancel token argument.                             |

#### `IFtpServerEx.WriteFile`
| Parameter | Type                | Description                                                  |
|-----------|---------------------|--------------------------------------------------------------|
| `request` | `WriteRequest`      | The write request containing session and offset information. |
| `buffer`  | `Memory<byte>`      | The buffer containing the data to write.                     |
| `cancel`  | `CancellationToken` | Optional cancel token argument.                              |
