using System;
using Asv.Common;
using Geodesy;

namespace Asv.Mavlink.Vehicle
{
    public static class GeoMath
    {
        private static readonly GeodeticCalculator Calculator = new(Ellipsoid.WGS84);

        public static GeoPoint SetAltitude(this GeoPoint src, double elevaltion)
        {
            return new GeoPoint(src.Latitude,src.Longitude, elevaltion);
        }

        /// <summary>
        /// Calculates the great circle distance in meters between two points.
        /// </summary>
        /// <param name="point1">The location of the first point.</param>
        /// <param name="point2">The location of the second point.</param>
        /// <returns>The great circle distance in meters.</returns>
        /// <remarks>The antemeridian is not considered.</remarks>
        /// <exception cref="ArgumentNullException">point1 or point2 is null.</exception>
        public static double Distance(GeoPoint point1, GeoPoint point2)
        {
            return Distance(point1.Latitude, point1.Longitude, point1.Altitude, point2.Latitude, point2.Longitude, point2.Altitude);
        }

        /// <summary>
        /// Calculates the great circle distance in meters between two points on
        /// the Earth's surface.
        /// </summary>
        /// <param name="latitude1">The latitude of the first point.</param>
        /// <param name="longitude1">The longitude of the first point.</param>
        /// <param name="latitude2">The latitude of the second point.</param>
        /// <param name="longitude2">The longitude of the second point.</param>
        /// <returns>The great circle distance in meters.</returns>
        /// <remarks>The antemeridian is not considered.</remarks>
        public static double Distance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var measurement = Calculator.CalculateGeodeticMeasurement(
                new GlobalPosition(new GlobalCoordinates(new Angle(latitude1), new Angle(longitude1))),
                new GlobalPosition(new GlobalCoordinates(new Angle(latitude2), new Angle(longitude2))));

            return measurement.EllipsoidalDistance;
        }

        /// <summary>
        /// Calculates the great circle distance in meters between two points in
        /// all three dimensions.
        /// </summary>
        /// <param name="latitude1">The latitude of the first point.</param>
        /// <param name="longitude1">The longitude of the first point.</param>
        /// <param name="altitude1">The altitude of the first point.</param>
        /// <param name="latitude2">The latitude of the second point.</param>
        /// <param name="longitude2">The longitude of the second point.</param>
        /// <param name="altitude2">The altitude of the second point.</param>
        /// <returns>The great circle distance in meters.</returns>
        /// <remarks>The antemeridian is not considered.</remarks>
        public static double Distance(double latitude1, double longitude1, double altitude1, double latitude2, double longitude2, double altitude2)
        {
            var measurement = Calculator.CalculateGeodeticMeasurement(
                new GlobalPosition(new GlobalCoordinates(new Angle(latitude1), new Angle(longitude1)), altitude1),
                new GlobalPosition(new GlobalCoordinates(new Angle(latitude2), new Angle(longitude2)), altitude2));

            return measurement.PointToPointDistance;
        }

        /// <summary>Converts the specified value in radians to degrees.</summary>
        /// <param name="radians">The angle in radians.</param>
        /// <returns>The specified angle converted to degrees.</returns>
        public static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        public static double DistanceTo(this GeoPoint a, GeoPoint b)
        {
            return GeoMath.Distance(a, b);
        }

        /// <summary>Converts the specified value in degrees to radians.</summary>
        /// <param name="degrees">The angle in degrees.</param>
        /// <returns>The specified angle converted to radians.</returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        public static double Azimuth(this GeoPoint a, GeoPoint b)
        {
            var measurement = Calculator.CalculateGeodeticMeasurement(
                new GlobalPosition(new GlobalCoordinates(a.Latitude, a.Longitude)),
                new GlobalPosition(new GlobalCoordinates(b.Latitude, b.Longitude)));

            return measurement.Azimuth.Degrees;
        }

        /// <summary>
        /// Calculates the initial azimuth (the angle measured clockwise from
        /// true north) at a point from that point to a second point.
        /// </summary>
        /// <param name="latitude1">The latitude of the first point.</param>
        /// <param name="longitude1">The longitude of the first point.</param>
        /// <param name="latitude2">The latitude of the second point.</param>
        /// <param name="longitude2">The longitude of the second point.</param>
        /// <returns>
        /// The initial azimuth of the first point to the second point.
        /// </returns>
        /// <example>
        /// The azimuth from 0,0 to 1,0 is 0 degrees. From 0,0 to 0,1 is 90
        /// degrees (due east). The range of the result is [-180, 180].
        /// </example>
        public static double Azimuth(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var measurement = Calculator.CalculateGeodeticMeasurement(
                new GlobalPosition(new GlobalCoordinates(new Angle(latitude1), new Angle(longitude1))),
                new GlobalPosition(new GlobalCoordinates(new Angle(latitude2), new Angle(longitude2))));

            return measurement.Azimuth.Degrees;
        }

        public static GeoPoint RadialPoint(this GeoPoint point, double distance, double radialDeg)
        {
            return GeoMath.RadialPoint(point.Latitude, point.Longitude, point.Altitude, distance, radialDeg);
        }

        /// <summary>
        /// Calculates a point at the specified distance along a radial from a
        /// center point.
        /// </summary>
        /// <param name="latitude">The latitude of the center point.</param>
        /// <param name="longitude">The longitude of the center point.</param>
        /// <param name="distance">The distance in meters.</param>
        /// <param name="radialDeg">
        /// The radial in degrees, measures clockwise from north.
        /// </param>
        /// <returns>
        /// A <see cref="GeoPoint"/> containing the Latitude and Longitude of the
        /// calculated point.
        /// </returns>
        /// <remarks>The antemeridian is not considered.</remarks>
        public static GeoPoint RadialPoint(double latitude, double longitude, double altitude, double distance, double radialDeg)
        {
            radialDeg = !double.IsNaN(radialDeg) ? radialDeg : 0;
            var coordinates = Calculator.CalculateEndingGlobalCoordinates(
                new GlobalCoordinates(new Angle(latitude), new Angle(longitude)), new Angle(radialDeg), distance);

            return new GeoPoint(coordinates.Latitude.Degrees, coordinates.Longitude.Degrees, altitude);
        }

        /// <summary>
        /// Высота точки, при заданном угле подъема и удалении(по земле)
        /// </summary>
        /// <param name="groundRange">The distance in meters on the ground.</param>
        /// <param name="elevation">
        /// The angle in degrees from the horizontal plane.
        /// </param>
        /// <returns>The absolute height in meters.</returns>
        public static double HeightFromGroundRange(double groundRange, double elevation)
        {
            return Math.Abs(Math.Tan(DegreesToRadians(elevation)) * groundRange);
        }
    }
}