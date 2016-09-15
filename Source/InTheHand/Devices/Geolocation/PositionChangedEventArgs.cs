// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionChangedEventArgs.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.PositionChangedEventArgs))]
#else

namespace Windows.Devices.Geolocation
{
    public sealed class PositionChangedEventArgs
    {
        internal PositionChangedEventArgs(Geoposition position)
        {
            this.Position = position;
        }

        public Geoposition Position
        {
            get;
            private set;
        }
    }
}
#endif