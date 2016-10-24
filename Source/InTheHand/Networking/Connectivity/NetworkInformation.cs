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
#elif WIN32
using System.Net.NetworkInformation;
#endif

namespace InTheHand.Networking.Connectivity
{
    /// <summary>
    /// Provides access to network connection information for the local machine.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
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
            return new ConnectionProfile(Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile());
#elif WIN32
            foreach(var i in NetworkInterface.GetAllNetworkInterfaces())
            {
                if(i.OperationalStatus == OperationalStatus.Up && i.NetworkInterfaceType != NetworkInterfaceType.Tunnel && i.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    return new ConnectionProfile(i);
                }
            }
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
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                    Windows.Networking.Connectivity.NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
#elif WIN32
                    NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
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
#elif WIN32
                    NetworkChange.NetworkAvailabilityChanged -= NetworkChange_NetworkAvailabilityChanged;
#endif
                }
            }
        }



#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private static void NetworkInformation_NetworkStatusChanged(object sender)
        {
            OnNetworkStatusChanged();
        }
#elif WIN32
        private static void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            OnNetworkStatusChanged();
        }
#endif

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