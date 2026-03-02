# Ftp Entry

[Source code](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Client/Ex/Entry/IFtpEntry.cs#L22)

Represents a single filesystem entry (file or directory) on a remote MAVLink FTP server.

| Property     | Type           | Description                                                           |
|--------------|----------------|-----------------------------------------------------------------------|
| `ParentPath` | `string`       | Gets the parent directory path of this entry.                         |
| `Path`       | `string`       | Gets the full path of this entry.                                     |
| `Name`       | `string`       | Gets the entry name (file or directory name) without its parent path. |
| `Type`       | `FtpEntryType` | Gets the entry type (file or directory).                              |

## [FtpEntryType](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Client/Ex/Entry/IFtpEntry.cs#L6)

Specifies the type of entry returned by the MAVLink FTP directory listing.

| Value       | Description           |
|-------------|-----------------------|
| `File`      | A regular file.       |
| `Directory` | A directory (folder). |

## [FtpFile](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Client/Ex/Entry/FtpFile.cs#L4)

Inherits from [IFtpEntry](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Client/Ex/Entry/IFtpEntry#L22). 
Class represents a file of a filesystem.

| Property | Type   | Description                  |
|----------|--------|------------------------------|
| `Size`   | `uint` | Gets the file size in bytes. |

| Constructor                                          | Description                         |
|------------------------------------------------------|-------------------------------------|
| `FtpFile(string name, uint size, string parentPath)` | Initializes a new FtpFile instance. |

### `FtpFile(string name, uint size, string parentPath)`
| Parameter    | Type     | Description                                        |
|--------------|----------|----------------------------------------------------|
| `name`       | `string` | The file name (without the parent path).           |
| `size`       | `uint`   | The file size.                                     |
| `parentPath` | `string` | The parent directory path that contains this file. |

## [FtpDirectory](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Client/Ex/Entry/FtpDirectory.cs#L4)

Inherits from [IFtpEntry](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Client/Ex/Entry/IFtpEntry#L22).
Class represents a directory of a filesystem.

| Constructor                                    | Description                                                        |
|------------------------------------------------|--------------------------------------------------------------------|
| `FtpDirectory(string name, string parentPath)` | Initializes a new FtpDirectory instance.                           |
| `FtpDirectory(string name)`                    | Initializes a new FtpDirectory instance with an empty parent path. |

### `FtpDirectory(string name, string parentPath)`
| Parameter    | Type     | Description                                             |
|--------------|----------|---------------------------------------------------------|
| `name`       | `string` | The directory name (without the parent path).           |
| `parentPath` | `string` | The parent directory path that contains this directory. |

### `FtpDirectory(string name)`
| Parameter    | Type     | Description                                             |
|--------------|----------|---------------------------------------------------------|
| `name`       | `string` | The directory name (without the parent path).           |
