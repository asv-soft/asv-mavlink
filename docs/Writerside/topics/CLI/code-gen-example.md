# Code-gen example
This command generates example of files for the mavlink code generator.

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

For testing, you can use:
1.  [template.tpl](../exampleFiles/generateCommand/template.tpl)
2. standart.xml file (targetFile):
https://github.com/mavlink/mavlink/blob/master/message_definitions/v1.0/standard.xml