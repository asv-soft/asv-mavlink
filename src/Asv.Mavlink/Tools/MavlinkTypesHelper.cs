using System;
using System.Text;
using Asv.Common;

namespace Asv.Mavlink;

public static class MavlinkTypesHelper
{
    private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, 0);
    
    public static DateTime FromUnixTimeUs(ulong unixTime)
    {
        return UnixEpoch.AddMilliseconds(unixTime);
    }
    
    public static ulong ToUnixTimeUs(DateTime unixTime)
    {
        return (ulong)(unixTime - UnixEpoch).TotalMicroseconds;
    }
    
    public static DateTime FromUnixTimeSec(uint unixTime)
    {
        return UnixEpoch.AddSeconds(unixTime);
    }
    public static DateTime FromUnixTimeSec(ulong unixTime)
    {
        return UnixEpoch.AddSeconds(unixTime);
    }
    
    public static uint ToUnixTimeSec(DateTime unixTime)
    {
        return (uint)(unixTime - UnixEpoch).TotalSeconds;
    }
    
    public static Guid GetGuid(byte[] data)
    {
        return new Guid(data);
    }
    
    public static void SetGuid(byte[] data, Guid value)
    {
        value.TryWriteBytes(data);
    }
    
    public static string GetString(char[] data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        var sb = new StringBuilder(data.Length);
        foreach (var _ in data)
        {
            if (_ == '\0') break;
            sb.Append(_);
        }
        return sb.ToString();
    }
    public static void SetString(byte[] data,string value)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        if (value.IsNullOrWhiteSpace())
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = (byte)'\0';
            }
            return;
        }
        Encoding.ASCII.GetBytes(value,0,value.Length, data, 0);
    }
    
    public static void SetString(Span<byte> data,string value)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        if (value.IsNullOrWhiteSpace())
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = (byte)'\0';
            }
            return;
        }
        Encoding.ASCII.GetBytes(value,data);
    }
    public static void SetString(char[] data,string value)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        if (value.IsNullOrWhiteSpace())
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = '\0';
            }
            return;
        }
        value.CopyTo(0, data, 0, value.Length);
    }

    public static byte[] GetBytes(string value)
    {
        return Encoding.ASCII.GetBytes(value);
    }
    
    public static byte[] GetBytes(char[] value)
    {
        return Encoding.ASCII.GetBytes(value);
    }
    
    public static string GetString(byte[] data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        var sb = new StringBuilder(data.Length);
        foreach (var _ in data)
        {
            if (_ == '\0') break;
            sb.Append((char)_);
        }
        return sb.ToString();
    }
    
    public static string GetString(ReadOnlySpan<byte> data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        var sb = new StringBuilder(data.Length);
        foreach (var _ in data)
        {
            if (_ == '\0') break;
            sb.Append((char)_);
        }
        return sb.ToString();
    }

    public static GeoPoint FromInt32ToGeoPoint(int lat, int lon,float alt)
    {
        return new GeoPoint(lat / 1E7, lon / 1E7, alt / 1E3);
    }
    
    public static double LatLonFromInt32E7ToDegDouble(int value)
    {
        return value / 1E7D;
    }
    
    public static int LatLonDegDoubleToFromInt32E7To(double value)
    {
        return (int)Math.Round(value * 1E7D,0);
    }

    public static double AltFromMmToDoubleMeter(int altitudeInMm)
    {
        return altitudeInMm / 1E3D;
    }
    public static int AltFromDoubleMeterToInt32Mm(double altitudeInMm)
    {
        return (int)Math.Round(altitudeInMm * 1E3D);
    }
}