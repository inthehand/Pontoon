// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geocircle.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.IGeoshape))]
[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.Geocircle))]
#else

namespace Windows.Devices.Geolocation
{
    public interface IGeoshape
    {

    }

    public sealed class Geocircle : IGeoshape
    {
        private BasicGeoposition _position;
        private double _radius;

        public Geocircle(BasicGeoposition position, double radius)
        {
            _position = position;
            _radius = radius;
        }

        public BasicGeoposition Center
        {
            get
            {
                return _position;
            }
        }

        public double Radius
        {
            get
            {
                return _radius;
            }
        }

    }
}
#endif