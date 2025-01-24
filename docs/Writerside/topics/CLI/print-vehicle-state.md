# Print Vehicle State

```bash
Asv.Mavlink.Shell.exe print-vehicle-state --connection tcp://127.0.0.1:5762
```
This command starts the console implementation of UAV controls and Telemetry

<note>
<format color="Yellow" style="bold">
    WARNING! Use this command only with a simulator. It does not provide the full functionality required for safe flight. 
    Use this command only for education or introductory purposes.
</format>
</note>



![image](asv-drones-print-vehicle-state.png)

The Print Vehicle State command provides control over the UAV and displays telemetry data, including:
- Link - current state of the link between the UAV and the router;
- Home Position - the starting point for the UAV, which is also the target for the RTL (Return to Launch) command;
- Global Position - current UAV location with altitudes MSL (Mean Sea Level) and AGL (Above Ground Level);
- Current Azimuth - the current azimuth of the UAV;
- Mavlink Version - the version of the protocol operating the process;
- Base Mode - list of base modes supported by the current vehicle;
- AutoPilot - the current type of autopilot in use;
- System Status - the current status of the system;
- Type - representation of the device type according to MAV_TYPE;

The "Log" table displays a list of recent commands executed in the CLI.
