// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageExtensions.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-16 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Provides information about an app package.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using InTheHand.ApplicationModel;
using Windows.ApplicationModel;
using System.Reflection;

#if __ANDROID__
using Android.App;
using Android.Content.PM;
using InTheHand;
#elif __IOS__
using Foundation;
#endif

namespace InTheHand.ApplicationModel
{
    /// <summary>
    /// Provides information about a package.
    /// </summary>
    public static class PackageExtensions
    {
        private static bool? _isDevelopmentMode;

        /// <summary>
        /// Gets the date on which the application package was installed or last updated.
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static DateTimeOffset GetInstalledDate(this Package package)
        {
#if __ANDROID__ || __IOS__ || WINDOWS_UWP || WIN32
            return package.InstalledDate;

#elif WINDOWS_APP
            PropertyInfo pi = package.GetType().GetRuntimeProperty("InstalledDate");
            if (pi != null)
            {
                DateTimeOffset dt = (DateTimeOffset)pi.GetValue(package);
                return dt;
            }

            return package.InstalledLocation.DateCreated;

#elif WINDOWS_PHONE || WINDOWS_PHONE_APP
            return package.InstallDate;

#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Indicates whether the package is installed in development mode.
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static bool GetIsDevelopmentMode(this Package package)
        {
#if __ANDROID__||  __IOS__ ||  WINDOWS_UWP
            return package.IsDevelopmentMode;
#elif WINDOWS_PHONE_APP || WINDOWS_PHONE
                if (!_isDevelopmentMode.HasValue)
                {
#if WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                    Task<bool> t = Task.Run<bool>(async () =>
                    {
                        foreach (StorageFile file in await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFilesAsync())
                        {
                            if (file.Name == "AppxSignature.p7x")
                            {
                                return false;
                            }
                        }

                        return true;
                    });

                    t.Wait();
                    _isDevelopmentMode = t.Result;
#else
                    // Side-loaded apps do not contain WMAppPRHeader license
                    global::System.Windows.Resources.StreamResourceInfo sri = global::System.Windows.Application.GetResourceStream(new Uri("WMAppPRHeader.xml", UriKind.Relative));
                    _isDevelopmentMode = (sri == null);
#endif
                }
                return _isDevelopmentMode.Value;
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the logo of the package.
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static Uri GetLogo(this Package package)
        {
#if WINDOWS_UWP || WINDOWS_APP
            return package.Logo;
#elif WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return AppxManifest.Current.Logo;
#elif WINDOWS_PHONE
            return WMAppManifest.Current.Logo;
#else
            throw new PlatformNotSupportedException();
#endif
        }
    }
}