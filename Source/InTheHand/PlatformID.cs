// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlatformID.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if __ANDROID__ || __IOS__ || WINDOWS_PHONE || WIN32
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(global::System.PlatformID))]
#else

namespace System
{
    /// <summary>
    /// Identifies the operating system, or platform, supported by an assembly.
    /// </summary>
    public enum PlatformID
    {
        /// <summary>
        /// The operating system is Win32s.
        /// Win32s is a layer that runs on 16-bit versions of Windows to provide access to 32-bit applications.
        /// </summary>
        Win32S = 0,
        /// <summary>
        /// The operating system is Windows 95 or Windows 98.
        /// </summary>
        Win32Windows = 1,
        /// <summary>
        /// The operating system is Windows NT or later.
        /// </summary>
        Win32NT = 2,
        /// <summary>
        /// The operating system is Windows CE.
        /// </summary>
        WinCE = 3,
        /// <summary>
        /// The operating system is Unix.
        /// </summary>
        Unix = 4,
        /// <summary>
        /// The development platform is Xbox 360.
        /// </summary>
        Xbox = 5,
        /// <summary>
        /// The operating system is Macintosh.
        /// </summary>
        MacOSX = 6,
    }
}
#endif