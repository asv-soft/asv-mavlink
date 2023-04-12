using System;
using Asv.Common;
using Asv.Mavlink.Server;
using Asv.Mavlink.V2.Common;
using NLog;

namespace Asv.Mavlink.Sdr;

public class SdrServerDevice:DisposableOnceWithCancel, ISdrServerDevice
{
    private readonly ISdrClientDevice _interfaceImplementation;
    public static Logger Logger = LogManager.GetCurrentClassLogger();
    public SdrServerDevice(ISdrClientDevice interfaceImplementation, IMavlinkServer server, int statusRateMs = 1000, bool disposeServer = true)
    {
        _interfaceImplementation = interfaceImplementation ?? throw new ArgumentNullException(nameof(interfaceImplementation));
        Server = server ?? throw new ArgumentNullException(nameof(server));
        if (disposeServer)
        {
            server.DisposeItWith(Disposable);
        }
        Server.Heartbeat.Set(_ =>
        {
            _.Autopilot = MavAutopilot.MavAutopilotInvalid;
            _.Type = (MavType)V2.AsvSdr.MavType.MavTypeAsvSdrPayload;
            _.SystemStatus = MavState.MavStateActive;
            _.BaseMode = MavModeFlag.MavModeFlagCustomModeEnabled;
            _.MavlinkVersion = 3;
            _.CustomMode = (uint)V2.AsvSdr.AsvSdrCustomMode.AsvSdrCustomModeIdle;
        });
        Server.Heartbeat.Start();
        
        _interfaceImplementation.CustomMode.Subscribe(mode => Server.Heartbeat.Set(_ =>
        {
            _.CustomMode = (uint)mode;
        })).DisposeItWith(Disposable);
        
        Server.Gbs.Start();
        
        
    }
    
}