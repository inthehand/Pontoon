//-----------------------------------------------------------------------
// <copyright file="KnownFolders.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace InTheHand.Storage
{

    /// <summary>
    /// Provides access to common locations that contain user content.
    /// This includes content from a user's local libraries (such as Documents, Pictures, Music, and Videos).
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
    public static class KnownFolders
    {
        /// <summary>
        /// Gets the Camera Roll folder.
        /// </summary>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list>
        /// </remarks>
        public static StorageFolder CameraRoll
        {
            get
            {
#if __ANDROID__
                var t = StorageFolder.GetFolderFromPathAsync(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim).AbsolutePath);
                t.Wait();
                return t.Result;
#elif TIZEN
                return GetStorageFolderForDirectoryType(Tizen.System.DirectoryType.Camera);
                
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return new StorageFolder(Windows.Storage.KnownFolders.CameraRoll);
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
#if __ANDROID__ || __IOS__ || __MAC__ || WIN32
                return GetStorageFolderForSpecialFolder(global::System.Environment.SpecialFolder.MyDocuments);
#elif TIZEN
                return GetStorageFolderForDirectoryType(Tizen.System.DirectoryType.Documents);

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return new StorageFolder(Windows.Storage.KnownFolders.DocumentsLibrary);
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
#if __ANDROID__ || __IOS__ || __MAC__ || WIN32
                return GetStorageFolderForSpecialFolder(global::System.Environment.SpecialFolder.MyMusic);
#elif TIZEN
                return GetStorageFolderForDirectoryType(Tizen.System.DirectoryType.Music);

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return new StorageFolder(Windows.Storage.KnownFolders.MusicLibrary);
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
#if __ANDROID__ || __IOS__ || __MAC__ || WIN32
                return GetStorageFolderForSpecialFolder(global::System.Environment.SpecialFolder.MyPictures);
#elif TIZEN
                return GetStorageFolderForDirectoryType(Tizen.System.DirectoryType.Images);

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return new StorageFolder(Windows.Storage.KnownFolders.PicturesLibrary);
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
#if __ANDROID__ || __IOS__ || __MAC__ || WIN32
                return GetStorageFolderForSpecialFolder(global::System.Environment.SpecialFolder.MyVideos);
#elif TIZEN
                return GetStorageFolderForDirectoryType(Tizen.System.DirectoryType.Videos);

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return new StorageFolder(Windows.Storage.KnownFolders.VideosLibrary);
#else
                return null;
#endif
            }
        }

#if __ANDROID__ || __UNIFIED__ || WIN32
        private static StorageFolder GetStorageFolderForSpecialFolder(global::System.Environment.SpecialFolder folder)
        {
            string path = global::System.Environment.GetFolderPath(folder);
            var t = StorageFolder.GetFolderFromPathAsync(path);
            t.Wait();
            return t.Result;
        }
#elif TIZEN
        private static StorageFolder GetStorageFolderForDirectoryType(Tizen.System.DirectoryType type)
        {
            foreach (Tizen.System.Storage s in Tizen.System.StorageManager.Storages)
            {
                if (s.State == Tizen.System.StorageState.Mounted)
                {
                    return new StorageFolder(s.GetAbsolutePath(type));
                }
            }

            return null;
        }
#endif
    }
}