// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusChangedEventArgs.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Devices.Geolocation
{
    /// <summary>
    /// Provides information for the <see cref="Geolocator.StatusChanged"/> event.
    /// </summary>
    public sealed class StatusChangedEventArgs
    {
        internal StatusChangedEventArgs(PositionStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// The updated status of the <see cref="Geolocator"/> object.
        /// </summary>
        public PositionStatus Status
        {
            get;
            private set;
        }
    }
}