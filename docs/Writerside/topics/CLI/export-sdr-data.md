# Export sdr data

This command extracts SDR (Software Defined Radio) data from a binary file and exports it into a CSV format. The SDR data is deserialized using the AsvSdrRecordDataLlzPayload class, and each record is written as a row in the CSV file with specific data fields such as altitude, signal strength, and power levels.

## Features:

- Reads binary SDR data from an input file.
- Exports the data to a CSV file for further analysis or storage.
- Provides a simple and automated way to convert SDR logs into human-readable tabular data.
```bash
Asv.Mavlink.Shell.exe export-sdr
```

You may also use some parameters in the command.
```bash
Usage: export-sdr [options...] [-h|--help] [--version]

Export sdt data to csv format

Options:
-i|--input-file <string>     Input file (Required)
-o|--output-file <string>    Output file (Default: @"out.csv")
```