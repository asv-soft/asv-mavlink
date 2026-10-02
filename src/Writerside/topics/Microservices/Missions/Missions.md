# Missions

The Missions microservice provides mission upload, download, storage, and execution APIs for MAVLink devices.
It is based on the MAVLink Mission Protocol: [official documentation](https://mavlink.io/en/services/mission.html).

This package exposes both low-level and high-level mission APIs:

- [Client](MissionsClient.md) implementing [IMissionClient](MissionsClient.md#imissionclient)  
  - low-level mission packets;
  - no local mission storage.
- [Server](MissionsServer.md) implementing [IMissionServer](MissionsServer.md#imissionserver)  
  - low-level mission request handlers and packet send helpers.
- [ClientEx](MissionsClientEx.md) implementing [IMissionClientEx](MissionsClientEx.md#imissionclientex)  
  - local mission cache, sync state, upload/download orchestration, and helper methods to build mission items.
- [ServerEx](MissionsServerEx.md) implementing [IMissionServerEx](MissionsServerEx.md#imissionserverex)  
  - mission storage and execution loop with command handlers.
