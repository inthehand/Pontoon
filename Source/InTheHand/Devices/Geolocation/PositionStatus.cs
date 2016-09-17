﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionStatus.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.PositionStatus))]
#else

namespace Windows.Devices.Geolocation
{
    /// <summary>
    /// Indicates the ability of the Geolocator object to provide location data. 
    /// </summary>
    public enum PositionStatus
    {
        /// <summary>
        /// Location data is available.
        /// </summary>
        Ready = 0,

        /// <summary>
        /// Location services is initializing.
        /// This is the status if a GPS is the source of location data and the GPS receiver does not yet have the required number of satellites in view to obtain an accurate position.
        /// </summary>
        Initializing = 1,

        /// <summary>
        /// No location data is available from any source. 
        /// </summary>
        NoData = 2,

        /// <summary>
        /// Location settings are turned off.
        /// This status indicates that the user has not granted the application permission to access location.
        /// </summary>
        Disabled = 3,

        /// <summary>
        /// An operation to retrieve location has not yet been initialized.
        /// LocationStatus will have this value if the application has not yet called GetGeopositionAsync or registered an event handler for the PositionChanged event.
        /// LocationStatus may also have this value if your app doesn’t have permission to access location.
        /// </summary>
        NotInitialized = 4,

        /// <summary>
        /// Location services is not available on this version of Windows.
        /// </summary>
        NotAvailable = 5,
    }
}
#endif