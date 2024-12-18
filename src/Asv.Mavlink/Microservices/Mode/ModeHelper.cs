using System;
using System.Collections.Generic;
using Asv.Mavlink.Common;

namespace Asv.Mavlink;

public static class ModeHelper
{
    public const string MicroserviceName = "MODE";
    
    #region ServerFactory

    public static IMavlinkServerMicroserviceBuilder RegisterMode(this IMavlinkServerMicroserviceBuilder builder,
        ICustomMode idleMode, IEnumerable<ICustomMode> availableModes,
        Func<ICustomMode, IWorkModeHandler> handlerFactory)
    {
        builder
            .Register<IModeServer, IHeartbeatServer, ICommandServerEx<CommandLongPacket>, IStatusTextServer>(
                (identity, _, _, hb, cmd, status) =>
                    new ModeServer(identity, hb, cmd, status, idleMode, availableModes, handlerFactory));
        return builder;
    }

    public static IModeServer GetMode(this IMavlinkServerMicroserviceFactory factory) 
        => factory.Get<IModeServer>();

    #endregion
}