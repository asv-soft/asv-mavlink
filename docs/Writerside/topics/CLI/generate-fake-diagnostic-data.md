# Generate fake diagnostic data
This command generates fake diagnostic with customizable frequency.

```bash
Asv.Mavlink.Shell.exe generate-diagnostics
```

![image](asv-drones-mavlink-generate-diag-command.png)

The program generates a default configuration file by default, but you can provide a custom configuration.
Simply pass the path to your configuration file as a command-line parameter.

*Note: config is a json file.*

```bash
Asv.Mavlink.Shell.exe generate-diagnostics -cfg "path/to/your/cfg.json"
```

All the possible parameters for the command:
```bash
Usage: generate-diagnostics [options...] [-h|--help] [--version]

Command creates fake diagnostics data from file and opens a mavlink connection.

Options:
  -cfg|--cfg-path <string?>    location of the config file for the generator (Default: null)
  -r|--refresh-rate <uint>     (in ms) States how fast should the console be refreshed (Default: 2000)
```

Full command with all the parameters
```bash
Asv.Mavlink.Shell.exe generate-diagnostics -cfg "path/to/your/cfg.json" -r 3000
```

## Test diagnostic data
This command creates Diagnostic client and prints all diagnostics that the client retrieves.

```bash
Asv.Mavlink.Shell.exe test-diagnostics -cs tcp://127.0.0.1:7342?srv=true -tsid 1 -tcid 241 -r 3000
```

![image](asv-drones-mavlink-test-diag-command.png)

All the possible parameters for the command:
```bash
Command creates diagnostic client that retrieves diagnostic data.

Options:
  -cs|--connection-string <string>      The address of the connection to the mavlink diagnostic server (Required)
  -tsid|--target-system-id <byte>       Server's system id (Required)
  -tcid|--target-component-id <byte>    Server's component id (Required)
  -r|--refresh-rate <uint>              (in ms) States how fast should the console be refreshed (Default: 1000)
```