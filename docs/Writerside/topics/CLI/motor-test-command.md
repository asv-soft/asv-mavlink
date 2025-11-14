# Motor test

Тестирование моторов MAVLink устройств

```bash
Asv.Mavlink.Shell.exe motor-test -cs tcp://127.0.0.1:5760
```

## Usage

```bash
Usage: motor-test [options...] [-h|--help] [--version]

Тестирование моторов с получением телеметрии и хода испытания

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

- Поведение тестов зависит от конкретной реализации MAVLink команды DO_MOTOR_TEST. Consult your autopilot documentation.