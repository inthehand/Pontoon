// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkConnectivityLevel.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-16 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Networking.Connectivity.NetworkConnectivityLevel))]
//#else

namespace InTheHand.Networking.Connectivity
{
    /// <summary>
    /// Defines the level of connectivity currently available.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public enum NetworkConnectivityLevel
    {
        /// <summary>
        /// No connectivity.
        /// </summary>
        None = 0,

        /// <summary>
        /// Local network access only.
        /// </summary>
        LocalAccess = 1,

        /// <summary>
        /// Limited internet access. 
        /// <para>This value indicates captive portal connectivity, where local access to a web portal is provided, but access to the Internet requires that specific credentials are provided via the portal.
        /// This level of connectivity is generally encountered when using connections hosted in public locations (e.g. coffee shops and book stores).</para>
        /// Note  This doesn't guarantee detection of a captive portal.
        /// Windows Store apps should also test if the captive portal can be reached using a URL for the captive portal, or by attempting access to a public web site which will then redirect to the captive portal when Windows reports LocalAccess as the current NetworkConnectivityLevel.
        /// </summary>
        ConstrainedInternetAccess = 2,

        /// <summary>
        /// Local and Internet access.
        /// </summary>
        InternetAccess = 3,
    }
}
//#endif