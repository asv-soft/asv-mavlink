namespace Asv.Mavlink.Shell;

public static class TelemetryParams
{
    public const int DeltaXy = 10;
    
    public const int DeltaZ = 5;
    
    public const int RedialDegRight = 90;

    public const int RedialDegLeft = -90;

    public const int RedialDegForward = 0;

    public const int RedialDegBackwards = 180;

    public const float TakeOffDelta = 50f;
    
    public const float VelocityDelta = 50f;
    
    public const string VelocityMaxParamName = "WPNAV_SPEED";
}

public static class TelemetryKeys
{
    public const string Link = "Link";
    public const string PacketRateHz = "PacketRateHz";
    public const string Type = "Type";
    public const string SystemStatus = "SystemStatus";
    public const string Autopilot = "Autopilot";
    public const string BaseMode = "BaseMode";
    public const string CustomMode = "CustomMode";
    public const string MavlinkVersion = "MavlinkVersion";
    public const string Home = "Home";
    public const string GlobalPosition = "GlobalPosition";
    public const string LastCommand = "LastCommand";
}