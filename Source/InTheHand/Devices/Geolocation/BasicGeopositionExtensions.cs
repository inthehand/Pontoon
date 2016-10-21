//-----------------------------------------------------------------------
// <copyright file="BasicGeopositionExtensions.cs" company="In The Hand Ltd">
//     Copyright © 2010-2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Geolocation
{
    /// <summary>
    /// Provides extension methods for <see cref="BasicGeoposition"/>.
    /// </summary>
    public static class BasicGeopositionExtensions
    {
        /// <summary>
        /// Returns the distance between the latitude and longitude coordinates that are specified by this <see cref="BasicGeoposition"/> and another specified <see cref="BasicGeoposition"/>.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="other">The <see cref="BasicGeoposition"/> for the location to calculate the distance to.</param>
        /// <returns>The distance between the two coordinates, in meters.</returns>
        public static double GetDistanceTo(this BasicGeoposition b, BasicGeoposition other)
        {
            return GetDistance(b.Latitude, b.Longitude, other.Latitude, other.Longitude);
        }

        internal static double GetDistance(double thisLatitude, double thisLongitude, double otherLatitude, double otherLongitude)
        {
            double R = 6371000; // earth radius in metres
            double dLat = ToRadians(otherLatitude - thisLatitude);
            double dLon = ToRadians(otherLongitude - thisLongitude);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(thisLatitude)) * Math.Cos(ToRadians(otherLatitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
            return d;
        }

        // converts from degrees to radians
        private static double ToRadians(double degrees)
        {
            return (Math.PI / 180) * degrees;
        }
    }
}
