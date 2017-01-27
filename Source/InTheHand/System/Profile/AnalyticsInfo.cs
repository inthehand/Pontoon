//-----------------------------------------------------------------------
// <copyright file="AnalyticsInfo.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Reflection;

namespace InTheHand.System.Profile
{
    /// <summary>
    /// Provides information about the device for profiling purposes.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public static class AnalyticsInfo
    {
        private static AnalyticsVersionInfo _versionInfo;
#if WINDOWS_APP || WINDOWS_PHONE_APP 
        /*|| WIN32*/
        private static Type _type10;

        static AnalyticsInfo()
        {
            _type10 = Type.GetType("Windows.System.Profile.AnalyticsInfo, Windows, ContentType=WindowsRuntime");
        }
#endif
        /// <summary>
        /// Gets the device form factor.
        /// For example, the app could be running on a phone, tablet, desktop, and so on.
        /// </summary>
        public static string DeviceForm
        {
            get
            {
#if WINDOWS_UWP
                return Windows.System.Profile.AnalyticsInfo.DeviceForm;
#elif WINDOWS_APP || WINDOWS_PHONE_APP
                if(_type10 != null)
                {
                    return _type10.GetRuntimeProperty("DeviceForm").GetValue(null).ToString();
                }
#endif
                return "Unknown";
            }
        }

        /// <summary>
        /// Gets version info about the device family.
        /// </summary>
        public static AnalyticsVersionInfo VersionInfo
        {
            get
            {
                if (_versionInfo == null)
                {
#if __ANDROID__ || __UNIFIED__ || WINDOWS_PHONE || WIN32 || WINDOWS_UWP
                    _versionInfo = new AnalyticsVersionInfo(null);
/*#elif WIN32
                    if(_type10 != null)
                    {
                        _versionInfo = new AnalyticsVersionInfo(_type10.GetRuntimeProperty("VersionInfo").GetValue(null));
                    }
                    _versionInfo = new AnalyticsVersionInfo(null);*/
#elif WINDOWS_APP || WINDOWS_PHONE_APP
                    if (_type10 != null)
                    {
                        _versionInfo = new AnalyticsVersionInfo(_type10.GetRuntimeProperty("VersionInfo").GetValue(null));
                    }
                    else
                    {
                        _versionInfo = new AnalyticsVersionInfo(null);
                    }
#endif
                }

                return _versionInfo;
            }
        }
    }
}