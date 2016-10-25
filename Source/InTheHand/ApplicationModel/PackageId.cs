// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageId.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-16 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Provides package identification info, such as name, version, and publisher.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.ApplicationModel.PackageId))]
//#else

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.System;

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
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.ApplicationModel.PackageId _packageId;

        private PackageId(Windows.ApplicationModel.PackageId packageId)
        {
            _packageId = packageId;
        }

        public static implicit operator Windows.ApplicationModel.PackageId(PackageId id)
        {
            return id._packageId;
        }

        public static implicit operator PackageId(Windows.ApplicationModel.PackageId id)
        {
            return new PackageId(id);
        }
#elif __ANDROID__
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
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return (ProcessorArchitecture)((int)_packageId.Architecture);
#else
                return ProcessorArchitecture.Unknown;
#endif
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
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _packageId.FullName;
#elif WIN32
                return AssemblyManifest.Current.Guid.ToString();
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
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_81 || WINDOWS_PHONE_APP
                return _packageId.Name;
#elif WINDOWS_PHONE
                return WMAppManifest.Current.DisplayName;
#elif WIN32
                return AssemblyManifest.Current.Title;
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
        /// <remarks>Not supported on iOS or Android.</remarks>
        public string Publisher
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _packageId.Publisher;
#elif WINDOWS_PHONE
                return WMAppManifest.Current.PublisherDisplayName;
#elif WIN32
                return AssemblyManifest.Current.Company;
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
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _packageId.Version;
#elif WINDOWS_PHONE
                return WMAppManifest.Current.Version;
#elif WIN32
                return AssemblyManifest.Current.AssemblyVersion.ToPackageVersion();
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }
    }
}
//#endif