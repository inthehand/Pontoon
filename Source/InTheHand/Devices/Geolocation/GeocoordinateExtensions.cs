// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeocoordinateExtensions.cs" company="In The Hand Ltd">
//   Copyright (c) 2010-2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Geolocation
{
    /// <summary>
    /// Provides extension methods for <see cref="Geocoordinate"/>.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public static class GeocoordinateExtensions
    {
        /// <summary>
        /// Returns the distance between the latitude and longitude coordinates that are specified by this <see cref="Geocoordinate"/> and another specified <see cref="Geocoordinate"/>.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="other">The <see cref="Geocoordinate"/> for the location to calculate the distance to.</param>
        /// <returns>The distance between the two coordinates, in meters.</returns>
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
