// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeofenceState.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.Geofencing.GeofenceState))]
//#else

namespace InTheHand.Devices.Geolocation.Geofencing
{
    /// <summary>
    /// Indicates the current state of a Geofence. 
    /// </summary>
    public enum GeofenceState
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// The device has entered the geofence area.
        /// </summary>
        Entered = 1,

        /// <summary>
        /// The device has left the geofence area.
        /// </summary>
        Exited = 2,

        // <summary>
        // The geofence was removed.
        // </summary>
        //Removed = 4,
    }
}
//#endif