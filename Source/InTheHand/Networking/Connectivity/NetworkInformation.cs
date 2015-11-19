//-----------------------------------------------------------------------
// <copyright file="NetworkInformation.cs" company="In The Hand Ltd">
//     Copyright © 2014-15 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if __IOS__
using SystemConfiguration;
#endif
namespace InTheHand.Networking.Connectivity
{
    public static class NetworkInformation
    {
#if __IOS__
        private static NetworkReachability _reachability = new NetworkReachability("0.0.0.0");
        private static void ReachabilityNotification(NetworkReachabilityFlags flags)
        {
            if(NetworkStatusChanged != null)
            {
                NetworkStatusChanged(null);
            }
        }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private static void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (NetworkStatusChanged != null)
            {
                NetworkStatusChanged(null);
            }
        }
#endif

        static NetworkInformation()
        {
#if __IOS__
            _reachability.SetNotification(new NetworkReachability.Notification(ReachabilityNotification));
            _reachability.Schedule(CoreFoundation.CFRunLoop.Current, CoreFoundation.CFRunLoop.ModeDefault);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            Windows.Networking.Connectivity.NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
#endif
        }



        public static ConnectionProfile GetInternetConnectionProfile()
        {
#if __IOS__
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

        public static event NetworkStatusChangedEventHandler NetworkStatusChanged;
    }
}
