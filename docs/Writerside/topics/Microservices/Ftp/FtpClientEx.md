# Ftp ClientEx

`IFtpClientEx` is a higher-level FTP API built on top of [`IFtpClient`](FtpClient.md).
It adds a local cache of FTP entries and high-level [FTP operations](https://mavlink.io/en/services/ftp.html#ftp-operations).

> This class includes some specific operations needed to develop the [AsvDrones](https://github.com/asv-soft/asv-drones) software.

## Use case

We will repeat the example from [FtpClient](FtpClient.md#use-case) but with enhanced capabilities of the `IFtpClientEx`.

1. Get the `IFtpClientEx` microservice from the drone.
```C#
var ftpClientEx = drone.GetMicroservice<IFtpClientEx>() ?? throw new Exception("Ftp client ex not found");
```

2. Create a group of directories:
```C#
await ftpClientEx.Base.CreateDirectory("/FolderExample", cancel);
await ftpClientEx.Base.CreateDirectory("/FolderExample/dir1", cancel);
await ftpClientEx.Base.CreateDirectory("/FolderExample/dir2", cancel);
await ftpClientEx.Base.CreateDirectory("/FolderExample/dir3", cancel);
```

3. Create files in the directories:
```C#
var res = await ftpClientEx.Base.CreateFile("/FolderExample/dir1/temp.txt", cancel);
var session = res.ReadSession();
await ftpClientEx.Base.TerminateSession(session, cancel);

// Every file creation instantiates a new session
// We need to close the session before we can create a new one

res = await ftpClientEx.Base.CreateFile("/FolderExample/dir2/temp.txt", cancel);
session = res.ReadSession();
await ftpClientEx.Base.TerminateSession(session, cancel);
```

4. Refresh the local entry cache from the remote FTP server (drone):
```C#
await ftpClientEx.Refresh("/", cancel: cancel);
```

5. Print all paths to the console. You should see the files and directories you created.
```C#
foreach (var entry in ftpClientEx.Entries.Values)
{
    Console.WriteLine(entry.Path);
}
```

6. Remove `FolderExample` with all its contents:
```C#
await ftpClientEx.RemoveDirectory("/FolderExample", true, cancel);
```

7. Check that the directory is gone:
```C#
await ftpClientEx.Refresh("/", cancel: cancel);

foreach (var entry in ftpClientEx.Entries.Values)
{
    Console.WriteLine(entry.Path);
}
```

## Api {collapsible="true"}

### [IFtpClientEx](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Client/Ex/IFtpClientEx.cs#L21)

Provides a higher-level, convenience API over a low-level MAVLink FTP client.

| Property  | Type                                               | Description                                                |
|-----------|----------------------------------------------------|------------------------------------------------------------|
| `Base`    | `IFtpClient`                                       | Gets the underlying low-level FTP client.                  |
| `Entries` | `IReadOnlyObservableDictionary<string, IFtpEntry>` | Gets a read-only observable cache of known remote entries. |

| Method                                                                                                                                                                                       | Return Type | Description                                                                                          |
|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------|------------------------------------------------------------------------------------------------------|
| `Refresh(string path, bool recursive = true, CancellationToken cancel = default)`                                                                                                            | `Task`      | Refreshes the local entry cache.                                                                     |
| `DownloadFile(string filePath, Stream streamToSave, IProgress<double>? progress = null, byte partSize = MavlinkFtpHelper.MaxDataSize, CancellationToken cancel = default)`                   | `Task`      | Downloads a remote file into the specified destination stream.                                       |
| `DownloadFile(string filePath, IBufferWriter<byte> bufferToSave, IProgress<double>? progress = null, byte partSize = MavlinkFtpHelper.MaxDataSize, CancellationToken cancel = default)`      | `Task`      | Downloads a remote file into the specified IBufferWriter.                                            |
| `UploadFile(string filePath, Stream streamToUpload, IProgress<double>? progress = null, CancellationToken cancel = default)`                                                                 | `Task`      | Uploads data from the provided stream into a remote file.                                            |
| `BurstDownloadFile(string filePath, Stream streamToSave, IProgress<double>? progress = null, byte partSize = MavlinkFtpHelper.MaxDataSize, CancellationToken cancel = default)`              | `Task<int>` | Downloads a remote file using the MAVLink FTP burst-read mode into the specified destination stream. |
| `BurstDownloadFile(string filePath, IBufferWriter<byte> bufferToSave, IProgress<double>? progress = null, byte partSize = MavlinkFtpHelper.MaxDataSize, CancellationToken cancel = default)` | `Task<int>` | Downloads a remote file using the MAVLink FTP burst-read mode into the specified IBufferWriter.      |
| `RemoveDirectory(string path, bool recursive = true, CancellationToken cancel = default)`                                                                                                    | `Task`      | Removes a remote directory.                                                                          |

#### `IFtpClientEx.Refresh`
| Parameter   | Type                | Description                                                                                      |
|-------------|---------------------|--------------------------------------------------------------------------------------------------|
| `path`      | `string`            | Remote directory path to refresh.                                                                |
| `recursive` | `bool`              | If true, refreshes subdirectories recursively; otherwise refreshes only the specified directory. |
| `cancel`    | `CancellationToken` | Optional cancel token argument.                                                                  |

#### `IFtpClientEx.DownloadFile`
| Parameter      | Type                 | Description                                                                    |
|----------------|----------------------|--------------------------------------------------------------------------------|
| `filePath`     | `string`             | Remote file path to download.                                                  |
| `streamToSave` | `Stream`             | Destination stream that receives the downloaded bytes.                         |
| `progress`     | `IProgress<double>?` | Optional progress reporter.                                                    |
| `partSize`     | `byte`               | Maximum size of each read chunk. Must not exceed MavlinkFtpHelper.MaxDataSize. |
| `cancel`       | `CancellationToken`  | Optional cancel token argument.                                                |

| Parameter      | Type                  | Description                                                                    |
|----------------|-----------------------|--------------------------------------------------------------------------------|
| `filePath`     | `string`              | Remote file path to download.                                                  |
| `bufferToSave` | `IBufferWriter<byte>` | Destination buffer writer that receives the downloaded bytes.                  |
| `progress`     | `IProgress<double>?`  | Optional progress reporter.                                                    |
| `partSize`     | `byte`                | Maximum size of each read chunk. Must not exceed MavlinkFtpHelper.MaxDataSize. |
| `cancel`       | `CancellationToken`   | Optional cancel token argument.                                                |

#### `IFtpClientEx.UploadFile`
| Parameter        | Type                 | Description                                         |
|------------------|----------------------|-----------------------------------------------------|
| `filePath`       | `string`             | Remote file path to create/overwrite and upload to. |
| `streamToUpload` | `Stream`             | Source stream providing bytes to upload.            |
| `progress`       | `IProgress<double>?` | Optional progress reporter.                         |
| `cancel`         | `CancellationToken`  | Optional cancel token argument.                     |

#### `IFtpClientEx.BurstDownloadFile`
| Parameter      | Type                 | Description                                                                     |
|----------------|----------------------|---------------------------------------------------------------------------------|
| `filePath`     | `string`             | Remote file path to download.                                                   |
| `streamToSave` | `Stream`             | Destination stream that receives the downloaded bytes.                          |
| `progress`     | `IProgress<double>?` | Optional progress reporter.                                                     |
| `partSize`     | `byte`               | Maximum size of each burst chunk. Must not exceed MavlinkFtpHelper.MaxDataSize. |
| `cancel`       | `CancellationToken`  | Optional cancel token argument.                                                 |

| Parameter      | Type                  | Description                                                                     |
|----------------|-----------------------|---------------------------------------------------------------------------------|
| `filePath`     | `string`              | Remote file path to download.                                                   |
| `bufferToSave` | `IBufferWriter<byte>` | Destination buffer writer that receives the downloaded bytes.                   |
| `progress`     | `IProgress<double>?`  | Optional progress reporter.                                                     |
| `partSize`     | `byte`                | Maximum size of each burst chunk. Must not exceed MavlinkFtpHelper.MaxDataSize. |
| `cancel`       | `CancellationToken`   | Optional cancel token argument.                                                 |

#### `IFtpClientEx.RemoveDirectory`

> Currently unstable.
> 
> This method sometimes cannot perform recursive deletions.
> { style="warning" }

| Parameter   | Type                 | Description                                                                     |
|-------------|----------------------|---------------------------------------------------------------------------------|
| `path`      | `string`             | Remote directory path to remove.                                                |
| `recursive` | `bool`               | If true, removes all children recursively before removing the directory itself. |
| `cancel`    | `CancellationToken`  | Optional cancel token argument.                                                 |

