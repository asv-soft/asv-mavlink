# Exceptions

The FTP microservice defines several custom exceptions. 
These exceptions provide microservice-specific details.

## [FtpException](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Exception/FtpException.cs#L5)

Base class for all FTP-related exceptions.

## [FtpNackException](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Exception/FtpNackException.cs#L5)

Base class for all FTP [NAK](https://mavlink.io/en/services/ftp.html#error_codes) exceptions.

| Property      | Type        | Description                                                                           |
|---------------|-------------|---------------------------------------------------------------------------------------|
| `Session`     | `FtpOpcode` | Gets the FTP opcode (action) that caused the NAK response.                            |
| `NackError`   | `NackError` | Gets the NAK error code reported by the FTP server.                                   |
| `FsErrorCode` | `byte?`     | Gets the file-system-specific error code when NackError is FailErrno otherwise, null. |

| Constructor                                               | Description                                                                                                                     |
|-----------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------|
| `FtpNackException(FtpOpcode action, NackError nackError)` | Initializes a new instance of the FtpNackException class with a protocol-level NAK error code.                                  |
| `FtpNackException(FtpOpcode action, byte fsErrorCode)`    | Initializes a new instance of the FtpNackException class for FailErrno and captures the server-provided file-system error code. |

### [FtpNackException(FtpOpcode action, NackError nackError)](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Exception/FtpNackException.cs#L11)

| Parameter   | Type        | Description                                |
|-------------|-------------|--------------------------------------------|
| `action`    | `FtpOpcode` | The FTP opcode (action) that failed.       |
| `nackError` | `NackError` | The NAK error code returned by the server. |

### [FtpNackException(FtpOpcode action, byte fsErrorCode)](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Exception/FtpNackException.cs#L17)

| Parameter     | Type        | Description                                                 |
|---------------|-------------|-------------------------------------------------------------|
| `action`      | `FtpOpcode` | The FTP opcode (action) that failed.                        |
| `fsErrorCode` | `byte`      | The file-system-specific error code returned by the server. |

## [FtpNackEndOfFileException](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Exception/FtpNackEndOfFileException.cs)

This is a special exception is used to indicate end-of-file (EOF).
Thrown when an operation reaches the end of a file.
