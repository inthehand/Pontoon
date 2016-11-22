// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Environment.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using InTheHand.System.Profile;
using System;

namespace InTheHand
{
    /// <summary>
    /// Provides information about the Operating System.
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
    /// <seealso cref="global::System.Environment"/>
    public static class Environment
    {
        private static OperatingSystem _operatingSystem;
        /// <summary>
        /// Gets an OperatingSystem object that contains the current platform identifier and version number.
        /// </summary>
        public static OperatingSystem OSVersion
        {
            get
            {
                if(_operatingSystem == null)
                {
#if __ANDROID__ || __IOS__ || __TVOS__ || WIN32 || WINDOWS_PHONE
                    _operatingSystem = global::System.Environment.OSVersion;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                    Version version;
                    string rawString = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
                    if (string.IsNullOrEmpty(rawString))
                    {
                        //default value
                        version = new Version(8, 1, 0, 0);
                    }
                    else
                    {
                        ulong raw = ulong.Parse(rawString);
                        int major = (int)(raw & 0xFFFF000000000000L) >> 48;
                        int minor = (int)(raw & 0x0000FFFF00000000L) >> 32;
                        int build = (int)(raw & 0x00000000FFFF0000L) >> 16;
                        int revision = (int)(raw & 0x000000000000FFFFL);
                        version = new Version(10 + major, minor, build, revision);
                    }
                    _operatingSystem = new OperatingSystem(PlatformID.Win32NT, version);
#endif
                }

                return _operatingSystem;
            }
        }
    }
}