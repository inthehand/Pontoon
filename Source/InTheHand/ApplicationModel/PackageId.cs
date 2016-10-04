// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageId.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-16 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Provides package identification info, such as name, version, and publisher.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.ApplicationModel.PackageId))]
#else

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.ApplicationModel;
using Windows.System;

#if __ANDROID__
using Android.App;
using Android.Content.PM;
#elif __IOS__
using Foundation;
#endif

namespace Windows.ApplicationModel
{
    /// <summary>
    /// Provides package identification info, such as name, version, and publisher.
    /// </summary>
    public sealed class PackageId
    {
#if __ANDROID__
        PackageInfo _packageInfo;
#endif
        internal PackageId()
        {
#if __ANDROID__
            _packageInfo = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.MetaData);
#endif
        }

        /// <summary>
        /// Gets the processor architecture for which the package was created.
        /// </summary>
        public ProcessorArchitecture Architecture
        {
            get
            {
#if __ANDROID__ || __IOS__ || WIN32
                return (ProcessorArchitecture)((int)global::System.Reflection.Assembly.GetEntryAssembly().GetName().ProcessorArchitecture);
#endif
                return ProcessorArchitecture.Unknown;
            }
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
#elif WIN32
                return Package.Current._manifest.Guid.ToString();
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
#elif WIN32
                return Package.Current._manifest.Title;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /*public string ProductId
        {
            get
            {
#if WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return InTheHand.ApplicationModel.Package.Current._appxManifest.PhoneProductId.ToString();
#elif WINDOWS_PHONE
                return InTheHand.ApplicationModel.Package.Current._appManifest.ProductID;
#elif WIN32
                return Package.Current._manifest.Guid.ToString();
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }*/

        /// <summary>
        /// Gets the publisher of the package.
        /// </summary>
        public string Publisher
        {
            get
            {
#if WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return InTheHand.ApplicationModel.Package.Current.Id.Publisher;
#elif WINDOWS_PHONE
                return InTheHand.ApplicationModel.Package.Current._appManifest.PublisherDisplayName;
#elif WIN32
                return Package.Current._manifest.Company.ToString();
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the package version info.
        /// </summary>
        /// <value>The package version information.</value>
        public PackageVersion Version
        {

            get
            {
#if __ANDROID__
                return global::System.Version.Parse(_packageInfo.VersionName).ToPackageVersion();
#elif __IOS__
                if (NSBundle.MainBundle.InfoDictionary.ContainsKey(new NSString("CFBundleShortVersionString")))
                {
                    return global::System.Version.Parse(NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString()).ToPackageVersion();
                }

                return global::System.Version.Parse(NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString()).ToPackageVersion();
#elif WINDOWS_PHONE
                return Package.Current._appManifest.Version;
#elif WIN32
                return Package.Current._manifest.AssemblyVersion.ToPackageVersion();
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }
    }
}
#endif