// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeofenceMonitor.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.Geofencing.GeofenceMonitor))]
//#else

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
    /// Contains the information about the monitored Geofence objects. 
    /// </summary>
    public sealed class GeofenceMonitor
    {

        private static GeofenceMonitor _current;

        /// <summary>
        /// Gets the GeofenceMonitor object which contains all of an app's <see cref="Geofence"/> information.
        /// </summary>
        public static GeofenceMonitor Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new GeofenceMonitor();
                }
                return _current;
            }
        }

#if __IOS__
        private CLLocationManager _locationManager;
#endif
        private Queue<GeofenceStateChangeReport> _reports = new Queue<GeofenceStateChangeReport>();

        internal double maxRegion;

        private GeofenceMonitor()
        {
#if __IOS__
            _locationManager = new CLLocationManager();
            _locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
            maxRegion = _locationManager.MaximumRegionMonitoringDistance;
            _locationManager.AllowsBackgroundLocationUpdates = true;
            _locationManager.AuthorizationChanged += _locationManager_AuthorizationChanged;
            _locationManager.LocationsUpdated += _locationManager_LocationsUpdated;
            UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
            {
                _locationManager.RequestAlwaysAuthorization();
            });

            if (!CLLocationManager.IsMonitoringAvailable(typeof(CLCircularRegion)))
            {
                Status = GeofenceMonitorStatus.NotAvailable;
                throw new PlatformNotSupportedException();
            }

            _locationManager.RegionEntered += _locationManager_RegionEntered;
            _locationManager.RegionLeft += _locationManager_RegionLeft;
#endif
        }

#if __IOS__
        private void _locationManager_LocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
        {
            Status = GeofenceMonitorStatus.Ready;
            _locationManager.LocationsUpdated -= _locationManager_LocationsUpdated;
        }

        private void _locationManager_AuthorizationChanged(object sender, CLAuthorizationChangedEventArgs e)
        {
            switch(e.Status)
            {
                case CLAuthorizationStatus.AuthorizedAlways:
                case CLAuthorizationStatus.AuthorizedWhenInUse:
                    Status = GeofenceMonitorStatus.Initializing;
                    break;

                case CLAuthorizationStatus.Denied:
                    Status = GeofenceMonitorStatus.Disabled;
                    break;
            }
        }
#endif

        /// <summary>
        /// Gets a collection of status changes to the <see cref="Geofence"/> objects in the Geofences collection of the GeofenceMonitor.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<GeofenceStateChangeReport> ReadReports()
        {
            List<GeofenceStateChangeReport> reportSnapshot = new List<Geofencing.GeofenceStateChangeReport>();

            lock (_reports)
            {
                reportSnapshot.AddRange(_reports.ToArray());
                _reports.Clear();
            }

            return reportSnapshot;
        }

#if __IOS__
        private void _locationManager_RegionLeft(object sender, CLRegionEventArgs e)
        {
            lock (_reports)
            {
                _reports.Enqueue(new Geofencing.GeofenceStateChangeReport(new Geofencing.Geofence(e.Region), LastKnownGeoposition, GeofenceState.Exited));
            }

            OnGeofenceStateChanged();
        }

        private void _locationManager_RegionEntered(object sender, CLRegionEventArgs e)
        {
            lock (_reports)
            {
                _reports.Enqueue(new Geofencing.GeofenceStateChangeReport(new Geofencing.Geofence(e.Region), LastKnownGeoposition, GeofenceState.Entered));
            }

            OnGeofenceStateChanged();
        }
#endif

        /// <summary>
        /// Returns a vector of the app's <see cref="Geofence"/> objects currently registered with the system wide GeofenceMonitor.
        /// </summary>
        public IList<Geofence> Geofences
        {
            get
            {
#if __IOS__
                GeofenceList fences = new GeofenceList(this);

                foreach (CLRegion region in _locationManager.MonitoredRegions)
                {
                    fences.Add(new Geofence(region));
                }

                return fences;
#else
                return null;
#endif
            }
        }

#if __IOS__
        internal void AddRegion(CLRegion region)
        {
            _locationManager.StartMonitoring(region, CLLocation.AccuracyBest);
            Status = GeofenceMonitorStatus.Ready;
        }

        internal void RemoveRegion(CLRegion region)
        {
            _locationManager.StopMonitoring(region);
        }
#endif

        /// <summary>
        /// Last reading of the device's location.
        /// </summary>
        public Geoposition LastKnownGeoposition
        {
            get
            {
#if __IOS__
                _locationManager.RequestLocation();
                return new Geoposition(_locationManager.Location);
#else
                return new Geoposition();
#endif
            }
        }

        private GeofenceMonitorStatus _status = GeofenceMonitorStatus.NoData;

        /// <summary>
        /// Indicates the current state of the GeofenceMonitor.
        /// </summary>
        public GeofenceMonitorStatus Status
        {
            get
            {
                return _status;
            }
            private set
            {
                if(_status != value)
                {
                    _status = value;
                    StatusChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raised when the state of one or more <see cref="Geofence"/> objects in the Geofences collection of the GeofenceMonitor has changed.
        /// </summary>
        public event EventHandler GeofenceStateChanged;

        private void OnGeofenceStateChanged()
        {
            GeofenceStateChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raised when the status of the GeofenceMonitor has changed.
        /// </summary>
        public event EventHandler StatusChanged;

    }

#if __IOS__
    internal sealed class GeofenceList : Collection<Geofence>
    {

        private GeofenceMonitor _monitor;

        internal GeofenceList(GeofenceMonitor monitor)
        {
            _monitor = monitor;
        }

        protected override void InsertItem(int index, Geofence item)
        {
            _monitor.AddRegion(item._region);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            try
            {
                _monitor.RemoveRegion(this[index]._region);
            }
            catch { }
            base.RemoveItem(index);
        }
    }
#endif

    public sealed class GeofenceStateChangeReport
    {
        private Geofence _geofence;
        private Geoposition _geoposition;
        private GeofenceState _newState;

        internal GeofenceStateChangeReport(Geofence geofence, Geoposition geoposition, GeofenceState newstate)
        {
            _geofence = geofence;
            _geoposition = geoposition;
            _newState = newstate;
        }

        public Geofence Geofence
        {
            get
            {
                return _geofence;
            }
        }

        public Geoposition Geoposition
        {
            get
            {
                return _geoposition;
            }
        }

        public GeofenceState NewState
        {
            get
            {
                return _newState;
            }
        }
    }
}
//#endif