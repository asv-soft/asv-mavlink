# Benchmark-serialization

This command benchmarks the serialization and deserialization performance of MAVLink packets. It uses BenchmarkDotNet to measure the efficiency of the serialization process, focusing on how MAVLink packets are serialized and deserialized using spans.### Features:

- Connects to multiple MAVLink streams, allowing you to route messages between different systems (e.g., vehicle and multiple ground stations).
- Supports filtering by system ID, message ID, message name (using regex), and message content (JSON text).
- Can log filtered MAVLink messages to a file.
- Allows disabling console output for silent operation.
- Automatically propagates MAVLink messages between the connected links.

```bash
Asv.Mavlink.Shell.exe benchmark-serialization
```

![image](asv-drones-mavlink-benchmark-serialization-command.png)
