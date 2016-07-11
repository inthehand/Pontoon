// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Package.cs" company="In The Hand Ltd">
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

#if __ANDROID__
using Android.App;
using Android.Content.PM;
#elif __IOS__
using Foundation;
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
using System.Reflection;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Storage;
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
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE || WINDOWS_UWP
        private Windows.ApplicationModel.Package _package;

        [CLSCompliant(false)]
        public static implicit operator Windows.ApplicationModel.Package(Package p)
        {
            return p._package;
        }
#endif

#if WINDOWS_PHONE_APP || WINDOWS_PHONE_81 || WINDOWS_UWP || WINDOWS_APP
        internal AppxManifest _appxManifest;
#endif

#if WINDOWS_PHONE
        internal WMAppManifest _appManifest;
#endif
        
        private Package()
        {
#if __ANDROID__
            _packageInfo = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.MetaData);

#elif __IOS__
            _mainBundle = NSBundle.MainBundle;
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE || WINDOWS_UWP
            _package = Windows.ApplicationModel.Package.Current;
#endif

#if WINDOWS_PHONE
            _appManifest = new WMAppManifest();
            Capabilities |= _appManifest.Capabilities;
            DeviceCapabilities |= _appManifest.DeviceCapabilities;
            Logo = _appManifest.Logo;
#endif
#if WINDOWS_PHONE_APP || WINDOWS_PHONE_81 || WINDOWS_UWP || WINDOWS_APP

            _appxManifest = new AppxManifest();
            BackgroundColor = _appxManifest.BackgroundColor;
            Capabilities |= _appxManifest.Capabilities;
            Description = _appxManifest.Description;
            DeviceCapabilities |= _appxManifest.DeviceCapabilities;           
#endif
#if WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            Logo = _appxManifest.Logo;
#endif
        }

#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_PHONE_81
        /// <summary>
        /// Returns the background color of the application's primary tile.
        /// </summary>
        public Windows.UI.Color BackgroundColor
        {
            get;
            private set;
        }
#endif

        /// <summary>
        /// Application capabilities requested by the package.
        /// </summary>
        [CLSCompliant(false)]
        public Capability Capabilities
        {
            get;
            private set;
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
        /// Device capabilities requested by the package.
        /// </summary>
        [CLSCompliant(false)]
        public DeviceCapability DeviceCapabilities
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
#elif WINDOWS_APP || WINDOWS_UWP
                return _package.DisplayName;
#elif WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _appxManifest.DisplayName;
#elif WINDOWS_PHONE
                return _appManifest.DisplayName;
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
                    _id = new PackageId();
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
#elif __IOS__
                DateTime d = Directory.GetCreationTime(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
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
#elif WINDOWS_UWP
                return _package.InstalledDate;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        /// <summary>
        /// Gets the location of the installed package.
        /// </summary>
        public InTheHand.Storage.StorageFolder InstalledLocation
        {
            get
            {
                return new Storage.StorageFolder(_package.InstalledLocation);
            }
        }
#endif

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
#elif WINDOWS_APP || WINDOWS_UWP
                return _package.IsDevelopmentMode;
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
#if WINDOWS_UWP || WINDOWS_APP
            get
            {
                return _package.Logo;
            }
#else
            get;
            private set;
#endif
        }

        /// <summary>
        /// Gets the publisher display name of the package.
        /// </summary>
        public string PublisherDisplayName
        {
            get
            {
#if __ANDROID__ || __IOS__
                return string.Empty;
#elif WINDOWS_APP || WINDOWS_UWP
                return _package.PublisherDisplayName;
#elif WINDOWS_PHONE_APP
                return _appxManifest.PublisherDisplayName;
#elif WINDOWS_PHONE
                return _appManifest.PublisherDisplayName;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }
    }
}
