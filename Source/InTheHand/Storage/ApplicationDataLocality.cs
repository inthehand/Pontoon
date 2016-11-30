//-----------------------------------------------------------------------
// <copyright file="ApplicationDataLocality.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Storage
{
    /// <summary>
    /// Provides access to the application data store.
    /// Application data consists of settings that are local.
    /// </summary>
    public enum ApplicationDataLocality
    {
        /// <summary>
        /// The data resides in the local application data store.
        /// </summary>
        Local = 0,

        /// <summary>
        /// The data resides in the roaming application data store.
        /// </summary>
        Roaming = 1,

        /// <summary>
        /// The data resides in the temporary application data store.
        /// </summary>
        Temporary = 2,

        /// <summary>
        /// The data resides in the local cache for the application data store.
        /// </summary>
        LocalCache = 3,

        /// <summary>
        /// The data resides in the shared application data store.
        /// </summary>
        SharedLocal = 4,
    }
}