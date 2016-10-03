//-----------------------------------------------------------------------
// <copyright file="ApplicationDataLocality.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Storage.ApplicationDataLocality))]
#else

namespace Windows.Storage
{
    /// <summary>
    /// Provides access to the application data store.
    /// Application data consists of settings that are local.
    /// </summary>
    public enum ApplicationDataLocality
    {
       Local,
       LocalCache,
       Roaming,
       Temporary,
    }
}
#endif