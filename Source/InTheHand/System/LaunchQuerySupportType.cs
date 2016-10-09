//-----------------------------------------------------------------------
// <copyright file="LaunchQuerySupportType.cs" company="In The Hand Ltd">
//     Copyright © 2015-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.System.LaunchQuerySupportType))]
#else

namespace Windows.System
{
    /// <summary>
    /// Specifies the type of activation to query for.
    /// </summary>
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
#endif