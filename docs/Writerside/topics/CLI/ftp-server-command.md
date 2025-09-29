# FTP virtual server

This command creates ftp server and opens connection to it.

```bash
Asv.Mavlink.Shell.exe run-ftp-server
```

![image](asv-drones-mavlink-ftp-server.png)

The program generates a default configuration file by default, but you can provide a custom configuration. Simply pass the path to your configuration file as a command-line parameter.

_Note: config is a json file._

```bash
Asv.Mavlink.Shell.exe run-ftp-server -cfg "path/to/your/cfg.json"
```

All the possible parameters for the command:
```bash
Usage: run-ftp-server [options...] [-h|--help] [--version]

Command creates virtual ftp server.

Options:
  -cfg|--cfg-path <string?>    location of the config file (Default: null)

````