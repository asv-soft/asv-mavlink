# asv-mavlink

[![Deploy Nuget for Windows](https://github.com/asv-soft/asv-mavlink/actions/workflows/nuget_windows.yml/badge.svg)](https://github.com/asv-soft/asv-mavlink/actions/workflows/nuget_windows.yml)

Mavlink library for .NET 6.0, .NET 7.0 (support MAVLinkV2 packets)

You can control vehicles, that support [MAVLink](https://mavlink.io/en/) protocol.


# Code gen

Generate C# code for packet serialization\deserialization
```bash
Asv.Mavlink.Shell.exe gen -t=all.xml -i=messages -o=src/Asv.Mavlink/Connection/Dialects -e=cs src/Asv.Mavlink.Shell/Resources/csharp.tpl
```
You can create your own code generator for any language by writing template file (.tpl) with [liquid](https://shopify.github.io/liquid/) syntax

# Example console application

Example packet statistic:
```sh
Asv.Mavlink.Shell.exe mavlink --cs=tcp://127.0.0.1:5760
```
![screenshot-1](img/screenshot-1.png)

## How To Build Asv-MavLink using Asv.Drones

**1. Setup required dependencies for Asv.Drones:**
Make sure next components installed: 
- .NET SDK 7 - https://dotnet.microsoft.com/en-us/download/dotnet/7.0 ; 
-  AvaloniaUI - https://docs.avaloniaui.net/docs/get-started/install ;
- Execute **dotnet install (package name)** command to setup required packages.

**2. Clone project repository:**
[ git clone https://github.com/your-repository-url.git](https://github.com/asv-soft/asv-drones.git)


## Versioning

Project is maintained under [the Semantic Versioning guidelines](http://semver.org/).
