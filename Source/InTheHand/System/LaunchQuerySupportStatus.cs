//-----------------------------------------------------------------------
// <copyright file="LaunchQuerySupportStatus.cs" company="In The Hand Ltd">
//     Copyright © 2015-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.System.LaunchQuerySupportStatus))]
//#else

namespace InTheHand.System
{
    /// <summary>
    /// Specifies whether an app is available that supports activation
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>-</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>-</description></item>
    /// <item><term>Windows Phone Store</term><description>-</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>-</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>-</description></item></list>
    /// </remarks>
    public enum LaunchQuerySupportStatus
    {
        /// <summary>
        /// An app that handles the activation is available and may be activated.
        /// </summary>
        Available = 0,

        /// <summary>
        /// No app is installed to handle the activation.
        /// </summary>
        AppNotInstalled = 1,

        /// <summary>
        /// An app that handles the activation is installed but not available because it is being updated by the store or it was installed on a removable device that is not available.
        /// </summary>
        AppUnavailable = 2,

        /// <summary>
        /// The app does not handle the activation.
        /// </summary>
        NotSupported = 3,

        /// <summary>
        /// An unknown error was encountered while determining whether an app supports the activation.
        /// </summary>
        Unknown = 4,
    }
}
//#endif