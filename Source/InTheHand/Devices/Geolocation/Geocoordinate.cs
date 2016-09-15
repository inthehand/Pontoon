// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geocoordinate.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.Geocoordinate))]
#else

using System;

namespace Windows.Devices.Geolocation
{
    /// <summary>
    /// Contains the information for identifying a geographic location.
    /// </summary>
    public sealed class Geocoordinate
    {
        /// <summary>
        /// The accuracy of the location in meters.
        /// </summary>
        /// <value>The accuracy in meters.</value>
        /// <remarks>The Windows Location Provider and the Windows Phone Location Services accuracy depends on the location data available.
        /// For example,iIf Wifi is available, data is accurate to within 50 meters.
        /// If Wifi is not available, the data could be accurate to within 10 miles or larger. 
        /// A GNSS device can provide data accurate to within a few meters.
        /// However, its accuracy can vary if the GNSS sensor is obscured by buildings, trees, or cloud cover.
        /// GNSS data may not be available at all within a building.</remarks>
        public double Accuracy
        {
            get;
            internal set;
        }

        /// <summary>
        /// The accuracy of the altitude, in meters.
        /// </summary>
        /// <value>The accuracy of the altitude.</value>
        /// <remarks>It is optional for a location provider to set this property.
        /// If the property is not provided, the value will be NULL. 
        /// The Windows Location Provider and the Windows Phone Location Services do not set this property.</remarks>
        public double? AltitudeAccuracy
        {
            get;
            internal set;
        }

        /// <summary>
        /// The current heading in degrees relative to true north.
        /// </summary>
        /// <value>The current heading in degrees relative to true north.</value>
        /// <remarks>It is optional for a location provider to set this property.
        /// If the property is not provided, the value will be NULL.
        /// The Windows Location Provider does not set this property.</remarks>
        public double? Heading
        {
            get;
            internal set;
        }

        /// <summary>
        /// The location of the Geocoordinate.
        /// </summary>
        /// <value>The location of the Geocoordinate.</value>
        public Geopoint Point
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the source used to obtain a Geocoordinate.
        /// </summary>
        /// <value>The source used to obtain a Geocoordinate.</value>
        public PositionSource PositionSource
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the time at which the associated Geocoordinate position was calculated.
        /// </summary>
        /// <value>The time at which the associated Geocoordinate position was calculated.</value>
        /// <remarks>When this property is not available, the value will be null.
        /// <para>The timestamp returned by this property depends on how the location was obtained and may be completely unrelated to the system time on the device.
        /// For example, if the position is obtained from the Global Navigation Satellite System (GNSS) the timestamp would be obtained from the satellites.
        /// If the position was is obtained from Secure User Plane Location (SUPL), the timestamp would be obtained from SUPL servers.
        /// This means that the timestamps obtained from these services will be precise and, most importantly, consistent across all devices regardless of whether the system time on the devices is set correctly.</para></remarks>
        public DateTimeOffset? PositionSourceTimestamp
        {
            get;
            internal set;
        }

        /// <summary>
        /// The speed in meters per second.
        /// </summary>
        /// <value>The speed in meters per second.</value>
        /// <remarks>It is optional for a location provider to set this property.
        /// If the property is not provided, the value will be NULL.
        /// The Windows Location Provider does not set this property.</remarks>
        public double? Speed
        {
            get;
            internal set;
        }

        /// <summary>
        /// The system time at which the location was determined.
        /// </summary>
        /// <value>The system time at which the location was determined.</value>
        public DateTimeOffset Timestamp
        {
            get;
            internal set;
        }
    }
}
#endif