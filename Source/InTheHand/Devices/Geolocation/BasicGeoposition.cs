// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasicGeoposition.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.BasicGeoposition))]
//#else

using System;

namespace InTheHand.Devices.Geolocation
{
    /// <summary>
    /// The basic information to describe a geographic position.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list></remarks>
    public struct BasicGeoposition
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        public static implicit operator Windows.Devices.Geolocation.BasicGeoposition(BasicGeoposition bg)
        {
            return new Windows.Devices.Geolocation.BasicGeoposition { Altitude = bg.Altitude, Latitude = bg.Latitude, Longitude = bg.Longitude };
        }
        
        internal BasicGeoposition(Windows.Devices.Geolocation.BasicGeoposition position)
        {
            this.Altitude = position.Altitude;
            this.Latitude = position.Latitude;
            this.Longitude = position.Longitude;
        }
#endif
        /// <summary>
        /// The altitude of the geographic position in meters.
        /// </summary>
        public double Altitude;

        /// <summary>
        /// The latitude of the geographic position.
        /// The valid range of latitude values is from -90.0 to 90.0 degrees.
        /// </summary>
        public double Latitude;

        /// <summary>
        /// The longitude of the geographic position.
        /// This can be any value.
        /// For values less than or equal to-180.0 or values greater than 180.0, the value may be wrapped and stored appropriately before it is used.
        /// For example, a longitude of 183.0 degrees would become -177.0 degrees.
        /// </summary>
        public double Longitude;
    }
}
//#endif