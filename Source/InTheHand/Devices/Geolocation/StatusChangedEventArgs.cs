// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusChangedEventArgs.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.StatusChangedEventArgs))]
#else

namespace Windows.Devices.Geolocation
{
    /// <summary>
    /// Provides information for the StatusChanged event.
    /// </summary>
    public sealed class StatusChangedEventArgs
    {
        internal StatusChangedEventArgs(PositionStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// The updated status of the Geolocator object.
        /// </summary>
        public PositionStatus Status
        {
            get;
            private set;
        }
    }
}
#endif