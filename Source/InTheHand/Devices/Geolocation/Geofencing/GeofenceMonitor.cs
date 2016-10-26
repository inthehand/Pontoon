// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeofenceMonitor.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.Geofencing.GeofenceMonitor))]
//#else

using InTheHand.Foundation;
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
    /// <remarks>
    /// <list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
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

#if __IOS__
        internal CLLocationManager _locationManager;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Devices.Geolocation.Geofencing.GeofenceMonitor _monitor;

        public static implicit operator Windows.Devices.Geolocation.Geofencing.GeofenceMonitor(GeofenceMonitor m)
        {
            return m._monitor;
        }
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
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            _monitor = Windows.Devices.Geolocation.Geofencing.GeofenceMonitor.Current;
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

#if __IOS__
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
#if __IOS__
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
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _monitor.LastKnownGeoposition;
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

#if __IOS__
    internal sealed class GeofenceList : Collection<Geofence>
    {

        private GeofenceMonitor _monitor;

        internal GeofenceList(GeofenceMonitor monitor)
        {
            _monitor = monitor;
            foreach(CLRegion r in _monitor._locationManager.MonitoredRegions)
            {
                // add all the currently monitored regions as geofences
                base.InsertItem(base.Count, r);
            }
        }

        protected override void InsertItem(int index, Geofence item)
        {
            _monitor.AddRegion(item);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            try
            {
                _monitor.RemoveRegion(this[index]);
            }
            catch { }
            base.RemoveItem(index);
        }
    }
#endif
}
//#endif