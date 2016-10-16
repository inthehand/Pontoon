// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Package.cs" company="In The Hand Ltd">
//   Copyright (c) 2013-16 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Provides information about an app package.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
/*#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.ApplicationModel.Package))]
#else*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using InTheHand.ApplicationModel;

#if __ANDROID__
using Android.App;
using Android.Content.PM;
using InTheHand;
#elif __IOS__
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
#elif __IOS__
        NSBundle _mainBundle;
#endif

        private Package()
        {
#if __ANDROID__
            _packageInfo = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.MetaData);
#elif __IOS__
            _mainBundle = NSBundle.MainBundle;
#endif

#if WINDOWS_PHONE
            Capabilities |= _appManifest.Capabilities;
            DeviceCapabilities |= _appManifest.DeviceCapabilities;
            Logo = _appManifest.Logo;
#endif
#if WINDOWS_PHONE_APP || WINDOWS_PHONE_81 || WINDOWS_UWP || WINDOWS_APP

            //BackgroundColor = _appxManifest.BackgroundColor;
            //Capabilities |= _appxManifest.Capabilities;
            Description = AppxManifest.Current.Description;
            //DeviceCapabilities |= _appxManifest.DeviceCapabilities;           
#endif
#if WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            Logo = AppxManifest.Current.Logo;
#endif
#if WIN32
            Description = AssemblyManifest.Current.Description;
#endif
        }

        /// <summary>
        /// Gets the description of the package.
        /// </summary>
        public string Description
        {
            get;
            private set;
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
#elif __IOS__
                return _mainBundle.InfoDictionary["CFBundleDisplayName"].ToString();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return AppxManifest.Current.DisplayName;
#elif WINDOWS_PHONE
                return WMAppManifest.Current.DisplayName;
#elif WIN32
                return AssemblyManifest.Current.Product;
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
                if(_id==null)
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
        /// <remarks>
        /// On Windows Phone use the extension method <see cref="PackageExtensions.GetInstalledDate(Package)"/>.
        /// </remarks>
        /// <seealso cref="PackageExtensions.GetInstalledDate(Package)"/> 
        public DateTimeOffset InstalledDate
        {
            get
            {
#if __ANDROID__
                return DateTimeOffsetHelper.FromUnixTimeMilliseconds(_packageInfo.FirstInstallTime);
#elif __IOS__
                DateTime d = Directory.GetCreationTime(global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.MyDocuments));
                return new DateTimeOffset(d);
#elif WINDOWS_APP
                PropertyInfo pi = _package.GetType().GetRuntimeProperty("InstalledDate");
                if(pi != null)
                {
                    DateTimeOffset dt = (DateTimeOffset)pi.GetValue(_package);
                    return dt;
                }
                return _package.InstalledLocation.DateCreated;
#elif WINDOWS_PHONE || WINDOWS_PHONE_APP
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
#elif __IOS__
                return new StorageFolder(NSBundle.MainBundle.BundlePath);
#elif WIN32
                return new StorageFolder(AssemblyManifest.Current.InstalledLocation);
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
#elif __IOS__
                return !File.Exists(_mainBundle.AppStoreReceiptUrl.Path);
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
        }

        /// <summary>
        /// Gets the logo of the package.
        /// </summary>
        public Uri Logo
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the publisher display name of the package.
        /// </summary>
        /// <remarks>Android and iOS don't provide a way to query the publisher at runtime.</remarks>
        public string PublisherDisplayName
        {
            get
            {
#if __ANDROID__ || __IOS__
                return string.Empty;
#elif WINDOWS_PHONE_APP
                return _appxManifest.PublisherDisplayName;
#elif WINDOWS_PHONE
                return _appManifest.PublisherDisplayName;
#elif WIN32
                return AssemblyManifest.Current.Company;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }
    }
}
//#endif