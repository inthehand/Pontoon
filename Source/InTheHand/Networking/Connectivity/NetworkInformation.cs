//-----------------------------------------------------------------------
// <copyright file="NetworkInformation.cs" company="In The Hand Ltd">
//     Copyright © 2014-15 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if __ANDROID__
using Android.Net;
#elif __IOS__
using SystemConfiguration;
#endif
namespace InTheHand.Networking.Connectivity
{
    public static class NetworkInformation
    {
#if __ANDROID__
        private static ConnectivityManager _manager;

        private static void _manager_DefaultNetworkActive(object sender, EventArgs e)
        {
            OnNetworkStatusChanged();
        }
#elif __IOS__
        private static NetworkReachability _reachability;
        private static void ReachabilityNotification(NetworkReachabilityFlags flags)
        {
            OnNetworkStatusChanged();
        }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private static void NetworkInformation_NetworkStatusChanged(object sender)
        {
            OnNetworkStatusChanged();
        }
#endif

        static NetworkInformation()
        {
#if __ANDROID__
            _manager = ConnectivityManager.FromContext(InTheHand.Platform.Android.ContextManager.Context);;
#elif __IOS__
            _reachability = new NetworkReachability("0.0.0.0");
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE

#endif
        }



        public static ConnectionProfile GetInternetConnectionProfile()
        {
#if __ANDROID__
            return new ConnectionProfile(_manager.ActiveNetworkInfo);
#elif __IOS__
            NetworkReachabilityFlags flags;
            if(_reachability.TryGetFlags(out flags))
            {
                // only return if internet is reachable
                if (flags.HasFlag(NetworkReachabilityFlags.Reachable))
                {
                    return new ConnectionProfile(flags);
                }
            }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            Windows.Networking.Connectivity.ConnectionProfile p = Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile();
            if(p != null)
            {
                return new ConnectionProfile(p);
            }
#endif

            return null;
        }

        private static event NetworkStatusChangedEventHandler networkStatusChanged;
        public static event NetworkStatusChangedEventHandler NetworkStatusChanged
        {
            add
            {
                if (networkStatusChanged == null)
                {
#if __ANDROID__
                    _manager.DefaultNetworkActive += _manager_DefaultNetworkActive;
#elif __IOS__
                    _reachability.SetNotification(new NetworkReachability.Notification(ReachabilityNotification));
                    _reachability.Schedule(CoreFoundation.CFRunLoop.Current, CoreFoundation.CFRunLoop.ModeDefault);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
           Windows.Networking.Connectivity.NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
#endif
                }
                networkStatusChanged += value;
            }
            remove
            {
                networkStatusChanged -= value;
                if (networkStatusChanged == null)
                {
#if __ANDROID__
                    _manager.DefaultNetworkActive -= _manager_DefaultNetworkActive;
#elif __IOS__
                    _reachability.Unschedule(CoreFoundation.CFRunLoop.Current, CoreFoundation.CFRunLoop.ModeDefault);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                    Windows.Networking.Connectivity.NetworkInformation.NetworkStatusChanged -= NetworkInformation_NetworkStatusChanged;
#endif
                }
            }
        }

        private static void OnNetworkStatusChanged()
        {
            if(networkStatusChanged != null)
            {
                networkStatusChanged(null);
            }
        }
    }
}
