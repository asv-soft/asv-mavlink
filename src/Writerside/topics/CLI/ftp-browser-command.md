# FTP browser

This command is a file manager for interacting with a drone's file system via FTP. It allows users to browse directories, view files, and perform various file operations (e.g., download, rename, remove, etc.) in an interactive console environment. The tool is designed for MAVLink-based systems and provides an intuitive way to manage the drone’s files and directories.

```bash
Asv.Mavlink.Shell.exe ftp-browser -cs tcp://127.0.0.1:5760
```

## Features:
- FTP Connection: The command connects to a drone via TCP using a specified connection string, establishing an FTP client for file interactions.
- Tree Navigation: The file system is presented in a hierarchical structure using a tree model. The user can browse through directories interactively.
- File and Directory Operations: The user can:
    - Open directories.
    - Remove, rename, or create directories.
    - Perform file operations such as downloading, removing, truncating, renaming, and calculating CRC32.

![image](asv-drones-mavlink-ftp-browser-command.png)

You may also use some parameters in the command.
```bash
Usage: ftp-browser [options...] [-h|--help] [--version]

File manager for interacting with a drone's file system via FTP

Options:
  -cs|--connection <string>          The address of the connection to the mavlink device, e.g. tcp://127.0.0.1:5760 (Required)
  -t|--timeout-ms <int>              The connection timeout in ms (Default: 1000)
  --command-attempt-count <int>      The command attempts count (Default: 5)
  -tid|--target-network-id <byte>    The target id of the network (Default: 0)
  --burst-timeout-ms <int>           The burst timeout in ms (Default: 1000)

````