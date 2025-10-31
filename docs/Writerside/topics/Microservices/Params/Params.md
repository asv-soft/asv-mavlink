# Params

The Params microservice provides access to configuration parameters on MAVLink devices. 
It is an implementation of the [MAVLink Parameter Protocol](https://mavlink.io/en/services/parameter.html).

Parameters are key-value pairs where the key is a parameter name (up to 16 characters) and the value can be one of several numeric types.

The Params microservice can be used in two roles:

- [Client](ParamsClient.md) implementing [IParamsClient](ParamsClient.md#iparamsclient-source)  
  — reads and writes parameters on a remote device (e.g., ground control station).

- [Server](ParamsServer.md) implementing [IParamsServer](ParamsServer.md#iparamsserver-source)  
  — stores parameters and responds to parameter requests (e.g., drone or autopilot).

There are also extended versions that provide higher-level abstractions:

- [ParamsClientEx](ParamsClientEx.md) implementing [IParamsClientEx](ParamsClientEx.md#iparamsclientex-source)  
  — provides caching, synchronization, and simplified parameter access.

- [ParamsServerEx](ParamsServerEx.md) implementing [IParamsServerEx](ParamsServerEx.md#iparamsserverex-source)  
  — provides metadata support, type-safe parameter handling, and reactive updates.

## Parameter

### Type

MAVLink parameters support the following types (defined in [MavParamType](https://mavlink.io/en/messages/common.html#MAV_PARAM_TYPE)):

| Type                 | Description                  |
|----------------------|------------------------------|
| `MavParamTypeUint8`  | 8-bit unsigned integer       |
| `MavParamTypeInt8`   | 8-bit signed integer         |
| `MavParamTypeUint16` | 16-bit unsigned integer      |
| `MavParamTypeInt16`  | 16-bit signed integer        |
| `MavParamTypeUint32` | 32-bit unsigned integer      |
| `MavParamTypeInt32`  | 32-bit signed integer        |
| `MavParamTypeUint64` | 64-bit unsigned integer      |
| `MavParamTypeInt64`  | 64-bit signed integer        |
| `MavParamTypeReal32` | 32-bit floating point number |
| `MavParamTypeReal64` | 64-bit floating point number |

### Value

Parameters are represented using the [MavParamValue](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Params/Tools/MavParamValue.cs) struct, 
which is a type-safe wrapper around parameter values. It implements **comparison** operations with different numeric data 
types and supports implicit type conversions. It allows automatic **casting** from numeric types to `MavParamValue` and 
from `MavParamValue` back to numeric types, enabling direct assignment, comparison, and arithmetic operations without explicit conversion.