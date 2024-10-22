using System;
using System.Reactive.Concurrency;
using Asv.Mavlink.V2.Common;
using Microsoft.Extensions.Logging;

namespace Asv.Mavlink;

public class CommandLongServerEx(ICommandServer server) : CommandServerEx<CommandLongPacket>(server,
    server.OnCommandLong, p => (ushort)p.Payload.Command, p => p.Payload.Confirmation);