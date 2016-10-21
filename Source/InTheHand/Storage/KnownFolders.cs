//-----------------------------------------------------------------------
// <copyright file="KnownFolders.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Storage.KnownFolders))]
//#else

using System;
using System.Threading.Tasks;

namespace InTheHand.Storage
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
                var t = StorageFolder.GetFolderFromPathAsync(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim).AbsolutePath);
                t.Wait();
                return t.Result;
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
#if __ANDROID__ || __IOS__ || WIN32
                return GetStorageFolderForSpecialFolder(global::System.Environment.SpecialFolder.MyDocuments);
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
#if __ANDROID__ || __IOS__ || WIN32
                return GetStorageFolderForSpecialFolder(global::System.Environment.SpecialFolder.MyMusic);
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
#if __ANDROID__ || __IOS__ || WIN32
                return GetStorageFolderForSpecialFolder(global::System.Environment.SpecialFolder.MyPictures);
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
#if __ANDROID__ || __IOS__ || WIN32
                return GetStorageFolderForSpecialFolder(global::System.Environment.SpecialFolder.MyVideos);
#else
                return null;
#endif
            }
        }

#if __ANDROID__ || __IOS__ || WIN32
        private static StorageFolder GetStorageFolderForSpecialFolder(global::System.Environment.SpecialFolder folder)
        {
            string path = global::System.Environment.GetFolderPath(folder);
            var t = StorageFolder.GetFolderFromPathAsync(path);
            t.Wait();
            return t.Result;
        }
#endif
    }
}
//#endif