// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Environment.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if WINDOWS_UWP
using Windows.System.Profile;
#else
using InTheHand.System.Profile;
#endif
using System;

namespace InTheHand
{
    /// <summary>
    /// 
    /// </summary>
    public static class Environment
    {
        private static OperatingSystem _operatingSystem = new OperatingSystem();
        public static OperatingSystem OSVersion
        {
            get
            {
                return _operatingSystem;
            }
        }
    }

    public sealed class OperatingSystem
    {
        private Version _version;
        public Version Version
        {
            get
            {

                if (_version == null)
                {
#if __ANDROID__ || __IOS__ || WINDOWS_PHONE
                    _version = global::System.Environment.OSVersion.Version;
#else
                    string rawString = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
                    if (string.IsNullOrEmpty(rawString))
                    {
                        //default value
                        _version = new Version(8, 1, 0, 0);
                    }
                    else
                    {
                        ulong raw = ulong.Parse(rawString);
                        int major = (int)(raw & 0xFFFF000000000000L) >> 48;
                        int minor = (int)(raw & 0x0000FFFF00000000L) >> 32;
                        int build = (int)(raw & 0x00000000FFFF0000L) >> 16;
                        int revision = (int)(raw & 0x000000000000FFFFL);
                        _version = new Version(10 + major, minor, build, revision);
                    }
#endif
                }

                return _version;
            }
        }

        public string VersionString
        {
            get
            {
                return Version.ToString();
            }
        }
    }
}