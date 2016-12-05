// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geofence.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if __UNIFIED__
using Foundation;
using CoreLocation;
#endif

namespace InTheHand.Devices.Geolocation.Geofencing
{
    /// <summary>
    /// Contains the information to define a geofence, an area of interest, to monitor.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list></remarks>
    public sealed class Geofence
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Devices.Geolocation.Geofencing.Geofence _fence;

        private Geofence(Windows.Devices.Geolocation.Geofencing.Geofence fence)
        {
            _fence = fence;
        }

        public static implicit operator Windows.Devices.Geolocation.Geofencing.Geofence(Geofence f)
        {
            return f._fence;
        }

        public static implicit operator Geofence(Windows.Devices.Geolocation.Geofencing.Geofence f)
        {
            return new Geofence(f);
        }
#elif __UNIFIED__
        private Geocircle _shape;


        private CLRegion _region;

        private Geofence(CLRegion region)
        {
            _region = region;
        }

        public static implicit operator CLRegion(Geofence g)
        {
            return g._region;
        }

        public static implicit operator Geofence(CLRegion r)
        {
            return new Geofence(r);
        }
#endif


        /// <summary>
        /// Initializes a new Geofence object given the id and the shape of the geofence.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="geoshape"></param>
        public Geofence(string id, IGeoshape geoshape)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            _fence = new Windows.Devices.Geolocation.Geofencing.Geofence(id, (Windows.Devices.Geolocation.Geocircle)((Geocircle)geoshape));
#elif __UNIFIED__
            _shape = (Geocircle)geoshape;

            if(_shape.Radius > GeofenceMonitor.Current.maxRegion)
            {
                throw new PlatformNotSupportedException("Geofence Radius is greater than the maximum supported on this platform");
            }

            _region = new CLCircularRegion(new CLLocationCoordinate2D(_shape.Center.Latitude, _shape.Center.Longitude), _shape.Radius, id);
#else
                throw new PlatformNotSupportedException();
#endif
        }


        /// <summary>
        /// The shape of the geofence region.
        /// </summary>
        public IGeoshape Geoshape
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return (Geocircle)(Windows.Devices.Geolocation.Geocircle)_fence.Geoshape;
#elif __UNIFIED__
                if (_shape == null)
                {
                    _shape = new Geocircle(new BasicGeoposition() { Latitude= _region.Center.Latitude, Longitude = _region.Center.Longitude }, _region.Radius);
                }

                return _shape;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// The id of the Geofence.
        /// </summary>
        public string Id
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _fence.Id;
#elif __UNIFIED__
                return _region.Identifier;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        public override bool Equals(object obj)
        {
            if(obj != null && ((Geofence)obj).Id == Id)
            {
                return true;
            }

            return base.Equals(obj);
        }

#if __UNIFIED__
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _region.GetHashCode();
        }
#endif
    }
}