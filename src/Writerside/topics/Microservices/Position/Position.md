# Position (Offboard Control Interface)

The Position microservice provides access to position-related data and commands for a vehicle. 
It allows a client to observe the vehicle's current location, altitude, attitude, target position, and other relevant telemetry.

The microservice currently has only a [client](PositionClient.md) implementation, which can be used by ground control apps or external controllers to interact with the vehicle's position data.  
There is also a [PositionClientEx](PositionClientEx.md), which provides a higher-level abstraction over the [PositionClient](PositionClient.md).

>You can read more about the Offboard Control Interface in the official [MAVLink docs](https://mavlink.io/en/services/offboard_control.html).