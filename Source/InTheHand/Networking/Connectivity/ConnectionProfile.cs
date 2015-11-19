// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionProfile.cs" company="In The Hand Ltd">
//   Copyright (c) 2015 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
#if __IOS__
using SystemConfiguration;
#endif

namespace InTheHand.Networking.Connectivity
{
    public sealed class ConnectionProfile
    {
#if __ANDROID__
        internal ConnectionProfile()
        {
        }
#elif __IOS__
        private NetworkReachabilityFlags _flags;

        internal ConnectionProfile(NetworkReachabilityFlags flags)
        {
            this._flags = flags;
        }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Networking.Connectivity.ConnectionProfile _profile;
        internal ConnectionProfile(Windows.Networking.Connectivity.ConnectionProfile profile)
        {
            _profile = profile;
        }
#endif

        public NetworkConnectivityLevel GetNetworkConnectivityLevel()
        {
#if __IOS__
            switch(_flags)
            {
                case NetworkReachabilityFlags.Reachable:
                    return NetworkConnectivityLevel.InternetAccess;

                default:
                    return NetworkConnectivityLevel.None;
            }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return (NetworkConnectivityLevel)((int)_profile.GetNetworkConnectivityLevel());
#else
            return NetworkConnectivityLevel.None;
#endif
        }
    }
}
