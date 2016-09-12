//-----------------------------------------------------------------------
// <copyright file="ApplicationDataContainer.cs" company="In The Hand Ltd">
//     Copyright (c) 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Windows.Foundation.Collections;
using System;
using System.Runtime.CompilerServices;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
[assembly: TypeForwardedTo(typeof(Windows.Storage.ApplicationDataContainer))]
#else

namespace Windows.Storage
{
    /// <summary>
    /// Represents a container for app settings.
    /// The methods and properties of this class support creating, deleting, enumerating, and traversing the container hierarchy.
    /// </summary>
    public sealed class ApplicationDataContainer
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Storage.ApplicationDataContainer _container;

        [CLSCompliant(false)]
        public static implicit operator Windows.Storage.ApplicationDataContainer(ApplicationDataContainer c)
        {
            return c._container;
        }

        internal ApplicationDataContainer(Windows.Storage.ApplicationDataContainer container)
        {
            _container = container;
        }
#else
        private ApplicationDataContainerSettings _settings;
        private ApplicationDataLocality _locality;

        internal ApplicationDataContainer(ApplicationDataLocality locality)
        {
            _locality = locality;
            _settings = new ApplicationDataContainerSettings(locality);
        }
#endif


        public ApplicationDataLocality Locality
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return (ApplicationDataLocality)((int)_container.Locality);
#else
                return _locality;
#endif
            }
        }

        public string Name
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _container.Name;
#else
                return string.Empty;
#endif
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
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return new ApplicationDataContainerSettings(_container.Values);
#else
                return _settings;
#endif
            }
        }

        
    }
}
#endif