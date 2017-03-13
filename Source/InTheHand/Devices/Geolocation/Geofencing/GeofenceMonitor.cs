// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeofenceMonitor.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using InTheHand.Foundation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#if __UNIFIED__
using Foundation;
using CoreLocation;
#if __IOS__
using UIKit;
#endif
#endif


namespace InTheHand.Devices.Geolocation.Geofencing
{
    /// <summary>
    /// Contains the information about the monitored Geofence objects. 
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list></remarks>
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

#if __UNIFIED__
        internal CLLocationManager _locationManager;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Devices.Geolocation.Geofencing.GeofenceMonitor _monitor;

        public static implicit operator Windows.Devices.Geolocation.Geofencing.GeofenceMonitor(GeofenceMonitor m)
        {
            return m._monitor;
        }
#elif TIZEN
        internal Tizen.Location.Locator _locator;
#endif
        private Queue<GeofenceStateChangeReport> _reports = new Queue<GeofenceStateChangeReport>();

        internal double maxRegion;

        private GeofenceMonitor()
        {
#if __UNIFIED__
            _locationManager = new CLLocationManager();
            _locationManager.DesiredAccuracy = CLLocation.AccuracyBest;            
            _locationManager.AuthorizationChanged += _locationManager_AuthorizationChanged;
            _locationManager.LocationsUpdated += _locationManager_LocationsUpdated;
#if __IOS__
            _locationManager.AllowsBackgroundLocationUpdates = true;
           
            UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
            {
                _locationManager.RequestAlwaysAuthorization();
            });
            if (!CLLocationManager.IsMonitoringAvailable(typeof(CLCircularRegion)))
#else
            if (!CLLocationManager.IsMonitoringAvailable(new ObjCRuntime.Class("CLCircularRegion")))          
#endif
            {
                Status = GeofenceMonitorStatus.NotAvailable;
            }
            else
            {
                maxRegion = _locationManager.MaximumRegionMonitoringDistance;
                _locationManager.RegionEntered += _locationManager_RegionEntered;
                _locationManager.RegionLeft += _locationManager_RegionLeft;
            }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            _monitor = Windows.Devices.Geolocation.Geofencing.GeofenceMonitor.Current;
#elif TIZEN
            _locator = new Tizen.Location.Locator(Tizen.Location.LocationType.Hybrid);
            _locator.ZoneChanged += _locator_ZoneChanged;
            _locator.ServiceStateChanged += _locator_ServiceStateChanged;
#endif
        }

     



#if __UNIFIED__
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
#elif TIZEN
        private void _locator_ZoneChanged(object sender, Tizen.Location.ZoneChangedEventArgs e)
        {
            lock (_reports)
            {
                _reports.Enqueue(new Geofencing.GeofenceStateChangeReport(null, new Geolocation.Geoposition(new Tizen.Location.Location { Latitude = e.Latitude, Longitude = e.Longitude, Altitude = e.Altitude }), e.BoundState == Tizen.Location.BoundaryState.In ? GeofenceState.Entered : GeofenceState.Exited));
            }

            OnGeofenceStateChanged();
        }

        private void _locator_ServiceStateChanged(object sender, Tizen.Location.ServiceStateChangedEventArgs e)
        {
            switch(e.ServiceState)
            {
                case Tizen.Location.ServiceState.Enabled:
                    Status = GeofenceMonitorStatus.Ready;
                    break;

                default:
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
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            foreach(Windows.Devices.Geolocation.Geofencing.GeofenceStateChangeReport r in _monitor.ReadReports())
            {
                reportSnapshot.Add(r);
            }
#else
            lock (_reports)
            {
                reportSnapshot.AddRange(_reports.ToArray());
                _reports.Clear();
            }
#endif
            return reportSnapshot;
        }

#if __UNIFIED__
        private void _locationManager_RegionLeft(object sender, CLRegionEventArgs e)
        {
            lock (_reports)
            {
                _reports.Enqueue(new Geofencing.GeofenceStateChangeReport(e.Region, LastKnownGeoposition, GeofenceState.Exited));
            }

            OnGeofenceStateChanged();
        }

        private void _locationManager_RegionEntered(object sender, CLRegionEventArgs e)
        {
            lock (_reports)
            {
                _reports.Enqueue(new Geofencing.GeofenceStateChangeReport(e.Region, LastKnownGeoposition, GeofenceState.Entered));
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
#if __UNIFIED__ || TIZEN
                return new GeofenceList(this);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                List<Geofence> fences = new List<Geofence>();
                foreach(Windows.Devices.Geolocation.Geofencing.Geofence f in _monitor.Geofences)
                {
                    fences.Add(f);
                }

                return fences;
#else
                return null;
#endif
            }
        }

#if __UNIFIED__
    
        internal void AddRegion(CLRegion region)
        {
#if __IOS__
            _locationManager.StartMonitoring(region, CLLocation.AccuracyBest);
#else
            _locationManager.StartMonitoring(region);
#endif
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
#if __UNIFIED__
#if __IOS__
                _locationManager.RequestLocation();
#endif
                return new Geoposition(_locationManager.Location);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _monitor.LastKnownGeoposition;
#elif TIZEN
                return new Geoposition(_locator.Location);
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
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            get
            {
                return (GeofenceMonitorStatus)((int)_monitor.Status);
            }
#else
            get
            {
                return _status;
            }
            private set
            {
                if(_status != value)
                {
                    _status = value;
                    StatusChanged?.Invoke(this, null);
                }
            }
#endif
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private event TypedEventHandler<GeofenceMonitor, object> _geofenceStateChanged;
#endif
        /// <summary>
        /// Raised when the state of one or more <see cref="Geofence"/> objects in the Geofences collection of the GeofenceMonitor has changed.
        /// </summary>
        public event TypedEventHandler<GeofenceMonitor, object> GeofenceStateChanged
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        {
            add
            {
                if(_geofenceStateChanged == null)
                {
                    _monitor.GeofenceStateChanged += _monitor_GeofenceStateChanged;
                }
                _geofenceStateChanged += value;
            }
            remove
            {
                _geofenceStateChanged -= value;

                if(_geofenceStateChanged == null)
                {
                    _monitor.GeofenceStateChanged -= _monitor_GeofenceStateChanged;
                }
            }
        }

        private void _monitor_GeofenceStateChanged(Windows.Devices.Geolocation.Geofencing.GeofenceMonitor sender, object args)
        {
            _geofenceStateChanged?.Invoke(this, EventArgs.Empty);
        }
#else
        ;

        private void OnGeofenceStateChanged()
        {
            GeofenceStateChanged?.Invoke(this, null);
        }
#endif

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private event TypedEventHandler<GeofenceMonitor, object> _statusChanged;
#endif
        /// <summary>
        /// Raised when the status of the GeofenceMonitor has changed.
        /// </summary>
        public event TypedEventHandler<GeofenceMonitor, object> StatusChanged
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        {
            add
            {
                if (_statusChanged == null)
                {
                    _monitor.StatusChanged += _monitor_StatusChanged;
                }
                _statusChanged += value;
            }
            remove
            {
                _statusChanged -= value;

                if (_geofenceStateChanged == null)
                {
                    _monitor.StatusChanged -= _monitor_StatusChanged;
                }
            }
        }

        private void _monitor_StatusChanged(Windows.Devices.Geolocation.Geofencing.GeofenceMonitor sender, object args)
        {
            _statusChanged?.Invoke(this, EventArgs.Empty);
        }
#else
        ;
#endif

    }

#if __UNIFIED__ || TIZEN
    internal sealed class GeofenceList : Collection<Geofence>
    {

        private GeofenceMonitor _monitor;

        internal GeofenceList(GeofenceMonitor monitor)
        {
            _monitor = monitor;
#if __UNIFIED__
            foreach(CLRegion r in _monitor._locationManager.MonitoredRegions)
            {
                // add all the currently monitored regions as geofences
                base.InsertItem(base.Count, r);
            }
#endif
        }

        protected override void InsertItem(int index, Geofence item)
        {
#if __UNIFIED__
            _monitor.AddRegion(item);
#elif TIZEN
            _monitor._locator.AddBoundary((Geocircle)item.Geoshape);
#endif
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            try
            {
#if __UNIFIED__
                _monitor.RemoveRegion(this[index]);
#elif TIZEN
                _monitor._locator.RemoveBoundary((Geocircle)this[index].Geoshape);
#endif
            }
            catch { }
            base.RemoveItem(index);
        }
    }
#endif
}