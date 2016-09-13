//-----------------------------------------------------------------------
// <copyright file="AnalyticsInfo.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if __ANDROID__
using Android.App;
using Android.Content;
#endif
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Windows.System.Profile
{
    /// <summary>
    /// Starts the default app associated with the specified file or URI.
    /// </summary>
    public static class AnalyticsInfo
    {
        private static AnalyticsVersionInfo _versionInfo;
#if WINDOWS_APP || WINDOWS_PHONE_APP
        private static Type _type10;

        static AnalyticsInfo()
        {
            _type10 = Type.GetType("Windows.System.Profile.AnalyticsInfo, Windows, ContentType=WindowsRuntime");
        }
#endif

        public static string DeviceForm
        {
            get
            {
#if WINDOWS_APP || WINDOWS_PHONE_APP
                if(_type10 != null)
                {
                    return _type10.GetRuntimeProperty("DeviceForm").GetValue(null).ToString();
                }
#elif WINDOWS_UWP
                return Windows.System.Profile.AnalyticsInfo.DeviceForm;
#endif
                return "Unknown";
            }
        }

        public static AnalyticsVersionInfo VersionInfo
        {
            get
            {
                if (_versionInfo == null)
                {
#if __ANDROID__ || __IOS__ || WINDOWS_PHONE
                    _versionInfo = new AnalyticsVersionInfo(null);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
                    if (_type10 != null)
                    {
                        _versionInfo = new AnalyticsVersionInfo(_type10.GetRuntimeProperty("VersionInfo").GetValue(null));
                    }
                    else
                    {
                        _versionInfo = new AnalyticsVersionInfo(null);
                    }
#elif WINDOWS_UWP
                    _versionInfo = new AnalyticsVersionInfo(Windows.System.Profile.AnalyticsInfo.VersionInfo);
#endif
                }

                return _versionInfo;
            }
        }
    }
}
