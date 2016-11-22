//-----------------------------------------------------------------------
// <copyright file="LaunchQuerySupportType.cs" company="In The Hand Ltd">
//     Copyright © 2015-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.System.LaunchQuerySupportType))]
//#else

namespace InTheHand.System
{
    /// <summary>
    /// Specifies the type of activation to query for.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>-</description></item>
    /// <item><term>iOS</term><description>-</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>-</description></item>
    /// <item><term>Windows Phone Store</term><description>-</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>-</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>-</description></item></list>
    /// </remarks>

    public enum LaunchQuerySupportType
    {
        /// <summary>
        /// Activate by URI but do not return a result to the calling app.
        /// This is the default.
        /// </summary>
        Uri = 0,

        /// <summary>
        /// Activate by URI and return a result to the calling app.
        /// </summary>
        UriForResults = 1,
    }
}
//#endif