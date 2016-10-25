// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeofenceStateChangeReport.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#if __IOS__
using Foundation;
using CoreLocation;
using UIKit;
#endif


namespace InTheHand.Devices.Geolocation.Geofencing
{
    /// <summary>
    /// Contains the information about the state changes for a <see cref="Geofence"/>.
    /// </summary>
    public sealed class GeofenceStateChangeReport
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Devices.Geolocation.Geofencing.GeofenceStateChangeReport _report;

        private GeofenceStateChangeReport(Windows.Devices.Geolocation.Geofencing.GeofenceStateChangeReport report)
        {
            _report = report;
        }

        public static implicit operator Windows.Devices.Geolocation.Geofencing.GeofenceStateChangeReport(GeofenceStateChangeReport gscr)
        {
            return gscr._report;
        }

        public static implicit operator GeofenceStateChangeReport(Windows.Devices.Geolocation.Geofencing.GeofenceStateChangeReport gscr)
        {
            return new GeofenceStateChangeReport(gscr);
        }
#else
        private Geofence _geofence;
        private Geoposition _geoposition;
        private GeofenceState _newState;

        internal GeofenceStateChangeReport(Geofence geofence, Geoposition geoposition, GeofenceState newstate)
        {
            _geofence = geofence;
            _geoposition = geoposition;
            _newState = newstate;
        }
#endif
        /// <summary>
        /// The Geofence object whose state has changed.
        /// </summary>
        public Geofence Geofence
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _report.Geofence;
#else
                return _geofence;
#endif
            }
        }

        /// <summary>
        /// The position of the Geofence object whose state has changed.
        /// </summary>
        public Geoposition Geoposition
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _report.Geoposition;
#else
                return _geoposition;
#endif
            }
        }

        /// <summary>
        /// The new state of the Geofence object whose state has changed.
        /// </summary>
        public GeofenceState NewState
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return (GeofenceState)((int)_report.NewState);
#else
                return _newState;
#endif
            }
        }
    }
}