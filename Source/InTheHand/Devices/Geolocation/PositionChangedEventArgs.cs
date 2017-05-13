// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionChangedEventArgs.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Devices.Geolocation
{
    /// <summary>
    /// Provides data for the <see cref="Geolocator.PositionChanged"/> event.
    /// </summary>
    public sealed class PositionChangedEventArgs
    {
        internal PositionChangedEventArgs(Geoposition position)
        {
            Position = position;
        }
        /// <summary>
        /// The location data associated with the <see cref="Geolocator.PositionChanged"/> event.
        /// </summary>
        public Geoposition Position
        {
            get;
            private set;
        }
    }
}