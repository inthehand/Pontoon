// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionProfile.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Networking.Connectivity.ConnectionProfile))]
//#else

using System;
#if __ANDROID__
using Android.Net;
#elif __IOS__ || __TVOS__
using SystemConfiguration;
#elif WIN32
using System.Net.NetworkInformation;
#endif

namespace InTheHand.Networking.Connectivity
{
    /// <summary>
    /// Represents a network connection, which includes either the currently connected network or prior network connections.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public sealed class ConnectionProfile
    {
#if __ANDROID__
        private NetworkInfo _info;
        internal ConnectionProfile(NetworkInfo info)
        {
            _info = info;
        }
#elif __IOS__ || __TVOS__
        private NetworkReachabilityFlags _flags;

        internal ConnectionProfile(NetworkReachabilityFlags flags)
        {
            _flags = flags;
        }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Networking.Connectivity.ConnectionProfile _profile;

        internal ConnectionProfile(Windows.Networking.Connectivity.ConnectionProfile profile)
        {
            _profile = profile;
        }

        public static implicit operator Windows.Networking.Connectivity.ConnectionProfile(ConnectionProfile p)
        {
            return p._profile;
        }
#elif WIN32
        private NetworkInterface _interface;

        internal ConnectionProfile(NetworkInterface networkInterface)
        {
            _interface = networkInterface;
        }

        public static implicit operator NetworkInterface(ConnectionProfile p)
        {
            return p._interface;
        }
#endif
        /// <summary>
        /// Gets the network connectivity level for this connection.
        /// This value indicates what network resources, if any, are currently available.
        /// </summary>
        /// <returns></returns>
        public NetworkConnectivityLevel GetNetworkConnectivityLevel()
        {
#if __ANDROID__
            if(_info.IsConnected)
            {
                return NetworkConnectivityLevel.InternetAccess;
            }

            return NetworkConnectivityLevel.None;
#elif __IOS__ || __TVOS__
            switch (_flags)
            {
                case NetworkReachabilityFlags.Reachable:
                    return NetworkConnectivityLevel.InternetAccess;

                default:
                    return NetworkConnectivityLevel.None;
            }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return (NetworkConnectivityLevel) ((int)_profile.GetNetworkConnectivityLevel());
#elif WIN32
            return _interface.OperationalStatus == OperationalStatus.Up ? NetworkConnectivityLevel.InternetAccess : NetworkConnectivityLevel.None;
#else
            return NetworkConnectivityLevel.None;
#endif
        }
    }
}
//#endif