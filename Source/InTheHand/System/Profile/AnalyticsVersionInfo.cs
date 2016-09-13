//-----------------------------------------------------------------------
// <copyright file="AnalyticsVersionInfo.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Reflection;

namespace Windows.System.Profile
{
    /// <summary>
    /// Provides version information about the device family.
    /// </summary>
    public sealed class AnalyticsVersionInfo
    {
        private object _native;

        internal AnalyticsVersionInfo(object native)
        {
            _native = native;
        }

        public string DeviceFamily
        {
            get
            {
                if(_native != null)
                {
                    return _native.GetType().GetRuntimeProperty("DeviceFamily").GetValue(_native).ToString();
                }
#if WINDOWS_APP
                return "Windows.Desktop";
#elif WINDOWS_PHONE_APP || WINDOWS_PHONE
                return "Windows.Mobile";
#else
                return string.Empty;
#endif
            }
        }

        public string DeviceFamilyVersion
        {
            get
            {
#if __ANDROID__ || __IOS__ || WINDOWS_PHONE
                return global::System.Environment.OSVersion.Version.ToString();
#else
                if (_native != null)
                {
                    return _native.GetType().GetRuntimeProperty("DeviceFamilyVersion").GetValue(_native).ToString();
                }

                return string.Empty;
#endif
            }
        }
    }
}
