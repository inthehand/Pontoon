//-----------------------------------------------------------------------
// <copyright file="ApplicationData.cs" company="In The Hand Ltd">
//     Copyright © 2013-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using InTheHand.ApplicationModel;

namespace InTheHand.Storage
{
    /// <summary>
    /// Provides access to the application data store.
    /// Application data consists of settings that are local.
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
    public sealed class ApplicationData
    {
        private static ApplicationData s_current = new ApplicationData();

        /// <summary>
        /// Provides access to the app data store associated with the app's app package.
        /// </summary>
        public static ApplicationData Current
        {
            get
            {
                return s_current;
            }
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Storage.ApplicationData _applicationData;
#else
#if WINDOWS_PHONE
        private Windows.Storage.ApplicationData _applicationData;
#endif
        private ApplicationDataContainer _localSettings = null;
        private ApplicationDataContainer _roamingSettings = null;
#endif

        private ApplicationData()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            _applicationData = Windows.Storage.ApplicationData.Current;
#endif
        }

        /// <summary>
        /// Removes all application data from the local, roaming, and temporary app data stores.
        /// </summary>
        /// <returns></returns>
        public Task ClearAsync()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return _applicationData.ClearAsync().AsTask();
#else
            return Task.Run(async () =>
            {
                await LocalFolder?.DeleteAllItems();
                LocalSettings?.Values.Clear();
                await RoamingFolder?.DeleteAllItems();
                RoamingSettings?.Values.Clear();
                await TemporaryFolder?.DeleteAllItems();
            });
#endif
        }
        
        /// <summary>
        /// Removes all application data from the specified app data store.
        /// </summary>
        /// <param name="locality">One of the enumeration values.</param>
        /// <returns></returns>
        public Task ClearAsync(ApplicationDataLocality locality)
        {
            return Task.Run(async () =>
            {
                switch(locality)
                {
                    case ApplicationDataLocality.Local:
                        await LocalFolder?.DeleteAllItems();
                        LocalSettings?.Values.Clear();
                        break;
                    case ApplicationDataLocality.Roaming:
                        await RoamingFolder?.DeleteAllItems();
                        RoamingSettings?.Values.Clear();
                        break;
                    case ApplicationDataLocality.Temporary:
                        await TemporaryFolder?.DeleteAllItems();
                        break;
                }
            });
        }


        /// <summary>
        /// Gets the root folder in the local app data store.
        /// </summary>
        public StorageFolder LocalFolder
        {
            get
            {
#if __ANDROID__ || __UNIFIED__ 
                return new StorageFolder(global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.Personal));

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _applicationData.LocalFolder;

#elif WIN32
                return InTheHand.ApplicationModel.Package.Current.InstalledLocation;
#else
                return null;
#endif
            }
        }


        /// <summary>
        /// Gets the application settings container in the local app data store.
        /// </summary>
        /// <value>The application settings container.</value>
        public ApplicationDataContainer LocalSettings
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return new ApplicationDataContainer(_applicationData.LocalSettings);
#else
                if (_localSettings == null)
                {
#if !WIN32
                    _localSettings = new ApplicationDataContainer(ApplicationDataLocality.Local, string.Empty);
#endif
                }

                return _localSettings;
#endif
            }
        }

        /// <summary>
        /// Gets the root folder in the roaming app data store.
        /// </summary>
        public StorageFolder RoamingFolder
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _applicationData.RoamingFolder;
#elif WIN32
                return new StorageFolder(Path.Combine(global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.ApplicationData), Package.Current.Id.Publisher, Package.Current.Id.Name));
#else
                return null;
#endif
            }
        }

        /// <summary>
        /// Gets the application settings container in the local app data store.
        /// </summary>
        /// <value>The application settings container.</value>
        public ApplicationDataContainer RoamingSettings
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return new ApplicationDataContainer(_applicationData.RoamingSettings);
#else
                if (_roamingSettings == null)
                {
#if __UNIFIED__ 
                    _roamingSettings = new ApplicationDataContainer(ApplicationDataLocality.Roaming, string.Empty);
#endif
                }

                return _roamingSettings;
#endif
            }
        }

        /// <summary>
        /// Gets the root folder in the temporary app data store.
        /// </summary>
        public StorageFolder TemporaryFolder
        {
            get
            {
#if __ANDROID__
                return new StorageFolder(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.CacheDir.AbsolutePath);

#elif __UNIFIED__ 
                return new StorageFolder("tmp/");

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _applicationData.TemporaryFolder;

#elif WIN32
                return new StorageFolder(Path.GetTempPath());

#else
                return null;
#endif
            }
        }
    }
}