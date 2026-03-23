# Results

### [ReadResult](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Common/Results/ReadResult.cs#L8)

Result of a read operation into a caller-provided buffer.

| Field       | Type          | Description                    |
|-------------|---------------|--------------------------------|
| `Request`   | `ReadRequest` | Original read request.         |
| `ReadCount` | `byte`        | Number of bytes actually read. |

### [BurstReadResult](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Common/Results/BurstReadResult.cs#L8)

Result of a burst-read chunk.

| Field         | Type          | Description                                             |
|---------------|---------------|---------------------------------------------------------|
| `Request`     | `ReadRequest` | Original read request.                                  |
| `ReadCount`   | `byte`        | Number of bytes actually read.                          |
| `IsLastChunk` | `bool`        | Indicates that the stream is complete after this chunk. |
