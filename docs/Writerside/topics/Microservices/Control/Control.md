# Control

The Control microservice provides a high-level interface to control vehicles. 
It is not part of the official MAVLink specification. 
Instead, it provides a convenient abstraction for common vehicle control operations such as takeoff, land, RTL, navigation, and mode switching.

Control is useful for:
- Writing control logic in ground stations, scripts, or higher-level services.
- Sending standard flight commands without dealing directly with low-level MAVLink messages.
- Hiding differences between vehicle types (for example, ArduCopter and ArduPlane).

In the current implementation, Control microservice exists only as a [client](ControlClient.md) 
implementing [IControlClient](ControlClient.md#icontrolclient-source).
