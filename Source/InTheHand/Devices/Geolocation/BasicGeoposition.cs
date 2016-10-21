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
    public struct BasicGeoposition
    {
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