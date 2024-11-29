using System;
using Asv.Common;

namespace Asv.Mavlink
{
    public static class __REMOVE_GeoConverter
    {
        private static double Pi = 3.14159265358979;             // Число Пи
        private static double ro = 206264.8062;                  // Число угловых секунд в радиане

        // Эллипсоид Красовского
        private static double aP  = 6378136;                     // Большая полуось
        private static double alP = 1 / 298.257839303;           // Сжатие
        private static double e2P = 2 * alP - Math.Pow(alP,2);   // Квадрат эксцентриситета

        // Элипсоид WGS84 (GRS80, эти два эллипсоида сходны по большинству параметров)
        private static double aW  = 6378137;                     // Большая полуось
        private static double alW = 1 / 298.257223563;           // Сжатие
        private static double e2W = 2 * alW - Math.Pow(alW,2);    // Квадрат эксцентриситета

        // Вспомогательные значения для преобразования эллипсоидов
        private static double a   = (aP + aW) / 2;
        private static double e2  = (e2P + e2W) / 2;
        private static double da  = aW - aP;
        private static double de2 = e2W - e2P;

        // Линейные элементы трансформирования, в метрах
        private static double dx_42_84 = 28;
        private static double dy_42_84 = -130;
        private static double dz_42_84 = -95;

        private static double dx_42_90 = 23.92;
        private static double dy_42_90 = -141.27;
        private static double dz_42_90 = -80.9;

        private static double dx_90_84 = -1.08;
        private static double dy_90_84 = -0.27;
        private static double dz_90_84 = -0.9;

        // Угловые элементы трансформирования, в секундах
        private static double wx = 0;
        private static double wy = 0;
        private static double wz = 0;
        // Дифференциальное различие масштабов
        private static double ms = 0;

        public static GeoPoint PZ90_WGS84(this GeoPoint point)
        {
            var lat = PZ90_WGS84_Lat(point.Latitude, point.Longitude, point.Altitude);
            var lon = PZ90_WGS84_Long(point.Latitude, point.Longitude, point.Altitude);
            var alt = WGS84Alt(point.Latitude, point.Longitude, point.Altitude, dx_90_84, dy_90_84, dz_90_84);
            return new GeoPoint(lat,lon,alt);
        }

        public static GeoPoint WGS84_PZ90(this GeoPoint point)
        {
            var lat = WGS84_PZ90_Lat(point.Latitude, point.Longitude, point.Altitude);
            var lon = WGS84_PZ90_Long(point.Latitude, point.Longitude, point.Altitude);
            var alt = WGS84Alt(point.Latitude, point.Longitude, point.Altitude, dx_90_84, dy_90_84, dz_90_84);
            return new GeoPoint(lat, lon, alt);
        }


        private static double dB(double Bd, double Ld, double H, double dx, double dy, double dz)
        {
            double B, L, M, N;
            B = Bd * Pi / 180;
            L = Ld * Pi / 180;
            M = a * (1 - e2) / Math.Pow(1 - e2 * Math.Pow(Math.Sin(B),2), 1.5);
            N = a * Math.Pow(1 - e2 * Math.Pow(Math.Sin(B),2), -0.5);

            return ro / (M + H) * (N / a * e2 * Math.Sin(B) * Math.Cos(B) * da +
                                   (Math.Pow(N,2) / Math.Pow(a,2) + 1) * N * Math.Sin(B) * Math.Cos(B) * de2 / 2
                                   - (dx * Math.Cos(L) + dy * Math.Sin(L)) * Math.Sin(B) + dz * Math.Cos(B))
                   - wx * Math.Sin(L) * (1 + e2 * Math.Cos(2 * B)) + wy * Math.Cos(L) * (1 + e2 * Math.Cos(2 * B))
                   - ro * ms * e2 * Math.Sin(B) * Math.Cos(B);
        }

        private static double dL(double Bd, double Ld, double H, double dx, double dy, double dz)
        {
            double B, L, N;
            B = Bd * Pi / 180;
            L = Ld * Pi / 180;
            N = a * Math.Pow(1 - e2 * Math.Pow(Math.Sin(B),2), -0.5);
            return ro / ((N + H) * Math.Cos(B)) * (-dx * Math.Sin(L) + dy * Math.Cos(L))
                   + Math.Tan(B) * (1 - e2) * (wx * Math.Cos(L) + wy * Math.Sin(L)) - wz;
        }

        private static double WGS84Alt(double Bd, double Ld, double H, double dx, double dy, double dz)
        {
            double B, L, N, dH;
            B = Bd * Pi / 180;
            L = Ld * Pi / 180;
            N = a * Math.Pow(1 - e2 * Math.Pow(Math.Sin(B),2), -0.5);
            dH = -a / N * da + N * Math.Pow(Math.Sin(B),2) * de2 / 2
                             + (dx * Math.Cos(L) + dy * Math.Sin(L)) * Math.Cos(B) + dz * Math.Sin(B)
                 - N * e2 * Math.Sin(B) * Math.Cos(B) * (wx / ro * Math.Sin(L) - wy / ro * Math.Cos(L))
                 + (Math.Pow(a,2) / N + H) * ms;
            return H + dH;
        }

        private static double PZ90_WGS84_Lat(double Bd, double Ld, double H)
        {
            return Bd + dB(Bd, Ld, H, dx_90_84, dy_90_84, dz_90_84) / 3600;
        }

        private static double PZ90_WGS84_Long(double Bd, double Ld, double H)
        {
            return Ld + dL(Bd, Ld, H, dx_90_84, dy_90_84, dz_90_84) / 3600;
        }

        private static double WGS84_PZ90_Lat(double Bd, double Ld, double H)
        {
            return Bd - dB(Bd, Ld, H, dx_90_84, dy_90_84, dz_90_84) / 3600;
        }

        private static double WGS84_PZ90_Long(double Bd, double Ld, double H)
        {
            return Ld - dL(Bd, Ld, H, dx_90_84, dy_90_84, dz_90_84) / 3600;
        }

        //
        private static double CK42_PZ90_Lat(double Bd, double Ld, double H)
        {
            return Bd + dB(Bd, Ld, H, dx_42_90, dy_42_90, dz_42_90) / 3600;
        }

        private static double CK42_PZ90_Long(double Bd, double Ld, double H)
        {
            return Ld + dL(Bd, Ld, H, dx_42_90, dy_42_90, dz_42_90) / 3600;
        }

        private static double PZ90_CK42_Lat(double Bd, double Ld, double H)
        {
            return Bd - dB(Bd, Ld, H, dx_42_90, dy_42_90, dz_42_90) / 3600;
        }

        private static double PZ90_CK42_Long(double Bd, double Ld, double H)
        {
            return Ld - dL(Bd, Ld, H, dx_42_90, dy_42_90, dz_42_90) / 3600;
        }

        //
        private static double CK42_WGS84_Lat(double Bd, double Ld, double H)
        {
            return Bd + dB(Bd, Ld, H, dx_42_84, dy_42_84, dz_42_84) / 3600;
        }

        private static double CK42_WGS84_Long(double Bd, double Ld, double H)
        {
            return Ld + dL(Bd, Ld, H, dx_42_84, dy_42_84, dz_42_84) / 3600;
        }

        private static double WGS84_CK42_Lat(double Bd, double Ld, double H)
        {
            return Bd - dB(Bd, Ld, H, dx_42_84, dy_42_84, dz_42_84) / 3600;
        }

        public static double WGS84_CK42_Long(double Bd, double Ld, double H)
        {
            return Ld - dL(Bd, Ld, H, dx_42_84, dy_42_84, dz_42_84) / 3600;
        }
    }
}
