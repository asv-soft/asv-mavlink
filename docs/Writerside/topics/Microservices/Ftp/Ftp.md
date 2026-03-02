# FTP

FTP (File Transfer Protocol) microservice implements the well-known [internet protocol](https://en.wikipedia.org/wiki/File_Transfer_Protocol) in the MAVLink ecosystem.
This microservice enables file transfer over a MAVLink network.
You can read more about the original MAVLink FTP protocol [here](https://mavlink.io/en/services/ftp.html).

This package exposes both low-level and high-level FTP APIs:
- [Client](FtpClient.md) implementing [IFtpClient](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Client/IFtpClient.cs#19)
    - low-level FTP operations (OpenFileRead, ReadFile, CreateFile, etc.);
    - does not store file entries locally;
- [Server](FtpServer.md) implementing [IFtpServer](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Server/IFtpServer.cs#L139)
    - contracts for basic operations and callbacks;
- [ClientEx](FtpClientEx.md) implementing [IFtpClientEx](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Client/Ex/IFtpClientEx.cs#L21)
    - high-level operations (recursive directory deletion, file system refresh);
    - stores/caches FTP entries locally;
- [ServerEx](FtpServerEx.md) implementing [IFtpServerEx](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Ftp/Server/IFtpServerEx.cs#L11)
    - implementations of low-level operations;
    - implementation of sessions;
