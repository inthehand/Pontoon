// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Package.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-17 In The Hand Ltd, All rights reserved.
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
using InTheHand.Storage;

#if __ANDROID__
using Android.App;
using Android.Content.PM;
using InTheHand;
#elif __UNIFIED__
using Foundation;
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
using System.Reflection;
using Windows.Data.Xml.Dom;
using Windows.ApplicationModel.Core;
#endif

namespace InTheHand.ApplicationModel
{
    /// <summary>
    /// Provides information about a package.
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
    public sealed class Package
    {
        private static Package current;

        /// <summary>
        /// Gets the package for the current app.
        /// </summary>
        public static Package Current
        {
            get
            {
                if (current == null)
                {
                    current = new Package();
                }

                return current;
            }
        }

#if __ANDROID__
        PackageInfo _packageInfo;
#elif __UNIFIED__
        NSBundle _mainBundle;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        Windows.ApplicationModel.Package _package;

        public static implicit operator Windows.ApplicationModel.Package(Package p)
        {
            return p._package;
        }

        public static implicit operator Package(Windows.ApplicationModel.Package p)
        {
            return new Package();
        }
#elif TIZEN
        Tizen.Applications.ApplicationInfo _info;
#endif

        private Package()
        {
#if __ANDROID__
            _packageInfo = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.MetaData);
#elif __UNIFIED__
            _mainBundle = NSBundle.MainBundle;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _package = Windows.ApplicationModel.Package.Current;
#elif TIZEN
            _info = Tizen.Applications.Application.Current.ApplicationInfo;
#endif
        }

        /// <summary>
        /// Gets the description of the package.
        /// </summary>
        public string Description
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP
                return _package.Description;
#elif WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return AppxManifest.Current.Description;
#elif WINDOWS_PHONE
                return WMAppManifest.Current.Description;
#elif WIN32
                return AssemblyManifest.Current.Description;
#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// Gets the display name of the package.
        /// </summary>
        public string DisplayName
        {

            get
            {
#if __ANDROID__
                return Application.Context.PackageManager.GetApplicationLabel(_packageInfo.ApplicationInfo);
                //return _packageInfo.PackageName;
#elif __UNIFIED__
                return _mainBundle.InfoDictionary["CFBundleDisplayName"].ToString();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return AppxManifest.Current.DisplayName;
#elif WINDOWS_PHONE
                return WMAppManifest.Current.DisplayName;
#elif WIN32
                return AssemblyManifest.Current.Product;
#elif TIZEN
                return _info.Label;
#else
                throw new PlatformNotSupportedException();
#endif
            }

        }
        
        private PackageId _id;
        /// <summary>
        /// Gets the package identity of the current package.
        /// </summary>
        /// <value>The package identity.</value>
        public PackageId Id
        {
            get
            {
                if(_id == null)
                {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                    _id = Windows.ApplicationModel.Package.Current.Id;
#else
                    _id = new PackageId();
#endif
                }

                return _id;
            }
        }

        /// <summary>
        /// Gets the date the application package was installed on the user's phone.
        /// </summary>
        public DateTimeOffset InstalledDate
        {
            get
            {
#if __ANDROID__
                return DateTimeOffsetHelper.FromUnixTimeMilliseconds(_packageInfo.FirstInstallTime);
#elif __UNIFIED__
                DateTime d = Directory.GetCreationTime(global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.MyDocuments));
                return new DateTimeOffset(d);
#elif WINDOWS_UWP
                return _package.InstalledDate;
#elif WINDOWS_APP
                PropertyInfo pi = _package.GetType().GetRuntimeProperty("InstalledDate");
                if(pi != null)
                {
                    DateTimeOffset dt = (DateTimeOffset)pi.GetValue(_package);
                    return dt;
                }
                return _package.InstalledLocation.DateCreated;
#elif WINDOWS_PHONE_81 || WINDOWS_PHONE_APP
                // using the date the folder was created (on initial install)
                return _package.InstalledLocation.DateCreated;
#elif WIN32
                return AssemblyManifest.Current.InstalledDate;

#else
                throw new PlatformNotSupportedException();
#endif
            }
        }


        /// <summary>
        /// Gets the location of the installed package.
        /// </summary>
        /// <remarks>This folder is read-only except for Windows Desktop applications.</remarks>
        public StorageFolder InstalledLocation
        {
            get
            {
#if __ANDROID__
                return new StorageFolder(Application.Context.PackageCodePath);

#elif __UNIFIED__
                return new StorageFolder(NSBundle.MainBundle.BundlePath);

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _package.InstalledLocation;

#elif WIN32
                return new StorageFolder(AssemblyManifest.Current.InstalledLocation);

#elif TIZEN
                return new StorageFolder(Path.GetDirectoryName(_info.ExecutablePath));

#else
                return null;
#endif
            }
        }

#if WINDOWS_PHONE_APP || WINDOWS_PHONE
        private bool? _isDevelopmentMode;
#endif
                /// <summary>
                /// Indicates whether the package is installed in development mode.
                /// </summary>
                /// <remarks>A Boolean value that indicates whether the package is installed in development mode.
                /// TRUE indicates that the package is installed in development mode; otherwise FALSE.</remarks>
        public bool IsDevelopmentMode
        {
            get
            {
#if __ANDROID__
                return Android.App.Application.Context.PackageManager.GetInstallerPackageName(_packageInfo.PackageName) == null;
#elif __UNIFIED__
                return !File.Exists(_mainBundle.AppStoreReceiptUrl.Path);
#elif WINDOWS_UWP || WINDOWS_APP
                return _package.IsDevelopmentMode;
#elif WINDOWS_PHONE_APP || WINDOWS_PHONE
                if (!_isDevelopmentMode.HasValue)
                {
#if WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                    Task<bool> t = Task.Run<bool>(async () =>
                    {
                        foreach (Windows.Storage.StorageFile file in await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFilesAsync())
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
        }

        /// <summary>
        /// Gets the logo of the package.
        /// </summary>
        public Uri Logo
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP
                return _package.Logo;
#elif WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return AppxManifest.Current.Logo;
#elif WINDOWS_PHONE
                return WMAppManifest.Current.Logo;
#elif TIZEN
                return new Uri("file:///" + _info.IconPath);
#else
                return null;
#endif
            }
        }

        /// <summary>
        /// Gets the publisher display name of the package.
        /// </summary>
        /// <remarks>Android and iOS don't provide a way to query the publisher at runtime.</remarks>
        public string PublisherDisplayName
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP
                return _package.PublisherDisplayName;
#elif WINDOWS_PHONE_APP
                return AppxManifest.Current.PublisherDisplayName;
#elif WINDOWS_PHONE
                return WMAppManifest.Current.PublisherDisplayName;
#elif WIN32
                return AssemblyManifest.Current.Company;
#else
                return string.Empty;
#endif
            }
        }
    }
}
//#endif