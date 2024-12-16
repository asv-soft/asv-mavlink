# Ftp tree

This command provides a tree representation of all available files and directories on the drone's FTP server. It allows users to see the entire file structure in a hierarchical format, making it easy to browse and understand the file layout without navigating through individual folders.

```bash
Asv.Mavlink.Shell.exe ftp-tree -cs tcp://127.0.0.1:5760
```

### Features:
- Display the full directory structure of the drone's file system in a tree format.
- Automatically refreshes and loads the / and @SYS directories.
- Displays directories and files with visual guides for better clarity.

You may also use some parameters in the command.
```bash
Usage: ftp-tree [options...] [-h|--help] [--version]

Tree representation of all available files and directories on the drones FTP server

Options:
-cs|--connection <string>    The address of the connection to the mavlink device (Required)
````

<figure><img src="../.gitbook/assets/asv-drones-mavlink-ftp-treecommand.png" alt=""><figcaption><p>Asv.Mavlink.Shell.exe ftp-tree output</p></figcaption></figure>
