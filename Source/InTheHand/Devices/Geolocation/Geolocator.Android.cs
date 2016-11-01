// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geolocator.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;

namespace InTheHand.Devices.Geolocation
{
    partial class Geolocator
    {
        private LocationManager _manager;

        private void Initialize()
        {
            _manager = (LocationManager)Application.Context.GetSystemService(Context.LocationService);
        }


        internal sealed class NmeaListener : Java.Lang.Object, Android.Locations.ILocationListener
        {
            private Geolocator _owner;

            public NmeaListener(Geolocator owner)
            {
                _owner = owner;
            }
            public void OnLocationChanged(Location location)
            {
                
            }

            public void OnProviderDisabled(string provider)
            {
                throw new NotImplementedException();
            }

            public void OnProviderEnabled(string provider)
            {
                throw new NotImplementedException();
            }

            public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
            {
                throw new NotImplementedException();
            }
        }
    }
}