using Asv.Mavlink.Common;
using Asv.Mavlink.V2.Common;

namespace Asv.Mavlink;

public static class GpsInfoHelper
{
    public static DopStatusEnum GetDopStatus(double? value)
    {
        if (!value.HasValue) return DopStatusEnum.Unknown;
        if (value <= 1) return DopStatusEnum.Ideal;
        if (value <= 2) return DopStatusEnum.Excellent;
        if (value <= 5) return DopStatusEnum.Good;
        if (value <= 10) return DopStatusEnum.Moderate;
        if (value <= 20) return DopStatusEnum.Fair;
        return DopStatusEnum.Poor;
    }

    public static string GetDescription(this DopStatusEnum src)
    {
        switch (src)
        {
            case DopStatusEnum.Ideal:
                return RS.GpsInfoHelper_GetDescription_Ideal;
            case DopStatusEnum.Excellent:
                return RS.GpsInfoHelper_GetDescription_Excellent;
            case DopStatusEnum.Good:
                return RS.GpsInfoHelper_GetDescription_Good;
            case DopStatusEnum.Moderate:
                return RS.GpsInfoHelper_GetDescription_Moderate;
            case DopStatusEnum.Fair:
                return RS.GpsInfoHelper_GetDescription_Fair;
            case DopStatusEnum.Poor:
                return RS.GpsInfoHelper_GetDescription_Poor;
            case DopStatusEnum.Unknown:
            default:
                return RS.GpsInfoHelper_GetDescription_Unknown;
        }
    }

    public static string GetDisplayName(this DopStatusEnum src)
    {
        switch (src)
        {
            case DopStatusEnum.Ideal:
                return RS.GpsInfoHelper_GetDisplayName_Ideal;
            case DopStatusEnum.Excellent:
                return RS.GpsInfoHelper_GetDisplayName_Excellent;
            case DopStatusEnum.Good:
                return RS.GpsInfoHelper_GetDisplayName_Good;
            case DopStatusEnum.Moderate:
                return RS.GpsInfoHelper_GetDisplayName_Moderate;
            case DopStatusEnum.Fair:
                return RS.GpsInfoHelper_GetDisplayName_Fair;
            case DopStatusEnum.Poor:
                return RS.GpsInfoHelper_GetDisplayName_Poor;
            case DopStatusEnum.Unknown:
            default:
                return RS.GpsInfoHelper_GetDescription_Unknown;
        }
    }

    public static string GetShortDisplayName(this GpsFixType fixType)
    {
        switch (fixType)
        {
            case GpsFixType.GpsFixTypeNoGps:
                return "No GPS";
            case GpsFixType.GpsFixTypeNoFix:
                return "No Fix";
            case GpsFixType.GpsFixType2dFix:
                return "2D Fix";
            case GpsFixType.GpsFixType3dFix:
                return "3D Fix";
            case GpsFixType.GpsFixTypeDgps:
                return "Dgps";
            case GpsFixType.GpsFixTypeRtkFloat:
                return "RTK Float";
            case GpsFixType.GpsFixTypeRtkFixed:
                return "RTK Fix";
            case GpsFixType.GpsFixTypeStatic:
                return "Static";
            case GpsFixType.GpsFixTypePpp:
                return "Ppp";
            default:
                return RS.GpsInfoHelper_GetDescription_Unknown;
        }
    }
}