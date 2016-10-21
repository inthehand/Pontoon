//-----------------------------------------------------------------------
// <copyright file="NetworkInformation.cs" company="In The Hand Ltd">
//     Copyright © 2014-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Networking.Connectivity.NetworkInformation))]
//#else

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
    /// <summary>
    /// Provides access to network connection information for the local machine.
    /// </summary>
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
#endif

        static NetworkInformation()
        {
#if __ANDROID__
            _manager = ConnectivityManager.FromContext(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity);
            //_manager = ConnectivityManager.FromContext(InTheHand.Platform.Android.ContextManager.Context);
#elif __IOS__
            _reachability = new NetworkReachability("0.0.0.0");
#endif
        }


        /// <summary>
        /// Gets the connection profile associated with the internet connection currently used by the local machine.
        /// </summary>
        /// <returns></returns>
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
            return new Connectivity.ConnectionProfile(Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile());
#endif
            return null;
        }

        private static event NetworkStatusChangedEventHandler networkStatusChanged;
        /// <summary>
        /// Occurs when the network status changes for a connection.
        /// </summary>
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
//#endif