//-----------------------------------------------------------------------
// <copyright file="ApplicationDataContainer.cs" company="In The Hand Ltd">
//     Copyright (c) 2013-15 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
#if WINDOWS_PHONE
using Windows.Foundation.Collections;
#endif

namespace InTheHand.Storage
{
    /// <summary>
    /// Represents a container for app settings.
    /// The methods and properties of this class support creating, deleting, enumerating, and traversing the container hierarchy.
    /// </summary>
    public sealed class ApplicationDataContainer
    {
        private ApplicationDataContainerSettings settings = new ApplicationDataContainerSettings();

        internal ApplicationDataContainer()
        {
        }

        /// <summary>
        /// Gets an object that represents the settings in this settings container.
        /// </summary>
        /// <value>The settings map object.</value>
        public IDictionary<string, object> Values
        {
            get
            {
                return settings;
            }
        }
    }
}
