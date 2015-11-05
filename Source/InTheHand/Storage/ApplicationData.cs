//-----------------------------------------------------------------------
// <copyright file="ApplicationData.cs" company="In The Hand Ltd">
//     Copyright © 2013-14 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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

        private ApplicationDataContainer localSettings = new ApplicationDataContainer();

        private ApplicationData()
        {
        }

        /// <summary>
        /// Gets the application settings container in the local app data store.
        /// </summary>
        /// <value>The application settings container.</value>
        public ApplicationDataContainer LocalSettings
        {
            get
            {
                return localSettings;
            }
        }
    }
}
