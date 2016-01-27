//-----------------------------------------------------------------------
// <copyright file="ApplicationDataContainer.cs" company="In The Hand Ltd">
//     Copyright (c) 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Foundation.Collections;

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
        public IPropertySet Values
        {
            get
            {
                return settings;
            }
        }
    }
}
