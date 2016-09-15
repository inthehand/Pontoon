// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geopoint.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.Geopoint))]
#else

namespace Windows.Devices.Geolocation
{
    /// <summary>
    /// Describes a geographic point.
    /// </summary>
    public sealed class Geopoint
    {
        public Geopoint(BasicGeoposition position)
        {
            this.Position = position;
        }

        /// <summary>
        /// The position of a geographic point.
        /// </summary>
        public BasicGeoposition Position
        {
            get;
            private set;
        }
    }
}
#endif