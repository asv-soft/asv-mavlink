# Handles

## [ReadHandle](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Common/Handles/ReadHandle.cs#L10)

Handle that describes an opened remote read session (session id and remote file size).

| Field     | Type   | Description                |
|-----------|--------|----------------------------|
| `Session` | `byte` | Remote session id.         |
| `Size`    | `uint` | Remote file size in bytes. |

## [WriteHandle](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Common/Handles/WriteHandle.cs#L10)

Handle that describes an opened remote write session (session id and remote file size).

| Field     | Type   | Description                |
|-----------|--------|----------------------------|
| `Session` | `byte` | Remote session id.         |
| `Size`    | `uint` | Remote file size in bytes. |

## [CreateHandle](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Common/Handles/CreateHandle.cs#L10)

Generic handle that binds a remote session id with a path.

| Field     | Type     | Description        |
|-----------|----------|--------------------|
| `Session` | `byte`   | Remote session id. |
| `Path`    | `string` | Remote path.       |
