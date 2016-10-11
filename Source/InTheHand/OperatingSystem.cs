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
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public sealed class OperatingSystem
    {
        /// <summary>
        /// Initializes a new instance of the OperatingSystem class, using the specified platform identifier value and version object.
        /// </summary>
        /// <param name="platform">One of the <see cref="PlatformID"/> values that indicates the operating system platform.</param>
        /// <param name="version">A <see cref="Version"/> object that indicates the version of the operating system. </param>
        public OperatingSystem(PlatformID platform, Version version)
        {
            _platform = platform;
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