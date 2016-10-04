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
        X86 = 0,
        Arm = 5,
        X64 = 9,
        Neutral = 11,
        Unknown = 0xFFFF,
    }
}
#endif