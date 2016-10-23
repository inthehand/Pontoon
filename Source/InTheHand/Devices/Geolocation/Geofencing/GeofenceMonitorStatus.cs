// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeofenceMonitorStatus.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.Geofencing.GeofenceMonitorStatus))]
//#else

namespace InTheHand.Devices.Geolocation.Geofencing
{
    /// <summary>
    /// Indicates the current state of a <see cref="GeofenceMonitor"/>.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list></remarks>
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
//#endif