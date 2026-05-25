# Packet Viewer

Command below starts the console implementation of packet viewer.
```bash
Asv.Mavlink.Shell.exe packetviewer --connection tcp://127.0.0.1:5762
```

![image](asv-drones-mavlink-packets.png){ thumbnail="true" }

Packet Viewer sets up the Mavlink router and waits for a connection using parameters provided in the command line.
Launch a real drone or simulator to connect and start receiving packets from it. Once the connection is established, the packets will be displayed in the "Packets" section below.

It provides the following actions:
- Search for the packet you need;
- Adjust the size of the output;
- Pause the output;
- Safely terminate the execution;

By default, the viewer has no filters enabled and displays all received packets.