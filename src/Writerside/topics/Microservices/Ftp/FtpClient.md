# Ftp Client

FtpClient provides basic operations for communicating with the MAVLink FTP server.
Use the FTP client if you need to exchange files over a MAVLink network, for example, to download logs from the drone (server).

## Use case
As an example we will show you how to create a directory and list its contents.

1. Get FtpClient through the microservice locator:

```C#
var ftpClient = drone.GetMicroservice<IFtpClient>() 
    ?? throw new Exception("Ftp client not found");
```

2. Call CreateDirectory to create a new folder in the root directory:

```C#
var result = await ftpClient.CreateDirectory("/FolderExample", cancel);
```

> You may use `ReadOpCode` to check the operation result.
> If you get `FtpOpcode.Ack`, the operation was successful.

3. Call ListDirectory to get data about the root directory:

```C#
// create buffer to store directory's data
var buffer = new char[MavlinkFtpHelper.FtpEncoding.GetMaxCharCount(MavlinkFtpHelper.MaxDataSize)];
var offset = 0u; // use offsets to scan directories with a lot of entities.
var directory = "/"; // use '/' to scan the root folder
var size = await ftpClient.ListDirectory(directory, offset, buffer, cancel);
```

4. Print the directory listing (decoded characters) to the console:
```C#
Console.WriteLine("Directory: " + string.Join(string.Empty, buffer));
```

> The buffer contains raw directory data returned by the server.
> If you want to parse it into a human-readable structure, check the high-level methods in [FtpClientEx](FtpClientEx.md).

## Api {collapsible="true"}

### [MavlinkFtpClientConfig](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Client/FtpClient.cs#L15)

| Property              | Type   | Default | Description                                                  |
|-----------------------|--------|---------|--------------------------------------------------------------|
| `TimeoutMs`           | `int`  | `500`   | Timeout (in milliseconds) for a single FTP command.          |
| `CommandAttemptCount` | `int`  | `6`     | How many times to retry a command on timeout failure.        |
| `TargetNetworkId`     | `byte` | `0`     | Target MAVLink network ID (target_network) used for routing. |
| `BurstTimeoutMs`      | `int`  | `1000`  | Burst-read inactivity timeout (in milliseconds).             |

### [IFtpClient](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Client/IFtpClient.cs#19)

MAVLink FTP (File Transfer Protocol) client abstraction.
Provides remote filesystem operations over the MAVLink FTP microservice.

| Method                                                                                                                   | Return Type                        | Description                                                                                              |
|--------------------------------------------------------------------------------------------------------------------------|------------------------------------|----------------------------------------------------------------------------------------------------------|
| `OpenFileRead(string path, CancellationToken cancel = default)`                                                          | `Task<ReadHandle>`                 | Opens a file for reading on the remote filesystem.                                                       |
| `OpenFileWrite(string path, CancellationToken cancel = default)`                                                         | `Task<WriteHandle>`                | Opens a file for writing on the remote filesystem.                                                       |
| `CreateDirectory(string path, CancellationToken cancellationToken = default)`                                            | `Task<FileTransferProtocolPacket>` | Creates a directory on the remote filesystem.                                                            |
| `CreateFile(string path, CancellationToken cancellationToken = default)`                                                 | `Task<FileTransferProtocolPacket>` | Creates an empty file on the remote filesystem.                                                          |
| `ResetSessions(CancellationToken cancellationToken = default)`                                                           | `Task<FileTransferProtocolPacket>` | Resets (closes) all FTP sessions on the remote side.                                                     |
| `RemoveDirectory(string path, CancellationToken cancellationToken = default)`                                            | `Task<FileTransferProtocolPacket>` | Removes a directory at the specified path on the remote filesystem.                                      |
| `RemoveFile(string path, CancellationToken cancellationToken = default)`                                                 | `Task<FileTransferProtocolPacket>` | Removes a file at the specified path on the remote filesystem.                                           |
| `CalcFileCrc32(string path, CancellationToken cancellationToken = default)`                                              | `Task<uint>`                       | Calculates CRC32 of a remote file.                                                                       |
| `TruncateFile(TruncateRequest request, CancellationToken cancellationToken = default)`                                   | `Task<FileTransferProtocolPacket>` | Truncates a remote file to the specified byte offset.                                                    |
| `WriteFile(WriteRequest request, Memory<byte> buffer, CancellationToken cancellationToken = default)`                    | `Task<FileTransferProtocolPacket>` | Writes a chunk of bytes into an already opened write session on the remote filesystem.                   |
| `BurstReadFile(ReadRequest request, Action<FileTransferProtocolPacket> onBurstData, CancellationToken cancel = default)` | `Task`                             | Performs burst file read from the remote filesystem. Data is delivered to onBurstData as packets arrive. |
| `Rename(string path1, string path2, CancellationToken cancel = default)`                                                 | `Task<FileTransferProtocolPacket>` | Renames or moves a file/directory on the remote filesystem.                                              |
| `TerminateSession(byte session, CancellationToken cancel = default)`                                                     | `Task`                             | Terminates an FTP session on the remote side.                                                            |
| `ReadFile(ReadRequest request, CancellationToken cancel = default)`                                                      | `Task<FileTransferProtocolPacket>` | Reads a chunk of bytes from an already opened read session on the remote filesystem.                     |
| `ReadFile(ReadRequest request, Memory<byte> buffer, CancellationToken cancel = default)`                                 | `Task<ReadResult>`                 | Reads file data into a byte memory buffer from the remote filesystem.                                    |
| `ReadFile(ReadRequest request, IBufferWriter<byte> buffer, CancellationToken cancel = default)`                          | `Task<ReadResult>`                 | Reads file data into a buffer from the remote filesystem.                                                |
| `ListDirectory(string path, uint offset, CancellationToken cancel = default)`                                            | `Task<FileTransferProtocolPacket>` | Lists a directory starting at the given offset cursor from the remote filesystem.                        |
| `ListDirectory(string path, uint offset, IBufferWriter<byte> buffer, CancellationToken cancel = default)`                | `Task<byte>`                       | Lists directory and writes raw bytes into a buffer from the remote filesystem.                           |
| `ListDirectory(string path, uint offset, IBufferWriter<char> buffer, CancellationToken cancel = default)`                | `Task<byte>`                       | Lists directory and writes decoded characters into a buffer from the remote filesystem.                  |
| `ListDirectory(string path, uint offset, Memory<char> buffer, CancellationToken cancel = default)`                       | `Task<byte>`                       | Lists directory and writes decoded characters into a memory block from the remote filesystem.            |

#### `IFtpClient.OpenFileRead`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | Remote file path.               |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

#### `IFtpClient.OpenFileWrite`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | Remote file path.               |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

#### `IFtpClient.CreateDirectory`
| Parameter           | Type                | Description                     |
|---------------------|---------------------|---------------------------------|
| `path`              | `string`            | Remote directory path.          |
| `cancellationToken` | `CancellationToken` | Optional cancel token argument. |

#### `IFtpClient.CreateFile`
| Parameter           | Type                | Description                     |
|---------------------|---------------------|---------------------------------|
| `path`              | `string`            | Remote file path.               |
| `cancellationToken` | `CancellationToken` | Optional cancel token argument. |

#### `IFtpClient.ResetSessions`
| Parameter           | Type                | Description                     |
|---------------------|---------------------|---------------------------------|
| `cancellationToken` | `CancellationToken` | Optional cancel token argument. |

#### `IFtpClient.RemoveDirectory`
| Parameter           | Type                | Description                     |
|---------------------|---------------------|---------------------------------|
| `path`              | `string`            | Remote directory path.          |
| `cancellationToken` | `CancellationToken` | Optional cancel token argument. |

#### `IFtpClient.RemoveFile`
| Parameter           | Type                | Description                     |
|---------------------|---------------------|---------------------------------|
| `path`              | `string`            | Remote file path.               |
| `cancellationToken` | `CancellationToken` | Optional cancel token argument. |

#### `IFtpClient.CalcFileCrc32`
| Parameter           | Type                | Description                     |
|---------------------|---------------------|---------------------------------|
| `path`              | `string`            | Remote file path.               |
| `cancellationToken` | `CancellationToken` | Optional cancel token argument. |

#### `IFtpClient.TruncateFile`
| Parameter           | Type                | Description                         |
|---------------------|---------------------|-------------------------------------|
| `request`           | `TruncateRequest`   | Truncate request (path and offset). |
| `cancellationToken` | `CancellationToken` | Optional cancel token argument.     |

#### `IFtpClient.WriteFile`
| Parameter           | Type                | Description                                |
|---------------------|---------------------|--------------------------------------------|
| `request`           | `WriteRequest`      | Write request (session, offset, and size). |
| `buffer`            | `Memory<byte>`      | Source buffer.                             |
| `cancellationToken` | `CancellationToken` | Optional cancel token argument.            |

#### `IFtpClient.BurstReadFile`
| Parameter     | Type                                 | Description                                                      |
|---------------|--------------------------------------|------------------------------------------------------------------|
| `request`     | `ReadRequest`                        | Read request (session, offset, and size).                        |
| `onBurstData` | `Action<FileTransferProtocolPacket>` | Callback invoked for each received FTP packet during burst read. |
| `cancel`      | `CancellationToken`                  | Optional cancel token argument.                                  |

#### `IFtpClient.Rename`
| Parameter | Type                | Description                               |
|-----------|---------------------|-------------------------------------------|
| `path1`   | `string`            | Source path.                              |
| `path2`   | `string`            | Destination path (new name for the path). |
| `cancel`  | `CancellationToken` | Optional cancel token argument.           |

#### `IFtpClient.TerminateSession`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `session` | `byte`              | Session id.                     |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

#### `IFtpClient.ReadFile`
| Parameter | Type                | Description                               |
|-----------|---------------------|-------------------------------------------|
| `request` | `ReadRequest`       | Read request (session, offset, and size). |
| `cancel`  | `CancellationToken` | Optional cancel token argument.           |

| Parameter | Type                | Description                               |
|-----------|---------------------|-------------------------------------------|
| `request` | `ReadRequest`       | Read request (session, offset, and size). |
| `buffer`  | `Memory<byte>`      | Destination buffer.                       |
| `cancel`  | `CancellationToken` | Optional cancel token argument.           |

| Parameter | Type                  | Description                               |
|-----------|-----------------------|-------------------------------------------|
| `request` | `ReadRequest`         | Read request (session, offset, and size). |
| `buffer`  | `IBufferWriter<byte>` | Destination buffer writer.                |
| `cancel`  | `CancellationToken`   | Optional cancel token argument.           |

#### `IFtpClient.ListDirectory`
| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | Remote directory path.          |
| `offset`  | `uint`              | Paging offset.                  |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |

| Parameter | Type                  | Description                     |
|-----------|-----------------------|---------------------------------|
| `path`    | `string`              | Remote directory path.          |
| `offset`  | `uint`                | Paging offset.                  |
| `buffer`  | `IBufferWriter<byte>` | Destination buffer writer.      |
| `cancel`  | `CancellationToken`   | Optional cancel token argument. |

| Parameter | Type                  | Description                     |
|-----------|-----------------------|---------------------------------|
| `path`    | `string`              | Remote directory path.          |
| `offset`  | `uint`                | Paging offset.                  |
| `buffer`  | `IBufferWriter<char>` | Destination buffer writer.      |
| `cancel`  | `CancellationToken`   | Optional cancel token argument. |

| Parameter | Type                | Description                     |
|-----------|---------------------|---------------------------------|
| `path`    | `string`            | Remote directory path.          |
| `offset`  | `uint`              | Paging offset.                  |
| `buffer`  | `Memory<char>`      | Destination buffer writer.      |
| `cancel`  | `CancellationToken` | Optional cancel token argument. |
