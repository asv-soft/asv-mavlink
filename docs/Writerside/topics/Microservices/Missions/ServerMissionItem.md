# ServerMissionItem

[Source code](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Missions/Server/IMIssionServer.cs)

`ServerMissionItem` is a server-side mission item wrapper around `MissionItemIntPayload`.

| Property       | Type             | Description                                                                                                                    |
|----------------|------------------|--------------------------------------------------------------------------------------------------------------------------------|
| `Param1`       | `float`          | PARAM1, see MAV_CMD enum                                                                                                       |
| `Param2`       | `float`          | PARAM2, see MAV_CMD enum                                                                                                       |
| `Param3`       | `float`          | PARAM3, see MAV_CMD enum                                                                                                       |
| `Param4`       | `float`          | PARAM4, see MAV_CMD enum                                                                                                       |
| `X`            | `int`            | PARAM5 / local: x position in meters * 1e4, global: latitude in degrees * 10^7                                                 |
| `Y`            | `int`            | PARAM6 / y position: local: x position in meters * 1e4, global: longitude in degrees *10^7                                     |
| `Z`            | `float`          | PARAM7 / z position: global: altitude in meters (relative or absolute, depending on frame.                                     |
| `Seq`          | `ushort`         | Waypoint ID (sequence number). Starts at zero. Increases monotonically for each waypoint, no gaps in the sequence (0,1,2,3,4). |
| `Command`      | `MavCmd`         | The scheduled action for the waypoint.                                                                                         |
| `Frame`        | `MavFrame`       | The coordinate system of the waypoint.                                                                                         |
| `Autocontinue` | `byte`           | Autocontinue to next waypoint.                                                                                                 |
| `MissionType`  | `MavMissionType` | Mission type.                                                                                                                  |
