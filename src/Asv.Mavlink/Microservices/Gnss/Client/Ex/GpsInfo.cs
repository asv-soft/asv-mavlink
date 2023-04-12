using System;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public class GpsInfo
{
    private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public GpsInfo(GpsRawIntPayload rawGps)
    {
        if (rawGps.FixType == GpsFixType.GpsFixTypeNoGps) return;
        if (Enum.IsDefined(typeof(GpsFixType), rawGps.FixType) == false) return;


        Vdop = rawGps.Epv == ushort.MaxValue ? (double?)null : rawGps.Epv / 100D;
        Hdop = rawGps.Eph == ushort.MaxValue ? (double?)null : rawGps.Eph / 100D;
        Pdop = Hdop.HasValue && Vdop.HasValue
            ? Math.Sqrt(Hdop.Value * Hdop.Value + Vdop.Value * Vdop.Value)
            : default(double?);
        AltitudeMsl = rawGps.Alt / 1000D;
        AltitudeEllipsoid = rawGps.AltEllipsoid / 1000D;
        CourseOverGround = rawGps.Cog / 100D;

        FixType = rawGps.FixType;
        SatellitesVisible = rawGps.SatellitesVisible;
        var num = (long)(rawGps.TimeUsec * (double)1000 + (rawGps.TimeUsec >= 0.0 ? 0.5 : -0.5));
        if (num is > -315537897600000L and < 315537897600000L && rawGps.TimeUsec < 253402967000)
        {
            Time = Epoch.AddSeconds(rawGps.TimeUsec);
        }

    }

    public GpsInfo(Gps2RawPayload rawGps)
    {
        if (rawGps.FixType == GpsFixType.GpsFixTypeNoGps) return;
        if (Enum.IsDefined(typeof(GpsFixType), rawGps.FixType) == false) return;

        Vdop = rawGps.Epv == ushort.MaxValue ? (double?)null : rawGps.Epv / 100D;
        Hdop = rawGps.Eph == ushort.MaxValue ? (double?)null : rawGps.Eph / 100D;
        Pdop = Hdop.HasValue && Vdop.HasValue
            ? Math.Sqrt(Hdop.Value * Hdop.Value + Vdop.Value * Vdop.Value)
            : default(double?);
        AltitudeMsl = rawGps.Alt / 1000D;

        CourseOverGround = rawGps.Cog / 100D;
        FixType = rawGps.FixType;
        SatellitesVisible = rawGps.SatellitesVisible;

        // check because sometime argument out of range exception
        var num = (long)(rawGps.TimeUsec * (double)1000 + (rawGps.TimeUsec >= 0.0 ? 0.5 : -0.5));
        if (num > -315537897600000L && num < 315537897600000L)
        {
            Time = Epoch.AddSeconds(rawGps.TimeUsec);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public double CourseOverGround { get; set; }

    /// <summary>
    /// Altitude (above WGS84, EGM96 ellipsoid). Positive for up.
    /// </summary>
    public double? AltitudeEllipsoid { get; }

    /// <summary>
    /// Altitude (MSL). Positive for up. Note that virtually all GPS modules provide the MSL altitude in addition to the WGS84 altitude.
    /// </summary>
    public double AltitudeMsl { get; }

    public GpsFixType FixType { get; }
    public int SatellitesVisible { get; }
    public DateTime Time { get; }
    /// <summary>
    /// HDOP � horizontal dilution of precision
    /// </summary>
    public double? Hdop { get; }
    public DopStatusEnum HdopStatus => GpsInfoHelper.GetDopStatus(Hdop);

    /// <summary>
    /// position (3D) dilution of precision
    /// </summary>
    public double? Pdop { get; }
    public DopStatusEnum PdopStatus => GpsInfoHelper.GetDopStatus(Pdop);

    /// <summary>
    ///  vertical dilution of precision
    /// </summary>
    public double? Vdop { get; }
    public DopStatusEnum VdopStatus => GpsInfoHelper.GetDopStatus(Vdop);
}

/// <summary>
/// https://en.wikipedia.org/wiki/Dilution_of_precision_(navigation)
/// </summary>
public enum DopStatusEnum
{
    Unknown,
    Ideal,
    Excellent,
    Good,
    Moderate,
    Fair,
    Poor,
}
