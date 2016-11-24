// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessorArchitecture.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.System.ProcessorArchitecture))]
//#else

namespace InTheHand.System
{
    /// <summary>
    /// Specifies the processor architecture supported by an app.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
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
//#endif