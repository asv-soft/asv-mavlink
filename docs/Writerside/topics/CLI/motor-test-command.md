# Motor test

Motor testing for MAVLink devices.

```bash
Asv.Mavlink.Shell.exe motor-test -cs tcp://127.0.0.1:5760
```

## Usage

```bash
Usage: motor-test [options...] [-h|--help] [--version]

Run motor tests with telemetry and test progress reporting.

Options:
  -cs|--connection <string>    The address of the connection to the MAVLink device, e.g., tcp://127.0.0.1:5760 (Required)
  -t|--refresh-time <int>      The telemetry refresh rate in ms (Default: 300)

```

## Examples

- TCP:
```bash
Asv.Mavlink.Shell.exe motor-test -cs tcp://127.0.0.1:5760
```

- UDP:
```bash
Asv.Mavlink.Shell.exe motor-test -cs udp://127.0.0.1:14550
```

## Notes

- Test duration for each motor depends on the specific implementation of the MAVLink MAV_CMD_DO_MOTOR_TEST command.
Consult your autopilot documentation.