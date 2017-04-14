//-----------------------------------------------------------------------
// <copyright file="Launcher.cs" company="In The Hand Ltd">
//     Copyright © 2015-17 In The Hand Ltd. All rights reserved.
//     This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Storage;
using System;
using System.Threading.Tasks;

namespace InTheHand.System
{
    /// <summary>
    /// Starts the default app associated with the specified file or URI.
    /// </summary>
    /// <remarks>    
    /// <list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list></remarks>
    public static partial class Launcher
    {
        /// <summary>
        /// Starts the app associated with the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The launch operation.</returns>
        /// <remarks>    
        /// <list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
        /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list></remarks>
        public static Task<bool> LaunchFileAsync(IStorageFile file)
        {
#if __ANDROID__ || __IOS__ || __MAC__
            return LaunchFileAsyncImpl(file);
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Windows.System.Launcher.LaunchFileAsync((Windows.Storage.StorageFile)((StorageFile)file)).AsTask();
#elif WIN32
            return Task.FromResult<bool>(Launch(file.Path, null));
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Launches File Explorer and displays the contents of the specified folder.
        /// </summary>
        /// <param name="folder">The folder to display in File Explorer.</param>
        /// <returns>The result of the operation.</returns>
        /// <remarks>    
        /// <list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list></remarks>
        public static Task<bool> LaunchFolderAsync(IStorageFolder folder)
        {
#if __MAC__
            return LaunchFolderAsyncImpl(folder);
#elif WINDOWS_UWP
            return Windows.System.Launcher.LaunchFolderAsync((Windows.Storage.StorageFolder)((StorageFolder)folder)).AsTask();
#elif WIN32
            return Task.FromResult<bool>(Launch("explorer.exe", folder.Path));
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Starts the default app associated with the URI scheme name for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="options">Ignored on Android and iOS.</param>
        /// <returns>The launch operation.</returns>
        /// <remarks>    
        /// <list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
        /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
        /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list></remarks>
        internal static Task<bool> LaunchUriAsync(Uri uri, LauncherOptions options)
        {
#if __ANDROID__ || __UNIFIED__
            return LaunchUriAsyncImpl(uri, options);

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return Windows.System.Launcher.LaunchUriAsync(uri).AsTask();
#elif WIN32
            return Task.FromResult<bool>(Launch(uri.ToString(), null));
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Starts the default app associated with the URI scheme name for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The launch operation.</returns>
        /// <remarks>    
        /// <list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
        /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
        /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list></remarks>
        public static Task<bool> LaunchUriAsync(Uri uri)
        {
            return LaunchUriAsync(uri, new LauncherOptions());
        }

        /// <summary>
        /// Asynchronously query whether an app can be activated for the specified URI and launch type.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="launchQuerySupportType">The type of launch for which to query support.</param>
        /// <returns>Returns true if the default app for the URI scheme was launched; false otherwise.</returns>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
        /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>-</description></item>
        /// <item><term>Windows Phone Store</term><description>-</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>-</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public static Task<LaunchQuerySupportStatus> QueryUriSupportAsync(Uri uri, LaunchQuerySupportType launchQuerySupportType)
        {
#if __ANDROID__ || __IOS__ || __TVOS__
            return QueryUriSupportAsyncImpl(uri, launchQuerySupportType);
#elif WINDOWS_UWP
            return Task.Run<LaunchQuerySupportStatus>(async () =>
            {
                var s = await Windows.System.Launcher.QueryUriSupportAsync(uri, (Windows.System.LaunchQuerySupportType)((int)launchQuerySupportType)).AsTask();
                return (LaunchQuerySupportStatus)((int)s);
            });
#elif WIN32
            return Task.Run<LaunchQuerySupportStatus>(() =>
            {
                using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(uri.Scheme))
                {
                    if (rk != null)
                    {
                        if (rk.GetValue("URL Protocol") != null)
                        {
                            return LaunchQuerySupportStatus.Available;
                        }
                    }
                }

                return LaunchQuerySupportStatus.NotSupported;
            });
#else
            return Task.FromResult<LaunchQuerySupportStatus>(LaunchQuerySupportStatus.Unknown);
#endif
        }
    }
}