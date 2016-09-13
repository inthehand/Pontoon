//-----------------------------------------------------------------------
// <copyright file="KnownFolders.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
[assembly: TypeForwardedTo(typeof(Windows.Storage.KnownFolders))]
#else

namespace Windows.Storage
{

    /// <summary>
    /// Provides access to common locations that contain user content.
    /// This includes content from a user's local libraries (such as Documents, Pictures, Music, and Videos), HomeGroup, removable devices, and media server devices.
    /// </summary>
    public static class KnownFolders
    {
        
    }
}
#endif