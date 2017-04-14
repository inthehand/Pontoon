// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geolocator.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Diagnostics;
using InTheHand.Foundation;

#if __UNIFIED__
using Foundation;
using CoreLocation;
#elif TIZEN
using Tizen.Location;
#elif WIN32
using System.Device.Location;
#endif

namespace InTheHand.Devices.Geolocation
{
    /// <summary>
    /// Provides access to the current geographic location. 
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed partial class Geolocator
    {
#if __UNIFIED__
        CLLocationManager manager = new CLLocationManager();
#elif TIZEN
        Locator _locator = new Locator(LocationType.Hybrid);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Devices.Geolocation.Geolocator _locator = new Windows.Devices.Geolocation.Geolocator();
#elif WIN32
        GeoCoordinateWatcher _watcher;
#endif
        private bool isUpdating = false;

        /// <summary>
        /// Initializes a new Geolocator object.
        /// </summary>
        public Geolocator()
        {
            LocationStatus = PositionStatus.NotInitialized;
#if __UNIFIED__
#if __IOS__ || __MAC__
#if __IOS__
            manager.ActivityType = CLActivityType.Other;
            manager.AllowsBackgroundLocationUpdates = true;
            manager.PausesLocationUpdatesAutomatically = false;
            manager.AuthorizationChanged += manager_AuthorizationChanged;
#endif
            manager.LocationUpdatesPaused += Manager_LocationUpdatesPaused;
            manager.LocationUpdatesResumed += Manager_LocationUpdatesResumed;
            manager.DeferredUpdatesFinished += Manager_DeferredUpdatesFinished;
#endif
            if (CLLocationManager.LocationServicesEnabled)
            {
#if __IOS__
                // ask for the authorization based on what is in the app manifest.
                if (NSBundle.MainBundle.InfoDictionary.ContainsKey(new NSString("NSLocationAlwaysUsageDescription")))
                {
                    manager.InvokeOnMainThread(manager.RequestAlwaysAuthorization);
                }
                else
                {
                    manager.InvokeOnMainThread(manager.RequestWhenInUseAuthorization);
                }
#elif __TVOS__
                manager.InvokeOnMainThread(manager.RequestWhenInUseAuthorization);
#endif
            }
            else
            {
                LocationStatus = PositionStatus.Disabled;
            }
#elif TIZEN
            _locator.ServiceStateChanged += _locator_ServiceStateChanged;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE

            _locator.StatusChanged += _locator_StatusChanged;
#elif WIN32
#endif
        }



#if __IOS__ || __MAC__
        private bool isDeferred = false;
        private void Manager_DeferredUpdatesFinished(object sender, NSErrorEventArgs e)
        {
            isDeferred = false;
        }

        private void Manager_LocationUpdatesResumed(object sender, EventArgs e)
        {
            Debug.WriteLine("LocationUpdatesResumed");
        }

        private void Manager_LocationUpdatesPaused(object sender, EventArgs e)
        {
            Debug.WriteLine("LocationUpdatesPaused");
        }

        
        private void SetDeferrment()
        {
            if(ReportInterval == 1000 && MovementThreshold > 0)
            {
#if __IOS__
                manager.DisallowDeferredLocationUpdates();
#endif
                if (MovementThreshold > 0)
                {
                    manager.DistanceFilter = MovementThreshold;
                }
                else
                {
                    manager.DistanceFilter = CLLocationDistance.FilterNone;
                }
            }
            else
            {
                if (!isDeferred)
                {
                    manager.DistanceFilter = CLLocationDistance.FilterNone;
                    double dist = MovementThreshold == 0.0 ? CLLocationDistance.MaxDistance : MovementThreshold;
#if __IOS__
                    manager.AllowDeferredLocationUpdatesUntil(dist, ReportInterval / 1000);
#endif
                    isDeferred = true;
                }
            }
        }

#elif TIZEN
        private void _locator_ServiceStateChanged(object sender, ServiceStateChangedEventArgs e)
        {
            LocationStatus = e.ServiceState == ServiceState.Enabled ? PositionStatus.Ready : PositionStatus.NotInitialized;
        }
#endif

        /// <summary>
        /// The accuracy level at which the Geolocator provides location updates.
        /// </summary>
        public PositionAccuracy DesiredAccuracy
        {
            get
            {
#if __UNIFIED__
                return manager.DesiredAccuracy == CLLocation.AccuracyBest ? PositionAccuracy.High : PositionAccuracy.Default;
#elif TIZEN
                return _locator.LocationType == LocationType.Gps ? PositionAccuracy.High : PositionAccuracy.Default;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return (PositionAccuracy)((int)_locator.DesiredAccuracy);
#elif WIN32
                return _watcher.DesiredAccuracy == GeoPositionAccuracy.High ? PositionAccuracy.High : PositionAccuracy.Default;
#else
                return PositionAccuracy.Default;
#endif
            }

            set
            {
#if __UNIFIED__
                // TODO: check that Kilometer is a suitable equivalent for cell-tower location
                manager.DesiredAccuracy = value == PositionAccuracy.High ? CLLocation.AccuracyBest : CLLocation.AccuracyKilometer;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _locator.DesiredAccuracy = (Windows.Devices.Geolocation.PositionAccuracy)((int)value);
#elif WIN32
                _watcher = new GeoCoordinateWatcher(value == PositionAccuracy.High ? GeoPositionAccuracy.High : GeoPositionAccuracy.Default);
                _watcher.MovementThreshold = _movementThreshold;
                _watcher.StatusChanged += _watcher_StatusChanged;
#endif
            }
        }

#if WIN32
        private void _watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch(e.Status)
            {
                case GeoPositionStatus.Initializing:
                    LocationStatus = PositionStatus.Initializing;
                    break;

                case GeoPositionStatus.NoData:
                    LocationStatus = PositionStatus.NoData;
                    break;

                case GeoPositionStatus.Ready:
                    LocationStatus = PositionStatus.Ready;
                    break;

                case GeoPositionStatus.Disabled:
                    LocationStatus = PositionStatus.Disabled;
                    break;
            }
        }
#endif

        private PositionStatus _locationStatus;
        /// <summary>
        /// The status that indicates the ability of the Geolocator to provide location updates.
        /// </summary>
        public PositionStatus LocationStatus
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return (PositionStatus)((int)_locator.LocationStatus);
#else
                return _locationStatus;
#endif
            }

            internal set
            {
                if(_locationStatus != value)
                {
                    _locationStatus = value;
                    if(StatusChanged !=null)
                    {
                        StatusChanged(this, new StatusChangedEventArgs(value));
                    }
                }
            }
        }

        private double _movementThreshold = 0.0;

        /// <summary>
        /// Gets and sets the distance of movement, in meters, relative to the coordinate from the last PositionChanged event, that is required for the Geolocator to raise a PositionChanged event.
        /// </summary>
        public double MovementThreshold
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _locator.MovementThreshold;
#elif __UNIFIED__
                return manager.DistanceFilter;
#elif TIZEN
                return _locator.Distance;
#else
#if WIN32
                if (_watcher != null)
                {
                    return _watcher.MovementThreshold;
                }
#endif
                return _movementThreshold;
#endif
            }

            set
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _locator.MovementThreshold = value;
#elif __UNIFIED__
                manager.DistanceFilter = value;
#elif TIZEN
                if(0 > value || value > 120)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _locator.Distance = value;
#else
#if WIN32
                if (_watcher != null)
                {
                    _watcher.MovementThreshold = value;
                }
#endif
                if (_movementThreshold != value)
                {
                    _movementThreshold = value;
                }
#endif
            }
        }

        private uint _reportInterval = 1000;
        /// <summary>
        /// The requested minimum time interval between location updates, in milliseconds.
        /// If your application requires updates infrequently, set this value so that you only receive location updates when needed.
        /// </summary>
        public uint ReportInterval
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _locator.ReportInterval;
#elif TIZEN
                return (uint)_locator.Interval * 1000;
#else
                return _reportInterval;
#endif
            }

            set
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                _locator.ReportInterval = value;
#elif TIZEN
                _locator.Interval = Convert.ToInt32(value / 1000);
#else
                if(_reportInterval != value)
                {
                    _reportInterval = value;
                }
#endif
            }
        }

        /// <summary>
        /// Starts an asynchronous operation to retrieve the current location of the device.
        /// </summary>
        /// <returns></returns>
        public async Task<Geoposition> GetGeopositionAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            var g = await _locator.GetGeopositionAsync();
            return g == null ? null : g;
#elif __UNIFIED__
#if __IOS__ || __TVOS__
            manager.RequestLocation();
#endif
            CLLocation current = manager.Location;

            Geoposition pos = new Geoposition(current);

            return pos;
#elif TIZEN

            var loc = await _locator.GetLocationAsync(-1);
            return new Geoposition(loc);
#elif WIN32
            if (_watcher == null)
                _watcher = new GeoCoordinateWatcher(DesiredAccuracy == PositionAccuracy.High ? GeoPositionAccuracy.High : GeoPositionAccuracy.Default);
            var pos = _watcher.Position;
            return new Geoposition(pos);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Raised when the ability of the Geolocator to provide updated location changes.
        /// </summary>
        public event TypedEventHandler<Geolocator, StatusChangedEventArgs> StatusChanged;


        private event TypedEventHandler<Geolocator, PositionChangedEventArgs> _positionChanged;
        /// <summary>
        /// Raised when the location is updated.
        /// </summary>
        public event TypedEventHandler<Geolocator, PositionChangedEventArgs> PositionChanged
        {
            add
            {
                _positionChanged += value;

                if (!isUpdating)
                {
                    isUpdating = true;
#if __IOS__ || __MAC__
                    manager.LocationsUpdated += manager_LocationsUpdated;
                    
                    Debug.WriteLine("StartUpdatingLocation");
                    manager.StartUpdatingLocation();
                    manager.StartMonitoringSignificantLocationChanges();
#elif TIZEN
                    _locator.LocationChanged += _locator_LocationChanged;
                    _locator.Start();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                    _locator.PositionChanged += _locator_PositionChanged;
#elif WIN32
                    if (_watcher == null)
                    {
                        _watcher = new GeoCoordinateWatcher(DesiredAccuracy == PositionAccuracy.High ? GeoPositionAccuracy.High : GeoPositionAccuracy.Default);
                        _watcher.StatusChanged += _watcher_StatusChanged;
                        _watcher.MovementThreshold = _movementThreshold;
                    }

                    _watcher.PositionChanged += _watcher_PositionChanged;
                    _watcher.Start();
#endif
                }
            }

            remove
            {
                _positionChanged -= value;

                if(_positionChanged == null || _positionChanged.GetInvocationList().Length == 0)
                {
                    isUpdating = false;
#if __IOS__ || __MAC__
                    manager.LocationsUpdated -= manager_LocationsUpdated;
                    Debug.WriteLine("StopUpdatingLocation");      
                    manager.StopUpdatingLocation();
                    manager.StopMonitoringSignificantLocationChanges();
#elif TIZEN
                    _locator.LocationChanged -= _locator_LocationChanged;
                    _locator.Stop();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                    _locator.PositionChanged -= _locator_PositionChanged;
#elif WIN32
                    _watcher.PositionChanged -= _watcher_PositionChanged;
                    _watcher.Stop();
#endif
                }
            }
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        
        private void _locator_StatusChanged(Windows.Devices.Geolocation.Geolocator sender, Windows.Devices.Geolocation.StatusChangedEventArgs args)
        {
            StatusChanged?.Invoke(this, new StatusChangedEventArgs((PositionStatus)((int)args.Status)));
        }

        private void _locator_PositionChanged(Windows.Devices.Geolocation.Geolocator sender, Windows.Devices.Geolocation.PositionChangedEventArgs args)
        {
            _positionChanged?.Invoke(this, new PositionChangedEventArgs(args.Position));
        }
#endif


#if __UNIFIED__

        void manager_AuthorizationChanged(object sender, CLAuthorizationChangedEventArgs e)
        {
            if (e.Status == CLAuthorizationStatus.AuthorizedAlways | e.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
            {
                Debug.WriteLine("User authorized");
                if (LocationStatus == PositionStatus.Disabled | LocationStatus == PositionStatus.NotAvailable)
                {
                    LocationStatus = PositionStatus.Initializing;
                }
            }
        }
#if __IOS__ || __MAC__
        private CLLocation _lastLocation;

        void manager_LocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
        {
            LocationStatus = PositionStatus.Ready;

            foreach(CLLocation l in e.Locations)
            {
                // only update if over the movement threshold or report interval
                if(_lastLocation == null || InTheHand.Devices.Geolocation.BasicGeopositionExtensions.GetDistance(l.Coordinate.Latitude, l.Coordinate.Longitude, _lastLocation.Coordinate.Latitude, _lastLocation.Coordinate.Longitude) >= MovementThreshold || _lastLocation.Timestamp == NSDate.DistantPast || l.Timestamp.SecondsSinceReferenceDate >= _lastLocation.Timestamp.AddSeconds((double)ReportInterval / 1000.0).SecondsSinceReferenceDate)
                {
                    _lastLocation = l;
            
                    Geoposition p = new Geoposition(l);
                    if (_positionChanged != null)
                    {
                        _positionChanged(this, new PositionChangedEventArgs(p));
                    }
                }
            }

            SetDeferrment();
        }
#endif

#elif TIZEN
        private void _locator_LocationChanged(object sender, LocationChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

#elif WIN32
        private void _watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Geoposition p = new Geoposition(e.Position);
            if (_positionChanged != null)
            {
                _positionChanged(this, new PositionChangedEventArgs(p));
            }
        }

#endif

    }
}