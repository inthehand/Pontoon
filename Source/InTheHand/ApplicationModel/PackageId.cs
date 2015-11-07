// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageId.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-15 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Provides package identification info, such as name, version, and publisher.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if __ANDROID__
using Android.App;
using Android.Content.PM;
#elif __IOS__
using Foundation;
#endif
namespace InTheHand.ApplicationModel
{
    /// <summary>
    /// Provides package identification info, such as name, version, and publisher.
    /// </summary>
    public sealed class PackageId
    {
#if __ANDROID__
        PackageInfo _packageInfo;
#elif __IOS__
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81 || WINDOWS_UWP
        private Windows.ApplicationModel.PackageId _packageId;
#endif
        internal PackageId()
        {
#if __ANDROID__
            _packageInfo = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.MetaData);
#elif __IOS__
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81 || WINDOWS_UWP
            _packageId = Windows.ApplicationModel.Package.Current.Id;
#endif
        }

        /// <summary>
        /// Gets the name of the package.
        /// </summary>
        /// <value>The package name.</value>
        public string FullName
        {
            get
            {
#if __ANDROID__
                return Android.App.Application.Context.PackageName;
#elif __IOS__
                return NSBundle.MainBundle.InfoDictionary["CFBundleIdentifier"].ToString();
#elif WINDOWS_APP || WINDOWS_PHONE_81 || WINDOWS_PHONE_APP || WINDOWS_UWP
                return _packageId.FullName;
#elif WINDOWS_PHONE
                return Package.Current._appManifest.FullName;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }


        /// <summary>
        /// Gets the name of the package.
        /// </summary>
        /// <value>The package name.</value>
        public string Name
        {
            get
            {
#if __ANDROID__
                return _packageInfo.PackageName;
#elif __IOS__
                return NSBundle.MainBundle.InfoDictionary["CFBundleExecutable"].ToString();
#elif WINDOWS_APP || WINDOWS_PHONE_81 || WINDOWS_PHONE_APP || WINDOWS_UWP
                return _packageId.Name;
#elif WINDOWS_PHONE
                return Package.Current._appManifest.DisplayName;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }
        
        /// <summary>
        /// Gets the package version info.
        /// </summary>
        /// <value>The package version information.</value>
        public Version Version
        {

            get
            {
#if __ANDROID__
                return Version.Parse(_packageInfo.VersionName);
#elif __IOS__
                return Version.Parse(NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString());
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
                return _packageId.Version.ToVersion();
#elif WINDOWS_PHONE
                return Package.Current._appManifest.Version;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }
    }
}
