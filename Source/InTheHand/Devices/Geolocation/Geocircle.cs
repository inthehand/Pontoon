// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geocircle.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.IGeoshape))]
[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.GeoshapeType))]
[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.Geocircle))]
#else

using System;

namespace Windows.Devices.Geolocation
{
    /// <summary>
    /// Interface to define a geographic shape.
    /// </summary>
    public interface IGeoshape
    {
        /// <summary>
        /// The type of geographic shape.
        /// </summary>
        GeoshapeType GeoshapeType { get; }
    }

    /// <summary>
    /// Indicates the shape of a geographic region.
    /// </summary>
    public enum GeoshapeType
    {
        /// <summary>
        /// The geographic region is a circle with a center point and a radius.
        /// </summary>
        Geocircle = 1,
    }

    /// <summary>
    /// Describes a geographic circle with a center point and a radius.
    /// </summary>
    public sealed class Geocircle : IGeoshape
    {
        private BasicGeoposition _position;
        private double _radius;

        /// <summary>
        /// Create a geographic circle object for the given position and radius.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        public Geocircle(BasicGeoposition position, double radius)
        {
            _position = position;
            _radius = radius;
        }

        /// <summary>
        /// The center point of a geographic circle.
        /// </summary>
        public BasicGeoposition Center
        {
            get
            {
                return _position;
            }
        }

        /// <summary>
        /// The type of geographic shape.
        /// </summary>
        public GeoshapeType GeoshapeType
        {
            get
            {
                return GeoshapeType.Geocircle;
            }
        }

        /// <summary>
        /// The radius of a geographic circle in meters.
        /// </summary>
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