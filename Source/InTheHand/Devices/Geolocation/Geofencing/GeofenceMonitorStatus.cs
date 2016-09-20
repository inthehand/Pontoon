// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeofenceMonitorStatus.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.Geofencing.GeofenceMonitorStatus))]
#else

namespace Windows.Devices.Geolocation.Geofencing
{
    /// <summary>
    /// Indicates the current state of a <see cref="GeofenceMonitor"/>.
    /// </summary>
    public enum GeofenceMonitorStatus
    {
        /// <summary>
        /// Access to location is denied.
        /// </summary>
        Disabled,

        /// <summary>
        /// The monitor is in the process of initializing.
        /// </summary>
        Initializing,

        /// <summary>
        /// There is no data on the status of the monitor.
        /// </summary>
        NoData,

        /// <summary>
        /// The geofence monitor is not available.
        /// </summary>
        NotAvailable,

        /// <summary>
        /// The geofence monitor has not been initialized.
        /// </summary>
        NotInitialized,

        /// <summary>
        /// The monitor is ready and active.
        /// </summary>
        Ready,
    }
}
#endif