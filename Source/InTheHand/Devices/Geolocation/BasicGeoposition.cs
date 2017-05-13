// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasicGeoposition.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list></remarks>
    public struct BasicGeoposition
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        public static implicit operator Windows.Devices.Geolocation.BasicGeoposition(BasicGeoposition bg)
        {
            return new Windows.Devices.Geolocation.BasicGeoposition { Altitude = bg.Altitude, Latitude = bg.Latitude, Longitude = bg.Longitude };
        }

        public static implicit operator BasicGeoposition(Windows.Devices.Geolocation.BasicGeoposition bg)
        {
            return new BasicGeoposition { Altitude = bg.Altitude, Latitude = bg.Latitude, Longitude = bg.Longitude };
        }
#elif __UNIFIED__
        public static implicit operator CoreLocation.CLLocationCoordinate2D(BasicGeoposition bg)
        {
            return new CoreLocation.CLLocationCoordinate2D { Latitude = bg.Latitude, Longitude = bg.Longitude };
        }

        public static implicit operator BasicGeoposition(CoreLocation.CLLocationCoordinate2D c)
        {
            return new BasicGeoposition { Latitude = c.Latitude, Longitude = c.Longitude };
        }
#elif TIZEN
        public static implicit operator Tizen.Location.Coordinate(BasicGeoposition bg)
        {
            return new Tizen.Location.Coordinate { Latitude = bg.Latitude, Longitude = bg.Longitude };
        }

        public static implicit operator BasicGeoposition(Tizen.Location.Coordinate c)
        {
            return new BasicGeoposition { Latitude = c.Latitude, Longitude = c.Longitude };
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


        public override string ToString()
        {
            return Latitude.ToString("f6") + "," + Longitude.ToString("f6");
        }
    }
}