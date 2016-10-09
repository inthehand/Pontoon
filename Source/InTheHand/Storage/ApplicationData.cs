//-----------------------------------------------------------------------
// <copyright file="ApplicationData.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Storage.ApplicationData))]
#else

using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using InTheHand.Storage;

namespace Windows.Storage
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

        private ApplicationDataContainer localSettings;
        private ApplicationDataContainer roamingSettings;

        private ApplicationData()
        {
        }

        /// <summary>
        /// Removes all application data from the local, roaming, and temporary app data stores.
        /// </summary>
        /// <returns></returns>
        public IAsyncAction ClearAsync()
        {
            return Task.Run(async () =>
            {
                await LocalFolder.DeleteAllItems();
                LocalSettings.Values.Clear();
                await RoamingFolder.DeleteAllItems();
                RoamingSettings?.Values.Clear();
                await TemporaryFolder.DeleteAllItems();
            }).AsAsyncAction();
        }

        /// <summary>
        /// Removes all application data from the specified app data store.
        /// </summary>
        /// <param name="locality">One of the enumeration values.</param>
        /// <returns></returns>
        public IAsyncAction ClearAsync(ApplicationDataLocality locality)
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
            }).AsAsyncAction();
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
#elif WIN32
                return new StorageFolder(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Package.Current.Id.Publisher, Package.Current.Id.Name));
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
                if(localSettings == null)
                {
#if !WIN32
                    localSettings = new ApplicationDataContainer(ApplicationDataLocality.Local);
#endif
                }

                return localSettings;
            }
        }

        /// <summary>
        /// Gets the root folder in the roaming app data store.
        /// </summary>
        public StorageFolder RoamingFolder
        {
            get
            {
#if WIN32
                return new StorageFolder(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Package.Current.Id.Publisher, Package.Current.Id.Name));
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
                if (roamingSettings == null)
                {
#if __IOS__
                    roamingSettings = new ApplicationDataContainer(ApplicationDataLocality.Roaming);
#endif
                }

                return roamingSettings;
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
#elif WIN32
                return new StorageFolder(Path.GetTempPath());
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }
    }
}
#endif