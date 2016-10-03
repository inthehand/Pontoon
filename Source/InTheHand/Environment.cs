// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Environment.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Windows.System.Profile;
using System;

namespace InTheHand
{
    /// <summary>
    /// Provides information about the Operating System.
    /// </summary>
    /// <seealso cref="System.Environment"/>
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
#if __ANDROID__ || __IOS__ || WIN32 || WINDOWS_PHONE
                    _operatingSystem = System.Environment.OSVersion;
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