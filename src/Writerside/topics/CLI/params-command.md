# Params

This command connects to a MAVLink device and displays its parameters in an interactive table. It downloads the parameter list after device initialization and supports paging, filtering by parameter name, and refreshing values from the device.

```bash
Asv.Mavlink.Shell.exe params --connection tcp://127.0.0.1:7341
```

## Usage

```bash
Usage: params [options...] [-h|--help] [--version]

Vehicle params real time monitoring

Options:
  -c, --connection <string>    Connection string (Default: tcp://127.0.0.1:7341)
```

## Controls

- Type letters, digits, or `_` to filter parameters by name.
- Press **Backspace** to remove the last search character.
- Press **Left Arrow** or **Right Arrow** to move between pages.
- Press **F7** to download the parameter list again.
- Press **F6** to exit.
