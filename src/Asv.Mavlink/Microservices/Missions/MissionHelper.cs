using System;
using Asv.Mavlink.Common;

namespace Asv.Mavlink;

public static class MissionHelper
{
    public const string MicroserviceName = "MISSION";
    public const string MicroserviceExName = $"{MicroserviceName}EX";
    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterMission(this IServerDeviceBuilder builder)
    {
        builder
            .Register<IMissionServer>((identity, context, _) => new MissionServer(identity, context));
        return builder;
    }
    
    public static IServerDeviceBuilder RegisterMissionEx(this IServerDeviceBuilder builder)
    {
        builder
            .Register<IMissionServerEx, IMissionServer, IStatusTextServer, ICommandServerEx<CommandLongPacket>>((_, _, _, mis, status, command) =>
                new MissionServerEx(mis, status,command));
        return builder;
    }

    public static IMissionServer GetMission(this IServerDevice factory) 
        => factory.Get<IMissionServer>();
    
    public static IMissionServerEx GetMissionEx(this IServerDevice factory) 
        => factory.Get<IMissionServerEx>();

    #endregion

    public static void GetStartMissionCommandArgs(CommandLongPayload args, out ushort firstItem, out ushort lastItem)
    {
        if (args.Command != MavCmd.MavCmdMissionStart)
        {
            throw new ArgumentException($"Invalid command. Expected {nameof(MavCmd.MavCmdMissionStart)}, got {args.Command : G}");
        }
        firstItem = (ushort)args.Param1;
        lastItem = (ushort)args.Param2;
    }
    
    public static void SetStartMissionCommandArgs(CommandLongPayload args, ushort firstItem, ushort lastItem)
    {
        args.Command = MavCmd.MavCmdMissionStart;
        args.Param1 = firstItem;
        args.Param2 = lastItem;
    }

    public static void GetChangeMissionCurrentCommandArgs(CommandLongPacket args, out ushort index)
    {   
        index = (ushort)args.Payload.Param1;
    }
    
    public static void SetChangeMissionCurrentCommandArgs(ref CommandLongPacket args, ushort index)
    {
        args.Payload.Param1 = index;
    }
}