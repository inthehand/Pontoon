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

namespace InTheHand.System.Profile
{
    /// <summary>
    /// Starts the default app associated with the specified file or URI.
    /// </summary>
    public static class AnalyticsInfo
    {
        private static AnalyticsVersionInfo _versionInfo;
        private static Type _type10;
        static AnalyticsInfo()
        {
            _type10 = Type.GetType("Windows.System.Profile.AnalyticsInfo, Windows, ContentType=WindowsRuntime");
        }

        public static string DeviceForm
        {
            get
            {
                if(_type10 != null)
                {
                    return _type10.GetRuntimeProperty("DeviceForm").GetValue(null).ToString();
                }

                return "Unknown";
            }
        }

        public static AnalyticsVersionInfo VersionInfo
        {
            get
            {
                if (_versionInfo == null)
                {
                    if (_type10 != null)
                    {
                        _versionInfo = new AnalyticsVersionInfo(_type10.GetRuntimeProperty("VersionInfo").GetValue(null));
                    }
                    else
                    {
                        _versionInfo = new AnalyticsVersionInfo(null);
                    }
                }

                return _versionInfo;
            }
        }
    }
}
