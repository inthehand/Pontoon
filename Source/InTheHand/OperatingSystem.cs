// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperatingSystem.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if __ANDROID__ || __IOS__ || WINDOWS_PHONE || WIN32
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(global::System.OperatingSystem))]
#else

namespace System
{
    /// <summary>
    /// Represents information about an operating system, such as the version and platform identifier.
    /// </summary>
    public sealed class OperatingSystem
    {
        public OperatingSystem(PlatformID platform, Version version)
        {
            _version = version;
        }

        private PlatformID _platform;

        /// <summary>
        /// Gets a <see cref="PlatformID"/> enumeration value that identifies the operating system platform.
        /// </summary>
        public PlatformID Platform
        {
            get
            {
                return _platform;
            }
        }

        private Version _version;
        /// <summary>
        /// Gets a <see cref="Version"/> object that identifies the operating system.
        /// </summary>
        public Version Version
        {
            get
            {
                return _version;
            }
        }
    }
}
#endif