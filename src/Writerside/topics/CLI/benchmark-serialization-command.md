# Benchmark-serialization

This command benchmarks the serialization and deserialization performance of a generated MAVLink packet. It uses BenchmarkDotNet to measure span-based serialization, deserialization, execution time, and managed memory allocations.

The command does not connect to a vehicle or route MAVLink traffic.

```bash
Asv.Mavlink.Shell.exe benchmark-serialization
```

![image](asv-drones-mavlink-benchmark-serialization-command.png){style="block"}
