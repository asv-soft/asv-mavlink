using System;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;

namespace Asv.Mavlink;

public class UnknownMode : ICustomMode
{
    #region MyRegion
    public static UnknownMode Instance = new();
    #endregion
    private UnknownMode()
    {
        
    }
    public string Name => "Unknown"; 
    public string Description => "Unknown mode";
    public bool InternalMode => true;
    public void GetCommandLongArgs(out uint baseMode, out uint customMode, out uint customSubMode)
    {
        throw new NotImplementedException();
    }

    public bool IsCurrentMode(HeartbeatPayload? hb)
    {
        if (hb == null) return true;
        return hb.CustomMode == 0;
    }

    public bool IsCurrentMode(CommandLongPayload payload)
    {
        throw new NotImplementedException();
    }

    public void Fill(HeartbeatPayload hb)
    {
        throw new NotImplementedException();
    }
}