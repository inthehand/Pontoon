// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeocoordinateExtensions.cs" company="In The Hand Ltd">
//   Copyright (c) 2010-2015 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Windows.Devices.Geolocation;

namespace InTheHand.Devices.Geolocation
{
    /// <summary>
    /// Provides extension methods for <see cref="Geocoordinate"/>.
    /// </summary>
    public static class GeocoordinateExtensions
    {
        /// <summary>
        /// Returns the distance between the latitude and longitude coordinates that are specified by this <see cref="Geocoordinate"/> and another specified <see cref="Geocoordinate"/>.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="other">The <see cref="Geocoordinate"/> for the location to calculate the distance to.</param>
        /// <returns>The distance between the two coordinates, in meters.</returns>
        [CLSCompliant(false)]
        public static double GetDistanceTo(this Geocoordinate g, Geocoordinate other)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }

            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            return g.Point.Position.GetDistanceTo(other.Point.Position);
        }
    }
}
