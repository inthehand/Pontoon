// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geofence.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if __IOS__
using Foundation;
using UIKit;
using CoreLocation;
#endif

namespace Windows.Devices.Geolocation.Geofencing
{
    /// <summary>
    /// Contains the information to define a geofence, an area of interest, to monitor.
    /// </summary>
    public sealed class Geofence
    {
        private Geocircle _shape;

#if __IOS__
        internal CLRegion _region;

        internal Geofence(CLRegion region)
        {
            _region = region;
        }

        public static implicit operator CLRegion(Geofence g)
        {
            return g._region;
        }
#endif


        /// <summary>
        /// Initializes a new Geofence object given the id and the shape of the geofence.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="geoshape"></param>
        public Geofence(string id, IGeoshape geoshape)
        { 
            _shape = (Geocircle)geoshape;
#if __IOS__
            if(_shape.Radius > GeofenceMonitor.Current.maxRegion)
            {
                throw new PlatformNotSupportedException("Geofence Radius is greater than the maximum supported on this platform");
            }

            _region = new CLCircularRegion(new CLLocationCoordinate2D(_shape.Center.Latitude, _shape.Center.Longitude), _shape.Radius, id);
#endif
        }


        /// <summary>
        /// The shape of the geofence region.
        /// </summary>
        public IGeoshape Geoshape
        {
            get
            {
#if __IOS__
                if(_shape == null)
                {
                    _shape = new Geocircle(new BasicGeoposition() { Latitude= _region.Center.Latitude, Longitude = _region.Center.Longitude }, _region.Radius);
                }
#endif

                return _shape;
            }
        }

        /// <summary>
        /// The id of the Geofence.
        /// </summary>
        public string Id
        {
            get
            {
#if __IOS__
                return _region.Identifier;
#else
                return string.Empty;
#endif
            }
        }

        public override bool Equals(object obj)
        {
            if(obj != null && ((Geofence)obj).Id == this.Id)
            {
                return true;
            }

            return base.Equals(obj);
        }

#if __IOS__
        public override int GetHashCode()
        {
            return _region.GetHashCode();
        }
#endif
    }

    [Flags]
    public enum MonitoredGeofenceStates : uint
    {
        Entered,
        Exited,
        None,
        Removed,
    }
}