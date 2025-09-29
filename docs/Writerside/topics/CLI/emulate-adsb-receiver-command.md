# Emulate ADSB receiver

This command starts a virtual ADS-B receiver that sends [ADSB\_VEHICLE](https://mavlink.io/en/messages/common.html#ADSB\_VEHICLE) packets at a specified rate for every vehicle defined in the configuration file.

Executing this command launches an emulator for an ADS-B receiver, generating and transmitting ADSB\_VEHICLE data packets for virtual vehicles. These packets include information such as position, speed, and other ADS-B message parameters. The vehicles and their respective parameters are specified in the configuration file.

```bash
// run adsb simulator
Asv.Mavlink.Shell adsb -cfg adsb.json
```

![image](asv-drones-mavlink-adsb-command.png)

## Configuration file

If the configuration file does not exist, the command generates a default configuration file named `adsb.json` with two vehicles that fly in a box pattern over an airport.

### Configuration file: Base properties

```json5
{
    "SystemId": 1,         // Mavlink System ID for ADSB Receiver
    "ComponentId": 240,    // Mavlink Component ID for ADSB Receiver
    "Ports":[],            // Connection ports
    "Vehicles": []         // Vehicles and their routes
}
```

### Configuration file: Connections

You can add multiple ports at once. All packets will be routed by other ports.

```json5
{
  "Ports": [
    {
      "ConnectionString": "tcp://127.0.0.1:5760", // TCP client example
      "Name": "TCP client",
      "IsEnabled": true,
      "PacketLossChance": 0 // this is for packet loss testing. Must be 0.
    },
    {
      "ConnectionString": "tcp://127.0.0.1:7341?srv=true", // TCP server example
      "Name": "TCP server",
      "IsEnabled": true,
      "PacketLossChance": 0
    },
    {
      "ConnectionString": "serial:COM1?br=115200", // Serial on Windows example
      "Name": "Serial on Windows",
      "IsEnabled": true,
      "PacketLossChance": 0
    },
    {
      "ConnectionString": "serial:/dev/ttyS0?br=115200", // Serial on Linux example
      "Name": "Serial on Linux",
      "IsEnabled": true,
      "PacketLossChance": 0
    },
    {
      "ConnectionString": "udp:127.0.0.1:7341?rhost=127.0.0.1&rport=7342", // UDP example
      "Name": "UDP",
      "IsEnabled": true,
      "PacketLossChance": 0
    }
  ]
}
```

#### Configuration file: Vehicles

Base properties are needed to fill [ADSB\_VEHICLE](https://mavlink.io/en/messages/common.html#ADSB\_VEHICLE). You can add multiple route points and different velocities for each point. Velocity will be interpolated between points. Latitude and Longitude can be in DMS or angle format (see [GeoPointLatitudeTest.cs](https://github.com/asv-soft/asv-common/blob/main/src/Asv.Common.Test/GeoPointLatitudeTest.cs) and [GeoPointLongitudeTest.cs](https://github.com/asv-soft/asv-common/blob/main/src/Asv.Common.Test/GeoPointLongitudeTest.cs)). Altitude is in meters. Velocity is in three dimensions in m/s. It will be separated by ground and vertical velocity if altitude between two route points is different. Velocity must be greater than 0.

```json5
{
  "Vehicles": [
    {
      "CallSign": "PLANE1",   // Call sign ADSB_VEHICLE (max 9 char)
      "Squawk": 777,
      "UpdateRateMs": 500,    // Rate for sending ADSB_VEHICLE packets
      "IcaoAddress": 1234,    // 24 bit ICAO address (DEC format)
      "Route": [              // Vehicle mission points list (must be > 2)
        {
          "Lat": "55.305641", // Latitude at first point
          "Lon": "61.500886", // Longitude at first point
          "Alt": 250.0,       // Altitude at first point (m)
          "Velocity": 10.0    // Velocity at first point (m/s). Must be > 0
        },
        {
          "Lat": "55.362666", // Latitude at second point
          "Lon": "61.210466", // Latitude at second point
          "Alt": 1000.0,      // Altitude at second point (m)
          "Velocity": 300.0   // Velocity at second point (m/s). Must be > 0
        }
      ]
    },
    {
      "CallSign": "PLANE2", // Second vehicle example
      ...
}
```

#### Here's an example of ADSB utility being used with [Asv.Drones](https://github.com/asv-soft/asv-drones).

![image](adsb-vehicles-in-asv-drones.png)

#### Here's an example of ADSB utility being used with [Mission Planner](https://ardupilot.org/planner/)

![image](adsb-vehicles-in-mission-planner.png)
