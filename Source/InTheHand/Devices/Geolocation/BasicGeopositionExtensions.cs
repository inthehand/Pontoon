//-----------------------------------------------------------------------
// <copyright file="BasicGeopositionExtensions.cs" company="In The Hand Ltd">
//     Copyright © 2010-2015 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Windows.Devices.Geolocation;

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
        [CLSCompliant(false)]
        public static double GetDistanceTo(this BasicGeoposition b, BasicGeoposition other)
        {
            double R = 6371000; // earth radius in metres
            double dLat = ToRadians(other.Latitude - b.Latitude);
            double dLon = ToRadians(other.Longitude - b.Longitude);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(b.Latitude)) * Math.Cos(ToRadians(other.Latitude)) *
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
