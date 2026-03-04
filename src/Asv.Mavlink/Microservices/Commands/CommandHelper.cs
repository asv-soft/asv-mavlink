using Asv.Mavlink.Common;

namespace Asv.Mavlink;

/// <summary>
/// Provides helper methods for registering and accessing command-related microservices,
/// including base command server and extended CommandLong/CommandInt handlers.
/// </summary>
public static class CommandHelper
{
    /// <summary>
    /// The microservice type identifier.
    /// </summary>
    public const string MicroserviceTypeName = "COMMAND";
    
    #region ServerFactory

    /// <summary>
    /// Registers <see cref="CommandServer"/> at device builder with default parameters.
    /// </summary>
    public static IServerDeviceBuilder RegisterCommand(this IServerDeviceBuilder builder)
    {
        builder.Register<ICommandServer>((identity, context,_) => new CommandServer(identity, context));
        return builder;
    }
   
    /// <summary>
    /// Registers <see cref="CommandLongServerEx"/> at device builder with default parameters.
    /// </summary>
    public static IServerDeviceBuilder RegisterCommandLongEx(this IServerDeviceBuilder builder)
    {
        builder
            .Register<ICommandServerEx<CommandLongPacket>, ICommandServer>((_, _, _, @base) =>
                new CommandLongServerEx(@base));
        return builder;
    }
    
    /// <summary>
    /// Registers <see cref="CommandIntServerEx"/> at device builder with default parameters.
    /// </summary>
    public static IServerDeviceBuilder RegisterCommandIntEx(this IServerDeviceBuilder builder)
    {
        builder
            .Register<ICommandServerEx<CommandIntPacket>, ICommandServer>((_, _, _, @base) =>
                new CommandIntServerEx(@base));
        return builder;
    }
    
    /// <summary>
    /// Resolves <see cref="ICommandServer"/> instance from the server device factory.
    /// </summary>
    public static ICommandServer GetCommand(this IServerDevice factory) 
        => factory.Get<ICommandServer>();

    /// <summary>
    /// Resolves <see cref="ICommandServerEx{CommandIntPacket}"/> instance from the server device factory.
    /// </summary>
    public static ICommandServerEx<CommandIntPacket> GetCommandIntEx(this IServerDevice factory) 
        => factory.Get<ICommandServerEx<CommandIntPacket>>();
    
    /// <summary>
    /// Resolves <see cref="ICommandServerEx{CommandLongPacket}"/> instance from the server device factory.
    /// </summary>
    public static ICommandServerEx<CommandLongPacket> GetCommandLongEx(this IServerDevice factory) 
        => factory.Get<ICommandServerEx<CommandLongPacket>>();

    #endregion
}