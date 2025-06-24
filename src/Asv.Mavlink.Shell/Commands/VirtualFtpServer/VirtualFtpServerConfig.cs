using System;
using System.IO;

namespace Asv.Mavlink.Shell;

public class VirtualFtpServerConfig
{
    public static VirtualFtpServerConfig Default { get; } = new()
    {
        Ports =
        [
            "tcps://127.0.0.1:7343"
        ],
        FtpServerConfig = new MavlinkFtpServerConfig
        {
            NetworkId = 0,
            BurstReadChunkDelayMs = 30
        },
        FtpServerExConfig = new MavlinkFtpServerExConfig
        {
            RootDirectory = Path.Combine($"{AppDomain.CurrentDomain.BaseDirectory}", "temp")
        },
        SystemId = 1,
        ComponentId = 1
    };

    public required string[] Ports { get; set; }
    public required MavlinkFtpServerConfig FtpServerConfig { get; set; }
    public required MavlinkFtpServerExConfig FtpServerExConfig { get; set; }
    public required byte SystemId { get; set; }
    public required byte ComponentId { get; set; }
}