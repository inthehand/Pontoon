// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessorArchitecture.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.System.ProcessorArchitecture))]
#else

namespace Windows.System
{
    /// <summary>
    /// Specifies the processor architecture supported by an app.
    /// </summary>
    public enum ProcessorArchitecture
    {
        /// <summary>
        /// The x86 processor architecture.
        /// </summary>
        X86 = 0,
        /// <summary>
        /// The ARM processor architecture.
        /// </summary>
        Arm = 5,
        /// <summary>
        /// The x64 processor architecture.
        /// </summary>
        X64 = 9,
        /// <summary>
        /// A neutral processor architecture.
        /// </summary>
        Neutral = 11,
        /// <summary>
        /// An unknown processor architecture.
        /// </summary>
        Unknown = 0xFFFF,
    }
}
#endif