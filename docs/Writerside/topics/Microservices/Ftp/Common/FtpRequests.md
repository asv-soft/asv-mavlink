# Requests

### [ReadRequest](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Common/Requests/ReadRequest.cs#L9)

Parameters for a single read operation inside an already opened read session.

| Field     | Type   | Description                                 |
|-----------|--------|---------------------------------------------|
| `Session` | `byte` | Remote session id.                          |
| `Skip`    | `uint` | Byte offset from the beginning of the file. |
| `Take`    | `byte` | Requested number of bytes to read.          |

### [WriteRequest](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Common/Requests/WriteRequest.cs#L9)

Parameters for a single write operation inside an already opened write session.

| Field     | Type   | Description                                        |
|-----------|--------|----------------------------------------------------|
| `Session` | `byte` | Remote session id.                                 |
| `Skip`    | `uint` | Byte offset from the beginning of the file.        |
| `Take`    | `byte` | Number of bytes to write from the provided buffer. |

### [TruncateRequest](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Common/Requests/TruncateRequest.cs#L8)

Generic handle that binds a remote session id with a path.

| Field    | Type     | Description                                 |
|----------|----------|---------------------------------------------|
| `Path`   | `string` | Remote file path.                           |
| `Offset` | `uint`   | New file length / truncate offset in bytes. |
