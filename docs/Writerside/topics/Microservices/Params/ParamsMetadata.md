# Params metadata

You can provide additional metadata for parameters on the server.
Learn more in the [official Mavlink docs](https://mavlink.io/en/services/component_information.html).

## IMavParamTypeMetadata ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Params/Tools/IMavParamTypeMetadata.cs))

Represents metadata for a MAVLink parameter.

| Property         | Type                         | Description                                                                                      |
|------------------|------------------------------|--------------------------------------------------------------------------------------------------|
| `Name`           | `string`                     | Parameter Name.                                                                                  |
| `Type`           | `MavParamType`               | Parameter type.                                                                                  |
| `Group`          | `string?`                    | User readable name for a group of parameters which are commonly modified together.               |
| `Category`       | `string?`                    | User readable name for a 'type' of parameter (e.g., 'Developer', 'System', 'Advanced').          |
| `ShortDesc`      | `string?`                    | Short user facing description/name for parameter. Used in UI instead of internal parameter name. |
| `LongDesc`       | `string?`                    | Long user facing documentation of how the parameters works.                                      |
| `Units`          | `string?`                    | Units for parameter value.                                                                       |
| `DecimalPlaces`  | `int`                        | Number of decimal places to show for user facing display.                                        |
| `RebootRequired` | `bool`                       | true: Vehicle must be rebooted if this value is changed.                                         |
| `Volatile`       | `bool`                       | true: value is volatile. Should not be included in creation of a CRC over param values.          |
| `MinValue`       | `MavParamValue`              | Minimum valid value.                                                                             |
| `MaxValue`       | `MavParamValue`              | Maximum valid value.                                                                             |
| `DefaultValue`   | `MavParamValue`              | Default value for parameter.                                                                     |
| `Increment`      | `MavParamValue`              | Increment to use for user facing UI which increments a value.                                    |
| `Values`         | `(MavParamValue, string)[]?` | Array of values and textual descriptions for use by GCS ui.                                      |
| `Bitmask`        | `(uint, MavParamValue)[]?`   | Bitmask of allowed values.                                                                       |

| Method                                                                             | Return Type     | Description                                                                       |
|------------------------------------------------------------------------------------|-----------------|-----------------------------------------------------------------------------------|
| `IsValid(MavParamValue newValue)`                                                  | `bool`          | Validates the given MavParamValue.                                                |
| `GetValidationError(MavParamValue newValue)`                                       | `string?`       | Validates a MAV parameter value and returns an error message if validation fails. |
| `ReadFromConfig(IConfiguration config, string? prefix = null)`                     | `MavParamValue` | Reads the parameter value from configuration.                                     |
| `WriteToConfig(IConfiguration config, MavParamValue value, string? prefix = null)` | `void`          | Writes the parameter value to configuration.                                      |

### `IMavParamTypeMetadata.IsValid`
| Parameter  | Type            | Description                    |
|------------|-----------------|--------------------------------|
| `newValue` | `MavParamValue` | The MavParamValue to validate. |

### `IMavParamTypeMetadata.GetValidationError`
| Parameter  | Type            | Description                    |
|------------|-----------------|--------------------------------|
| `newValue` | `MavParamValue` | The new value to be validated. |

### `IMavParamTypeMetadata.ReadFromConfig`
| Parameter | Type             | Description                                  |
|-----------|------------------|----------------------------------------------|
| `config`  | `IConfiguration` | The configuration instance to read from.     |
| `prefix`  | `string?`        | (Optional) Prefix for the configuration key. |

### `IMavParamTypeMetadata.WriteToConfig`
| Parameter | Type             | Description                                  |
|-----------|------------------|----------------------------------------------|
| `config`  | `IConfiguration` | The configuration instance to write to.      |
| `value`   | `MavParamValue`  | The value to write.                          |
| `prefix`  | `string?`        | (Optional) Prefix for the configuration key. |

## MavParam ([source](https://github.com/asv-soft/asv-mavlink/blob/main/src/Asv.Mavlink/Microservices/Tools/IMavParamTypeMetadata.cs))

A factory class for creating instances of `IMavParamTypeMetadata` objects.

Examples:
```C#
IMavParamTypeMetadata boolParamMetadata = MavParam.SysU8AsBool("some_param", "Some param");
IMavParamTypeMetadata intParamMetadata = MavParam.AdvS32("timeout", "Timeout", "ms", 1000, 0, 10000);
```

There are many methods like this with different params and types. 
Also, method prefixes (*Dev*S32, *Sys*S32, ...) indicate parameter category:

- **Sys*** - system parameters
- **Adv*** - advanced parameters
- **Dev*** - developer parameters
