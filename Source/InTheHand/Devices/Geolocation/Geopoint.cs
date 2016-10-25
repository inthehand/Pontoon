// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geopoint.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.Geopoint))]
//#else

namespace InTheHand.Devices.Geolocation
{
    /// <summary>
    /// Describes a geographic point.
    /// </summary>
    public sealed class Geopoint
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Devices.Geolocation.Geopoint _point;

        private Geopoint(Windows.Devices.Geolocation.Geopoint point)
        {
            _point = point;
        }

        public static implicit operator Windows.Devices.Geolocation.Geopoint(Geopoint gp)
        {
            return gp._point;
        }

        public static implicit operator Geopoint(Windows.Devices.Geolocation.Geopoint gp)
        {
            return new Geopoint(gp);
        }
#endif
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        public Geopoint(BasicGeoposition position)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            this._point = new Windows.Devices.Geolocation.Geopoint(position);
#else
            this.Position = position;
#endif
        }

        /// <summary>
        /// The position of a geographic point.
        /// </summary>
        public BasicGeoposition Position
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            get
            {
                return _point.Position;
            }
#else

            get;
            private set;
#endif
        }
    }
}
//#endif