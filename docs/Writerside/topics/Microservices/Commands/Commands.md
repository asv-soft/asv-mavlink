# Commands

The Commands microservice provides command request/ack APIs for MAVLink devices.
It is based on the MAVLink Command Protocol: [official documentation](https://mavlink.io/en/services/command.html).

This microservice exposes low-level command APIs and high-level command dispatcher APIs:

- [Client](CommandsClient.md) implementing [ICommandClient](CommandsClient.md#icommandclient)
  - sends `COMMAND_LONG` / `COMMAND_INT` to the device and handles ACK-based request-response flow.
- [Server](CommandsServer.md) implementing [ICommandServer](CommandsServer.md#icommandserver)
  - low-level command server: receives command packets and sends `COMMAND_ACK`.
- [ServerEx](CommandsServerEx.md) implementing [ICommandServerEx&lt;TArgPacket&gt;](CommandsServerEx.md#icommandserverex-targpacket)
  - high-level dispatcher with command handlers registry by `MavCmd`.
