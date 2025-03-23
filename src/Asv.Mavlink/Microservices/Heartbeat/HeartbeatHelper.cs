using System;
using System.IO.Abstractions;
using Asv.Cfg;
using Asv.Common;
using Asv.Mavlink.Minimal;

namespace Asv.Mavlink;

public static class HeartbeatHelper
{
    public const string MicroserviceName = "HEARTBEAT";
    
    #region ServerFactory

    public static IServerDeviceBuilder RegisterHeartbeat(this IServerDeviceBuilder builder)
    {
        builder.Register<IHeartbeatServer>((identity, context,config) => new HeartbeatServer(identity, config.Get<MavlinkHeartbeatServerConfig>(), context));
        return builder;
    }
   
    public static IServerDeviceBuilder RegisterHeartbeat(this IServerDeviceBuilder builder, MavlinkHeartbeatServerConfig config)
    {
        builder
            .Register<IHeartbeatServer>((identity, context,_) =>  new HeartbeatServer(identity,config,context));
        return builder;
    }

    public static IHeartbeatServer GetHeartbeat(this IServerDevice factory) 
        => factory.Get<IHeartbeatServer>();

    #endregion


    public static void EditCustomMode(this HeartbeatPayload payload, Action<UintBitArray> edit)
    {
        var mode = new UintBitArray(payload.CustomMode, 32);
        edit.Invoke(mode);
        payload.CustomMode = mode.Value;
    }
    
    public static void SetCustomMode(this HeartbeatPayload payload, int bitIndex, int bitLength, uint value)
    {
        var mode = new UintBitArray(payload.CustomMode, 32);
        mode.SetBitU(bitIndex, bitLength, value);
        payload.CustomMode = mode.Value;
    }
    
    public static uint GetCustomMode(this HeartbeatPayload payload, int bitIndex, int bitLength)
    {
        var mode = new UintBitArray(payload.CustomMode, 32);
        return mode.GetBitU(bitIndex, bitLength);
    }
   
}