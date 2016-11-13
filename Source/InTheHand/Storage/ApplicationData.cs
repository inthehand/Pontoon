//-----------------------------------------------------------------------
// <copyright file="ApplicationData.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Storage.ApplicationData))]
//#else

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
    public sealed class ApplicationData
    {
        private static ApplicationData current = new ApplicationData();

        /// <summary>
        /// Provides access to the app data store associated with the app's app package.
        /// </summary>
        public static ApplicationData Current
        {
            get
            {
                return current;
            }
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Storage.ApplicationData _applicationData;
#else
#if WINDOWS_PHONE
        private Windows.Storage.ApplicationData _applicationData;
#endif
        private ApplicationDataContainer _localSettings;
        private ApplicationDataContainer _roamingSettings;
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
                await LocalFolder.DeleteAllItems();
                LocalSettings.Values.Clear();
                await RoamingFolder.DeleteAllItems();
                RoamingSettings?.Values.Clear();
                await TemporaryFolder.DeleteAllItems();
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
                        await LocalFolder.DeleteAllItems();
                        LocalSettings.Values.Clear();
                        break;
                    case ApplicationDataLocality.Roaming:
                        await RoamingFolder.DeleteAllItems();
                        RoamingSettings?.Values.Clear();
                        break;
                    case ApplicationDataLocality.Temporary:
                        await TemporaryFolder.DeleteAllItems();
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
#if __ANDROID__ || __IOS__
                return new StorageFolder(global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.Personal));
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return new StorageFolder(_applicationData.LocalFolder);
#elif WIN32
                return new StorageFolder(Path.Combine(global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.LocalApplicationData), Package.Current.Id.Publisher, Package.Current.Id.Name));
#else
                throw new PlatformNotSupportedException();
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
                return new StorageFolder(_applicationData.RoamingFolder);
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
#if __IOS__
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
#elif __IOS__
                return new StorageFolder("tmp/");
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return new StorageFolder(_applicationData.TemporaryFolder);
#elif WIN32
                return new StorageFolder(Path.GetTempPath());
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }
    }
}
//#endif