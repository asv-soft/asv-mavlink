# Code-gen example
This command generates example of files for the mavlink code generator.

> It is usually used together with the [packet code generator](packet-code-generation.md) command

```bash
Asv.Mavlink.Shell.exe code-gen-example
```

![image](asv-drones-mavlink-code-gen-example.png)


You may also use some parameters in the command to customise the output
```bash
Usage: code-gen-example [options...] [-h|--help] [--version]

Example command for the code generator

Options:
  -d|--directory <string>    directory where the files should be generated in the root folder (Default: @"in")
  -v|--virtual               use this parameter if you want to use a virtual file system (Optional)

Process finished with exit code 0.
```

Full possible command with all the parameters
```bash
Asv.Mavlink.Shell.exe code-gen-example -v -d custom_folder_name
```