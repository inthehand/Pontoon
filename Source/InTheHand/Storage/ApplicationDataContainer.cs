//-----------------------------------------------------------------------
// <copyright file="ApplicationDataContainer.cs" company="In The Hand Ltd">
//     Copyright (c) 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Storage.ApplicationDataContainer))]
#else

using System;
using Windows.Foundation.Collections;
namespace Windows.Storage
{
    /// <summary>
    /// Represents a container for app settings.
    /// The methods and properties of this class support creating, deleting, enumerating, and traversing the container hierarchy.
    /// </summary>
    public sealed class ApplicationDataContainer
    {
        private ApplicationDataContainerSettings _settings;
        private ApplicationDataLocality _locality;

        internal ApplicationDataContainer(ApplicationDataLocality locality)
        {
            _locality = locality;
            _settings = new ApplicationDataContainerSettings(locality);
        }


        public ApplicationDataLocality Locality
        {
            get
            {
                return _locality;
            }
        }

        public string Name
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets an object that represents the settings in this settings container.
        /// </summary>
        /// <value>The settings map object.</value>
        public IPropertySet Values
        {
            get
            {
                return _settings;
            }
        }
    }
}
#endif