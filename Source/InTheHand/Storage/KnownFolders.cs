//-----------------------------------------------------------------------
// <copyright file="KnownFolders.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Storage.KnownFolders))]
#else

using System;

namespace Windows.Storage
{

    /// <summary>
    /// Provides access to common locations that contain user content.
    /// This includes content from a user's local libraries (such as Documents, Pictures, Music, and Videos).
    /// </summary>
    public static class KnownFolders
    {
        /// <summary>
        /// Gets the Camera Roll folder.
        /// </summary>
        public static StorageFolder CameraRoll
        {
            get
            {
#if __ANDROID__
                return StorageFolder.GetFolderFromPathAsync(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim).AbsolutePath).GetResults();
#else
                return null;
#endif
            }
        }

        /// <summary>
        /// Gets the Documents library.
        /// </summary>
        public static StorageFolder DocumentsLibrary
        {
            get
            {
#if __ANDROID__ || __IOS__
                return GetStorageFolderForSpecialFolder(Environment.SpecialFolder.MyDocuments);
#else
                return null;
#endif
            }
        }

        /// <summary>
        /// Gets the Music library.
        /// </summary>
        public static StorageFolder MusicLibrary
        {
            get
            {
#if __ANDROID__ || __IOS__
                return GetStorageFolderForSpecialFolder(Environment.SpecialFolder.MyMusic);
#else
                return null;
#endif
            }
        }

        /// <summary>
        /// Gets the Pictures library.
        /// </summary>
        public static StorageFolder PicturesLibrary
        {
            get
            {
#if __ANDROID__ || __IOS__
                return GetStorageFolderForSpecialFolder(Environment.SpecialFolder.MyPictures);
#else
                return null;
#endif
            }
        }

        /// <summary>
        /// Gets the Videos library.
        /// </summary>
        public static StorageFolder VideosLibrary
        {
            get
            {
#if __ANDROID__ || __IOS__
                return GetStorageFolderForSpecialFolder(Environment.SpecialFolder.MyVideos);
#else
                return null;
#endif
            }
        }

#if __ANDROID__ || __IOS__
        private static StorageFolder GetStorageFolderForSpecialFolder(global::System.Environment.SpecialFolder folder)
        {
            string path = global::System.Environment.GetFolderPath(folder);
            return StorageFolder.GetFolderFromPathAsync(path).GetResults();
        }
#endif
    }
}
#endif