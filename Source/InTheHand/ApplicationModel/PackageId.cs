// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageId.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-17 In The Hand Ltd, All rights reserved.
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
using InTheHand.System;

#if __ANDROID__
using Android.App;
using Android.Content.PM;
#elif __UNIFIED__
using Foundation;
#endif

namespace InTheHand.ApplicationModel
{
    /// <summary>
    /// Provides package identification info, such as name, version, and publisher.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed class PackageId
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.ApplicationModel.PackageId _packageId;

        private PackageId(Windows.ApplicationModel.PackageId packageId)
        {
            _packageId = packageId;
        }

        /// <summary>
        /// Converts a Windows PackageId to a Pontoon PackageId.
        /// </summary>
        /// <param name="id"></param>
        public static implicit operator Windows.ApplicationModel.PackageId(PackageId id)
        {
            return id._packageId;
        }

        /// <summary>
        /// Converts a Pontoon PackageId to a Windows PackageId.
        /// </summary>
        /// <param name="id"></param>
        public static implicit operator PackageId(Windows.ApplicationModel.PackageId id)
        {
            return new PackageId(id);
        }

#elif __ANDROID__
        PackageInfo _packageInfo;

        public static implicit operator PackageInfo(PackageId id)
        {
            return id._packageInfo;
        }

#elif TIZEN
        Tizen.Applications.ApplicationInfo _info;

        /// <summary>
        /// Converts a Pontoon PackageId to a Tizen ApplicationInfo.
        /// </summary>
        /// <param name="id"></param>
        public static implicit operator Tizen.Applications.ApplicationInfo(PackageId id)
        {
            return id._info;
        }
#endif

        internal PackageId()
        {
#if __ANDROID__
            _packageInfo = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.MetaData);
#elif TIZEN
            _info = Tizen.Applications.Application.Current.ApplicationInfo;
#endif
        }

        /// <summary>
        /// Gets the processor architecture for which the package was created.
        /// </summary>
        public ProcessorArchitecture Architecture
        {
            get
            {
#if __ANDROID__ || __UNIFIED__ || WIN32
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

#elif __UNIFIED__
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
#elif __UNIFIED__
                return NSBundle.MainBundle.InfoDictionary["CFBundleExecutable"].ToString();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_81 || WINDOWS_PHONE_APP
                return _packageId.Name;
#elif WINDOWS_PHONE
                return WMAppManifest.Current.DisplayName;
#elif WIN32
                return AssemblyManifest.Current.Title;
#elif TIZEN
                return _info.Label;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the value of the ProductID attribute associated with this application package.
        /// </summary>
        /// <value>The value of the ProductID attribute associated with this application package.</value>
        public string ProductId
        {
            get
            {
#if WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return AppxManifest.Current.PhoneProductId.ToString();
#elif WINDOWS_PHONE
                return WMAppManifest.Current.ProductID;
#elif WIN32
                return AssemblyManifest.Current.Guid.ToString();
#elif TIZEN
                return _info.ApplicationId;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

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
#elif __UNIFIED__
                string rawVersion = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
                // TODO: use Regex to replace alpha char with period
                string cleanVersion = rawVersion.Replace('a', '.').Replace('b','.').Replace('d','.').Replace("fc", ".");

                return global::System.Version.Parse(cleanVersion).ToPackageVersion();
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