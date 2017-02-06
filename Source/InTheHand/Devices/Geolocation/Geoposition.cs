// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geoposition.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if __UNIFIED__
using CoreLocation;
#elif WIN32
using System.Device.Location;
#endif

using System;

namespace InTheHand.Devices.Geolocation
{
    /// <summary>
    /// Represents a location that may contain latitude and longitude data or venue data.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public sealed class Geoposition
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Devices.Geolocation.Geoposition _position;

        private Geoposition(Windows.Devices.Geolocation.Geoposition position)
        {
            _position = position;
        }

        public static implicit operator Windows.Devices.Geolocation.Geoposition(Geoposition gp)
        {
            return gp._position;
        }

        public static implicit operator Geoposition(Windows.Devices.Geolocation.Geoposition gp)
        {
            return new Geoposition(gp);
        }
#elif __UNIFIED__
        // constructor from CoreLocation location
        internal Geoposition(CLLocation location)
        {
            Coordinate = new Geocoordinate();
            if (location != null)
            {
                Coordinate.Point = new Geopoint(new BasicGeoposition() { Latitude = location.Coordinate.Latitude, Longitude = location.Coordinate.Longitude, Altitude = location.Altitude });

                Coordinate.Accuracy = location.HorizontalAccuracy;

                if (!double.IsNaN(location.VerticalAccuracy))
                {
                    Coordinate.AltitudeAccuracy = location.VerticalAccuracy;
                }
#if __IOS__ || __MAC__
                if (!double.IsNaN(location.Course) && location.Course != -1)
                {
                    Coordinate.Heading = location.Course;
                }

                if (!double.IsNaN(location.Speed) && location.Speed != -1)
                {
                    Coordinate.Speed = location.Speed;
                }
#endif
                Coordinate.Timestamp = InTheHand.DateTimeOffsetHelper.FromNSDate(location.Timestamp);
            }
        }
#elif TIZEN
        internal Geoposition(Tizen.Location.Location location)
        {
            Coordinate = new Geolocation.Geocoordinate();
            Coordinate.Point = new Geolocation.Geopoint(new Geolocation.BasicGeoposition() { Latitude = location.Latitude, Longitude = location.Longitude, Altitude = location.Altitude });
            Coordinate.Accuracy = location.HorizontalAccuracy;
            Coordinate.Timestamp = new DateTimeOffset(location.Timestamp);
            Coordinate.Heading = location.Direction;
            Coordinate.Speed = location.Speed;
        }
#elif WIN32
        internal Geoposition(GeoPosition<GeoCoordinate> position)
        {
            Coordinate = new Geocoordinate();
            Coordinate.Point = new Geopoint(new BasicGeoposition() { Latitude = position.Location.Latitude, Longitude = position.Location.Longitude, Altitude = position.Location.Altitude });
            Coordinate.Accuracy = position.Location.HorizontalAccuracy;
            Coordinate.Timestamp = position.Timestamp;
            Coordinate.AltitudeAccuracy = position.Location.VerticalAccuracy;
            Coordinate.Heading = position.Location.Course;
            Coordinate.Speed = position.Location.Speed;
            Coordinate.PositionSource = PositionSource.Unknown;
        }
#endif

                /// <summary>
                /// The latitude and longitude associated with a geographic location.
                /// </summary>
        public Geocoordinate Coordinate
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            get
            {
                return _position.Coordinate;
            }
#else
            get;
            internal set;
#endif
        }
    }
}