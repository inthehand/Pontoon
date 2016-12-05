// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonitoredGeofenceStates.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Geolocation.Geofencing
{
    /// <summary>
    /// Indicates the state or states of the Geofences that are currently being monitored by the system.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>maxOS 10.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list></remarks>
    [Flags]
    public enum MonitoredGeofenceStates : uint
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// The device has entered a geofence area.
        /// </summary>
        Entered = 1,

        /// <summary>
        /// The device has left a geofence area.
        /// </summary>
        Exited = 2,

        /// <summary>
        /// The geofence has been removed.
        /// <para>Not supported on iOS.</para>
        /// </summary>
        Removed = 4,
    }
}