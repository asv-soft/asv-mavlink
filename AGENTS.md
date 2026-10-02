# AGENTS.md

This file provides guidance to Codex (Codex.ai/code) when working with code in this repository.

## Overview

**asv-mavlink** is a .NET MAVLink library targeting `net10.0`. It provides protocol parsing/serialization, generated MAVLink packet models, client/server microservices, device abstractions, and tools for working with MAVLink-compatible vehicles and payloads such as PX4, ArduPilot, ADS-B, SDR, GBS, RSGA, radio, FTP, missions, parameters, diagnostics, and related ASV extensions.

The repository also contains **Asv.Mavlink.Shell**, a console application used for simulation, diagnostics, packet viewing, FTP browsing, proxying, benchmarking, and MAVLink C# code generation.

All source lives under `src/`. The solution file is `src/Asv.Mavlink.slnx`; shared build properties live in `src/Directory.Build.props`.

## Build Commands

```powershell
# Restore
dotnet restore src

# Build all projects
dotnet build src -c Release

# Build the solution explicitly
dotnet build src/Asv.Mavlink.slnx -c Release

# Pack NuGet package after build
dotnet pack src -c Release --no-build --no-restore
```

## Testing

```powershell
# Run all tests
dotnet test src

# Run the test project
dotnet test src/Asv.Mavlink.Test

# Run a specific test by name filter
dotnet test src/Asv.Mavlink.Test --filter "DisplayName~MyTestName"
```

Tests use xUnit v3, FluentAssertions, DeepEqual, Moq, AutoFixture, and Asv.XUnit helpers. Prefer deterministic in-memory streams, fake time providers, and test file-system abstractions for protocol and microservice tests.

Tests marked with `[LocalFact]` / `[LocalTheory]` (from **Asv.XUnit**) are skipped in CI. Use standard `[Fact]` / `[Theory]` for tests that must run everywhere.

## Version Management

Package and dependency versions are centralized in `src/Directory.Build.props`:

```xml
<ProductVersion>4.3.0-dev.1</ProductVersion>
<TargetFrameworksValue>net10.0</TargetFrameworksValue>
<AsvCommonVersion>3.6.0-dev.18</AsvCommonVersion>
```

`Asv.Mavlink` inherits `Version`, `PackageVersion`, and `FileVersion` from `ProductVersion`. Do not set package versions per-project unless a release workflow or explicit task requires it.

Release workflows compare Git tags with `ProductVersion`; keep tags and `src/Directory.Build.props` synchronized.

### Dev Feed Dependency Updates

When asked to update `Asv.Common` or shared ASV dependencies for the dev feed:

1. Check the working tree first and keep the change scoped to version metadata unless the task explicitly asks for more.
2. Update `AsvCommonVersion` in `src/Directory.Build.props` to the requested version. This property is used for the shared ASV package family, including `Asv.Cfg`, `Asv.Common`, `Asv.IO`, `Asv.Store`, and `Asv.XUnit`.
3. Increment the numeric `ProductVersion` dev suffix by one, for example `4.3.0-dev.3` -> `4.3.0-dev.4`.
4. Confirm the dev release workflow tag pattern in `.github/workflows/release_debug_version.yml`; the tag must be `v<ProductVersion>`, for example `v4.3.0-dev.4`, because the workflow compares the tag without `v` to `ProductVersion`.
5. Run focused validation when credentials are available:

```powershell
dotnet restore src/Asv.Mavlink.slnx
dotnet build src/Asv.Mavlink.slnx -c Release --no-restore
dotnet test src
```

If local restore fails with `401 Unauthorized` from GitHub Packages for private ASV packages such as `Asv.Store` or `Asv.XUnit`, report that validation is blocked by local NuGet credentials. The GitHub release workflow uses repository secrets for package restore and publish.

6. Commit, tag, and push:

```powershell
git add src/Directory.Build.props
git commit -m "Bump Asv.Common to <version>"
git tag -a v<ProductVersion> -m "v<ProductVersion>"
git push origin main
git push origin v<ProductVersion>
```

## Projects

```
Asv.Mavlink          Main library: protocol, generated messages, devices, microservices, store, tools
Asv.Mavlink.Shell    CLI utility for simulation, diagnostics, proxying, FTP tools, packet viewing, code generation, benchmarks
Asv.Mavlink.Test     Unit and integration-style tests for protocol, devices, and microservices
```

`Asv.Mavlink` depends on `Asv.Common`, `Asv.IO`, `Asv.Cfg`, `Asv.Store`, ZLogger, and System.IO.Abstractions. `Asv.Mavlink.Shell` references `Asv.Mavlink` and adds ConsoleAppFramework, Spectre.Console, DotLiquid, BenchmarkDotNet, and related CLI dependencies.

## Architecture

### Protocol Layer

Protocol code lives under `src/Asv.Mavlink/Protocol`.

- `V1` and `V2` contain MAVLink protocol implementations.
- `Messages` contains generated packet and payload classes.
- `Dialects` contains MAVLink XML dialect definitions used by the generator.
- `Features` contains protocol pipeline features such as wrapping, filtering, and ID mapping.
- `IPayload`, `MavlinkMessage`, and `MavlinkV2Message` are central serialization abstractions.

Packet serialization is span-based and allocation-conscious. Preserve field order, sizes, CRC behavior, sequence handling, signatures, and MAVLink wire compatibility.

### Generated Messages

Most files in `src/Asv.Mavlink/Protocol/Messages` are generated from XML dialects using `Asv.Mavlink.Shell`.

Regeneration helpers:

```powershell
.\regenerate_mavlink.bat
.\regenerate_asv.bat
```

Prefer changing XML dialects, generator models, or Liquid templates instead of hand-editing generated packet files. If a generated file must be edited temporarily, document why and expect it to be overwritten by regeneration.

### Microservices

Microservices live under `src/Asv.Mavlink/Microservices`. They expose typed client/server behavior over MAVLink packets: heartbeat, command, params, params-ext, missions, FTP, logging, mode, GNSS, ADS-B, SDR, GBS, radio, chart, diagnostic, audio, and ASV-specific extensions.

Use R3 observables and reactive properties consistently. Prefer typed packet filters and existing `InternalSend`, `InternalCall`, and `InternalFilter` patterns instead of ad hoc message routing.

### Devices

Device abstractions live under `src/Asv.Mavlink/Devices`.

- Client devices model discovered MAVLink systems and compose microservice clients.
- Server devices expose local MAVLink services.
- Factories identify PX4, ArduPilot, generic, ADS-B, radio, SDR, GBS, RFSA, RSGA, and other supported device types.

Keep device discovery, identity, heartbeat handling, and component IDs explicit. Do not hide target system/component assumptions in helper code.

### CLI

CLI commands live under `src/Asv.Mavlink.Shell`.

The shell is built with ConsoleAppFramework and uses Spectre.Console for interactive output. Keep command options stable, documented, and script-friendly. Existing commands include packet generation, MAVLink monitor, proxy, FTP tree/browser/server, packet viewer, devices info, diagnostics, SDR export, and serialization benchmarks.

### Storage and File Systems

Use the existing `Asv.Store` and System.IO.Abstractions patterns for file-backed behavior. Tests should avoid real file-system coupling unless explicitly validating integration behavior.

## Code Quality

- Nullable reference types are enabled (`<Nullable>enable</Nullable>`).
- `<LangVersion>latest</LangVersion>` is used; modern C# features are acceptable when they keep code clear.
- `Asv.Mavlink` allows unsafe blocks; use them only where protocol performance or binary interop justifies it.
- Generated `.Designer.cs` files and generated protocol messages should not be reformatted manually.
- Keep public APIs stable and explicit; this package is published to NuGet.

## MAVLink Guidelines

- Treat MAVLink wire compatibility as a public contract.
- Preserve little-endian binary layout, array lengths, extension-field behavior, truncation rules, CRC extra values, and MAVLink v2 signing semantics.
- Prefer typed packet classes and payload objects over raw byte manipulation outside parser/serializer internals.
- Use `MavlinkIdentity` and target system/component IDs deliberately in clients, servers, and tests.
- For request/response flows, respect cancellation tokens, timeouts, retries, and existing `InternalCall` conventions.
- Avoid blocking calls in packet-processing paths; use async APIs and observables consistently.

## Naming Conventions

- Interfaces: `IFoo`
- MAVLink packet classes: `FooPacket`
- MAVLink payload classes: `FooPayload`
- Client microservices: `FooClient` / `IFooClient`
- Server microservices: `FooServer` / `IFooServer`
- Extended services: `FooClientEx` / `FooServerEx` where the existing convention already uses `Ex`
- Helpers: `FooHelper`
- Resource files: `RS.resx`, `RS.Designer.cs`, and domain-specific `.resx` files such as `ParamsDesc.resx`

## Comments and Documentation

- Write code comments, XML docs, Markdown, and README content in English unless editing localized resource files.
- Use clear English names for types, members, variables, files, modules, and public APIs.
- Keep MAVLink terminology consistent with upstream MAVLink specs and ASV dialect XML files.
- Add comments only when they explain intent, constraints, protocol quirks, compatibility requirements, assumptions, tradeoffs, or non-obvious behavior.
- Keep comments concise and up to date; remove or update them when the code changes.

## Design Principles

- Follow SOLID principles. Each class/service should have a single, well-defined responsibility.
- Prefer composition over inheritance unless inheritance is clearly justified by existing protocol or device patterns.
- Keep protocol parsing, microservice behavior, device composition, CLI presentation, and storage concerns separated.
- Depend on abstractions at I/O boundaries to preserve testability.
- Keep public APIs explicit, stable, and easy to understand.
- Eliminate duplicated logic through extraction rather than copying.
- Avoid god objects, hidden side effects, unclear ownership, and implicit global protocol state.

## Coding Guidelines

**Think before coding:** State assumptions explicitly. If multiple interpretations exist, present them rather than picking silently. If something is unclear, ask before implementing. If a simpler approach exists, say so; push back when warranted.

**Simplicity first:** Write the minimum code that solves the problem. No speculative features, no abstractions for single-use code, no "flexibility" that was not requested, no error handling for impossible scenarios.

**Surgical changes:** Touch only what the task requires. Do not improve adjacent code, comments, or formatting. Match existing style. If you notice unrelated dead code, mention it; do not delete it. Remove only imports, variables, or functions that your changes made unused.

**Goal-driven execution:** For multi-step tasks, state a brief plan with verifiable steps before starting. Define success criteria and loop until the change is verified, for example by adding or running focused tests for protocol, serialization, and microservice changes.
