using Asv.Mavlink.Common;

namespace Asv.Mavlink;

public static class CommandHelper
{
    public const string MicroserviceTypeName = "COMMAND";
    
    #region ServerFactory

    public static IMavlinkServerMicroserviceBuilder RegisterCommand(this IMavlinkServerMicroserviceBuilder builder)
    {
        builder.Register<ICommandServer>((identity, context,_) => new CommandServer(identity, context));
        return builder;
    }
   
    public static IMavlinkServerMicroserviceBuilder RegisterCommandLongEx(this IMavlinkServerMicroserviceBuilder builder)
    {
        builder
            .Register<ICommandServerEx<CommandLongPacket>, ICommandServer>((_, _, _, @base) =>
                new CommandLongServerEx(@base));
        return builder;
    }
    
    public static IMavlinkServerMicroserviceBuilder RegisterCommandIntEx(this IMavlinkServerMicroserviceBuilder builder)
    {
        builder
            .Register<ICommandServerEx<CommandIntPacket>, ICommandServer>((_, _, _, @base) =>
                new CommandIntServerEx(@base));
        return builder;
    }
   

    public static ICommandServer GetCommand(this IMavlinkServerMicroserviceFactory factory) 
        => factory.Get<ICommandServer>();

    public static ICommandServerEx<CommandIntPacket> GetCommandIntEx(this IMavlinkServerMicroserviceFactory factory) 
        => factory.Get<ICommandServerEx<CommandIntPacket>>();
    
    public static ICommandServerEx<CommandLongPacket> GetCommandLongEx(this IMavlinkServerMicroserviceFactory factory) 
        => factory.Get<ICommandServerEx<CommandLongPacket>>();

    #endregion
}