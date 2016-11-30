//-----------------------------------------------------------------------
// <copyright file="ApplicationDataContainer.cs" company="In The Hand Ltd">
//     Copyright (c) 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using InTheHand.Foundation.Collections;

namespace InTheHand.Storage
{
    /// <summary>
    /// Represents a container for app settings.
    /// The methods and properties of this class support creating, deleting, enumerating, and traversing the container hierarchy.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public sealed class ApplicationDataContainer
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Storage.ApplicationDataContainer _container;

        internal ApplicationDataContainer(Windows.Storage.ApplicationDataContainer container)
        {
            _container = container;
        }

        public static implicit operator Windows.Storage.ApplicationDataContainer(ApplicationDataContainer c)
        {
            return c._container;
        }
#else
        private ApplicationDataContainerSettings _settings;
        private ApplicationDataLocality _locality;
        private string _name;

        internal ApplicationDataContainer(ApplicationDataLocality locality, string name)
        {
            _locality = locality;
            _name = name;
            _settings = new ApplicationDataContainerSettings(locality, name);
        }
#endif

        /// <summary>
        /// Creates or opens the specified settings container in the current settings container.
        /// </summary>
        /// <param name="name">The name of the container.</param>
        /// <param name="disposition">One of the enumeration values.</param>
        /// <remarks>On iOS the name must be a value Shared App Group name and disposition must be Existing.</remarks>
        /// <returns>The settings container.</returns>
        public ApplicationDataContainer CreateContainer(string name, ApplicationDataCreateDisposition disposition)
        {
#if __IOS__ || __TVOS__ 
            if (disposition != ApplicationDataCreateDisposition.Existing)
            {
                throw new ArgumentException("Only ApplicationDataCreateDisposition.Existing is supported", "disposition");
            }

            return new Storage.ApplicationDataContainer(ApplicationDataLocality.SharedLocal, name);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return new ApplicationDataContainer(_container.CreateContainer(name, (Windows.Storage.ApplicationDataCreateDisposition)((int)disposition)));
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the type (local or roaming) of the app data store that is associated with the current settings container.
        /// </summary>
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

        /// <summary>
        /// Gets the name of the current settings container.
        /// </summary>
        public string Name
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _container.Name;
#else
                return _name;
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

    /// <summary>
    /// Specifies options for creating application data containers or returning existing containers.
    /// <para>This enumeration is used by the <see cref="ApplicationDataContainer.CreateContainer"/> method.</para>
    /// </summary>
    public enum ApplicationDataCreateDisposition
    {
        /// <summary>
        /// Always returns the specified container.
        /// Creates the container if it does not exist.
        /// </summary>
        Always = 0,

        /// <summary>
        /// Returns the specified container only if it already exists.
        /// Raises an exception of type System.Exception if the specified container does not exist.
        /// </summary>
        Existing = 1,
    }
}