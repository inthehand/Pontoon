// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkConnectivityLevel.cs" company="In The Hand Ltd">
//   Copyright (c) 2015 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Networking.Connectivity
{
    /// <summary>
    /// Defines the level of connectivity currently available.
    /// </summary>
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