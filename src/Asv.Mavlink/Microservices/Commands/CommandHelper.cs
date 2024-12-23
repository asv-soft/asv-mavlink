using Asv.Mavlink.Common;

namespace Asv.Mavlink;

public static class CommandHelper
{
    public const string MicroserviceTypeName = "COMMAND";
    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterCommand(this IServerDeviceBuilder builder)
    {
        builder.Register<ICommandServer>((identity, context,_) => new CommandServer(identity, context));
        return builder;
    }
   
    public static IServerDeviceBuilder RegisterCommandLongEx(this IServerDeviceBuilder builder)
    {
        builder
            .Register<ICommandServerEx<CommandLongPacket>, ICommandServer>((_, _, _, @base) =>
                new CommandLongServerEx(@base));
        return builder;
    }
    
    public static IServerDeviceBuilder RegisterCommandIntEx(this IServerDeviceBuilder builder)
    {
        builder
            .Register<ICommandServerEx<CommandIntPacket>, ICommandServer>((_, _, _, @base) =>
                new CommandIntServerEx(@base));
        return builder;
    }
   

    public static ICommandServer GetCommand(this IServerDevice factory) 
        => factory.Get<ICommandServer>();

    public static ICommandServerEx<CommandIntPacket> GetCommandIntEx(this IServerDevice factory) 
        => factory.Get<ICommandServerEx<CommandIntPacket>>();
    
    public static ICommandServerEx<CommandLongPacket> GetCommandLongEx(this IServerDevice factory) 
        => factory.Get<ICommandServerEx<CommandLongPacket>>();

    #endregion
}