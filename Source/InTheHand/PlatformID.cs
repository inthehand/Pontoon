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
        Win32S = 0,
        Win32Windows = 1,
        Win32NT = 2,
        WinCE = 3,
        Unix = 4,
        Xbox = 5,
        MacOSX = 6,
    }
}
#endif