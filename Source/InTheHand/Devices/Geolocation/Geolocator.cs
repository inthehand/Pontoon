// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geolocator.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Devices.Geolocation.Geolocator))]
//#else

using System;
using System.Threading.Tasks;
using System.Diagnostics;
using InTheHand.Foundation;

#if __IOS__
using Foundation;
using CoreLocation;
#elif WIN32
using System.Device.Location;
#endif

namespace InTheHand.Devices.Geolocation
{
    /// <summary>
    /// Provides access to the current geographic location. 
    /// </summary>
    public sealed class Geolocator
    {
#if __IOS__
        CLLocationManager manager = new CLLocationManager();
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
#if __IOS__
            manager.ActivityType = CLActivityType.Other;
            manager.AllowsBackgroundLocationUpdates = true;
            manager.PausesLocationUpdatesAutomatically = false;
            manager.LocationUpdatesPaused += Manager_LocationUpdatesPaused;
            manager.LocationUpdatesResumed += Manager_LocationUpdatesResumed;
            manager.AuthorizationChanged += manager_AuthorizationChanged;
            manager.DeferredUpdatesFinished += Manager_DeferredUpdatesFinished;
            
            if (CLLocationManager.LocationServicesEnabled)
            {
                // ask for the authorization based on what is in the app manifest.
                if (NSBundle.MainBundle.InfoDictionary.ContainsKey(new NSString("NSLocationAlwaysUsageDescription")))
                {
                    manager.InvokeOnMainThread(manager.RequestAlwaysAuthorization);
                }
                else
                {
                    manager.InvokeOnMainThread(manager.RequestWhenInUseAuthorization);
                }
            }
            else
            {
                LocationStatus = PositionStatus.Disabled;
            }
#elif WIN32
#endif
        }

#if __IOS__
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
                manager.DisallowDeferredLocationUpdates();
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
                    manager.AllowDeferredLocationUpdatesUntil(dist, ReportInterval / 1000);
                    isDeferred = true;
                }
            }
        }
#endif

        /// <summary>
        /// The accuracy level at which the Geolocator provides location updates.
        /// </summary>
        public PositionAccuracy DesiredAccuracy
        {
            get
            {
#if __IOS__
                return manager.DesiredAccuracy == CLLocation.AccuracyBest ? PositionAccuracy.High : PositionAccuracy.Default;
#elif WIN32
                return _watcher.DesiredAccuracy == GeoPositionAccuracy.High ? PositionAccuracy.High : PositionAccuracy.Default;
#else
                return PositionAccuracy.Default;
#endif
            }

            set
            {
#if __IOS__
                // TODO: check that Kilometer is a suitable equivalent for cell-tower location
                manager.DesiredAccuracy = value == PositionAccuracy.High ? CLLocation.AccuracyBest : CLLocation.AccuracyKilometer;
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
                return _locationStatus;
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
#if WIN32
                if (_watcher != null)
                {
                    return _watcher.MovementThreshold;
                }
#endif
                return _movementThreshold;
            }

            set
            {

#if WIN32
                if (_watcher != null)
                {
                    _watcher.MovementThreshold = value;
                }
#endif
                if(_movementThreshold != value)
                {
                    _movementThreshold = value;
                }
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
                return _reportInterval;
            }

            set
            {
                if(_reportInterval != value)
                {
                    _reportInterval = value;
                }
            }
        }

        /// <summary>
        /// Starts an asynchronous operation to retrieve the current location of the device.
        /// </summary>
        /// <returns></returns>
        public async Task<Geoposition> GetGeopositionAsync()
        {
#if __IOS__
            manager.RequestLocation();
            CLLocation current = manager.Location;

            Geoposition pos = new Geoposition(current);

            return pos;
#elif WIN32
            if (_watcher == null)
                _watcher = new GeoCoordinateWatcher(this.DesiredAccuracy == PositionAccuracy.High ? GeoPositionAccuracy.High : GeoPositionAccuracy.Default);
            var pos = _watcher.Position;
            return new Geoposition(pos);
#else
            return new Geoposition();
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
#if __IOS__
                    manager.LocationsUpdated += manager_LocationsUpdated;
                    
                    Debug.WriteLine("StartUpdatingLocation");
                    manager.StartUpdatingLocation();
                    manager.StartMonitoringSignificantLocationChanges();
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
#if __IOS__
                    manager.LocationsUpdated -= manager_LocationsUpdated;
                    Debug.WriteLine("StopUpdatingLocation");      
                    manager.StopUpdatingLocation();
                    manager.StopMonitoringSignificantLocationChanges();
#elif WIN32
                    _watcher.PositionChanged -= _watcher_PositionChanged;
                    _watcher.Stop();
#endif
                }
            }
        }


#if __IOS__

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
//#endif